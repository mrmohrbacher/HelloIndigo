using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


namespace OnlineReporting.Framework.DataView
   {
	public interface IHTMLReportView : ITextControl
		{
		/// <summary>
		/// Model definition
		/// </summary>
		IEnumerable DataSource { get; set; }

		/// <summary>
		/// Generates a unique identifier for each view
		/// </summary>
		string ID { get; }

		/// <summary>
		/// Render the current DataSource onto a Stream
		/// </summary>
		/// <returns></returns>
		Stream Render();
      Stream Render(IEnumerable dataSource);
      Stream Render(IDataSource dataSource);

		/// <summary>
		/// Retrieve Styles dictionary. Client adds new style
		/// definitions keyed by selector.
		/// </summary>
		IDictionary<string, string> Styles { get; }

		new string Text
		{ get; set; }

		}

	////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////

	/// <summary>
	/// Using an embedded 'Renderer', write HTML shaped-text onto the
	/// a stream.
	/// </summary>
	/// <typeparam name="RenderType">Control, IDataBoundControl, new()</typeparam>
	public class HTMLReportView<RenderType> : Control, IHTMLReportView, IDisposable
		where RenderType : Control, IDataBoundControl, new()
		{

		#region ID
		int _id;
		public override string ID
			{
			get
				{
				return string.Format("VIEW{0}", _id);
				}
			}
		#endregion

		#region Writer
		/// <summary>
		/// true : View owns the underlying Stream
		/// false : View was created with an external Stream
		/// </summary>
		protected bool _ownStream = false;

		private StreamWriter _writer = null;

		protected StreamWriter Writer 
			{
			get
				{
				if (_ownStream && _writer == null)
					{
					// Deferred initialization of the Stream
					Initialize(new System.IO.StreamWriter(new MemoryStream()));
					}
				return _writer;
				}
			set
				{
				Initialize(value);
				}
			}

		#endregion

		#region Renderer
		private RenderType _renderer;
		public RenderType Renderer { get { return _renderer; } }
		#endregion

		#region "Constructors : default, Stream, StreamWriter"

		/// <summary>
		/// Default Renderer writes onto a StringStream.
		/// </summary>
		public HTMLReportView()
			{
			_ownStream = true;
			}

		public HTMLReportView(Stream outputStream)
			{
			Initialize(new StreamWriter(outputStream));
			}

		public HTMLReportView(StreamWriter writer)
			{
			Initialize(writer);
			}

		private void Initialize(StreamWriter writer)
			{
			_writer = writer;
			_renderer = new RenderType();
         _id = OnlineReporting.Framework.Utility.Identifier.ID;
			}

		#endregion

		#region IHTMLView Members

		#region DataSource
		private IEnumerable _dataSource = null;
		public IEnumerable DataSource
			{
			get
				{
				return _dataSource;
				}
			set
				{
				_dataSource = value;
				}
			}
		#endregion

		#region Render
		public virtual Stream Render()
			{
			if (DataSource == null)
				throw new ApplicationException("'Render' called without a DataSource given or defined.");

			return Render(DataSource);
			}

      public virtual Stream Render(IEnumerable dataSource)
         {
         XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(_writer);

         // <div class='core'>
         xhtmlWriter.AddAttribute("class", "core");
         xhtmlWriter.RenderBeginTag("div");
         xhtmlWriter.WriteLine(Writer.NewLine);


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
         if (dataSource != null)
            {
            Renderer.DataSource = dataSource;
            Renderer.DataBind();
            }

         xhtmlWriter.Flush();

         // Render unto the Stream that which belongs to the Stream...
         Renderer.ID = ID;
         Renderer.RenderControl(xhtmlWriter);

         // </div>
         xhtmlWriter.WriteEndTag("div");

         xhtmlWriter.Flush();

         return _writer.BaseStream;
         }

      public virtual Stream Render(IDataSource dataSource)
         {
         XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(_writer);

         // <div class='core'>
         xhtmlWriter.AddAttribute("class", "core");
         xhtmlWriter.RenderBeginTag("div");
         xhtmlWriter.WriteLine(Writer.NewLine);


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
         if (dataSource != null)
            {
            Renderer.DataSource = dataSource;
            Renderer.DataBind();
            }

         xhtmlWriter.Flush();

         // Render unto the Stream that which belongs to the Stream...
         Renderer.ID = ID;
         Renderer.RenderControl(xhtmlWriter);

         // </div>
         xhtmlWriter.WriteEndTag("div");

         xhtmlWriter.Flush();

         return _writer.BaseStream;
         }
      #endregion

		#region Styles
		protected IDictionary<string, string> _styles;
		public IDictionary<string, string> Styles
			{
			get
				{
				return _styles;
				}
			}
		#endregion

		#region Text
		public string Text
		{ get; set; }
		#endregion

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Finalizer 
		/// </summary>
		~HTMLReportView()
			{
			Dispose(false);
			}

		bool _isDisposed = false;

		public override void Dispose()
			{
			base.Dispose();

			Dispose(true);

			GC.SuppressFinalize(this);
			}

		protected virtual void Dispose(bool disposing)
			{
			if (!_isDisposed)
				return;

			if (disposing)
				{
				if (_ownStream)
					_writer.Dispose();

				Renderer.Dispose();
				}

			_isDisposed = true;
			}

		#endregion
		}


	/// <summary>
	/// Emit DataSource bound to a HTMLView within an simple XHTML page. 
	/// Sub-class of HTMLView, this creates an entire HTML Document.
	/// </summary>
	public class HTMLReportDocumentView<RenderType> : HTMLReportView<RenderType>, IDisposable
		where RenderType : Control, IDataBoundControl, new()
		{
		#region "Constructors : default, stream, writer"
		/// <summary>
		/// Default Renderer writes onto a StringStream.
		/// </summary>
		public HTMLReportDocumentView() : 
			this(new MemoryStream())
			{
			_ownStream = true;
			}

		public HTMLReportDocumentView(Stream outputStream) :
			this(new StreamWriter((outputStream != null)?outputStream:(new MemoryStream())))
			{
			_ownStream = (outputStream == null);
			}

		public HTMLReportDocumentView(StreamWriter writer) : base(writer)
			{
			ReportHeading = "";

			_styles = new Dictionary<string, string>();
			_styles.Add(".core th, .core td", "font-size:9pt;font-family:sans-serif;border-style:solid;border-width:1pt;padding:4px;");
			_styles.Add(".core th", "background-color:#222244;color: white;font-family:sans-serif;");
			_styles.Add(".heading", "font: 12pt sans-serif;");
			_styles.Add(".text", "font: 10pt sans-serif;");
			}

		#endregion

		public Stream RenderPageBegin()
			{
			XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(Writer);
			xhtmlWriter.Indent = 3;

			xhtmlWriter.AddAttribute("xmlns", "http://www.w3.org/1999/xhtml");
			xhtmlWriter.RenderBeginTag("html");

			xhtmlWriter.RenderBeginTag("head");
			if (ReportHeading != null && ReportHeading.Length > 0)
				{
				xhtmlWriter.RenderBeginTag("title");
				xhtmlWriter.Write(ReportHeading);
				xhtmlWriter.WriteEndTag("title");
				}

			// <style type='text/css'>
			xhtmlWriter.WriteLine(Writer.NewLine);
			xhtmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
			xhtmlWriter.RenderBeginTag("style");
			foreach (var style in _styles)
				{
				xhtmlWriter.WriteLine(style.Key);
				xhtmlWriter.WriteLine("{");
				xhtmlWriter.WriteLine(style.Value);
				xhtmlWriter.WriteLine("}");
				}
			xhtmlWriter.WriteEndTag("style");
			xhtmlWriter.WriteLine(Writer.NewLine);
			// </style>

			xhtmlWriter.WriteEndTag("head");

			// <body>
			xhtmlWriter.RenderBeginTag("body");

			if (ReportHeading != null && ReportHeading.Length > 0)
				{
				// <h2 class='heading'>
				xhtmlWriter.AddAttribute("class", "heading");
				xhtmlWriter.RenderBeginTag("h2");
				xhtmlWriter.Write(ReportHeading);
				xhtmlWriter.WriteEndTag("h2");
				xhtmlWriter.WriteLine(Writer.NewLine);
				// </h2>
				}

			if (!(Renderer is ITextControl) && Text != null && Text.Length > 0)
				{
				// <p class='text'>
				xhtmlWriter.AddAttribute("class", "caption");
				xhtmlWriter.RenderBeginTag("p");
				xhtmlWriter.Write(Text);
				xhtmlWriter.WriteEndTag("p");
				xhtmlWriter.WriteLine(Writer.NewLine);
				// </p>
				}

			// Flush writer contents to output.
			xhtmlWriter.Flush();

			return Writer.BaseStream;
			}

		public Stream RenderPageEnd()
			{
			XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(Writer);
			xhtmlWriter.Indent = 3;

			// Flush writer contents to output.
			xhtmlWriter.Flush();

			if (ReportFooter != null && ReportFooter.Length > 0)
				{
				xhtmlWriter.RenderBeginTag("hr");
				xhtmlWriter.WriteEndTag("hr");

				// 
				xhtmlWriter.AddAttribute("class", "footer");
				xhtmlWriter.RenderBeginTag("div");

				xhtmlWriter.Write(ReportFooter);
				xhtmlWriter.WriteEndTag("div");

				xhtmlWriter.RenderBeginTag("hr");
				xhtmlWriter.WriteEndTag("hr");

				xhtmlWriter.WriteLine(Writer.NewLine);
				// </h2>
				}
			//--------------------------------------------------------
			// No matter what state the Renderer left us in, close 
			// off the <body> and <html> elements.
			//--------------------------------------------------------

			// </body>
			xhtmlWriter.WriteEndTag("body");

			// </html>
			xhtmlWriter.WriteEndTag("html");

			// Flush writer contents to output.
			xhtmlWriter.Flush();

			return Writer.BaseStream;
			}

      public Stream RenderPage(IEnumerable dataSource)
         {
         RenderPageBegin();

         XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(Writer);
         xhtmlWriter.Indent = 3;

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         // Emit Core of Page
         HTMLReportView<RenderType> view = new HTMLReportView<RenderType>(Writer);
         view.Text = Text;
         view.Render(dataSource);

         RenderPageEnd();

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         return Writer.BaseStream;
         }

      public Stream RenderPage(IDataSource dataSource)
         {
         RenderPageBegin();

         XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(Writer);
         xhtmlWriter.Indent = 3;

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         // Emit Core of Page
         HTMLReportView<RenderType> view = new HTMLReportView<RenderType>(Writer);
         view.Text = Text;
         view.Render(dataSource);

         RenderPageEnd();

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         return Writer.BaseStream;
         }

      public override Stream Render(IEnumerable dataSource)
         {
         // If we have not written anything _yet_ to the Stream, we will
         // be writing a complete page with this call.
         if (Writer.BaseStream.Position == 0)
            return RenderPage(dataSource);

         XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(Writer);
         xhtmlWriter.Indent = 3;

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         // Emit Core of Page
         HTMLReportView<RenderType> view = new HTMLReportView<RenderType>(Writer);
         view.Text = Text;
         view.Render(dataSource);

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         return Writer.BaseStream;
         }

      public override Stream Render(IDataSource dataSource)
         {
         // If we have not written anything _yet_ to the Stream, we will
         // be writing a complete page with this call.
         if (Writer.BaseStream.Position == 0)
            return RenderPage(dataSource);

         XhtmlTextWriter xhtmlWriter = new XhtmlTextWriter(Writer);
         xhtmlWriter.Indent = 3;

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         // Emit Core of Page
         HTMLReportView<RenderType> view = new HTMLReportView<RenderType>(Writer);
         view.Text = Text;
         view.Render(dataSource);

         // Flush writer contents to output.
         xhtmlWriter.Flush();

         return Writer.BaseStream;
         }

      public string ReportHeading
		{ get; set; }

		public string ReportFooter
		{ get; set; }

		}

	}
