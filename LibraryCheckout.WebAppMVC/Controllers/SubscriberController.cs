using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using LibraryCheckout.Client.SubscriberService;
using Library.Model;

namespace LibraryCheckout.WebAppMVC.Controllers
	{
	public class SubscriberController : Controller
		{
		[HttpGet]
		public ActionResult Read(string email)
			{
			string endpointName = GlobalCache.GetResolvedString("SubscriberServiceEndpoint");
			if (endpointName == null)
				{
				throw new ApplicationException("Could not find 'SubscriberServiceEndpoint' in configuration settings.");
				}
			Debug.WriteLine(string.Format("SubscriberServiceEndpoint='{0}'", endpointName));

			Subscriber subscriber = null;
			// ---------------------------------------------------------
			// 
			// ---------------------------------------------------------
			using (SubscriberClient proxy = new SubscriberClient(endpointName))
				{
				if (proxy.Read(email, out subscriber))
					{
					return Json(subscriber, JsonRequestBehavior.AllowGet);
					}
				}

			Response.StatusCode = 404;
			return Content(string.Format("Email {0} Not Found", email));
			}
		}
	}