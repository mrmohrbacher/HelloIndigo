Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class clsEncryption

    '------------------------------------------------------------------------------
    ' Generate a cryptographically strong random hex password 8 chars in length

    Public Shared Function GeneratePassword() As String
        Dim random() As Byte = New Byte(3) {} ' Stupid array notation... this is actually 4 bytes
        Dim rng As New RNGCryptoServiceProvider()
        rng.GetBytes(random)
        Dim b As Byte
        Dim sPwd As String = ""
        For Each b In random
            sPwd &= b.ToString("x")
        Next
        rng = Nothing
        Return sPwd.PadLeft(8, "0") ' Needed because hex ToString for one is just 1 not 01
    End Function

    '------------------------------------------------------------------------------

    Private moRijn As RijndaelManaged

    '------------------------------------------------------------------------------
    ' Initializes the Rijndael encryption object

    Public Sub New()
        moRijn = New RijndaelManaged()
        Dim converter As New ASCIIEncoding()

        ' TODO:  Keep the key & IV in more secure place(s)!!!
        ' (DPAPI won't work because multiple apps on different computers running under 
        '  different user accounts have to use this DLL.  A plain-text config file or  
        '  registry settings are even less secure than hard-coding the keys.)
        moRijn.Key = converter.GetBytes("R1jnd@el@V@n31la")
        moRijn.IV = converter.GetBytes("0ldPwdV3t3r@n!42")
    End Sub

    '------------------------------------------------------------------------------
    ' Encrypts using Rijndael 128-bit and then Base-64 encodes the result

    Public Function Encrypt(ByVal ValueToEncrypt As String) As String
        Dim converter As New ASCIIEncoding()
        Return Convert.ToBase64String(Encrypt(converter.GetBytes(ValueToEncrypt)))
    End Function

    '------------------------------------------------------------------------------

    Public Function Encrypt(ByVal toEncrypt() As Byte) As Byte()
        Dim msEncrypt As New MemoryStream()
        Dim encryptor As ICryptoTransform = moRijn.CreateEncryptor()
        Dim csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
        csEncrypt.Write(toEncrypt, 0, toEncrypt.Length)
        csEncrypt.FlushFinalBlock()
        Dim encrypted() As Byte = msEncrypt.ToArray()

        csEncrypt.Close()
        msEncrypt.Close()
        csEncrypt = Nothing
        msEncrypt = Nothing

        Return encrypted
    End Function

    '------------------------------------------------------------------------------
    ' Base-64 decodes the input and then decrypts using Rijndael 128-bit

    Public Function Decrypt(ByVal ValueToDecrypt As String) As String
        ' Decode, decrypt, and trim trailing blanks
        Dim converter As New ASCIIEncoding()
        Return TrimBlanks(converter.GetString(Decrypt(Convert.FromBase64String(ValueToDecrypt))))
    End Function

    '------------------------------------------------------------------------------
    ' Remove any blank chars at the end (trim doesn't work ?!)

    Public Function TrimBlanks(ByVal decryptedText As String) As String
        Dim acOutput() As Char = decryptedText.ToCharArray(0, decryptedText.Length)
        Dim sb As New StringBuilder()
        With sb
            Dim i As Integer = 0
            While (i < acOutput.Length AndAlso acOutput(i) <> Nothing)
                .Append(acOutput(i))
                i += 1
            End While
        End With
        Return sb.ToString
    End Function

    '------------------------------------------------------------------------------

    Public Function Decrypt(ByVal toDecrypt() As Byte) As Byte()
        Dim decryptor As ICryptoTransform = moRijn.CreateDecryptor()
        Dim decrypted() As Byte = New Byte(toDecrypt.Length) {}
        Dim msDecrypt As New MemoryStream(toDecrypt)
        Dim csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
        If msDecrypt.Length > 1 Then
            csDecrypt.Read(decrypted, 0, decrypted.Length)
            csDecrypt.Close()
        End If


        msDecrypt.Close()
        csDecrypt = Nothing
        msDecrypt = Nothing

        Return decrypted
    End Function

    '------------------------------------------------------------------------------

End Class