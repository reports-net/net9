Imports System
Imports Pao.Reports

Friend Module Make10の倍数

    Public Sub SetupData(paoRep As IReport)
        paoRep.LoadDefFile(Util.SharePath & "レポート定義ファイル.prepd")

        Dim page As Integer = 0
        Dim line As Integer = 0

        For i As Integer = 0 To 59
            If i Mod 15 = 0 Then
                paoRep.PageStart()
                page += 1
                line = 0

                paoRep.Write("日付", DateTime.Now.ToString())
                paoRep.Write("頁数", "Page - " & page.ToString())

                paoRep.z_Objects.SetObject("フォントサイズ")
                paoRep.z_Objects.z_Text.z_FontAttr.Size = 12
                paoRep.Write("フォントサイズ", "フォントサイズ" & Environment.NewLine & " 変更後")

                If page = 2 Then
                    paoRep.Write("Line3", "")
                End If
            End If
            line += 1

            paoRep.Write("行番号", (i + 1).ToString(), line)
            paoRep.Write("10倍数", ((i + 1) * 10).ToString(), line)
            paoRep.Write("横線", line)

            If ((i + 1) Mod 15) = 0 Then paoRep.PageEnd()
        Next
    End Sub

End Module
