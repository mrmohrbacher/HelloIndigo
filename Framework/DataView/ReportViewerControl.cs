using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using Microsoft.Reporting.Common;
using Microsoft.Reporting.WebForms;

namespace OnlineReporting.Framework.DataView
   {
	public class ReportViewerControl : ReportViewer, IDataBoundControl
		{

		private ReportDataSource _dataSource;

		#region IDataBoundControl Members

		public string[] DataKeyNames
			{
			get
				{
				return null;
				}
			set
				{
				throw new NotImplementedException();
				}
			}

		public string DataMember
			{ get; set; }

		public object DataSource
			{
			get
				{
				if (DataSourceID != null)
					return LocalReport.DataSources[DataSourceID];

				return _dataSource;
				}
			set
				{
				_dataSource = new ReportDataSource(DataSourceID, value);
				LocalReport.DataSources.Add(_dataSource);
				}
			}

		/// <summary>
		/// Identifies DataSource in LocalReport.DataSources
		/// </summary>
		public string DataSourceID
			{
			get
				{
				if (LocalReport.GetDataSourceNames().Count > 0)
					return LocalReport.GetDataSourceNames()[0];
				return null;
				}
			set
				{
				throw new NotImplementedException();
				}
			}

		public System.Web.UI.IDataSource DataSourceObject
			{
			get { return null; }
			}

		#endregion
		}
	}
