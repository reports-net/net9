Namespace Sample
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class Form1
        Inherits Form

        'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Windows フォーム デザイナーで必要です。
        Private components As System.ComponentModel.IContainer

        'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
        'Windows フォーム デザイナーを使用して変更できます。  
        'コード エディターを使って変更しないでください。
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
            btnExecute = New Button()
            panel1 = New Panel()
            radXPS = New RadioButton()
            radSVG = New RadioButton()
            radPDF = New RadioButton()
            radPrint = New RadioButton()
            radPreview = New RadioButton()
            richTextBox1 = New RichTextBox()
            grpDesign = New GroupBox()
            radDesign2 = New RadioButton()
            radDesign1 = New RadioButton()
            richTextBox2 = New RichTextBox()
            cmbReportType = New ComboBox()
            label2 = New Label()
            pnl = New Panel()
            panel1.SuspendLayout()
            grpDesign.SuspendLayout()
            pnl.SuspendLayout()
            SuspendLayout()
            ' 
            ' btnExecute
            ' 
            btnExecute.BackColor = Color.FromArgb(CByte(75), CByte(95), CByte(131))
            btnExecute.Font = New Font("Yu Gothic UI", 20F, FontStyle.Bold, GraphicsUnit.Point, CByte(128))
            btnExecute.ForeColor = Color.White
            btnExecute.Location = New Point(751, 4)
            btnExecute.Margin = New Padding(5, 6, 5, 6)
            btnExecute.Name = "btnExecute"
            btnExecute.Size = New Size(188, 56)
            btnExecute.TabIndex = 2
            btnExecute.Text = "実行"
            btnExecute.UseVisualStyleBackColor = False
            ' 
            ' panel1
            ' 
            panel1.BorderStyle = BorderStyle.Fixed3D
            panel1.Controls.Add(radXPS)
            panel1.Controls.Add(radSVG)
            panel1.Controls.Add(radPDF)
            panel1.Controls.Add(radPrint)
            panel1.Controls.Add(radPreview)
            panel1.Controls.Add(btnExecute)
            panel1.Location = New Point(14, 4)
            panel1.Margin = New Padding(2)
            panel1.Name = "panel1"
            panel1.Size = New Size(968, 64)
            panel1.TabIndex = 11
            ' 
            ' radXPS
            ' 
            radXPS.AutoSize = True
            radXPS.Font = New Font("Yu Gothic UI", 18F, FontStyle.Bold)
            radXPS.Location = New Point(596, 17)
            radXPS.Margin = New Padding(3, 4, 3, 4)
            radXPS.Name = "radXPS"
            radXPS.Size = New Size(122, 36)
            radXPS.TabIndex = 13
            radXPS.Text = "XPS出力"
            radXPS.UseVisualStyleBackColor = True
            ' 
            ' radSVG
            ' 
            radSVG.AutoSize = True
            radSVG.Font = New Font("Yu Gothic UI", 18F, FontStyle.Bold)
            radSVG.Location = New Point(448, 19)
            radSVG.Margin = New Padding(3, 4, 3, 4)
            radSVG.Name = "radSVG"
            radSVG.Size = New Size(125, 36)
            radSVG.TabIndex = 12
            radSVG.Text = "SVG出力"
            radSVG.UseVisualStyleBackColor = True
            ' 
            ' radPDF
            ' 
            radPDF.AutoSize = True
            radPDF.Font = New Font("Yu Gothic UI", 18F, FontStyle.Bold)
            radPDF.Location = New Point(301, 17)
            radPDF.Margin = New Padding(3, 4, 3, 4)
            radPDF.Name = "radPDF"
            radPDF.Size = New Size(123, 36)
            radPDF.TabIndex = 11
            radPDF.Text = "PDF出力"
            radPDF.UseVisualStyleBackColor = True
            ' 
            ' radPrint
            ' 
            radPrint.AutoSize = True
            radPrint.Font = New Font("Yu Gothic UI", 18F, FontStyle.Bold)
            radPrint.Location = New Point(192, 19)
            radPrint.Margin = New Padding(3, 4, 3, 4)
            radPrint.Name = "radPrint"
            radPrint.Size = New Size(80, 36)
            radPrint.TabIndex = 10
            radPrint.Text = "印刷"
            radPrint.UseVisualStyleBackColor = True
            ' 
            ' radPreview
            ' 
            radPreview.AutoSize = True
            radPreview.Checked = True
            radPreview.Font = New Font("Yu Gothic UI", 18F, FontStyle.Bold)
            radPreview.Location = New Point(47, 19)
            radPreview.Margin = New Padding(3, 4, 3, 4)
            radPreview.Name = "radPreview"
            radPreview.Size = New Size(116, 36)
            radPreview.TabIndex = 9
            radPreview.TabStop = True
            radPreview.Text = "プレビュー"
            radPreview.UseVisualStyleBackColor = True
            ' 
            ' richTextBox1
            ' 
            richTextBox1.BackColor = Color.FromArgb(CByte(255), CByte(253), CByte(231))
            richTextBox1.Font = New Font("BIZ UDゴシック", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(128))
            richTextBox1.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
            richTextBox1.Location = New Point(14, 80)
            richTextBox1.Margin = New Padding(2)
            richTextBox1.Name = "richTextBox1"
            richTextBox1.ReadOnly = True
            richTextBox1.Size = New Size(990, 138)
            richTextBox1.TabIndex = 13
            richTextBox1.Text = vbLf & "  このプログラムは Windows Form のプログラムですが、" & vbLf & "  WPFのプロジェクトをスタートアップに設定すれば 同じ機能のWPF版を起動することができます。" & vbLf & vbLf & "  このサンプルプログラムでは、手軽にお試しいただける目的で、Excelファイルをデータベースとして使用しております。" & vbLf & "  そのため、Office 64bit版がインストールされている必要があります。"
            ' 
            ' grpDesign
            ' 
            grpDesign.Controls.Add(radDesign2)
            grpDesign.Controls.Add(radDesign1)
            grpDesign.Font = New Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(128))
            grpDesign.ForeColor = Color.Blue
            grpDesign.Location = New Point(41, 80)
            grpDesign.Margin = New Padding(2)
            grpDesign.Name = "grpDesign"
            grpDesign.Padding = New Padding(2)
            grpDesign.Size = New Size(384, 58)
            grpDesign.TabIndex = 26
            grpDesign.TabStop = False
            grpDesign.Text = "デザインを選択してください"
            ' 
            ' radDesign2
            ' 
            radDesign2.AutoSize = True
            radDesign2.Font = New Font("Yu Gothic UI", 14F, FontStyle.Bold)
            radDesign2.Location = New Point(217, 27)
            radDesign2.Margin = New Padding(3, 4, 3, 4)
            radDesign2.Name = "radDesign2"
            radDesign2.Size = New Size(99, 29)
            radDesign2.TabIndex = 17
            radDesign2.Tag = "1"
            radDesign2.Text = "デザイン2"
            radDesign2.UseVisualStyleBackColor = True
            ' 
            ' radDesign1
            ' 
            radDesign1.AutoSize = True
            radDesign1.Checked = True
            radDesign1.Font = New Font("Yu Gothic UI", 14F, FontStyle.Bold)
            radDesign1.Location = New Point(17, 27)
            radDesign1.Margin = New Padding(3, 4, 3, 4)
            radDesign1.Name = "radDesign1"
            radDesign1.Size = New Size(96, 29)
            radDesign1.TabIndex = 16
            radDesign1.TabStop = True
            radDesign1.Tag = "0"
            radDesign1.Text = "デザイン1"
            radDesign1.UseVisualStyleBackColor = True
            ' 
            ' richTextBox2
            ' 
            richTextBox2.BackColor = Color.FromArgb(CByte(225), CByte(245), CByte(254))
            richTextBox2.Font = New Font("BIZ UDゴシック", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(128))
            richTextBox2.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
            richTextBox2.Location = New Point(14, 226)
            richTextBox2.Margin = New Padding(2)
            richTextBox2.Name = "richTextBox2"
            richTextBox2.ReadOnly = True
            richTextBox2.Size = New Size(990, 270)
            richTextBox2.TabIndex = 18
            richTextBox2.Text = resources.GetString("richTextBox2.Text")
            ' 
            ' cmbReportType
            ' 
            cmbReportType.DropDownStyle = ComboBoxStyle.DropDownList
            cmbReportType.Font = New Font("Yu Gothic UI", 16F, FontStyle.Bold, GraphicsUnit.Point, CByte(128))
            cmbReportType.FormattingEnabled = True
            cmbReportType.Items.AddRange(New Object() {"見積書 (四角で表を作成する手法・表紙と明細別デザイン・ハンコの印影出力)", "郵便番号一覧 (罫線で表を作成する手法・大量ページ・一覧のデザインを途中で変更・QRコード出力)", "請求書 (ほぼコードのみで帳票作成)", "商品大小分類 (グループごとに中間小計)", "いつでもデザイン変更 (データ再設定なしでデザイン変更)", "広告 (イメージPDF出力・装飾文字使用・画像出力)"})
            cmbReportType.Location = New Point(36, 36)
            cmbReportType.Margin = New Padding(2)
            cmbReportType.Name = "cmbReportType"
            cmbReportType.Size = New Size(1016, 38)
            cmbReportType.TabIndex = 27
            ' 
            ' label2
            ' 
            label2.AutoSize = True
            label2.Font = New Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(128))
            label2.ForeColor = Color.FromArgb(CByte(102), CByte(102), CByte(102))
            label2.Location = New Point(30, 14)
            label2.Margin = New Padding(2, 0, 2, 0)
            label2.Name = "label2"
            label2.Size = New Size(217, 21)
            label2.TabIndex = 28
            label2.Text = "サンプル帳票を選択してください。"
            ' 
            ' pnl
            ' 
            pnl.Controls.Add(panel1)
            pnl.Controls.Add(richTextBox1)
            pnl.Controls.Add(richTextBox2)
            pnl.Location = New Point(30, 148)
            pnl.Margin = New Padding(2)
            pnl.Name = "pnl"
            pnl.Size = New Size(1016, 508)
            pnl.TabIndex = 29
            ' 
            ' Form1
            ' 
            AutoScaleDimensions = New SizeF(96F, 96F)
            AutoScaleMode = AutoScaleMode.Dpi
            AutoScroll = True
            ClientSize = New Size(1082, 668)
            Controls.Add(pnl)
            Controls.Add(label2)
            Controls.Add(cmbReportType)
            Controls.Add(grpDesign)
            Margin = New Padding(2)
            MinimumSize = New Size(1024, 550)
            Name = "Form1"
            Text = "WEB API (REST API) から印刷データを取得して帳票出力するサンプル"
            panel1.ResumeLayout(False)
            panel1.PerformLayout()
            grpDesign.ResumeLayout(False)
            grpDesign.PerformLayout()
            pnl.ResumeLayout(False)
            ResumeLayout(False)
            PerformLayout()
        End Sub

        ' ─── デザイナ保持フィールド ───
        Private WithEvents btnExecute As Button
        Private WithEvents panel1 As Panel
        Private WithEvents radXPS As RadioButton
        Private WithEvents radSVG As RadioButton
        Private WithEvents radPDF As RadioButton
        Private WithEvents radPrint As RadioButton
        Private WithEvents radPreview As RadioButton
        Private WithEvents richTextBox1 As RichTextBox
        Private WithEvents richTextBox2 As RichTextBox
        Private WithEvents label2 As Label
        Private WithEvents cmbReportType As ComboBox
        Private WithEvents grpDesign As GroupBox
        Private WithEvents radDesign2 As RadioButton
        Private WithEvents radDesign1 As RadioButton
        Private WithEvents pnl As Panel
    End Class
End Namespace