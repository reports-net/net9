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
            components = New ComponentModel.Container()
            prevWinForm = New PrintPreviewControl()
            panel1 = New Panel()
            radGetPrintDocument = New RadioButton()
            radXPS = New RadioButton()
            radSVG = New RadioButton()
            radPDF = New RadioButton()
            radPrint = New RadioButton()
            radPreview = New RadioButton()
            btnExecute = New Button()
            saveFileDialog = New SaveFileDialog()
            printDocument1 = New Printing.PrintDocument()
            toolTip1 = New ToolTip(components)
            panel1.SuspendLayout()
            SuspendLayout()
            ' 
            ' prevWinForm
            ' 
            prevWinForm.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
            prevWinForm.AutoZoom = False
            prevWinForm.Location = New Point(12, 171)
            prevWinForm.Margin = New Padding(3, 4, 3, 4)
            prevWinForm.Name = "prevWinForm"
            prevWinForm.Size = New Size(1229, 527)
            prevWinForm.TabIndex = 21
            prevWinForm.Zoom = 1.0R
            ' 
            ' panel1
            ' 
            panel1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
            panel1.BorderStyle = BorderStyle.Fixed3D
            panel1.Controls.Add(radGetPrintDocument)
            panel1.Controls.Add(radXPS)
            panel1.Controls.Add(radSVG)
            panel1.Controls.Add(radPDF)
            panel1.Controls.Add(radPrint)
            panel1.Controls.Add(radPreview)
            panel1.Controls.Add(btnExecute)
            panel1.Location = New Point(12, 8)
            panel1.Margin = New Padding(3, 4, 3, 4)
            panel1.Name = "panel1"
            panel1.Size = New Size(1229, 155)
            panel1.TabIndex = 20
            ' 
            ' radGetPrintDocument
            ' 
            radGetPrintDocument.AutoSize = True
            radGetPrintDocument.Checked = True
            radGetPrintDocument.Font = New Font("Yu Gothic UI", 18.0F, FontStyle.Bold)
            radGetPrintDocument.Location = New Point(71, 95)
            radGetPrintDocument.Margin = New Padding(4, 7, 4, 7)
            radGetPrintDocument.Name = "radGetPrintDocument"
            radGetPrintDocument.Size = New Size(243, 36)
            radGetPrintDocument.TabIndex = 14
            radGetPrintDocument.TabStop = True
            radGetPrintDocument.Text = "独自プレビュー ↓↓↓"
            radGetPrintDocument.UseVisualStyleBackColor = True
            ' 
            ' radXPS
            ' 
            radXPS.AutoSize = True
            radXPS.Font = New Font("Yu Gothic UI", 18.0F, FontStyle.Bold)
            radXPS.Location = New Point(733, 32)
            radXPS.Margin = New Padding(4, 7, 4, 7)
            radXPS.Name = "radXPS"
            radXPS.Size = New Size(122, 36)
            radXPS.TabIndex = 13
            radXPS.Text = "XPS出力"
            radXPS.UseVisualStyleBackColor = True
            ' 
            ' radSVG
            ' 
            radSVG.AutoSize = True
            radSVG.Font = New Font("Yu Gothic UI", 18.0F, FontStyle.Bold)
            radSVG.Location = New Point(555, 34)
            radSVG.Margin = New Padding(4, 7, 4, 7)
            radSVG.Name = "radSVG"
            radSVG.Size = New Size(125, 36)
            radSVG.TabIndex = 12
            radSVG.Text = "SVG出力"
            radSVG.UseVisualStyleBackColor = True
            ' 
            ' radPDF
            ' 
            radPDF.AutoSize = True
            radPDF.Font = New Font("Yu Gothic UI", 18.0F, FontStyle.Bold)
            radPDF.Location = New Point(369, 32)
            radPDF.Margin = New Padding(4, 7, 4, 7)
            radPDF.Name = "radPDF"
            radPDF.Size = New Size(123, 36)
            radPDF.TabIndex = 11
            radPDF.Text = "PDF出力"
            radPDF.UseVisualStyleBackColor = True
            ' 
            ' radPrint
            ' 
            radPrint.AutoSize = True
            radPrint.Font = New Font("Yu Gothic UI", 18.0F, FontStyle.Bold)
            radPrint.Location = New Point(234, 34)
            radPrint.Margin = New Padding(4, 7, 4, 7)
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
            radPreview.Font = New Font("Yu Gothic UI", 18.0F, FontStyle.Bold)
            radPreview.Location = New Point(71, 34)
            radPreview.Margin = New Padding(4, 7, 4, 7)
            radPreview.Name = "radPreview"
            radPreview.Size = New Size(116, 36)
            radPreview.TabIndex = 9
            radPreview.TabStop = True
            radPreview.Text = "プレビュー"
            radPreview.UseVisualStyleBackColor = True
            ' 
            ' btnExecute
            ' 
            btnExecute.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            btnExecute.BackColor = Color.FromArgb(CByte(75), CByte(95), CByte(131))
            btnExecute.Font = New Font("MS UI Gothic", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(128))
            btnExecute.ForeColor = Color.White
            btnExecute.Location = New Point(934, 20)
            btnExecute.Margin = New Padding(7, 10, 7, 10)
            btnExecute.Name = "btnExecute"
            btnExecute.Size = New Size(269, 111)
            btnExecute.TabIndex = 2
            btnExecute.Text = "実行"
            btnExecute.UseVisualStyleBackColor = False
            ' 
            ' toolTip1
            ' 
            toolTip1.IsBalloon = True
            toolTip1.ToolTipIcon = ToolTipIcon.Info
            toolTip1.ToolTipTitle = "Windows10/11でXPSビューワーを使う方法"
            ' 
            ' Form1
            ' 
            AutoScaleDimensions = New SizeF(96.0F, 96.0F)
            AutoScaleMode = AutoScaleMode.Dpi
            AutoScroll = True
            BackColor = Color.FromArgb(CByte(252), CByte(238), CByte(235))
            ClientSize = New Size(1255, 711)
            Controls.Add(prevWinForm)
            Controls.Add(panel1)
            MinimumSize = New Size(1024, 600)
            Name = "Form1"
            StartPosition = FormStartPosition.CenterScreen
            Text = "Reports.net かんたんなサンプル！"
            panel1.ResumeLayout(False)
            panel1.PerformLayout()
            ResumeLayout(False)
        End Sub

        Private WithEvents prevWinForm As PrintPreviewControl
        Private WithEvents panel1 As Panel
        Private WithEvents radGetPrintDocument As RadioButton
        Private WithEvents radXPS As RadioButton
        Private WithEvents radSVG As RadioButton
        Private WithEvents radPDF As RadioButton
        Private WithEvents radPrint As RadioButton
        Private WithEvents radPreview As RadioButton
        Private WithEvents btnExecute As Button
        Friend WithEvents saveFileDialog As SaveFileDialog
        Private WithEvents printDocument1 As Printing.PrintDocument
        Private WithEvents toolTip1 As ToolTip
    End Class
End Namespace