Imports System
Imports System.Text
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Threading
Imports Pao.Reports

Namespace Sample
    ''' <summary>
    ''' Window1.xaml の相互作用ロジック
    ''' </summary>
    Partial Public Class Window1
        Inherits Window
        
        Private printDocument1 As System.Drawing.Printing.PrintDocument = Nothing
        Private sharePath_ As String

        Public Sub New()
            InitializeComponent()

            Me.Height = SystemParameters.PrimaryScreenHeight - 50

            ' C# との共有リソースパス取得
            sharePath_ = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() + "/../../../../../")

        End Sub

        Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
            'IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            Dim paoRep As IReport
            
            '■インスタンスの生成がエラーとなる場合の対処法下記コメントを外してください
            'System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

            If radPreview.IsChecked = True Then ' WPFプレビュー が選択されている場合
                'WPF版プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf()
            ElseIf radPrint.IsChecked = True OrElse radXPS.IsChecked = True Then ' 印刷、又は、XPS出力 が選択されている場合
                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()
            ElseIf radPDF.IsChecked = True Then ' PDFが選択されている場合
                'PDF出力オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPdf()
            Else
                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()
            End If
            
            'レポート定義ファイルの読み込み
            paoRep.LoadDefFile(sharePath_ + "レポート定義ファイル.prepd")
            
            Dim page As Integer = 0 '頁数を定義
            Dim line As Integer = 0 '行数を定義
            
            For i As Integer = 0 To 59
                If i Mod 15 = 0 Then '1頁15行で開始
                    '頁開始を宣言
                    paoRep.PageStart()
                    page += 1 '頁数をインクリメント
                    line = 0 '行数を初期化
                    
                    '＊＊＊ヘッダのセット＊＊＊
                    '文字列のセット
                    paoRep.Write("日付", System.DateTime.Now.ToString())
                    paoRep.Write("頁数", "Page - " & page.ToString())
                    
                    'オブジェクトの属性変更
                    paoRep.z_Objects.SetObject("フォントサイズ")
                    paoRep.z_Objects.z_Text.z_FontAttr.Size = 12
                    paoRep.Write("フォントサイズ", "フォントサイズ" & Environment.NewLine & " 変更後")
                    
                    If page = 2 Then
                        paoRep.Write("Line3", "") '２頁目の線をを消す
                    End If
                End If
                
                line += 1 '行数をインクリメント
                
                '＊＊＊明細のセット＊＊＊
                '繰返し文字列のセット
                paoRep.Write("行番号", (i + 1).ToString(), line)
                paoRep.Write("10倍数", ((i + 1) * 10).ToString(), line)
                '繰返し図形(横線)のセット
                paoRep.Write("横線", line)
                
                If ((i + 1) Mod 15) = 0 Then paoRep.PageEnd() '1頁15行で終了宣言
            Next

            If radPreview.IsChecked = True OrElse radPrint.IsChecked = True Then '印刷・プレビューが選択されている場合
                'オマケのコメントです。m(_ _;)m 印刷の設定を色々試してみてください。m(_ _)m
                'Dim setting As New System.Drawing.Printing.PrinterSettings()
                'setting.PrinterName = "Acrobat Distiller"
                'setting.FromPage    = 1
                'setting.ToPage      = 5
                'setting.MinimumPage = 2
                'setting.MaximumPage = 3
                '       
                paoRep.DisplayDialog = True
                '
                'paoRep.Output(setting) ' 印刷又はプレビューを実行

                ' ドキュメント名
                paoRep.DocumentName = "10の倍数の印刷ドキュメント"

                ' プレビューウィンドウタイトル
                paoRep.z_PreviewWindowWpf.z_TitleText = "10の倍数の印刷プレビュー"

                ' プレビューウィンドウアイコン
                paoRep.z_PreviewWindowWpf.z_Icon = New System.Drawing.Icon(sharePath_ + "PreView.ico")

                ' (初期)プレビュー表示倍率
                paoRep.ZoomPreview = 77

                ' バージョンウィンドウの情報変更
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName = "御社製品名"
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName_ForeColor = System.Drawing.Color.Blue

                MessageBox.Show("ページ数 : " & paoRep.AllPages.ToString())

                paoRep.Output() ' 印刷又はプレビューを実行

            ElseIf radGetPrintDocument.IsChecked = True Then ' 独自プレビュー(PrrintDocument取得)が選択されている場合
                ' PrintDocument 取得
                printDocument1 = paoRep.GetPrintDocument()

                ' このフォームのプレビューコントロールへ プレビュー実行
                prevWinForm.Document = printDocument1
                prevWinForm.InvalidatePreview()

                ' ここでは、抜けることにします。(印刷データの保存・読み込み・プレビューはしない)
                Return

            ElseIf radPDF.IsChecked = True Then 'PDF出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("pdf") ' PDFファイル保存ダイアログ
                If saveFileName = "" Then Return

                'PDFの保存
                paoRep.SavePDF(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したPDFファイルを開く
            ElseIf radSVG.IsChecked = True Then 'SVG出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("html") ' SVGファイル保存ダイアログ
                If saveFileName = "" Then Return

                ' インラインSVG埋め込みHTML文字列を取得し、ファイルに保存
                Dim svgHtml As String = paoRep.GetSvg()
                System.IO.File.WriteAllText(saveFileName, svgHtml, System.Text.Encoding.UTF8)

                OpenSaveFile(saveFileName) ' 保存したSVGファイルを開く
            ElseIf radXPS.IsChecked = True Then 'XPS出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("xps") ' XPSファイル保存ダイアログ
                If saveFileName = "" Then Return

                'XPS印刷データの保存
                paoRep.SaveXPS(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したXPSファイルを開く
            End If


            'マニュアル・ヘルプにはありませんが付け加えました。
            If MessageBox.Show(Me, _
                "続いて、印刷データXMLファイルを保存して再度読み込んでプレビューを行います。", _
                "Save And Reload Print Data", _
                MessageBoxButton.OKCancel, _
                MessageBoxImage.Question, _
                MessageBoxResult.OK, _
                MessageBoxOptions.None _
                ) = System.Windows.MessageBoxResult.Cancel Then
                
                Return
            End If
            
            paoRep.SaveXMLFile("印刷データ.prepe") '印刷データの保存
            
            'プレビューオブジェクトのインスタンスを獲得しなおし(一旦初期化)
            paoRep = ReportCreator.GetPreview()
            
            paoRep.LoadXMLFile("印刷データ.prepe") '印刷データの読み込み
            
            paoRep.Output() ' プレビューを実行
        End Sub

        Private Sub Hyperlink_RequestNavigate(sender As Object, e As System.Windows.Navigation.RequestNavigateEventArgs)
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo With {
                .FileName = e.Uri.AbsoluteUri,
                .UseShellExecute = True
            })
            e.Handled = True
        End Sub

        ' *** 以下共通処理をメソッド化

        ' 指定された種類のファイル保存ダイアログを開き
        ' 確定した場合保存ファイル名(フルパス)を返す。確定しない場合空文字を返す。
        Private Function ShowSaveDialog(type As String) As String
            Dim dlg As New Microsoft.Win32.SaveFileDialog()
            dlg.FileName = "印刷データ"
            dlg.DefaultExt = "." & type.ToLower()
            dlg.Filter = type.ToUpper() & " Document (." & type.ToLower() & ")|*." & type.ToLower() ' Filter files by extension
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

            ' Show save file dialog box
            Dim result As Nullable(Of Boolean) = dlg.ShowDialog()

            ' Process save file dialog box results
            If result = True Then
                Return dlg.FileName
            End If

            Return ""
        End Function

        ' 保存したファイルの起動
        Private Sub OpenSaveFile(filePath As String)
            Dim type As String = System.IO.Path.GetExtension(filePath)?.TrimStart("."c).ToUpperInvariant()

            If MessageBox.Show("保存した " & type & " を表示しますか？", type & " の表示", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                Dim startInfo = New System.Diagnostics.ProcessStartInfo(filePath)
                startInfo.UseShellExecute = True
                System.Diagnostics.Process.Start(startInfo)
            End If
        End Sub


    End Class
End Namespace