※Visual Studio 2026では、このREADME.md が自動的にプレビュー表示されます。
※Visual Studio 2022では、Shift+F7 でプレビュー表示してください。
<br>

# 帳票WEBアプリケーション

WEB アプリケーションで帳票を出力するなら、PDF。
長い間、それが唯一の選択肢でした。

このサンプルは、その常識を書き換えます。

PDF に加えて **SVG** という新しい選択肢を用意し、
ブラウザの中だけで帳票のプレビューと印刷を完結させます。
PDF ビューアのプラグインも、外部アプリケーションも必要ありません。

---

## PDF では実現できなかったもの

PDF は帳票の配布には最適です。どの環境でも同じ見た目で表示される、その信頼性は揺るぎません。

しかし、ブラウザ上での「プレビュー」となると話が変わります。
PDF をブラウザで表示するには、ブラウザ内蔵の PDF ビューアか、外部プラグインに依存します。
ページ送りの操作感は PDF ビューア次第。印刷も PDF ビューアの機能を通すことになります。
WEB アプリケーション側からは、表示も操作もほとんどコントロールできません。

SVG なら、状況は一変します。

SVG はブラウザがネイティブに描画できるベクター画像です。
HTML の一部として埋め込めば、ブラウザが直接レンダリングします。
JavaScript でページを切り替え、CSS でスタイルを制御し、
`window.print()` で印刷ダイアログを呼び出す。
WEB アプリケーションが帳票の表示と印刷を完全にコントロールできるのです。

---

## 2つの SVG モード

このサンプルには、SVG による帳票表示の2つのアプローチが実装されています。

### SVG 一括 — `GetSvg()`

全ページの SVG を一つの HTML にまとめて返します。

```csharp
// コントローラー: 全ページを一括取得
string html = report.GetSvg();
return Content(html, "text/html", Encoding.UTF8);
```

ブラウザには全ページが縦に並んで表示されます。
スクロールするだけで全ページが閲覧でき、
そのまま「View」で表示、「Download」で HTML ファイルとして保存できます。
シンプルで直感的。全体を俯瞰したいときに最適な方式です。

### SVG プレビュー — `GetSvgTag()`

1ページ分の SVG タグだけを返します。ブラウザ上でページ送りの UI を構築できます。

```csharp
// コントローラー: 指定ページのSVGタグを返すエンドポイント
public IActionResult SvgTag(string kind, int page = 1)
{
    string svgTag = report.GetSvgTag(page);
    Response.Headers["X-Total-Pages"] = report.AllPages.ToString();
    return Content(svgTag, "image/svg+xml", Encoding.UTF8);
}
```

```javascript
// ShowSvgPreview.cshtml: ページ送り
fetch('/svgtag/' + reportsKind + '?page=' + currentPage)
    .then(function(r) { return r.text(); })
    .then(function(svgText) {
        document.getElementById('svgContainer').innerHTML = svgText;
    });
```

前へ、次へ、先頭へ、末尾へ。ページ番号を直接入力してジャンプ。
クイック印刷で現在のページだけを印刷。通常印刷で全ページを一括印刷。
PDF ビューアに頼ることなく、ブラウザの中だけで完結する印刷プレビューです。

`GetSvg()` が全体把握に向いているのに対し、
`GetSvgTag()` は1ページずつ丁寧に確認する用途に向いています。
Reports.net がこの2つのメソッドを用意したことで、
WEB アプリケーションの帳票プレビューに、PDF とは異なる新しい道が開かれました。

---

## 3つの出力形式

画面のラジオボタンで出力形式を選択します。

| 形式 | メソッド | 動作 |
|------|----------|------|
| **PDF** | `GetPdf()` + `SavePDF()` | ブラウザの PDF ビューアで表示 / ファイルダウンロード |
| **SVG 一括** | `GetSvg()` | 全ページの SVG を含む HTML を表示 / ダウンロード |
| **SVG プレビュー** | `GetSvgTag(page)` | ページ送り付きのインタラクティブプレビュー |

PDF も SVG も、帳票生成のロジックは同一です。

```csharp
// SVG の場合
IReport report = ReportCreator.GetReport();
Make見積書(report);  // 帳票データの流し込みは同じ

// PDF の場合
IReport report = ReportCreator.GetPdf();
Make見積書(report);  // まったく同じコード
```

`ReportCreator.GetReport()` か `ReportCreator.GetPdf()` か。
インスタンスの生成方法が違うだけで、帳票にデータを書き込む `Write()` や
ページを構成する `PageStart()` / `PageEnd()` のコードは一切変わりません。

---

## ここから始まった

このWEBアプリケーションで実現した「ブラウザ + SVG による帳票プレビュー」というアイデアは、
その後、デスクトップとモバイルに展開されていきました。

| サンプル | 方式 | 着想 |
|----------|------|------|
| **06.WPFでSVGプレビューと印刷** | WebView2 + `GetSvgTag()` | ブラウザで動くなら WebView2 でも動く |
| **07.MAUIでSVGプレビューと印刷** | WebView + `GetSvgTag()` | GDI+ のない MAUI でも SVG なら動く |
| **08.スマホやiPhoneで印刷とプレビュー** | WebView + `GetSvg()` | スマホでもブラウザエンジンは同じ |

ブラウザが SVG を描画できる。ブラウザの印刷機能で帳票を印刷できる。
この単純な事実から出発して、WPF の WebView2 へ、MAUI の WebView へ、
そしてスマートフォンの Android / iOS へと、同じ仕組みが広がっていきました。

すべては、この WEB アプリケーションが出発点です。

---

## 帳票サンプル

ラジオボタンから帳票を選択し、出力形式を選んで「View」または「Download」をクリックします。

| 帳票 | 内容 | 注目ポイント |
|------|------|-------------|
| **単純なサンプル** | 基本的な帳票出力 | Write メソッドの基本操作 |
| **10の倍数** | 複数ページにまたがる帳票 | SVG プレビューでのページ送り動作 |
| **郵便番号一覧** | SQL Server からの一覧帳票 | QRコード、交互背景色 |
| **見積書** | ヘッダ＋明細のビジネス帳票 | 表紙と本体で定義ファイルを分離 |
| **請求書** | 動的テーブル生成 | 罫線スタイル、交互色、小計・税計算 |
| **商品一覧** | 大分類・小分類付きリスト | カテゴリ集計、小計行の色分け |

> **データソース**: 単純なサンプルと10の倍数はローカルのみ。
> 郵便番号以降の帳票は Azure SQL Server からデータを取得します。

---

## 使い方

1. `PaoWebApp.sln`（Linux 互換版）または `PaoWebApp.Windows.sln`（Windows 版）を開く
2. F5 で実行
3. ブラウザで `http://localhost:5128` が自動的に開く
4. 帳票と出力形式を選んで「View」をクリック
5. SVG プレビューでページ送り、ズーム、印刷を試す

---

## 動作環境

- .NET 8 以上
- Visual Studio 2022 以降
- Azure SQL Server への接続（郵便番号以降の帳票）

### Windows 版と Linux 版

| | Windows 版 | Linux 版 |
|---|---|---|
| ソリューション | PaoWebApp.Windows.sln | PaoWebApp.sln |
| DLL | Pao.Reports.Azure.dll | Pao.Reports.Linux.dll |
| Linux 動作 | 不可 | 可 |
| Docker 対応 | なし | Dockerfile 同梱 |
| イメージ PDF | 対応 | 非対応 |

---

## ファイル構成

```
03.帳票WEBアプリケーション/
├── CS/
│   ├── PaoWebApp.sln                Linux 互換ソリューション
│   ├── PaoWebApp.Windows.sln        Windows 専用ソリューション
│   └── PaoWebApp/
│       ├── PaoWebApp.csproj          Linux 版プロジェクト
│       ├── PaoWebApp.Windows.csproj  Windows 版プロジェクト
│       ├── Program.cs                ASP.NET Core エントリポイント
│       ├── Dockerfile                Docker コンテナ化設定
│       ├── Controllers/
│       │   └── HomeController.cs     帳票生成・SVG/PDF 出力ロジック
│       ├── Views/Home/
│       │   ├── Index.cshtml          メイン画面（帳票選択・形式選択）
│       │   ├── ShowSvgPreview.cshtml SVG ページ送りプレビュー
│       │   └── ShowPdf.cshtml        PDF 表示（iframe）
│       ├── App_Data/                 帳票定義ファイル(.prepd)・画像
│       └── Reports.net.Assembly/     Docker 用 DLL フォルダ
└── VB/                               Visual Basic 版
```

---

## 技術メモ

### SVG プレビューのページ送り

`GetSvgTag(page)` は、指定ページの `<svg>` タグだけを返します。
HTML のラッパーは含まれず、SVG 要素そのものです。

```
GET /svgtag/mitsumori?page=2
→ Content-Type: image/svg+xml
→ X-Total-Pages: 5
→ <svg xmlns="http://www.w3.org/2000/svg" ...>...</svg>
```

JavaScript の `fetch()` でこのエンドポイントを呼び出し、
レスポンスの SVG を `innerHTML` に差し込むだけでページが切り替わります。
ページの総数は HTTP ヘッダ `X-Total-Pages` で取得できます。

### 印刷

```javascript
// 現在のページだけ印刷
window.print();

// 全ページ印刷: 新しいウィンドウに全SVGを集めて印刷
var w = window.open('', '_blank');
// 全ページのSVGを順次 fetch して結合
w.document.write(allSvgTags);
w.print();
```

現在ページの印刷は `window.print()` を呼ぶだけです。
全ページ印刷では、全ページの SVG を `page-break-after` 付きの HTML にまとめ、
新しいウィンドウで `window.print()` を実行します。

### Docker デプロイ

Linux 版は Docker コンテナとしてデプロイできます。

```bash
docker build -t reports-webapp .
docker run -d -p 5252:8080 reports-webapp:latest
```

AWS EC2、Azure App Service、GCP Cloud Run など、
各種クラウド環境で動作確認済みです。

---

## クラウドで動作中

このサンプルは、実際に複数のクラウド環境で公開動作しています。
Index.cshtml の下部にある「外部クラウド環境」リンクから、
Azure（Windows / Linux）、AWS EC2、GCP 上で動作するインスタンスにアクセスできます。

SVG プレビューの操作感を、ローカル環境だけでなくクラウド上でも試してみてください。

---

## トラブルシューティング

### OneDrive による IIS Express エラー

**症状**
- ソリューション(.sln)を開くとプロジェクトが「アンロード済み」として灰色表示される
- エラー: 構成ファイル `redirection.config` を読み取れません

**原因**
OneDrive が Documents フォルダーを管理している場合、IIS Express の設定ファイルが「オンラインのみ」状態になり、ローカルで利用できなくなることがあります。Windows Update 後に発生しやすい問題です。

> .NET 5 以降でも Visual Studio のデバッグ実行時は IIS Express が使用されるため、同様の問題が発生します。

**解決方法**
1. エクスプローラーを開く
2. 以下のパスを確認:
   - `C:\Users\[ユーザー名]\OneDrive\Documents\IISExpress`
   - `C:\Users\[ユーザー名]\Documents\IISExpress`
3. OneDrive 配下の `IISExpress` フォルダーを右クリック
4. 「このデバイス上に常に保持する」を選択
5. パソコンを再起動

### デプロイ時に App_Data がアップロードできない場合

2025年以降、Visual Studio .NET 5〜9 ではデプロイ時に App_Data 内のファイルのアップロードができない障害が発生しています。

**推奨: 埋め込み方式への変更**

外部ファイルを埋め込み方式に変更すれば、この問題を完全に回避できます。
単純なサンプル / 10の倍数 / 郵便番号のサンプルは、すべて埋め込み方式を採用しており、どの環境でも動作します。他のサンプルも同じ方式に変更可能です。手法についてはソースコードをご覧ください。

| 方式 | メリット | デメリット |
|------|----------|------------|
| 埋め込み | 確実に動作、デプロイ簡単 | ファイルサイズ増加、更新時要コンパイル |
| ファイル | 軽量、更新が容易 | デプロイ設定が必要、.NET5〜9 で障害発生 |

**それでもファイル方式を使う場合の手動アップロード方法**

Windows 版 Azure App Service の場合:
1. Azure Portal で対象の App Service を開く
2. 「開発ツール」→「高度なツール」→「移動」で Kudu を起動
3. コンソールで `cd site/wwwroot` に移動
4. 上部のファイラー画面にローカルの App_Data フォルダをドラッグ＆ドロップ

Linux 版 Azure App Service の場合:
1. Azure ポータルで「構成」→「全般設定」→「FTP 基本認証の発行資格情報」を「FTPS のみ」に変更
2. 「デプロイ」→「デプロイ センター」→「FTPS 資格情報」で認証情報を取得
3. WinSCP で FTP 接続（プロトコル: FTP 暗黙の TLS/SSL、ポート: 990）
4. `/home/site/wwwroot/App_Data/` にアップロード

---

## チュートリアル動画

### Docker デプロイ
1. [.NET5 Docker - AWS-EC2上で帳票出力WEBアプリを動作させる手順](https://youtu.be/0y3K3CW7DRM)
2. [.NET6 Docker - AWS-EC2上で帳票出力WEBアプリを動作させる手順](https://youtu.be/UnPXcadLwFY)
3. [AWS ECS/ECRで帳票出力WEBアプリをDockerで動作させる手順](https://youtu.be/TQpeQGwGNmM)
4. [GCP上で帳票出力WEBアプリをDockerで動作させる手順](https://youtu.be/YFdjUg9KgFo)
5. [複数クラウドにマルチデプロイ - Azure / AWS / GCP](https://youtu.be/igApoNMri7k)

### その他のデプロイ方法
1. [AzureへのWEBアプリをデプロイ手順 / Azure SQL Server使用](https://youtu.be/6UI_pP-ws3c)
2. [Linux上で動作する帳票出力WEBアプリ作成手順 - WSL2 & Azure-Linux編](https://youtu.be/OF3y7875BGo)
3. [AWS Linux Elastic Beanstalkへデプロイ - DynamoDB使用](https://youtu.be/1wTuV2ffATg)
4. [フォルダデプロイ方式で AWS EC2へ](https://youtu.be/3SE7hLNcOo8)

---

## .NET Framework 4.7.2 版

このサンプルの .NET Framework 4.7.2 移植版は
**VS2013-VS2017/03.帳票WEBアプリ(MVC)** に用意しています。
