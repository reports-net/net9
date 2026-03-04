※Visual Studio 2026では、このREADME.md が自動的にプレビュー表示されます。
※Visual Studio 2022では、Shift+F7 でプレビュー表示してください。
<br>

# スマホやiPhoneで印刷とプレビュー

スマートフォンから業務帳票を印刷する。
つい最近まで、それは「できたらいいな」の域を出ない話でした。

MAUI には GDI+ がありません。PrintDocument も、PrintPreviewControl も存在しません。
デスクトップですらそうなのだから、スマートフォンとなれば、なおさらです。
Android と iOS で共通に使える印刷プレビューの仕組みなど、フレームワーク側には用意されていません。

このサンプルは、**SVG** と **WebView** だけで、その壁を突破します。

---

## スマートフォンの帳票プレビューを再発明する

デスクトップ版の SVG プレビュー（06.WPF版、07.MAUI版）では、
`GetSvgTag(page)` で1ページずつ取得し、ページ送りボタンで閲覧する方式をとっていました。

しかし、スマートフォンでそれをやるとどうなるか。
小さな画面にページ送りのツールバーが並び、帳票は縮小されて読めない。
タップして拡大して、ページを送って、また縮小して――
とても業務で使える操作感ではありません。

そこでこのサンプルでは、方式を根本から変えています。

```csharp
// デスクトップ版: 1ページずつ取得してページ送り
string svgTag = report_.GetSvgTag(page);

// スマホ版: 全ページを一括取得、スクロールで閲覧
string html = report_.GetSvg();
```

`GetSvg()` は、全ページの SVG を一つの HTML に埋め込んで返します。
これを WebView に渡すだけで、帳票全体がスクロール可能な形で表示されます。

ページ送りボタンは不要になりました。
指でスワイプすれば次のページが見える。ピンチで拡大すれば細部が読める。
スマートフォンのユーザーにとって、これが自然な操作です。

```csharp
// ピンチズーム有効化: 0.1倍〜5.0倍まで自在に拡大縮小
html = html.Replace("<head>",
    "<head>\n<meta name=\"viewport\" content=\"width=device-width, " +
    "initial-scale=0.5, minimum-scale=0.1, maximum-scale=5.0, user-scalable=yes\">");

webView.Source = new HtmlWebViewSource { Html = html };
```

viewport メタタグを1行注入するだけで、iOS の Safari エンジンも、Android の Chrome エンジンも、
ピンチズームを正しく処理してくれます。
帳票のベクターデータは SVG なので、どれだけ拡大しても文字がぼやけることはありません。

---

## SVG と PDF を並べてみる

このサンプルには、SVG と PDF の2つのモードが用意されています。

| | SVG モード | PDF モード |
|---|---|---|
| 表示方法 | WebView 内でそのまま表示 | 外部 PDF ビューアを起動 |
| 拡大・縮小 | ピンチズーム（ベクター描画） | PDF ビューア依存 |
| 印刷 | `window.print()` でその場で印刷 | PDF ビューアの印刷機能 |
| アプリ内完結 | する | しない |

```csharp
// SVGモード: アプリ内で完結
report_ = ReportCreator.GetReport();
Make見積書.SetupData(report_);
webView.Source = new HtmlWebViewSource { Html = report_.GetSvg() };

// PDFモード: 外部アプリに委譲
report_ = ReportCreator.GetPdf();
Make見積書.SetupData(report_);
report_.SavePDF(stream);
await Launcher.Default.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(pdfPath) });
```

PDF モードでは、帳票を生成してファイルに保存し、OS の外部ビューアに渡します。
一方 SVG モードでは、WebView の中ですべてが完結します。
プレビューも、拡大も、印刷も、アプリから一歩も出る必要がありません。

どちらのモードも Reports.net の帳票生成ロジックは同一です。
`SetupData()` で帳票にデータを流し込む処理は、SVG でも PDF でもまったく同じ。
違うのは、最後の出力先だけです。

両方を実際に触ってみてください。
SVG の操作感が、PDF とは次元の違うものだということが体感できるはずです。

---

## プラットフォームを選ばない印刷

```csharp
webView.EvaluateJavaScriptAsync("window.print()");
```

この1行が、Android でも、iOS でも、Windows でも、印刷ダイアログを表示します。

Android なら Google クラウドプリント互換のシステム印刷ダイアログ。
iOS なら AirPrint。Windows なら OS 標準の印刷ダイアログ。
WebView の中のブラウザエンジンが `window.print()` を受け取り、
各プラットフォームのネイティブ印刷機能に橋渡しをしてくれます。

プラットフォーム別の印刷コードは1行も書いていません。
`Platforms/Android/` にも `Platforms/iOS/` にも、印刷のための特別なコードはありません。
MAUI のエントリポイントがあるだけです。

SVG という出力形式と、WebView というブラウザエンジンの組み合わせが、
プラットフォームの違いを吸収してくれるのです。

---

## 業務アプリとしてのスマートフォン

スマートフォンの画面が小さいからといって、
レシートやラベルしか印刷しないわけではありません。

営業先で見積書を開いて、その場で印刷する。
倉庫で在庫リストを確認して、商品一覧を出力する。
A4 の帳票を、スマートフォンからそのまま印刷にかける。
それが、このサンプルが想定している使い方です。

全ページがスクロールで閲覧でき、ピンチで拡大して細部を確認し、
そのまま印刷ボタンを押す。ページ送りボタンに手間取ることはありません。

---

## 6種類の帳票サンプル

ドロップダウンから帳票を選択し、「生成」ボタンをタップするだけで動作します。

| 帳票 | 内容 | 注目ポイント |
|------|------|-------------|
| **単純なサンプル** | 基本的な帳票出力 | Write メソッドの基本操作 |
| **10の倍数** | 4ページにまたがる帳票 | スワイプでのページ間移動 |
| **郵便番号一覧** | Azure SQL からの一覧帳票 | QRコード、6ページ超のスクロール |
| **見積書** | ヘッダ＋明細のビジネス帳票 | 表紙と本体で定義ファイルを分離 |
| **請求書** | 動的テーブル生成 | 罫線スタイル、交互色、小計・税計算 |
| **商品一覧** | 大分類・小分類付きリスト | 小計行の色分け、カテゴリ集計 |

> **データソース**: 単純なサンプルと10の倍数はローカルデータのみで動作します。
> 郵便番号一覧以降の帳票は Azure SQL Server からデータを取得します。

---

## デスクトップ版との違い

隣の **06.WPFでSVGプレビューと印刷** や **07.MAUIでSVGプレビューと印刷** と比較してみてください。

| | デスクトップ版（06/07） | モバイル版（08） |
|---|---|---|
| 表示方式 | `GetSvgTag(page)` で1ページずつ | `GetSvg()` で全ページ一括 |
| ページ移動 | 前へ/次へボタン | 指でスワイプ |
| 拡大・縮小 | ツールバーのズームボタン | ピンチズーム |
| PDF 比較 | なし | SVG/PDF 切替トグルで比較可能 |
| データソース | ローカル Excel ファイル | Azure SQL Server |
| DLL | Pao.Reports.dll（Windows専用） | Pao.Reports.Linux.dll（クロスプラットフォーム） |

帳票生成のロジック（Common フォルダの Make*.cs）の構造は同一ですが、
データの取得先が異なります。デスクトップ版がローカル Excel ファイルを使うのに対し、
モバイル版は Azure SQL Server からネットワーク経由でデータを取得します。
クラウド上のデータベースから帳票を生成して、その場で印刷する。
モバイルならではの運用形態です。

---

## 使い方

1. `CS\SvgPreviewMobile.sln` を Visual Studio で開く
2. ターゲットを **Windows Machine**、**Android 実機/Emulator**、または **iOS Simulator** に設定
3. F5 で実行
4. SVG / PDF モードを選択
5. ドロップダウンから帳票を選び「生成」をタップ
6. ピンチで拡大、スワイプでスクロール、「印刷」をタップ

> **帳票の生成が遅い場合は、スマートフォンや iPhone を再起動してください。**
> 長期間再起動していない端末ではリソースが解放されず、生成に時間がかかることがあります。

---

## 動作環境

- .NET 8（MAUI ワークロードが必要）
- Android 5.0 (API 21) 以上 / iOS 15.0 以上 / Windows 10 以上
- Reports.net（Pao.Reports.Linux.dll — クロスプラットフォーム版）
- Visual Studio 2022（.NET 8 / iOS Hot Restart 対応）

> MAUI ワークロードが未インストールの場合:
> ```
> dotnet workload install maui
> ```

### .NET 10 以降の iOS 制限事項

**.NET 10 (Visual Studio 2026) では、Windows から USB ケーブル経由で iPad / iPhone にアプリを配置する「Hot Restart」機能が廃止されました。**

- .NET 8 / .NET 9 + Visual Studio 2022: Hot Restart で Windows から直接 iOS デバイスに配置可能
- .NET 10 + Visual Studio 2026: **iOS デバイスへの配置には Mac が必須**（Pair to Mac またはコマンドライン経由）

Mac でビルド・配置する場合は、`SvgPreviewMobile.macos.csproj` を使用してください。
コピー不要で、そのままビルドできます。

| | `.csproj`（Windows 用） | `.macos.csproj`（Mac 用） |
|---|---|---|
| ターゲット | `net8.0-android;net8.0-ios;net8.0-windows` | `net8.0-maccatalyst` |
| パス区切り | バックスラッシュ `\` | スラッシュ `/` |
| 用途 | Visual Studio (Windows) | Mac 上でのビルド・デバッグ |

> .NET 10 版の `.macos.csproj` は `sample/.NET 10/08.スマホやiPhoneで印刷とプレビュー/CS/` にあり、
> Mac Catalyst に加えて **iOS ターゲットが追加**されています（Hot Restart 廃止への対応）。

### Windows から SSH 経由で Mac ビルド・iPad 配置

Mac が同一ネットワーク上にあれば、Windows から SSH で接続して
ビルドからiPad への配置まで一貫して行えます。
Visual Studio の Pair to Mac やリモートデバイス機能は不要です。

```bash
# 1. ソースを Mac に転送
scp -r CS/* user@mac:~/SvgPreviewMobile/

# 2. SSH で Mac に接続してビルド（署名は Mac の Keychain から自動取得）
ssh user@mac
cd ~/SvgPreviewMobile
dotnet build SvgPreviewMobile.macos.csproj -f net10.0-ios -r ios-arm64

# 3. iPad に配置（USB 接続の iPad を自動検出）
xcrun devicectl device install app --device <DEVICE_ID> \
  bin/Debug/net10.0-ios/ios-arm64/SvgPreviewMobile.macos.app
```

Claude Code 等の AI ツールからも SSH 経由で同様の操作が可能です。
デバッグが必要な場合は Mac 上で VS Code を使用してください。

---

## ファイル構成

```
08.スマホやiPhoneで印刷とプレビュー/
├── CS/                            .NET 8 版（メイン）
│   ├── SvgPreviewMobile.sln       ソリューションファイル
│   ├── SvgPreviewMobile.csproj    MAUI プロジェクト（Android/iOS/Windows）
│   ├── global.json                .NET 8 SDK に固定
│   ├── MainPage.xaml              メイン画面（WebView + ツールバー）
│   ├── MainPage.xaml.cs           SVG/PDF 生成・プレビュー・印刷ロジック
│   ├── Platforms/
│   │   ├── Android/               Android エントリポイント + AndroidManifest.xml
│   │   ├── iOS/                   iOS エントリポイント + Info.plist
│   │   └── Windows/               Windows (WinUI) エントリポイント
│   ├── Common/                    帳票生成ロジック（6種類）
│   │   ├── Util.cs                Azure SQL 接続・パス管理
│   │   └── Make*.cs               各帳票のデータ作成クラス
│   ├── Data/                      帳票定義ファイル(.prepd)・画像（EmbeddedResource）
│   ├── Pao.Reports.Linux.dll     Reports.net クロスプラットフォーム版
│   └── SixLabors.*.dll           ImageSharp ライブラリ
├── NET10/                         .NET 10 版（VS2026 用・iOS は Mac 必須）
│   ├── SvgPreviewMobile.sln
│   ├── SvgPreviewMobile.csproj    net10.0-android/ios/windows
│   ├── SvgPreviewMobile.csproj.macos  Mac ビルド用（maccatalyst + ios）
│   └── （ソースコードは CS/ と同一、csproj のみ差分）
└── _Test/                         全帳票自動テスト（コンソール）
```

---

## 技術メモ

### なぜ GetSvg() なのか

デスクトップ版で使った `GetSvgTag(page)` は、1ページのSVGタグだけを返します。
ページ送りのUIと組み合わせる前提の設計です。

一方、`GetSvg()` は全ページのSVGを一つの HTML にまとめて返します。
ページの区切りも、スタイルも、すべて含まれた完成品の HTML です。

```csharp
// GetSvgTag(page): SVGタグ1ページ分（デスクトップ向き）
string svgTag = report_.GetSvgTag(1);  // → <svg>...</svg>

// GetSvg(): 全ページのSVG入りHTML（モバイル向き）
string html = report_.GetSvg();        // → <!DOCTYPE html><html>...<svg>...</svg>...<svg>...</svg>...</html>
```

スマートフォンでは、この HTML をそのまま WebView に渡すだけで、
ブラウザエンジンが描画とスクロールとピンチズームのすべてを処理してくれます。

### クロスプラットフォーム DLL

Windows 専用の `Pao.Reports.dll` は System.Drawing (GDI+) に依存しているため、
Android や iOS では動作しません。

このサンプルでは `Pao.Reports.Linux.dll` を使用しています。
これは SixLabors.ImageSharp ベースの描画エンジンで、
Windows でも Android でも iOS でも同一のバイナリで動作します。

### Azure SQL Server 接続

```csharp
// Util.cs: Azure SQL Server への接続
Server=tcp:fzxu46e9ck.database.windows.net,1433;
Initial Catalog=Reports.net.Sample;
```

デスクトップ版がローカルの Excel ファイルからデータを読み取るのに対し、
モバイル版は `Microsoft.Data.SqlClient` で Azure SQL Server に直接接続します。
03.帳票WEBアプリケーションと同じデータベースを共有しています。

---

## MAUI の可能性

Flutter は美しい UI を描けます。React Native はウェブの資産を活かせます。
しかし、業務アプリケーションに不可欠な「帳票の印刷」を、
プラットフォーム固有のコードなしで実現できる組み合わせは、
MAUI + SVG + WebView の他にありません。

このサンプルの `Platforms/` ディレクトリを見てください。
Android にも iOS にも、印刷のための特別なコードは1行もありません。
帳票エンジンが SVG を出力し、WebView がそれを描画し、
`window.print()` が OS の印刷機能を呼び出す。
この仕組みが、3つのプラットフォームで同じように動きます。

営業先で見積書をその場で印刷する。
現場で作業指示書を確認して、必要なページだけ出力する。
スマートフォンが業務の現場に持ち込まれたとき、
帳票の印刷は「あったら便利」ではなく「なければ困る」機能になります。

その機能を、プラットフォームを問わず、一つのコードベースで提供する。
MAUI + SVG + WebView が切り開いた道は、業務アプリケーションの新しい標準になり得るものです。
