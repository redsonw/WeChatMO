namespace YueHuan
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            LoggerListBox = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label5 = new Label();
            PatchesButton = new Button();
            VersionLabel = new Label();
            DownloadLinkLabel = new LinkLabel();
            PatchInfoLabel = new Label();
            ReleaseLabel = new Label();
            groupBox1 = new GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // LoggerListBox
            // 
            LoggerListBox.Dock = DockStyle.Fill;
            LoggerListBox.FormattingEnabled = true;
            LoggerListBox.ItemHeight = 31;
            LoggerListBox.Location = new Point(6, 36);
            LoggerListBox.Margin = new Padding(6, 5, 6, 5);
            LoggerListBox.Name = "LoggerListBox";
            LoggerListBox.Size = new Size(874, 267);
            LoggerListBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 22);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(140, 31);
            label1.TabIndex = 1;
            label1.Text = "[ 最新版本 ]";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 75);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new Size(140, 31);
            label2.TabIndex = 2;
            label2.Text = "[ 下载微信 ]";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 128);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new Size(140, 31);
            label3.TabIndex = 3;
            label3.Text = "[ 补丁信息 ]";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(24, 181);
            label5.Margin = new Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Size = new Size(140, 31);
            label5.TabIndex = 5;
            label5.Text = "[ 发布时间 ]";
            // 
            // PatchesButton
            // 
            PatchesButton.Location = new Point(728, 117);
            PatchesButton.Margin = new Padding(0);
            PatchesButton.Name = "PatchesButton";
            PatchesButton.Size = new Size(180, 100);
            PatchesButton.TabIndex = 11;
            PatchesButton.Text = "解除限制";
            PatchesButton.UseVisualStyleBackColor = true;
            PatchesButton.Click += PatchesButton_Click;
            // 
            // VersionLabel
            // 
            VersionLabel.BorderStyle = BorderStyle.Fixed3D;
            VersionLabel.Location = new Point(180, 16);
            VersionLabel.Margin = new Padding(6, 0, 6, 0);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new Size(730, 42);
            VersionLabel.TabIndex = 12;
            VersionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DownloadLinkLabel
            // 
            DownloadLinkLabel.BorderStyle = BorderStyle.Fixed3D;
            DownloadLinkLabel.Location = new Point(180, 69);
            DownloadLinkLabel.Margin = new Padding(6, 0, 6, 0);
            DownloadLinkLabel.Name = "DownloadLinkLabel";
            DownloadLinkLabel.Size = new Size(730, 42);
            DownloadLinkLabel.TabIndex = 13;
            DownloadLinkLabel.TextAlign = ContentAlignment.MiddleLeft;
            DownloadLinkLabel.LinkClicked += DownloadLinkLabel_LinkClicked;
            // 
            // PatchInfoLabel
            // 
            PatchInfoLabel.BorderStyle = BorderStyle.Fixed3D;
            PatchInfoLabel.Location = new Point(180, 122);
            PatchInfoLabel.Margin = new Padding(6, 0, 6, 0);
            PatchInfoLabel.Name = "PatchInfoLabel";
            PatchInfoLabel.Size = new Size(538, 42);
            PatchInfoLabel.TabIndex = 14;
            PatchInfoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ReleaseLabel
            // 
            ReleaseLabel.BorderStyle = BorderStyle.Fixed3D;
            ReleaseLabel.Location = new Point(180, 175);
            ReleaseLabel.Margin = new Padding(6, 0, 6, 0);
            ReleaseLabel.Name = "ReleaseLabel";
            ReleaseLabel.Size = new Size(538, 42);
            ReleaseLabel.TabIndex = 16;
            ReleaseLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LoggerListBox);
            groupBox1.Location = new Point(24, 224);
            groupBox1.Margin = new Padding(6, 5, 6, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(6, 5, 6, 5);
            groupBox1.Size = new Size(886, 308);
            groupBox1.TabIndex = 17;
            groupBox1.TabStop = false;
            groupBox1.Text = "日志信息";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(936, 551);
            Controls.Add(groupBox1);
            Controls.Add(ReleaseLabel);
            Controls.Add(PatchInfoLabel);
            Controls.Add(DownloadLinkLabel);
            Controls.Add(VersionLabel);
            Controls.Add(PatchesButton);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6, 5, 6, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            Opacity = 0.98D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "解除微信多开限制";
            Load += MainForm_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox LoggerListBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox TextBox;
        private TextBox DownloadTextBox;
        private TextBox PatchInfoTextBox;
        private TextBox AuthorTextBox;
        private TextBox ReleaseTextBox;
        private Button PatchesButton;
        private Label VersionLabel;
        private LinkLabel DownloadLinkLabel;
        private Label PatchInfoLabel;
        private Label ReleaseLabel;
        private GroupBox groupBox1;
    }
}