using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Blackriverinc.Framework.DataStore;

namespace LibraryCheckout.WebAppMVC
	{
	public class GlobalCacheConfig
		{
		public static void Load()
			{
			IDataStoreProvider provider = ConfigProviderBase.Open();
			provider = new CloudSettingsProvider(provider);

			GlobalCache.LoadConfigurationSettings(provider, false);
			}
		}
	}