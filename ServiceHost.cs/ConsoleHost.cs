using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using HelloIndigo;

namespace HelloIndigo.ServiceHost
   {
   class ConsoleHost
      {
      static void Main(string[] args)
         {
         AppTraceListener tracer = null;
			Trace.Listeners.Add(new ConsoleTraceListener());
         try
            {
            tracer = new AppTraceListener(@"C:\Logs\HelloIndigoConsoleHost");

				var parameters = new KeyedDataStore(System.Environment.CommandLine.CommandLineParser(new Dictionary<string, object>(),
																							  StringFunction.DictionaryNotFoundOptions.AddKey));

				bool debugFlag = false;
				parameters.get("debug", ref debugFlag);
				if (debugFlag)
					Debugger.Break();

				string typeName = parameters["Service"] as string;
				var serviceType = TypeHandler.GetReferencedTypeByName(typeName, "HelloIndigo", "HelloIndigoService");
				if (serviceType == null)
					throw new Exception(string.Format("Type [{0}] not available", typeName));

            using (System.ServiceModel.ServiceHost host
					= new System.ServiceModel.ServiceHost(serviceType))
               {
               host.Open();
					
               int i = 0;

               Trace.WriteLine(string.Format("Service : {0}", host.Description.ConfigurationName));
               Trace.WriteLine(string.Format("+ EndPoints +"));
               foreach (var ep in host.Description.Endpoints)
                  {
                  Trace.WriteLine(string.Format("  [{0}] = {1} {2} {3}",
                        i++, ep.Address.Uri, ep.Binding.Name, ep.Contract.Name));
                  }
               Trace.WriteLine(string.Format("- EndPoints -"));

               /*
               i = 0;
               Trace.WriteLine(string.Format("+ BaseAddresses +"));
               foreach(Uri baseAddress in host.BaseAddresses)
                  {
                  Trace.WriteLine(string.Format("  [{0}] = {1}", 
                        i++, baseAddress.AbsoluteUri));               
                  }
               Trace.WriteLine(string.Format("- BaseAddresses -"));
               */

               Console.WriteLine(string.Format("Press ENTER to terminate '{0}'", host.Description.ConfigurationName));
               Console.ReadLine();
               }
            }
         catch (Exception exp)
            {
            Trace.WriteLine(exp.ToString());
            }
         finally
            {
            if (tracer != null)
               tracer.Close();
            }
         }
      }
   }
