※Visual Studio 2022では、Shift+F7 でこのREADME.md をプレビュー表示してください。

※Visual Studio 2019以前ではMarkdownプレビュー機能はありません。  
※プレビューが必要な場合は、Visual Studio Code等の外部ツールをご利用ください。
<br>
# Reports.net WEB アプリケーション

## 概要

Reports.net WEB アプリケーションは、ブラウザ上で直接帳票を作成・表示できる Web アプリケーションです。Windows および Linux 環境で動作し、Docker コンテナ化やクラウドデプロイにも対応しています。

## フォルダとソリューション構成

```
03.帳票WEBアプリケーション(各種クラウド公開動作中)
  ├── PaoWebApp.sln                 - Linux互換 Web アプリケーション
  ├── PaoWebApp.Windows.sln         - Windows専用 Web アプリケーション
  └── PaoWebApp/                    - WEBアプリプロジェクト一式
       ├── PaoWebApp.csproj         - Linuxサーバ版 WEBアプリ
       ├── PaoWebApp.Windows.csproj - Windowsサーバ版 WEBアプリ
       ├── App_Data/                - コントローラで利用する帳票デザイン・画像
       ├── Dockerfile               - Docker コンテナ化用設定ファイル
       └── Reports.net.Assembly/    - Docker 参照用アセンブリフォルダ(遠いパスでは参照ができないため)
 ```

## クイックスタート

1. **ソリューションを開く**:
   - `PaoWebApp.sln` (Linux互換版) または
   - `PaoWebApp.Windows.sln` (Windows専用版)

2. **実行**:
   - F5キーまたは実行ボタンをクリック
   - 自動的にブラウザが起動し、アプリケーションが表示されます

3. **帳票の作成・表示**:
   - 画面の指示に従って帳票を作成
   - ブラウザ上で直接プレビューまたはダウンロード

**🌐 動作確認:**
```
まずは ブラウザで http://localhost:5128
```
Reports.net WEBアプリケーションのホーム画面が表示され、帳票作成機能が利用できます。

---

## 🐳 Docker コンテナ化（Linux版のみ）

Linux版アプリケーションはDockerコンテナとして動作させることができます。

### 🚀 クイックスタート（Visual Studio）

1. **ソリューションエクスプローラー**で `Dockerfile` を右クリック
2. **「Dockerイメージのビルド」**を選択
3. 自動的にイメージが作成されます

**📋 作成されたイメージの確認:**
```bash
docker images
# 出力例:
# REPOSITORY        TAG       IMAGE ID       CREATED              SIZE
# paowebapp         latest    587a3f3ac9a2   About a minute ago   250MB
```

**🚀 コンテナ起動:**
```bash
# ↑で確認したREPOSITORY名を使用
docker run -d -p 5252:8080 --name webapp-container paowebapp:latest
```

### ⚡ コマンドライン（上級者向け）

```bash
# プロジェクトルートディレクトリで実行
docker build -t reports-webapp .

# コンテナ起動
docker run -d -p 5252:8080 --name webapp-container reports-webapp:latest
```

### 🌐 動作確認

```bash
# コンテナ起動後
curl http://localhost:5252
# または ブラウザで http://localhost:5252
```

**ポート番号の使い分け:**

| 環境 | ポート | 用途 |
|------|--------|------|
| IIS Express | 5128 | Visual Studio F5デバッグ |
| Docker | 5252 | コンテナ動作（例） |
| クラウド | 80/443 | 本番環境 |

> 💡 **Tips**: クラウドデプロイ時は、このDockerイメージをそのまま使用できます（AWS/Azure/GCP対応済み）

> 📦 **移植性**: 作成したDockerイメージは `docker save` / `docker load` コマンドを用いて他の環境に移植できます。クラウドサーバーや別の開発環境への展開が簡単に行えます。詳細はDockerコマンドをご参照ください。

---

## 🚨 トラブルシューティング

### プロジェクトが読み込めない・起動できない場合

#### A. 【最優先】OneDriveによるIIS Express エラー（2025年現在の主要原因）

**🎯 症状**
- ソリューション(.sln)を開くとプロジェクトが「アンロード済み」として灰色表示される
- プロジェクトを直接開くとエラー発生：
  - ファイル名: redirection.config
  - エラー: 構成ファイルを読み取れません

**🔍 原因**  
OneDriveがDocumentsフォルダーを管理している場合、IIS Expressの設定ファイルが「オンラインのみ」状態になり、ローカルで利用できなくなることがある。Windows Update後に発生しやすい問題です。

> ⚠️ **重要**: .NET 5以降でもVisual Studioのデバッグ実行時はIIS Expressが使用されるため、同様の問題が発生します。

**✅ 解決方法（推奨⭐）**
1. エクスプローラーを開く
2. 以下のパスを確認：
   - `C:\Users\[ユーザー名]\OneDrive\Documents\IISExpress`
   - `C:\Users\[ユーザー名]\Documents\IISExpress`
3. `C:\Users\[ユーザー名]\OneDrive\Documents\IISExpress`フォルダーを右クリック
4. 「このデバイス上に常に保持する」を選択
5. パソコンを再起動

#### B. 🚀 デプロイ時にApp_Dataがアップロードできないときの対処法

**⚠️ 既知の問題**  
2025年以降(2025年7月現在)Visual Studio .NET 5～9以降では、デプロイ時にApp_Data内のファイルのアップロードができない障害が多発しています。このサンプルプログラムでもそのままデプロイすると動作しない帳票がございます。

**🌟 最強の回避方法：埋め込み方式への変更**

外部ファイルを埋め込み方式に変更すれば、この障害を完全に回避できます！

**✨ 埋め込み方式サンプル**
- **単純なサンプル** / **10の倍数** / **郵便番号** のサンプルは、すべて外部ファイル埋め込み方式を採用
- **どんな環境でも必ず動作します**！💪
- 他のサンプルも同じ方式に変更すれば、確実に出力できるようになります。手法についてはソースコードをご覧ください。

**💡 開発者向けヒント：埋め込み方式 vs ファイル方式**

| 方式 | メリット | デメリット |
|------|----------|------------|
| 埋め込み | 🟢 確実動作 🟢 デプロイ簡単 🟢 障害回避 | 🔴 ファイルサイズ増加 🔴 更新時要コンパイル |
| ファイル | 🟢 軽量 🟢 更新が容易 | 🔴 デプロイ設定必要 🔴 環境依存あり 🔴 .NET5～9で障害発生 |

> 💡 **Pro Tips**: 埋め込み方式のサンプルコードを参考に、既存のサンプルを変更すると開発効率がアップします！

**🛠️ それでもファイル方式を使いたい場合の手動アップロード方法**

##### 【Windows版 Azure App Service の場合】

**📋 ステップ・バイ・ステップ**

1. **Azure Portal** にアクセス
2. 対象の **App Service** をクリック
3. 左メニューから **「開発ツール」** → **「高度なツール」** → **「移動」**
4. **Kudu** の画面が起動します ✨
5. コンソールで `cd site/wwwroot` に移動 (文字入力ができない場合、Ctrl+Vで文字列貼り付け)
6. 上部のファイラー画面に **ローカルの App_Data フォルダ**をドラッグ＆ドロップ 🎉

**🎯 成功のコツ**
- ドラッグ＆ドロップは上部のファイラー部分（フォルダ一覧が表示されている箇所）に行ってください
- フォルダ全体を一度にアップロードできます
- アップロード完了後、すぐに帳票機能が利用可能になります

##### 【Linux版 Azure App Service の場合】(Windows版でもこの方式は使えると思います)

**🔧 解決手順:**
1. **Azure ポータル**で対象のWeb Appを開く
2. **「構成」** → **「全般設定」** → **「FTP 基本認証の発行資格情報」** を **「FTPS のみ」** に変更して保存
3. **「デプロイ」** → **「デプロイ センター」** → **「FTPS 資格情報」** で認証情報を取得：
   - FTPS エンドポイント
   - ユーザー名  
   - パスワード 

※Azure のメニューの項目名などは変更になることがあります。

**💻 WinSCP での接続:**
- **プロトコル:** FTP 暗黙のTLS/SSL暗号化
- **ホスト名:** 上記で取得したFTPS エンドポイント
- **ポート:** 990
- **ユーザー名・パスワード:** 上記で取得した認証情報

**📁 アップロード先パス:** `/home/site/wwwroot/App_Data/`

> 💡 **Pro Tips**: 
> - Windows版：Kuduでドラッグ&ドロップが最も簡単 🚀
> - Linux版：FTP接続後、App_Dataフォルダごとドラッグ&ドロップで一括アップロード可能
> - 今後のファイル更新もそれぞれの方法で簡単に行えます

---

## Linux版とWindows版の違い

| 機能・特性 | Windows版 | Linux版 |
|------------|-----------|---------|
| 参照DLL    | Pao.Reports.Azure.dll | Pao.Reports.Linux.dll |
| Windows上での動作 | ✅ | ✅ |
| Linux上での動作   | ❌ | ✅ |
| イメージPDF出力   | ✅ | ❌ |

> 📝 **Note**: Windows版を選択する主な理由は「イメージPDF出力」機能が必要な場合です。それ以外の用途では、より互換性の高いLinux版を推奨します。

## クラウドデプロイ

このWebアプリケーションは、様々なクラウド環境へのデプロイに対応しています：

### AWS
- EC2 (Windows/Linux)
- Elastic Beanstalk
- ECS/ECR (Dockerコンテナ)

### Azure
- App Service (Windows/Linux)
- Azure Container Instances

### GCP
- GCE (Compute Engine)
- Cloud Run (Dockerコンテナ)

## よくある質問 (FAQ)

**Q: どのソリューションファイルを選べばよいですか？**  
A: 基本的には `PaoWebApp.sln` (Linux互換版) がおすすめです。イメージPDF出力が必要な場合のみ `PaoWebApp.Windows.sln` を選択してください。

**Q: Windows版とLinux版の選択基準は？**  
A: Linux版は互換性が高くほとんどの環境で動作します。Windows版はイメージPDF出力が必要な場合や、Windows専用の機能を使用する場合に選びます。

**Q: Docker化する際の注意点は？**  
A: Visual Studioの不具合による参照パスの問題を避けるため、必要なDLLは `Reports.net.Assembly` フォルダにコピーしています。このフォルダを削除すると Docker ビルドが失敗する可能性があります。

**Q: クラウドにデプロイするための推奨方法は？**  
A: 複数の方法がありますが、Docker コンテナ化して各クラウドのコンテナサービスにデプロイするのが最も移植性が高く推奨されます。

**Q: プロジェクトが起動しない場合は？**  
A: 上記の「トラブルシューティング」セクションをご確認ください。OneDriveによるIIS Express問題が最も多い原因です。

**Q: Dockerコンテナが起動しません**  
A: `docker logs コンテナ名` でエラーログを確認してください。ポート競合（既に5252番使用中）の場合は別のポート番号を使用してください。

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

## アーキテクチャ概要

```
ブラウザ ───┬──[HTTP/HTTPS]──→ WEB アプリケーション
            │                    │ 
            │                    ├─[帳票データ処理]
            │                    │ 
            └──[帳票表示]←───────┘
               (PDF/SVG/XPS)
```

---

> 📝 **Note**: このサンプルは**WEBアプリケーション**用です。WEB API サンプルとは異なりますのでご注意ください。

----

© Reports.net - クラウド対応帳票出力ソリューション