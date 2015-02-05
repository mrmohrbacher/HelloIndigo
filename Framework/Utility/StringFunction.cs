using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Blackriverinc.Framework.Utility
	{
	public static class StringFunction
		{
		static Regex camelCaseParser;
		static Regex commandLineParser;

		public enum DictionaryNotFoundOptions
			{
			Ignore,
			AddKey,
			ThrowException
			}

		static StringFunction()
			{
			camelCaseParser = new Regex(@"(?<token>[A-Z][a-z0-9]+)", RegexOptions.IgnorePatternWhitespace);
			commandLineParser = new Regex(@"(?: (?:/|-) (?<keyword> \w+ )(?: \s*=\s* )(?<value> \S+ ) )",
															RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			}

		/// <summary>
		/// Converts 'CamelCased' string into a delimitted string. 'CamelCase' => 'Camel Case'.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="delimitter">Default is ' '</param>
		/// <returns></returns>
		public static string CamelCaseToDelimitted(this string input, char delimitter = ' ')
			{
			StringBuilder result = new StringBuilder();

			if (input != null)
				{
				MatchCollection matches = camelCaseParser.Matches(input);
				foreach (Match match in matches)
					result.AppendFormat("{0}{1}", match.Groups["token"].Value, delimitter);
				}

			return result.ToString();
			}

		public static string DelimittedToCamelCase(this string input, string delimitters = " ")
			{
			StringBuilder result = new StringBuilder();

			if (input != null)
				{
				char[] _delimitters = new char[delimitters.Length];
				for (int i = 0; i < delimitters.Length; i++ )
					{
					_delimitters[i] = delimitters[i];
					}
				string[] components = input.Split(_delimitters, StringSplitOptions.RemoveEmptyEntries);
				foreach (string component in components)
					{
					result.AppendFormat("{0}{1}", component.Substring(0, 1).ToUpper(),
															component.Substring(1));
					}
				}

			return result.ToString();
			}

		/// <summary>
		/// Parse inpput string for Command-line style arguments
		/// </summary>
		/// <param name="input"></param>
		/// <param name="dictionary"></param>
		/// <param name="amendable">true : add new words to keyWords; false : throw Exception</param>
		/// <returns></returns>
		public static IDictionary<string, object> CommandLineParser(this string input, IDictionary<string, object> dictionary, 
																	DictionaryNotFoundOptions option = DictionaryNotFoundOptions.Ignore)
			{

			MatchCollection matches = commandLineParser.Matches(input);
			if (matches.Count == 0)
				throw new ArgumentOutOfRangeException(string.Format("{0} : Does not parse.", input));

			foreach (Match match in matches)
				{
				string keyword = match.Groups["keyword"].Value;
				string value = match.Groups["value"].Value;

				if (!dictionary.ContainsKey(keyword))
					{
					switch(option)
						{
					case DictionaryNotFoundOptions.AddKey:
						dictionary.Add(keyword, value);
						break;
					case DictionaryNotFoundOptions.ThrowException:
						throw new ArgumentOutOfRangeException(keyword, "Not in list of acceptable keywords.");
					default:
						Debug.WriteLine(string.Format("{0} : Not in list of known keywords.", keyword));
						break;
						}
					}
				else
					dictionary[keyword] = value;
				}

			return dictionary;
			}

		/////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Resturns a subsetof the cardinal numbers as a string representing
		/// its name.
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string CardinalNumbers(string numberString)
			{

			ushort number = ushort.MaxValue;
			ushort.TryParse(numberString, out number);
			return CardinalNumbers(number);
			}

		public static string CardinalNumbers(ushort number)
			{
			if (number == 0)
				return "zero";

			if (number >= 100)
				throw new ArgumentOutOfRangeException("number", 
													string.Format("Input out of range [0..99]"));

			string[] onesPlace = 
				{ "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", 
				  "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", 
									"eighteen", "ninteen"};
			string[] tensPlace = { "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

			string result = "";

			int ones = 0;
			int tens = (int)Math.DivRem(number, 10, out ones);
			result = string.Format("{0}{2}{1}", 
						tensPlace[tens], 
						(number < 20)?onesPlace[number]:onesPlace[ones],
						(tens > 1 && ones > 0) ? "-" : "");
			
			return result;
			}


		}
	}
