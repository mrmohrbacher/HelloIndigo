﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryCheckout.WebAppMVC.HelloIndigoService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", ConfigurationName="HelloIndigoService.ILibraryService")]
    public interface ILibraryService {
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/List", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ListResponse")]
        LibraryCheckout.WebAppMVC.HelloIndigoService.ListResponse List(LibraryCheckout.WebAppMVC.HelloIndigoService.ListRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/List", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ListResponse")]
        System.Threading.Tasks.Task<LibraryCheckout.WebAppMVC.HelloIndigoService.ListResponse> ListAsync(LibraryCheckout.WebAppMVC.HelloIndigoService.ListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Read", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ReadResponse")]
        LibraryCheckout.WebAppMVC.HelloIndigoService.ReadResponse Read(LibraryCheckout.WebAppMVC.HelloIndigoService.ReadRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Read", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ReadResponse")]
        System.Threading.Tasks.Task<LibraryCheckout.WebAppMVC.HelloIndigoService.ReadResponse> ReadAsync(LibraryCheckout.WebAppMVC.HelloIndigoService.ReadRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Update", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/UpdateResponse" +
            "")]
        LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateResponse Update(LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Update", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/UpdateResponse" +
            "")]
        System.Threading.Tasks.Task<LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateResponse> UpdateAsync(LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Add", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/AddResponse")]
        bool Add(Library.Model.Book book);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Add", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/AddResponse")]
        System.Threading.Tasks.Task<bool> AddAsync(Library.Model.Book book);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Delete", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/DeleteResponse" +
            "")]
        bool Delete(string key, System.DateTime timeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Delete", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/DeleteResponse" +
            "")]
        System.Threading.Tasks.Task<bool> DeleteAsync(string key, System.DateTime timeStamp);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Load", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/LoadResponse")]
        bool Load(System.IO.Stream input);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Load", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/LoadResponse")]
        System.Threading.Tasks.Task<bool> LoadAsync(System.IO.Stream input);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="List", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class ListRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public string searchKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public int qParm;
        
        public ListRequest() {
        }
        
        public ListRequest(string searchKey, int qParm) {
            this.searchKey = searchKey;
            this.qParm = qParm;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ListResponse", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class ListResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public bool ListResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public Library.Model.Book[] books;
        
        public ListResponse() {
        }
        
        public ListResponse(bool ListResult, Library.Model.Book[] books) {
            this.ListResult = ListResult;
            this.books = books;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Read", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class ReadRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public string key;
        
        public ReadRequest() {
        }
        
        public ReadRequest(string key) {
            this.key = key;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ReadResponse", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class ReadResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public bool ReadResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public Library.Model.Book book;
        
        public ReadResponse() {
        }
        
        public ReadResponse(bool ReadResult, Library.Model.Book book) {
            this.ReadResult = ReadResult;
            this.book = book;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Update", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class UpdateRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public Library.Model.Book book;
        
        public UpdateRequest() {
        }
        
        public UpdateRequest(Library.Model.Book book) {
            this.book = book;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UpdateResponse", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class UpdateResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public bool UpdateResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public Library.Model.Book book;
        
        public UpdateResponse() {
        }
        
        public UpdateResponse(bool UpdateResult, Library.Model.Book book) {
            this.UpdateResult = UpdateResult;
            this.book = book;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILibraryServiceChannel : LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LibraryServiceClient : System.ServiceModel.ClientBase<LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService>, LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService {
        
        public LibraryServiceClient() {
        }
        
        public LibraryServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LibraryServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LibraryServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LibraryServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.WebAppMVC.HelloIndigoService.ListResponse LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService.List(LibraryCheckout.WebAppMVC.HelloIndigoService.ListRequest request) {
            return base.Channel.List(request);
        }
        
        public bool List(string searchKey, int qParm, out Library.Model.Book[] books) {
            LibraryCheckout.WebAppMVC.HelloIndigoService.ListRequest inValue = new LibraryCheckout.WebAppMVC.HelloIndigoService.ListRequest();
            inValue.searchKey = searchKey;
            inValue.qParm = qParm;
            LibraryCheckout.WebAppMVC.HelloIndigoService.ListResponse retVal = ((LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService)(this)).List(inValue);
            books = retVal.books;
            return retVal.ListResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.WebAppMVC.HelloIndigoService.ListResponse> ListAsync(LibraryCheckout.WebAppMVC.HelloIndigoService.ListRequest request) {
            return base.Channel.ListAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.WebAppMVC.HelloIndigoService.ReadResponse LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService.Read(LibraryCheckout.WebAppMVC.HelloIndigoService.ReadRequest request) {
            return base.Channel.Read(request);
        }
        
        public bool Read(string key, out Library.Model.Book book) {
            LibraryCheckout.WebAppMVC.HelloIndigoService.ReadRequest inValue = new LibraryCheckout.WebAppMVC.HelloIndigoService.ReadRequest();
            inValue.key = key;
            LibraryCheckout.WebAppMVC.HelloIndigoService.ReadResponse retVal = ((LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService)(this)).Read(inValue);
            book = retVal.book;
            return retVal.ReadResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.WebAppMVC.HelloIndigoService.ReadResponse> ReadAsync(LibraryCheckout.WebAppMVC.HelloIndigoService.ReadRequest request) {
            return base.Channel.ReadAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateResponse LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService.Update(LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateRequest request) {
            return base.Channel.Update(request);
        }
        
        public bool Update(ref Library.Model.Book book) {
            LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateRequest inValue = new LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateRequest();
            inValue.book = book;
            LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateResponse retVal = ((LibraryCheckout.WebAppMVC.HelloIndigoService.ILibraryService)(this)).Update(inValue);
            book = retVal.book;
            return retVal.UpdateResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateResponse> UpdateAsync(LibraryCheckout.WebAppMVC.HelloIndigoService.UpdateRequest request) {
            return base.Channel.UpdateAsync(request);
        }
        
        public bool Add(Library.Model.Book book) {
            return base.Channel.Add(book);
        }
        
        public System.Threading.Tasks.Task<bool> AddAsync(Library.Model.Book book) {
            return base.Channel.AddAsync(book);
        }
        
        public bool Delete(string key, System.DateTime timeStamp) {
            return base.Channel.Delete(key, timeStamp);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteAsync(string key, System.DateTime timeStamp) {
            return base.Channel.DeleteAsync(key, timeStamp);
        }
        
        public bool Load(System.IO.Stream input) {
            return base.Channel.Load(input);
        }
        
        public System.Threading.Tasks.Task<bool> LoadAsync(System.IO.Stream input) {
            return base.Channel.LoadAsync(input);
        }
    }
}
