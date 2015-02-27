using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace LibraryCheckout.WebAppMVC
	{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
		{
		protected void Application_Start()
			{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			GlobalCacheConfig.Load();
			}

		protected void Application_BeginRequest(Object sender, EventArgs e)
			{
			HttpContextBase ctx = new HttpContextWrapper(Context);
			foreach (Route rte in RouteTable.Routes)
				{
				if (rte.GetRouteData(ctx) != null)
					{
					if (rte.RouteHandler.GetType().Name == "MvcRouteHandler")
						Trace.WriteLine(string.Format("Following: {1} for request: {0}", 
															Context.Request.Url, rte.Url));
					else
						//{System.Web.Routing.StopRoutingHandler}
						Trace.WriteLine(string.Format("Ignore: {1} for request: {0}",
														Context.Request.Url, rte.Url));
					break;
					}

				}
			}
		}
	}
