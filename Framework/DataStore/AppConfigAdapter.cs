using System;
using System.Configuration;
using System.Diagnostics;


namespace Blackriverinc.Framework.DataStore
   {
	public class AppConfigAdapter : ConfigProviderBase
      {
      public AppConfigAdapter() :
         this(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None))
         {       
         }

      protected AppConfigAdapter(Configuration configuration) : base(null)
         {
         _configuration = configuration;
         }

      static public IDataStoreProvider Open(string filePath)
         {
         IDataStoreProvider adapter = null;
         try
            {
            Configuration configuration =
                  ConfigurationManager.OpenExeConfiguration(filePath);
            if (!configuration.HasFile)
               // No Configuration File to Process.
               return null;

            Debug.WriteLine(string.Format("      Configuration : {0}", configuration.FilePath));

            adapter = new AppConfigAdapter(configuration);
            }
         catch (Exception exp)
            {
            Trace.WriteLine(exp.ToString());
            }
         return adapter;
         }
      }
   }
