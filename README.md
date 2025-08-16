# Reports.net サンプルプログラム集

> **一つのソリューションで “プレビュー → PDF/XPS/印刷” まで最短ルート**。目的別に 01 〜 05 のサンプルを並べ、段階的に学べる構成にしています。

---

## 目次

1. [フォルダ早見表](#フォルダ早見表)
2. [クイックスタート](#クイックスタート)
3. [FAQ](#faq)
4. [動画で学ぶ Reports.net](#動画で学ぶ-reportsnet)

---

## フォルダ早見表

|    #   | フォルダ                               | サンプルの目的                                     | ひと言まとめ                                              |
| :----: | :--------------------------------- | :------------------------------------------ | :-------------------------------------------------- |
| **01** | **01.簡単なサンプル – あっという間に帳票出力**       | 10 行コードで PDF まで出力できる**超入門**                 | “まずは 1 分で動かしたい” 人向け                                 |
| **02** | **02.実践帳票サンプル集 – 様々な帳票手法**         | 請求書・送り状など**実務帳票**テンプレ一式                     | テンプレ改造で即プロトタイプ完成                                    |
| **03** | **03.帳票WEBアプリケーション – 各種クラウド公開動作中** | MVC/Razor Pages で **ブラウザ完結** 帳票             | [詳細 README](03.帳票WEBアプリケーション－各種クラウド公開動作中\CS\README.md) |
| **04** | **04.帳票作成WEB API – 各種クラウドに配置・連携中** | REST API で帳票生成、Docker & 多クラウド対応             | [詳細 README](04.帳票作成WEB%20API－各種クラウドに配置・連携中\CS\README.md) |
| **05** | **05.XAML (WPF 版プレビュー → XPS 出力)**  | **WPF プレビュー画面を XAML で自在にデザイン**し XPS/PDF に出力 | [プレビュー画面デザイン変更方法.pdf](https://www.pao.ac/reports.net/file/preview-design-guide.pdf) |

> 💡 **動作環境** .NET 8 SDK / Visual Studio 2022 17.9+ / Office 64‑bit（Excel DB サンプルのみ）など。詳細は [トラブルシューティング.md](トラブルシューティング.md) を参照してください。

---

## クイックスタート

1. **フォルダを選択**して `.sln` を Visual Studio で開いてください。
2. **ビルド／実行** – F5。
3. **帳票を確認** – プレビュー/プリンター出力/PDF/XPSをお試しください。

> Docker 派の方は `03` または `04` 配下の `Dockerfile` で `docker build` → `docker run` で体験できます。

---

## FAQ

| 質問                       | 回答                                                                   |
| ------------------------ | -------------------------------------------------------------------- |
| エラーで起動しない                | **必須ランタイム** が不足していないか、[トラブルシューティング.md)](トラブルシューティング.md) を参照 |
| Windows と Linux どちらを選べば？ | 画像を含む **イメージ PDF 出力** が必要な場合 Windows 版、それ以外は互換性の高い Linux 版がおすすめ       |
| クラウドへデプロイしたい             | `03`/`04` の README に Azure/AWS/GCP への手順と動画リンクがあります                   |

---

## 動画で学ぶ Reports.net

【学習リソースのご案内】  
Reports.net の様々な活用方法について、動画や解説をご用意しております。  
以下のコンテンツでは：

- Linux や Windows 環境での帳票出力方法  
- Docker やクラウドへのアプリ展開  
- .NET Web API による帳票出力の自動化  
- WPF／WinForms UI との連携とカスタマイズ  
- 初心者からプロまで役立つ開発テクニック  

【公式チャンネル】  
- 動画サイト: https://www.youtube.com/@pao-jp  

# Reports.net できること動画集 🎬

> **短時間で「できる」が増える！**  
> ハンズオン形式の実践動画で帳票開発のノウハウを紹介します。

---

「01 基本の帳票作成手順」は、最初にご覧ください。  
他の動画は、興味を惹かれた動画から見ていただいて構いません。  
上から順ではなく「逆に高度なもの」が上の方に来ている場合もあります。  
まずは「できるんだ！」と感じてもらえることを重視しています。

---

## コンテンツ一覧

| #  | タイトル | 概要 | リンク |
|----|----------|------|--------|
| 01 | **基本の帳票作成手順** | 最初に見るべき一本！帳票作成・出力の基礎 | [▶️ YouTube](https://youtu.be/I0XQq4VYO7U) |
| 02 | **WPF 印刷プレビューのUI変更** | デザイン自在なWPF印刷プレビュー画面を構築 | [▶️ YouTube](https://youtu.be/mFf64CehJEY) |
| 03 | **マルチクラウドにWEBアプリ公開** | Azure / AWS / GCP へ帳票WEBアプリ配置 | [▶️ YouTube](https://youtu.be/igApoNMri7k) |
| 04 | **マルチクラウドに WEB API デプロイ** | Azure / AWS / GCP へ帳票 WEB API 配置 | [▶️ YouTube](https://youtu.be/KW_RK8PmXro) |
| 05 | **AWS EC2へ簡単デプロイ** | フォルダ単位で手軽にデプロイ | [▶️ YouTube](https://youtu.be/3SE7hLNcOo8) |
| 06 | **Docker → AWS EC2** | コンテナで動くPDF出力アプリ (AWS) | [▶️ YouTube](https://youtu.be/UnPXcadLwFY) |
| 07 | **GCPでDockerアプリ動作** | コンテナで動くPDF出力アプリ (GCP) | [▶️ YouTube](https://youtu.be/YFdjUg9KgFo) |
| 08 | **AWS ECS/ECR で運用** | AWS ECS/ECR 方式 | [▶️ YouTube](https://youtu.be/TQpeQGwGNmM) |
| 09 | **AWS EC2 + Docker** | 最もシンプルな方式 | [▶️ YouTube](https://youtu.be/0y3K3CW7DRM) |
| 10 | **AWS Elastic Beanstalk へ公開** | Linuxホスティングへのデプロイ方式 | [▶️ YouTube](https://youtu.be/1wTuV2ffATg) |
| 11 | **Linux上の.NET帳票出力** | WSL2 & Azure-Linux編 | [▶️ YouTube](https://youtu.be/OF3y7875BGo) |
| 12 | **Azure SQL連携アプリ構築** | Webアプリ+PDF出力+Azure SQL Server | [▶️ YouTube](https://youtu.be/6UI_pP-ws3c) |
| 13 | **REST API + WinForms出力** | WEB API + SQL Server に接続してPDF化 | [▶️ YouTube](https://youtu.be/VNeD7w3LdV0) |
| 14 | **REST API (別バージョン)** | WEB APIを使ってWinFormsから出力 | [▶️ YouTube](https://youtu.be/Bolfww56aWY) |
| 15 | **WEB API を IIS へ公開** | Visual Studio から簡単デプロイ | [▶️ YouTube](https://youtu.be/xHNLlPuMFEs) |
| 16 | **REST API 最短実装（POST編）** | POST WEB API 実装 | [▶️ YouTube](https://youtu.be/EflMRmMYU4A) |
| 17 | **REST API 最短実装（GET編）** | GET WEB API 実装 | [▶️ YouTube](https://youtu.be/cYEtHFpa8G4) |

> 🔔 **新着動画は YouTube の再生リストに順次追加中！**  
> チャンネル登録でアップデートを見逃さずに！
---

## 公式サイト Pao At Office

[**Pao At Office**](https://www.pao.ac) では、Reports.net の最新版ダウンロード、ドキュメント、価格情報、掲示板(FAQ) などを公開しています。

> **サポート窓口:** サイトの「お問い合わせ」[**掲示板**](https://www.pao.ac/cgi-bin/bbs_new/reports.net/yybbs.cgi)または [info@pao.ac](mailto:info@pao.ac) までお気軽にどうぞ。
