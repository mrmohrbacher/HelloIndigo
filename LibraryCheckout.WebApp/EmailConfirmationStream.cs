using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LibraryCheckout.WebApp
	{
	public class EmailConfirmationStream : Stream
		{
		Stream _responseStream;

		public EmailConfirmationStream(Stream responseStream)
			{
			_responseStream = responseStream;
			}

		public override void Write(byte[] buffer, int offset, int count)
			{
			
			}

	
		}
	}