using System;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Pao.Reports;

namespace Sample
{


    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {

            InitializeComponent();

            this.Height = SystemParameters.PrimaryScreenHeight - 50;

            // VB.NET との共有リソースパス(Util.SharePath)設定
            Util.SetSharePath();

            // コンボボックスの選択変更イベントを登録
            cmbReportType.SelectionChanged += CmbReportType_SelectionChanged;

        }

        private bool dataLoaded = false;

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {

            //IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            IReport paoRep;

            // ■インスタンスの生成がエラーとなる場合の対処法：下記コメントを外してください
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // *** 出力別にインスタンス生成 ***

            if (radPreview.IsChecked == true) // WPFプレビュー が選択されている場合
            {
                //WPF版プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf();
            }
            else if (radPrint.IsChecked == true || radXPS.IsChecked == true) // 印刷、又は、XPS出力 が選択されている場合
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }
            else if (radPDF.IsChecked == true) // PDFが選択されている場合
            {
                if (cmbReportType.Text.Contains("広告"))
                {
                    //イメージPDF出力オブジェクトのインスタンスを獲得
                    paoRep = ReportCreator.GetImagePdf();
                }
                else
                {
                    //PDF出力オブジェクトのインスタンスを獲得
                    paoRep = ReportCreator.GetPdf();
                }
            }
            else
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }

            // **** 帳票別に帳票データをセット ***
 
            if (cmbReportType.Text.Contains("郵便番号一覧"))
            {
                Make郵便番号一覧.SetupData(paoRep);
            }
            else if (cmbReportType.Text.Contains("見積書"))
            {
                Make見積書.SetupData(paoRep);
            }
            else if (cmbReportType.Text.Contains("請求書"))
            {
                Make請求書.SetupData(paoRep);
            }
            else if (cmbReportType.Text.Contains("商品大小分類"))
            {
                Make商品大小分類.SetupData(paoRep);
            }
            else if (cmbReportType.Text.Contains("デザイン変更"))
            {
                int defIndex = 0;
                if (radDesign2.IsChecked == true) defIndex = 1;

                if (dataLoaded)
                {
                    // デザインファイルのみ変更
                    paoRep.ChangeDefFile(Util.GetDefFile(defIndex));
                }
                else
                {
                    Makeデザイン変更.SetupData(paoRep, defIndex);
                    dataLoaded = true;
                }
            }
            else if (cmbReportType.Text.Contains("広告"))
            {
                Make広告.SetupData(paoRep);
            }

            // データ読み込み済みフラグをクリアしておかないと
            if (!cmbReportType.Text.Contains("デザイン変更")) dataLoaded = false;


                // *** 各種出力 ***

                if (radPreview.IsChecked == true || radPrint.IsChecked == true) //印刷・プレビューが選択されている場合
            {
                paoRep.Output(); // 印刷又はプレビューを実行
            }
            else if (radPDF.IsChecked == true) //PDF出力が選択されている場合
            {

                string saveFileName = ShowSaveDialog("pdf");　// PDFファイル保存ダイアログ
                if (saveFileName == "") return;

                //PDFの保存
                paoRep.SavePDF(saveFileName);

                OpenSaveFile(saveFileName); // 保存したPDFファイルを開く
            }
            else if (radSVG.IsChecked == true) //SVG出力が選択されている場合
            {
                string saveFileName = ShowSaveDialog("html");　// SVGファイル保存ダイアログ
                if (saveFileName == "") return;

                // インラインSVG埋め込みHTML文字列を取得し、ファイルに保存
                string svgHtml = paoRep.GetSvg();
                System.IO.File.WriteAllText(saveFileName, svgHtml, System.Text.Encoding.UTF8);

                OpenSaveFile(saveFileName); // 保存したSVGファイルを開く
            }

            else if (radXPS.IsChecked == true) //XPS出力が選択されている場合
            {
                string saveFileName = ShowSaveDialog("xps");　// XPSファイル保存ダイアログ
                if (saveFileName == "") return;

                //XPS印刷データの保存
                paoRep.SaveXPS(saveFileName);

                OpenSaveFile(saveFileName); // 保存したXPSファイルを開く

            }

        }

        //*** 共通処理をメソッド化

        // 指定された種類のファイル保存ダイアログを開き
        // 確定した場合保存ファイル名(フルパス)を返す。確定しない場合空文字を返す。
        private string ShowSaveDialog(string type)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "印刷データ";
            dlg.DefaultExt = "." + type.ToLower();
            dlg.Filter = type.ToUpper() + " Document (." + type.ToLower() + ")|*." + type.ToLower(); // Filter files by extension
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                return dlg.FileName;
            }
            return "";
        }

        // 保存したファイルの起動
        private void OpenSaveFile(string filePath)
        {
            string type = System.IO.Path.GetExtension(filePath)?.TrimStart('.').ToUpperInvariant();

            if (MessageBox.Show("保存した " + type + " を表示しますか？", type + " の表示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var startInfo = new System.Diagnostics.ProcessStartInfo(filePath);
                startInfo.UseShellExecute = true;
                System.Diagnostics.Process.Start(startInfo);
            }

        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            });
            e.Handled = true;
        }

        private void CmbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ComboBoxItemからContentを取得
            ComboBoxItem selectedItem = cmbReportType.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string content = selectedItem.Content.ToString();

                // 「いつでもデザイン変更」が選択された場合のみデザイン選択表示
                if (content.Contains("いつでもデザイン変更"))
                {
                    borderDesignSelection.Visibility = Visibility.Visible;
                }
                else
                {
                    borderDesignSelection.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
