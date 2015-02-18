using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;
using BookSelection.WebApp;
using LibraryCheckout.WebAppMVC;
using LibraryCheckout.WebAppMVC.HelloIndigoService;

namespace LibraryCheckout.WebAppMVC.Controllers
	{
	public class LibraryController : Controller
		{
		//
		// GET: /Library/
		[HttpGet]
		public ActionResult BookSelection()
			{
			Book[] books = null;
			try
				{
				IDataStoreProvider provider = WebConfigProvider.Open();
				if (provider == null)
					{
					throw new ApplicationException("Could not open Web Configuration Settings.");
					}
				provider = new CloudSettingsProvider(provider);
				IKeyedDataStore settings = new KeyedDataStore(provider);

				string endpointName = settings["LibraryServiceEndpoint"];
				if (endpointName == null)
					{
					throw new ApplicationException("Could not find 'LibraryServiceEndpoint' in configuration settings.");
					}
				Debug.WriteLine(string.Format("LibraryServiceEndpoint='{0}'", endpointName));

				// ---------------------------------------------------------
				// 
				// ---------------------------------------------------------
				using (LibraryServiceClient proxy = new LibraryServiceClient(endpointName))
					{
					proxy.List(null, out books);
					}

				}
			catch (Exception exp)
				{
				Request.PostError(exp, false);
				}

			return View(new List<Book>(books));
			}

		}
	}
