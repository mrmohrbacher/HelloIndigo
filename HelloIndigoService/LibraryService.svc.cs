using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


#if _AZURE
using Microsoft.WindowsAzure.ServiceRuntime;
#endif

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using Library.Model;
using Library.Model.Helpers;

namespace HelloIndigo
	{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true,
						  Name = "HelloIndigo.LibraryService",
						  InstanceContextMode = InstanceContextMode.PerCall)]
	public class LibraryService : ILibraryService
		{
		static AppTraceListener tracer = null;
		static int instanceCount = 0;

		static LibraryService()
			{
			try
				{
				IDataStoreProvider provider = ConfigProviderBase.Open();
				GlobalCache.LoadConfigurationSettings(provider, true);

				string logPath = (GlobalCache.GetResolvedString("LogPath") ?? @"C:\Logs\HelloIndigo");

#if _AZURE
				// Running in the Cloud; look for the local drive
				if (!Path.IsPathRooted(logPath)
				&& RoleEnvironment.IsAvailable
				&& !RoleEnvironment.IsEmulated)
					{
					LocalResource localResource = RoleEnvironment.GetLocalResource("LogFiles");
					logPath = Path.Combine(localResource.RootPath, logPath);
					}
#endif
				tracer = new AppTraceListener(logPath);

				// Resolve streams out of resource fork.
				StreamFactory.Register("res", (path, args) =>
					{
						Assembly assembly = Assembly.GetExecutingAssembly();
						string resourcePath = string.Format("{0}.{1}",
								assembly.GetName().Name,
								path);
						Trace.WriteLine(string.Format("StreamFactory::Create(res://{0})", resourcePath));
						Stream result = assembly.GetManifestResourceStream(resourcePath);
						return result;
					});
				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				}
			}

		public LibraryService()
			{
			try
				{
				if (Interlocked.Add(ref instanceCount, 1) == 1)
					{
					Trace.WriteLine(string.Format("+ LibraryService[{0}]+", instanceCount));

					// Populate the Books Table
					using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("DatabaseConfiguration")))
						{
						if (context.Books.Count() == 0)
							{
							Stream xstream = StreamFactory.Create(@"res://AppData.Books.xml");
							this.Load(xstream);
							}
						}
					}
				}
			catch (Exception exp)
				{
                Interlocked.Decrement(ref instanceCount);
				Trace.WriteLine(exp.ToString());
				}
			}

		~LibraryService()
			{
            if (Interlocked.CompareExchange(ref instanceCount, 0, 0) > 0)
                Interlocked.Decrement(ref instanceCount);

			Trace.WriteLine(string.Format("- LibraryService[{0}]-", instanceCount));
			if (Interlocked.CompareExchange(ref instanceCount, 0, 0) == 0)
				if (tracer != null)  tracer.Close();
			}

		#region ILibraryService Implementation

		public bool Load(Stream xstream)
			{
			// Populate the Books Table
			using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("DatabaseConfiguration")))
				{
				context.DeserializeBooks(xstream);
				context.SaveChanges();
				}
			return true;
			}

		public bool List(string searchPattern, out Book[] books)
			{
			Trace.WriteLine(string.Format("List searchPattern='{0}' ",
					searchPattern));
			try
				{
				if (searchPattern == null || searchPattern.Trim().Length == 0)
					searchPattern = @".*";
				Regex regex = new Regex(searchPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

				Func<Book, bool> searchPredicate = ((book) =>
				{
					string rowData = book.ToCSVString();
					return (regex.IsMatch(rowData));
				});

				var results = new List<Book>();

				using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("DatabaseConfiguration")))
					{
					var query = (from book in context.Books
									 select book)
									  .Where(searchPredicate).ToList();
					foreach (var entity in query)
						{
						Book book = new Book(entity as Book);

						Checkout checkout = context.Checkouts
															.Where(bc => bc.DateIn == null
																		 && bc.ISBN == book.ISBN)															
															.FirstOrDefault();
						if (checkout != null)
							{
							book.CheckedOut = checkout.DateOut;
							}
						results.Add(book);
						}
					books = results.ToArray();
					}

				}
			catch (Exception exp)
				{
				Trace.WriteLine(string.Format("LibraryService::List Failed\n{0}", exp));
				books = new Book[0];
				}
			return (books.Length > 0);
			}

		public bool Read(string isbn, out Book book)
			{
			Trace.WriteLine(string.Format("Read key='{0}'", isbn));
			if (isbn == null || isbn.Trim().Length == 0)
				{
				book = null;
				return false;
				}

			using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("DatabaseConfiguration")))
				{
				book = new Book((from b in context.Books
									  where b.ISBN == isbn
									  select b).FirstOrDefault());

				Checkout checkout = context.Checkouts
													.Where(bc => bc.DateIn == null
																 && bc.ISBN == isbn)
													.FirstOrDefault();
				book.CheckedOut = (checkout != null ? checkout.DateOut : (DateTime?)null);
				}
			return (book != null);
			}

		public bool Update(ref Book book)
			{
			throw new NotImplementedException();
			}

		public bool Add(Book book)
			{
			throw new NotImplementedException();
			}

		public bool Delete(string key, DateTime timeStamp)
			{
			throw new NotImplementedException();
			}

		public bool Checkout(Checkout checkout, bool updateSubscriber, out DateTime? checkedout)
			{
			bool result = false;
			string isbn = checkout.ISBN;
			checkedout = null;

			try
				{
				using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("DatabaseConfiguration")))
					{
					//-------------------------------------------------------------
					// Verify Book exists.
					//-------------------------------------------------------------
					var book = new Book((from b in context.Books
												where b.ISBN == isbn
												select b).FirstOrDefault());
					if (book == null)
						return false;

					//-------------------------------------------------------------
					// Update Subscriber table with MERGE operation.
					//-------------------------------------------------------------
					var subscriberService = new HelloIndigo.SubscriberService();
					Subscriber subscriber = null;
					if (!subscriberService.Read(checkout.Email, out subscriber))
						{
						subscriberService.Add(new Subscriber()
										{
											Email = checkout.Email,
											Name = checkout.Name,
											Address = checkout.Address,
											City = checkout.City,
											State = checkout.State,
											PostalCode = checkout.PostalCode
										});
						}
					else
						{
						if (updateSubscriber)
							subscriberService.Update(ref subscriber);
						}

					//-------------------------------------------------------------
					// Verify Book not already Checkedout.
					//-------------------------------------------------------------
					checkedout = (from co in context.Checkouts
										where co.ISBN == isbn
											&& co.DateIn == null
										select (co.DateOut == DateTime.MinValue)
														? null : (DateTime?)co.DateOut)
										.FirstOrDefault();

					if (checkedout.HasValue)
						return false;

					//-------------------------------------------------------------
					// Checkout Book to Subscriber.
					// *NOTE*	Times in UTC to make checkout operations
					//				TZ independent.
					//-------------------------------------------------------------
					checkedout = DateTime.UtcNow;
					checkout.DateOut = checkedout.Value;
					checkout.Title = book.Title;
					context.Checkouts.Add(checkout);

					result = (context.SaveChanges() > 0);
					}
				}
			catch (Exception exp)
				{
				StringBuilder msg = new StringBuilder();
				msg.AppendFormat("*Exception* : {0} ", exp.Message);
				msg.AppendLine();
				msg.AppendFormat("  Operation : Checkout {0}", checkout.ToObjectString("ISBN Email Name"));
				Trace.WriteLine(msg.ToString());
				}
			return result;
			}


		public bool Checkin(string isbn, DateTime checkedout, out DateTime? checkedin)
			{
			bool result = false;
			checkedin = null;

			using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("DatabaseConfiguration")))
				{
				var book = context.Books
										.Where(b => (b.ISBN == isbn))
										.FirstOrDefault();
				if (book == null)
					return false;

				Checkout checkout = context.Checkouts
															.Where(bc => bc.DateOut == checkedout
																       && bc.DateIn == null
																		 && bc.ISBN == isbn)
															.FirstOrDefault();
				if (checkout == null)
					return false;

				checkout.DateIn = checkedin = DateTime.UtcNow;
				result = (context.SaveChanges() > 0);
				}

			return result;
			}

		#endregion

		}
	}
