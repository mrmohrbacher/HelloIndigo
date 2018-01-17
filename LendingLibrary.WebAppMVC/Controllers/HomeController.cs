using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Blackriverinc.Framework.EmailClient;
using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using LendingLibrary.WebAppMVC.ViewModels;
using LendingLibrary.Client.EchoService;
using BookSelection.WebApp;

namespace LendingLibrary.WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Hello()
        {
            string result = "*No Response*";
            string endpointName = null;
            try
            {
                endpointName = GlobalCache.GetResolvedString("EchoServiceEndpoint");
                if (endpointName == null)
                {
                    throw new ApplicationException("Could not find 'EchoServiceEndpoint' in configuration settings.");
                }
                Debug.WriteLine(string.Format("EchoServiceEndpoint='{0}'", endpointName));

                // ---------------------------------------------------------
                // 
                // ---------------------------------------------------------
                using (EchoServiceClient proxy = new EchoServiceClient(endpointName))
                {
                    result = proxy.Ping();
                }

            }
            catch (Exception exp)
            {
                Request.PostError(exp);
            }

            return View("Hello", (object)$"Endpoint: {endpointName ?? "*Unknown*"}, Result: {result}");
        }

        public ActionResult Login(string ReturnUrl)
        {
            return View();
        }

        public ActionResult Error(ErrorModel error)
        {
            return View(error);
        }
    }
}
