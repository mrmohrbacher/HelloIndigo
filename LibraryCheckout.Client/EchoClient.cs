using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.ScheduledJob;
using Blackriverinc.Framework.Utility;

namespace Client
   {
   public class EchoClient : IRunnable
      {
		string endPointName = null;

		public EchoClient()
			{
			try
				{
				var config = new KeyedDataStore(new AppConfigProvider());
				endPointName = config["EchoServiceEndpoint"] as string;
				Trace.WriteLine(string.Format("+ EchoClient : EndPointName '{0}'", endPointName??"*null*"));
				}
			catch (Exception exp)
				{
				Trace.WriteLine(string.Format("*EchoClient Exception* {0}", exp.Message));
				}
			}

		 ~EchoClient()
			{
			Trace.WriteLine("- EchoClient");
			}

      public void Run()
         {
			using (var proxy = new LibraryCheckout.Client.EchoService.EchoServiceClient(endPointName))
            {
            string result = "";

            result = proxy.Ping();
            Console.WriteLine(result);

            Console.WriteLine("Press ENTER to terminate.");
            string input = "";
            while ((input = Console.ReadLine()).Length > 0)
               {
               proxy.Echo(input, out result);
               Console.WriteLine(string.Format("{0} => {1}", input, result));
               }
            }
         }

      public void Stop()
         {
         
         }
      }
   }
