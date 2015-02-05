using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Blackriverinc.Framework.Utility
	{
	public class CommandLineParser
		{
		// --------------------------------------------------------------------------
		// 
		//  Parse the command line arguments
		// 
		private static Regex cmdLineParser = null;

		static CommandLineParser()
			{
			cmdLineParser = new Regex(@"(?:(?:/|-)(?<keyword>\w+)((?:\s*(?: = | : )\s*)(?<value>(?:(?:""[^""]*"")|(?:'[^']*')|(?:\S+))))?)", 
					RegexOptions.IgnoreCase|RegexOptions.IgnorePatternWhitespace);
			}


		public static bool Parse(StringDictionary keyWords, string cmdLine, bool addMissing = true)
			{
			int results = 0;

			Debug.WriteLine(string.Format("CmdLine : '{0}'", cmdLine));

			MatchCollection matches = cmdLineParser.Matches(cmdLine);
			foreach (Match match in matches)
				{
				string keyWord = match.Groups["keyword"].Value;
				string value = match.Groups["value"].Value;
				// The pattern's lexer will guarentee that we have balanced quotes.
				if (value.StartsWith(@"""") || value.StartsWith(@"'"))
					value = value.Substring(1, value.Length - 2);


				if (!keyWords.ContainsKey(keyWord))
					{
					if (!addMissing)
						continue;

					keyWords.Add(keyWord, value);
					}
				else
					keyWords[keyWord] = value;	

				if (keyWord != null)
					results++;
				}

			return (results > 0);
			}

		public static bool Parse(IDictionary<string, object> keyWords, string cmdLine, bool addMissing = true)
			{
			int results = 0;

			Debug.WriteLine(string.Format("CmdLine : '{0}'", cmdLine));

			MatchCollection matches = cmdLineParser.Matches(cmdLine);
			foreach (Match match in matches)
				{
				string keyWord = match.Groups["keyword"].Value;
				string value = match.Groups["value"].Value;
				// The pattern's lexer will guarentee that we have balanced quotes.
				if (value.StartsWith(@"""") || value.StartsWith(@"'"))
					value = value.Substring(1, value.Length - 2);


				if (!keyWords.ContainsKey(keyWord))
					{
					if (!addMissing)
						continue;

					keyWords.Add(keyWord, value);
					}

				// Keyword is a 'switch' : its value is its presence
				if (value.Length == 0)
					{
					if (keyWords[keyWord] == null)
						keyWords[keyWord] = true;
					else
						switch (System.Type.GetTypeCode(keyWords[keyWord].GetType()))
							{
						case TypeCode.Int16:
						case TypeCode.Int32:
						case TypeCode.Int64:
							keyWords[keyWord] = 0;
							break;
						case TypeCode.String:
							keyWords[keyWord] = "";
							break;
						default:
							keyWords[keyWord] = true;
							break;
							}
					}
				else
					switch (System.Type.GetTypeCode(keyWords[keyWord].GetType()))
						{
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
						keyWords[keyWord] = int.Parse(value);
						break;
					case TypeCode.String:
						keyWords[keyWord] = value;
						break;
					case TypeCode.DateTime:
						keyWords[keyWord] = DateTime.Parse(value);
						break;
					default:
						keyWords[keyWord] = true;
						break;
						}

				Debug.WriteLine(string.Format("   {0} = {1}", 
					keyWord, (keyWords[keyWord] == null)?"*null*":keyWords[keyWord]));

				if (keyWord != null) 
					results++;
				}

			return (results > 0);
			}
		}
	}
