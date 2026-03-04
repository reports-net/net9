Imports System
Imports System.Data
Imports System.Data.OleDb
Imports Pao.Reports

Friend Module Make郵便番号一覧

    Public Sub SetupData(paoRep As IReport)
        Dim connection As OleDbConnection = Util.ConnectExcelDB("zip.xls")

        Dim SQL As String = "select * from [郵便番号テーブル$]"

        Dim dataAdapter As New OleDbDataAdapter(SQL, connection)
        Dim ds As New DataSet()
        dataAdapter.Fill(ds, "PostTable")
        Dim table As DataTable = ds.Tables("PostTable")

        Dim page As Integer = 0
        Dim line As Integer = 999
        Dim hDate As String = DateTime.Now.ToString()

        paoRep.LoadDefFile(Util.SharePath & "zip1.prepd")
        For Each row As DataRow In table.Rows
            line += 1
            If line > 32 Then
                If page <> 0 Then paoRep.PageEnd()

                page += 1

                If page = 6 Then
                    paoRep.LoadDefFile(Util.SharePath & "zip2.prepd")
                End If

                paoRep.PageStart()

                paoRep.Write("日時", hDate)
                paoRep.Write("ページ", "Page-" & page.ToString())

                If page < 6 Then
                    paoRep.Write("QR", row("郵便番号").ToString() & " " & row("市区町村").ToString() & row("住所").ToString())
                End If

                line = 1
            End If

            paoRep.Write("郵便番号", row("郵便番号").ToString(), line)
            paoRep.Write("市区町村", row("市区町村").ToString(), line)
            paoRep.Write("住所", row("住所").ToString(), line)
            paoRep.Write("横罫線", line)

            If page > 5 AndAlso line Mod 2 = 0 Then
                paoRep.Write("網掛け", line \ 2)
            End If
        Next
        paoRep.PageEnd()
    End Sub

End Module
