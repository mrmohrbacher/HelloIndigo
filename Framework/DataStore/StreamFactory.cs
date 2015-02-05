using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;


namespace Blackriverinc.Framework.DataStore
	{
	public class StreamFactory
		{
		static Regex _pathParser;

		public delegate Stream FactoryDelegate(string resourceSpecifier, object[] args);

		static IDictionary<string, FactoryDelegate> _registry;

		/// <summary>
		/// Register a new factory with the registery. Replaces an existing
		/// factory if the scheme is already registered.
		/// </summary>
		static public void Register(string scheme, FactoryDelegate factory)
			{
			if (_registry == null)
				{
				_registry = new Dictionary<string, FactoryDelegate>(16);
				}

			// Register Scheme
			if (!_registry.ContainsKey(scheme))
				{
				_registry.Add(scheme, factory);
				}
			else
				{
				_registry[scheme] = factory;
				}
			}


		/// <summary>
		/// Register well-known schemes: 'http', 'file', 'string'(null).
		/// <para>
		/// 
		/// Example: Recursively resolve the path from a Settings store.
		/// 
		/// StreamFactory::Register("setting", ((path, args) =>
		///		{
		///		Stream result = null;
		///		string sectionName = Path.GetDirectoryName(path);
		///		string key = Path.GetFileName(path);
		///
		///		TDataStoreAdapter adapter = new TDataStoreAdapter();
		///		IKeyedDataStore settings = new KeyedDataStore(adapter);
		///
		///		if (key != null && key.Length > 0)
		///			result = StreamFactory::Create(settings[key], args);
		///		return result;
		///		}));
		///		
		/// </para>
		/// </summary>
		static StreamFactory()
			{
			try
				{
				//----------------------------------------------------------
				// Register some well-known schemes

				// Text is simply the path itself. Return a buffer over
				// the content.
				Register("string", ((path, args) =>
					{
					Stream result;
					StringReader reader = new StringReader(path);
					UTF8Encoding encoder = new UTF8Encoding();
					byte[] buffer = encoder.GetBytes(path);
					result = new MemoryStream(buffer);
					return result;
					}));

				// Create a Stream from the response stream of an web-resource
				Register("http", ((path, args) =>
					{
						Stream result = null;
						Uri uri = new Uri("http://" + path);
						WebRequest request = WebRequest.CreateDefault(uri);
						using (WebResponse response = request.GetResponse())
							{
							// Make a copy of the contents of the response stream. The Response object
							// and it's stream will be closed upon exiting the using-block, and any
							// attempt to read it will result in System.Net.WebException.
							result = new MemoryStream();
							response.GetResponseStream().CopyTo(result);
							result.Position = 0;
							}
						return result;
					}));

				//-----------------------------------------------------------
				// Contents are stored in a file. The path will either be
				// rooted or releative. IN the rooted case, in which case 
				// we just use the spath to open the file.
				// In the 2nd case we look relative to the 
				// base directory of the running module.
				//-----------------------------------------------------------
				Register("file", ((path, args) =>
					{
						Stream result;
						// Use the current directory as the root of the file's path.
						string baseDir = null;
						path = String.Format(path, DateTime.Now);
						if (!Path.IsPathRooted(path))
							{
							Process process = Process.GetCurrentProcess();
							ProcessModule module = process.MainModule;
							baseDir = Path.GetDirectoryName(module.FileName);
							path = String.Format(@"{0}\{1}", baseDir, path);
							}

						baseDir = Path.GetDirectoryName(path);
						if (!Directory.Exists(baseDir))
							Directory.CreateDirectory(baseDir);

						// Parse File Access parameters out of argument list.
						Func<object[], Type, object> argParser = 
							((al, type) => 
								{
								if (al != null && al.Length > 0)
									return (from fm in al where fm.GetType() == type select fm).First();
								return null;
								});


			
						FileMode fileMode = (FileMode)(argParser(args, typeof(FileMode)) ?? FileMode.Open);
						FileAccess fileAccess = (FileAccess)(argParser(args, typeof(FileMode)) ?? FileAccess.Read);
						FileShare fileShare = (FileShare)(argParser(args, typeof(FileMode)) ?? FileShare.Read);
						result = File.Open(path, fileMode, fileAccess, fileShare);
						Debug.WriteLine(string.Format("StreamFactory {0}: 'file://{1}'",
																					fileMode,
																					path));

						return result;
					}));

				const string schemeTemplate = @"(?: (?<scheme> \w+ ) :(?: //)?)?" +
											"(?<path> .*)";
				_pathParser = new Regex(schemeTemplate,
												RegexOptions.IgnorePatternWhitespace
											 | RegexOptions.IgnoreCase);

				}
			catch (Exception ex)
				{
				Trace.TraceError(ex.ToString());
				throw;
				}
			}

		
		/// <summary>
		/// Parse the Scheme and Path out of a ResourceSpecifier.
		/// <para>ResourceSpecifier := (Scheme ':' ('//')?)? Path
		/// </para>
		/// </summary>
		/// <returns>true iff a valid ResourceSpecifier is given.</returns>
		static public bool Parse(string resourceSpecifier, ref string scheme, ref string path)
			{
			MatchCollection matches = _pathParser.Matches(resourceSpecifier);
			if (matches.Count > 0)
				{
				if (matches[0].Groups["scheme"] != null)
					scheme = matches[0].Groups["scheme"].Value;
				path = matches[0].Groups["path"].Value;
				return true;
				}
			return false;
			}

		static public Stream Create(string resourceSpecifier, params object[] args)
			{
			Stream result = null;
			try
				{
				string scheme = null;
				string path = null;
				if (!Parse(resourceSpecifier, ref scheme, ref path))
					{
					// Text is simply the specifier itself. Return a buffer over
					// the input.
					StringReader reader = new StringReader(resourceSpecifier);
					UTF8Encoding encoder = new UTF8Encoding();
					byte[] buffer = encoder.GetBytes(resourceSpecifier);
					result = new MemoryStream(buffer);
					return result;
					}

				if (!_registry.ContainsKey(scheme))
					{
					throw new StreamFactoryRegistrationException(scheme);
					}

				result = _registry[scheme](path, args);
				}
			catch (Exception ex)
				{
				Trace.TraceError(ex.ToString());
				throw;
				}

			return result;
			}
		}
	}
