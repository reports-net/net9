namespace Sample
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnExecute = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            radGetPrintDocument = new System.Windows.Forms.RadioButton();
            radXPS = new System.Windows.Forms.RadioButton();
            radSVG = new System.Windows.Forms.RadioButton();
            radPDF = new System.Windows.Forms.RadioButton();
            radPrint = new System.Windows.Forms.RadioButton();
            radPreview = new System.Windows.Forms.RadioButton();
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            prevWinForm = new System.Windows.Forms.PrintPreviewControl();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnExecute
            // 
            btnExecute.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnExecute.BackColor = System.Drawing.Color.FromArgb(75, 95, 131);
            btnExecute.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            btnExecute.ForeColor = System.Drawing.Color.White;
            btnExecute.Location = new System.Drawing.Point(934, 20);
            btnExecute.Margin = new System.Windows.Forms.Padding(7, 10, 7, 10);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new System.Drawing.Size(269, 111);
            btnExecute.TabIndex = 2;
            btnExecute.Text = "実行";
            btnExecute.UseVisualStyleBackColor = false;
            btnExecute.Click += btnExecute_Click;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Controls.Add(radGetPrintDocument);
            panel1.Controls.Add(radXPS);
            panel1.Controls.Add(radSVG);
            panel1.Controls.Add(radPDF);
            panel1.Controls.Add(radPrint);
            panel1.Controls.Add(radPreview);
            panel1.Controls.Add(btnExecute);
            panel1.Location = new System.Drawing.Point(12, 8);
            panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1229, 155);
            panel1.TabIndex = 20;
            // 
            // radGetPrintDocument
            // 
            radGetPrintDocument.AutoSize = true;
            radGetPrintDocument.Checked = true;
            radGetPrintDocument.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radGetPrintDocument.Location = new System.Drawing.Point(71, 95);
            radGetPrintDocument.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            radGetPrintDocument.Name = "radGetPrintDocument";
            radGetPrintDocument.Size = new System.Drawing.Size(243, 36);
            radGetPrintDocument.TabIndex = 14;
            radGetPrintDocument.TabStop = true;
            radGetPrintDocument.Text = "独自プレビュー ↓↓↓";
            radGetPrintDocument.UseVisualStyleBackColor = true;
            // 
            // radXPS
            // 
            radXPS.AutoSize = true;
            radXPS.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radXPS.Location = new System.Drawing.Point(733, 32);
            radXPS.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            radXPS.Name = "radXPS";
            radXPS.Size = new System.Drawing.Size(122, 36);
            radXPS.TabIndex = 13;
            radXPS.Text = "XPS出力";
            radXPS.UseVisualStyleBackColor = true;
            // 
            // radSVG
            // 
            radSVG.AutoSize = true;
            radSVG.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radSVG.Location = new System.Drawing.Point(555, 34);
            radSVG.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            radSVG.Name = "radSVG";
            radSVG.Size = new System.Drawing.Size(125, 36);
            radSVG.TabIndex = 12;
            radSVG.Text = "SVG出力";
            radSVG.UseVisualStyleBackColor = true;
            // 
            // radPDF
            // 
            radPDF.AutoSize = true;
            radPDF.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radPDF.Location = new System.Drawing.Point(369, 32);
            radPDF.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            radPDF.Name = "radPDF";
            radPDF.Size = new System.Drawing.Size(123, 36);
            radPDF.TabIndex = 11;
            radPDF.Text = "PDF出力";
            radPDF.UseVisualStyleBackColor = true;
            // 
            // radPrint
            // 
            radPrint.AutoSize = true;
            radPrint.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radPrint.Location = new System.Drawing.Point(234, 34);
            radPrint.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            radPrint.Name = "radPrint";
            radPrint.Size = new System.Drawing.Size(80, 36);
            radPrint.TabIndex = 10;
            radPrint.Text = "印刷";
            radPrint.UseVisualStyleBackColor = true;
            // 
            // radPreview
            // 
            radPreview.AutoSize = true;
            radPreview.Checked = true;
            radPreview.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radPreview.Location = new System.Drawing.Point(71, 34);
            radPreview.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            radPreview.Name = "radPreview";
            radPreview.Size = new System.Drawing.Size(116, 36);
            radPreview.TabIndex = 9;
            radPreview.TabStop = true;
            radPreview.Text = "プレビュー";
            radPreview.UseVisualStyleBackColor = true;
            // 
            // prevWinForm
            // 
            prevWinForm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            prevWinForm.AutoZoom = false;
            prevWinForm.Location = new System.Drawing.Point(12, 171);
            prevWinForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            prevWinForm.Name = "prevWinForm";
            prevWinForm.Size = new System.Drawing.Size(1229, 527);
            prevWinForm.TabIndex = 21;
            prevWinForm.Zoom = 1D;
            // 
            // toolTip1
            // 
            toolTip1.IsBalloon = true;
            toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            toolTip1.ToolTipTitle = "Windows10/11でXPSビューワーを使う方法";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoScroll = true;
            BackColor = System.Drawing.Color.FromArgb(252, 238, 235);
            ClientSize = new System.Drawing.Size(1255, 711);
            Controls.Add(prevWinForm);
            Controls.Add(panel1);
            MinimumSize = new System.Drawing.Size(1024, 600);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Reports.net かんたんなサンプル！";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radPDF;
        private System.Windows.Forms.RadioButton radPrint;
        private System.Windows.Forms.RadioButton radPreview;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.RadioButton radXPS;
        private System.Windows.Forms.RadioButton radSVG;
        private System.Windows.Forms.PrintPreviewControl prevWinForm;
        private System.Windows.Forms.RadioButton radGetPrintDocument;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}