using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Blackriverinc.DataAccess
{
	/// <summary>
	///  Provides some common functions required by various pages and classes.
	/// </summary>
	public static class GeneralUtility
	{

		#region YATTP
		/// <summary>
		/// Substitutes matching tokens (like this {token}), with values
		/// that are in the symbol dictionary.
		/// </summary>
		/// <param name="inputs">Input text</param>
		/// <param name="symbols">Dictionary</param>
		/// <returns></returns>
		public static string BuildText(string[] inputs, IDictionary symbols)
		{
			StringBuilder output = new StringBuilder();
			Regex regex = new Regex(@"( \{ (\w+) \} )", 
				RegexOptions.IgnorePatternWhitespace);
			foreach (string input in inputs)
			{
				MatchCollection matches = regex.Matches(input);
				if (matches.Count == 0)					
					output.Append(input);
				else	
				{
					string result = input.Clone().ToString();
					foreach(Match match in matches)
					{
						if (!match.Groups[0].Success)
							throw new ApplicationException("Bad parser pattern in 'BuildText'");

						string symbol = match.Groups[2].Value.ToString();
						string val;
						if (symbols.Contains(symbol))
							val = symbols[symbol].ToString();
						else
							val = "";

#if DEBUG && _TRACEX
						Debug.WriteLine(String.Format(" Token : {0} = {1}", 
							symbol, val));
#endif
						Regex substituter = new Regex(@"\{" + symbol+ @" \}", RegexOptions.IgnorePatternWhitespace);
						result = substituter.Replace(result, val); 
					}
					output.Append(result);
				}
			}

			return output.ToString();
		}

		public static string BuildText(System.IO.Stream istr, IDictionary symbols)
		{
			string[] inputs = null;
			try 
			{
				
				StringCollection template = new StringCollection();
				System.IO.TextReader rdr = new System.IO.StreamReader(istr);
				string line = null;
				while((line = rdr.ReadLine()) != null)
					template.Add(line + "\n");

				inputs = new string[template.Count];
				template.CopyTo(inputs, 0);

			}
			catch(Exception exp)
			{
				Trace.WriteLine(exp);
			}
			return BuildText((inputs != null)?inputs:new string[] {""}, symbols);
		}

		public static string ReadTextFile(string filePath) 
		{
			string text = null;
			StreamReader rdr = null;

			try
			{
				rdr = File.OpenText(filePath);

				text = rdr.ReadToEnd();

			}
			catch(Exception exp)
			{
				Trace.WriteLine(exp);
				text = null;
			}
			finally
			{
				if (rdr != null)
					rdr.Close();
			}

			return text;
		}

		public static int WriteTextFile(string path, string text, System.IO.FileMode mode)
		{
			int writeCnt= -1;
			Stream stream = null;

			try
			{
				Debug.WriteLine(String.Format(@"WriteTextFile : '{0}'", path));

				string dirRoot = System.IO.Path.GetDirectoryName(path);
				if (!System.IO.Directory.Exists(dirRoot))
					System.IO.Directory.CreateDirectory(dirRoot);
				stream = new FileStream(path, mode);
				ASCIIEncoding enc = new ASCIIEncoding();
				Byte[] bytes = enc.GetBytes(text);
				stream.Write(bytes, 0, bytes.GetLength(0));
				writeCnt = bytes.GetLength(0);
			}
			catch(Exception ex)
			{
				Trace.WriteLine(ex.ToString());
				writeCnt = -1;
			}
			finally
			{

				if (stream != null)
				{
					stream.Close()	;
					stream = null;
					}
			}

			return(writeCnt);
		}

		#endregion
	}
}
