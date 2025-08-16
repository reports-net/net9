namespace Sample
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnExecute = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            radXPS = new System.Windows.Forms.RadioButton();
            radSVG = new System.Windows.Forms.RadioButton();
            radPDF = new System.Windows.Forms.RadioButton();
            radPrint = new System.Windows.Forms.RadioButton();
            radPreview = new System.Windows.Forms.RadioButton();
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            grpDesign = new System.Windows.Forms.GroupBox();
            radDesign2 = new System.Windows.Forms.RadioButton();
            radDesign1 = new System.Windows.Forms.RadioButton();
            richTextBox2 = new System.Windows.Forms.RichTextBox();
            cmbReportType = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            pnl = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            grpDesign.SuspendLayout();
            pnl.SuspendLayout();
            SuspendLayout();
            // 
            // btnExecute
            // 
            btnExecute.BackColor = System.Drawing.Color.FromArgb(75, 95, 131);
            btnExecute.Font = new System.Drawing.Font("Yu Gothic UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            btnExecute.ForeColor = System.Drawing.Color.White;
            btnExecute.Location = new System.Drawing.Point(751, 5);
            btnExecute.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            btnExecute.Name = "btnExecute";
            btnExecute.Size = new System.Drawing.Size(188, 56);
            btnExecute.TabIndex = 2;
            btnExecute.Text = "実行";
            btnExecute.UseVisualStyleBackColor = false;
            btnExecute.Click += btnExecute_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Controls.Add(radXPS);
            panel1.Controls.Add(radSVG);
            panel1.Controls.Add(radPDF);
            panel1.Controls.Add(radPrint);
            panel1.Controls.Add(radPreview);
            panel1.Controls.Add(btnExecute);
            panel1.Location = new System.Drawing.Point(14, 4);
            panel1.Margin = new System.Windows.Forms.Padding(2);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(968, 64);
            panel1.TabIndex = 11;
            // 
            // radXPS
            // 
            radXPS.AutoSize = true;
            radXPS.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radXPS.Location = new System.Drawing.Point(596, 17);
            radXPS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            radSVG.Location = new System.Drawing.Point(448, 19);
            radSVG.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            radPDF.Location = new System.Drawing.Point(301, 17);
            radPDF.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            radPrint.Location = new System.Drawing.Point(192, 19);
            radPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            radPreview.Location = new System.Drawing.Point(47, 19);
            radPreview.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            radPreview.Name = "radPreview";
            radPreview.Size = new System.Drawing.Size(116, 36);
            radPreview.TabIndex = 9;
            radPreview.TabStop = true;
            radPreview.Text = "プレビュー";
            radPreview.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = System.Drawing.Color.FromArgb(255, 253, 231);
            richTextBox1.Font = new System.Drawing.Font("BIZ UDゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            richTextBox1.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            richTextBox1.Location = new System.Drawing.Point(14, 80);
            richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new System.Drawing.Size(990, 138);
            richTextBox1.TabIndex = 13;
            richTextBox1.Text = "\n  このプログラムは Windows Form のプログラムですが、\n  WPFのプロジェクトをスタートアップに設定すれば 同じ機能のWPF版を起動することができます。\n\n  このサンプルプログラムでは、手軽にお試しいただける目的で、Excelファイルをデータベースとして使用しております。\n  そのため、Office 64bit版がインストールされている必要があります。";
            // 
            // grpDesign
            // 
            grpDesign.Controls.Add(radDesign2);
            grpDesign.Controls.Add(radDesign1);
            grpDesign.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            grpDesign.ForeColor = System.Drawing.Color.Blue;
            grpDesign.Location = new System.Drawing.Point(41, 84);
            grpDesign.Margin = new System.Windows.Forms.Padding(2);
            grpDesign.Name = "grpDesign";
            grpDesign.Padding = new System.Windows.Forms.Padding(2);
            grpDesign.Size = new System.Drawing.Size(384, 58);
            grpDesign.TabIndex = 16;
            grpDesign.TabStop = false;
            grpDesign.Text = "デザインを選択してください";
            // 
            // radDesign2
            // 
            radDesign2.AutoSize = true;
            radDesign2.Font = new System.Drawing.Font("Yu Gothic UI", 14F, System.Drawing.FontStyle.Bold);
            radDesign2.Location = new System.Drawing.Point(217, 27);
            radDesign2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            radDesign2.Name = "radDesign2";
            radDesign2.Size = new System.Drawing.Size(99, 29);
            radDesign2.TabIndex = 17;
            radDesign2.Tag = "1";
            radDesign2.Text = "デザイン2";
            radDesign2.UseVisualStyleBackColor = true;
            // 
            // radDesign1
            // 
            radDesign1.AutoSize = true;
            radDesign1.Checked = true;
            radDesign1.Font = new System.Drawing.Font("Yu Gothic UI", 14F, System.Drawing.FontStyle.Bold);
            radDesign1.Location = new System.Drawing.Point(17, 27);
            radDesign1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            radDesign1.Name = "radDesign1";
            radDesign1.Size = new System.Drawing.Size(96, 29);
            radDesign1.TabIndex = 16;
            radDesign1.TabStop = true;
            radDesign1.Tag = "0";
            radDesign1.Text = "デザイン1";
            radDesign1.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = System.Drawing.Color.FromArgb(225, 245, 254);
            richTextBox2.Font = new System.Drawing.Font("BIZ UDゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            richTextBox2.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
            richTextBox2.Location = new System.Drawing.Point(14, 226);
            richTextBox2.Margin = new System.Windows.Forms.Padding(2);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new System.Drawing.Size(990, 270);
            richTextBox2.TabIndex = 18;
            richTextBox2.Text = resources.GetString("richTextBox2.Text");
            richTextBox2.LinkClicked += richTextBox2_LinkClicked;
            // 
            // cmbReportType
            // 
            cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbReportType.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            cmbReportType.FormattingEnabled = true;
            cmbReportType.Items.AddRange(new object[] { "見積書 (四角で表を作成する手法・表紙と明細別デザイン・ハンコの印影出力)", "郵便番号一覧 (罫線で表を作成する手法・大量ページ・一覧のデザインを途中で変更・QRコード出力)", "請求書 (ほぼコードのみで帳票作成)", "商品大小分類 (グループごとに中間小計)", "いつでもデザイン変更 (データ再設定なしでデザイン変更)", "広告 (イメージPDF出力・装飾文字使用・画像出力)" });
            cmbReportType.Location = new System.Drawing.Point(36, 40);
            cmbReportType.Margin = new System.Windows.Forms.Padding(2);
            cmbReportType.Name = "cmbReportType";
            cmbReportType.Size = new System.Drawing.Size(1016, 38);
            cmbReportType.TabIndex = 19;
            cmbReportType.SelectedIndexChanged += cmbReportType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            label2.ForeColor = System.Drawing.Color.FromArgb(102, 102, 102);
            label2.Location = new System.Drawing.Point(30, 18);
            label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(217, 21);
            label2.TabIndex = 20;
            label2.Text = "サンプル帳票を選択してください。";
            // 
            // pnl
            // 
            pnl.Controls.Add(panel1);
            pnl.Controls.Add(richTextBox1);
            pnl.Controls.Add(richTextBox2);
            pnl.Location = new System.Drawing.Point(30, 152);
            pnl.Margin = new System.Windows.Forms.Padding(2);
            pnl.Name = "pnl";
            pnl.Size = new System.Drawing.Size(1016, 503);
            pnl.TabIndex = 21;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoScroll = true;
            BackColor = System.Drawing.Color.FromArgb(252, 238, 235);
            ClientSize = new System.Drawing.Size(1082, 668);
            Controls.Add(pnl);
            Controls.Add(label2);
            Controls.Add(cmbReportType);
            Controls.Add(grpDesign);
            Margin = new System.Windows.Forms.Padding(2);
            MinimumSize = new System.Drawing.Size(1024, 600);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "WEB API (REST API) から印刷データを取得して帳票出力するサンプル";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            grpDesign.ResumeLayout(false);
            grpDesign.PerformLayout();
            pnl.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        // ─── デザイナ保持フィールド ───
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radPDF;
        private System.Windows.Forms.RadioButton radPrint;
        private System.Windows.Forms.RadioButton radPreview;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox grpDesign;
        private System.Windows.Forms.RadioButton radDesign2;
        private System.Windows.Forms.RadioButton radDesign1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radXPS;
        private System.Windows.Forms.RadioButton radSVG;
        private System.Windows.Forms.Panel pnl;
    }
}