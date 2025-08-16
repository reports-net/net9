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

            ' C# との共有リソースパス(Util.SharePath)設定
            Util.SetSharePath()

            ' コンボボックスの選択変更イベントを登録
            AddHandler cmbReportType.SelectionChanged, AddressOf CmbReportType_SelectionChanged
        End Sub

        Private dataLoaded As Boolean = False

        Private Sub ExecuteButton_Click(sender As Object, e As RoutedEventArgs)
            'IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            Dim paoRep As IReport

            '■インスタンスの生成がエラーとなる場合の対処法下記コメントを外してください
            'System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

            ' *** 出力別にインスタンス生成 ***

            If radPreview.IsChecked = True Then ' WPFプレビュー が選択されている場合
                'WPF版プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf()
            ElseIf radPrint.IsChecked = True OrElse radXPS.IsChecked = True Then ' 印刷、又は、XPS出力 が選択されている場合
                '印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport()
            ElseIf radPDF.IsChecked = True Then ' PDFが選択されている場合
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
                If radDesign2.IsChecked = True Then defIndex = 1
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

            'データ読み込み済みフラグをクリアしておかないと
            If cmbReportType.Text.Contains("デザイン変更") Then dataLoaded = False

            ' *** 各種出力 ***

            If radPreview.IsChecked = True OrElse radPrint.IsChecked = True Then '印刷・プレビューが選択されている場合

                ' 印刷又はプレビューを実行
                paoRep.Output()

            ElseIf radPDF.IsChecked = True Then 'PDF出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("pdf") ' PDFファイル保存ダイアログ
                If saveFileName = "" Then Return

                'PDFの保存
                paoRep.SavePDF(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したPDFファイルを開く
            ElseIf radSVG.IsChecked = True Then 'SVG出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("html") ' SVGファイル保存ダイアログ
                If saveFileName = "" Then Return

                'SVGデータの保存
                paoRep.SaveSVGFile(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したSVGファイルを開く
            ElseIf radXPS.IsChecked = True Then 'XPS出力が選択されている場合
                Dim saveFileName As String = ShowSaveDialog("xps") ' XPSファイル保存ダイアログ
                If saveFileName = "" Then Return

                'XPS印刷データの保存
                paoRep.SaveXPS(saveFileName)

                OpenSaveFile(saveFileName) ' 保存したXPSファイルを開く
            End If
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

        Private Sub Hyperlink_RequestNavigate(sender As Object, e As System.Windows.Navigation.RequestNavigateEventArgs)
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo With {
                .FileName = e.Uri.AbsoluteUri,
                .UseShellExecute = True
            })
            e.Handled = True
        End Sub
        Private Sub CmbReportType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            ' ComboBoxItemからContentを取得
            Dim selectedItem As ComboBoxItem = TryCast(cmbReportType.SelectedItem, ComboBoxItem)
            If selectedItem IsNot Nothing Then
                Dim content As String = selectedItem.Content.ToString()

                ' 「いつでもデザイン変更」が選択された場合のみデザイン選択表示
                If content.Contains("いつでもデザイン変更") Then
                    borderDesignSelection.Visibility = Visibility.Visible
                Else
                    borderDesignSelection.Visibility = Visibility.Collapsed
                End If
            End If
        End Sub

    End Class
End Namespace