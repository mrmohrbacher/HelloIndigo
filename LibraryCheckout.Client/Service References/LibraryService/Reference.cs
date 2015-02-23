﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryCheckout.Client.LibraryService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", ConfigurationName="LibraryService.ILibraryService")]
    public interface ILibraryService {
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/List", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ListResponse")]
        LibraryCheckout.Client.LibraryService.ListResponse List(LibraryCheckout.Client.LibraryService.ListRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/List", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ListResponse")]
        System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.ListResponse> ListAsync(LibraryCheckout.Client.LibraryService.ListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Read", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ReadResponse")]
        LibraryCheckout.Client.LibraryService.ReadResponse Read(LibraryCheckout.Client.LibraryService.ReadRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Read", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/ReadResponse")]
        System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.ReadResponse> ReadAsync(LibraryCheckout.Client.LibraryService.ReadRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Update", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/UpdateResponse" +
            "")]
        LibraryCheckout.Client.LibraryService.UpdateResponse Update(LibraryCheckout.Client.LibraryService.UpdateRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Update", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/UpdateResponse" +
            "")]
        System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.UpdateResponse> UpdateAsync(LibraryCheckout.Client.LibraryService.UpdateRequest request);
        
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
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Checkin", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/CheckinRespons" +
            "e")]
        LibraryCheckout.Client.LibraryService.CheckinResponse Checkin(LibraryCheckout.Client.LibraryService.CheckinRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Checkin", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/CheckinRespons" +
            "e")]
        System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.CheckinResponse> CheckinAsync(LibraryCheckout.Client.LibraryService.CheckinRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Checkout", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/CheckoutRespon" +
            "se")]
        LibraryCheckout.Client.LibraryService.CheckoutResponse Checkout(LibraryCheckout.Client.LibraryService.CheckoutRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/Checkout", ReplyAction="uri://blackriverinc.com/helloindigo/LibraryService/ILibraryService/CheckoutRespon" +
            "se")]
        System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.CheckoutResponse> CheckoutAsync(LibraryCheckout.Client.LibraryService.CheckoutRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="List", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class ListRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public string searchKey;
        
        public ListRequest() {
        }
        
        public ListRequest(string searchKey) {
            this.searchKey = searchKey;
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Checkin", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class CheckinRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public string isbn;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public System.DateTime checkedout;
        
        public CheckinRequest() {
        }
        
        public CheckinRequest(string isbn, System.DateTime checkedout) {
            this.isbn = isbn;
            this.checkedout = checkedout;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CheckinResponse", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class CheckinResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public bool CheckinResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public System.Nullable<System.DateTime> checkedin;
        
        public CheckinResponse() {
        }
        
        public CheckinResponse(bool CheckinResult, System.Nullable<System.DateTime> checkedin) {
            this.CheckinResult = CheckinResult;
            this.checkedin = checkedin;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Checkout", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class CheckoutRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public Library.Model.BookCheckout checkout;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public bool updateSubscriber;
        
        public CheckoutRequest() {
        }
        
        public CheckoutRequest(Library.Model.BookCheckout checkout, bool updateSubscriber) {
            this.checkout = checkout;
            this.updateSubscriber = updateSubscriber;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CheckoutResponse", WrapperNamespace="uri://blackriverinc.com/helloindigo/LibraryService", IsWrapped=true)]
    public partial class CheckoutResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=0)]
        public bool CheckoutResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="uri://blackriverinc.com/helloindigo/LibraryService", Order=1)]
        public System.Nullable<System.DateTime> checkedout;
        
        public CheckoutResponse() {
        }
        
        public CheckoutResponse(bool CheckoutResult, System.Nullable<System.DateTime> checkedout) {
            this.CheckoutResult = CheckoutResult;
            this.checkedout = checkedout;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILibraryServiceChannel : LibraryCheckout.Client.LibraryService.ILibraryService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LibraryServiceClient : System.ServiceModel.ClientBase<LibraryCheckout.Client.LibraryService.ILibraryService>, LibraryCheckout.Client.LibraryService.ILibraryService {
        
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
        LibraryCheckout.Client.LibraryService.ListResponse LibraryCheckout.Client.LibraryService.ILibraryService.List(LibraryCheckout.Client.LibraryService.ListRequest request) {
            return base.Channel.List(request);
        }
        
        public bool List(string searchKey, out Library.Model.Book[] books) {
            LibraryCheckout.Client.LibraryService.ListRequest inValue = new LibraryCheckout.Client.LibraryService.ListRequest();
            inValue.searchKey = searchKey;
            LibraryCheckout.Client.LibraryService.ListResponse retVal = ((LibraryCheckout.Client.LibraryService.ILibraryService)(this)).List(inValue);
            books = retVal.books;
            return retVal.ListResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.ListResponse> ListAsync(LibraryCheckout.Client.LibraryService.ListRequest request) {
            return base.Channel.ListAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.Client.LibraryService.ReadResponse LibraryCheckout.Client.LibraryService.ILibraryService.Read(LibraryCheckout.Client.LibraryService.ReadRequest request) {
            return base.Channel.Read(request);
        }
        
        public bool Read(string key, out Library.Model.Book book) {
            LibraryCheckout.Client.LibraryService.ReadRequest inValue = new LibraryCheckout.Client.LibraryService.ReadRequest();
            inValue.key = key;
            LibraryCheckout.Client.LibraryService.ReadResponse retVal = ((LibraryCheckout.Client.LibraryService.ILibraryService)(this)).Read(inValue);
            book = retVal.book;
            return retVal.ReadResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.ReadResponse> ReadAsync(LibraryCheckout.Client.LibraryService.ReadRequest request) {
            return base.Channel.ReadAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.Client.LibraryService.UpdateResponse LibraryCheckout.Client.LibraryService.ILibraryService.Update(LibraryCheckout.Client.LibraryService.UpdateRequest request) {
            return base.Channel.Update(request);
        }
        
        public bool Update(ref Library.Model.Book book) {
            LibraryCheckout.Client.LibraryService.UpdateRequest inValue = new LibraryCheckout.Client.LibraryService.UpdateRequest();
            inValue.book = book;
            LibraryCheckout.Client.LibraryService.UpdateResponse retVal = ((LibraryCheckout.Client.LibraryService.ILibraryService)(this)).Update(inValue);
            book = retVal.book;
            return retVal.UpdateResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.UpdateResponse> UpdateAsync(LibraryCheckout.Client.LibraryService.UpdateRequest request) {
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
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.Client.LibraryService.CheckinResponse LibraryCheckout.Client.LibraryService.ILibraryService.Checkin(LibraryCheckout.Client.LibraryService.CheckinRequest request) {
            return base.Channel.Checkin(request);
        }
        
        public bool Checkin(string isbn, System.DateTime checkedout, out System.Nullable<System.DateTime> checkedin) {
            LibraryCheckout.Client.LibraryService.CheckinRequest inValue = new LibraryCheckout.Client.LibraryService.CheckinRequest();
            inValue.isbn = isbn;
            inValue.checkedout = checkedout;
            LibraryCheckout.Client.LibraryService.CheckinResponse retVal = ((LibraryCheckout.Client.LibraryService.ILibraryService)(this)).Checkin(inValue);
            checkedin = retVal.checkedin;
            return retVal.CheckinResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.CheckinResponse> CheckinAsync(LibraryCheckout.Client.LibraryService.CheckinRequest request) {
            return base.Channel.CheckinAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LibraryCheckout.Client.LibraryService.CheckoutResponse LibraryCheckout.Client.LibraryService.ILibraryService.Checkout(LibraryCheckout.Client.LibraryService.CheckoutRequest request) {
            return base.Channel.Checkout(request);
        }
        
        public bool Checkout(Library.Model.BookCheckout checkout1, bool updateSubscriber, out System.Nullable<System.DateTime> checkedout) {
            LibraryCheckout.Client.LibraryService.CheckoutRequest inValue = new LibraryCheckout.Client.LibraryService.CheckoutRequest();
            inValue.checkout = checkout1;
            inValue.updateSubscriber = updateSubscriber;
            LibraryCheckout.Client.LibraryService.CheckoutResponse retVal = ((LibraryCheckout.Client.LibraryService.ILibraryService)(this)).Checkout(inValue);
            checkedout = retVal.checkedout;
            return retVal.CheckoutResult;
        }
        
        public System.Threading.Tasks.Task<LibraryCheckout.Client.LibraryService.CheckoutResponse> CheckoutAsync(LibraryCheckout.Client.LibraryService.CheckoutRequest request) {
            return base.Channel.CheckoutAsync(request);
        }
    }
}