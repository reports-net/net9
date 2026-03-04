Imports System
Imports System.Data
Imports System.Data.OleDb
Imports Pao.Reports

Friend Module Make見積書

    Public Sub SetupData(paoRep As IReport)
        Dim connection As OleDbConnection = Util.ConnectExcelDB("見積書.xls")

        Dim ds As New DataSet()

        Dim sql As String = "SELECT * FROM [見積ヘッダ$] ORDER BY 見積番号"
        Dim oda As New OleDbDataAdapter(sql, connection)
        oda.Fill(ds, "見積ヘッダ")

        sql = "select * from [見積明細$] ORDER BY 見積番号,行番号"
        oda = New OleDbDataAdapter(sql, connection)
        oda.Fill(ds, "見積明細")

        Dim ht As DataTable = ds.Tables("見積ヘッダ")
        For Each hdr As DataRow In ht.Rows
            ' 表紙の生成
            paoRep.LoadDefFile(Util.SharePath & "表紙.prepd")
            paoRep.PageStart()
            paoRep.Write("お客様名", DirectCast(hdr("お客様名"), String))
            paoRep.Write("担当者名", DirectCast(hdr("担当者名"), String))
            paoRep.PageEnd()

            ' 見積書の生成
            paoRep.LoadDefFile(Util.SharePath & "見積書.prepd")
            paoRep.PageStart()

            paoRep.Write("見積番号", DirectCast(hdr("見積番号"), String))
            paoRep.Write("お客様名", DirectCast(hdr("お客様名"), String))
            paoRep.Write("担当者名", DirectCast(hdr("担当者名"), String))
            paoRep.Write("見積日", DirectCast(hdr("見積日"), DateTime).ToString("yyyy年M月d日"))
            paoRep.Write("ヘッダ合計", "\ " & String.Format("{0:N0}", hdr("合計金額")))
            paoRep.Write("消費税額", String.Format("{0:N0}", hdr("消費税額")))
            paoRep.Write("フッタ合計", String.Format("{0:N0}", hdr("合計金額")))

            ' 明細の背景作成
            For i As Integer = 0 To 6
                paoRep.Write("品番白", i + 1)
                paoRep.Write("品番白", i + 1)
                paoRep.Write("数量白", i + 1)
                paoRep.Write("単価白", i + 1)
                paoRep.Write("金額白", i + 1)
                paoRep.Write("品番青", i + 1)
                paoRep.Write("品名青", i + 1)
                paoRep.Write("数量青", i + 1)
                paoRep.Write("単価青", i + 1)
                paoRep.Write("金額青", i + 1)
            Next

            ' 明細の作成
            Dim dv As New DataView(ds.Tables("見積明細"))
            dv.RowFilter = "見積番号 = '" & DirectCast(hdr("見積番号"), String) & "'"
            For i As Integer = 0 To dv.Count - 1
                paoRep.Write("品番", DirectCast(dv(i)("品番"), String), i + 1)
                paoRep.Write("品名", DirectCast(dv(i)("品名"), String), i + 1)
                paoRep.Write("数量", dv(i)("数量").ToString(), i + 1)
                paoRep.Write("単価", String.Format("{0:N0}", dv(i)("単価")), i + 1)
                paoRep.Write("金額", String.Format("{0:N0}", dv(i)("金額")), i + 1)
            Next
            paoRep.PageEnd()
        Next
    End Sub

End Module
