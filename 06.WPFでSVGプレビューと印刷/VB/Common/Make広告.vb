Imports System
Imports System.Data
Imports System.Data.OleDb
Imports Pao.Reports

Friend Module Make広告

    Public Sub SetupData(paoRep As IReport)
        Dim connection As OleDbConnection = Util.ConnectExcelDB("広告.xls")
        Dim sql As String = "select * from [広告情報$]"

        Dim dataAdapter As New OleDbDataAdapter(sql, connection)
        Dim ds As New DataSet()
        dataAdapter.Fill(ds, "広告情報")

        Dim table As DataTable = ds.Tables("広告情報")

        paoRep.LoadDefFile(Util.SharePath & "広告.prepd")
        For Each row As DataRow In table.Rows
            paoRep.PageStart()

            paoRep.Write("製品名", DirectCast(row("製品名"), String))
            paoRep.Write("キャッチフレーズ", DirectCast(row("キャッチフレーズ"), String))
            paoRep.Write("商品コード", DirectCast(row("商品コード"), String))
            paoRep.Write("JANコード", DirectCast(row("商品コード"), String))
            paoRep.Write("売り文句", DirectCast(row("売り文句"), String))
            paoRep.Write("説明", DirectCast(row("説明"), String))
            paoRep.Write("価格", DirectCast(row("価格"), String))
            paoRep.Write("画像1", Util.SharePath & DirectCast(row("画像1"), String))
            paoRep.Write("画像2", Util.SharePath & DirectCast(row("画像2"), String))
            paoRep.Write("QR", DirectCast(row("製品名"), String) & " " & DirectCast(row("キャッチフレーズ"), String))

            paoRep.PageEnd()
        Next
    End Sub

End Module
