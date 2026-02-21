using System;
using System.Windows.Forms;
using Pao.Reports;
using System.IO;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic.Devices;


namespace Sample
{
    public partial class Form1 : Form
    {
        private string sharePath_;

        public Form1()
        {
            InitializeComponent();

            // VB.NET との共有リソースパス取得
            sharePath_ = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() + "/../../../../../");

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            //IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            IReport? paoRep = null;

            // ■インスタンスの生成がエラーとなる場合の対処法：下記コメントを外してください
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if (radPreview.Checked) //ラジオボタンでプレビューが選択されている場合
            {
                //プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf();
            }
            else if (radPrint.Checked) // 印刷が選択されている場合
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }
            else if (radGetPrintDocument.Checked) //ラジオボタンで独自プレビュー(GetPrintDocument取得)が選択されている場合
            {

                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();

                // ↑ OR ↓(どちらでも可)  

                //プレビューオブジェクトのインスタンスを獲得
                //paoRep = ReportCreator.GetPreview();


            }
            else if (radPDF.Checked) // PDFが選択されている場合
            {
                //PDF出力オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPdf();
            }
            else //SVG / XPS 出力が選択されている場合
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

            if (radPreview.Checked || radPrint.Checked) //印刷・プレビューが選択されている場合
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
                paoRep.z_PreviewWindowWpf.z_Icon = new Icon(sharePath_ + "PreView.ico");

                // (初期)プレビュー表示倍率
                paoRep.ZoomPreview = 77;

                // バージョンウィンドウの情報変更
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName = "御社製品名";
                paoRep.z_PreviewWindowWpf.z_VersionWindow.ProductName_ForeColor = Color.Blue;

                MessageBox.Show("ページ数 : " + paoRep.AllPages.ToString());

                paoRep.Output(); // 印刷又はプレビューを実行
            }
            else if (radGetPrintDocument.Checked) // 独自プレビュー(PrrintDocument取得)が選択されている場合
            {
                // PrintDocument 取得
                printDocument1 = paoRep.GetPrintDocument();

                // このフォームのプレビューコントロールへ プレビュー実行
                prevWinForm.Document = printDocument1;
                prevWinForm.InvalidatePreview();

                // ここでは、抜けることにします。(印刷データの保存・読み込み・プレビューはしない)
                return;

            }
            else if (radPDF.Checked) //PDF出力が選択されている場合
            {

                //PDF出力
                saveFileDialog.FileName = "印刷データ";
                saveFileDialog.Filter = "PDF形式 (*.pdf)|*.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    paoRep.SavePDF(saveFileDialog.FileName); //印刷データの保存

                    if (MessageBox.Show(this, "PDFを表示しますか？", "PDF の表示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName);
                        startInfo.UseShellExecute = true;
                        System.Diagnostics.Process.Start(startInfo);
                    }
                }

            }
            else if (radSVG.Checked) //SVG出力が選択されている場合
            {
                saveFileDialog.FileName = "印刷データ";
                saveFileDialog.Filter = "html形式 (*.html)|*.html";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // インラインSVG埋め込みHTML文字列を取得し、ファイルに保存
                    string svgHtml = paoRep.GetSvg();
                    System.IO.File.WriteAllText(saveFileDialog.FileName, svgHtml, System.Text.Encoding.UTF8);

                    if (MessageBox.Show(this, "ブラウザで表示しますか？", "SVG の表示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName);
                        startInfo.UseShellExecute = true;
                        System.Diagnostics.Process.Start(startInfo);
                    }
                }

            }

            else if (radXPS.Checked) //XPS出力が選択されている場合
            {
                saveFileDialog.FileName = "印刷データ";
                saveFileDialog.Filter = "XPS形式 (*.xps)|*.xps";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    paoRep.SaveXPS(saveFileDialog.FileName); // XPSデータの保存
                    if (MessageBox.Show(this, "XPSを表示しますか？", "XPS の表示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName);
                        startInfo.UseShellExecute = true;
                        System.Diagnostics.Process.Start(startInfo);
                    }
                }

            }

            //マニュアル・ヘルプにはありませんが付け加えました。
            if (MessageBox.Show(this, "続いて、印刷データXMLファイルを保存して再度読み込んでプレビューを行います。", "Save And Reload Print Data", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            paoRep.SaveXMLFile("印刷データ.prepe"); //印刷データの保存

            //プレビューオブジェクトのインスタンスを獲得しなおし(一旦初期化)
            paoRep = ReportCreator.GetPreview();

            paoRep.LoadXMLFile("印刷データ.prepe"); //印刷データの読み込み

            paoRep.Output(); // プレビューを実行
        }

    }
}
