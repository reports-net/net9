using System;
using System.Data.OleDb;

namespace SvgPreview
{
    static class Util
    {
        internal static string SharePath = "";

        internal static void SetSharePath(string basePath)
        {
            SharePath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(basePath, "Resources")) + System.IO.Path.DirectorySeparatorChar;
        }

        internal static string GetDefFile(int index)
        {
            return SharePath + "zip" + (index + 1).ToString() + ".prepd";
        }

        // 指定されたExcelファイルをデータベースとして接続し、コネクションを返す。
        internal static OleDbConnection ConnectExcelDB(string ExcelFileName)
        {
            string connectString;
            if (IntPtr.Size == 4)
            {
                connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + SharePath + ExcelFileName + ";Extended Properties=Excel 8.0;";
            }
            else
            {
                connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + SharePath + ExcelFileName + ";Extended Properties=Excel 12.0;";
            }
            return new OleDbConnection(connectString);
        }
    }
}
