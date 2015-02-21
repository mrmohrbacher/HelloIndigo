using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.WindowsAzure.ServiceRuntime;

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
				// Running in the Cloud; look for the local drive
				if (!Path.IsPathRooted(logPath)
				&& RoleEnvironment.IsAvailable
				&& !RoleEnvironment.IsEmulated)
					{
					LocalResource localResource = RoleEnvironment.GetLocalResource("LogFiles");
					logPath = Path.Combine(localResource.RootPath, logPath);
					}

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
					using(LibraryEntities context = new LibraryEntities(""))
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
				Trace.WriteLine(exp.ToString());
				}
			}

		~LibraryService()
			{
			Interlocked.Add(ref instanceCount, -1);
			Trace.WriteLine(string.Format("- LibraryService[{0}]-", instanceCount));
			if (Interlocked.CompareExchange(ref instanceCount, 0, 0) == 0)
				tracer.Close();
			}

		#region ILibraryService Implementation

		public bool Load(Stream xstream)
			{
			// Populate the Books Table
			using (LibraryEntities context = new LibraryEntities(""))
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

				Func<Book, bool> searchPredicate = ((book) => {
					string rowData = book.ToCSVString();
					return (regex.IsMatch(rowData));
					});

				var list = new List<Book>();

				using (LibraryEntities context = new LibraryEntities(""))
					{
					var result = (from book in context.Books
									  select book)
									  .Where(searchPredicate).ToList();
					foreach (var book in result)
						{
						list.Add(new Book(book as Book));
						}
					}

				books = list.ToArray();

				}
			catch (Exception exp)
				{
				Trace.WriteLine(string.Format("LibraryService::List Failed\n{0}", exp));
				books = new Book[0];
				}
			return (books.Length > 0);
			}

		public bool Read(string key, out Book book)
			{
			Trace.WriteLine(string.Format("Read key='{0}'", key));
			if (key == null || key.Trim().Length == 0)
				{
				book = null;
				return false;
				}

			using (LibraryEntities context = new LibraryEntities(""))
				{
				book = new Book((from b in context.Books
						  where b.ISBN == key
						  select b).FirstOrDefault());
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

		#endregion
		}
	}
