using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;


namespace Blackriverinc.Framework.ConfigurationSettingsProvider
	{

	/////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Retrieves connection string settings from app.config. UID and PWD are
	/// stored encrypted in separate keys.
	/// </summary>
	/////////////////////////////////////////////////////////////////////////////////
	public class AppConfigConnectionSettingsProvider : IConnectionSettingsProvider
		{
		private static ConnectionStringSettingsCollection cnxnSettings;

		static AppConfigConnectionSettingsProvider()
			{
			try
				{
				//------------------------------------------------------------------
				// Create the cnxnStrings cache on the first call into this method.
				//------------------------------------------------------------------
				cnxnSettings = new ConnectionStringSettingsCollection();
				ConnectionStringSettingsCollection dbConnectionStrings =
						  ConfigurationManager.ConnectionStrings;
				if (dbConnectionStrings != null)
					{
					foreach (ConnectionStringSettings setting in dbConnectionStrings)
						cnxnSettings.Add(setting);
					}
				}
			catch (Exception ex)
				{
				Trace.TraceError(ex.ToString());
				throw;
				}
			}

		/// <summary>
		/// Retrieve a ConnectionString from the CnxnSetttings Cache using the
		/// 'dbConnectionKey' as an index into the collection.
		/// </summary>
		public string this[string dbConnectionKey]
			{
			get
				{
				string cnxnString = null;

				try
					{
					ConnectionStringSettings cnxnSetting = null;

					if (dbConnectionKey == null)
						{
						// Pull default connectionKey from AppSettings
						dbConnectionKey = ConfigurationManager.AppSettings["DbConnectionKey"];
						if (dbConnectionKey == null)
							dbConnectionKey = "PRODUCTION DS";
						}

					lock (cnxnSettings)
						{
						if (dbConnectionKey != null)
							{
							//-------------------------------------------------
							// Pull Database connectivity and credentials from
							// the cache.
							//-------------------------------------------------
							cnxnSetting = cnxnSettings[dbConnectionKey];
							}

						}

					if (cnxnSetting == null)
						throw new ApplicationException(string.Format("Could not load ConnectionString [{0}]", dbConnectionKey));

					//---------------------------------------------
					// Add the encrypted UID and PWD to CnxnString
					//---------------------------------------------
					cnxnString = cnxnSetting.BindTokens(dbConnectionKey);

					ConnectionStringSettings settings = new ConnectionStringSettings(dbConnectionKey, cnxnString);
					}
				catch (Exception ex)
					{
					Trace.TraceError(ex.ToString());
					throw;
					}

				return cnxnString;
				}

			set
				{
				throw new NotImplementedException("Cannot set ConnectionString");
				}
			}
		};

	}
