Imports System
Imports System.Data
Imports System.Data.OleDb
Imports Pao.Reports

Friend Module Make請求書

    Public Sub SetupData(paoRep As IReport)
        Dim connection As OleDbConnection = Util.ConnectExcelDB("請求書.xls")

        Dim ds As New DataSet()

        Dim sql As String = "select * from [請求ヘッダ$] ORDER BY 請求番号"
        Dim oda As New OleDbDataAdapter(sql, connection)
        oda.Fill(ds, "請求ヘッダ")

        sql = "select * from [請求明細$] ORDER BY 請求番号, 行番号"
        oda = New OleDbDataAdapter(sql, connection)
        oda.Fill(ds, "請求明細")

        paoRep.LoadDefFile(Util.SharePath & "請求書.prepd")

        Dim arr_w As Single() = {-5, 44, -20, -10, -9}

        Dim ht As DataTable = ds.Tables("請求ヘッダ")
        For Each hdr As DataRow In ht.Rows
            paoRep.PageStart()

            paoRep.Write("txtNo", DirectCast(hdr("請求番号"), String))
            paoRep.Write("txtCustomer", DirectCast(hdr("お客様名"), String))
            paoRep.Write("txtDate", DateTime.Now.ToString("yyyy年M月d日"))
            paoRep.Write("Image1", Util.SharePath & "角印.png")

            paoRep.z_Objects.SetObject("hLine")
            Dim maxHLine As Integer = paoRep.z_Objects.z_Line.Repeat - 1
            paoRep.z_Objects.SetObject("vLine")
            Dim maxVLine As Integer = paoRep.z_Objects.z_Line.Repeat - 1

            For i As Integer = 0 To maxHLine - 1
                paoRep.Write("hLine", i + 1)

                If i = 0 Then
                    paoRep.z_Objects.SetObject("hLine", i + 1)
                    paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5F
                End If

                If i = 1 Then
                    paoRep.z_Objects.SetObject("hLine", i + 1)
                    paoRep.z_Objects.z_Line.z_LineAttr.Type = PmLineType.Double
                End If

                paoRep.Write("LineRect", i + 1)
                paoRep.z_Objects.SetObject("LineRect", i + 1)

                If i = 0 Then
                    ' ヘッダ行
                ElseIf i < maxHLine - 3 Then
                    If i Mod 2 = 1 Then
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.White
                    Else
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.LightSkyBlue
                    End If
                Else
                    paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.FromArgb(255, 255, 180)
                End If

                Dim svX As Single = -1

                For j As Integer = 0 To maxVLine - 1
                    paoRep.z_Objects.SetObject("field" & (j + 1).ToString(), i + 1)

                    paoRep.z_Objects.z_Text.Width = paoRep.z_Objects.z_Text.Width + arr_w(j)

                    If j > 0 Then
                        paoRep.z_Objects.z_Text.X = svX
                    End If
                    svX = paoRep.z_Objects.z_Text.X + paoRep.z_Objects.z_Text.Width

                    If i = 0 Then
                        paoRep.z_Objects.z_Text.z_FontAttr.Bold = True
                    Else
                        paoRep.z_Objects.z_Text.z_FontAttr.Bold = False
                        paoRep.z_Objects.z_Text.z_FontAttr.Size = 12

                        Select Case j + 1
                            Case 1
                                paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Center
                            Case 2
                                paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Left
                            Case 3, 4, 5
                                paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Right
                        End Select
                    End If
                Next

                For j As Integer = maxHLine To maxHLine - 2 Step -1
                    paoRep.z_Objects.SetObject("field4", j)
                    paoRep.z_Objects.z_Text.z_FontAttr.Size = 16
                    paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Center
                    paoRep.z_Objects.z_Text.z_FontAttr.Bold = True
                Next
            Next

            paoRep.z_Objects.SetObject("vLine")
            Dim baseX As Single = paoRep.z_Objects.z_Line.X
            For j As Integer = 0 To maxVLine
                paoRep.Write("vLine", j + 1)

                paoRep.z_Objects.SetObject("vLine", j + 1)

                For jj As Integer = 1 To j
                    If j >= maxVLine Then Exit For
                    Dim baseIntervalX As Single = paoRep.z_Objects.z_Line.IntervalX
                    paoRep.z_Objects.z_Line.IntervalX = baseIntervalX + arr_w(j - jj)
                Next

                If j = 0 OrElse j = maxVLine Then
                    paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5F
                End If
            Next

            paoRep.Write("field1", "品番", 1)
            paoRep.Write("field2", "品名", 1)
            paoRep.Write("field3", "数量", 1)
            paoRep.Write("field4", "単価", 1)
            paoRep.Write("field5", "金額", 1)

            Dim dv As New DataView(ds.Tables("請求明細"))
            dv.RowFilter = "請求番号 = '" & DirectCast(hdr("請求番号"), String) & "'"
            Dim totalAmount As Long = 0
            Dim ii As Integer = 0
            Do While ii < dv.Count
                paoRep.Write("field1", DirectCast(dv(ii)("品番"), String), ii + 2)
                paoRep.Write("field2", DirectCast(dv(ii)("品名"), String), ii + 2)
                paoRep.Write("field3", dv(ii)("数量").ToString(), ii + 2)
                paoRep.Write("field4", String.Format("{0:N0}", dv(ii)("単価")), ii + 2)
                Dim amount As Long = Convert.ToInt64(dv(ii)("数量")) * Convert.ToInt64(dv(ii)("単価"))
                paoRep.Write("field5", String.Format("{0:N0}", amount), ii + 2)
                totalAmount += amount
                ii += 1
            Loop

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

            paoRep.z_Objects.SetObject("hLine", maxHLine - 2)
            paoRep.z_Objects.z_Line.z_LineAttr.Type = PmLineType.Double

            paoRep.Write("hLine", maxHLine + 1)
            paoRep.z_Objects.SetObject("hLine", maxHLine + 1)
            paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5F

            paoRep.PageEnd()
        Next
    End Sub

End Module
