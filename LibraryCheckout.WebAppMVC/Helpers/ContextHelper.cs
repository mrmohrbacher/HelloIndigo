using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace BookSelection.WebApp
	{
	public static class ContextHelper
		{
		static Regex _actionParser = new Regex(@"(?: (action=)(""|')(.\S*)(""|') )",
			  RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

		static private Regex _fieldParser = new Regex(@"\[(?<Lookup>\w+)\]", RegexOptions.IgnoreCase);

		/// <summary>
		/// Post an Exception to the ErrorPage.
		/// </summary>
		public static void PostError(this HttpRequestBase request, Exception exp, bool showStack = false)
			{
			request.RequestContext.HttpContext.PostError(exp, showStack);
			}

		public static void PostError(this HttpContext context, Exception exp, bool showStack = false)
			{
			context.Request.RequestContext.HttpContext.PostError(exp, showStack);
			}

		public static void PostError(this HttpContextBase context, Exception exp, bool showStack = false)
			{
			Trace.WriteLine(exp.ToString());
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("~/ErrorPage.cshtml?ErrorMessage={0}&ErrorData=Web Page: {1}",
									HttpUtility.UrlEncode(exp.Message), context.Request.Url);
			if (showStack)
				{
				sb.AppendFormat("&ShowStack=true");
				sb.AppendFormat("&ErrorStack={0}", HttpUtility.UrlEncode(exp.StackTrace));
				}
			context.Response.StatusCode = 302;
			context.Response.Redirect(sb.ToString(), true);

			/*
			Dictionary<string, string> _errorMap = new Dictionary<string, string>(8)
            {
                {"ErrorMessage", exp.Message},
                {"ErrorStack", exp.StackTrace},
                {"ErrorData", string.Format("Web Page: {0}", context.Request.Url)},
                {"ShowStack", showStack.ToString()}
            };
			Uri responseUri = context.Request.UrlReferrer;
			string responseString = PostRequest(out responseUri,
												  context.Request,
												  "/ErrorPage.cshtml",
												  _errorMap);
			context.Response.ClearHeaders();
			context.Response.RedirectLocation = "/ErrorPage.cshtml";
			context.Response.Output.WriteLine(responseString);
			context.Response.End();
			 * */
			}

		public static string SelectXML(string xmlSource, string tag)
			{
			XDocument xdoc = XDocument.Parse(xmlSource);
			var results = (from node in xdoc.Descendants(tag)
								select node).FirstOrDefault();

			if (results == null)
				return "";

			return results.ToString();
			}

		/// <summary>
		/// Reflect the inbound request's form data to the target of the post.
		/// </summary>
		/// <param name="inboundRequest"></param>
		/// <returns></returns>
		public static string PostRequest(this HttpRequest inboundRequest, out Uri responseUri, string targetPath, IDictionary<string, string> fieldMap = null)
			{
			return PostRequest(inboundRequest.RequestContext.HttpContext.Request, out responseUri, targetPath, fieldMap);
			}

		public static string PostRequest(this HttpRequestBase inboundRequest, out Uri responseUri, string targetPath, IDictionary<string, string> fieldMap = null)
			{
			Debug.WriteLine("Target Path : {0}", targetPath);
			string recvString = null;

#if _POST_REQUEST
			UTF8Encoding encoding = new UTF8Encoding();

         Uri targetUri = new Uri(targetPath, UriKind.RelativeOrAbsolute);

         if (!targetUri.IsAbsoluteUri)
            {
            // Not an absolute Uri, use the original Request's scheme, host and port.
				UriBuilder uriBldr = new UriBuilder(inboundRequest.Url);
            uriBldr.Path = targetPath;
            targetUri = new Uri(uriBldr.ToString());
            }

			HttpWebRequest target = (HttpWebRequest)HttpWebRequest.Create(targetUri);
			target.Method = "POST";

			// Inject AuthenticationCookie into the new request so we do not
			// cause the Forms Authenticator to wake up and bolixup the request.
			HttpCookie cookie = inboundRequest.Cookies["Login"];
			Cookie authentificationCookie = new Cookie(
				"Login",
				cookie.Value,
				cookie.Path
				);
			target.CookieContainer = new CookieContainer();
			// Need to set the Cookie's Domain here; using the Request.Url.Authority
			// caused IIS to ...(Drum roll, please!)..."toss it's cookies!"
			target.CookieContainer.Add(inboundRequest.RequestContext.HttpContext.Request.Url,
												authentificationCookie);

         StringBuilder postDataBldr = new StringBuilder();
         if (fieldMap != null)
            {
            // ----------------------------------------------------
            // Generate the POST to the payment processor; sub-
            // stitutes values from the inbound form request
            // into POST values from the 'fieldMap'.
            foreach (string key in fieldMap.Keys)
               {
               string value = fieldMap[key];
               MatchCollection matches = _fieldParser.Matches(value);
               foreach (Match match in matches)
                  {
                  string lookup = match.Groups["Lookup"].Value;
						value = value.Replace("[" + lookup + "]", inboundRequest.Form[lookup]);
                  }

               if (value.Length > 0)
                  {
                  if (postDataBldr.Length > 0)
                     postDataBldr.Append("&");
                  postDataBldr.AppendFormat("{0}={1}", key, value);
                  }
               }

            }
         else
            {
            // No FieldMapping; copy Form data from inbound request to target
				foreach (string key in inboundRequest.Form.AllKeys)
               {
               if (postDataBldr.Length > 0)
                  postDataBldr.Append("&");
					postDataBldr.AppendFormat("{0}={1}", key, inboundRequest.Form[key]);
               }
            }

         byte[] requestData = encoding.GetBytes(postDataBldr.ToString());

         target.ContentType = "application/x-www-form-urlencoded";
         target.ContentLength = requestData.Length;

         // Stream to contain data portion of HTTP request. For a POST, this will
         // be the form data sent from the inbound request.
         Stream targetStream = target.GetRequestStream();
			
         // Write content to RequestStream
         targetStream.Write(requestData, 0, requestData.Length);

         // Get response from the target's request.
         using (HttpWebResponse response = (HttpWebResponse)target.GetResponse())
            {
            // Display the status.
            Trace.WriteLine(response.StatusDescription);

            // Get the stream containing content returned by the server.
            Stream recvStream = response.GetResponseStream();
            responseUri = response.ResponseUri;
            using (StreamReader reader = new StreamReader(recvStream, encoding))
               {
               recvString = reader.ReadToEnd();
               }
            }
#else
			StringBuilder sb = new StringBuilder();
			TextWriter writer = new StringWriter(sb);
			inboundRequest.RequestContext.HttpContext.Server.Execute(targetPath); //, writer, true);
			responseUri = new Uri(targetPath);
			recvString = sb.ToString();
#endif
			// Remap relative URLs to URL based on originator.
			return _actionParser.Replace(recvString, string.Format(@"$1$2{0}$2", responseUri));
			}

		}
	}