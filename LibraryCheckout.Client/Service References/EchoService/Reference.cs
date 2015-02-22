﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryCheckout.Client.EchoService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="uri://blackriverinc.com/helloindigo/EchoService", ConfigurationName="EchoService.IEchoService")]
    public interface IEchoService {
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/Echo", ReplyAction="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/EchoResponse")]
        LibraryCheckout.Client.EchoService.EchoResponse Echo(LibraryCheckout.Client.EchoService.EchoRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/Echo", ReplyAction="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/EchoResponse")]
        System.Threading.Tasks.Task<LibraryCheckout.Client.EchoService.EchoResponse> EchoAsync(LibraryCheckout.Client.EchoService.EchoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/Ping", ReplyAction="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/PingResponse")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="result")]
        string Ping();
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/Ping", ReplyAction="uri://blackriverinc.com/helloindigo/EchoService/IEchoService/PingResponse")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="result")]
        System.Threading.Tasks.Task<string> PingAsync();
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Echo", WrapperNamespace="uri://blackriverinc.com/helloindigo/EchoService", IsWrapped=true)]
    public partial class EchoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/EchoService", Order=0)]
        public string input;
        
        public EchoRequest() {
        }
        
        public EchoRequest(string input) {
            this.input = input;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="EchoResponse", WrapperNamespace="uri://blackriverinc.com/helloindigo/EchoService", IsWrapped=true)]
    public partial class EchoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/EchoService", Order=0)]
        public bool EchoResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/EchoService", Order=1)]
        public string result;
        
        public EchoResponse() {
        }
        
        public EchoResponse(bool EchoResult, string result) {
            this.EchoResult = EchoResult;
            this.result = result;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEchoServiceChannel : LibraryCheckout.Client.EchoService.IEchoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EchoServiceClient : System.ServiceModel.ClientBase<LibraryCheckout.Client.EchoService.IEchoService>, LibraryCheckout.Client.EchoService.IEchoService {
        
        public EchoServiceClient() {
        }
        
        public EchoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EchoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EchoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EchoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.Client.EchoService.EchoResponse LibraryCheckout.Client.EchoService.IEchoService.Echo(LibraryCheckout.Client.EchoService.EchoRequest request) {
            return base.Channel.Echo(request);
        }
        
        public bool Echo(string input, out string result) {
            LibraryCheckout.Client.EchoService.EchoRequest inValue = new LibraryCheckout.Client.EchoService.EchoRequest();
            inValue.input = input;
            LibraryCheckout.Client.EchoService.EchoResponse retVal = ((LibraryCheckout.Client.EchoService.IEchoService)(this)).Echo(inValue);
            result = retVal.result;
            return retVal.EchoResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.Client.EchoService.EchoResponse> EchoAsync(LibraryCheckout.Client.EchoService.EchoRequest request) {
            return base.Channel.EchoAsync(request);
        }
        
        public string Ping() {
            return base.Channel.Ping();
        }
        
        public System.Threading.Tasks.Task<string> PingAsync() {
            return base.Channel.PingAsync();
        }
    }
}
