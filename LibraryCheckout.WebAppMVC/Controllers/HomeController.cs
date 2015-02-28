using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LibraryCheckout.WebAppMVC.ViewModels;

namespace LibraryCheckout.WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
		 public ActionResult Hello()
			 {
			 return View();
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
