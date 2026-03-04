※Visual Studio 2026では、このREADME.md が自動的にプレビュー表示されます。
※Visual Studio 2022では、Shift+F7 でプレビュー表示してください。
<br>

# WPFでSVGプレビューと印刷

帳票を表示するのに、PDFリーダーは本当に必要でしょうか。

このサンプルは、**WebView2** と **SVG** の組み合わせで帳票のプレビューと印刷を実現します。
PDFリーダーも、PrintPreviewControlも使いません。
ブラウザの描画エンジンだけで、帳票の表示から印刷まですべてが完結します。

---

## このサンプルが示すこと

Reports.net の `GetSvgTag()` メソッドは、帳票1ページ分のSVGタグを返します。
これを WPF の WebView2 コントロールに渡すだけで、帳票がそのまま画面に表示されます。

SVGはベクター画像です。どれだけ拡大しても文字や罫線がぼやけることはありません。
25% から 300% まで自在にズームしても、帳票は常にくっきりと描画されます。

印刷も、ブラウザの `window.print()` を呼ぶだけです。
現在ページの印刷はもちろん、全ページを一括で印刷する機能もこのサンプルに含まれています。

つまり、帳票エンジンが SVG を出力し、ブラウザがそれを描画・印刷する。
Reports.net と WebView2 の間にあるのは、たった1行の HTML です。

---

## なぜ SVG なのか

従来、WPF での帳票プレビューには GDI+ ベースの PrintPreviewControl や
XAML の DocumentViewer が使われてきました。
これらは .NET Framework の世界では十分に機能していましたが、
MAUI やクロスプラットフォームの世界には持ち出せません。

SVG + WebView2 という組み合わせには、そうした制約がありません。
WebView2 が動作する環境であれば、WPF でも MAUI でも、同じ仕組みで帳票プレビューが実現できます。
このサンプルで体験する操作感は、そのままデスクトップの次の標準になり得るものです。

同じ仕組みを MAUI で動かすサンプルは、隣の **07.MAUIでSVGプレビューと印刷** に用意しています。

---

## 7種類の帳票サンプル

ドロップダウンから帳票を選択し、「帳票生成」ボタンをクリックするだけで動作します。

| 帳票 | 内容 | 注目ポイント |
|------|------|-------------|
| **単純なサンプル** | 基本的な帳票出力 | Write メソッドの基本操作 |
| **10の倍数** | 複数ページにまたがる帳票 | ページ送りの動作確認に最適 |
| **郵便番号一覧** | Excel データからの一覧帳票 | QRコード、途中での定義ファイル切り替え |
| **見積書** | ヘッダ＋明細のビジネス帳票 | 表紙と本体で定義ファイルを分離 |
| **請求書** | 動的テーブル生成 | 罫線スタイル、交互色、小計・税計算 |
| **商品一覧** | 大分類・小分類付きリスト | 小計行の色分け、カテゴリ集計 |
| **広告** | 画像・QRコード入り広告チラシ | 画像配置、バーコード生成 |

> **前提条件**: 郵便番号一覧、見積書、請求書、商品一覧、広告の各帳票は
> Excel ファイルをデータソースとして使用しています。
> **Office 64bit版** がインストールされている必要があります。

---

## 使い方

1. `CS\SvgPreview.sln` を Visual Studio で開く
2. NuGet パッケージの復元を実行（WebView2、Reports.net）
3. F5 で実行
4. ドロップダウンから帳票を選び「帳票生成」をクリック
5. ページ送り、ズーム、印刷を試す

---

## 動作環境

- .NET 8 以上
- Windows 10 / 11（WebView2 ランタイム）
- Reports.net（Pao.Reports.dll）
- Visual Studio 2022 以降

---

## ファイル構成

```
06.WPFでSVGプレビューと印刷/
├── CS/
│   ├── SvgPreview.sln         ソリューションファイル
│   ├── SvgPreview.csproj      .NET 8 WPF プロジェクト
│   ├── MainWindow.xaml         メイン画面（WebView2 + ツールバー）
│   ├── MainWindow.xaml.cs      プレビュー・ページ送り・印刷ロジック
│   ├── App.xaml / App.xaml.cs  アプリケーション定義
│   └── Common/                 帳票生成ロジック（7種類）
│       ├── Util.cs             パス管理・Excel接続ユーティリティ
│       └── Make*.cs            各帳票のデータ作成クラス
└── Resources/                  帳票定義ファイル(.prepd)・Excelデータ・画像
```

---

## 技術メモ

### SVG の表示

```csharp
string svgTag = report_.GetSvgTag(page);   // SVGタグを取得
webView.NavigateToString(html);             // WebView2 に表示
```

`GetSvgTag()` が返すSVGタグを、CSSでスタイリングした HTML に埋め込んで
WebView2 に渡しています。ズームは CSS の `transform: scale()` で実現しており、
ブラウザのレンダリングエンジンがすべてを処理します。

### 印刷

```csharp
await webView.CoreWebView2.ExecuteScriptAsync("window.print()");
```

ブラウザの印刷機能をそのまま呼び出すだけです。
CSS の `@media print` で印刷時のスタイルを制御し、
全ページ印刷では `page-break-after` でページ区切りを実現しています。

---

## .NET Framework 4.7.2 版

このサンプルの .NET Framework 4.7.2 移植版は
**VS2013-VS2017/08.WPFでSVGプレビューと印刷** に用意しています。
