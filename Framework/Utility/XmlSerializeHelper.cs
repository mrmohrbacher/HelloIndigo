using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Blackriverinc.Framework.Utility
   {
   public class XmlSerializeHelper
      {
      /// <summary>
      /// Helper function to serialize classe into an XML held
      /// by a StringBuilder.
      /// </summary>
      /// <param name="output">StringBuilder object owned by caller
      /// to act as a buffer for the serialied object.</param>
      /// <param name="obj">Object to serialize. It must be decorated
      /// with [Serializable] attribute.</param>
      /// <returns>String representation of current state of 'output'</returns>
      static ListDictionary serializers = new ListDictionary();

      public static string SerializeToString<T>(ref StringBuilder output, IEnumerable<T> list) where T : new()
         {
         try
            {
            if (list == null)
               return output.ToString();
            output.AppendLine("<!-- Start List -->");
            output.AppendLine(String.Format("<{0}>", list.GetType().Name));
            foreach (T item in list)
               SerializeToString(ref output, item);
            output.AppendLine(String.Format("</{0}>", list.GetType().Name));
            output.AppendLine("<!-- End List -->");
            }
         catch (Exception exp)
            {
            Trace.WriteLine(exp.ToString());
            throw;
            }
         return output.ToString();
         }

		public static string SerializeToString(ref StringBuilder output, KeyValuePair<string, object> kv)
			{
			try
				{
				if (kv.Key == null)
					return output.ToString();

				output.AppendFormat("   <add key='{0}' value='{1}' />", kv.Key, kv.Value.ToString());
				output.AppendLine();
				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				throw;
				}
			return output.ToString();
			}

		public static string SerializeToString(ref StringBuilder output, KeyValuePair<string, string> kv)
			{
			try
				{
				if (kv.Key == null)
					return output.ToString();

				output.AppendFormat("   <add key='{0}' value='{1}' />", kv.Key, kv.Value.ToString());
				output.AppendLine();
				}
			catch (Exception exp)
				{
				Trace.WriteLine(exp.ToString());
				throw;
				}
			return output.ToString();
			}

      public static string SerializeToString<T>(ref StringBuilder output, T obj) where T : new()
         {
         try
            {
            Type type = typeof(T);
            if (type.IsClass && obj == null)
               return output.ToString();

            System.Xml.Serialization.XmlSerializer serializer = null;
            lock (serializers.SyncRoot)
               {
               if (serializers.Contains(type))
                  serializer = (System.Xml.Serialization.XmlSerializer)serializers[type];
               else
                  {
                  serializer = new System.Xml.Serialization.XmlSerializer(type);
                  serializers.Add(type, serializer);
                  }
               }
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, obj);
            ASCIIEncoding encoding = new ASCIIEncoding();
            output.AppendLine(encoding.GetString(stream.GetBuffer()).TrimEnd());
            output.AppendLine();
            }
         catch (Exception exp)
            {
            Trace.WriteLine(exp.ToString());
            throw;
            }
         return output.ToString();
         }
      }
   }