using System;
using System.Collections.Generic;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop; 
using System.Runtime.InteropServices;

using System.IO;
using System.IO.Packaging;
//using System.Windows.Xps.Packaging;

using System.Threading;
using System.Windows.Threading;
using System.Reflection;

using Pao.Reports;

namespace Sample
{


    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {

        string sharePath_;
        public Window1()
        {
            // Reports.net のバーコードのエンコーディングの一部でSHIT_JISをサポートしているため
            // この１行が必要
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            InitializeComponent();

            // VB.NET との共有リソースパス取得
            sharePath_ = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() + "/../../../");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            IReport paoRep = null;

            // ■インスタンスの生成がエラーとなる場合の対処法：下記コメントを外してください
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if (radPreview_WPF.IsChecked == true) // WPFプレビュー が選択されている場合
            {
                //WPF版プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf();

                // プレビューウィンドウのアイコン・タイトルも変更できます。
                //paoRep.z_PreviewWindowWpf.z_Icon = new System.Drawing.Icon("./PreviewCustom.ico");
                //paoRep.z_PreviewWindowWpf.z_TitleText = "カスタムプレビュー画面";
            }
            else if (radPrint.IsChecked == true || radPreview_WPF_XPS.IsChecked == true || radXPS.IsChecked == true) // 印刷、又は、旧WPFプレビュー、XPS出力 が選択されている場合
            {
                //プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreview();
            }
            else if (radPreview.IsChecked == true) //ラジオボタンでプレビューが選択されている場合
            {
                //プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreview();
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

            if (radPreview_WPF_XPS.IsChecked == true) //旧XAP出力後のWPF版プレビューが選択されている場合
            {
                paoRep.WpfPreview(documentViewer); // 印刷又はプレビューを実行
            }
            else if (radPreview_WPF.IsChecked == true // WPF版プレビュー
                || radPreview.IsChecked == true || radPrint.IsChecked == true) // 印刷・プレビューが選択されている場合
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

                //プレビューウィンドウタイトル
                if (radPreview_WPF.IsChecked == true) // WPF版プレビュー
                {
                    paoRep.z_PreviewWindowWpf.z_TitleText = "10の倍数の印刷プレビュー";
                }
                else if(radPreview_WPF.IsChecked == true) // Windows Form版プレビュー
                {
                    paoRep.z_PreviewWindow.z_TitleText = "10の倍数の印刷プレビュー";
                }

                // ドキュメント名
                paoRep.DocumentName = "10の倍数の印刷ドキュメント";

                MessageBox.Show("ページ数 : " + paoRep.AllPages.ToString());

                paoRep.Output(); // 印刷又はプレビューを実行
            }
            else if (radPDF.IsChecked == true) //PDF出力が選択されている場合
            {


                //PDF出力

                // ファイル保存ダイアログ
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "印刷データ";
                dlg.DefaultExt = ".pdf";
                dlg.Filter = "PDF documents (.pdf)|*.pdf"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    paoRep.SavePDF(dlg.FileName); //印刷データの保存

                    if (MessageBox.Show(this, "PDFを表示しますか？", "PDF の表示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(dlg.FileName);
                    }
                }

            }
            else if (radXPS.IsChecked == true) //XPS出力が選択されている場合
            {

                // ファイル保存ダイアログ
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "印刷データ";
                dlg.DefaultExt = ".xps";
                dlg.Filter = "Microsoft XPS Document (.xps)|*.xps"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    paoRep.SaveXPS(dlg.FileName); //印刷データの保存

                    if (MessageBox.Show(this, "XPSを表示しますか？", "XPS の表示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(dlg.FileName);
                    }
                }

            }
            else //SVG出力
            {
                // ファイル保存ダイアログ
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "印刷データ";
                dlg.DefaultExt = ".html";
                dlg.Filter = "html Document (*.html)|*.htmls"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {

                    // インラインSVG埋め込みHTML文字列を取得し、ファイルに保存
                    string svgHtml = paoRep.GetSvg();
                    System.IO.File.WriteAllText(dlg.FileName, svgHtml, System.Text.Encoding.UTF8);

                    if (MessageBox.Show(this, "ブラウザで表示しますか？\n表示する場合、SVGプラグインが必要です。", "SVG / SVGZ の表示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(dlg.FileName);
                    }
                }

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


    }
}
