using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Blackriverinc.Framework.DataStore;
using Blackriverinc.EmailClient;
using Blackriverinc.Framework.Utility;
using BookSelection.WebApp;
using LibraryCheckout.WebAppMVC;
using LibraryCheckout.WebAppMVC.HelloIndigoService;


using Library.Model;
namespace LibraryCheckout.WebAppMVC.Controllers
<<<<<<< HEAD
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
               proxy.List(null, out books);
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
      public ActionResult Index()
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

      [HttpPost]
      public ActionResult Checkout(Library.Model.BookCheckout Selection)
         {
         StringBuilder sb = new StringBuilder();
         try
            {

            string response = null;

            IDataStoreProvider provider =
                  new HttpRequestDataStoreProvider(Request, new WebConfigProvider());
            if (provider == null)
               {
               return null;
               }
            IKeyedDataStore data = new KeyedDataStore(provider);
            data.Add("CheckoutDate", DateTime.Now.ToString("MMM dd, yyyy HH:mm"));
#if _WTF         
         // Create Confirmation from Template.
         Uri responseURI;
         response = BookSelection.WebApp.Helper.PostRequest(
                                                          out responseURI,
                                                          context.Request,
                                                          "CheckoutEmailTemplate.cshtml?SendEmail=false");
#else
            MemoryStream responseStream = new MemoryStream(2048);

            TextWriter writer = new StreamWriter(responseStream);

            Action<string, string> emitField = ((name, id) =>
            {
               writer.WriteLine("         <label class='{0}' style='{1}'>",
                        "oe-label", "display:block;margin-top: 5pt;margin-bottom:0pt");
               writer.Write(name.CamelCaseToDelimitted());
               writer.WriteLine("         </label>");  //                </label>

               writer.WriteLine("         <input type='{0}' name='{1}' id='{2}' style='{3}' value='{4}' readonly />",
                                       "text", name, id, "width:80%;color:Blue", data[name]);
            });

            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.Write("<title>");
            writer.Write("Black River Systems, Inc - Library Checkout");
            writer.Write("</title>");
            writer.WriteLine("</head>");

            writer.WriteLine("   <body>");
            writer.WriteLine("      <div id='oe-mail'>");

            writer.WriteLine("         <h2>{0}</h2>", "Checkout Confirmation");

            writer.WriteLine("         <fieldset style='{0}'>",
                             "margin-left: 27px; margin-right: 27px;");

            emitField("SubscriberName", "name");
            emitField("BookISBN", "book-isbn");
            emitField("BookTitle", "book-title");
            emitField("CheckoutDate", "checkout-date");

            writer.WriteLine("        </fieldset>");
            writer.WriteLine("      </div>");
            writer.WriteLine("   </body>");
            writer.WriteLine("</html>");

            writer.Flush();

            response = responseStream.ContentsToString();
#endif
            // Send confirmation Email
            string emailConnection = data["EMailConnectionMock"] as string;
            Encryption encryptor = new Encryption();
            IEmailClient client = EmailClientFactory.Create(encryptor.Decrypt(emailConnection));
            client.Send("mike@blackriverinc.com",
                         Request.Form.Get("Email"),
                         "Library Checkout Confirmation",
                         response);


            sb.AppendFormat("{0} - A confirmation email has been sent.",
                  data["CheckoutDate"]);
            }
         catch (Exception exp)
            {
            Trace.WriteLine(string.Format("Exception CheckoutBook::ProcessRequest\n{0}", exp.Message));
            Request.PostError(exp);
            }

         Response.ContentType = "text/text";
         return Content(sb.ToString());
         }

      }
   }
=======
	{
	public class LibraryController : Controller
		{
		[HttpGet]
		public ActionResult Export()
			{
			Book[] books = null;
			try
				{
				string endpointName = GlobalCache.GetResolvedString("LibraryServiceEndpoint");
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
				string endpointName = GlobalCache.GetResolvedString("LibraryServiceEndpoint");
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
>>>>>>> 3fe7d53947f9cfdf089e6066e6c8399fccce1f60
