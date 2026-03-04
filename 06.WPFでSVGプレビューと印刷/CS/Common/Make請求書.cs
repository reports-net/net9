using Pao.Reports;
using System;
using System.Data;
using System.Data.OleDb;

namespace SvgPreview
{
    static class Make請求書
    {
        public static void SetupData(IReport paoRep)
        {
            OleDbConnection connection = Util.ConnectExcelDB("請求書.xls");

            DataSet ds = new DataSet();

            string sql = "select * from [請求ヘッダ$] ORDER BY 請求番号";
            OleDbDataAdapter oda = new OleDbDataAdapter(sql, connection);
            oda.Fill(ds, "請求ヘッダ");

            sql = "select * from [請求明細$] ORDER BY 請求番号, 行番号";
            oda = new OleDbDataAdapter(sql, connection);
            oda.Fill(ds, "請求明細");

            paoRep.LoadDefFile(Util.SharePath + "請求書.prepd");

            float[] arr_w = { -5, 44, -20, -10, -9 };

            DataTable ht = ds.Tables["請求ヘッダ"];
            foreach (DataRow hdr in ht.Rows)
            {
                paoRep.PageStart();

                paoRep.Write("txtNo", (string)hdr["請求番号"]);
                paoRep.Write("txtCustomer", (string)hdr["お客様名"]);
                paoRep.Write("txtDate", DateTime.Now.ToString("yyyy年M月d日"));
                paoRep.Write("Image1", Util.SharePath + "角印.png");

                paoRep.z_Objects.SetObject("hLine");
                int maxHLine = paoRep.z_Objects.z_Line.Repeat - 1;
                paoRep.z_Objects.SetObject("vLine");
                int maxVLine = paoRep.z_Objects.z_Line.Repeat - 1;

                for (int i = 0; i < maxHLine; i++)
                {
                    paoRep.Write("hLine", i + 1);

                    if (i == 0)
                    {
                        paoRep.z_Objects.SetObject("hLine", i + 1);
                        paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5f;
                    }

                    if (i == 1)
                    {
                        paoRep.z_Objects.SetObject("hLine", i + 1);
                        paoRep.z_Objects.z_Line.z_LineAttr.Type = PmLineType.Double;
                    }

                    paoRep.Write("LineRect", i + 1);
                    paoRep.z_Objects.SetObject("LineRect", i + 1);

                    if (i == 0)
                    {
                    }
                    else if (i < maxHLine - 3)
                    {
                        if (i % 2 == 1)
                        {
                            paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.LightSkyBlue;
                        }
                    }
                    else
                    {
                        paoRep.z_Objects.z_Square.PaintColor = System.Drawing.Color.FromArgb(255, 255, 180);
                    }

                    float svX = -1;

                    for (int j = 0; j < maxVLine; j++)
                    {
                        paoRep.z_Objects.SetObject("field" + (j + 1).ToString(), i + 1);

                        paoRep.z_Objects.z_Text.Width = paoRep.z_Objects.z_Text.Width + arr_w[j];

                        if (j > 0)
                        {
                            paoRep.z_Objects.z_Text.X = svX;
                        }
                        svX = paoRep.z_Objects.z_Text.X + paoRep.z_Objects.z_Text.Width;

                        if (i == 0)
                        {
                            paoRep.z_Objects.z_Text.z_FontAttr.Bold = true;
                        }
                        else
                        {
                            paoRep.z_Objects.z_Text.z_FontAttr.Bold = false;
                            paoRep.z_Objects.z_Text.z_FontAttr.Size = 12;

                            switch (j + 1)
                            {
                                case 1:
                                    paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Center;
                                    break;
                                case 2:
                                    paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Left;
                                    break;
                                case 3:
                                case 4:
                                case 5:
                                    paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Right;
                                    break;
                            }
                        }
                    }

                    for (int j = maxHLine; j > maxHLine - 3; j--)
                    {
                        paoRep.z_Objects.SetObject("field4", j);
                        paoRep.z_Objects.z_Text.z_FontAttr.Size = 16;
                        paoRep.z_Objects.z_Text.TextAlign = PmAlignType.Center;
                        paoRep.z_Objects.z_Text.z_FontAttr.Bold = true;
                    }
                }

                paoRep.z_Objects.SetObject("vLine");
                float baseX = paoRep.z_Objects.z_Line.X;
                for (int j = 0; j <= maxVLine; j++)
                {
                    paoRep.Write("vLine", j + 1);

                    paoRep.z_Objects.SetObject("vLine", j + 1);

                    for (int jj = 1; jj <= j && j < maxVLine; jj++)
                    {
                        float baseIntervalX = paoRep.z_Objects.z_Line.IntervalX;
                        paoRep.z_Objects.z_Line.IntervalX = baseIntervalX + arr_w[j - jj];
                    }

                    if (j == 0 || j == maxVLine)
                    {
                        paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5f;
                    }
                }

                paoRep.Write("field1", "品番", 1);
                paoRep.Write("field2", "品名", 1);
                paoRep.Write("field3", "数量", 1);
                paoRep.Write("field4", "単価", 1);
                paoRep.Write("field5", "金額", 1);

                DataView dv = new DataView(ds.Tables["請求明細"]);
                dv.RowFilter = "請求番号 = '" + (string)hdr["請求番号"] + "'";
                long totalAmount = 0;
                int ii = 0;
                for (; ii < dv.Count; ii++)
                {
                    paoRep.Write("field1", (string)dv[ii]["品番"], ii + 2);
                    paoRep.Write("field2", (string)dv[ii]["品名"], ii + 2);
                    paoRep.Write("field3", dv[ii]["数量"].ToString(), ii + 2);
                    paoRep.Write("field4", string.Format("{0:N0}", dv[ii]["単価"]), ii + 2);
                    long amount = Convert.ToInt64(dv[ii]["数量"]) * Convert.ToInt64(dv[ii]["単価"]);
                    paoRep.Write("field5", string.Format("{0:N0}", amount), ii + 2);
                    totalAmount += amount;
                }

                double tax = 0.05;

                paoRep.Write("field4", "小計", maxHLine - 2);
                paoRep.Write("field5", string.Format("{0:N0}", totalAmount), maxHLine - 2);
                ii++;
                paoRep.Write("field4", "消費税", maxHLine - 1);
                paoRep.Write("field5", string.Format("{0:N0}", totalAmount * tax), maxHLine - 1);
                ii++;
                paoRep.Write("field4", "合計", maxHLine);
                paoRep.Write("field5", string.Format("{0:N0}", totalAmount + (totalAmount * tax)), maxHLine);

                paoRep.Write("txtTotal", string.Format("{0:N0}", totalAmount + (totalAmount * tax)));

                paoRep.z_Objects.SetObject("hLine", maxHLine - 2);
                paoRep.z_Objects.z_Line.z_LineAttr.Type = PmLineType.Double;

                paoRep.Write("hLine", maxHLine + 1);
                paoRep.z_Objects.SetObject("hLine", maxHLine + 1);
                paoRep.z_Objects.z_Line.z_LineAttr.Width = 0.5f;

                paoRep.PageEnd();
            }
        }
    }
}
