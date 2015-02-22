<%@ WebHandler Language="C#" Class="CheckoutBook" %>

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;

using Blackriverinc.EmailClient;
using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;
using BookSelection.WebApp;

public class CheckoutBook : IHttpHandler
   {
   public void ProcessRequest(HttpContext context)
      {
      try
         {
         string response = null;

         IDataStoreProvider provider = 
               new HttpRequestDataStoreProvider(context.Request, new WebConfigProvider());
         if (provider == null)
            {
            return;
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
         MemoryStream responseStream  = new MemoryStream(2048);

         TextWriter writer = new StreamWriter(responseStream);

         Action<string, string> emitField = ((name, id) =>
            {
               writer.WriteLine("         <label class='{0}' style='{1}'>", 
                        "oe-label", "display:block;margin-top: 5pt;margin-bottom:0pt");
               writer.Write(name.CamelCaseToDelimitted());
               writer.WriteLine("         </label>");  //                </label>

               writer.WriteLine("         <input type='{0}' name='{1}' id='{2}' style='{3}' value='{4}' readonly />", 
                                       "text", name, id, "width:80%;color:Blue",  data[name]);
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
         emitField("ISBN", "book-isbn");
         emitField("Title", "title");
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
                      context.Request.Form.Get("Email"),
                      "Library Checkout Confirmation",
                      response);

         StringBuilder sb = new StringBuilder();
         sb.AppendFormat("{0} - A confirmation email has been sent.",
               data["CheckoutDate"]);
           
         context.Response.Output.WriteLine(sb.ToString());
         }
      catch (Exception exp)
         {
         Trace.WriteLine(string.Format("Exception CheckoutBook::ProcessRequest\n{0}", exp.Message));
         context.PostError(exp);
         }
      }

   public bool IsReusable
      {
      get
         {
         return false;
         }
      }

   }