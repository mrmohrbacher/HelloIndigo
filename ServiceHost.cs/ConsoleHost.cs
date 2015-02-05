using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Blackriverinc.Framework.Utility;

using HelloIndigo;

namespace HelloIndigo.ServiceHost
   {
   class ConsoleHost
      {
      static void Main(string[] args)
         {
         AppTraceListener tracer = null;
         try
            {
            tracer = new AppTraceListener(@"C:\Logs\HelloIndigoConsoleHost");

            using (System.ServiceModel.ServiceHost host
               = new System.ServiceModel.ServiceHost(typeof(EchoService)))
               {
               host.Open();

               int i = 0;

               Trace.WriteLine(string.Format("Service : {0}", host.Description.ConfigurationName));
               Trace.WriteLine(string.Format("+ EndPoints +"));
               foreach (var ep in host.Description.Endpoints)
                  {
                  Trace.WriteLine(string.Format("  [{0}] = {1}",
                        i++, ep.Address.Uri));
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
