using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blackriverinc.Framework.Utility
   {
   public class Encryption
      {
      //-----------------------------------------------------------------------------
      // Generate a cryptographically strong random hex password 8 chars in length
      public static string GeneratePassword()
         {
         byte[] random = new byte[4];
         StringBuilder pwd = new StringBuilder();
         using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
            rng.GetBytes(random);
            foreach (byte b in random)
               {
               pwd.Append(b.ToString("0X"));
               }
            }
         return pwd.ToString();
         }

      //-----------------------------------------------------------------------------

      private RijndaelManaged moRijn = null;

      //-----------------------------------------------------------------------------
      // Initializes the Rijndael encryption object
      public Encryption()
         {
			moRijn = new RijndaelManaged();
			int blockSize = moRijn.BlockSize;
			moRijn.KeySize = 128;
         ASCIIEncoding converter = new ASCIIEncoding();

         moRijn.Key = converter.GetBytes("casenikeyurtrapt");
         moRijn.IV = converter.GetBytes("13126394289=====");
         }

      //-----------------------------------------------------------------------------
      // Encrypts using Rijndael 128-bit and then Base-64 encodes the result
      public string Encrypt(string plainText)
         {
         ASCIIEncoding converter = new ASCIIEncoding();
         return Convert.ToBase64String(Encrypt(converter.GetBytes(plainText)));
         }

      //-----------------------------------------------------------------------------

      public byte[] Encrypt(byte[] plainText)
         {
         MemoryStream msEncrypt = new MemoryStream();
			byte[] encrypted;

         ICryptoTransform encryptor = moRijn.CreateEncryptor();
         using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
            csEncrypt.Write(plainText, 0, plainText.Length);
            csEncrypt.FlushFinalBlock();
				encrypted = msEncrypt.ToArray();
            }

         return encrypted;
         }

      //-----------------------------------------------------------------------------
      // Base-64 decodes the input and then decrypts using Rijndael 128-bit

      public string Decrypt(string cryptText)
         {
         // Decode, decrypt, and trim trailing blanks
         ASCIIEncoding converter = new ASCIIEncoding();
         return converter.GetString(Decrypt(Convert.FromBase64String(cryptText)));
         }

      //-----------------------------------------------------------------------------

      public byte[] Decrypt(byte[] cryptText)
         {
         ICryptoTransform decryptor = moRijn.CreateDecryptor();
         MemoryStream msDecrypt = new MemoryStream();
         using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
            //      if (msDecrypt.Length > 1)
            //          csDecrypt.Read(decrypted, 0, decrypted.Length);
            }
         byte[] plainBytes = null;
         return plainBytes;
         }

      }
   }
