using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

using OnlineReportingServices.Framework.DataView;

public class DataTableToHTMLFormatter : IDataTableFormatter
   {
   private StreamWriter output;

   /// <summary>
   /// Default Formatter writes onto a StringStream.
   /// </summary>
   public DataTableToHTMLFormatter()
      {
      output = new System.IO.StreamWriter(new MemoryStream());
      rowTransform = new IdentityDataRowTransformer().Transform;
      }

   public DataTableToHTMLFormatter(string fileName)
      {
      Stream stream = StreamFactory.Create(fileName, FileMode.Create, FileAccess.ReadWrite);
      output = new StreamWriter(stream);
      rowTransform = new IdentityDataRowTransformer().Transform;
      }

   public DataTableToHTMLFormatter(Stream outputStream)
      {
      output = new StreamWriter(outputStream);
      }

   #region "RowTransform"
   private DataRowTransform rowTransform;
   public DataRowTransform RowTransform
      {
      set
         {
         rowTransform = value;
         }
      }
   #endregion

   public Stream Format(DataTable input)
      {
      XmlWriterSettings xmlSettings = new XmlWriterSettings();
      xmlSettings.Indent = true;
      xmlSettings.Encoding = Encoding.UTF8;
      xmlSettings.IndentChars = "   ";
      XmlWriter writer = XmlTextWriter.Create(output, xmlSettings);

      try
         {
         writer.WriteStartDocument();
         writer.WriteDocType("xhtml",
               "-//W3C//DTD XHTML 1.0 Transitional//EN",
               "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd",
               null);
         writer.WriteStartElement("html", "http://www.w3.org/1999/xhtml");

         writer.WriteStartElement("head");
         writer.WriteElementString("title", input.TableName);
         writer.WriteFullEndElement();

         writer.WriteStartElement("body");
         writer.WriteElementString("h2", input.TableName);
         writer.WriteStartElement("table");
         writer.WriteAttributeString("border", "1px solid black");
         writer.WriteAttributeString("cellspacing", "0");
         writer.WriteAttributeString("cellpadding", "2");

         writer.WriteStartElement("tr");
         writer.WriteAttributeString("style", "background-color:0xDDDDDD");

         foreach (DataColumn col in input.Columns)
            writer.WriteElementString("th", col.ColumnName);

         // </tr>
         writer.WriteEndElement();


         StringBuilder cellBuilder = new StringBuilder();
         foreach (DataRow rawRow in input.Rows)
            {
            // Copy of the row, because, unlike VB.NET, the foreach iteration
            // variable is immutable. (However the contents of the variable
            // are not!)
            DataRow row = null;

            //	Change the structure of the row per the requirements of this application
            row = rowTransform(rawRow);

            // Suppress row from being emitted if it was flagged as "bad".
            if (!row.HasErrors && row.RowState != DataRowState.Deleted)
               {
               writer.WriteStartElement("tr");

               foreach (DataColumn col in input.Columns)
                  {
                  cellBuilder.Length = 0;
                  if (row[col.ColumnName] == DBNull.Value)
                     cellBuilder.Append("&nbsp;");
                  else
                     cellBuilder.Append(row[col.ColumnName]);

                  writer.WriteStartElement("td");

                  writer.WriteString(cellBuilder.ToString());

                  writer.WriteEndElement();
                  }

               // </tr>
               writer.WriteEndElement();
               }
            }

         // </table></body></html>
         writer.WriteEndElement();
         writer.WriteEndElement();
         writer.WriteEndElement();

         // Flush writer contents to output.
         writer.Flush();
         }
      catch (Exception)
         {

         throw;
         }
      finally
         {
         }

      return output.BaseStream;
      }
   }
