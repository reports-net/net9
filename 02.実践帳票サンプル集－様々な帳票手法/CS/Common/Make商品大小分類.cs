using Pao.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Sample
{
    static class Make商品大小分類
    {
        private class PrintData
        {
            internal string s大分類コード;
            internal string s小分類コード;
            internal string s大分類名称;
            internal string s小分類名称;
            internal string s品番;
            internal string s品名;
        }

        public static void SetupData(IReport paoRep)
        {
            //Excel データベースへの接続
            OleDbConnection connection = Util.ConnectExcelDB("商品マスタ.xls");
            

            //データセットの作成
            DataSet ds = new DataSet();

            //データセットへテーブルをセットする。ヘッダと明細の2テーブル
            string sql = "";

            sql += " SELECT C.*, A.大分類名称, B.小分類名称 ";
            sql += " FROM ";
            sql += "   [M_大分類$] AS A";
            sql += " , [M_小分類$] AS B";
            sql += " , [M_商品$] AS C";
            sql += " WHERE";
            sql += " A.大分類コード = B.大分類コード";
            sql += " AND";
            sql += " A.大分類コード = C.大分類コード";
            sql += " AND";
            sql += " B.大分類コード = C.大分類コード";
            sql += " AND";
            sql += " B.小分類コード = C.小分類コード";
            sql += " ORDER BY C.大分類コード, C.小分類コード";

            OleDbDataAdapter oda = new OleDbDataAdapter(sql, connection);

            oda.Fill(ds, "商品一覧");

            // いったん構造体の配列にセット
            DataTable dt = ds.Tables["商品一覧"];

            string sv大分類名称 = null;
            string sv小分類名称 = null;
            int cnt大分類 = 0;
            int cnt小分類 = 0;
            List<PrintData> pds = new List<PrintData>();
            PrintData pd;
            foreach (DataRow dr in dt.Rows)
            {
                pd = new PrintData();

                // キーブレイク処理は、今回は構造体にセットするところでやってみました。
                // プログラム構造的にもっと汎用的な方法はあります。
                if (sv小分類名称 != null && sv小分類名称 != dr["小分類名称"].ToString())
                {
                    pd.s小分類コード = " ";
                    pd.s小分類名称 = "小分類(" + sv小分類名称 + ")小計";
                    pd.s品番 = cnt小分類.ToString() + " 冊";
                    cnt小分類 = 0;
                    pds.Add(pd);
                    pd = new PrintData();
                }
                if (sv大分類名称 != null && sv大分類名称 != dr["大分類名称"].ToString())
                {
                    pd.s大分類コード = " ";
                    pd.s小分類名称 = "大分類(" + sv大分類名称 + ")小計";
                    pd.s品番 = cnt大分類.ToString() + " 冊";
                    cnt大分類 = 0;
                    pds.Add(pd);
                    pd = new PrintData();
                }

                if (sv大分類名称 != dr["大分類名称"].ToString())
                {
                    pd.s大分類名称 = dr["大分類名称"].ToString();
                }
                if (sv小分類名称 != dr["小分類名称"].ToString())
                {
                    pd.s小分類名称 = dr["小分類名称"].ToString();
                }
                pd.s品番 = dr["品番"].ToString();
                pd.s品名 = dr["品名"].ToString();

                pds.Add(pd);

                sv大分類名称 = dr["大分類名称"].ToString();
                sv小分類名称 = dr["小分類名称"].ToString();

                cnt大分類++;
                cnt小分類++;
            }


            pd = new PrintData();
            pd.s小分類コード = " ";
            pd.s小分類名称 = "小分類(" + sv小分類名称 + ")小計";
            pd.s品番 = cnt小分類.ToString() + " 冊";
            pds.Add(pd);
            pd = new PrintData();
            pd.s大分類コード = " ";
            pd.s小分類名称 = "大分類(" + sv大分類名称 + ")小計";
            pd.s品番 = cnt大分類.ToString() + " 冊";
            pds.Add(pd);

            //商品一覧の生成
            paoRep.LoadDefFile(Util.SharePath + "商品一覧.prepd");
            paoRep.PageStart();

            const int RecnumInPage = 20;

            paoRep.z_Objects.SetObject("枠_大分類");
            System.Drawing.Color svBackColor = paoRep.z_Objects.z_Square.PaintColor;

            string[] filedNames_枠 = { "枠_大分類", "枠_小分類", "枠_品番", "枠_品名" };
            string[] filedNames = { "大分類", "小分類", "品番", "品名" };

            for (int recno = 0; recno < pds.Count; recno++)
            {

                if (recno % RecnumInPage == 0)
                {
                    if (recno != 0)
                    {
                        paoRep.PageEnd();
                        paoRep.PageStart();
                    }
                }

                // 値セット
                int lineno = (recno % RecnumInPage) + 1;
                paoRep.Write("大分類", pds[recno].s大分類名称, lineno);
                paoRep.Write("小分類", pds[recno].s小分類名称, lineno);
                paoRep.Write("品番", pds[recno].s品番, lineno);
                paoRep.Write("品名", pds[recno].s品名, lineno);

                // 枠描画
                for (int j = 0; j < filedNames_枠.Length; j++)
                {
                    paoRep.Write(filedNames_枠[j], lineno);
                }

                // 小分類小計行の色替え
                if (pds[recno].s小分類コード == " ")
                {
                    // 枠描画
                    for (int j = 0; j < filedNames_枠.Length; j++)
                    {
                        paoRep.z_Objects.SetObject(filedNames_枠[j], lineno);
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.LightYellow;
                    }

                }
                // 大分類小計行の色替え
                else if (pds[recno].s大分類コード == " ")
                {
                    // 枠描画
                    for (int j = 0; j < filedNames_枠.Length; j++)
                    {
                        paoRep.z_Objects.SetObject(filedNames_枠[j], lineno);
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.LightPink;
                    }

                }

            }

            paoRep.PageEnd();

            return;
        }
    }
}
