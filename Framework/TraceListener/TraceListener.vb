Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Runtime
Imports System.Text



'---------------------------------------------------------------------------------------
'
'   Class:       AppTraceListener
'   Author:      Mike Teather
'   Purpose:     Logs trace output to distinct files named according to the day
'                month and year.  The log file captures all Trace and Debug output
'                in addition to all exceptions published via the ExceptionManager.
'
'   Notes:       The path defaults to "c:\", but is read from the initializeData
'                attribute that can be set in the application configuration file. If
'                the path is set in the configuration file, it must include the ending
'                backslash (\).
'
'                Each write operation open a new streamwriter and then closes it
'                after writing the content.  This is necessary to prevent the file
'                from locking, we want a developer to be able to open the log file
'                at any time to assist with troubleshooting.
'
'   07/09/2010 Mmohrbacher
'
'              + Timestamp at start of line
'              + "Opening Log" "Closing Log" messages
'              + No param c-tor opens log at location of Main Module/Logs
'---------------------------------------------------------------------------------------
Public Class AppTraceListener : Inherits TextWriterTraceListener

    '  Member Variables
    Private m_strPath As String = Environment.GetEnvironmentVariable("SystemDrive") & "\"

    Private m_openDate As Date = Date.Today

    ' True when the last call was to WriteLine.
    Private m_lineState As Boolean = True

    Private m_logState As Boolean = False

#Region "DisplayTaskBoolean"
    ' Display TaskID inline in Message Line Header
    Private m_displayTaskID As Boolean = False
    Public Property DisplayTaskID As Boolean
        Get
            Return m_displayTaskID
        End Get
        Set(ByVal value As Boolean)
            m_displayTaskID = value
        End Set
    End Property
#End Region

    Private Sub initialize(ByVal name As String)

        Dim baseDir As String = Nothing
        If Not System.IO.Path.IsPathRooted(name) Then
            Dim process As Process = process.GetCurrentProcess()
            Dim mainModule As ProcessModule = process.MainModule
            baseDir = System.IO.Path.GetDirectoryName(mainModule.FileName)
            m_strPath = String.Format("{0}\Logs\{1}", baseDir, name)
        Else
            m_strPath = String.Format("{0}", name)
        End If


        baseDir = System.IO.Path.GetDirectoryName(m_strPath)
        If Not Directory.Exists(baseDir) Then Directory.CreateDirectory(baseDir)

        Trace.Listeners.Add(Me)
    End Sub


#Region "Constructors"

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Default Constructor
    '   Author:      Mike Teather
    '   Purpose:     Prepares the object for use.
    '
    '---------------------------------------------------------------------------------------
    Public Sub New()
        initialize("Trace")
    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Constructor
    '   Author:      Mike Teather
    '   Purpose:     Prepares the object for use with a text writer and a name.
    '
    '---------------------------------------------------------------------------------------
    Public Sub New(ByVal writer As TextWriter, _
       ByVal name As String)
        MyBase.New(writer, name)

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Constructor
    '   Author:      Mike Teather
    '   Purpose:     Prepares the object for use with a text writer and a name.
    '
    '---------------------------------------------------------------------------------------
    Public Sub New(ByVal writer As TextWriter)

        MyBase.New(writer)

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Constructor
    '   Author:      Mike Teather
    '   Purpose:     Prepares the object for use with a text writer and a name.
    '
    '---------------------------------------------------------------------------------------
    Public Sub New(ByVal stream As Stream)

        MyBase.New(stream)

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Constructor
    '   Author:      Mike Teather
    '   Purpose:     Prepares the object for use with a text writer and a name.
    '
    '---------------------------------------------------------------------------------------
    Public Sub New(ByVal stream As Stream, _
       ByVal name As String)

        MyBase.New(stream, name)

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Constructor
    '   Author:      Mike Teather
    '   Purpose:     Prepares the object for use with a text writer and a name.
    '
    '   Notes:       The fileName is assumed to contain a directory location
    '                including the ending \.  The fileName is passed from the config file
    '                using the form:  initializeData="c:\temp\test\".
    '
    '                The AppTraceListener.log file is used to initialize the writer
    '                but all the actual content is logged to files mm-dd-yyyy.log
    '
    '---------------------------------------------------------------------------------------
    Public Sub New(ByVal fileName As String)

        initialize(fileName)

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Constructor
    '   Author:      Mike Teather
    '   Purpose:     Prepares the object for use with a text writer and a name.
    '
    '   Notes:       The fileName is assumed to contain a directory location
    '                including the ending \.  The fileName is passed from the config file
    '                using the form:  initializeData="c:\temp\test\".
    '
    '                The AppTraceListener.log file is used to initialize the writer
    '                but all the actual content is logged to files mm-dd-yyyy.log
    '        
    '---------------------------------------------------------------------------------------
    Public Sub New(ByVal fileName As String, _
       ByVal name As String)

        initialize(fileName)

    End Sub

#End Region

#Region "Public Methods"

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Write
    '   Author:      Mike Teather
    '   Purpose:     Overrides the Write method to initialize the correct writer.
    '
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub Write(ByVal message As String)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.Write(Format(message))

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Write
    '   Author:      Mike Teather
    '   Purpose:     Overrides the Write method to initialize the correct writer.
    '
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub Write(ByVal o As Object)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.Write(Format("{0}", o))

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Write
    '   Author:      Mike Teather
    '   Purpose:     Overrides the Write method to initialize the correct writer.
    '
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub Write(ByVal o As Object, _
         ByVal category As String)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.Write(Format("{0}", o), category)

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Write
    '   Author:      Mike Teather
    '   Purpose:     Overrides the Write method to initialize the correct writer.
    '
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub Write(ByVal message As String, _
         ByVal category As String)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.Write(Format("{0}", message), category)

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      WriteLine
    '   Author:      Mike Teather
    '   Purpose:     Overrides the WriteLine method to initialize the correct writer.
    '
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub WriteLine(ByVal message As String)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.WriteLine(Format("{0}", message))
        m_lineState = True

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      WriteLine
    '   Author:      Mike Teather
    '   Purpose:     Overrides the WriteLine method to initialize the correct writer.
    '        
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub WriteLine(ByVal o As Object)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.WriteLine(Format("{0}", o))
        m_lineState = True

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      WriteLine
    '   Author:      Mike Teather
    '   Purpose:     Overrides the WriteLine method to initialize the correct writer.
    '        
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub WriteLine(ByVal o As Object, _
         ByVal category As String)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.WriteLine(Format("{0}", o), category)
        m_lineState = True

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      WriteLine
    '   Author:      Mike Teather
    '   Purpose:     Overrides the WriteLine method to initialize the correct writer.
    '        
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overloads Overrides Sub WriteLine(ByVal message As String, _
         ByVal category As String)

        '  Verify the writer is accessing the correct file
        PrepareWriter()

        '  Call the base method to write the output
        MyBase.WriteLine(Format("{0}", message), category)
        m_lineState = True

        '  Close the writer
        Me.Writer.Close()

    End Sub

    '---------------------------------------------------------------------------------------
    '
    '   Method:      Flush
    '   Author:      Mike Teather
    '   Purpose:     Overrides the Flush method to initialize hit the correct writer.
    '        
    '   Notes:       The writer must be opened and then closed for each operation
    '                to prevent the file from being locked (at any time a developer
    '                can manually open the log file to troubleshoot a problem).
    '
    '---------------------------------------------------------------------------------------
    Public Overrides Sub Flush()
        '  Close the writer
        Me.Writer.Close()

    End Sub

    Public Overrides Sub Close()
        '  Verify the writer is accessing the correct file
        PrepareWriter()

        Dim logPath As String = String.Format("{1}-{0:yyyyMMdd}.log", m_openDate, m_strPath)
        MyBase.WriteLine(Me.Format(String.Format("--- Closing Log : {0} ---",
             Path.GetFileName(logPath.ToString()))))
        m_lineState = True

        MyBase.Close()
    End Sub

#End Region

#Region "Private Methods"
    Private Function Format(ByVal fmt As String, ByVal ParamArray params As Object()) As String

        Dim result As String = Nothing
        If m_lineState Then
            m_lineState = False
            Dim msgBuilder As New StringBuilder()
            msgBuilder.AppendFormat("{0:HH:mm:ss} ", Date.Now)
            If DisplayTaskID Then msgBuilder.AppendFormat("[{0}]",
              System.Threading.Thread.CurrentThread.ManagedThreadId)
            msgBuilder.Append(" : ")
            result = msgBuilder.AppendFormat(fmt, params).ToString()
        Else
            result = String.Format(fmt, params)
        End If

        Return (result)
    End Function


    '---------------------------------------------------------------------------------------
    '
    '   Method:      PrepareWriter
    '   Author:      Mike Teather
    '   Purpose:     Creates a writer and prepares for use.
    '
    '---------------------------------------------------------------------------------------
    Private Sub PrepareWriter()

        Dim now As DateTime = Date.Now
        Dim logPath As New Text.StringBuilder()

        If Not now.Date = m_openDate.Date Then

            logPath.AppendFormat("{1}-{0:yyyyMMdd}.log", m_openDate, m_strPath)
            Me.Writer = TextWriter.Synchronized(New StreamWriter(File.Open(logPath.ToString(), FileMode.Append)))
            logPath.Length = 0

            MyBase.Flush()

            m_openDate = now.Date
            m_logState = False
            m_lineState = True
        End If

        logPath.AppendFormat("{1}-{0:yyyyMMdd}.log", m_openDate, m_strPath)

        Me.Writer = TextWriter.Synchronized(New StreamWriter(File.Open(logPath.ToString(), FileMode.Append)))

        If Not m_logState Then
            m_logState = True
            MyBase.WriteLine(Me.Format(String.Format("--- Opening Log : {0} ---", Path.GetFileName(logPath.ToString()))))
            m_lineState = True
        End If

    End Sub

#End Region

End Class


