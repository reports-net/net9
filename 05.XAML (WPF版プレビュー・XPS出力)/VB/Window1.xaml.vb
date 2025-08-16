Imports System.Text
Imports Pao.Reports


Namespace Sample

    Class Window1

        Dim sharePath_ As String

        Public Sub New()

            MyBase.New()

            ' Reports.net のバーコードのエンコーディングの一部でSHIT_JISをサポートしているため
            ' この１行が必要
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

            InitializeComponent()

            ' VB.NET との共有リソースパス取得
            sharePath_ = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() + "/../../../")

        End Sub

        Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim PaoRep As IReport

            '■インスタンスの生成がエラーとなる場合の対処法下記コメントを外してください
            'System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

            If radPreview_WPF.IsChecked = True Then ' WPFプレビュー が選択されている場合
                'プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf()
            ElseIf radPrint.IsChecked = True Or radPreview_WPF.IsChecked = True Or radXPS.IsChecked = True Then ' 印刷、又は、旧WPFプレビュー、XPS出力 が選択されている場合
                'プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreview()
            ElseIf radPreview.IsChecked = True Then 'ラジオボタンでプレビューが選択されている場合
                'プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreview()
            ElseIf radPrint.IsChecked = True Or radXPS.IsChecked = True Then ' 印刷、又は、XPS出力 が選択されている場合
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
            If radPreview_WPF_XPS.IsChecked = True Then  '旧XAP出力後のWPF版プレビューが選択されている場合

                paoRep.WpfPreview(documentViewer) ' 印刷又はプレビューを実行

            ElseIf radPreview_WPF.IsChecked = True _
Or radPreview.IsChecked = True _
            Or radPrint.IsChecked = True Then 'WPF版プレビュー・印刷・プレビューが選択されている場合

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
                If radPreview_WPF.IsChecked = True Then 'WPF版プレビュー
                    paoRep.z_PreviewWindowWpf().z_TitleText = "10の倍数の印刷プレビュー"
                ElseIf radPreview.IsChecked = True Then 'Windows Form版プレビュー
                    paoRep.z_PreviewWindow().z_TitleText = "10の倍数の印刷プレビュー"
                End If

                paoRep.Output() '印刷/プレビューを実行

            ElseIf radPDF.IsChecked = True Then  'PDF出力が選択されている場合

                ' ファイル保存ダイアログ
                Dim dlg As Microsoft.Win32.SaveFileDialog = New Microsoft.Win32.SaveFileDialog()
                dlg.FileName = "印刷データ"
                dlg.DefaultExt = ".pdf"
                dlg.Filter = "PDF documents (.pdf)|*.pdf" ' Filter files by extension

                ' Show save file dialog box
                Dim result As Nullable(Of Boolean) = dlg.ShowDialog()

                ' Process save file dialog box results
                If result = True Then
                    paoRep.SavePDF(dlg.FileName) 'PDF保存

                    If MessageBox.Show(Me, "PDFを表示しますか？", "PDF の表示", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                        System.Diagnostics.Process.Start(dlg.FileName)
                    End If
                End If

            ElseIf radXPS.IsChecked = True Then  'XPS出力が選択されている場合

                ' ファイル保存ダイアログ
                Dim dlg As Microsoft.Win32.SaveFileDialog = New Microsoft.Win32.SaveFileDialog()
                dlg.FileName = "印刷データ"
                dlg.DefaultExt = ".xps"
                dlg.Filter = "Microsoft XPS Document (.xps)|*.xps" ' Filter files by extension

                ' Show save file dialog box
                Dim result As Nullable(Of Boolean) = dlg.ShowDialog()

                ' Process save file dialog box results
                If result = True Then
                    paoRep.SaveXPS(dlg.FileName) 'XPS保存

                    If MessageBox.Show(Me, "XPSを表示しますか？", "XPS の表示", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then
                        System.Diagnostics.Process.Start(dlg.FileName)
                    End If
                End If

            Else 'SVG / SVGZ出力が選択されている場合

                ' ファイル保存ダイアログ
                Dim dlg As Microsoft.Win32.SaveFileDialog = New Microsoft.Win32.SaveFileDialog()
                dlg.FileName = "印刷データ"
                dlg.DefaultExt = ".html"
                dlg.Filter = "html Document (*.html)|*.htmls" ' Filter files by extension

                ' Show save file dialog box
                Dim result As Nullable(Of Boolean) = dlg.ShowDialog()

                ' Process save file dialog box results
                If result = True Then
                    paoRep.SaveSVGFile(dlg.FileName) 'SVG保存

                    If MessageBox.Show(Me, "ブラウザで表示しますか？" & vbCrLf & "表示する場合、SVGプラグインが必要です。" _
                                       , "SVG / SVGZ の表示", MessageBoxButton.YesNo) = MessageBoxResult.Yes Then

                        System.Diagnostics.Process.Start(dlg.FileName)

                    End If
                End If

            End If


        End Sub

        Private Sub Hyperlink_RequestNavigate(sender As Object, e As System.Windows.Navigation.RequestNavigateEventArgs)
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo With {
                .FileName = e.Uri.AbsoluteUri,
                .UseShellExecute = True
            })
            e.Handled = True
        End Sub



    End Class

End Namespace

