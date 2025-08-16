Imports Pao.Reports
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows

Namespace Sample
    Friend Module Make広告
        Public Sub SetupData(paoRep As IReport)
            'Excel データベースへの接続
            Dim connection As OleDbConnection = Util.ConnectExcelDB("広告.xls")
            Dim sql As String = "select * from [広告情報$]"
            Dim dataAdapter As New OleDbDataAdapter(sql, connection)
            Dim ds As New DataSet()

            dataAdapter.Fill(ds, "広告情報")

            Dim table As DataTable = ds.Tables("広告情報")
            paoRep.LoadDefFile(Util.SharePath & "広告.prepd")

            For Each row As DataRow In table.Rows
                paoRep.PageStart()
                paoRep.Write("製品名", CStr(row("製品名")))
                paoRep.Write("キャッチフレーズ", CStr(row("キャッチフレーズ")))
                paoRep.Write("商品コード", CStr(row("商品コード")))
                paoRep.Write("JANコード", CStr(row("商品コード")))
                paoRep.Write("売り文句", CStr(row("売り文句")))
                paoRep.Write("説明", CStr(row("説明")))
                paoRep.Write("価格", CStr(row("価格")))
                paoRep.Write("画像1", Util.SharePath & CStr(row("画像1")))
                paoRep.Write("画像2", Util.SharePath & CStr(row("画像2")))
                paoRep.Write("QR", CStr(row("製品名")) & " " & CStr(row("キャッチフレーズ")))
                paoRep.PageEnd()
            Next

            Return
        End Sub
    End Module
End Namespace