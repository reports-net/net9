using Pao.Reports;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace SvgPreviewMobile
{
    static class Make見積書
    {
        public static void SetupData(IReport paoRep)
        {
            using SqliteConnection connection = Util.ConnectDatabase();

            DataSet ds = new DataSet();

            ds.Tables.Add(Util.ExecuteQuery("SELECT * FROM 見積ヘッダ ORDER BY 見積番号", connection));
            ds.Tables[0].TableName = "見積ヘッダ";

            ds.Tables.Add(Util.ExecuteQuery("SELECT * FROM 見積明細 ORDER BY 見積番号,行番号", connection));
            ds.Tables[1].TableName = "見積明細";

            var asm = typeof(Util).Assembly;

            // EmbeddedResource を事前にメモリに全読み込み
            byte[] coverBytes;
            using (var s = asm.GetManifestResourceStream("cover.prepd")!)
            using (var ms = new MemoryStream())
            { s.CopyTo(ms); coverBytes = ms.ToArray(); }

            byte[] estimateBytes;
            using (var s = asm.GetManifestResourceStream("estimate.prepd")!)
            using (var ms = new MemoryStream())
            { s.CopyTo(ms); estimateBytes = ms.ToArray(); }

            DataTable ht = ds.Tables["見積ヘッダ"];
            foreach (DataRow hdr in ht.Rows)
            {
                // 表紙
                paoRep.LoadDefFile(new MemoryStream(coverBytes), "cover.prepd");
                paoRep.PageStart();
                paoRep.Write("お客様名", (string)hdr["お客様名"]);
                paoRep.Write("担当者名", (string)hdr["担当者名"]);
                paoRep.PageEnd();

                // 見積書
                paoRep.LoadDefFile(new MemoryStream(estimateBytes), "estimate.prepd");
                paoRep.PageStart();

                paoRep.Write("見積番号", (string)hdr["見積番号"]);
                paoRep.Write("お客様名", (string)hdr["お客様名"]);
                paoRep.Write("担当者名", (string)hdr["担当者名"]);
                paoRep.Write("見積日", DateTime.Parse(hdr["見積日"].ToString()).ToString("yyyy年M月d日"));
                paoRep.Write("ヘッダ合計", "\\ " + string.Format("{0:N0}", hdr["合計金額"]));
                paoRep.Write("消費税額", string.Format("{0:N0}", hdr["消費税額"]));
                paoRep.Write("フッタ合計", string.Format("{0:N0}", hdr["合計金額"]));

                // 背景
                for (int i = 0; i < 7; i++)
                {
                    paoRep.Write("品番白", i + 1);
                    paoRep.Write("品番白", i + 1);
                    paoRep.Write("数量白", i + 1);
                    paoRep.Write("単価白", i + 1);
                    paoRep.Write("金額白", i + 1);
                    paoRep.Write("品番青", i + 1);
                    paoRep.Write("品名青", i + 1);
                    paoRep.Write("数量青", i + 1);
                    paoRep.Write("単価青", i + 1);
                    paoRep.Write("金額青", i + 1);
                }

                // 明細
                DataView dv = new DataView(ds.Tables["見積明細"]);
                dv.RowFilter = "見積番号 = '" + (string)hdr["見積番号"] + "'";
                for (int i = 0; i < dv.Count; i++)
                {
                    paoRep.Write("品番", (string)dv[i]["品番"], i + 1);
                    paoRep.Write("品名", (string)dv[i]["品名"], i + 1);
                    paoRep.Write("数量", dv[i]["数量"].ToString(), i + 1);
                    paoRep.Write("単価", string.Format("{0:N0}", dv[i]["単価"]), i + 1);
                    paoRep.Write("金額", string.Format("{0:N0}", dv[i]["金額"]), i + 1);
                }

                paoRep.PageEnd();
            }
        }
    }
}
