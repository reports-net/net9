Imports Pao.Reports

Friend Module Make単純なサンプル

    Public Sub SetupData(paoRep As IReport)
        paoRep.LoadDefFile(Util.SharePath & "simple.prepd")

        paoRep.PageStart()
        paoRep.Write("Text2", "WPFデスクトップで作った" & vbLf & "印刷データですよ～ん♪")
        paoRep.PageEnd()
    End Sub

End Module
