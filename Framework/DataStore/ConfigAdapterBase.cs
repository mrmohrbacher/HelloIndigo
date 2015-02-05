using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Configuration;

namespace Blackriverinc.Framework.DataStore
	{
	public class ConfigProviderBase : DataStoreProvider
		{
		protected Configuration _configuration = null;

		public ConfigProviderBase(IDataStoreProvider interiorProvider = null)
			: base(interiorProvider)
			{
			}

		public override void Load(IKeyedDataStore store)
			{
			try
				{
				base.Load(store);

				foreach (string key in _configuration.AppSettings.Settings.AllKeys)
					store[key] = _configuration.AppSettings.Settings[key].Value;
				}
			catch (Exception ex)
				{
				Trace.TraceError(ex.ToString());
				throw;
				}
			}

		public override void Save(IKeyedDataStore store)
			{
			KeyValueConfigurationCollection appSettings = _configuration.AppSettings.Settings;
			appSettings.Clear();
			foreach (string key in store.Keys)
				{
				appSettings.Add(key, store[key]);
				}
			}
		}
	}
