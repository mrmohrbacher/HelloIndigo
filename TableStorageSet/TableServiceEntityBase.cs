//------------------------------------------------------------------------------
// 
//------------------------------------------------------------------------------

using System.Data.Services.Common;
using System.Web;

namespace TableStorageSet
   {
   /* Entity.Name = 'TableServiceEntity' */
   [DataServiceEntity, DataServiceKey("PartitionKey", "RowKey")]
   public abstract class TableServiceEntityBase
      // 
      {
      // Initialized by a static constructor to the qualified name 
      // of the concreate type.
      protected static string _tableName;

      public static string TableName { get { return _tableName; } }

      protected abstract string buildRowKey();

      protected abstract void parseRowKey(string rowKey);

      #region Primitive Properties

      public string RowKey
         {
         get
            {
            return buildRowKey();
            }
         set
            {
            parseRowKey(value);
            }
         }

      public string PartitionKey
         {
         get;
         set;
         }

      public System.DateTime Timestamp
         {
         get;
         set;
         }

      #endregion
      }
   }
