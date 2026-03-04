using Pao.Reports;
using System;

namespace SvgPreview
{
    static class Make10の倍数
    {
        public static void SetupData(IReport paoRep)
        {
            paoRep.LoadDefFile(Util.SharePath + "レポート定義ファイル.prepd");

            int page = 0;
            int line = 0;

            for (int i = 0; i < 60; i++)
            {
                if (i % 15 == 0)
                {
                    paoRep.PageStart();
                    page++;
                    line = 0;

                    paoRep.Write("日付", DateTime.Now.ToString());
                    paoRep.Write("頁数", "Page - " + page);

                    paoRep.z_Objects.SetObject("フォントサイズ");
                    paoRep.z_Objects.z_Text.z_FontAttr.Size = 12;
                    paoRep.Write("フォントサイズ", "フォントサイズ" + Environment.NewLine + " 変更後");

                    if (page == 2)
                        paoRep.Write("Line3", "");
                }
                line++;

                paoRep.Write("行番号", (i + 1).ToString(), line);
                paoRep.Write("10倍数", ((i + 1) * 10).ToString(), line);
                paoRep.Write("横線", line);

                if (((i + 1) % 15) == 0) paoRep.PageEnd();
            }
        }
    }
}
