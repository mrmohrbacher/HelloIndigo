using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Blackriverinc.DataAccess
   {
   public class BasicURIParser : GenericUriParser
      {

      static string _uriPattern =
            @"(?<SCHEME> \w+ ) \: " +
            @"(?<HOST> (?<ROOTED> //)? [\w-\+\.]+ )" +
            @"(?: \: (?<PORT> \d+ ) )? ";
      static Regex _rUserInfo = new Regex(_uriPattern,
            RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

      static Regex _rParameters = new Regex("(?<PARAMETERS>(?<=;)[\\w%]+\\=[\\w%]+(?=[;\\?\\s]|$))*");
      static Regex _rHeaders = new Regex("(?<HEADERS>(?<=[\\?&]).+?\\=.+?(?=[&;\\?\\s]|$))");

      int _defaultPort = -1;

      public BasicURIParser(GenericUriParserOptions options = GenericUriParserOptions.Default)
         : base(options) { }



      protected override void InitializeAndValidate(Uri uri, out UriFormatException parsingError)
         {
         parsingError = null;
         }

      protected override void OnRegister(string schemeName, int defaultPort)
         {
         _defaultPort = defaultPort;
         }

      protected override string GetComponents(Uri uri, UriComponents components, UriFormat format)
         {
         string s = uri.OriginalString;
         string t = "";

         string result = null;

         switch (components)
            {
         case UriComponents.Host | UriComponents.Port:
            result = GetComponents(uri, UriComponents.Host, format) + ":" +
                GetComponents(uri, UriComponents.Port, format);
            break;
         case UriComponents.AbsoluteUri:
            result = this.GetComponents(uri, UriComponents.SerializationInfoString, format);
            break;
         case UriComponents.Fragment:
            result = "";
            break;
         case UriComponents.Host:
            result = _rUserInfo.Match(s).Groups["HOST"].Value;
            break;
         case UriComponents.HostAndPort:
            result = GetComponents(uri, UriComponents.Host | UriComponents.Port, format);
            break;
         case UriComponents.HttpRequestUrl:
            result = this.GetComponents(uri, UriComponents.SchemeAndServer, format) +
                GetComponents(uri, UriComponents.PathAndQuery, format);
            break;
         case UriComponents.Path:
            MatchCollection mc = _rParameters.Matches(s);
            t = "";
            if (mc.Count > 0)
               {
               foreach (Match m in mc)
                  if (m.Value != "") t = t + m.Value + ";";
               if (t.Length > 0) return Uri.UnescapeDataString(t.Remove(t.Length - 1));
               }
            result = "";
            break;

         case UriComponents.Path | UriComponents.KeepDelimiter:
            t = GetComponents(uri, UriComponents.Path, format);
            if (t == "") return "";
            result = ";" + t;
            break;

         case UriComponents.PathAndQuery:
            result = GetComponents(uri, UriComponents.Path | UriComponents.KeepDelimiter, format) +
                GetComponents(uri, UriComponents.Query | UriComponents.KeepDelimiter, format);
            break;

         case UriComponents.Port:
            t = _rUserInfo.Match(s).Groups["PORT"].Value;
            if (t == "") result = _defaultPort.ToString();
            else result = t;
            break;

         case UriComponents.Query:
            t = "";
            mc = _rHeaders.Matches(s);
            if (mc.Count > 0)
               {
               foreach (Match m in mc)
                  if (m.Value != "") t = t + m.Value + "&";
               return Uri.UnescapeDataString(t.Remove(t.Length - 1));
               }
            return "";

         case UriComponents.Query | UriComponents.KeepDelimiter:
            t = GetComponents(uri, UriComponents.Query, format);
            if (t == "") return "";
            return "?" + t;

         case UriComponents.Scheme:
            result = _rUserInfo.Match(s).Groups["SCHEME"].Value;
            break;

         case UriComponents.SchemeAndServer:
            result = GetComponents(uri, UriComponents.Scheme, format) + ":" +
                GetComponents(uri, UriComponents.HostAndPort, format);
            break;

         case UriComponents.SerializationInfoString:
            result = GetComponents(uri, UriComponents.Scheme, format) + ":" +
                GetComponents(uri, UriComponents.UserInfo, format) +
                GetComponents(uri, UriComponents.HostAndPort, format) +
                GetComponents(uri, UriComponents.PathAndQuery, format);
            break;

         case UriComponents.StrongAuthority:
            return this.GetComponents(uri, UriComponents.UserInfo, format) +
                ":" + this.GetComponents(uri, UriComponents.StrongPort, format);

         case UriComponents.StrongPort:
            return this.GetComponents(uri, UriComponents.Port, format);

         case UriComponents.UserInfo:
            Match mu = _rUserInfo.Match(s);
            string ui = (mu.Groups[2].Value == "" ? mu.Groups[3].Value : mu.Groups[2].Value + ":" + mu.Groups[3].Value);
            if (ui != "") ui = ui + "@";
            return ui;

         default:
            return "";
            }

         return result;
         }
      }
   }