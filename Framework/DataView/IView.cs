using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRPlus.ScheduledJobs.DataView
   {
   public interface IView : ITextControl
      {
      Stream Render(IEnumerable dataSource);
		}

   }
