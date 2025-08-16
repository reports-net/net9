using Pao.Reports;
using System;
using System.Data;
using System.Data.OleDb;

namespace Sample
{
    static class Make見積書
    {
        public static void SetupData(IReport paoRep)
        {
            //Excel データベースへの接続
            OleDbConnection connection = Util.ConnectExcelDB("見積書.xls");

            //データセットの作成
            DataSet ds = new DataSet();

            //データセットへテーブルをセットする。ヘッダと明細の2テーブル
            string sql = "SELECT * FROM [見積ヘッダ$] ORDER BY 見積番号";
            OleDbDataAdapter oda = new OleDbDataAdapter(sql, connection);

            oda.Fill(ds, "見積ヘッダ");

            sql = "select * from [見積明細$] ORDER BY 見積番号,行番号";
            oda = new OleDbDataAdapter(sql, connection);
            oda.Fill(ds, "見積明細");

            DataTable ht = ds.Tables["見積ヘッダ"];
            foreach (DataRow hdr in ht.Rows)
            {
                //表紙の生成
                paoRep.LoadDefFile(Util.SharePath + "表紙.prepd");
                paoRep.PageStart();
                paoRep.Write("お客様名", (string)hdr["お客様名"]);
                paoRep.Write("担当者名", (string)hdr["担当者名"]);
                paoRep.PageEnd();

                //見積書の生成
                paoRep.LoadDefFile(Util.SharePath + "見積書.prepd");
                paoRep.PageStart();

                paoRep.Write("見積番号", (string)hdr["見積番号"]);
                paoRep.Write("お客様名", (string)hdr["お客様名"]);
                paoRep.Write("担当者名", (string)hdr["担当者名"]);
                paoRep.Write("見積日", ((DateTime)hdr["見積日"]).ToString("yyyy年M月d日"));
                paoRep.Write("ヘッダ合計", "\\ " + string.Format("{0:N0}", hdr["合計金額"]));
                paoRep.Write("消費税額", string.Format("{0:N0}", hdr["消費税額"]));
                paoRep.Write("フッタ合計", string.Format("{0:N0}", hdr["合計金額"]));


                //明細の背景作成
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

                //明細の作成
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

            return;
        }
    }
}
