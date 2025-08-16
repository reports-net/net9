※Visual Studio 2022では、Shift+F7 でこのREADME.md をプレビュー表示してください。

※Visual Studio 2019以前ではMarkdownプレビュー機能はありません。  
※プレビューが必要な場合は、Visual Studio Code等の外部ツールをご利用ください。
<br>
# Reports.net WEB API サンプルアプリケーション

## フォルダとソリューション構成

```
04.帳票Web API(クラウド/オンプレミス連携)
  ├── Server.Windows.sln            - Windows用サーバー WEB API + クライアント統合
  ├── Server.Linux.sln              - Linux用サーバー WEB API + クライアント統合
  ├── README.md                     - このファイル
  │
  ├── Client/                       - 帳票出力クライアントプロジェクト
  │    ├── WinForm/                 - Windows Form版クライアント
  │    └── WPF/                     - WPF版クライアント
  │
  └── Server/                       - 帳票作成 WEB API プロジェクト
       ├── Server.Windows.csproj    - Windowsサーバ版 WEB API
       ├── Server.Linux.csproj      - Linuxサーバ版 WEB API
       └── App_Data/          - コントローラで利用する帳票デザイン・画像
```

## クイックスタート

### 🚀 ローカル開発環境での実行（推奨）

1. **ソリューションを選択して起動**:
   - `Server.Windows.sln` または `Server.Linux.sln` のいずれかを開く
   
2. **F5キーで実行**:
   - **Visual Studio 2019/2022**: **3つのプロジェクトが同時に起動**されます
   - **Visual Studio 2017以前**: 各プロジェクトを個別に起動してください

> **Visual Studio 2017以前をご利用の場合**
> 
> マルチスタートアップ機能がないため、以下の順序で個別に起動してください：
> 1. Server プロジェクトを右クリック → 「デバッグ」→「新しいインスタンスを開始」
> 2. ClientForm プロジェクトを右クリック → 「デバッグ」→「新しいインスタンスを開始」  
> 3. ClientWPF プロジェクトを右クリック → 「デバッグ」→「新しいインスタンスを開始」

3. **帳票出力**:
   - Windows Form/WPF いずれかお好みのクライアントを選択
   - 「ローカルデバッグ環境」を選択
   - 「実行 (POST)」または「実行 (GET)」ボタンをクリック

**🌐 動作確認:**
```
まずは ブラウザで http://localhost:5555/Pao
```
WEB APIが正常動作していればJSONレスポンスが表示されます。

### ☁️ クラウド環境利用

1. **クライアントのみ起動**:
   - 上記の手順でクライアントが起動したら、クラウド環境を選択：
     - Azure Windows Server
     - Azure Linux
     - AWS EC2 Amazon Linux 2023
     - GCP GCE Debian

2. **帳票出力**:
   - 「実行 (POST)」または「実行 (GET)」ボタンをクリック

---

## 🐳 Docker コンテナ化（Linux版のみ）

Linux版サーバーはDockerコンテナとして動作させることができます。

### 🚀 クイックスタート（Visual Studio）

1. **ソリューションエクスプローラー**で `Server/Server.Linux/Dockerfile` を右クリック
2. **「Dockerイメージのビルド」**を選択
3. 自動的にイメージが作成されます

**📋 作成されたイメージの確認:**
```bash
docker images
# 出力例:
# REPOSITORY        TAG       IMAGE ID       CREATED        SIZE
# serverlinux       latest    1887bcb05197   14 hours ago   264MB
```

**🚀 コンテナ起動:**
```bash
# ↑で確認したREPOSITORY名を使用
docker run -d -p 5151:8080 --name reports-api serverlinux:latest
```

### ⚡ コマンドライン（上級者向け）

```bash
# プロジェクトルートで実行
cd Server
docker build -t serverlinux-webapi .

# コンテナ起動
docker run -d -p 5151:8080 --name reports-api serverlinux-webapi:latest
```

### 🔧 クライアント接続設定

Dockerコンテナに接続する場合、クライアントコードのポート番号調整が必要です：

**GetWebApiUrl()メソッドの修正例:**
```csharp
private string GetWebApiUrl()
{
    // 接続先URL : デフォルトはローカルでデバッグ環境
    string url = "http://localhost:5151/Pao";  // Dockerコンテナ用ポート
    
    if (radWebApiAzureWin.Checked)
    {
        url = "https://are-are.azurewebsites.net/Pao";
    }
    // ... 他のクラウド設定
    
    return url;
}
```

**ポート番号の使い分け:**

| 環境 | ポート | 用途 |
|------|--------|------|
| IIS Express | 5555 | Visual Studio F5デバッグ |
| Docker | 5151 | コンテナ動作（例） |
| クラウド | 80/443 | 本番環境 |

### 🌐 動作確認

```bash
# コンテナ起動後
curl http://localhost:5151/Pao
# または ブラウザで http://localhost:5151/Pao
```

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

## 処理の流れ

1. クライアント（Windows Form/WPF）→(印刷データ作成依頼)→サーバサイド(WEB API)
2. サーバサイド(WEB API)→(印刷データ)→クライアント→帳票出力
という流れになります。

このソリューションには、WEB APIサーバーと2種類のクライアント（Windows Form版・WPF版）が含まれています。

デフォルトでは、ソリューションのマルチプルスタートアップ設定により、F5キーを押すことで
「WEB API サーバ」「Windows Form版クライアント」「WPF版クライアント」の3つが同時に起動されます。
Windows Form/WPF いずれかお好みのクライアントをご利用ください。

※WEB APIプロジェクトが先に起動し、ブラウザで「Swagger UI」が表示されます。

プログラム起動後は、WEB APIの配置場所を選択してください：
[ローカル / Azure Windows / Azure Linux / AWS / GCP]

配置場所と「出力帳票の種類」を選択後、「実行 (POST)」「実行 (GET)」ボタンをクリックすると
WEB API (REST API)から印刷データを取得し、帳票出力を行います。

※WEB APIソリューションはLinux版(クロスプラットフォーム対応)とWindows版をご用意しています。

```
Windows Forms/WPF
クライアント ──[印刷データ作成依頼]─→ WEB API (REST API)
     ↓                                 │
【帳票出力】 ←──────[印刷データ]─────────┘
```

## Windows版とLinux版のWEB APIの違い

| 機能・特性 | Windows版 | Linux版 |
|------------|-----------|---------|
| 参照DLL    | Pao.Reports.Azure.dll | Pao.Reports.Linux.dll |
| Windows上での動作 | ✅ | ✅ |
| Linux上での動作   | ❌ | ✅ |
| イメージPDF出力   | ✅ | ❌ |

> 📝 **Note**: Windows版がLinux上で動作しない理由は、System.Drawingの依存関係によるものです。

## よくある質問 (FAQ)

**Q: どのソリューションファイルを選べばよいですか？**  
A: どちらのソリューションも同じクライアントが含まれています。Windows専用環境なら `Server.Windows.sln`、クロスプラットフォーム対応なら `Server.Linux.sln` を選択してください。

**Q: 3つのプロジェクトが同時に起動しますが、どれを使えばよいですか？**  
A: WEB API（Swagger UI）は開発用インターフェースです。実際の帳票出力には Windows Form または WPF のいずれかお好みのクライアントをご利用ください。

**Q: サーバーサイドのWEB APIを起動するとブラウザが開きますが、何をすればよいですか？**  
A: これはSwagger UIという開発用インターフェースです。特に操作は不要で、クライアントアプリから利用されます。

**Q: クラウド環境のAPIにアクセスできない場合は？**  
A: クラウド環境は予告なく停止する場合があります。その場合は、ローカル環境でサーバーを実行してください。

**Q: プロジェクトが起動しない場合は？**  
A: 上記の「トラブルシューティング」セクションをご確認ください。OneDriveによるIIS Express問題が最も多い原因です。

**Q: ローカル環境で接続エラーが発生します**  
A: WEB APIサーバーが起動していることを確認してください。マルチプルスタートアップ設定により、通常は自動的に起動されます。

**Q: Dockerコンテナが起動しません**  
A: `docker logs コンテナ名` でエラーログを確認してください。ポート競合（既に5151番使用中）の場合は別のポート番号を使用してください。

**Q: Docker化したWEB APIにクライアントが接続できません**  
A: クライアントコードの接続先ポート番号を確認してください。IIS Express（5555）とDocker（5151等）でポート番号が異なります。

## チュートリアル動画

1. [最短/最速 REST API実装 GET編](https://youtu.be/cYEtHFpa8G4)
2. [最短/最速 REST API実装 POST編](https://youtu.be/EflMRmMYU4A)
3. [Visual Studio から IISへデプロイ手順](https://youtu.be/xHNLlPuMFEs)
4. [REST APIで帳票作成](https://youtu.be/Bolfww56aWY)
5. [REST APIで帳票作成 SQL Server編](https://youtu.be/VNeD7w3LdV0)
6. [複数クラウドにマルチデプロイ](https://youtu.be/KW_RK8PmXro)

---

> 📝 **Note**: このサンプルは**WEB API**用です。WEBアプリケーション サンプルとは異なりますのでご注意ください。

----

© Reports.net - クラウド対応帳票出力ソリューション