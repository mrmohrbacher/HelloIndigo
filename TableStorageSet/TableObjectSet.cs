using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace TableStorageSet
   {
   public class TableObjectSet<TEntity> : TableServiceContext, IObjectSet<TEntity>
      where TEntity : TableServiceEntityBase, new()
      {

      public TableObjectSet(string baseAddress, StorageCredentials credentials) : 
         base(baseAddress, credentials)
         {

         }

      public new void SaveChanges()
         {
         base.SaveChanges();
         }

      #region IObjectSet

      // AddObject, Attach, DeleteObject, Detach, GetEnumerator,
      // ElementType, Expression, Provider

      public void AddObject(TEntity entity)
         {
         base.AddObject(TableServiceEntityBase.TableName, entity);
         }

      public void Attach(TEntity entity)
         {
         base.AttachTo(TableServiceEntityBase.TableName, entity);
         }

      public void DeleteObject(TEntity entity)
         {
         base.DeleteObject(entity);
         }

      public void Detach(TEntity entity)
         {
         base.Detach(entity);
         }

      public IEnumerator<TEntity> GetEnumerator()
         {
         throw new NotImplementedException();
         }

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
         {
         throw new NotImplementedException();
         }

      public Type ElementType
         {
         get { return typeof(TEntity); }
         }

      public System.Linq.Expressions.Expression Expression
         {
         get
            {
            string tableName = TableServiceEntityBase.TableName;
            return ((TableServiceContext)this).CreateQuery<TEntity>(tableName).Expression;
            }
         }

      public IQueryProvider Provider
         {
         get
            {
            string tableName = TableServiceEntityBase.TableName;
            return ((TableServiceContext)this).CreateQuery<TEntity>(tableName).Provider;
            }
         }

      #endregion
      }
   }
