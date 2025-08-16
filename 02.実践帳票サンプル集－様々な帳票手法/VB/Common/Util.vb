Imports System
Imports System.Data.OleDb
Imports System.Windows

Namespace Sample
    Friend Module Util
        Friend SharePath As String = ""

        Friend Sub SetSharePath()
            SharePath = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() & "/../../../../../Resources/")
        End Sub


        Friend Function GetDefFile(index As Integer) As String
            Return SharePath & "zip" & (index + 1).ToString() & ".prepd"
        End Function

        ' 指定されたExcelファイルををデータベースとして接続し、コネクションを返す。
        Friend Function ConnectExcelDB(ExcelFileName As String) As OleDbConnection
            Dim connectString As String = Nothing

            If IntPtr.Size = 4 Then
                connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & SharePath & ExcelFileName & ";Extended Properties=Excel 8.0;"
            ElseIf IntPtr.Size = 8 Then
                connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & SharePath & ExcelFileName & ";Extended Properties=Excel 12.0;"
            End If

            Return New OleDbConnection(connectString)
        End Function
    End Module
End Namespace