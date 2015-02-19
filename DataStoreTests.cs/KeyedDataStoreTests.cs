using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blackriverinc.Framework.DataStore;

namespace Blackriverinc.Framework.Tests
	{
	[TestClass]
	public class KeyedDataStoreTests
		{

		void trace(string fmt, params object[] args)
			{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0:HH:mm:ss.fff} ", DateTime.Now);
			sb.AppendFormat(fmt, args);
			Trace.WriteLine(sb.ToString());
			}

		IKeyedDataStore _store;

		public KeyedDataStoreTests()
			{
			_store = new KeyedDataStore(new AppConfigProvider());
			}

		[TestMethod]
		public void KeyedDataStore_GetString()
			{
			string expected = "A string pulled from 'setting' [string-1].";
			trace("Expected : {0}", expected);
			string actual = _store["string-1"] as string;
			trace(" Actual : {0}", actual);

			Assert.AreEqual(expected, actual);
			}


		[TestMethod]
		public void KeyedDataStore_GetInteger()
			{
			int expected = 5977;
			trace("Expected : {0}", expected);
			int? actual = null;
			bool result = _store.get("integer-5977", ref actual);
			trace(" Actual : {0}", actual);

			Assert.AreEqual(expected, actual);
			}

		[TestMethod]
		public void KeyedDataStore_GetBoolNotFound()
			{
			trace("Expected : null");
			bool? actual = null;
			bool result = _store.get("bool-truex", ref actual);
			trace(" Actual : {0}", (actual.HasValue)?actual.Value.ToString():"null");
			Assert.IsFalse(result);
			Assert.IsFalse(actual.HasValue);
			}

		[TestMethod]
		public void KeyedDataStore_GetBoolFoundTrue()
			{
			bool expected = true;
			trace("Expected : {0}", expected);
			bool? actual = null;
			bool result = _store.get("bool-true", ref actual);
			trace(" Actual : {0}", actual);

			Assert.AreEqual(expected, actual);
			}

		[TestMethod]
		public void KeyedDataStore_GetBoolFoundFalse()
			{
			bool expected = false;
			trace("Expected : {0}", expected);
			bool? actual = null;
			bool result = _store.get("bool-false", ref actual);
			trace(" Actual : {0}", actual);

			Assert.AreEqual(expected, actual);
			}
		}
	}
