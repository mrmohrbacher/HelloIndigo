using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;
using System.Text;

namespace HelloIndigo
   {
   [ServiceContract(Namespace = "uri://blackriversystemsinc.com/helloindgo.com")]
   public interface IHelloIndigoService
      {
      [OperationContract]
      bool Echo(out string result, string key);
      }
   }
