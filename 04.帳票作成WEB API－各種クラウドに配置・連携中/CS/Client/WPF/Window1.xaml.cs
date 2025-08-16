using Newtonsoft.Json.Linq;
using Pao.Reports;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sample
{


    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class Window1 : Window
    {
        public class PrintData
        {
            public int id { get; set; }
            public string data { get; set; }
        }

        public Window1()
        {

            InitializeComponent();

            this.Height = SystemParameters.PrimaryScreenHeight - 50;

        }

        /// <summary>
        /// GETクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExecuteGet_Click(object sender, RoutedEventArgs e)
        {
            // WEB API (REST API) の URL 取得
            string webApiUrl = GetWebApiUrl();
            // WEB API  (REST API) のURLセット
            HttpClient client = new()
            {
                BaseAddress = new Uri(webApiUrl)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            int id = GetReportTypeId();

            // パラメータに Json形式でIDをセットして、帳票データをPrintData型で取得
            PrintData pd = await client.GetFromJsonAsync<PrintData>("Pao?Id=" + id.ToString());
            byte[] data = Convert.FromBase64String(pd.data);
            Output帳票(data);
        }

        private void ExecutePost_Click(object sender, RoutedEventArgs e)
        {
            // WEB API (REST API) の URL 取得
            string webApiUrl = GetWebApiUrl();
            // WEB API  (REST API) のURLセット
            RestClient client = new(webApiUrl);
            RestRequest request = new(Method.POST)
            {
                RequestFormat = RestSharp.DataFormat.Json
            };

            PrintData pdIn = new()
            {
                id = GetReportTypeId()
            };

            // パラメータに PrintData型をJsonシリアライズしてセット
            request.AddParameter("application/json",
                JsonSerializer.Serialize(pdIn),
                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            // Json形式で戻ってきた帳票データを PrintData型にデシリアライズ
            PrintData pdOut = JsonSerializer.Deserialize<PrintData>(response.Content);

            byte[] data = Convert.FromBase64String(pdOut.data);


            Output帳票(data);

        }

        /// <summary>
        /// レポートタイプIDを取得
        /// </summary>
        /// <returns>レポートタイプID</returns>
        private int GetReportTypeId()
        {
            ComboBoxItem selectedItem = cmbReportType.SelectedItem as ComboBoxItem;
            string selectedContent = selectedItem?.Content?.ToString() ?? string.Empty;

            if (selectedContent.Contains("単純な印刷データ"))
                return 0;
            else if (selectedContent.Contains("10の倍数"))
                return 1;
            else if (selectedContent.Contains("郵便番号一覧"))
                return 2;
            else if (selectedContent.Contains("見積書"))
                return 3;
            else if (selectedContent.Contains("請求書"))
                return 4;
            else if (selectedContent.Contains("商品大小分類"))
                return 5;
            else if (selectedContent.Contains("広告"))
                return 6;
            else
                return 0; // デフォルト
        }

        /// <summary>
        /// WEB API URL 取得
        /// </summary>
        /// <returns>WEB API URL</returns>
        private string GetWebApiUrl()
        {
            // 接続先URL : デフォルトはローカルでデバッグ環境
            string url = "http://localhost:5555/Pao";

            // ※Azure Win / Azure Linux / AWS EC2 / GCP GCE 外部クラウドのURLは、
            // 変更になる可能性があります。
            // ※接続できないときは、次のサイトに各最新のURLを記載しておきますので
            // http://www.pao.ac/reports.net/cloud-url/
            // ※下の各クラウドのURLを上記サイト内に記載れているURLに書き換えてお試しください。

            if (radAzureWin.IsChecked == true)
            {
                // Azure Windowsサーバに配置してある WEB API を使用
                url = "https://are-are.azurewebsites.net/Pao";
            }
            else if (radAzureLinux.IsChecked == true)
            {
                // Azure Linuxサーバ上に配置してある WEB API を使用
                url = "https://hoi-hoi.azurewebsites.net/Pao";
            }
            else if (radAws.IsChecked == true)
            {
                // AWS EC2 Amazon Lnux 2サーバ上に配置してある WEB API を使用
                //url = "http://ec2-43-207-4-252.ap-northeast-1.compute.amazonaws.com:5001/Pao";

                //AWS EC2 は、URLが変更になるため 弊社サイト上のJSON から現在のURLを取得
                url = ResolveAwsUrl();

            }
            else if (radGcp.IsChecked == true)
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
        /// 帳票出力処理
        /// </summary>
        /// <param name="data"></param>
        private void Output帳票(byte[] data)
        {

#if NET5_0 || NET6_0 || NET7_0
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
            int repID = GetReportTypeId();

            //IReport インターフェースで宣言(印刷・レポートどちらでも使える入れ物の用意)
            IReport paoRep = null;

            // ■インスタンスの生成がエラーとなる場合の対処法：下記コメントを外してください
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if (radPreview.IsChecked == true) //ラジオボタンでプレビューが選択されている場合
            {
                //プレビューオブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetPreview();
            }
            else if (radPrint.IsChecked == true) // 印刷が選択されている場合
            {
                //印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }
            else if (radPDF.IsChecked == true) // PDFが選択されている場合
            {
                if (!cmbReportType.Text.Contains("広告"))
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
                //SVGとWPSは、印刷オブジェクトのインスタンスを獲得
                paoRep = ReportCreator.GetReport();
            }

            paoRep.LoadData(data); //印刷データを読み込む


            if (!radPDF.IsChecked == true) //PDF出力が選択されていない場合
            {
                paoRep.Output(); //印刷または、プレビューを実行

            }
            else
            {
                Microsoft.Win32.SaveFileDialog dlg = new()
                {
                    FileName = "印刷データ",
                    DefaultExt = ".pdf",
                    Filter = "PDF Document (.pdf)|*.pdf", // Filter files by extension
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == false) return;


                //PDFの保存
                paoRep.SavePDF(dlg.FileName);

                if (System.Windows.MessageBox.Show("保存した PDF を表示しますか？", "PDF の表示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var startInfo = new System.Diagnostics.ProcessStartInfo(dlg.FileName)
                    {
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(startInfo);
                }
            }

        }

    }
}
