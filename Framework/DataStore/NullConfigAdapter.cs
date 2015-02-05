using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Configuration;

using Microsoft.WindowsAzure.ServiceRuntime;

namespace Blackriverinc.Framework.DataStore
   {
   public class NullProvider : DataStoreProvider
      {
      public NullProvider() : base(null)
         {
         }

      public override void Load(IKeyedDataStore store)
         {
         }

      public override void Save(IKeyedDataStore store)
         {
         }
      }
   }
