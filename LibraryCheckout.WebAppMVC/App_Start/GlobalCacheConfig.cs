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
#if _AZURE
			provider = new CloudSettingsProvider(provider);
#endif
			GlobalCache.LoadConfigurationSettings(provider, false);
			}
		}
	}