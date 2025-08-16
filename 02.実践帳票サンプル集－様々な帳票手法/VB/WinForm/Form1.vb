Imports Pao.Reports
Imports System.IO

Namespace Sample
    Partial Public Class Form1
        Inherits Form


        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Me.Top = 5
            'Me.Height = Screen.PrimaryScreen.WorkingArea.Height - 50

            ' VB.NET との共有リソースパス(Util.SharePath)設定
            Util.SetSharePath()

            ' サンプル帳票初期値セット
            cmbReportType.SelectedIndex = 0
        End Sub


        Private dataLoaded As Boolean = False

        Private Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click
            'IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            Dim paoRep As IReport

            '■インスタンスの生成がエラーとなる場合の対処法下記コメントを外してください
            'System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

            ' *** 出力別にインスタンス生成 ***

            If radPreview.Checked Then ' WPFプレビュー が選択されている場合
                'WPF版プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf()
            ElseIf radPrint.Checked OrElse radXPS.Checked Then ' 印刷、又は、XPS出力 が選択されている場合
                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()
            ElseIf radPDF.Checked Then ' PDFが選択されている場合
                If cmbReportType.Text.Contains("広告") Then
                    'イメージPDF出力オブジェクトのインスタンスを獲得
                    paoRep = ReportCreator.GetImagePdf()
                Else
                    'PDF出力オブジェクトのインスタンスを獲得
                    paoRep = ReportCreator.GetPdf()
                End If
            Else
                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()
            End If

            ' **** 帳票別に帳票データをセット ***

            If cmbReportType.Text.Contains("郵便番号一覧") Then
                Make郵便番号一覧.SetupData(paoRep)
            ElseIf cmbReportType.Text.Contains("見積書") Then
                Make見積書.SetupData(paoRep)
            ElseIf cmbReportType.Text.Contains("請求書") Then
                Make請求書.SetupData(paoRep)
            ElseIf cmbReportType.Text.Contains("商品大小分類") Then
                Make商品大小分類.SetupData(paoRep)
            ElseIf cmbReportType.Text.Contains("デザイン変更") Then
                Dim defIndex As Integer = 0
                If radDesign2.Checked Then defIndex = 1

                If dataLoaded Then
                    ' デザインファイルのみ変更
                    paoRep.ChangeDefFile(Util.GetDefFile(defIndex))
                Else
                    Makeデザイン変更.SetupData(paoRep, defIndex)
                    dataLoaded = True
                End If
            ElseIf cmbReportType.Text.Contains("広告") Then
                Make広告.SetupData(paoRep)
            End If

            ' データ読み込み済みフラグをクリアしておかないと
            If Not cmbReportType.Text.Contains("デザイン変更") Then dataLoaded = False

            ' *** 各種出力 ***

            If radPreview.Checked OrElse radPrint.Checked Then '印刷・プレビューが選択されている場合
                paoRep.Output() ' 印刷又はプレビューを実行
            ElseIf radPDF.Checked Then 'PDF出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("pdf") ' PDFファイル保存ダイアログ
                If saveFileName = "" Then Return

                'PDFの保存
                paoRep.SavePDF(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したPDFファイルを開く
            ElseIf radSVG.Checked Then 'SVG出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("html") ' SVGファイル保存ダイアログ
                If saveFileName = "" Then Return

                'SVGデータの保存
                paoRep.SaveSVGFile(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したSVGファイルを開く
            ElseIf radXPS.Checked Then 'XPS出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("xps") ' XPSファイル保存ダイアログ
                If saveFileName = "" Then Return

                'XPS印刷データの保存
                paoRep.SaveXPS(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したXPSファイルを開く
            End If
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

        Private Sub cmbReportType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReportType.SelectedIndexChanged
            If cmbReportType.Text.Contains("デザイン変更") Then
                pnl.Top = grpDesign.Top + grpDesign.Height + 4
            Else
                pnl.Top = cmbReportType.Top + cmbReportType.Height + 4
            End If
        End Sub

        Private Sub richTextBox2_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles richTextBox2.LinkClicked
            Dim psi As New ProcessStartInfo With {
                .FileName = e.LinkText,
                .UseShellExecute = True
            }
            Process.Start(psi)
        End Sub
    End Class
End Namespace
