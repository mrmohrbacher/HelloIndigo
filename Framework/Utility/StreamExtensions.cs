using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Blackriverinc.Framework.Utility
   {
   public static class StreamExtensions
      {
      //
      public static string ContentsToString(this Stream input)
         {
         if (input.Length > int.MaxValue)
            throw new ArgumentOutOfRangeException(string.Format("'input' Stream contents too long"));

         long current = input.Position;
         string result = null;

         try
            {
            int inputLength = (int)input.Length;

            byte[] buffer = new byte[inputLength];

            input.Position = 0;
            input.Read(buffer, 0, inputLength);

            ASCIIEncoding encoder = new ASCIIEncoding();
            result = encoder.GetString(buffer);
            }
         finally
            {
            input.Position = current;
            }

         return result;
         }
      }
   }
