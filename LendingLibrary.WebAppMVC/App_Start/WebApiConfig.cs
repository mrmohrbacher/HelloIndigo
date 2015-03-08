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
				 routeTemplate: "api/Subscribers/{email}",
				 defaults: new { controller = "Subscribers" }

			);

			config.Routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{email}",
				 defaults: new { email = RouteParameter.Optional }

			);
			}
		}
	}
