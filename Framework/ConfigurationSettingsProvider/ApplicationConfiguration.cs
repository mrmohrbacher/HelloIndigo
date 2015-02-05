using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Blackriverinc.Framework.ConfigurationSettingsProvider
	{
	public class ApplicationConfiguration :
		ConfigSettings,
		IConfigSettings
		{
		Configuration _configuration = null;

		static ApplicationConfiguration()
			{
			try
				{
				//------------------------------------------------------------------
				// Create the settings cache on the first call into this method.
				//------------------------------------------------------------------
				_sections = new Dictionary<string, IDictionary<string, object>>(StringComparer.OrdinalIgnoreCase);
				}
			catch (Exception ex)
				{
				Trace.TraceError(ex.ToString());
				throw;
				}
			}

		public ApplicationConfiguration() :
			this(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None))
			{
			}

		public ApplicationConfiguration(Configuration configuration)
			{
			try
				{
				lock (_sections)
					{
					_configuration = configuration;

					// Clear out the previous cache
					_sections.Clear();

					_settings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

					foreach (string key in _configuration.AppSettings.Settings.AllKeys)
						_settings[key] = _configuration.AppSettings.Settings[key].Value;

					_sections.Add("appSettings", _settings);
					}
				}
			catch (Exception ex)
				{
				Trace.TraceError(ex.ToString());
				throw;
				}
			}

		public ApplicationConfiguration(string sectionName)
			: this()
			{
			Section = sectionName;
			}

		#region "IConfigSettings"

		/// <summary>
		/// Retrieve a string from the cache based on the key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public override string this[string key]
			{
			get
				{
				string result = null;
				try
					{
					lock (_settings)
						{
						if (_settings.ContainsKey(key))
							{
							object value = _settings[key];
							result = value.ToString();
							}
						}
					}
				catch (Exception ex)
					{
					Trace.TraceError(ex.ToString());
					throw;
					}

				return result;
				}
			set
				{
				try
					{
					lock (_settings)
						{
						if (!_settings.ContainsKey(key))
							_settings.Add(key, value);
						else
							_settings[key] = value;
						}
					}
				catch (Exception ex)
					{
					Trace.TraceError(ex.ToString());
					throw;
					}

				}
			}

		public string Section
			{
			get
				{
				return _section;
				}
			set
				{
				System.Xml.XmlNode _node = null;
				try
					{
					lock (_sections)
						{
						if (_sections.ContainsKey(value))
							{
							_settings = _sections[value];
							return;
							}

						DefaultSection section =
							(DefaultSection)_configuration.GetSection(value);

						if (section == null)
							throw new ApplicationException(
								String.Format("Could not find section '{0}' in the configuration file.",
													value));

						_settings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

						System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
						xmlDoc.LoadXml(section.SectionInformation.GetRawXml());

						System.Xml.XmlNode xList = xmlDoc.ChildNodes[0];
						foreach (System.Xml.XmlNode node in xList)
							{
							_node = node;
							if (node.NodeType == System.Xml.XmlNodeType.Element
							&& node.Attributes.Count > 1)
								{
								if (!_settings.ContainsKey(node.Attributes[0].Value))
									_settings.Add(node.Attributes[0].Value, node.Attributes[1].Value);
								else
									Trace.WriteLine(node.ToString());
								}
							}

						_sections.Add(value, _settings);
						_section = value;
						}
					}
				catch (Exception ex)
					{
					Trace.TraceError(ex.ToString());
					throw;
					}
				}
			}

		#endregion

		public void Add(KeyValuePair<string, object> item)
			{
			// KeyValuePair<string, object> item = (KeyValuePair<string, object>)obj;
			if (!_settings.ContainsKey(item.Key))
				_settings.Add(item.Key, item.Value);
			}

		#region IEnumerable<KeyValuePair<string,object>> Members

		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
			{
			return (IEnumerator<KeyValuePair<string, object>>)_settings.GetEnumerator();
			}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
			return _settings.GetEnumerator();
			}

		#endregion

		private void Save(KeyValuePair<string, IDictionary<string, object>> section)
			{
			IDictionary<string, object> settings = section.Value;
			if (section.Key == "appSettings")
				{
				KeyValueConfigurationCollection appSettings = _configuration.AppSettings.Settings;
				appSettings.Clear();
				foreach (string key in settings.Keys)
					{
					appSettings.Add(key, settings[key].ToString());
					}
				}
			else
				{
				// TODO: At the moment, we are only going to reserialize the 'appSettings' section
				}
			}

		public override bool Save()
			{
			if (!_configuration.HasFile)
				return false;

			foreach (var section in _sections)
				Save(section);

			_configuration.AppSettings.Settings.Remove("ModifiedDate");
			_configuration.AppSettings.Settings.Add("ModifiedDate", DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));

			_configuration.Save(ConfigurationSaveMode.Modified);

			return true;
			}
		}
	}