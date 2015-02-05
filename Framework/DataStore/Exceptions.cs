using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackriverinc.Framework.DataStore
	{
	public class DataStoreException : ApplicationException
		{
		}


	public class StreamFactoryRegistrationException : ApplicationException
		{
		public StreamFactoryRegistrationException(string scheme) 
			: base(scheme)
			{
			}

		public override string Message
			{
			get
				{
				return string.Format("Scheme ({0}) not registered with StreamFactory.", 
							base.Message);
				}
			}
		}
	}
