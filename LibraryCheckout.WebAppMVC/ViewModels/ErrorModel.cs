using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingLibrary.WebAppMVC.ViewModels
	{
	public class ErrorModel
		{
		public string ErrorMessage { get; set; }
		public string OriginalRequestUrl { get; set; }
		public bool ShowStack { get; set; }
		public string ErrorStack { get; set; }
		}
	}