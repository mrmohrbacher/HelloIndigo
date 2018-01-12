using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading;
#if _AZURE
using Microsoft.WindowsAzure.ServiceRuntime;
#endif

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

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
            initializeTracer();
        }

        private static void initializeTracer()
        {
            IDataStoreProvider provider = ConfigProviderBase.Open();
            iconfig = new KeyedDataStore(new CloudSettingsProvider(provider));

            _logPath = (iconfig["LogPath"] as string) ?? @"C:\Logs\HelloIndigo";
#if _AZURE
			// Running in the Cloud; look for the local drive
			if (!Path.IsPathRooted(_logPath)
			&& RoleEnvironment.IsAvailable
			&& !RoleEnvironment.IsEmulated)
				{
				LocalResource localResource = RoleEnvironment.GetLocalResource("LogFiles");
				_logPath = Path.Combine(localResource.RootPath, _logPath);
				}
#endif
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
#if _AZURE
            if (RoleEnvironment.IsAvailable)
               {
               env = "Azure";
               location = (RoleEnvironment.IsEmulated) ? "Local" : "Cloud";
               }
#endif
            }
#if _AZURE
         catch (RoleEnvironmentException ree)
            {
            Trace.WriteLine(ree.ToString());
            }
#endif
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
