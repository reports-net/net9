using Pao.Reports;

namespace SvgPreviewMaui
{
    static class Make単純なサンプル
    {
        public static void SetupData(IReport paoRep)
        {
            paoRep.LoadDefFile(Util.SharePath + "simple.prepd");

            paoRep.PageStart();
            paoRep.Write("Text2", "MAUIデスクトップで作った\n印刷データですよ～ん♪");
            paoRep.PageEnd();
        }
    }
}
