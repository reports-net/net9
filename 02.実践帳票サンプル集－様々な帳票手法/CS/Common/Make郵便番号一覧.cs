using Pao.Reports;
using System;
using System.Data;
using System.Data.OleDb;

namespace Sample
{
    static class Make郵便番号一覧
    {
        public static void SetupData(IReport paoRep)
        {
            //Excel データベースへの接続
            OleDbConnection connection = Util.ConnectExcelDB("zip.xls");

            //データセットへテーブルをセットする。ヘッダと明細の2テーブル
            string SQL = "select * from [郵便番号テーブル$]";

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(SQL, connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "PostTable");
            DataTable table = ds.Tables["PostTable"];

            int page = 0;
            int line = 999;
            string hDate = System.DateTime.Now.ToString();

            paoRep.LoadDefFile(Util.SharePath + "zip1.prepd");
            foreach (DataRow row in table.Rows)
            {
                line++;
                if (line > 32)
                { // Head Print
                    if (page != 0) paoRep.PageEnd();

                    page++;

                    if (page == 6)
                    {
                        paoRep.LoadDefFile(Util.SharePath + "zip2.prepd");
                    }

                    paoRep.PageStart();

                    paoRep.Write("日時", hDate);
                    paoRep.Write("ページ", "Page-" + page.ToString());

                    //QRコード描画
                    if (page < 6)
                    {
                        paoRep.Write("QR", row["郵便番号"].ToString() + " " + row["市区町村"].ToString() + row["住所"].ToString());
                    }

                    line = 1;

                }

                //Body Print
                paoRep.Write("郵便番号", row["郵便番号"].ToString(), line);
                paoRep.Write("市区町村", row["市区町村"].ToString(), line);
                paoRep.Write("住所", row["住所"].ToString(), line);
                paoRep.Write("横罫線", line);


                if (page > 5 && line % 2 == 0)
                    paoRep.Write("網掛け", line / 2);

            }
            paoRep.PageEnd();

            return;
        }
    }
}
