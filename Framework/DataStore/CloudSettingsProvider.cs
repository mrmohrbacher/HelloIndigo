using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.ServiceRuntime;

namespace Blackriverinc.Framework.DataStore
	{
	public class CloudSettingsProvider : DataStoreProvider
      {
		public CloudSettingsProvider() : base(null)
         {
         }

		public CloudSettingsProvider(IDataStoreProvider interiorProvider)
			: base(interiorProvider)
			{
			}

      public override void Load(IKeyedDataStore store)
         {
			base.Load(store);
			if (RoleEnvironment.IsAvailable)
				{
				string logPath = RoleEnvironment.GetConfigurationSettingValue("LogPath");
				store.Add("LogPath", logPath);
				}
         }

      public override void Save(IKeyedDataStore store)
         {
         }
		}
	}
