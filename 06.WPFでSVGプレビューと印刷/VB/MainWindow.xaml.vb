Imports System
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports Microsoft.Web.WebView2.Core
Imports Pao.Reports

''' <summary>
''' Reports.net SVGプレビュー (WPF + WebView2)
'''
''' GetSvgTag(page) で取得したSVGをWebView2に表示し、
''' ページ送り・ズーム・印刷を行うサンプルです。
''' 6種類の帳票をドロップダウンから選択して生成できます。
''' </summary>
Partial Public Class MainWindow
    Inherits Window

    ' ──────────────────────────────────
    '  フィールド
    ' ──────────────────────────────────
    Private report_ As IReport
    Private currentPage_ As Integer = 0
    Private totalPages_ As Integer = 0
    Private zoomPercent_ As Integer = 100

    ' サンプルルートパス（06.WPFでSVGプレビューと印刷/）
    Private ReadOnly sharePath_ As String

    Public Sub New()
        ' バーコードのSHIFT_JISサポートに必要
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

        InitializeComponent()

        ' 共有リソースパス取得（VB/bin/Debug/net8.0-windows/ → 4階層上がサンプルルート）
        sharePath_ = System.IO.Path.GetFullPath(
            System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."))

        ' Util にリソースパスをセット
        Util.SetSharePath(sharePath_)

        UpdateNavigationState()

        ' WebView2 初期化はイベントループ開始後に実行（Sub Main方式のため）
        AddHandler Loaded, AddressOf MainWindow_Loaded
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs)
        RemoveHandler Loaded, AddressOf MainWindow_Loaded
        InitializeWebView()
    End Sub

    ' ──────────────────────────────────
    '  WebView2 初期化
    ' ──────────────────────────────────
    Private Async Sub InitializeWebView()
        Await webView.EnsureCoreWebView2Async()
        webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = False
        webView.CoreWebView2.Settings.IsZoomControlEnabled = False
        webView.CoreWebView2.Settings.AreDevToolsEnabled = False

        webView.NavigateToString(BuildWelcomeHtml())
    End Sub

    ' ──────────────────────────────────
    '  帳票生成
    ' ──────────────────────────────────
    Private Sub BtnGenerate_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim selectedIndex As Integer = cmbReport.SelectedIndex
            If selectedIndex < 0 Then
                MessageBox.Show("帳票を選択してください。", "帳票未選択", MessageBoxButton.OK, MessageBoxImage.Information)
                Return
            End If
            Dim reportName As String = DirectCast(cmbReport.SelectedItem, ComboBoxItem).Content.ToString()

            lblStatus.Text = "「" & reportName & "」を生成中..."

            report_ = ReportCreator.GetReport()

            ' 選択された帳票タイプに応じてデータ作成
            Select Case selectedIndex
                Case 0 : Make単純なサンプル.SetupData(report_)
                Case 1 : Make10の倍数.SetupData(report_)
                Case 2 : Make郵便番号一覧.SetupData(report_)
                Case 3 : Make見積書.SetupData(report_)
                Case 4 : Make請求書.SetupData(report_)
                Case 5 : Make商品大小分類.SetupData(report_)
            End Select

            totalPages_ = report_.AllPages
            currentPage_ = 1
            zoomPercent_ = 100

            ShowPage(currentPage_)
            lblStatus.Text = "「" & reportName & "」生成完了 — " & totalPages_.ToString() & " ページ"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error)
            lblStatus.Text = "エラーが発生しました"
        End Try
    End Sub

    ' ──────────────────────────────────
    '  SVG 表示
    ' ──────────────────────────────────
    Private Sub ShowPage(page As Integer)
        If report_ Is Nothing OrElse page < 1 OrElse page > totalPages_ Then Return

        currentPage_ = page

        Dim svgTag As String = report_.GetSvgTag(page)
        Dim html As String = BuildPreviewHtml(svgTag)
        webView.NavigateToString(html)

        UpdateNavigationState()
    End Sub

    ' ──────────────────────────────────
    '  プレビュー用HTML構築
    ' ──────────────────────────────────
    Private Function BuildPreviewHtml(svgTag As String) As String
        Dim scale As Double = zoomPercent_ / 100.0

        Return "<!DOCTYPE html>" & vbCrLf &
"<html>" & vbCrLf &
"<head>" & vbCrLf &
"<meta charset=""utf-8""/>" & vbCrLf &
"<style>" & vbCrLf &
"  * { margin: 0; padding: 0; box-sizing: border-box; }" & vbCrLf &
"  html, body {" & vbCrLf &
"    width: 100%; height: 100%;" & vbCrLf &
"    background: #E8E8E8;" & vbCrLf &
"    display: flex;" & vbCrLf &
"    justify-content: center;" & vbCrLf &
"    align-items: flex-start;" & vbCrLf &
"    overflow: auto;" & vbCrLf &
"  }" & vbCrLf &
"  .page {" & vbCrLf &
"    margin: 24px auto;" & vbCrLf &
"    background: white;" & vbCrLf &
"    box-shadow: 0 2px 12px rgba(0,0,0,0.18), 0 0 0 1px rgba(0,0,0,0.06);" & vbCrLf &
"    transform: scale(" & scale.ToString("F2", System.Globalization.CultureInfo.InvariantCulture) & ");" & vbCrLf &
"    transform-origin: top center;" & vbCrLf &
"  }" & vbCrLf &
"  .page svg {" & vbCrLf &
"    display: block;" & vbCrLf &
"  }" & vbCrLf &
"  @media print {" & vbCrLf &
"    html, body { background: white; overflow: hidden; display: block; width: auto; height: auto; }" & vbCrLf &
"    .page { margin: 0; box-shadow: none; transform: none; }" & vbCrLf &
"    .page svg { max-width: 100%; height: auto; }" & vbCrLf &
"  }" & vbCrLf &
"</style>" & vbCrLf &
"</head>" & vbCrLf &
"<body>" & vbCrLf &
"  <div class=""page"">" & svgTag & "</div>" & vbCrLf &
"</body>" & vbCrLf &
"</html>"
    End Function

    ' ──────────────────────────────────
    '  全ページ印刷用HTML構築
    ' ──────────────────────────────────
    Private Function BuildPrintAllHtml() As String
        If report_ Is Nothing Then Return ""

        Dim sb As New StringBuilder()
        sb.Append("<!DOCTYPE html>" & vbCrLf &
"<html>" & vbCrLf &
"<head>" & vbCrLf &
"<meta charset=""utf-8""/>" & vbCrLf &
"<style>" & vbCrLf &
"  * { margin: 0; padding: 0; }" & vbCrLf &
"  .page { page-break-after: always; }" & vbCrLf &
"  .page:last-child { page-break-after: auto; }" & vbCrLf &
"  .page svg { display: block; margin: 0 auto; }" & vbCrLf &
"  @media screen {" & vbCrLf &
"    body { background: #E8E8E8; }" & vbCrLf &
"    .page { background: white; margin: 16px auto;" & vbCrLf &
"      box-shadow: 0 2px 8px rgba(0,0,0,0.15); }" & vbCrLf &
"  }" & vbCrLf &
"</style>" & vbCrLf &
"</head>" & vbCrLf &
"<body>")

        For p As Integer = 1 To totalPages_
            sb.Append("<div class=""page"">")
            sb.Append(report_.GetSvgTag(p))
            sb.Append("</div>")
        Next

        sb.Append("</body></html>")
        Return sb.ToString()
    End Function

    ' ──────────────────────────────────
    '  単ページ印刷用HTML構築
    ' ──────────────────────────────────
    Private Shared Function BuildSinglePagePrintHtml(svgTag As String) As String
        Return "<!DOCTYPE html>" & vbCrLf &
"<html>" & vbCrLf &
"<head>" & vbCrLf &
"<meta charset=""utf-8""/>" & vbCrLf &
"<style>" & vbCrLf &
"  * { margin: 0; padding: 0; }" & vbCrLf &
"  html, body { overflow: hidden; }" & vbCrLf &
"  .page svg { display: block; margin: 0 auto; max-width: 100%; height: auto; }" & vbCrLf &
"  @media screen {" & vbCrLf &
"    body { background: #E8E8E8; }" & vbCrLf &
"    .page { background: white; margin: 16px auto;" & vbCrLf &
"      box-shadow: 0 2px 8px rgba(0,0,0,0.15); }" & vbCrLf &
"  }" & vbCrLf &
"</style>" & vbCrLf &
"</head>" & vbCrLf &
"<body><div class=""page"">" & svgTag & "</div></body>" & vbCrLf &
"</html>"
    End Function

    ' ──────────────────────────────────
    '  Welcome 画面
    ' ──────────────────────────────────
    Private Shared Function BuildWelcomeHtml() As String
        Return "<!DOCTYPE html>" & vbCrLf &
"<html>" & vbCrLf &
"<head><meta charset=""utf-8""/></head>" & vbCrLf &
"<body style=""margin:0; height:100vh; display:flex; justify-content:center; align-items:center;" & vbCrLf &
"             background:#E8E8E8; font-family:'Segoe UI','Yu Gothic UI',sans-serif;"">" & vbCrLf &
"  <div style=""text-align:center; color:#666;"">" & vbCrLf &
"    <div style=""font-size:48px; margin-bottom:16px;"">&#128196;</div>" & vbCrLf &
"    <div style=""font-size:18px; font-weight:600;"">SVG プレビュー</div>" & vbCrLf &
"    <div style=""font-size:13px; margin-top:8px; color:#999;"">" & vbCrLf &
"      帳票を選択して「帳票生成」ボタンをクリックしてください" & vbCrLf &
"    </div>" & vbCrLf &
"  </div>" & vbCrLf &
"</body>" & vbCrLf &
"</html>"
    End Function

    ' ──────────────────────────────────
    '  ページナビゲーション
    ' ──────────────────────────────────
    Private Sub BtnFirst_Click(sender As Object, e As RoutedEventArgs)
        ShowPage(1)
    End Sub

    Private Sub BtnPrev_Click(sender As Object, e As RoutedEventArgs)
        ShowPage(currentPage_ - 1)
    End Sub

    Private Sub BtnNext_Click(sender As Object, e As RoutedEventArgs)
        ShowPage(currentPage_ + 1)
    End Sub

    Private Sub BtnLast_Click(sender As Object, e As RoutedEventArgs)
        ShowPage(totalPages_)
    End Sub

    Private Sub TxtPage_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Then
            Dim p As Integer
            If Integer.TryParse(txtPage.Text, p) Then
                ShowPage(p)
            End If
        End If
    End Sub

    Private Sub UpdateNavigationState()
        Dim hasPages As Boolean = totalPages_ > 0
        btnFirst.IsEnabled = hasPages AndAlso currentPage_ > 1
        btnPrev.IsEnabled = hasPages AndAlso currentPage_ > 1
        btnNext.IsEnabled = hasPages AndAlso currentPage_ < totalPages_
        btnLast.IsEnabled = hasPages AndAlso currentPage_ < totalPages_
        btnPrintPage.IsEnabled = hasPages
        btnPrintAll.IsEnabled = hasPages

        If hasPages Then
            txtPage.Text = currentPage_.ToString()
        Else
            txtPage.Text = "0"
        End If
        lblTotal.Text = " / " & totalPages_.ToString()
        lblZoom.Text = zoomPercent_.ToString() & "%"
    End Sub

    ' ──────────────────────────────────
    '  ズーム
    ' ──────────────────────────────────
    Private Shared ReadOnly ZoomLevels As Integer() = {25, 50, 75, 100, 125, 150, 200, 300}

    Private Sub BtnZoomOut_Click(sender As Object, e As RoutedEventArgs)
        For i As Integer = ZoomLevels.Length - 1 To 0 Step -1
            If ZoomLevels(i) < zoomPercent_ Then
                zoomPercent_ = ZoomLevels(i)
                ShowPage(currentPage_)
                Return
            End If
        Next
    End Sub

    Private Sub BtnZoomIn_Click(sender As Object, e As RoutedEventArgs)
        For i As Integer = 0 To ZoomLevels.Length - 1
            If ZoomLevels(i) > zoomPercent_ Then
                zoomPercent_ = ZoomLevels(i)
                ShowPage(currentPage_)
                Return
            End If
        Next
    End Sub

    ' ──────────────────────────────────
    '  印刷
    ' ──────────────────────────────────
    Private Sub BtnPrintPage_Click(sender As Object, e As RoutedEventArgs)
        If report_ Is Nothing Then Return
        lblStatus.Text = "印刷ダイアログを表示中..."

        Dim svgTag As String = report_.GetSvgTag(currentPage_)
        Dim printHtml As String = BuildSinglePagePrintHtml(svgTag)
        webView.NavigateToString(printHtml)

        Dim handler As EventHandler(Of CoreWebView2NavigationCompletedEventArgs) = Nothing
        handler = Sub(s, args)
                      RemoveHandler webView.CoreWebView2.NavigationCompleted, handler
                      Dispatcher.InvokeAsync(Async Function()
                                                 Await webView.CoreWebView2.ExecuteScriptAsync("window.print()")
                                                 ShowPage(currentPage_)
                                                 lblStatus.Text = "ページ " & currentPage_.ToString() & " / " & totalPages_.ToString()
                                             End Function)
                  End Sub
        AddHandler webView.CoreWebView2.NavigationCompleted, handler
    End Sub

    Private Sub BtnPrintAll_Click(sender As Object, e As RoutedEventArgs)
        If report_ Is Nothing Then Return
        lblStatus.Text = "全ページの印刷を準備中..."

        Dim allHtml As String = BuildPrintAllHtml()
        webView.NavigateToString(allHtml)

        Dim handler As EventHandler(Of CoreWebView2NavigationCompletedEventArgs) = Nothing
        handler = Sub(s, args)
                      RemoveHandler webView.CoreWebView2.NavigationCompleted, handler
                      Dispatcher.InvokeAsync(Async Function()
                                                 Await webView.CoreWebView2.ExecuteScriptAsync("window.print()")
                                                 ShowPage(currentPage_)
                                                 lblStatus.Text = "全 " & totalPages_.ToString() & " ページの印刷完了"
                                             End Function)
                  End Sub
        AddHandler webView.CoreWebView2.NavigationCompleted, handler
    End Sub

End Class
