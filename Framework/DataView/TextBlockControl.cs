using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRPlus.ScheduledJobs.DataView
	{
	public class TextBlockControl :
		Control, ITextControl
		{

		#region Control Overrides
		protected override void Render(HtmlTextWriter writer)
			{
			writer.AddAttribute("class", "text");
			writer.RenderBeginTag("div");
			writer.WriteLine(writer.NewLine);

			bool containsHTML = Text.StartsWith("<");
			if (!containsHTML)
				writer.WriteBeginTag("p");

			writer.WriteLine(Text);

			if (!containsHTML)
				writer.WriteEndTag("p");

			writer.WriteLine(writer.NewLine);
			base.Render(writer);

			writer.WriteEndTag("div");
			}
		#endregion

		#region ITextControl Members

		public string Text { get; set; }

		#endregion
		}
	}
