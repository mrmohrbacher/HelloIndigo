﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Blackriverinc.Framework.DataStore;
    
    public partial class LibraryEntities : DbContext
    {
        static LibraryEntities()
        {
            var ensureProviderLoadedHack = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    
        /* Modified from original version of the template */
        public LibraryEntities(string connectionString)
                : base(string.IsNullOrEmpty(connectionString)?"name=LibraryEntities":connectionString)
        {
        }
        public LibraryEntities()
             : base(ConnectionFactory.GetConnection(), true)
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public IDbSet<Library.Model.Book> Books { get; set; }
        public IDbSet<Library.Model.BookCheckout> BookCheckouts { get; set; }
        public IDbSet<Library.Model.Subscriber> Subscribers { get; set; }
    }
}
