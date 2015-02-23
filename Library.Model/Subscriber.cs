//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library.Model
{
    using System;
    using System.Collections.Generic;
    
    
    [Serializable]
    public partial class Subscriber
    {
        public Subscriber()
        {
            this.BookCheckouts = new HashSet<BookCheckout>();
        }
    
        public Subscriber(Subscriber source)
        {
            if (source != null)
            {
                this.Email = source.Email;
                this.Name = source.Name;
                this.Address = source.Address;
                this.City = source.City;
                this.State = source.State;
                this.PostalCode = source.PostalCode;
            }
        }
    
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    
        public virtual ICollection<BookCheckout> BookCheckouts { get; set; }
    }
}
