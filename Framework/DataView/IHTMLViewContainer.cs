using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRPlus.ScheduledJobs.DataView
	{
	public interface IHTMLViewContainer 
		{
		ICollection<IHTMLReportView> Views { get; }
		}

	public class ViewContainerControl : Control, IDataBoundControl, IHTMLViewContainer
		{
		private void initialize()
			{
			_views = new List<IHTMLReportView>();
			}

		public ViewContainerControl()
			{
			initialize();
			}

		public override void RenderControl(HtmlTextWriter writer)
			{	
			
			foreach (IHTMLReportView view in Views)
				{
				view.Render();
				}
			}



		#region IHTMLViewContainer Members

		ICollection<IHTMLReportView> _views;

		public ICollection<IHTMLReportView> Views
			{
			get
				{
				return _views;
				}
			}

		#endregion
		}
	}
