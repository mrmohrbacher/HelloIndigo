using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Configuration;

namespace Blackriverinc.Framework.DataStore
   {
   public class WebConfigAdapter : ConfigProviderBase
      {

      public WebConfigAdapter() :
         this(WebConfigurationManager.OpenWebConfiguration(null))
         {
         }

      protected WebConfigAdapter(Configuration configuration) : base(null)
         {
         _configuration = configuration;
         }

      static public IDataStoreProvider Open(string webPath = "/")
         {
         IDataStoreProvider adapter = null;
         try
            {
            Configuration configuration =
                  WebConfigurationManager.OpenWebConfiguration(webPath);
            if (!configuration.HasFile)
               // No Configuration File to Process.
               return null;

            Debug.WriteLine(string.Format("      Configuration : {0}", configuration.FilePath));

            adapter = new WebConfigAdapter(configuration);
            }
         catch (Exception exp)
            {
            Trace.WriteLine(exp.ToString());
            }
         return adapter;
         }
      }
   }
