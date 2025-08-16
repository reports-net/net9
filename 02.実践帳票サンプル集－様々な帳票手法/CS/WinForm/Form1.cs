using System;
using System.Windows.Forms;
using Pao.Reports;
using System.IO;


namespace Sample
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();

            // VB.NET との共有リソースパス(Util.SharePath)設定
            Util.SetSharePath();

            // サンプル帳票初期値セット
            cmbReportType.SelectedIndex = 0;

        }

        private bool dataLoaded = false;

        private void btnExecute_Click(object sender, EventArgs e)
        {
            //IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            IReport paoRep;

            // ■インスタンスの生成がエラーとなる場合の対処法：下記コメントを外してください
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // *** 出力別にインスタンス生成 ***

            if (radPreview.Checked) // WPFプレビュー が選択されている場合
            {
                //WPF版プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreviewWpf();
            }
            else if (radPrint.Checked || radXPS.Checked) // 印刷、又は、XPS出力 が選択されている場合
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }
            else if (radPDF.Checked) // PDFが選択されている場合
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
                if (radDesign2.Checked) defIndex = 1;

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

            if (radPreview.Checked || radPrint.Checked) //印刷・プレビューが選択されている場合
            {
                paoRep.Output(); // 印刷又はプレビューを実行
            }
            else if (radPDF.Checked) //PDF出力が選択されている場合
            {
                string saveFileName = ShowSaveDialog("pdf"); // PDFファイル保存ダイアログ
                if (saveFileName == "") return;

                //PDFの保存
                paoRep.SavePDF(saveFileName);

                OpenSaveFile(saveFileName); // 保存したPDFファイルを開く
            }
            else if (radSVG.Checked) //SVG出力が選択されている場合
            {
                string saveFileName = ShowSaveDialog("html"); // SVGファイル保存ダイアログ
                if (saveFileName == "") return;

                //SVGデータの保存
                paoRep.SaveSVGFile(saveFileName);

                OpenSaveFile(saveFileName); // 保存したSVGファイルを開く
            }
            else if (radXPS.Checked) //XPS出力が選択されている場合
            {
                string saveFileName = ShowSaveDialog("xps"); // XPSファイル保存ダイアログ
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
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "印刷データ";
            dlg.DefaultExt = "." + type.ToLower();
            dlg.Filter = type.ToUpper() + " Document (." + type.ToLower() + ")|*." + type.ToLower(); // Filter files by extension
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Show save file dialog box
            DialogResult result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == DialogResult.OK)
            {
                return dlg.FileName;
            }
            return "";
        }

        // 保存したファイルの起動
        private void OpenSaveFile(string filePath)
        {
            string type = Path.GetExtension(filePath)?.TrimStart('.').ToUpperInvariant();

            if (MessageBox.Show("保存した " + type + " を表示しますか？", type + " の表示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var startInfo = new System.Diagnostics.ProcessStartInfo(filePath);
                startInfo.UseShellExecute = true;
                System.Diagnostics.Process.Start(startInfo);
            }
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReportType.Text.Contains("デザイン変更"))
            {
                pnl.Top = grpDesign.Top + grpDesign.Height + 4;
            }
            else
            {
                pnl.Top = cmbReportType.Top + cmbReportType.Height + 4;
            }

        }

        private void richTextBox2_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.LinkText,
                UseShellExecute = true
            });
        }
    }
}
