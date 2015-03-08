using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LendingLibrary.WebAppMVC
	{
	public static class WebApiConfig
		{
		public static void Register(HttpConfiguration config)
			{
			config.Routes.MapHttpRoute(
				 name: "SubscriberApi",
				 routeTemplate: "api/Subscriber/{email}",
				 defaults: new { controller = "Subscriber" }

			);

			config.Routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{email}",
				 defaults: new { email = RouteParameter.Optional }

			);
			}
		}
	}
