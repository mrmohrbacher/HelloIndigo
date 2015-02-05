using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Blackriverinc.Framework;

namespace Blackriverinc.Framework.DataStore
   {
      public class StreamFactory<TDataStoreAdapter> 
         {
         static Regex pathParser;

         static StreamFactory()
            {
            try
               {
               // Construct the QuerySpecifier Parser
               string pattern = "(?: (?<scheme> file | setting ) ://)?" +
                                 "(?<path> .*)";
               RegexOptions options = RegexOptions.IgnorePatternWhitespace
                                    | RegexOptions.IgnoreCase;
               pathParser = new Regex(pattern, options);
               }
            catch (Exception ex)
               {
               Trace.TraceError(ex.ToString());
               throw;
               }
            }

         static public bool Parse(string resourceSpecifier, ref string scheme, ref string path)
            {
            MatchCollection matches = pathParser.Matches(resourceSpecifier);
            if (matches.Count > 0)
               {
               if (matches[0].Groups["scheme"] != null)
                  scheme = matches[0].Groups["scheme"].Value;
               path = matches[0].Groups["path"].Value;
               return true;
               }
            return false;
            }

         static public Stream Create(string resourceSpecifier,
            FileMode fileMode = FileMode.Open,
            FileAccess fileAccess = FileAccess.Read,
            FileShare fileShare = FileShare.Read)
            {
            Stream result = null;
            try
               {
               string scheme = null;
               string path = null;
               if (!Parse(resourceSpecifier, ref scheme, ref path)
               || scheme.Length == 0)
                  {
                  // Text is simply the specifier itself. Return a buffer over
                  // the input.
                  StringReader reader = new StringReader(resourceSpecifier);
                  UTF8Encoding encoder = new UTF8Encoding();
                  byte[] buffer = encoder.GetBytes(resourceSpecifier);
                  result = new MemoryStream(buffer);
                  return result;
                  }

               //-----------------------------------------------------------
               // Text is stored in a file. The path will either be
               // rooted, in which case we just use it to open the file,
               // relative. In the 2nd case we look relative to the 
               // base directory of the running module.
               //-----------------------------------------------------------
               if (scheme == "file")
                  {
                  // Use the current directory as the root of the file's path.
                  string baseDir = null;
                  path = String.Format(path, DateTime.Now);
                  if (Path.GetPathRoot(path) == string.Empty)
                     {
                     Process process = Process.GetCurrentProcess();
                     ProcessModule module = process.MainModule;
                     baseDir = Path.GetDirectoryName(module.FileName);
                     path = String.Format(@"{0}\{1}", baseDir, path);
                     }

                  baseDir = Path.GetDirectoryName(path);
                  if (!Directory.Exists(baseDir))
                     Directory.CreateDirectory(baseDir);

                  result = File.Open(path, fileMode, fileAccess, fileShare);
                  Debug.WriteLine(string.Format("StreamFactory {0}: 'file://{1}'",
                                                               fileMode,
                                                               path));
                  }

               if (scheme == "setting")
                  {
                  //Debug.WriteLine(string.Format("StreamFactory {0}: 'setting://{1}'",
                  //                           fileMode,
                  //                           path));
                  string sectionName = Path.GetDirectoryName(path);
                  string key = Path.GetFileName(path);


                  IConfigSettings section = new ApplicationConfiguration(sectionName);
                  if (key != null && key.Length > 0)
                     result = Create(section[key], fileMode, fileAccess);
                  }
               }
            catch (Exception ex)
               {
               Trace.TraceError(ex.ToString());
               HandleableException hex = new HandleableException(ex);
               hex.Handled = true;
               throw hex;

               }

            return result;
            }
         }
      }
   }
