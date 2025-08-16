Imports Pao.Reports
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Forms

Namespace Sample
    Friend Module Make請求書
        Public Sub SetupData(paoRep As IReport)
            'Excel データベースへの接続
            Dim connection As OleDbConnection = Util.ConnectExcelDB("請求書.xls")

            'データセットの作成
            Dim ds As New DataSet()

            'データセットへテーブルをセットする。ヘッダと明細の2テーブル
            Dim sql As String = "select * from [請求ヘッダ$] ORDER BY 請求番号"
            Dim oda As New OleDbDataAdapter(sql, connection)

            oda.Fill(ds, "請求ヘッダ")

            sql = "select * from [請求明細$] ORDER BY 請求番号, 行番号"
            oda = New OleDbDataAdapter(sql, connection)
            oda.Fill(ds, "請求明細")

            '請求書の生成
            paoRep.LoadDefFile(Util.SharePath & "請求書.prepd")

            ' 各列幅調整の配列
            Dim arr_w As Single() = {-5, 44, -20, -10, -9}

            Dim ht As DataTable = ds.Tables("請求ヘッダ")
            For Each hdr As DataRow In ht.Rows
                paoRep.PageStart()

                paoRep.Write("txtNo", CStr(hdr("請求番号")))
                paoRep.Write("txtCustomer", CStr(hdr("お客様名")))
                paoRep.Write("txtDate", DateTime.Now.ToString("yyyy年M月d日"))
                paoRep.Write("Image1", Util.SharePath & "角印.png")

                ' デザイン時の行数・列数取得
                paoRep.z_Objects.SetObject("hLine")
                Dim maxHLine As Integer = paoRep.z_Objects.z_Line.Repeat - 1
                paoRep.z_Objects.SetObject("vLine")
                Dim maxVLine As Integer = paoRep.z_Objects.z_Line.Repeat - 1

                '空の表を作成
                For i As Integer = 0 To maxHLine - 1
                    ' 「横罫線」描画
                    paoRep.Write("hLine", i + 1)

                    ' 外枠の上を太く
                    If i = 0 Then
                        paoRep.z_Objects.SetObject("hLine", i + 1)
                        paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5F
                    End If

                    ' 行ヘッダの下を二重線
                    If i = 1 Then
                        paoRep.z_Objects.SetObject("hLine", i + 1)
                        paoRep.z_Objects.z_Line.z_LineAttr.Type = PmLineType.Double
                    End If

                    ' 「行の背景」描画
                    paoRep.Write("LineRect", i + 1)
                    paoRep.z_Objects.SetObject("LineRect", i + 1)

                    If i = 0 Then
                        ' 行ヘッダはデザイン通り
                    ElseIf i < maxHLine - 3 Then
                        ' 明細行
                        ' 白・青の順番で背景色をセット
                        If i Mod 2 = 1 Then
                            paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.White
                        Else
                            paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.LightSkyBlue
                        End If
                    Else
                        ' 集計行
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.FromArgb(255, 255, 180)
                    End If

                    ' 次回のXの位置
                    Dim svX As Single = -1

                    For j As Integer = 0 To maxVLine - 1
                        ' 文字列項目の属性(幅/Font/Align/)変更
                        paoRep.z_Objects.SetObject("field" & (j + 1).ToString(), i + 1)

                        ' 幅(TextBox)
                        paoRep.z_Objects.z_Text.Width = paoRep.z_Objects.z_Text.Width + arr_w(j)

                        ' 位置(TextBox)
                        If j > 0 Then
                            paoRep.z_Objects.z_Text.X = svX
                        End If
                        svX = paoRep.z_Objects.z_Text.X + paoRep.z_Objects.z_Text.Width

                        ' 行ヘッダの場合
                        If i = 0 Then
                            paoRep.z_Objects.z_Text.z_FontAttr.Bold = True
                        Else
                            ' 明細行の場合
                            paoRep.z_Objects.z_Text.z_FontAttr.Bold = False
                            paoRep.z_Objects.z_Text.z_FontAttr.Size = 12

                            ' 文字位置(Text Align)
                            Select Case j + 1
                                Case 1
                                    paoRep.z_Objects.z_Text.TextAlign = Pao.Reports.PmAlignType.Center
                                Case 2
                                    paoRep.z_Objects.z_Text.TextAlign = Pao.Reports.PmAlignType.Left
                                Case 3, 4, 5
                                    paoRep.z_Objects.z_Text.TextAlign = Pao.Reports.PmAlignType.Right
                            End Select
                        End If
                    Next

                    '集計行の文字設定
                    For j As Integer = maxHLine To maxHLine - 2 Step -1
                        paoRep.z_Objects.SetObject("field4", j)
                        paoRep.z_Objects.z_Text.z_FontAttr.Size = 16
                        paoRep.z_Objects.z_Text.TextAlign = Pao.Reports.PmAlignType.Center
                        paoRep.z_Objects.z_Text.z_FontAttr.Bold = True
                    Next
                Next

                ' 縦罫線描画
                paoRep.z_Objects.SetObject("vLine")
                Dim baseX As Single = paoRep.z_Objects.z_Line.X

                For j As Integer = 0 To maxVLine
                    paoRep.Write("vLine", j + 1)

                    paoRep.z_Objects.SetObject("vLine", j + 1)

                    ' 幅調整
                    For jj As Integer = 1 To j
                        If j < maxVLine Then
                            Dim baseIntervalX As Single = paoRep.z_Objects.z_Line.IntervalX
                            paoRep.z_Objects.z_Line.IntervalX = baseIntervalX + arr_w(j - jj)
                        End If
                    Next

                    ' 外枠を太線にする
                    If j = 0 OrElse j = maxVLine Then
                        paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5F
                    End If
                Next

                ' 見出し文字入れ
                paoRep.Write("field1", "品番", 1)
                paoRep.Write("field2", "品名", 1)
                paoRep.Write("field3", "数量", 1)
                paoRep.Write("field4", "単価", 1)
                paoRep.Write("field5", "金額", 1)

                '明細の作成
                Dim dv As New DataView(ds.Tables("請求明細"))
                dv.RowFilter = "請求番号 = '" & CStr(hdr("請求番号")) & "'"
                Dim totalAmount As Long = 0
                Dim ii As Integer = 0

                For ii = 0 To dv.Count - 1
                    paoRep.Write("field1", CStr(dv(ii)("品番")), ii + 2)
                    paoRep.Write("field2", CStr(dv(ii)("品名")), ii + 2)
                    paoRep.Write("field3", dv(ii)("数量").ToString(), ii + 2)
                    paoRep.Write("field4", String.Format("{0:N0}", dv(ii)("単価")), ii + 2)
                    Dim amount As Long = Convert.ToInt64(dv(ii)("数量")) * Convert.ToInt64(dv(ii)("単価"))
                    paoRep.Write("field5", String.Format("{0:N0}", amount), ii + 2)
                    totalAmount += amount
                Next

                Dim tax As Double = 0.05

                paoRep.Write("field4", "小計", maxHLine - 2)
                paoRep.Write("field5", String.Format("{0:N0}", totalAmount), maxHLine - 2)
                ii += 1

                paoRep.Write("field4", "消費税", maxHLine - 1)
                paoRep.Write("field5", String.Format("{0:N0}", totalAmount * tax), maxHLine - 1)
                ii += 1

                paoRep.Write("field4", "合計", maxHLine)
                paoRep.Write("field5", String.Format("{0:N0}", totalAmount + (totalAmount * tax)), maxHLine)

                paoRep.Write("txtTotal", String.Format("{0:N0}", totalAmount + (totalAmount * tax)))

                ' 小計の上を二重線
                paoRep.z_Objects.SetObject("hLine", maxHLine - 2)
                paoRep.z_Objects.z_Line.z_LineAttr.Type = PmLineType.Double

                ' 最終行を太く
                paoRep.Write("hLine", maxHLine + 1)
                paoRep.z_Objects.SetObject("hLine", maxHLine + 1)
                paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5F

                paoRep.PageEnd()
            Next

            Return
        End Sub
    End Module
End Namespace