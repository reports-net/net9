using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace WinFormsApp1
{
 
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if NET5_0
            // .NET 5用の設定
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#elif NET6_0 || NET7_0
            // .NET 6-7用の設定（DPI処理の改善）
            Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
#elif NET8_0 || NET9_0
            // .NET 8-9用の設定
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#else
            // フォールバック（未対応バージョン用）
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
