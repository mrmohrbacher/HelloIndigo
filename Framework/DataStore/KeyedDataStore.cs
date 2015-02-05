using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Blackriverinc.Framework.DataStore
   {
   public interface IKeyedDataStore :
      IEnumerable<KeyValuePair<string, string>>,
      IDictionary<string, string>
      {

      new string this[string key] { get; set; } 

      #region Type-conversion Getters
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

      #endregion

      }

   /// <summary>
   /// Basically, a Dictionary<string, object> with the ConfigSettings
   /// converting accessors added.
   /// </summary>
   public class KeyedDataStore : IKeyedDataStore
      {
      protected IDictionary<string, string> _cache;

      public string this[string key]
         {
         get
            {
            return ((IDictionary<string, string>)this)[key];
            }
         set
            {
            ((IDictionary<string, string>)this)[key] = value;
            }
         }

      protected void initialize(IEnumerable<KeyValuePair<string, string>> rows)
         {
			try
				{
				//------------------------------------------------------------------
				// Create the settings cache on the first call into this method.
				//------------------------------------------------------------------
            _cache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);            
				}
			catch (Exception ex)
				{
				Trace.TraceError(ex.ToString());
				throw;
				}

         if (rows != null)
            foreach (KeyValuePair<string, string> key_value in rows)
               ((IDictionary<string, string>)this)[key_value.Key] = key_value.Value;
         }

      public KeyedDataStore(IEnumerable<KeyValuePair<string, string>> ienum)
         {
         initialize(ienum);
         }

      public KeyedDataStore(IDataStoreProvider provider)
         {
         initialize(null);
			provider.Load(this);
         }


      #region Type-conversion gets
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

      #region IEnumerable<KeyValuePair<string,string>> Members

      IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
         {
         return (IEnumerator<KeyValuePair<string, string>>)_cache.GetEnumerator();
         }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
         {
         return _cache.GetEnumerator();
         }

      #endregion

      #region IDictionary<string,string> Members

      string IDictionary<string, string>.this[string key]
         {
         get
            {
            string result = null;
            try
               {
               lock (_cache)
                  {
                  if (_cache.ContainsKey(key))
                     {
                     object value = _cache[key];
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
               lock (_cache)
                  {
                  if (!_cache.ContainsKey(key))
                     _cache.Add(key, value);
                  else
                     _cache[key] = value;
                  }
               }
            catch (Exception ex)
               {
               Trace.TraceError(ex.ToString());
               throw;
               }

            }
         }

      void IDictionary<string, string>.Add(string key, string value)
         {
         _cache.Add(key, value);
         }

      public bool ContainsKey(string key)
         {
         return _cache.ContainsKey(key);
         }

      ICollection<string> IDictionary<string, string>.Keys
         {
         get { return _cache.Keys; }
         }

      bool IDictionary<string, string>.Remove(string key)
         {
         return _cache.Remove(key);
         }

      bool IDictionary<string, string>.TryGetValue(string key, out string value)
         {
         return _cache.TryGetValue(key, out value);
         }

      ICollection<string> IDictionary<string, string>.Values
         {
         get { return _cache.Values; }
         }

      #endregion

      #region ICollection<KeyValuePair<string,string>> Members

      void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
         {
         ((ICollection<KeyValuePair<string, string>>)_cache).Add(item);
         }

      void ICollection<KeyValuePair<string, string>>.Clear()
         {
         ((ICollection<KeyValuePair<string, string>>)_cache).Clear();
         }

      bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
         {
         return ((ICollection<KeyValuePair<string, string>>)_cache).Contains(item);
         }

      void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
         {
         ((ICollection<KeyValuePair<string, string>>)_cache).CopyTo(array, arrayIndex);
         }

      int ICollection<KeyValuePair<string, string>>.Count
         {
         get { return ((ICollection<KeyValuePair<string, string>>)_cache).Count; }
         }

      bool ICollection<KeyValuePair<string, string>>.IsReadOnly
         {
         get { return ((ICollection<KeyValuePair<string, string>>)_cache).IsReadOnly; }
         }

      bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
         {
         return ((ICollection<KeyValuePair<string, string>>)_cache).Remove(item);
         }

      #endregion
      }

   }
