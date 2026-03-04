using System;
using System.IO;
using System.Text;
using Pao.Reports;
#if ANDROID
using Android.Content;
using Android.Print;
#elif IOS
using UIKit;
using WebKit;
#endif

namespace SvgPreviewMobile
{
    /// <summary>
    /// Reports.net SVG/PDF 印刷プレビュー (スマホ・iPhone・タブレット対応)
    ///
    /// SVGモード: GetSvg() で全ページのSVGを含むHTMLを一括取得し WebView に表示。
    ///            印刷は各プラットフォームのネイティブ印刷APIを呼び出す。
    /// PDFモード: SavePDF() でPDFファイルを生成し、外部ビューアで開く。
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private IReport? report_;
        private int totalPages_ = 0;
        private bool isSvgMode_ = true;
        private string? pdfPath_;  // PDF モードで生成したファイルパスを保持

        // Resources/Raw/ → AppDataDirectory コピータスク
        private readonly Task resourcesTask_;

        public MainPage()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            InitializeComponent();

            // MAUI既知バグ: ItemsSource+SelectedIndex をXAMLで同時指定すると
            // SelectedIndex が反映されないため、コードビハインドで設定する
            pickerReport.SelectedIndex = 3; // 見積書

            // Resources/Raw/ のファイルをローカルストレージにコピー開始
            // （Android / iOS / Windows 共通。生成ボタン押下時に await する）
            resourcesTask_ = Util.CopyResourcesAsync();

            // 初期画面
            webView.Source = new HtmlWebViewSource { Html = BuildWelcomeHtml() };
        }

        // ──────────────────────────────────
        //  SVG / PDF 切替
        // ──────────────────────────────────
        private void BtnSvg_Clicked(object? sender, EventArgs e)
        {
            if (!isSvgMode_)
            {
                isSvgMode_ = true;
                UpdateFormatToggle();
            }
        }

        private void BtnPdf_Clicked(object? sender, EventArgs e)
        {
            if (isSvgMode_)
            {
                isSvgMode_ = false;
                UpdateFormatToggle();
            }
        }

        private void UpdateFormatToggle()
        {
            var active = Color.FromArgb("#0E639C");
            var inactive = Color.FromArgb("#3E3E42");
            var activeText = Colors.White;
            var inactiveText = Color.FromArgb("#999999");

            btnSvg.BackgroundColor = isSvgMode_ ? active : inactive;
            btnSvg.TextColor = isSvgMode_ ? activeText : inactiveText;
            btnPdf.BackgroundColor = isSvgMode_ ? inactive : active;
            btnPdf.TextColor = isSvgMode_ ? inactiveText : activeText;

            btnPrint.IsEnabled = false;
        }

        // ──────────────────────────────────
        //  帳票生成 → SVG: WebView表示 / PDF: 外部ビューア
        // ──────────────────────────────────
        private bool isGenerating_ = false;

        private async void BtnGenerate_Clicked(object? sender, EventArgs e)
        {
            // 多重実行ガード
            if (isGenerating_) return;
            isGenerating_ = true;
            btnGenerate.IsEnabled = false;

            try
            {
                // リソースコピーが完了するまで待つ
                await resourcesTask_;

                int selectedIndex = pickerReport.SelectedIndex;
                if (selectedIndex < 0)
                {
                    await DisplayAlert("帳票未選択", "帳票を選択してください。", "OK");
                    return;
                }
                string reportName = pickerReport.SelectedItem?.ToString() ?? "";
                string format = isSvgMode_ ? "SVG" : "PDF";
                lblStatus.Text = $"「{reportName}」{format} 生成中...";

                var report = isSvgMode_ ? ReportCreator.GetReport() : ReportCreator.GetPdf();

                // DB接続＋帳票データ設定＋SVG/PDF生成をバックグラウンドで実行
                // （UIスレッドのブロックを避ける）
                if (isSvgMode_)
                {
                    string html = await Task.Run(() =>
                    {
                        SetupReportData(report, selectedIndex);
                        return report.GetSvg();
                    });

                    report_ = report;
                    totalPages_ = report.AllPages;

                    // モバイル用: SVGを横幅にフィットさせ、ピンチズーム有効化
                    html = html.Replace("<head>",
                        "<head>\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=0.1, maximum-scale=5.0, user-scalable=yes\">" +
                        "\n<style>body{margin:0;padding:0} svg{display:block;width:100%;height:auto}</style>");

                    webView.Source = new HtmlWebViewSource { Html = html };

                    btnPrint.IsEnabled = true;
                    lblStatus.Text = $"「{reportName}」全 {totalPages_} ページを表示中";
                }
                else
                {
                    string pdfPath = Path.Combine(FileSystem.CacheDirectory, reportName + ".pdf");

                    await Task.Run(() =>
                    {
                        SetupReportData(report, selectedIndex);
                        using var fs = new FileStream(pdfPath, FileMode.Create);
                        report.SavePDF(fs);
                    });

                    report_ = report;
                    totalPages_ = report.AllPages;
                    pdfPath_ = pdfPath;

                    btnPrint.IsEnabled = true;
                    lblStatus.Text = $"「{reportName}」全 {totalPages_} ページの PDF を生成しました";

                    // 外部PDFビューアで開く
                    await Launcher.Default.OpenAsync(new OpenFileRequest
                    {
                        Title = reportName,
                        File = new ReadOnlyFile(pdfPath)
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("エラー", ex.ToString(), "OK");
                lblStatus.Text = "エラーが発生しました";
            }
            finally
            {
                isGenerating_ = false;
                btnGenerate.IsEnabled = true;
            }
        }

        private static void SetupReportData(IReport report, int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0: Make単純なサンプル.SetupData(report); break;
                case 1: Make10の倍数.SetupData(report); break;
                case 2: Make郵便番号一覧.SetupData(report); break;
                case 3: Make見積書.SetupData(report); break;
                case 4: Make請求書.SetupData(report); break;
                case 5: Make商品大小分類.SetupData(report); break;
            }
        }

        // ──────────────────────────────────
        //  印刷（SVG: ネイティブ印刷API / PDF: ファイル印刷）
        // ──────────────────────────────────
        private void BtnPrint_Clicked(object? sender, EventArgs e)
        {
            if (report_ == null) return;

            if (isSvgMode_)
                PrintWebView();
            else
                PrintPdf();
        }

        /// <summary>SVG モード: WebView の内容を印刷</summary>
        private void PrintWebView()
        {
#if ANDROID
            var native = webView.Handler?.PlatformView as Android.Webkit.WebView;
            if (native == null) return;
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            if (activity == null) return;
            var pm = (PrintManager)activity.GetSystemService(Context.PrintService)!;
            pm.Print("Reports.net 帳票印刷", native.CreatePrintDocumentAdapter("Reports.net"), null);
#elif IOS
            var native = webView.Handler?.PlatformView as WKWebView;
            if (native == null) return;
            var pc = UIPrintInteractionController.SharedPrintController;
            var pi = UIPrintInfo.PrintInfo;
            pi.OutputType = UIPrintInfoOutputType.General;
            pi.JobName = "Reports.net 帳票印刷";
            pc.PrintInfo = pi;
            pc.PrintFormatter = native.ViewPrintFormatter;
            pc.Present(true, null);
#else
            webView.EvaluateJavaScriptAsync("window.print()");
#endif
            lblStatus.Text = $"全 {totalPages_} ページの印刷";
        }

        /// <summary>PDF モード: 生成済み PDF ファイルを印刷</summary>
        private void PrintPdf()
        {
            if (pdfPath_ == null || !File.Exists(pdfPath_)) return;
#if ANDROID
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            if (activity == null) return;
            var pm = (PrintManager)activity.GetSystemService(Context.PrintService)!;
            pm.Print("Reports.net PDF", new PdfPrintAdapter(pdfPath_), null);
            lblStatus.Text = $"全 {totalPages_} ページの PDF 印刷";
#elif IOS
            var url = Foundation.NSUrl.FromFilename(pdfPath_);
            var pc = UIPrintInteractionController.SharedPrintController;
            var pi = UIPrintInfo.PrintInfo;
            pi.OutputType = UIPrintInfoOutputType.General;
            pi.JobName = "Reports.net PDF";
            pc.PrintInfo = pi;
            pc.PrintingItem = url;
            pc.Present(true, null);
            lblStatus.Text = $"全 {totalPages_} ページの PDF 印刷";
#endif
        }

#if ANDROID
        /// <summary>Android PrintManager 用: PDF バイトをそのまま流すアダプター</summary>
        private class PdfPrintAdapter : PrintDocumentAdapter
        {
            private readonly string filePath_;

            public PdfPrintAdapter(string filePath)
            {
                filePath_ = filePath;
            }

            public override void OnLayout(PrintAttributes? oldAttributes, PrintAttributes? newAttributes,
                Android.OS.CancellationSignal? cancellationSignal, LayoutResultCallback? callback,
                Android.OS.Bundle? extras)
            {
                var info = new PrintDocumentInfo.Builder("Reports.net.pdf")
                    .SetContentType((int)PrintContentType.Document)
                    .SetPageCount(PrintDocumentInfo.PageCountUnknown)
                    .Build();
                callback?.OnLayoutFinished(info, true);
            }

            public override void OnWrite(PageRange[]? pages, Android.OS.ParcelFileDescriptor? destination,
                Android.OS.CancellationSignal? cancellationSignal, WriteResultCallback? callback)
            {
                try
                {
                    using var input = File.OpenRead(filePath_);
                    using var output = new Java.IO.FileOutputStream(destination!.FileDescriptor);
                    var buf = new byte[8192];
                    int len;
                    while ((len = input.Read(buf, 0, buf.Length)) > 0)
                        output.Write(buf, 0, len);
                    callback?.OnWriteFinished(new[] { PageRange.AllPages });
                }
                catch (Exception)
                {
                    callback?.OnWriteFailed("PDF の書き出しに失敗しました");
                }
            }
        }
#endif

        // ──────────────────────────────────
        //  Welcome 画面
        // ──────────────────────────────────
        private static string BuildWelcomeHtml()
        {
            return @"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8""/>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/>
</head>
<body style=""margin:0; height:100vh; display:flex; justify-content:center; align-items:center;
             background:#E8E8E8; font-family:'Segoe UI','Yu Gothic UI',sans-serif;"">
  <div style=""text-align:center; color:#666;"">
    <div style=""font-size:48px; margin-bottom:16px;"">&#128196;</div>
    <div style=""font-size:18px; font-weight:600;"">SVG / PDF 印刷プレビュー</div>
    <div style=""font-size:13px; margin-top:8px; color:#999;"">
      帳票を選択して「生成」ボタンをタップしてください
    </div>
  </div>
</body>
</html>";
        }
    }
}
