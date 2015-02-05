using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackriverinc.Framework.Utility
	{
	public interface IParameters :
		IEnumerable<KeyValuePair<string, object>>,
		IDictionary<string, object>
		{

		new string this[string key] { get; set; }

		#region Type-conversion Getters
		/// <summary>
		/// Return value from configuration source or default value
		/// </summary>
		/// <param name="key">Retrieval key</param>
		/// <param name="value">Reference to returned value</param>
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
	
		#endregion

		}

	/// <summary>
	/// Basically, a Dictionary<string, object> with the ConfigSettings
	/// converting accessors added.
	/// </summary>
	public class Parameters : IParameters
		{
		Dictionary<string, object> _dictionary;

		protected void initialize(IEnumerable<KeyValuePair<string, object>> parameters)
			{
			_dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			if (parameters != null)
				foreach (KeyValuePair<string, object> key_value in parameters)
					_dictionary.Add(key_value.Key, key_value.Value);
			}

		public Parameters(IEnumerable<KeyValuePair<string, object>> parameters)
			{
			initialize(parameters);
			}

		public Parameters()
			{
			initialize(null);
			}

		public string this[string key]
			{
			get
				{
				if (_dictionary.ContainsKey(key))
					return _dictionary[key].ToString();
				return null;
				}
			set
				{
				if (!_dictionary.ContainsKey(key.ToUpper()))
					_dictionary.Add(key, value);
				else
					_dictionary[key] = value;
				}
			}

		#region Parameter : Type-conversion gets
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

		#endregion

		#region IEnumerable<KeyValuePair<string,object>> Members

		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
			{
			return (IEnumerator<KeyValuePair<string, object>>)_dictionary.GetEnumerator();
			}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
			return _dictionary.GetEnumerator();
			}

		#endregion

		#region IDictionary<string,object> Members

		void IDictionary<string, object>.Add(string key, object value)
			{
			_dictionary.Add(key, value);
			}

		public bool ContainsKey(string key)
			{
			return _dictionary.ContainsKey(key);
			}

		ICollection<string> IDictionary<string, object>.Keys
			{
			get { return _dictionary.Keys; }
			}

		bool IDictionary<string, object>.Remove(string key)
			{
			return _dictionary.Remove(key);
			}

		bool IDictionary<string, object>.TryGetValue(string key, out object value)
			{
			return _dictionary.TryGetValue(key, out value);
			}

		ICollection<object> IDictionary<string, object>.Values
			{
			get { return _dictionary.Values; }
			}

		object IDictionary<string, object>.this[string key]
			{
			get
				{
				return _dictionary[key];
				}
			set
				{
				_dictionary[key] = value;
				}
			}

		#endregion

		#region ICollection<KeyValuePair<string,object>> Members

		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
			{
			((ICollection<KeyValuePair<string, object>>)_dictionary).Add(item);
			}

		void ICollection<KeyValuePair<string, object>>.Clear()
			{
			((ICollection<KeyValuePair<string, object>>)_dictionary).Clear();
			}

		bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
			{
			return ((ICollection<KeyValuePair<string, object>>)_dictionary).Contains(item);
			}

		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
			{
			((ICollection<KeyValuePair<string, object>>)_dictionary).CopyTo(array, arrayIndex);
			}

		int ICollection<KeyValuePair<string, object>>.Count
			{
			get { return ((ICollection<KeyValuePair<string, object>>)_dictionary).Count; }
			}

		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
			{
			get { return ((ICollection<KeyValuePair<string, object>>)_dictionary).IsReadOnly; }
			}

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
			{
			return ((ICollection<KeyValuePair<string, object>>)_dictionary).Remove(item);
			}

		#endregion

		}

	}
