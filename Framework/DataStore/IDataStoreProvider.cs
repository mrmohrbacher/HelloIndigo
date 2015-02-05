using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackriverinc.Framework.DataStore
	{
	public interface IDataStoreProvider : IDisposable
		{
		void Load(IKeyedDataStore store);
		void Save(IKeyedDataStore store);
		}

	public abstract class DataStoreProvider : IDataStoreProvider
		{

		IDataStoreProvider _interiorProvider = null;

		public DataStoreProvider(IDataStoreProvider interiorProvider)
			{
			_interiorProvider = interiorProvider;
			}

		public virtual void Load(IKeyedDataStore store)
			{
			if (_interiorProvider != null)
				{
				_interiorProvider.Load(store);
				}
			}

		public abstract void Save(IKeyedDataStore store);

		#region Disposable
		~DataStoreProvider()
			{
			Dispose(false);
			}

		protected virtual void Dispose(bool disposing)
			{
			if (disposing)
				{

				}
			}

		public void Dispose()
			{
			GC.SuppressFinalize(this);
			Dispose(true);
			}
		#endregion

		}


	}
