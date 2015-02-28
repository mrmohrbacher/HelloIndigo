using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Library.Model;

namespace HelloIndigo
	{
	[ServiceContract(Namespace = "uri://blackriverinc.com/helloindigo/SubscriberService")]
	public interface ISubscriber
		{
		[OperationContract]
		bool List(string searchEmail, out Subscriber[] subscribers);

		[OperationContract]
		bool Read(string email, out Subscriber subscriber);

		[OperationContract]
		bool Update(ref Subscriber subscriber);

		[OperationContract]
		bool Add(Subscriber subscriber);

		[OperationContract]
		bool Delete(string email, DateTime timeStamp);

		[OperationContract]
		bool Load(Stream input);
		}
	}
