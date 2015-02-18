using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.ScheduledJob;
using Blackriverinc.Framework.Utility;

using LibraryModel;
using Client.LibraryService;
using Client.EchoService;

namespace Client
   {
   public class LibraryClient<TSettingsProvider> :
		IRunnable
		where TSettingsProvider : IDataStoreProvider, new()
      {

      Book[] books = null;

		IKeyedDataStore _settings = null;

		string _echoServiceEndpointName = "IEchoService_LocalHttp";
		string _libraryServiceEndpointName = "ILibraryService_LocalHttp";

		public LibraryClient()
			{
			IDataStoreProvider provider = new TSettingsProvider();
			_settings = new KeyedDataStore(provider);

			if (!_settings.get("EchoServiceEndpoint", ref _echoServiceEndpointName))
				{
				throw new ApplicationException(string.Format("Missing Configuration element ''",
								"EchoServiceEndpoint"));
				}
			Trace.WriteLine(string.Format("EchoServiceEndpoint='{0}'", _echoServiceEndpointName));

			if (!_settings.get("LibraryServiceEndpoint", ref _libraryServiceEndpointName))
				{
				throw new ApplicationException(string.Format("Missing Configuration element ''",
								"LibraryServiceEndpoint"));
				}
			Trace.WriteLine(string.Format("LibraryServiceEndpoint='{0}'", _libraryServiceEndpointName));

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

      public void Run()
         {
			try
				{
				using (EchoServiceClient proxy = new EchoServiceClient(_echoServiceEndpointName))
					{
					string result = "";

					result = proxy.Ping();
					Console.WriteLine(string.Format("EchoService : {0}", result));
					}
				}
			catch (System.ServiceModel.EndpointNotFoundException)
				{
				Trace.WriteLine(string.Format("!Note! : No endpoint listening at [{0}]", _echoServiceEndpointName));
				}

			using (LibraryServiceClient proxy = new LibraryServiceClient(_libraryServiceEndpointName))
            {
            Book book = null;

				Action prompt = new Action(() =>
					{
					Console.WriteLine("LIST - Return a list of Books using the Search Pattern.");
					Console.WriteLine("READ - Return a Book using the Key value.");				
					Console.WriteLine("Press ENTER to terminate.");
					});

				prompt();

            string input = "";
            StringBuilder sb = new StringBuilder();
            while ((input = Console.ReadLine().ToUpper()).Length > 0)
               {
					try
						{
						string[] tokens = input.Split(' ');
						switch ((tokens.Length) > 0 ? tokens[0] : "")
							{
							case "LIST":
								Console.Write("Search Pattern : ");
								string searchPattern = Console.ReadLine();
								proxy.List(out books, searchPattern);

								sb.Length = 0;
								XmlSerializeHelper.SerializeToString(ref sb, books.ToList());
								Console.Write(sb.ToString());

								break;

							case "LOAD":
								Stream xstream = StreamFactory.Create(@"res://AppData.Books.xml");
								Console.WriteLine(string.Format("Load = {0}", proxy.Load(xstream)));
								break;

							case "READ":
								Console.Write("Key : ");
								string key = Console.ReadLine();
								if (proxy.Read(out book, key))
									{
									sb.Length = 0;
									XmlSerializeHelper.SerializeToString(ref sb, book);
									Console.Write(sb.ToString().Trim());
									}
								else
									{
									Console.WriteLine(" *Not Found*");
									}
								break;
							default:
								prompt();
								break;
							}
						}
					catch (Exception exp)
						{
						Trace.WriteLine(string.Format("Error {0} : {1}", input, exp.Message));
						}
               }            
            }
         }

      public void Stop()
         {
        
         }
      }
   }
