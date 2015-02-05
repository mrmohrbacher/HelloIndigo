using System.ServiceModel;

namespace HelloIndigo
   {
   [ServiceContract(Namespace = "uri://blackriverinc.com/helloindigo/EchoService",
						 SessionMode=SessionMode.Allowed)]
   public interface IEchoService
      {
      [OperationContract]
      bool Echo(out string result, string input);

      [OperationContract]
      void Ping(out string result);
      }
   }
