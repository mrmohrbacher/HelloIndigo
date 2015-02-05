using System;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Text;


namespace Blackriverinc.Framework.Utility
   {

   //---------------------------------------------------------------------------------------
   //
   //   Class:       AppTraceListener
   //   Author:      Mike Teather
   //   Purpose:     Logs trace output to distinct files named according to the day
   //                month and year.  The log file captures all Trace and Debug output
   //                in addition to all exceptions published via the ExceptionManager.
   //
   //   Notes:       The path defaults to "c:\", but is read from the initializeData
   //                attribute that can be set in the application configuration file. if (
   //                the path is set in the configuration file, it must include the ending
   //                backslash (\).
   //
   //                Each write operation open a new streamwriter and) closes it
   //                after writing the content.  This is necessary to prevent the file
   //                from locking, we want a developer to be able to open the log file
   //                at any time to assist with troubleshooting.
   //
   //   07/09/2010 Mmohrbacher
   //
   //              + Timestamp at start of line
   //              + "Opening Log" "Closing Log" messages
   //              + No param c-tor opens log at location of Main Module/Logs
   //---------------------------------------------------------------------------------------
   public class AppTraceListener : TextWriterTraceListener
      {
      //  Member Variables
      private string m_strPath = Environment.GetEnvironmentVariable("SystemDrive") + @"\";

      private DateTime m_openDate = DateTime.Now;

      // true; when the last call was to WriteLine.
      private bool m_lineState = true;

      private bool m_logState = false;

      #region "DisplayTaskBoolean"
      // Display TaskID inline in Message Line Header
      public bool DisplayTaskID
         {
         get;
         set;
         }
      #endregion

      private void initialize(string name)
         {
         try
            {
            string traceMask = ConfigurationManager.AppSettings["TraceMask"];
            if (traceMask != null 
            && traceMask.ToUpper().Contains("LOG"))
               {
               string baseDir = null;
               if (!Path.IsPathRooted(name))
                  {
                  Process process = Process.GetCurrentProcess();
                  ProcessModule mainModule = process.MainModule;
                  baseDir = System.IO.Path.GetDirectoryName(mainModule.FileName);
                  m_strPath = string.Format(@"{0}\Logs\{1}", baseDir, name);
                  }
               else
                  m_strPath = name;

               if (traceMask.ToUpper().Contains("TASK"))
                  DisplayTaskID = true;

               baseDir = System.IO.Path.GetDirectoryName(m_strPath);
               if (!Directory.Exists(baseDir))
                  Directory.CreateDirectory(baseDir);

               Trace.WriteLine(string.Format("Opening TraceListener Log at '{0}'",
                              m_strPath));

               System.Diagnostics.Trace.Listeners.Add(this);
               }
            }
         catch (Exception ex)
            {
            Trace.WriteLine(ex.ToString());
            }
         }

      #region "Constructors"

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Default Constructor
      //   Author:      Mike Teather
      //   Purpose:     Prepares the object for use.
      //
      //---------------------------------------------------------------------------------------
      public AppTraceListener()
         {
         DisplayTaskID = false;
         initialize("Trace");
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Constructor
      //   Author:      Mike Teather
      //   Purpose:     Prepares the object for use with a text writer and a name.
      //
      //---------------------------------------------------------------------------------------
      public AppTraceListener(TextWriter writer, string name) :
         base(writer, name)
         {
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Constructor
      //   Author:      Mike Teather
      //   Purpose:     Prepares the object for use with a text writer and a name.
      //
      //---------------------------------------------------------------------------------------
      public AppTraceListener(TextWriter writer) :
         base(writer)
         {
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Constructor
      //   Author:      Mike Teather
      //   Purpose:     Prepares the object for use with a text writer and a name.
      //
      //---------------------------------------------------------------------------------------
      public AppTraceListener(Stream stream) :
         base(stream)
         {
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Constructor
      //   Author:      Mike Teather
      //   Purpose:     Prepares the object for use with a text writer and a name.
      //
      //---------------------------------------------------------------------------------------
      public AppTraceListener(Stream stream, string name) :
         base(stream, name)
         {
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Constructor
      //   Author:      Mike Teather
      //   Purpose:     Prepares the object for use with a text writer and a name.
      //
      //   Notes:       The fileName is assumed to contain a directory location
      //                including the ending \.  The fileName is passed from the config file
      //                using the form:  initializeData="c:\temp\test\".
      //
      //                The AppTraceListener.log file is used to initialize the writer
      //                but all the actual content is logged to files mm-dd-yyyy.log
      //
      //---------------------------------------------------------------------------------------
      public AppTraceListener(string fileName)
         {
         initialize(fileName);
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Constructor
      //   Author:      Mike Teather
      //   Purpose:     Prepares the object for use with a text writer and a name.
      //
      //   Notes:       The fileName is assumed to contain a directory location
      //                including the ending \.  The fileName is passed from the config file
      //                using the form:  initializeData="c:\temp\test\".
      //
      //                The AppTraceListener.log file is used to initialize the writer
      //                but all the actual content is logged to files mm-dd-yyyy.log
      //        
      //---------------------------------------------------------------------------------------
      public AppTraceListener(string fileName, string name)
         {
         initialize(fileName);
         }

      #endregion

      #region "Public Methods"

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Write
      //   Author:      Mike Teather
      //   Purpose:     Overrides the Write method to initialize the correct writer.
      //
      //   Notes:       The writer must be opened and closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void Write(string message)
         {
         //  Verif (y the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.Write(Format(message));
				}
         }
      //---------------------------------------------------------------------------------------
      //
      //   Method:      Write
      //   Author:      Mike Teather
      //   Purpose:     Overrides the Write method to initialize the correct writer.
      //
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void Write(object o)
         {
         //  Verify the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.Write(Format("{0}", o));
				}
         }


      //---------------------------------------------------------------------------------------
      //
      //   Method:      Write
      //   Author:      Mike Teather
      //   Purpose:     Overrides the Write method to initialize the correct writer.
      //
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void Write(object o, string category)
         {
         //  Verify the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.Write(Format("{0}", o));
				}
         }
      //---------------------------------------------------------------------------------------
      //
      //   Method:      Write
      //   Author:      Mike Teather
      //   Purpose:     Overrides the Write method to initialize the correct writer.
      //
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void Write(string message, string category)
         {
         //  Verif (y the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.Write(Format("{0}", message), category);
				}
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      WriteLine
      //   Author:      Mike Teather
      //   Purpose:     Overrides the WriteLine method to initialize the correct writer.
      //
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void WriteLine(string message)
         {
         //  Verify the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.WriteLine(Format("{0}", message));
				m_lineState = true;
				}
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      WriteLine
      //   Author:      Mike Teather
      //   Purpose:     Overrides the WriteLine method to initialize the correct writer.
      //        
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void WriteLine(object o)
         {
         //  Verif (y the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.WriteLine(Format("{0}", o));
				m_lineState = true;
				}
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      WriteLine
      //   Author:      Mike Teather
      //   Purpose:     Overrides the WriteLine method to initialize the correct writer.
      //        
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void WriteLine(object o, string category)
         {
         //  Verif (y the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.WriteLine(Format("{0}", o), category);
				m_lineState = true;
				}
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      WriteLine
      //   Author:      Mike Teather
      //   Purpose:     Overrides the WriteLine method to initialize the correct writer.
      //        
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void WriteLine(string message, string category)
         {
         //  Verif (y the writer is accessing the correct file
			using (open())
				{
				//  Call the base method to write the output
				base.WriteLine(Format("{0}", message), category);
				m_lineState = true;
				}
         }

      //---------------------------------------------------------------------------------------
      //
      //   Method:      Flush
      //   Author:      Mike Teather
      //   Purpose:     Overrides the Flush method to initialize hit the correct writer.
      //        
      //   Notes:       The writer must be opened and) closed for each operation
      //                to prevent the file from being locked (at any time a developer
      //                can manually open the log file to troubleshoot a problem).
      //
      //---------------------------------------------------------------------------------------
      override public void Flush()
         {
         //  Close the writer
         Writer.Close();
         }

      override public void Close()
         {
         //  Verif (y the writer is accessing the correct file
			using (open())
				{
				string logPath = String.Format("{1}-{0:yyyyMMdd}.log", m_openDate, m_strPath);
				base.WriteLine(Format(String.Format("--- Closing Log : {0} ---",
																	  Path.GetFileName(logPath.ToString()))));
				m_lineState = true;

				System.Diagnostics.Trace.Listeners.Remove(this);
				}
         base.Close();

         }

      #endregion
      #region "Private Methods"
      private string Format(string fmt, params object[] arguments)
         {
         if (m_lineState)
            {
            m_lineState = false;
            StringBuilder msgBuilder = new StringBuilder();
            msgBuilder.AppendFormat("{0:HH:mm:ss.fff} ", DateTime.Now);
            if (DisplayTaskID)
               msgBuilder.AppendFormat("[{0}]",
                  System.Threading.Thread.CurrentThread.ManagedThreadId);
            msgBuilder.Append(" : ");
            return msgBuilder.AppendFormat(fmt, arguments).ToString();
            }
         else
            return string.Format(fmt, arguments);
         }


      //---------------------------------------------------------------------------------------
      //
      //   Method:      PrepareWriter
      //   Author:      Mike Teather
      //   Purpose:     Creates a writer and prepares for use.
      //
      //---------------------------------------------------------------------------------------
		private TextWriter open()
         {
         DateTime now = DateTime.Now;
         StringBuilder logPath = new StringBuilder();

         if (now.Date != m_openDate.Date)
            {
            logPath.AppendFormat("{1}-{0:yyyyMMdd}.log", m_openDate, m_strPath);
            this.Writer = TextWriter.Synchronized(new StreamWriter(File.Open(logPath.ToString(), FileMode.Append)));
            logPath.Length = 0;

            base.Flush();

            m_openDate = now.Date;
            m_logState = false;
            m_lineState = true;
            }

         logPath.AppendFormat("{1}-{0:yyyyMMdd}.log", m_openDate, m_strPath);

         this.Writer = TextWriter.Synchronized(new StreamWriter(File.Open(logPath.ToString(), FileMode.Append)));

         if (!m_logState)
            {
            m_logState = true;
            base.WriteLine(Format(String.Format("--- Opening Log : {0} ---", Path.GetFileName(logPath.ToString()))));
            m_lineState = true;
            }

			return this.Writer;
         }

      #endregion

      };

   }
