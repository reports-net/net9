namespace WinFormsApp1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnGet = new System.Windows.Forms.Button();
            btnPost = new System.Windows.Forms.Button();
            opt広告 = new System.Windows.Forms.RadioButton();
            opt見積書 = new System.Windows.Forms.RadioButton();
            opt住所一覧 = new System.Windows.Forms.RadioButton();
            opt10の倍数 = new System.Windows.Forms.RadioButton();
            opt単純な印刷データ = new System.Windows.Forms.RadioButton();
            panel1 = new System.Windows.Forms.Panel();
            radPDF = new System.Windows.Forms.RadioButton();
            radPrint = new System.Windows.Forms.RadioButton();
            radPreview = new System.Windows.Forms.RadioButton();
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            label1 = new System.Windows.Forms.Label();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            opt請求書 = new System.Windows.Forms.RadioButton();
            opt商品一覧 = new System.Windows.Forms.RadioButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            radWebApiGcp = new System.Windows.Forms.RadioButton();
            radWebApiAws = new System.Windows.Forms.RadioButton();
            radWebApiAzureLinux = new System.Windows.Forms.RadioButton();
            radWebApiAzureWin = new System.Windows.Forms.RadioButton();
            radWebApiLocal = new System.Windows.Forms.RadioButton();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnGet
            // 
            btnGet.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnGet.BackColor = System.Drawing.Color.FromArgb(200, 230, 255);
            btnGet.Location = new System.Drawing.Point(828, 12);
            btnGet.Margin = new System.Windows.Forms.Padding(7);
            btnGet.Name = "btnGet";
            btnGet.Size = new System.Drawing.Size(236, 76);
            btnGet.TabIndex = 0;
            btnGet.Text = "実行 (GET)";
            btnGet.UseVisualStyleBackColor = false;
            btnGet.Click += BtnGet_Click;
            // 
            // btnPost
            // 
            btnPost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnPost.BackColor = System.Drawing.Color.FromArgb(255, 255, 200);
            btnPost.Location = new System.Drawing.Point(552, 12);
            btnPost.Margin = new System.Windows.Forms.Padding(7);
            btnPost.Name = "btnPost";
            btnPost.Size = new System.Drawing.Size(256, 76);
            btnPost.TabIndex = 2;
            btnPost.Text = "実行 (POST)";
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += BtnPost_Click;
            // 
            // opt広告
            // 
            opt広告.AutoSize = true;
            opt広告.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            opt広告.Location = new System.Drawing.Point(1044, 133);
            opt広告.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            opt広告.Name = "opt広告";
            opt広告.Size = new System.Drawing.Size(75, 34);
            opt広告.TabIndex = 10;
            opt広告.Tag = "6";
            opt広告.Text = "広告";
            opt広告.UseVisualStyleBackColor = true;
            // 
            // opt見積書
            // 
            opt見積書.AutoSize = true;
            opt見積書.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            opt見積書.Location = new System.Drawing.Point(595, 133);
            opt見積書.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            opt見積書.Name = "opt見積書";
            opt見積書.Size = new System.Drawing.Size(97, 34);
            opt見積書.TabIndex = 9;
            opt見積書.Tag = "3";
            opt見積書.Text = "見積書";
            opt見積書.UseVisualStyleBackColor = true;
            // 
            // opt住所一覧
            // 
            opt住所一覧.AutoSize = true;
            opt住所一覧.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            opt住所一覧.Location = new System.Drawing.Point(436, 133);
            opt住所一覧.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            opt住所一覧.Name = "opt住所一覧";
            opt住所一覧.Size = new System.Drawing.Size(119, 34);
            opt住所一覧.TabIndex = 8;
            opt住所一覧.Tag = "2";
            opt住所一覧.Text = "住所一覧";
            opt住所一覧.UseVisualStyleBackColor = true;
            // 
            // opt10の倍数
            // 
            opt10の倍数.AutoSize = true;
            opt10の倍数.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            opt10の倍数.Location = new System.Drawing.Point(291, 132);
            opt10の倍数.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            opt10の倍数.Name = "opt10の倍数";
            opt10の倍数.Size = new System.Drawing.Size(114, 34);
            opt10の倍数.TabIndex = 7;
            opt10の倍数.Tag = "1";
            opt10の倍数.Text = "10の倍数";
            opt10の倍数.UseVisualStyleBackColor = true;
            // 
            // opt単純な印刷データ
            // 
            opt単純な印刷データ.AutoSize = true;
            opt単純な印刷データ.Checked = true;
            opt単純な印刷データ.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            opt単純な印刷データ.Location = new System.Drawing.Point(78, 133);
            opt単純な印刷データ.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            opt単純な印刷データ.Name = "opt単純な印刷データ";
            opt単純な印刷データ.Size = new System.Drawing.Size(188, 34);
            opt単純な印刷データ.TabIndex = 6;
            opt単純な印刷データ.TabStop = true;
            opt単純な印刷データ.Tag = "0";
            opt単純な印刷データ.Text = "単純な印刷データ";
            opt単純な印刷データ.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel1.Controls.Add(radPDF);
            panel1.Controls.Add(radPrint);
            panel1.Controls.Add(radPreview);
            panel1.Controls.Add(btnPost);
            panel1.Controls.Add(btnGet);
            panel1.Location = new System.Drawing.Point(72, 175);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1098, 104);
            panel1.TabIndex = 11;
            // 
            // radPDF
            // 
            radPDF.AutoSize = true;
            radPDF.Font = new System.Drawing.Font("Yu Gothic UI", 18F, System.Drawing.FontStyle.Bold);
            radPDF.Location = new System.Drawing.Point(383, 30);
            radPDF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            radPrint.Location = new System.Drawing.Point(228, 30);
            radPrint.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            radPreview.Location = new System.Drawing.Point(57, 30);
            radPreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            radPreview.Name = "radPreview";
            radPreview.Size = new System.Drawing.Size(116, 36);
            radPreview.TabIndex = 9;
            radPreview.TabStop = true;
            radPreview.Text = "プレビュー";
            radPreview.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(393, 126);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(0, 37);
            label1.TabIndex = 12;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            richTextBox1.BackColor = System.Drawing.Color.Gray;
            richTextBox1.Font = new System.Drawing.Font("BIZ UDゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            richTextBox1.ForeColor = System.Drawing.Color.White;
            richTextBox1.Location = new System.Drawing.Point(72, 285);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new System.Drawing.Size(1098, 420);
            richTextBox1.TabIndex = 13;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // opt請求書
            // 
            opt請求書.AutoSize = true;
            opt請求書.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            opt請求書.Location = new System.Drawing.Point(733, 132);
            opt請求書.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            opt請求書.Name = "opt請求書";
            opt請求書.Size = new System.Drawing.Size(97, 34);
            opt請求書.TabIndex = 14;
            opt請求書.Tag = "4";
            opt請求書.Text = "請求書";
            opt請求書.UseVisualStyleBackColor = true;
            // 
            // opt商品一覧
            // 
            opt商品一覧.AutoSize = true;
            opt商品一覧.Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold);
            opt商品一覧.Location = new System.Drawing.Point(878, 133);
            opt商品一覧.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            opt商品一覧.Name = "opt商品一覧";
            opt商品一覧.Size = new System.Drawing.Size(141, 34);
            opt商品一覧.TabIndex = 15;
            opt商品一覧.Tag = "5";
            opt商品一覧.Text = "商品一覧　";
            opt商品一覧.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radWebApiGcp);
            groupBox1.Controls.Add(radWebApiAws);
            groupBox1.Controls.Add(radWebApiAzureLinux);
            groupBox1.Controls.Add(radWebApiAzureWin);
            groupBox1.Controls.Add(radWebApiLocal);
            groupBox1.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 128);
            groupBox1.ForeColor = System.Drawing.Color.Blue;
            groupBox1.Location = new System.Drawing.Point(39, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(1131, 111);
            groupBox1.TabIndex = 16;
            groupBox1.TabStop = false;
            groupBox1.Text = "帳票作成 WEB API (REST API) の配置場所選択";
            // 
            // radWebApiGcp
            // 
            radWebApiGcp.AutoSize = true;
            radWebApiGcp.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Bold);
            radWebApiGcp.Location = new System.Drawing.Point(896, 44);
            radWebApiGcp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            radWebApiGcp.Name = "radWebApiGcp";
            radWebApiGcp.Size = new System.Drawing.Size(189, 34);
            radWebApiGcp.TabIndex = 21;
            radWebApiGcp.Tag = "4";
            radWebApiGcp.Text = "GCP GCE Debian";
            radWebApiGcp.UseVisualStyleBackColor = true;
            // 
            // radWebApiAws
            // 
            radWebApiAws.AutoSize = true;
            radWebApiAws.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Bold);
            radWebApiAws.Location = new System.Drawing.Point(625, 44);
            radWebApiAws.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            radWebApiAws.Name = "radWebApiAws";
            radWebApiAws.Size = new System.Drawing.Size(258, 34);
            radWebApiAws.TabIndex = 19;
            radWebApiAws.Tag = "3";
            radWebApiAws.Text = "AWS EC2 Amazon Linux";
            radWebApiAws.UseVisualStyleBackColor = true;
            // 
            // radWebApiAzureLinux
            // 
            radWebApiAzureLinux.AutoSize = true;
            radWebApiAzureLinux.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Bold);
            radWebApiAzureLinux.Location = new System.Drawing.Point(467, 44);
            radWebApiAzureLinux.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            radWebApiAzureLinux.Name = "radWebApiAzureLinux";
            radWebApiAzureLinux.Size = new System.Drawing.Size(142, 34);
            radWebApiAzureLinux.TabIndex = 18;
            radWebApiAzureLinux.Tag = "2";
            radWebApiAzureLinux.Text = "Azure Linux";
            radWebApiAzureLinux.UseVisualStyleBackColor = true;
            // 
            // radWebApiAzureWin
            // 
            radWebApiAzureWin.AutoSize = true;
            radWebApiAzureWin.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Bold);
            radWebApiAzureWin.Location = new System.Drawing.Point(259, 44);
            radWebApiAzureWin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            radWebApiAzureWin.Name = "radWebApiAzureWin";
            radWebApiAzureWin.Size = new System.Drawing.Size(195, 34);
            radWebApiAzureWin.TabIndex = 17;
            radWebApiAzureWin.Tag = "1";
            radWebApiAzureWin.Text = "Azure Win Server";
            radWebApiAzureWin.UseVisualStyleBackColor = true;
            // 
            // radWebApiLocal
            // 
            radWebApiLocal.AutoSize = true;
            radWebApiLocal.Checked = true;
            radWebApiLocal.Font = new System.Drawing.Font("Yu Gothic UI", 15.75F, System.Drawing.FontStyle.Bold);
            radWebApiLocal.Location = new System.Drawing.Point(24, 44);
            radWebApiLocal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            radWebApiLocal.Name = "radWebApiLocal";
            radWebApiLocal.Size = new System.Drawing.Size(202, 34);
            radWebApiLocal.TabIndex = 16;
            radWebApiLocal.TabStop = true;
            radWebApiLocal.Tag = "0";
            radWebApiLocal.Text = "ローカルデバッグ環境";
            radWebApiLocal.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoScroll = true;
            BackColor = System.Drawing.Color.FromArgb(252, 238, 235);
            ClientSize = new System.Drawing.Size(1221, 740);
            Controls.Add(opt広告);
            Controls.Add(groupBox1);
            Controls.Add(opt商品一覧);
            Controls.Add(opt請求書);
            Controls.Add(richTextBox1);
            Controls.Add(label1);
            Controls.Add(panel1);
            Controls.Add(opt見積書);
            Controls.Add(opt住所一覧);
            Controls.Add(opt10の倍数);
            Controls.Add(opt単純な印刷データ);
            Font = new System.Drawing.Font("Yu Gothic UI", 20.25F, System.Drawing.FontStyle.Bold);
            Margin = new System.Windows.Forms.Padding(7);
            MinimumSize = new System.Drawing.Size(1024, 600);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "WEB API (REST API) から印刷データを取得して帳票出力するサンプル";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.RadioButton opt広告;
        private System.Windows.Forms.RadioButton opt見積書;
        private System.Windows.Forms.RadioButton opt住所一覧;
        private System.Windows.Forms.RadioButton opt10の倍数;
        private System.Windows.Forms.RadioButton opt単純な印刷データ;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radPDF;
        private System.Windows.Forms.RadioButton radPrint;
        private System.Windows.Forms.RadioButton radPreview;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RadioButton opt請求書;
        private System.Windows.Forms.RadioButton opt商品一覧;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radWebApiGcp;
        private System.Windows.Forms.RadioButton radWebApiAws;
        private System.Windows.Forms.RadioButton radWebApiAzureLinux;
        private System.Windows.Forms.RadioButton radWebApiAzureWin;
        private System.Windows.Forms.RadioButton radWebApiLocal;
    }
}