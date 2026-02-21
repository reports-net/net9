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
        private System.Drawing.Printing.PrintDocument printDocument1 = null;

        private string sharePath_;

        public Window1()
        {

            InitializeComponent();

            this.Height = SystemParameters.PrimaryScreenHeight - 50;

            // VB.NET との共有リソースパス取得
            sharePath_ = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() + "/../../../../../");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            IReport paoRep;

            // ■インスタンスの生成がエラーとなる場合の対処法：下記コメントを外してください
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

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
                //PDF出力オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPdf();
            }
            else
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }


            //レポート定義ファイルの読み込み
            paoRep.LoadDefFile(sharePath_ + "レポート定義ファイル.prepd");


            int page = 0; //頁数を定義
            int line = 0; //行数を定義

            for (int i = 0; i < 60; i++)
            {
                if (i % 15 == 0) //1頁15行で開始
                {
                    //頁開始を宣言
                    paoRep.PageStart();
                    page++;		//頁数をインクリメント
                    line = 0;	//行数を初期化

                    //＊＊＊ヘッダのセット＊＊＊
                    //文字列のセット
                    paoRep.Write("日付", System.DateTime.Now.ToString());
                    paoRep.Write("頁数", "Page - " + page.ToString());

                    //オブジェクトの属性変更
                    paoRep.z_Objects.SetObject("フォントサイズ");
                    paoRep.z_Objects.z_Text.z_FontAttr.Size = 12;
                    paoRep.Write("フォントサイズ", "フォントサイズ" + Environment.NewLine + " 変更後");

                    if (page == 2)
                        paoRep.Write("Line3", "");　 //２頁目の線をを消す

                }
                line++; //行数をインクリメント

                //＊＊＊明細のセット＊＊＊
                //繰返し文字列のセット
                paoRep.Write("行番号", (i + 1).ToString(), line);
                paoRep.Write("10倍数", ((i + 1) * 10).ToString(), line);
                //繰返し図形(横線)のセット
                paoRep.Write("横線", line);

                if (((i + 1) % 15) == 0) paoRep.PageEnd(); //1頁15行で終了宣言
            }

            if (radPreview.IsChecked == true || radPrint.IsChecked == true) //印刷・プレビューが選択されている場合
            {
                //オマケのコメントです。m(_ _;)m 印刷の設定を色々試してみてください。m(_ _)m
                //System.Drawing.Printing.PrinterSettings setting = new System.Drawing.Printing.PrinterSettings();
                //setting.PrinterName = "Acrobat Distiller";
                //setting.FromPage    = 1;
                //setting.ToPage      = 5;
                //setting.MinimumPage = 2;
                //setting.MaximumPage = 3;
                //		
                paoRep.DisplayDialog = true;
                //
                //paoRep.Output(setting); // 印刷又はプレビューを実行

                // ドキュメント名
                paoRep.DocumentName = "10の倍数の印刷ドキュメント";

                // プレビューウィンドウタイトル
                paoRep.z_PreviewWindowWpf.z_TitleText = "10の倍数の印刷プレビュー";

                // プレビューウィンドウアイコン
                paoRep.z_PreviewWindowWpf.z_Icon = new System.Drawing.Icon(sharePath_ + "PreView.ico");

                // (初期)プレビュー表示倍率
                paoRep.ZoomPreview = 77;

                // バージョンウィンドウの情報変更
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName = "御社製品名";
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName_ForeColor = System.Drawing.Color.Blue;

                MessageBox.Show("ページ数 : " + paoRep.AllPages.ToString());

                paoRep.Output(); // 印刷又はプレビューを実行
            }
            else if (radGetPrintDocument.IsChecked == true) // 独自プレビュー(PrrintDocument取得)が選択されている場合
            {
                // PrintDocument 取得
                printDocument1 = paoRep.GetPrintDocument();

                // このフォームのプレビューコントロールへ プレビュー実行
                prevWinForm.Document = printDocument1;
                prevWinForm.InvalidatePreview();

                // ここでは、抜けることにします。(印刷データの保存・読み込み・プレビューはしない)
                return;

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

            //マニュアル・ヘルプにはありませんが付け加えました。
            if (MessageBox.Show(this,
                "続いて、印刷データXMLファイルを保存して再度読み込んでプレビューを行います。",
                "Save And Reload Print Data",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question,
                MessageBoxResult.OK,
                MessageBoxOptions.None
                ) == System.Windows.MessageBoxResult.Cancel)
            {
                return;
            }

            paoRep.SaveXMLFile("印刷データ.prepe"); //印刷データの保存

            //プレビューオブジェクトのインスタンスを獲得しなおし(一旦初期化)
            paoRep = ReportCreator.GetPreview();

            paoRep.LoadXMLFile("印刷データ.prepe"); //印刷データの読み込み

            paoRep.Output(); // プレビューを実行

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
 
    }
}
