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
	public class HTMLDictionaryTableControl : DictionaryBoundControl
		{
		protected override void PerformDataBinding(IEnumerable data)
			{
			Trace.WriteLineIf(TraceFlag, string.Format("+{0}::PerformDataBinding", this));

			base.PerformDataBinding(data);

			Table table = new Table();
			TableRow row = new TableRow();

			table.Rows.Add(row);
			TableHeaderCell hdrCell;
			hdrCell = new TableHeaderCell();
			hdrCell.Text = "Key";
			row.Cells.Add(hdrCell);
			hdrCell = new TableHeaderCell();
			hdrCell.Text = "Value";
			row.Cells.Add(hdrCell);

			TableCell cell;
			foreach (var item in _dictionary)
				{
				row = new TableRow();
				table.Rows.Add(row);

				cell = new TableCell();
				cell.Text = item.Key;
				row.Cells.Add(cell);
				cell = new TableCell();
				cell.Text = (item.Value != null)?item.Value.ToString():"";
				row.Cells.Add(cell);
				}

			this.Controls.Add(table);

			Trace.WriteLineIf(TraceFlag, string.Format("-{0}::PerformDataBinding", this));
			}

		public override void RenderControl(HtmlTextWriter writer)
			{
			base.RenderControl(writer);
			}

		protected override void Render(HtmlTextWriter writer)
			{
			base.Render(writer);
			}
		}
	}
