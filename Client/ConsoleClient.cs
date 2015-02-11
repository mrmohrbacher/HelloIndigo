using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using LibraryModel;

namespace HelloIndigo.ServiceClient
	{
	class ConsoleClient
		{

#if ServiceHost
      static void MainEx(string[] args)
         {
         try
            {
            EndpointAddress ep = new EndpointAddress("http://localhost:8088/HelloIndigo/EchoService");

            IEchoService proxy
               = ChannelFactory<IEchoService>.CreateChannel(new BasicHttpBinding(),
                                                                   ep);

            string s = null;
            proxy.Echo(out s, "Echo, me!");

            Console.WriteLine(string.Format("HelloIndigo.HelloIndigoService::Echo : {0}", s));
            }
         catch (Exception exp)
            {
            Trace.WriteLine(exp.ToString());
            }
         }
#endif
		static AppTraceListener tracer;


		static void Main(string[] args)
			{
			try
				{
				Trace.Listeners.Add(new ConsoleTraceListener());

				tracer = new AppTraceListener(@"C:\Logs\HelloIndigoClient");

				if (args.Length > 0 && args[0].ToUpper() == "ECHO")
					{
					Client.EchoClient client = new Client.EchoClient();
					client.Run();
					}
				else
					{
					Client.LibraryClient<AppConfigProvider> client
						= new Client.LibraryClient<AppConfigProvider>();
					client.Run();
					}
				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				}
			finally
				{
				tracer.Close();
				}
			}

		}
	}
