Imports Pao.Reports
Imports System.Drawing.Printing
Imports System.IO

Namespace Sample
    Partial Public Class Form1
        Inherits Form

        Private sharePath_ As String

        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

            ' C# との共有リソースパス取得
            sharePath_ = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() + "/../../../../../")

        End Sub

        Private Sub btnExecute_Click_1(sender As Object, e As EventArgs) Handles btnExecute.Click
            Dim paoRep As IReport = Nothing

            '■インスタンスの生成がエラーとなる場合の対処法下記コメントを外してください
            'System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

            If radPreview.Checked = True Then   'ラジオボタンでプレビューが選択されている場合
                'プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf()
            ElseIf radPrint.Checked = True Then  'ラジオボタンで印刷が選択されている場合
                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()
            ElseIf radGetPrintDocument.Checked = True Then  'ラジオボタンで独自プレビュー(GetPrintDocument取得)が選択されている場合

                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()

                ' ↑ OR ↓(どちらでも可) 

                'プレビューオブジェクトのインスタンスを獲得
                'paoRep = ReportCreator.GetPreview()


            ElseIf radPDF.Checked = True Then    'ラジオボタンでPDF出力が選択されている場合
                'PDF出力オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPdf()
            Else 'ラジオボタンでSVG / XPSが選択されている場合
                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()
            End If

            'レポート定義ファイルの読み込み
            paoRep.LoadDefFile(sharePath_ & "レポート定義ファイル.prepd")

            Dim page As Integer = 0 '頁数を定義
            Dim line As Integer = 0 '行数を定義
            Dim i As Integer
            For i = 1 To 60
                If ((i - 1) Mod 15 = 0) Then '1頁15行で開始
                    '頁開始を宣言
                    paoRep.PageStart()
                    page = page + 1  '頁数をインクリメント
                    line = 0 '行数を初期化

                    '＊＊＊ヘッダのセット＊＊＊
                    '文字列のセット
                    paoRep.Write("日付", System.DateTime.Now.ToString())
                    paoRep.Write("頁数", "Page - " + page.ToString())

                    'オブジェクトの属性変更
                    paoRep.z_Objects.SetObject("フォントサイズ")
                    paoRep.z_Objects.z_Text.z_FontAttr.Size = 12
                    paoRep.Write("フォントサイズ", "フォントサイズ変更後")

                    '２頁目の線をを消す
                    If page = 2 Then paoRep.Write("Line3", "")

                End If
                line = line + 1 '行数をインクリメント

                '＊＊＊明細のセット＊＊＊
                '繰返し文字列のセット
                paoRep.Write("行番号", i.ToString(), line)
                paoRep.Write("10倍数", (i * 10).ToString(), line)
                '繰返し図形(横線)のセット
                paoRep.Write("横線", line)

                If ((i Mod 15) = 0) Then paoRep.PageEnd() '1頁15行で終了
            Next i

            If radPreview.Checked = True _
        Or radPrint.Checked = True Then '印刷・プレビューが選択されている場合
                ' オマケのコメントです。m(_ _;)m 印刷の設定を色々試してみてください。m(_ _)m
                'Dim setting = New System.Drawing.Printing.PrinterSettings()
                'setting.PrinterName = "Acrobat Distiller"
                'setting.FromPage = 1
                'setting.ToPage = 5
                'setting.MinimumPage = 2
                'setting.MaximumPage = 3
                'paoRep.DisplayDialog = False
                'paoRep.Output(setting) ' 印刷又はプレビューを実行

                'ドキュメント名
                paoRep.DocumentName = "10の倍数の印刷ドキュメント"

                'プレビューウィンドウタイトル
                paoRep.z_PreviewWindowWpf.z_TitleText = "10の倍数の印刷プレビュー"

                'プレビューウィンドウアイコン
                paoRep.z_PreviewWindowWpf.z_Icon = New Icon(sharePath_ & "PreView.ico")

                'バージョンウィンドウの情報変更
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName = "御社製品名"
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName_ForeColor = Color.Blue

                '(初期)プレビュー表示倍率
                paoRep.ZoomPreview = 77


                paoRep.Output() '印刷/プレビューを実行

            ElseIf radGetPrintDocument.Checked = True Then  '独自プレビュー(PrrintDocument取得)が選択されている場合

                ' PrintDocument 取得
                printDocument1 = paoRep.GetPrintDocument()

                ' このフォームのプレビューコントロールへ プレビュー実行
                prevWinForm.Document = printDocument1
                prevWinForm.InvalidatePreview()

                'ここでは、抜けることにします。(印刷データの保存・読み込み・プレビューはしない)
                Return


            ElseIf radPDF.Checked = True Then  'PDF出力が選択されている場合

                'PDF出力
                saveFileDialog.FileName = "印刷データ"
                saveFileDialog.Filter = "PDF形式 (*.pdf)|*.pdf"

                If saveFileDialog.ShowDialog() = DialogResult.OK Then

                    paoRep.SavePDF(saveFileDialog.FileName) '印刷データの保存

                    If (MessageBox.Show(Me, "PDFを表示しますか？", "PDF の表示", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                        Dim startInfo As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName)
                        startInfo.UseShellExecute = True
                        System.Diagnostics.Process.Start(startInfo)
                    End If

                End If

            ElseIf radSVG.Checked = True Then  'SVGが選択されている場合

                'SVG出力
                saveFileDialog.FileName = "印刷データ"
                saveFileDialog.Filter = "html形式 (*.html)|*.html"

                If saveFileDialog.ShowDialog() = DialogResult.OK Then

                    'SVGデータの保存
                    paoRep.SaveSVGFile(saveFileDialog.FileName)

                    If (MessageBox.Show(Me, "ブラウザで表示しますか？" & vbCrLf & "表示する場合、SVGプラグインが必要です。", "SVG / SVGZ の表示", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                        Dim startInfo As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName)
                        startInfo.UseShellExecute = True
                        System.Diagnostics.Process.Start(startInfo)
                    End If

                End If

            ElseIf radXPS.Checked = True Then 'XPS出力が選択されている場合

                'XPS出力
                saveFileDialog.FileName = "印刷データ"
                saveFileDialog.Filter = "XPS形式 (*.xps)|*.xps"

                If saveFileDialog.ShowDialog() = DialogResult.OK Then

                    'XPSデータの保存
                    paoRep.SaveXPS(saveFileDialog.FileName)

                    If (MessageBox.Show(Me, "XPSを表示しますか？", "XPS の表示", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                        Dim startInfo As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName)
                        startInfo.UseShellExecute = True
                        System.Diagnostics.Process.Start(startInfo)
                    End If

                End If


            End If


            'マニュアル・ヘルプにはありませんが付け加えました。
            If MessageBox.Show(Me, "続いて、印刷データXMLファイルを保存して再度読み込んでプレビューを行います。", "Save And Reload Print Data", MessageBoxButtons.OKCancel) = DialogResult.Cancel Then
                Exit Sub
            End If

            paoRep.SaveXMLFile("印刷データファイル.prepe") '印刷データの保存

            'プレビューオブジェクトのインスタンスを獲得しなおし(一旦初期化)
            paoRep = ReportCreator.GetPreview()

            paoRep.LoadXMLFile("印刷データファイル.prepe") '印刷データの読み込み

            paoRep.Output() ' プレビューを実行

        End Sub

        '*** 共通処理をメソッド化

        ' 指定された種類のファイル保存ダイアログを開き
        ' 確定した場合保存ファイル名(フルパス)を返す。確定しない場合空文字を返す。
        Private Function ShowSaveDialog(type As String) As String
            Dim dlg As New SaveFileDialog()
            dlg.FileName = "印刷データ"
            dlg.DefaultExt = "." & type.ToLower()
            dlg.Filter = type.ToUpper() & " Document (." & type.ToLower() & ")|*." & type.ToLower() ' Filter files by extension
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

            ' Show save file dialog box
            Dim result As DialogResult = dlg.ShowDialog()

            ' Process save file dialog box results
            If result = DialogResult.OK Then
                Return dlg.FileName
            End If
            Return ""
        End Function

        ' 保存したファイルの起動
        Private Sub OpenSaveFile(filePath As String)
            Dim type As String = Path.GetExtension(filePath)?.TrimStart("."c).ToUpperInvariant()

            If MessageBox.Show("保存した " & type & " を表示しますか？", type & " の表示", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Dim startInfo As New System.Diagnostics.ProcessStartInfo(filePath)
                startInfo.UseShellExecute = True
                System.Diagnostics.Process.Start(startInfo)
            End If
        End Sub


    End Class
End Namespace
