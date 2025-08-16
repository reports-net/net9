Imports Pao.Reports
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows.Forms

Namespace Sample
    Friend Module Makeデザイン変更
        Public Sub SetupData(paoRep As IReport, defIndex As Integer)
            'Excel データベースへの接続
            Dim connection As OleDbConnection = Util.ConnectExcelDB("zip.xls")
            'データセットへテーブルをセットする。ヘッダと明細の2テーブル
            Dim SQL As String = "select * from [郵便番号テーブル$]"
            Dim dataAdapter As New OleDbDataAdapter(SQL, connection)
            Dim ds As New DataSet()

            Try
                dataAdapter.Fill(ds, "PostTable")
            Catch
                MessageBox.Show("このサンプルプログラムを動作させるためには、Microsoft Office をインストールする必要があります。")
                Return
            End Try

            Dim table As New DataTable()
            table = ds.Tables("PostTable")
            Dim page As Integer = 0
            Dim line As Integer = 999

            paoRep.LoadDefFile(Util.GetDefFile(defIndex))
            Dim hDate As String = System.DateTime.Now.ToString()

            For Each row As DataRow In table.Rows
                line = line + 1
                If line > 32 Then
                    ' Head Print
                    If page <> 0 Then paoRep.PageEnd()
                    page = page + 1
                    paoRep.PageStart()
                    paoRep.Write("日時", hDate)
                    paoRep.Write("ページ", "Page-" & page.ToString())
                    line = 1
                End If

                'Body Print
                paoRep.Write("郵便番号", row("郵便番号").ToString(), line)
                paoRep.Write("市区町村", row("市区町村").ToString(), line)
                paoRep.Write("住所", row("住所").ToString(), line)
                paoRep.Write("横罫線", line)
            Next

            paoRep.PageEnd()
            Return
        End Sub
    End Module
End Namespace