using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using LibraryModel;

namespace HelloIndigo
	{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true, Name = "HelloIndigo.LibraryService")]
	public class LibraryService : ILibraryService
		{
		static AppTraceListener tracer = null;
		static int instanceCount = 0;

		static KeyedDataStore iconfig;

		static LibraryService()
         {
			try
				{
				IDataStoreProvider provider = WebConfigProvider.Open();
				if (provider != null)
					{
					iconfig = new KeyedDataStore(new CloudSettingsProvider(provider));
					}

				string logPath = iconfig["LogPath"] ?? @"C:\Logs\HelloIndigo";
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
					LibraryEntities<AppConfigProvider> context = new LibraryEntities<AppConfigProvider>(iconfig["LibraryEntities.ConnectionString"]);

					if (!context.BooksTableExists)
						{
						bool created = context.CreateBooks(true);
						if (created)
							{
							Stream xstream = StreamFactory.Create(@"res://AppData.Books.xml");
							context.DeserializeBooks(xstream);
							context.SaveChanges();
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

		public bool List(string searchPattern, out Book[] books)
			{
			Trace.WriteLine(string.Format("List searchPattern='{0}'", searchPattern));
			try
				{
				if (searchPattern == null || searchPattern.Trim().Length == 0)
					searchPattern = @".*";
				Regex regex = new Regex(searchPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

				Func<Book, bool> searchPredicate = ((book) => (regex.IsMatch(book.RowKey)));

				LibraryEntities<AppConfigProvider> context = new LibraryEntities<AppConfigProvider>(iconfig["LibraryEntities.ConnectionString"]);

				var result = (from book in context.Books
								  select book);
				books = result.Where(searchPredicate).ToArray();
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

			LibraryEntities<AppConfigProvider> context = new LibraryEntities<AppConfigProvider>(iconfig["LibraryEntities.ConnectionString"]);

			book = (from b in context.Books
					  where b.RowKey == key
					  select b).FirstOrDefault();

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
