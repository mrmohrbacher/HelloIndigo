using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;

namespace HRPlus.ScheduledJobs.DataView
	{
	/// <summary>
	/// Wraps ReportViewer in an HTML Page
	/// </summary>
	public class ReportViewerPage : IHTMLView
		{
		private ReportViewer _viewer;

		#region IHTMLView Members

		public string Text
		{ get; set; }


		public string ReportHeading
		{ get; set; }

		public virtual Stream Render(IEnumerable dataSource)
			{
			XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(_writer);

			//-----------------------------------------------------------------
			// If the 'Renderer' supports the ITextControl interface, we will
			// defer rendering of the 'Text' block to it. The assignment needs
			// to take place before 'DataBind' so the 'Renderer' can act
			// on the Text. (Maybe a token substitution will happen, perhaps!)
			//-----------------------------------------------------------------
			ITextControl _textControl = Renderer as ITextControl;
			if (_textControl != null)
				_textControl.Text = Text;

			// Bind the Data to the Renderer
			Renderer.DataSource = dataSource;
			Renderer.DataBind();

			xhtmlWriter.Flush();

			// Render unto the Stream that which belongs to the Stream...
			Renderer.RenderControl(xhtmlWriter);

			xhtmlWriter.Flush();

			return _writer.BaseStream;
			}

		#endregion

		}
	}
