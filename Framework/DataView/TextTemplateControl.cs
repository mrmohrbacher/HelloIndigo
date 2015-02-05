using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace OnlineReporting.Framework.DataView
   {
	public class TextTemplateControl : 
		DictionaryBoundControl, ITextControl
		{
		private Regex _tokenParser;

		private const string _defaultTokenPattern = @"(?: \# (?<token> \w+ ) \# )";
		private string _tokenPattern;
		public string TokenPattern
			{
			get
				{
				if (_tokenPattern == null || _tokenPattern.Length == 0)
					_tokenPattern = _defaultTokenPattern;
				return _tokenPattern;
				}
			set
				{
				// Use a fixed Parser to verify the new Pattern
				if (!_patternParser.Match(value).Success)
					throw new ApplicationException(string.Format("Token Pattern '{0}' must contain named group (?:<token> )", 
																				value));

				_tokenPattern = value;
				_tokenParser = new Regex(_tokenPattern, 
					RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
				}
			}

		// Verifies that any new pattern is valid
		private Regex _patternParser;
		private const string _patternVerifier = @"\<token\>";


		protected override void Initialize(StreamWriter writer)
			{
			base.Initialize(writer);

			_patternParser = new Regex(_patternVerifier);
			TokenPattern = _defaultTokenPattern;
			}

		/// <summary>
		/// Bind the DataSource (as a Dictionary) by using it to substitute
		/// values for tokens.
		/// </summary>
		/// <param name="data"></param>
		protected override void PerformDataBinding(IEnumerable data)
			{
			base.PerformDataBinding(data);

			MatchEvaluator substitute = new MatchEvaluator(t =>
				{
				string value = "";
				string key = t.Groups["token"].Value;
				if (_dictionary.ContainsKey(key))
					value = _dictionary[key].ToString();
				return value;
				});
			Text = _tokenParser.Replace(Text, substitute);

			}

		public override void RenderControl(HtmlTextWriter writer)
			{
			bool containsHTML = Text.StartsWith("<");
			if (!containsHTML)
				writer.RenderBeginTag("p");

			base.RenderControl(writer);

			if (!containsHTML)
				writer.WriteEndTag("p");
			}
		protected override void Render(HtmlTextWriter writer)
			{
			writer.WriteLine(Text);
			writer.WriteLine(writer.NewLine);
//			base.Render(writer);
			}

		#region ITextControl Members

		public string Text {	get; set; }

		#endregion

		}

	}
