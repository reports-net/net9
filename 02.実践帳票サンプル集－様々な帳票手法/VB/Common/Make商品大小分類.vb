Imports Pao.Reports
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Forms

Namespace Sample
    Friend Module Make商品大小分類
        Private Class PrintData
            Friend s大分類コード As String
            Friend s小分類コード As String
            Friend s大分類名称 As String
            Friend s小分類名称 As String
            Friend s品番 As String
            Friend s品名 As String
        End Class

        Public Sub SetupData(paoRep As IReport)
            'Excel データベースへの接続
            Dim connection As OleDbConnection = Util.ConnectExcelDB("商品マスタ.xls")

            'データセットの作成
            Dim ds As New DataSet()

            'データセットへテーブルをセットする。ヘッダと明細の2テーブル
            Dim sql As String = ""

            sql &= " SELECT C.*, A.大分類名称, B.小分類名称 "
            sql &= " FROM "
            sql &= "   [M_大分類$] AS A"
            sql &= " , [M_小分類$] AS B"
            sql &= " , [M_商品$] AS C"
            sql &= " WHERE"
            sql &= " A.大分類コード = B.大分類コード"
            sql &= " AND"
            sql &= " A.大分類コード = C.大分類コード"
            sql &= " AND"
            sql &= " B.大分類コード = C.大分類コード"
            sql &= " AND"
            sql &= " B.小分類コード = C.小分類コード"
            sql &= " ORDER BY C.大分類コード, C.小分類コード"

            Dim oda As New OleDbDataAdapter(sql, connection)

            oda.Fill(ds, "商品一覧")

            ' いったん構造体の配列にセット
            Dim dt As DataTable = ds.Tables("商品一覧")

            Dim sv大分類名称 As String = Nothing
            Dim sv小分類名称 As String = Nothing
            Dim cnt大分類 As Integer = 0
            Dim cnt小分類 As Integer = 0
            Dim pds As New List(Of PrintData)()
            Dim pd As PrintData

            For Each dr As DataRow In dt.Rows
                pd = New PrintData()

                ' キーブレイク処理は、今回は構造体にセットするところでやってみました。
                ' プログラム構造的にもっと汎用的な方法はあります。
                If sv小分類名称 IsNot Nothing AndAlso sv小分類名称 <> dr("小分類名称").ToString() Then
                    pd.s小分類コード = " "
                    pd.s小分類名称 = "小分類(" & sv小分類名称 & ")小計"
                    pd.s品番 = cnt小分類.ToString() & " 冊"
                    cnt小分類 = 0
                    pds.Add(pd)
                    pd = New PrintData()
                End If

                If sv大分類名称 IsNot Nothing AndAlso sv大分類名称 <> dr("大分類名称").ToString() Then
                    pd.s大分類コード = " "
                    pd.s小分類名称 = "大分類(" & sv大分類名称 & ")小計"
                    pd.s品番 = cnt大分類.ToString() & " 冊"
                    cnt大分類 = 0
                    pds.Add(pd)
                    pd = New PrintData()
                End If

                If sv大分類名称 <> dr("大分類名称").ToString() Then
                    pd.s大分類名称 = dr("大分類名称").ToString()
                End If

                If sv小分類名称 <> dr("小分類名称").ToString() Then
                    pd.s小分類名称 = dr("小分類名称").ToString()
                End If

                pd.s品番 = dr("品番").ToString()
                pd.s品名 = dr("品名").ToString()

                pds.Add(pd)

                sv大分類名称 = dr("大分類名称").ToString()
                sv小分類名称 = dr("小分類名称").ToString()

                cnt大分類 += 1
                cnt小分類 += 1
            Next

            pd = New PrintData()
            pd.s小分類コード = " "
            pd.s小分類名称 = "小分類(" & sv小分類名称 & ")小計"
            pd.s品番 = cnt小分類.ToString() & " 冊"
            pds.Add(pd)

            pd = New PrintData()
            pd.s大分類コード = " "
            pd.s小分類名称 = "大分類(" & sv大分類名称 & ")小計"
            pd.s品番 = cnt大分類.ToString() & " 冊"
            pds.Add(pd)

            '商品一覧の生成
            paoRep.LoadDefFile(Util.SharePath & "商品一覧.prepd")
            paoRep.PageStart()

            Const RecnumInPage As Integer = 20

            paoRep.z_Objects.SetObject("枠_大分類")
            Dim svBackColor As System.Drawing.Color = paoRep.z_Objects.z_Square.PaintColor

            Dim filedNames_枠 As String() = {"枠_大分類", "枠_小分類", "枠_品番", "枠_品名"}
            Dim filedNames As String() = {"大分類", "小分類", "品番", "品名"}

            For recno As Integer = 0 To pds.Count - 1
                If recno Mod RecnumInPage = 0 Then
                    If recno <> 0 Then
                        paoRep.PageEnd()
                        paoRep.PageStart()
                    End If
                End If

                ' 値セット
                Dim lineno As Integer = (recno Mod RecnumInPage) + 1
                paoRep.Write("大分類", pds(recno).s大分類名称, lineno)
                paoRep.Write("小分類", pds(recno).s小分類名称, lineno)
                paoRep.Write("品番", pds(recno).s品番, lineno)
                paoRep.Write("品名", pds(recno).s品名, lineno)

                ' 枠描画
                For j As Integer = 0 To filedNames_枠.Length - 1
                    paoRep.Write(filedNames_枠(j), lineno)
                Next

                ' 小分類小計行の色替え
                If pds(recno).s小分類コード = " " Then
                    ' 枠描画
                    For j As Integer = 0 To filedNames_枠.Length - 1
                        paoRep.z_Objects.SetObject(filedNames_枠(j), lineno)
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.LightYellow
                    Next
                    ' 大分類小計行の色替え
                ElseIf pds(recno).s大分類コード = " " Then
                    ' 枠描画
                    For j As Integer = 0 To filedNames_枠.Length - 1
                        paoRep.z_Objects.SetObject(filedNames_枠(j), lineno)
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.LightPink
                    Next
                End If
            Next

            paoRep.PageEnd()

            Return
        End Sub
    End Module
End Namespace