using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LibraryCheckout.WebAppMVC
	{
	public class RouteConfig
		{
		public static void RegisterRoutes(RouteCollection routes)
			{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "ErrorRoute",
				url: "Home/Error/{ErrorMessage}",
				defaults: new { controller = "Home", action = "Error", ErrorMessage = UrlParameter.Optional }
			 );

			routes.MapRoute(
				name: "HomeRoutes",
				url: "Home/{action}/{id}",
				defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
			 );


			routes.MapRoute(
				name: "SubscriberRoutes",
				url: "Subscriber/{action}/{id}",
			   defaults: new { controller = "Subscriber", action = "Index", id = UrlParameter.Optional }
			 );

			routes.MapRoute(
				 name: "Default",
				 url: "{controller}/{action}/{id}",
				 defaults: new { controller = "Library", action = "Index", id = UrlParameter.Optional }
			);

			}
		}
	}