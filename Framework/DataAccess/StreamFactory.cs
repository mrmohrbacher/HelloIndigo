using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using OnlineReporting.Framework;
using OnlineReporting.Framework.ConfigurationSettingsProvider;
using OnlineReporting.Framework.Utility;

namespace OnlineReporting.Framework.DataAccess
   {
   public class StreamFactory
      {
      static Regex pathParser;

      static StreamFactory()
         {
         try
            {
            // Construct the QuerySpecifier Parser
            string pattern = "(?<scheme> file | setting ) ://" +
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

      static public Stream Create(string resourceSpecifier, 
         FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.Read)
         {
         Stream result = null;
            try
               {
               MatchCollection matches = pathParser.Matches(resourceSpecifier);
               if (matches.Count == 0)
                  {
                  // Text is simply the specifier itself. Return a buffer over
                  // the input.
                  StringReader reader = new StringReader(resourceSpecifier);
                  UTF8Encoding encoder = new UTF8Encoding();
                  byte[] buffer = encoder.GetBytes(resourceSpecifier);
                  result = new MemoryStream(buffer);
                  return result;
                  }

               string scheme = matches[0].Groups["scheme"].Value;
               string path   = matches[0].Groups["path"].Value;

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

                  result = File.Open(path, fileMode, fileAccess);
                  Debug.WriteLine(string.Format("StreamFactory {0}: 'file://{1}'", 
                                                               fileMode,
                                                               path));
                  }

               if (scheme == "setting")
                  {
                  string section = Path.GetDirectoryName(path);
                  string key = Path.GetFileName(path);

                  result = Create(new ApplicationConfiguration(section)[key],
                                    fileMode, fileAccess);
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
