using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineReporting.Framework.Utility;

namespace OnlineReporting.Framework.DataView
   {
	/// <summary>
	/// <para>
	/// A GridView has the sexy capability to accept any IEnumerable as a
	/// data source and to auto-generate an HTML table, with headers, by
	/// reflection over the items within the list!
	/// </para>
	/// <para>We sex-it-up even more by converting the 
	/// column names into space-delimited text, and adding the original
	/// column names as 'class' attributes to each of the data cells
	/// underneath the column headings.
	/// </para>
	/// </summary>
	public class ReportGridControl : GridView, ITextControl
		{
		/// <summary>
		/// An indexed array of CSS Class Names. This array is populated by
		/// the creation of the Header rows. It is referenced by the creation
		/// of the Data rows. The corresponding Column's Class is inserted as
		/// the class attribute's value of each Cell as the Data Row is created.
		/// <see cref="OnRowCreated"/>
		/// <remarks>There is a 'scope' attribute which looks like it should
		/// automatically apply to all cells within the column, but, none of
		/// the HTML renderers' seem to obey it.
		/// </remarks>
		/// </summary>
		private string[] _columnClasses;

		public ReportGridControl()
			{
			this.RowCreated += new GridViewRowEventHandler(OnRowCreated);
			this.DataBound += new EventHandler(ReportGridControl_DataBound);
			}


		/// <summary>
		/// Convert entity references created during the GridView rendering,
		/// into tag-tokens.
		/// </summary>
		/// <param name="sender">GridView</param>
		/// <param name="e">useless</param>
		void ReportGridControl_DataBound(object sender, EventArgs e)
			{
			foreach (GridViewRow row in Rows)
				{
				foreach (TableCell cell in row.Cells)
					{
					cell.Text = cell.Text.Replace("&lt;", "<");
					cell.Text = cell.Text.Replace("&gt;", ">");
					cell.Text = cell.Text.Replace("&#39;", "'");
					cell.Text = cell.Text.Replace("&amp;", "&");
					}
				}
			}

		/// <summary>
		/// Three functions:
		/// <list type="n">
		/// <item>Modify Column Headings from run-on camel-case into space-seperated text.</item>
		/// <item>Inject the original heading text as CSS class attributes into corresponding
		/// data cells.</item>
		/// </list>
		/// </summary>
		/// <param name="sender">this GridView</param>
		/// <param name="gvre">A Reference to the new Row. These are categorzied by 
		/// Header, DataRow, and Footer.</param>
		protected void OnRowCreated(object sender, GridViewRowEventArgs gvre)
			{
			if (gvre.Row.RowType == DataControlRowType.Header)
				{
				_columnClasses = new string[gvre.Row.Cells.Count];
				int index = 0;
				foreach (TableCell cell in gvre.Row.Cells)
					{
					_columnClasses[index] = cell.Text;
					cell.Text = cell.Text.Trim().CamelCaseToDelimitted();
					index++;
					}
				}
			else if (gvre.Row.RowType == DataControlRowType.DataRow)
				{
				// Inject the Column's Style (referenced as a CSS class) 
				// into the Data Cell.
				for (int i = 0; i < gvre.Row.Cells.Count; i++)
					{
					TableCell cell = gvre.Row.Cells[i];
					cell.CssClass = _columnClasses[i];
					}
				}
			}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
			{
			writer.AddAttribute("id", ID);			
			base.Render(writer);
			}

		#region ITextControl Members

		public string Text
			{
			get
				{
				return this.Caption;
				}
			set
				{
				this.Caption = value;
				}
			}

		#endregion
		}
	}
