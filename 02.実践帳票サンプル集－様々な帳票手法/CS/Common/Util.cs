using System;
using System.Data.OleDb;
using System.Windows;

namespace Sample
{
    static class Util
    {
        internal static string SharePath = "";

        internal static void SetSharePath()
        {
            SharePath = System.IO.Path.GetFullPath(System.IO.Directory.GetCurrentDirectory() + "/../../../../../Resources/");
        }

        internal static string GetDefFile(int index)
        {
           return  SharePath + "zip" + (index + 1).ToString() + ".prepd";
        }

        // 指定されたExcelファイルををデータベースとして接続し、コネクションを返す。
        internal static OleDbConnection ConnectExcelDB (string ExcelFileName)
        {
            string connectString = null;
            if (IntPtr.Size == 4)
            {
                connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + SharePath + ExcelFileName + ";Extended Properties=Excel 8.0;";
            }
            else if (IntPtr.Size == 8)
            {
                connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + SharePath + ExcelFileName + ";Extended Properties=Excel 12.0;";
            }
            return new OleDbConnection(connectString);
        }
    }
}
