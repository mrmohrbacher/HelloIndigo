using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Blackriverinc.Framework.Utility;

namespace Blackriverinc.Framework.Tests
	{
	/// <summary>
	/// Summary description for UtilityTests
	/// </summary>
	[TestClass]
	public class UtilityTests
		{
		public UtilityTests()
			{
			//
			// TODO: Add constructor logic here
			//
			}

		[TestMethod]
		public void Encryption_DefaultSeed()
			{
			string plainText = "planet!express!delivery!service";
			encryption(plainText);
			}


		[TestMethod]
		public void Encryption_NewSeed()
			{
			string plainText = "planet!express!delivery!service";
			encryption(plainText);
			}


		private static void encryption(string plainText, string seed = null)
			{
			Trace.WriteLine(string.Format("Plain Text '{0}'", plainText));
			Encryption encryptor = new Encryption(seed);
			string cryptText = encryptor.Encrypt(plainText);
			Trace.WriteLine(string.Format("Crypt Text '{0}'", cryptText));
			string decryptedText = encryptor.Decrypt(cryptText);
			Trace.WriteLine(string.Format("Decrypted Text '{0}'", decryptedText));
			Assert.AreEqual(plainText, decryptedText);
			}

		[TestMethod]
		public void String_CamelCaseToDelimited()
			{
			string camelText = "BookISBN";
			Trace.WriteLine(string.Format("Camel Text '{0}'", camelText));
			string delimText = camelText.CamelCaseToDelimitted();
			Trace.WriteLine(string.Format("Delim Text '{0}'", delimText));
			Assert.AreEqual("Book ISBN", delimText);
			}

		}
	}
