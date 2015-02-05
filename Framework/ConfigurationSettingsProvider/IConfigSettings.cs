using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackriverinc.Framework.ConfigurationSettingsProvider
   {
   public interface IConfigSettings : IEnumerable<KeyValuePair<string, object>>
      {
      /// <summary>
      /// Retrieve a string from the cache based on the key.
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
		string this[string key] { get; set; }

      /// <summary>
      /// Return value from configuration source or default value
      /// </summary>
      /// <param name="key"></param>
      /// <param name="value"></param>
      /// <returns>true : value retrieved; false : value unchanged</returns>
      bool get(string key, ref bool value);
      bool get(string key, ref int value);
      bool get(string key, ref double value);
		bool get(string key, ref string value);
		bool get(string key, ref DateTime value);
      bool get(string key, ref Nullable<int> value);
      bool get(string key, ref Nullable<bool> value);
      bool get(string key, ref Nullable<double> value);
		bool get(string key, ref Nullable<DateTime> value);

      string Section { get; set; }
      }

   /// <summary>
   /// Base class to implement common functions
   /// </summary>
   public abstract class ConfigSettings
      {
		protected IDictionary<string, object> _settings;
		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Conglomerates Application Settings and custom Configuration Sections 
		/// into a single cache.
		/// </summary>
		/////////////////////////////////////////////////////////////////////////////////

		protected static Dictionary<string, IDictionary<string, object>> _sections;
		protected string _section;


		public abstract string this[string key]
		{ get; set; }


      /// <summary>
      /// Type-converting version of 'get'. Retrieve string-value
      /// from store. Attempt to convert to type of [value]. Return [false]
      /// if conversion fails or key not present in store. On fail, do not 
      /// modify the [value] parameter.
      /// </summary>
      public bool get(string key, ref bool value)
         {
         string strValue = this[key];
         if (strValue == null)
            return false;
         int intValue = 0;
         if (int.TryParse(strValue, out intValue))
            value = (intValue > 0);
         if (!bool.TryParse(strValue, out value))
            // We are here because we have a non-standard token in the value 
            value = (strValue.ToLower() == "yes" || strValue.ToLower() == "y" || strValue.ToLower() == "t");
         return true;
         }


      public bool get(string key, ref Nullable<bool> value)
         {
         // Fetch raw string
         string strValue = this[key];
         if (strValue == null)
            return false;

         bool result = false;
         // Attempt parsing as integer (0 : false; >0 : true)
         int intValue = 0;
         if (int.TryParse(strValue, out intValue))
            {
            result = (intValue > 0);
            }
         // Attempt parse as a standard {'True','False'} value.
         else if (!bool.TryParse(strValue, out result))
            // We are here because we have a non-standard token in the value 
            result = (strValue.ToLower() == "yes" || strValue.ToLower() == "y" || strValue.ToLower() == "t");

         // Regardless of results of parsing; we did find the key in the store.
         value = result;
         return true;
         }


      public bool get(string key, ref int value)
         {
         string strValue = this[key];
         if (strValue == null)
            return false;
         return int.TryParse(strValue, out value);
         }


      public bool get(string key, ref double value)
         {
         string strValue = this[key];
         if (strValue == null)
            return false;
         return double.TryParse(strValue, out value);
         }

		public bool get(string key, ref string value)
			{
			string strValue = this[key];
			if (strValue != null)
				value = strValue;
			return (strValue != null);
			}

		public bool get(string key, ref DateTime value)
			{
			string strValue = this[key];
			if (strValue == null)
				return false;
			return DateTime.TryParse(strValue, out value);
			}


		public bool get(string key, ref Nullable<int> value)
			{
			string strValue = this[key];
			if (strValue == null)
				return false;

			int result;
			if (int.TryParse(strValue, out result))
				{
				value = result;
				return true;
				}
			return false;
			}


		public bool get(string key, ref Nullable<double> value)
			{
			string strValue = this[key];
			if (strValue == null)
				return false;

			double result;
			if (double.TryParse(strValue, out result))
				{
				value = result;
				return true;
				}
			return false;
			}


		public bool get(string key, ref Nullable<DateTime> value)
			{
			string strValue = this[key];
			if (strValue == null)
				return false;

			DateTime result;
			if (DateTime.TryParse(strValue, out result))
				{
				value = result;
				return true;
				}
			return false;
			}

		public abstract bool Save();
		}

   }
