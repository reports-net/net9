※Visual Studio 2026では、このREADME.md が自動的にプレビュー表示されます。
※Visual Studio 2022では、Shift+F7 でプレビュー表示してください。
<br>

# MAUIでSVGプレビューと印刷

MAUIの世界には、GDI+ がありません。
PrintDocument も、PrintPreviewControl も、DocumentViewer もありません。
クロスプラットフォームという新しい地平を手にした代わりに、
帳票の印刷プレビューという、業務アプリケーションに不可欠な機能が失われました。

このサンプルは、その失われた機能を **SVG** で取り戻します。

---

## SVG が予想外の活躍をする

Reports.net の `GetSvgTag()` で取得したSVGを、MAUI の WebView に渡す。
たったそれだけで、帳票が画面に表示されます。

ページ送りも、拡大・縮小も、WebView のブラウザ機能がすべて引き受けてくれます。
そしてブラウザの印刷機能を呼び出せば、そのまま印刷も完了します。
特別なライブラリも、プラットフォーム固有のAPIも必要ありません。

GDI+ がなくても、帳票の印刷とプレビューは実現できる。
SVGという選択が、その道を切り開きました。

---

## WPF版と何が違うのか

隣の **06.WPFでSVGプレビューと印刷** と、このMAUI版を見比べてみてください。

UIの見た目も、操作感も、ほぼ同じです。
帳票生成のロジック（Common フォルダ）も同一です。
違うのは、WPF が WebView2 コントロールを使い、MAUI が WebView コントロールを使うという点だけです。

```
// WPF 版
webView.NavigateToString(html);

// MAUI 版
webView.Source = new HtmlWebViewSource { Html = html };
```

```
// WPF 版
await webView.CoreWebView2.ExecuteScriptAsync("window.print()");

// MAUI 版
webView.EvaluateJavaScriptAsync("window.print()");
```

SVGという共通の出力形式を使うことで、WPF と MAUI の間にある技術的な壁が消えています。
帳票エンジンが SVG を出力し、ブラウザがそれを描画する。
このシンプルな構造は、プラットフォームを選びません。

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

1. `CS\SvgPreviewMaui.sln` を Visual Studio で開く
2. ターゲットを **Windows Machine** に設定
3. F5 で実行
4. ドロップダウンから帳票を選び「帳票生成」をクリック
5. ページ送り、ズーム、印刷を試す

---

## 動作環境

- .NET 8 以上（MAUI ワークロードが必要）
- Windows 10 / 11
- Reports.net（Pao.Reports.dll）
- Visual Studio 2022 以降

> MAUI ワークロードが未インストールの場合:
> ```
> dotnet workload install maui
> ```

---

## ファイル構成

```
07.MAUIでSVGプレビューと印刷/
├── CS/
│   ├── SvgPreviewMaui.sln      ソリューションファイル
│   ├── SvgPreviewMaui.csproj   MAUI プロジェクト（Windows ターゲット）
│   ├── MauiProgram.cs           MAUI アプリケーション構成
│   ├── App.xaml / App.xaml.cs   アプリケーション定義・ウィンドウ設定
│   ├── MainPage.xaml            メイン画面（WebView + ツールバー）
│   ├── MainPage.xaml.cs         プレビュー・ページ送り・印刷ロジック
│   ├── Platforms/Windows/       WinUI プラットフォーム固有コード
│   ├── Common/                  帳票生成ロジック（7種類）
│   │   ├── Util.cs              パス管理・Excel接続ユーティリティ
│   │   └── Make*.cs             各帳票のデータ作成クラス
│   ├── Resources/               帳票定義ファイル(.prepd)・Excelデータ・画像
│   └── Pao.Reports.dll          Reports.net エンジン（ローカルコピー）
└── _Test/                       全帳票自動テスト（コンソール）
```

---

## 技術メモ

### SVG の表示

```csharp
string svgTag = report_.GetSvgTag(page);                        // SVGタグを取得
webView.Source = new HtmlWebViewSource { Html = html };          // WebView に表示
```

WPF版と同じく、`GetSvgTag()` が返すSVGタグを HTML に埋め込んで WebView に渡しています。
ズームは CSS の `transform: scale()` で、印刷スタイルは `@media print` で制御します。

### 印刷

```csharp
webView.EvaluateJavaScriptAsync("window.print()");
```

全ページ印刷では、全ページのSVGを `page-break-after` 付きの HTML にまとめ、
`window.onload` で自動的に印刷ダイアログを表示します。

### MAUI 固有の注意点

- MAUI の Picker は、XAML で `ItemsSource` と `SelectedIndex` を同時指定すると
  `SelectedIndex` が反映されない既知バグがあります。コードビハインドで設定しています
- 出力パスの階層が WPF 版（4階層）より深い（5階層）ため、
  `sharePath_` の計算が異なります

---

## この先にあるもの

このサンプルは Windows デスクトップで動作していますが、SVG + WebView という仕組みは
Android や iOS でも同様に機能します。
MAUI アプリケーションから帳票のプレビューと印刷を行う――
一つの SVG 帳票出力が、すべてのプラットフォームをカバーする。
その可能性を、このサンプルで確かめてください。
