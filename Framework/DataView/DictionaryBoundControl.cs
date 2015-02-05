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
	public abstract class DictionaryBoundControl : 
		DataBoundControl, IDataBoundControl
		{
		protected bool TraceFlag = 
#if _TRACE
			true;
#else
			false;
#endif

		protected StreamWriter _outputWriter;

		protected Dictionary<string, object> _dictionary;

		#region "Constructors : default, filename, stream, writer"
		/// <summary>
		/// Default Renderer writes onto a Stream.
		/// </summary>
		public DictionaryBoundControl()
			{
			Initialize(new System.IO.StreamWriter(new MemoryStream()));
			}

		public DictionaryBoundControl(StreamWriter writer) 
			{
			Initialize(writer);
			}
		#endregion
		
		protected virtual void Initialize(StreamWriter writer)
			{
			_outputWriter = writer;
			}

		protected override void PerformDataBinding(IEnumerable data)
			{
			_dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

			IEnumerable<KeyValuePair<string, object>> keyedData =
					data as IEnumerable<KeyValuePair<string, object>>;
			if (keyedData != null)
				{
				foreach (KeyValuePair<string, object> item in keyedData)
					_dictionary.Add(item.Key, item.Value);
				}
			}

		public override void RenderControl(HtmlTextWriter writer)
			{
			base.RenderControl(writer);
			}

		protected override void Render(HtmlTextWriter writer)
			{
			base.Render(writer);
			}

		#region IDataBoundControl Members
		string[] _dataKeyNames = new string[] { "Key", "Value" };
		public string[] DataKeyNames
			{
			get
				{
				return _dataKeyNames;
				}
			set
				{
				return;
				}
			}

		#endregion

		}

	}
