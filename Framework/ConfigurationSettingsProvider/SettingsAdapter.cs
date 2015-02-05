using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OnlineReportingServices.Framework.ConfigurationSettingsProvider
	{
	public static class ConfigurationAdapter
		{
		static IDictionary<string, string> _system = null;

		static Regex _systemParser = null;

		static ConfigurationAdapter()
			{
			_systemParser = new Regex(@"(?<pretext>[^\{]*)(?:{system:(?<key>\w+)})(?<posttext>[^\{]*)",
				RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			}

		static IDictionary<string, string> GetSystem(ConfigSettings settings, StringDictionary parameters)
			{
			string remotePWD = null;
			string domain = null;
			string remoteUID = UpdateSettings(settings, "remoteloginid", parameters["remoteuid"], true);
			if (remoteUID != null && remoteUID.Length > 0)
				{
            //--------------------------------------------------
				// If we have specified remote crendentials, 
            // squirt them into the Settings
            //--------------------------------------------------
            remotePWD = UpdateSettings(settings, "remotepassword", parameters["remotepwd"], true);
				domain = UpdateSettings(settings, "domain", parameters["domain"], true);
				UpdateSettings(settings, "UserRemoteCredential", "YES", true);
				}

			if (_system == null)
				{
				WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message, false);
				EndpointAddress endpointAdddress = new EndpointAddress(
							UpdateSettings(settings, "systemstoreendpointaddress", parameters["systemstoreendpointaddress"], true));
            ConfigurationSettingsProvider.DashBoardStore.DashBoardClient client = new Common.ConfigurationSettingsProvider.DashBoardStore.DashBoardClient(binding, endpointAdddress);

				string environment = UpdateSettings(settings, "environment", parameters["environment"], true);
				string database = UpdateSettings(settings, "database", parameters["database"]);
				string datasource = UpdateSettings(settings, "datasource", parameters["datasource"]);

				string uid = UpdateSettings(settings, "uid", "{default}");
				string pwd = UpdateSettings(settings, "password", "{default}");

				StringBuilder sb = new StringBuilder();
				sb.Append("    GetHRPlusSystemObject ");
				sb.AppendLine();
				sb.AppendFormat("      Environment : '{0}'", environment);
				sb.AppendLine();
				sb.AppendFormat("      Database : '{0}'", database);
				sb.AppendLine();
				sb.AppendFormat("      Datasource : '{0}'", datasource);
				sb.AppendLine();
				sb.AppendFormat("      UID : '{0}'", uid);
				sb.AppendLine();
				sb.AppendFormat("      PASSWORD : '{0}'", pwd);
				sb.AppendLine();
				Debug.WriteLine(sb.ToString());

				// If we have specified remote drendentails, squirt them into the Settings
				if (remoteUID != null && remoteUID.Length > 0)
					{
					System.Net.NetworkCredential networkCredential =
						new System.Net.NetworkCredential(remoteUID, remotePWD, domain);
					client.ClientCredentials.Windows.ClientCredential = networkCredential;
					}
				_system = client.GetHRPlusSystemObject(environment, database, datasource, uid, pwd);
				if (_system == null)
					throw new NullReferenceException("GetHRPlusSystemObject returned a *null* reference.");
				}

			return _system;
			}

		/// <summary>
		/// Load the configuration file associated with an executable assembly.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static ConfigSettings LoadConfiguration(string filePath)
			{
			Trace.WriteLine(string.Format("      LoadConfiguration : Assembly = '{0}'", filePath));

			ConfigSettings iconfig = null;
			try
				{
				Configuration configuration =
						ConfigurationManager.OpenExeConfiguration(filePath);
				if (!configuration.HasFile)
					// No Configuration File to Process.
					return null;

				Debug.WriteLine(string.Format("      Configuration : {0}", configuration.FilePath));
				iconfig = new ApplicationConfiguration(configuration);
				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				iconfig = null;
				}
			return iconfig;
			}

		/// <summary>
		/// Map the command-line parameters onto the appSettings of the
		/// configuration file.
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static ConfigSettings UpdateSettings(ConfigSettings settings, StringDictionary parameters)
			{
			// Use the parameters to load the correct System environment.
			GetSystem(settings, parameters);

			StringBuilder sb = new StringBuilder();
			foreach (string key in parameters.Keys)
				{
				sb.Length = 0;
				string token = ConfigurationAdapter.UpdateSettings(settings, key, parameters[key]);
				if (token != null)
					{
					// Substitute the returned token for the original value in the settings
					settings[key] = token;
					sb.AppendFormat("  Setting[{0}] = '{1}'", key, settings[key]);
					Debug.WriteLine(sb.ToString());
					}
				}

			return settings;
			}

		/// <summary>
		/// Substitute a parameter'd value from the Context for the
		/// current value in the settings.
		/// </summary>
		/// <param name="settings">Settings section of target Configuration</param>
		/// <param name="key">A key from the Context.Parameters</param>
		/// <param name="overrideValue">Value from the Context.Parameters at 'key'</param>
		/// <returns></returns>
		public static string UpdateSettings(ConfigSettings settings, string key, string overrideValue, bool insert = false)
			{
			//key = key.ToUpper();
			Debug.WriteLine(string.Format("  Parameter : {0}, Value : '{1}'", key, overrideValue));

			string value = null;
			bool present = settings.get(key, ref value);
			// Is the key even something I care about?
			if (present || insert)
				{
				// Resolve references to the SystemSetingsStore
				MatchCollection matches = ConfigurationAdapter._systemParser.Matches(overrideValue);
				StringBuilder tokenBldr = null;
				foreach (Match match in matches)
					{
					if (tokenBldr == null)
						tokenBldr = new StringBuilder();
					string systemKey = match.Groups["key"].Value.ToUpperInvariant();
					if (_system != null && _system.ContainsKey(systemKey))
						{
						tokenBldr.AppendFormat("{0}{1}{2}",
							match.Groups["pretext"], _system[systemKey], match.Groups["posttext"]);
						}
					}
				// Values were resolved from the SystemSettingsStore
				if (tokenBldr != null && tokenBldr.Length > 0)
					{
					value = tokenBldr.ToString();
					}
				else if (overrideValue == "{default}")
					{
					if (!present)
						throw new ConfigurationErrorsException(string.Format("Setting [{0}] is not present in the 'appSettings' section and a parameter value of '{1}' was specified.", 
																				key, "{default}"));
					// Do nothing; we already have what we need.
					// value = settings[key];
					}
				else
					// Override the value with the matching value
					// from the Parameters collection.
					value = overrideValue;
				}

			// Update the Settings with the new, non-empty parameter value;
			if (!present && insert && value != null && value.Length > 0)
				settings[key] = value;

			return value;
			}
		}
	}
