using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Pao.Reports;

namespace SvgPreview
{
    /// <summary>
    /// Reports.net SVGプレビュー (WPF + WebView2)
    ///
    /// GetSvgTag(page) で取得したSVGをWebView2に表示し、
    /// ページ送り・ズーム・印刷を行うサンプルです。
    /// 6種類の帳票をドロップダウンから選択して生成できます。
    /// </summary>
    public partial class MainWindow : Window
    {
        // ──────────────────────────────────
        //  フィールド
        // ──────────────────────────────────
        private IReport? report_;
        private int currentPage_ = 0;
        private int totalPages_ = 0;
        private int zoomPercent_ = 100;

        // サンプルルートパス（06.WPFでSVGプレビューと印刷/）
        private readonly string sharePath_;

        public MainWindow()
        {
            // バーコードのSHIFT_JISサポートに必要
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            InitializeComponent();

            // 共有リソースパス取得（CS/bin/Debug/net8.0-windows/ → 4階層上がサンプルルート）
            sharePath_ = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

            // Util にリソースパスをセット
            Util.SetSharePath(sharePath_);

            InitializeWebView();
            UpdateNavigationState();
        }

        // ──────────────────────────────────
        //  WebView2 初期化
        // ──────────────────────────────────
        private async void InitializeWebView()
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView.CoreWebView2.Settings.IsZoomControlEnabled = false;
            webView.CoreWebView2.Settings.AreDevToolsEnabled = false;

            webView.NavigateToString(BuildWelcomeHtml());
        }

        // ──────────────────────────────────
        //  帳票生成
        // ──────────────────────────────────
        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = cmbReport.SelectedIndex;
                if (selectedIndex < 0)
                {
                    MessageBox.Show("帳票を選択してください。", "帳票未選択", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                string reportName = ((ComboBoxItem)cmbReport.SelectedItem).Content.ToString()!;

                lblStatus.Text = $"「{reportName}」を生成中...";

                report_ = ReportCreator.GetReport();

                // 選択された帳票タイプに応じてデータ作成
                switch (selectedIndex)
                {
                    case 0: Make単純なサンプル.SetupData(report_); break;
                    case 1: Make10の倍数.SetupData(report_); break;
                    case 2: Make郵便番号一覧.SetupData(report_); break;
                    case 3: Make見積書.SetupData(report_); break;
                    case 4: Make請求書.SetupData(report_); break;
                    case 5: Make商品大小分類.SetupData(report_); break;
                }

                totalPages_ = report_.AllPages;
                currentPage_ = 1;
                zoomPercent_ = 100;

                ShowPage(currentPage_);
                lblStatus.Text = $"「{reportName}」生成完了 — {totalPages_} ページ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                lblStatus.Text = "エラーが発生しました";
            }
        }

        // ──────────────────────────────────
        //  SVG 表示
        // ──────────────────────────────────
        private void ShowPage(int page)
        {
            if (report_ == null || page < 1 || page > totalPages_) return;

            currentPage_ = page;

            string svgTag = report_.GetSvgTag(page);
            string html = BuildPreviewHtml(svgTag);
            webView.NavigateToString(html);

            UpdateNavigationState();
        }

        // ──────────────────────────────────
        //  プレビュー用HTML構築
        // ──────────────────────────────────
        private string BuildPreviewHtml(string svgTag)
        {
            double scale = zoomPercent_ / 100.0;

            return $@"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8""/>
<style>
  * {{ margin: 0; padding: 0; box-sizing: border-box; }}
  html, body {{
    width: 100%; height: 100%;
    background: #E8E8E8;
    display: flex;
    justify-content: center;
    align-items: flex-start;
    overflow: auto;
  }}
  .page {{
    margin: 24px auto;
    background: white;
    box-shadow: 0 2px 12px rgba(0,0,0,0.18), 0 0 0 1px rgba(0,0,0,0.06);
    transform: scale({scale.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)});
    transform-origin: top center;
  }}
  .page svg {{
    display: block;
  }}
  @media print {{
    html, body {{ background: white; overflow: hidden; display: block; width: auto; height: auto; }}
    .page {{ margin: 0; box-shadow: none; transform: none; }}
    .page svg {{ max-width: 100%; height: auto; }}
  }}
</style>
</head>
<body>
  <div class=""page"">{svgTag}</div>
</body>
</html>";
        }

        // ──────────────────────────────────
        //  全ページ印刷用HTML構築
        // ──────────────────────────────────
        private string BuildPrintAllHtml()
        {
            if (report_ == null) return "";

            var sb = new StringBuilder();
            sb.Append(@"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8""/>
<style>
  * { margin: 0; padding: 0; }
  .page { page-break-after: always; }
  .page:last-child { page-break-after: auto; }
  .page svg { display: block; margin: 0 auto; }
  @media screen {
    body { background: #E8E8E8; }
    .page { background: white; margin: 16px auto;
      box-shadow: 0 2px 8px rgba(0,0,0,0.15); }
  }
</style>
</head>
<body>");

            for (int p = 1; p <= totalPages_; p++)
            {
                sb.Append("<div class=\"page\">");
                sb.Append(report_.GetSvgTag(p));
                sb.Append("</div>");
            }

            sb.Append("</body></html>");
            return sb.ToString();
        }

        // ──────────────────────────────────
        //  単ページ印刷用HTML構築
        // ──────────────────────────────────
        private static string BuildSinglePagePrintHtml(string svgTag)
        {
            return @"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8""/>
<style>
  * { margin: 0; padding: 0; }
  html, body { overflow: hidden; }
  .page svg { display: block; margin: 0 auto; max-width: 100%; height: auto; }
  @media screen {
    body { background: #E8E8E8; }
    .page { background: white; margin: 16px auto;
      box-shadow: 0 2px 8px rgba(0,0,0,0.15); }
  }
</style>
</head>
<body><div class=""page"">" + svgTag + @"</div></body>
</html>";
        }

        // ──────────────────────────────────
        //  Welcome 画面
        // ──────────────────────────────────
        private static string BuildWelcomeHtml()
        {
            return @"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""/></head>
<body style=""margin:0; height:100vh; display:flex; justify-content:center; align-items:center;
             background:#E8E8E8; font-family:'Segoe UI','Yu Gothic UI',sans-serif;"">
  <div style=""text-align:center; color:#666;"">
    <div style=""font-size:48px; margin-bottom:16px;"">&#128196;</div>
    <div style=""font-size:18px; font-weight:600;"">SVG プレビュー</div>
    <div style=""font-size:13px; margin-top:8px; color:#999;"">
      帳票を選択して「帳票生成」ボタンをクリックしてください
    </div>
  </div>
</body>
</html>";
        }

        // ──────────────────────────────────
        //  ページナビゲーション
        // ──────────────────────────────────
        private void BtnFirst_Click(object sender, RoutedEventArgs e) => ShowPage(1);
        private void BtnPrev_Click(object sender, RoutedEventArgs e) => ShowPage(currentPage_ - 1);
        private void BtnNext_Click(object sender, RoutedEventArgs e) => ShowPage(currentPage_ + 1);
        private void BtnLast_Click(object sender, RoutedEventArgs e) => ShowPage(totalPages_);

        private void TxtPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(txtPage.Text, out int p))
                    ShowPage(p);
            }
        }

        private void UpdateNavigationState()
        {
            bool hasPages = totalPages_ > 0;
            btnFirst.IsEnabled = hasPages && currentPage_ > 1;
            btnPrev.IsEnabled = hasPages && currentPage_ > 1;
            btnNext.IsEnabled = hasPages && currentPage_ < totalPages_;
            btnLast.IsEnabled = hasPages && currentPage_ < totalPages_;
            btnPrintPage.IsEnabled = hasPages;
            btnPrintAll.IsEnabled = hasPages;

            txtPage.Text = hasPages ? currentPage_.ToString() : "0";
            lblTotal.Text = $" / {totalPages_}";
            lblZoom.Text = $"{zoomPercent_}%";
        }

        // ──────────────────────────────────
        //  ズーム
        // ──────────────────────────────────
        private static readonly int[] ZoomLevels = { 25, 50, 75, 100, 125, 150, 200, 300 };

        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            for (int i = ZoomLevels.Length - 1; i >= 0; i--)
            {
                if (ZoomLevels[i] < zoomPercent_)
                {
                    zoomPercent_ = ZoomLevels[i];
                    ShowPage(currentPage_);
                    return;
                }
            }
        }

        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ZoomLevels.Length; i++)
            {
                if (ZoomLevels[i] > zoomPercent_)
                {
                    zoomPercent_ = ZoomLevels[i];
                    ShowPage(currentPage_);
                    return;
                }
            }
        }

        // ──────────────────────────────────
        //  印刷
        // ──────────────────────────────────
        private void BtnPrintPage_Click(object sender, RoutedEventArgs e)
        {
            if (report_ == null) return;
            lblStatus.Text = "印刷ダイアログを表示中...";

            string svgTag = report_.GetSvgTag(currentPage_);
            string printHtml = BuildSinglePagePrintHtml(svgTag);
            webView.NavigateToString(printHtml);

            void handler(object? s, CoreWebView2NavigationCompletedEventArgs args)
            {
                webView.CoreWebView2.NavigationCompleted -= handler;
                Dispatcher.InvokeAsync(async () =>
                {
                    await webView.CoreWebView2.ExecuteScriptAsync("window.print()");
                    ShowPage(currentPage_);
                    lblStatus.Text = $"ページ {currentPage_} / {totalPages_}";
                });
            }
            webView.CoreWebView2.NavigationCompleted += handler;
        }

        private async void BtnPrintAll_Click(object sender, RoutedEventArgs e)
        {
            if (report_ == null) return;
            lblStatus.Text = "全ページの印刷を準備中...";

            string allHtml = BuildPrintAllHtml();
            webView.NavigateToString(allHtml);

            void handler(object? s, CoreWebView2NavigationCompletedEventArgs args)
            {
                webView.CoreWebView2.NavigationCompleted -= handler;
                Dispatcher.InvokeAsync(async () =>
                {
                    await webView.CoreWebView2.ExecuteScriptAsync("window.print()");
                    ShowPage(currentPage_);
                    lblStatus.Text = $"全 {totalPages_} ページの印刷完了";
                });
            }
            webView.CoreWebView2.NavigationCompleted += handler;
        }
    }
}
