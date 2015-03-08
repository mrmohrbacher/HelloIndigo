using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using Newtonsoft.Json.Serialization;

namespace LendingLibrary.WebAppMVC
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

			// Include this to supress <name>k_BackingField JSON property names.
			var serializerSettings =
			  GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
			var contractResolver =
			  (DefaultContractResolver)serializerSettings.ContractResolver;
			contractResolver.IgnoreSerializableAttribute = true; 

			GlobalCacheConfig.Load();
			}

		protected void Application_BeginRequest(Object sender, EventArgs e)
			{
			HttpContextBase ctx = new HttpContextWrapper(Context);
			foreach (Route rte in RouteTable.Routes)
				{
				if (rte.GetRouteData(ctx) != null)
					{
					Trace.WriteLine(string.Format("Route: {1} for request: {0}", 
															Context.Request.Url, rte.Url));
					break;
					}

				}
			}
		}
	}
