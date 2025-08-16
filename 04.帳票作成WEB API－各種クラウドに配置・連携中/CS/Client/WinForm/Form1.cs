using Pao.Reports;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {

        public class PrintData
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("data")]
            public string Data { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private string GetWebApiUrl()
        {
            // 接続先URL : デフォルトはローカルでデバッグ環境
            string url = "http://localhost:5555/Pao";

            // ※Azure Win / Azure Linux / AWS EC2 / GCP GCE 外部クラウドのURLは、
            // 変更になる可能性があります。
            // ※接続できないときは、次のサイトに各最新のURLを記載しておきますので
            // http://www.pao.ac/reports.net/cloud-url/
            // ※下の各クラウドのURLを上記サイト内に記載れているURLに書き換えてお試しください。

            if (radWebApiAzureWin.Checked)
            {
                // Azure Windowsサーバに配置してある WEB API を使用
                url = "https://are-are.azurewebsites.net/Pao";
            }
            else if (radWebApiAzureLinux.Checked)
            {
                // Azure Linuxサーバ上に配置してある WEB API を使用
                url = "https://hoi-hoi.azurewebsites.net/Pao";
            }
            else if (radWebApiAws.Checked)
            {
                // AWS EC2 Amazon Lnux 2サーバ上に配置してある WEB API を使用
                //url = "http://ec2-43-207-4-252.ap-northeast-1.compute.amazonaws.com:5001/Pao";

                //AWS EC2 は、URLが変更になるため 弊社サイト上のJSON から現在のURLを取得
                url = ResolveAwsUrl();
            }
            else if (radWebApiGcp.Checked)
            {
                // GCP GCE Debian GNU/Linuxサーバ上に配置してある WEB API を使用
                url = "http://34.145.123.177:5000/Pao";
            }

            return url;
        }

        private static readonly HttpClient _hc = new HttpClient();
        private static string _awsUrlCache; // キャッシュ
        private string ResolveAwsUrl()
        {
            if (!string.IsNullOrEmpty(_awsUrlCache)) return _awsUrlCache;

            const string jsonUrl = "https://www.pao.ac/reports.net/redirect-azure-aws-gcp/aws_api.json";
            const string fallback = "http://ec2-43-207-4-252.ap-northeast-1.compute.amazonaws.com:5001/Pao";

            // aws_api.json内は、こういう形式
            // {"api":"http://ec2-43-207-4-252.ap-northeast-1.compute.amazonaws.com:5001/Pao"}


            try
            {
                // 同期的に取得（UIスレッドデッドロック回避用に ConfigureAwait(false)）
                var json = _hc.GetStringAsync(jsonUrl).ConfigureAwait(false).GetAwaiter().GetResult();

                using (var doc = JsonDocument.Parse(json))
                {
                    if (doc.RootElement.TryGetProperty("api", out var apiElem))
                    {
                        var api = apiElem.GetString();
                        if (!string.IsNullOrWhiteSpace(api))
                        {
                            _awsUrlCache = api;
                            return api;
                        }
                    }
                }
            }
            catch { /* 無視してフォールバック */ }

            return fallback;
        }


        /// <summary>
        /// GETクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGet_Click(object sender, EventArgs e)
        {

            // WEB API (REST API) の URL 取得
            string webApiUrl = GetWebApiUrl();


            // WEB API  (REST API) のURLセット
            HttpClient Client = new()
            {
                BaseAddress = new Uri(webApiUrl)
            };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            int id = 0;
            if (opt単純な印刷データ.Checked) id = 0;
            if (opt10の倍数.Checked) id = 1;
            if (opt住所一覧.Checked) id = 2;
            if (opt見積書.Checked) id = 3;
            if (opt請求書.Checked) id = 4;
            if (opt商品一覧.Checked) id = 5;
            if (opt広告.Checked) id = 6;

            // パラメータに Json形式でIDをセットして、帳票データをPrinntData型で取得
            PrintData pd = await Client.GetFromJsonAsync<PrintData>("Pao?Id=" + id.ToString());

            byte[] data = Convert.FromBase64String(pd.Data);

            Output帳票(data);

        }

        /// <summary>
        /// POSTクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPost_Click(object sender, EventArgs e)
        {

            // WEB API (REST API) の URL 取得
            string webApiUrl = GetWebApiUrl();

            // WEB API  (REST API) のURLセット
            RestClient client = new(webApiUrl);

            RestRequest request = new(Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            PrintData pdIn = new();

            if (opt単純な印刷データ.Checked) pdIn.Id = 0;
            if (opt10の倍数.Checked) pdIn.Id = 1;
            if (opt住所一覧.Checked) pdIn.Id = 2;
            if (opt見積書.Checked) pdIn.Id = 3;
            if (opt請求書.Checked) pdIn.Id = 4;
            if (opt商品一覧.Checked) pdIn.Id = 5;
            if (opt広告.Checked) pdIn.Id = 6;

            // パラメータに PrintData型をJsonシリアライズしてセット
            request.AddParameter("application/json"
                , JsonSerializer.Serialize(pdIn)
                , ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            // Json形式で戻ってきた帳票データを PrintData型にデシリアライズ
            PrintData pdOut = JsonSerializer.Deserialize<PrintData>(response.Content);

            byte[] data = Convert.FromBase64String(pdOut.Data);


            Output帳票(data);

        }

        private void Output帳票(byte[] data)
        {

#if NET5_0 || NET6_0 || NET7_0
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif

            //IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            IReport paoRep = null;

            if (radPreview.Checked) //ラジオボタンでプレビューが選択されている場合
            {
                //プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreview();
            }
            else if (radPrint.Checked) // 印刷が選択されている場合
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }
            else if (radPDF.Checked) // PDFが選択されている場合
            {
                if (!opt広告.Checked)
                {
                    //PDF出力オブジェクトのインスタンスを獲得
                    paoRep = ReportCreator.GetPdf();
                }
                else
                {
                    //イメージPDF出力オブジェクトのインスタンスを獲得
                    paoRep = ReportCreator.GetImagePdf();
                }
            }
            else
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }


            paoRep.LoadData(data); //印刷データを読み込む


            if (!radPDF.Checked) //PDF出力が選択されていない場合
            {
                paoRep.Output(); //印刷または、プレビューを実行

            }
            else
            {

                saveFileDialog.FileName = "印刷データ";
                saveFileDialog.DefaultExt = ".pdf";
                saveFileDialog.Filter = "PDF Document (.pdf)|*.pdf"; // Filter files by extension
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // Show save file dialog box
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;


                //PDFの保存
                paoRep.SavePDF(saveFileDialog.FileName);

                if (MessageBox.Show("保存した PDF を表示しますか？", "PDF の表示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var startInfo = new System.Diagnostics.ProcessStartInfo(saveFileDialog.FileName)
                    {
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(startInfo);
                }


            }

        }

    }
}
