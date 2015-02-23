using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading;
using System.Text;

#if _AZURE
using Microsoft.WindowsAzure.ServiceRuntime;
#endif

using Blackriverinc.Framework.DataStore;
using Blackriverinc.Framework.Utility;

using Library.Model;
using Library.Model.Helpers;

namespace HelloIndigo
	{
	[ServiceBehavior(IncludeExceptionDetailInFaults = true,
						  Name = "HelloIndigo.SubscriberService",
						  InstanceContextMode = InstanceContextMode.PerCall)]
	public class SubscriberService : ISubscriber
		{
		static AppTraceListener tracer = null;
		static int instanceCount = 0;

		static SubscriberService()
			{
			try
				{
				IDataStoreProvider provider = ConfigProviderBase.Open();
				GlobalCache.LoadConfigurationSettings(provider, true);

				string logPath = (GlobalCache.GetResolvedString("LogPath") ?? @"C:\Logs\HelloIndigo");

#if _AZURE
				// Running in the Cloud; look for the local drive
				if (!Path.IsPathRooted(logPath)
				&& RoleEnvironment.IsAvailable
				&& !RoleEnvironment.IsEmulated)
					{
					LocalResource localResource = RoleEnvironment.GetLocalResource("LogFiles");
					logPath = Path.Combine(localResource.RootPath, logPath);
					}
#endif
				tracer = new AppTraceListener(logPath);

				// Resolve streams out of resource fork.
				StreamFactory.Register("res", (path, args) =>
					{
						Assembly assembly = Assembly.GetExecutingAssembly();
						string resourcePath = string.Format("{0}.{1}",
								assembly.GetName().Name,
								path);
						Trace.WriteLine(string.Format("StreamFactory::Create(res://{0})", resourcePath));
						Stream result = assembly.GetManifestResourceStream(resourcePath);
						return result;
					});
				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				}
			}


		public SubscriberService()
			{
			try
				{
				if (Interlocked.Add(ref instanceCount, 1) == 1)
					{
					Trace.WriteLine(string.Format("+ SubscriberService[{0}]+", instanceCount));

					// Populate the Books Table
					using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("Environment")))
						{
						//if (context.Books.Count() == 0)
						//	{
						//	Stream xstream = StreamFactory.Create(@"res://AppData.Books.xml");
						//	this.Load(xstream);
						//	}
						}
					}
				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				}
			}

		~SubscriberService()
			{
			Interlocked.Add(ref instanceCount, -1);
			Trace.WriteLine(string.Format("- SubscriberService[{0}]-", instanceCount));
			if (Interlocked.CompareExchange(ref instanceCount, 0, 0) == 0)
				tracer.Close();
			}


		public bool List(string searchEmail, out Subscriber[] subscribers)
			{
			throw new NotImplementedException();
			}

		public bool Read(string email, out Subscriber subscriber)
			{
			Trace.WriteLine(string.Format("Read key='{0}'", email));
			if (email == null || email.Trim().Length == 0)
				{
				subscriber = null;
				return false;
				}

			using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("Environment")))
				{
				subscriber = new Subscriber((from s in context.Subscribers
													  where s.Email == email
													  select s).FirstOrDefault());


				}
			return (subscriber != null && subscriber.Email != null);
			}

		public bool Update(ref Subscriber subscriber)
			{
			Trace.WriteLine(subscriber.ToObjectString());
			if (subscriber == null
			|| string.IsNullOrEmpty(subscriber.Email))
				{
				return false;
				}

			using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("Environment")))
				{
				string email = subscriber.Email.ToLower();
				var currentSubscriber = context.Subscribers
												.FirstOrDefault(s => s.Email.ToLower() == email);
				if (currentSubscriber == null)
					return false;

				currentSubscriber.Email = subscriber.Email;
				currentSubscriber.Name = subscriber.Name;
				currentSubscriber.Address = subscriber.Address;
				currentSubscriber.City = subscriber.City;
				currentSubscriber.State = subscriber.State;
				currentSubscriber.PostalCode = subscriber.PostalCode;

				return (context.SaveChanges() > 0);
				}
			}

		public bool Add(Subscriber subscriber)
			{
			Trace.WriteLine(subscriber.ToObjectString());
			if (subscriber == null
			|| string.IsNullOrEmpty(subscriber.Email))
				{
				return false;
				}

			using (LibraryEntities context = new LibraryEntities("name=" + GlobalCache.GetResolvedString("Environment")))
				{
				if (context.Subscribers
						.Any(s => s.Email.ToLower() == subscriber.Email.ToLower()))
					return false;

				context.Subscribers.Add(subscriber);

				return (context.SaveChanges() > 0);
				}
			}

		public bool Delete(string email, DateTime timeStamp)
			{
			throw new NotImplementedException();
			}

		public bool Load(System.IO.Stream input)
			{
			throw new NotImplementedException();
			}
		}
	}
