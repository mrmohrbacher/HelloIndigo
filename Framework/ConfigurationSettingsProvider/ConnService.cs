using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Common.ConfigurationSettingsProvider
   {
   public class ConnService :
      ConfigSettings,
      IConfigSettings,
      IDisposable
      {
      private static Dictionary<string, object> _settings;

      private static string resultPattern;
      private static Regex resultParser;

      static ConnService()
         {
         _settings = new Dictionary<string, object>();

         resultPattern = @"(?<TAG> \w+ ) \= (?<VALUE> .+)";
         resultParser = new Regex(resultPattern,
         RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
         }

      //private dbconn.conn _conn;

      //public dbconn.conn Conn
      //   {
      //   get
      //      {
      //      if (!_disposed)
      //         {
      //         if (_conn == null)
      //            _conn = new dbconn.conn();
      //         if (_conn == null)
      //            throw new ApplicationException(String.Format("Cannot create dbconn object. {0}", _conn));
      //         }
      //      return _conn;
      //      }
      //   }

      public ConnService()
         {
         }

      public ConnService(string parent)
         : this()
         {
         Section = parent;
         }

      #region "ENVIRON"
      //private string _environment;
      //public string uENVIRON
      //   {
      //   get
      //      {
      //      lock (_settings)
      //         {
      //         try
      //            {
      //            if (_environment == null)
      //               {
      //               _environment = Conn.getResource("PRODUCTION", "GENERAL", "WEBENVIRON");
      //               if (_environment.Contains("ERROR"))
      //                  throw new ApplicationException(String.Format("DbConn[{0}] Failed to fetch : {1}\n{2}",
      //                     Conn.Url, "WEBENVIRON", _environment));

      //               }
      //            }
      //         catch (Exception exp)
      //            {
      //            Trace.WriteLine(exp.ToString());
      //            }

      //         return _environment;
      //         }
      //      }
      //   set
      //      {
      //      string resource = getResource(value);
      //      if (resource != null)
      //         _environment = value;
      //      }
      //   }

      #endregion

      #region "PARENT"
      private string _parent;
      //public string uPARENT
      //   {
      //   get
      //      {
      //      lock (_settings)
      //         {
      //         try
      //            {
      //            if (_parent == null)
      //               {
      //               _parent = Conn.getResource("PRODUCTION", "GENERAL", "WEBPARENT");
      //               if (_parent.Contains("ERROR"))
      //                  throw new ApplicationException(String.Format("DbConn[{0}] Failed to fetch : {1}\n{2}",
      //                     Conn.Url, "WEBPARENT", _parent));

      //               }
      //            }
      //         catch (Exception exp)
      //            {
      //            Trace.WriteLine(exp.ToString());
      //            }

      //         return _parent;
      //         }
      //      }
      //   set
      //      {
      //      Section = value;
      //      }
      //   }

      #endregion

      //private string getValue(string tag)
      //   {
      //   //string result = Conn.getConnectionString(uENVIRON, uPARENT, tag);
      //   //if (result.Contains("ERROR"))
      //   //   {
      //   //   Trace.WriteLine(String.Format("DbConn Failed to fetch : {0}\n{1}",
      //   //                           tag, result));
      //   //   result = null;
      //   //   }
      //   //return result;
      //   }

      private string getResource(string tag)
         {
             string result = "";
         //string result = Conn.getResource("PRODUCTION", "GENERAL", tag);
         //if (result.Contains("ERROR"))
         //   {
         //   Trace.WriteLine(String.Format("DbConn Failed to fetch resource: {0}\n{1}",
         //                           tag, result));
         //   result = null;
         //   }
         return result;
         }

      #region IConfigurationSettingsProvider Members

      public override string this[string key]
         {
         get
            {
                string value = "";
            //string value = getValue(key);
            //if (value != null)
            //   {
            //   Match match = resultParser.Match(value);
            //   if (match.Success)
            //      {
            //      string tag = match.Groups["TAG"].Value;
            //      if (tag.ToUpper() == key.ToUpper())
            //         value = match.Groups["VALUE"].Value;
            //      }
            //   }
            return value;
            }
         }

      public string Section
         {
         get
            {
            return _parent;
            }
         set
             {
                 ;
            //string resource = getResource(value);
            //if (resource == null)
            //   throw new ApplicationException(string.Format("Invalid 'Parent' : {0}", value));
            //_parent = resource;
            }
         }

      #endregion

      #region IEnumerable<KeyValuePair<string,object>> Members

      public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
         {
         return _settings.GetEnumerator();
         }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
         {
         return _settings.GetEnumerator();
         }

      #endregion

      #region "IDisposable Members"

      private bool _disposed = false;
      ~ConnService()
         {
         Dispose(false);
         }

      public void Dispose()
         {
         Dispose(true);

         GC.SuppressFinalize(this);
         }

      public void Dispose(bool disposing)
         {
         //if (_disposed)
         //   return;

         //if (disposing)
         //   {
         //   if (_conn != null)
         //      _conn.Dispose();
         //   _conn = null;
         //   }

         //_disposed = true;
         }

      #endregion
      }
   }
