using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using LibraryModel;

namespace HelloIndigo
   {
   [ServiceBehavior(IncludeExceptionDetailInFaults = true, 
						  Name = "HelloIndigo.EchoService",
						  InstanceContextMode = InstanceContextMode.PerSession)]
   public class EchoService : IEchoService, IDisposable
      {
      static AppTraceListener tracer = null;
      static int instanceCount = 0;
		bool disposed = false;

      int instanceID = -1;
		static IKeyedDataStore iconfig = null;

		static string _logPath = null;

      static EchoService()
         {
			System.Diagnostics.Trace.Listeners.Add(new ConsoleTraceListener());
			initializeTracer();				
			}

		private static void initializeTracer()
			{
			IDataStoreProvider provider = ConfigProvider.Open();
			iconfig = new KeyedDataStore(new CloudSettingsProvider(provider));

			_logPath = iconfig["LogPath"] ?? @"C:\Logs\HelloIndigo";

			// Running in the Cloud; look for the local drive
			if (!Path.IsPathRooted(_logPath)
			&& RoleEnvironment.IsAvailable
			&& !RoleEnvironment.IsEmulated)
				{
				LocalResource localResource = RoleEnvironment.GetLocalResource("LogFiles");
				_logPath = Path.Combine(localResource.RootPath, _logPath);
				}

			tracer = new AppTraceListener(_logPath);
			}

      public EchoService()
         {
         instanceID = Interlocked.Add(ref instanceCount, 1);
         Trace.WriteLine(string.Format("+ EchoService [{0}] +", instanceID));       
         }

      ~EchoService()
         {
			Dispose(false);
         }

      #region IEchoService Implementation

      public void Ping(out string result)
         {
         string env = "App";
         string location = "Local";
         try
            {
            if (RoleEnvironment.IsAvailable)
               {
               env = "Azure";
               location = (RoleEnvironment.IsEmulated) ? "Local" : "Cloud";
               }
            }
         catch (RoleEnvironmentException ree)
            {
            Trace.WriteLine(ree.ToString());
            }
         catch (Exception exp)
            {
            Trace.WriteLine(exp.ToString());
            throw;
            }
         result = "{" + string.Format("Environment:'{0}',Location:'{1}',Version:'{2}', LogPath={3}",
            location, env, this.GetType().Assembly.GetName().Version, _logPath) + "}";
         Trace.WriteLine(result);
         }

      public bool Echo(out string result, string input)
         {
         result = input;
         Trace.WriteLine(string.Format("{0} => {1}", input, result));
         return true;
         }

      #endregion

		protected void Dispose(bool manualDispose)
			{
			if (!disposed)
				{
				int cnt = Interlocked.Add(ref instanceCount, -1);
				Trace.WriteLine(string.Format("- EchoService [{0}] -", instanceID));
				if (cnt == 0)
					if (tracer != null) tracer.Close();
				}
			disposed = true;
			}

		public void Dispose()
			{
			Dispose(true);
			GC.SuppressFinalize(this);
			}
		}
   }
