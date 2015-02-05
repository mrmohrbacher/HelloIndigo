using System;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Blackriverinc.Framework.Utility;

namespace Blackriverinc.Framework.ConfigurationSettingsProvider
	{
	public interface IConnectionSettingsProvider
		{
		string this[string connectionKey] { get; }
		}

	public static class ConnectionSettings
		{
		public static string BindTokens(this ConnectionStringSettings cnxnSetting, string dbConnectionKey)
			{
			string cnxnString = null;

			//---------------------------------------------
			// Add the encrypted UID and PWD to CnxnString
			//---------------------------------------------
			Encryption decryptor = new Encryption();

			// MRM : Great place for a lambda expressions...no time today, maybe later...
			// MRM : ...It's later! Define a decoder functor.
			Func<string, string, bool, string> decoder = ((input, key, decrypt) =>
			{
				Regex keyPattern = new Regex(string.Format(@"(?:(?:{0})=\[(?<SettingKey>\w*)\])", key),
									 RegexOptions.IgnoreCase);
				var match = keyPattern.Match(cnxnSetting.ConnectionString);
				if (match.Success)
					{
					string lookup = match.Groups["SettingKey"].Value;
					string text = (new ApplicationConfiguration())[lookup];
					if (text == null)
						throw new ApplicationException(string.Format("Token '{0}' in ConnectionString '{1}' cannot be resolved to any key in the <appSettings>.",
															 lookup, dbConnectionKey));
					if (decrypt)
						text = decryptor.Decrypt(text);
					return input.Replace("[" + lookup + "]", text);
					}
				else
					return input;
			}
			);

			cnxnString = decoder(cnxnSetting.ConnectionString, "Data Source|Server", false);
			cnxnString = decoder(cnxnString, "Initial Catalog|Database", false);
			Debug.WriteLine(string.Format("Connection String[{0}]={1}", dbConnectionKey, cnxnString));
			cnxnString = decoder(cnxnString, "UID|Username", true);
			cnxnString = decoder(cnxnString, "PWD|Password", true);
			return cnxnString;
			}
		}
	}
