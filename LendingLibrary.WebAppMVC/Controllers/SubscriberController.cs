using System;
using System.Diagnostics;
using System.Net;
using System.Web.Http;

using Blackriverinc.Framework.DataStore;
using LendingLibrary.Client.SubscriberService;
using Library.Model;

namespace LendingLibrary.WebAppMVC.Controllers
	{
	public class SubscriberController : ApiController
		{
		public Subscriber Get(string email)
			{
			string endpointName = GlobalCache.GetResolvedString("SubscriberServiceEndpoint");
			if (endpointName == null)
				{
				throw new ApplicationException("Could not find 'SubscriberServiceEndpoint' in configuration settings.");
				}
			Debug.WriteLine(string.Format("SubscriberServiceEndpoint='{0}'", endpointName));

			Subscriber subscriber = null;
			// ---------------------------------------------------------
			// 
			// ---------------------------------------------------------
			using (SubscriberClient proxy = new SubscriberClient(endpointName))
				{
				if (!proxy.Read(email, out subscriber))
					throw new HttpResponseException(HttpStatusCode.NotFound);
				}

			return subscriber;
			}
		}
	}