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
            // .NET 5�p�̐ݒ�
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#elif NET6_0 || NET7_0
            // .NET 6-7�p�̐ݒ�iDPI�����̉��P�j
            Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
#elif NET8_0 || NET9_0
            // .NET 8-9�p�̐ݒ�
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#else
            // �t�H�[���o�b�N�i���Ή��o�[�W�����p�j
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
