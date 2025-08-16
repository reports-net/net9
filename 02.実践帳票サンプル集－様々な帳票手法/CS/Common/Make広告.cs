using Pao.Reports;
using System;
using System.Data;
using System.Data.OleDb;

namespace Sample
{
    static class Make広告
    {
        public static void SetupData(IReport paoRep)
        {
            //Excel データベースへの接続
            OleDbConnection connection = Util.ConnectExcelDB("広告.xls");
            string sql = "select * from [広告情報$]";

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            dataAdapter.Fill(ds, "広告情報");

            DataTable table = ds.Tables["広告情報"];

            paoRep.LoadDefFile(Util.SharePath + "広告.prepd");
            foreach (DataRow row in table.Rows)
            {

                paoRep.PageStart();

                paoRep.Write("製品名", (string)row["製品名"]);
                paoRep.Write("キャッチフレーズ", (string)row["キャッチフレーズ"]);
                paoRep.Write("商品コード", (string)row["商品コード"]);
                paoRep.Write("JANコード", (string)row["商品コード"]);
                paoRep.Write("売り文句", (string)row["売り文句"]);
                paoRep.Write("説明", (string)row["説明"]);
                paoRep.Write("価格", (string)row["価格"]);
                paoRep.Write("画像1", Util.SharePath + (string)row["画像1"]);
                paoRep.Write("画像2", Util.SharePath + (string)row["画像2"]);
                paoRep.Write("QR", (string)row["製品名"] + " " + (string)row["キャッチフレーズ"]);

                paoRep.PageEnd();

            }

            return;
        }
    }
}
