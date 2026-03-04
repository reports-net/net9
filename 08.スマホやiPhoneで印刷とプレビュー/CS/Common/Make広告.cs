using Pao.Reports;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace SvgPreviewMobile
{
    static class Make広告
    {
        public static void SetupData(IReport paoRep)
        {
            using SqliteConnection connection = Util.ConnectDatabase();

            DataTable table = Util.ExecuteQuery("select * from 広告情報", connection);

            // 画像ファイルパス
            string gpath = Util.SharePath;

            paoRep.LoadDefFile(Util.SharePath + "ad.prepd");
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
                paoRep.Write("画像1", gpath + (string)row["画像1"]);
                paoRep.Write("画像2", gpath + (string)row["画像2"]);
                paoRep.Write("QR", (string)row["製品名"] + " " + (string)row["キャッチフレーズ"]);

                paoRep.PageEnd();
            }
        }
    }
}
