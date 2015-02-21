using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;
using BookSelection.WebApp;
using LibraryCheckout.WebAppMVC;
using LibraryCheckout.WebAppMVC.HelloIndigoService;


using Library.Model;
namespace LibraryCheckout.WebAppMVC.Controllers
	{
	public class LibraryController : Controller
		{
		[HttpGet]
		public ActionResult Export()
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

				string endpointName = settings["LibraryServiceEndpoint"] as string;
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
					proxy.List(null, 18, out books);
					}

				}
			catch (Exception exp)
				{
				Request.PostError(exp, false);
				}

			Response.ContentType = "text/csv";
			Response.ContentEncoding = System.Text.ASCIIEncoding.ASCII;
			StringBuilder content = new StringBuilder();
			StringWriter writer = new StringWriter(content);
			foreach (var book in books)
				{
				if (content.Length == 0)
					writer.WriteLine(book.ToPropertyNameList("ISBN Author Title Publisher Synopsis"));
				writer.WriteLine(book.ToCSVString("ISBN Author Title Publisher Synopsis"));
				}

			return File(new System.Text.UTF8Encoding().GetBytes(content.ToString()), "text/csv", "Library.csv");
			}

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

				string endpointName = settings["LibraryServiceEndpoint"] as string;
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
