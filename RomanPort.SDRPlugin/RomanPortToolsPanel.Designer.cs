namespace RomanPort.BetterSDRRecorder
{
    partial class RomanPortToolsPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rewindStatus = new System.Windows.Forms.Label();
            this.rewindSaveBtn = new System.Windows.Forms.Button();
            this.rewindUsageBar = new System.Windows.Forms.ProgressBar();
            this.recordAudioBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.recordAudioOutput = new System.Windows.Forms.ComboBox();
            this.recordAudioFormat = new System.Windows.Forms.ComboBox();
            this.recordAudioStatus = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.rewindSaveAndRecordBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.recordBroadbandStatus = new System.Windows.Forms.Label();
            this.recordBroadbandOutput = new System.Windows.Forms.ComboBox();
            this.recordBroadbandFormat = new System.Windows.Forms.ComboBox();
            this.recordBroadbandBtn = new System.Windows.Forms.Button();
            this.rewindTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rewindTime)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rewindTime);
            this.groupBox1.Controls.Add(this.rewindSaveAndRecordBtn);
            this.groupBox1.Controls.Add(this.rewindStatus);
            this.groupBox1.Controls.Add(this.rewindSaveBtn);
            this.groupBox1.Controls.Add(this.rewindUsageBar);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rewind Recording";
            // 
            // rewindStatus
            // 
            this.rewindStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rewindStatus.Location = new System.Drawing.Point(52, 20);
            this.rewindStatus.Name = "rewindStatus";
            this.rewindStatus.Size = new System.Drawing.Size(159, 15);
            this.rewindStatus.TabIndex = 2;
            // 
            // rewindSaveBtn
            // 
            this.rewindSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rewindSaveBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rewindSaveBtn.Location = new System.Drawing.Point(93, 43);
            this.rewindSaveBtn.Name = "rewindSaveBtn";
            this.rewindSaveBtn.Size = new System.Drawing.Size(52, 23);
            this.rewindSaveBtn.TabIndex = 1;
            this.rewindSaveBtn.Text = "Save";
            this.rewindSaveBtn.UseVisualStyleBackColor = true;
            this.rewindSaveBtn.Click += new System.EventHandler(this.rewindSaveBtn_Click);
            // 
            // rewindUsageBar
            // 
            this.rewindUsageBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rewindUsageBar.Location = new System.Drawing.Point(6, 43);
            this.rewindUsageBar.Name = "rewindUsageBar";
            this.rewindUsageBar.Size = new System.Drawing.Size(81, 23);
            this.rewindUsageBar.TabIndex = 0;
            // 
            // recordAudioBtn
            // 
            this.recordAudioBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.recordAudioBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.recordAudioBtn.Location = new System.Drawing.Point(121, 45);
            this.recordAudioBtn.Name = "recordAudioBtn";
            this.recordAudioBtn.Size = new System.Drawing.Size(43, 23);
            this.recordAudioBtn.TabIndex = 0;
            this.recordAudioBtn.Text = "Start";
            this.recordAudioBtn.UseVisualStyleBackColor = true;
            this.recordAudioBtn.Click += new System.EventHandler(this.recordAudioBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.recordAudioStatus);
            this.groupBox2.Controls.Add(this.recordAudioOutput);
            this.groupBox2.Controls.Add(this.recordAudioFormat);
            this.groupBox2.Controls.Add(this.recordAudioBtn);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(3, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(172, 74);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recording - Audio";
            // 
            // recordAudioOutput
            // 
            this.recordAudioOutput.DisplayMember = "PCM16";
            this.recordAudioOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recordAudioOutput.FormattingEnabled = true;
            this.recordAudioOutput.Items.AddRange(new object[] {
            "Raw",
            "Wav"});
            this.recordAudioOutput.Location = new System.Drawing.Point(7, 19);
            this.recordAudioOutput.Name = "recordAudioOutput";
            this.recordAudioOutput.Size = new System.Drawing.Size(71, 21);
            this.recordAudioOutput.TabIndex = 3;
            // 
            // recordAudioFormat
            // 
            this.recordAudioFormat.DisplayMember = "PCM16";
            this.recordAudioFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recordAudioFormat.FormattingEnabled = true;
            this.recordAudioFormat.Items.AddRange(new object[] {
            "PCM8",
            "PCM16",
            "Float32"});
            this.recordAudioFormat.Location = new System.Drawing.Point(83, 19);
            this.recordAudioFormat.Name = "recordAudioFormat";
            this.recordAudioFormat.Size = new System.Drawing.Size(81, 21);
            this.recordAudioFormat.TabIndex = 2;
            // 
            // recordAudioStatus
            // 
            this.recordAudioStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordAudioStatus.Location = new System.Drawing.Point(6, 45);
            this.recordAudioStatus.Name = "recordAudioStatus";
            this.recordAudioStatus.Size = new System.Drawing.Size(109, 23);
            this.recordAudioStatus.TabIndex = 4;
            this.recordAudioStatus.Text = "00:00:00 - 0 MB";
            this.recordAudioStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.button1.Location = new System.Drawing.Point(179, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 149);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start All";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // rewindSaveAndRecordBtn
            // 
            this.rewindSaveAndRecordBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rewindSaveAndRecordBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rewindSaveAndRecordBtn.Location = new System.Drawing.Point(144, 43);
            this.rewindSaveAndRecordBtn.Name = "rewindSaveAndRecordBtn";
            this.rewindSaveAndRecordBtn.Size = new System.Drawing.Size(67, 23);
            this.rewindSaveAndRecordBtn.TabIndex = 3;
            this.rewindSaveAndRecordBtn.Text = "+ Record";
            this.rewindSaveAndRecordBtn.UseVisualStyleBackColor = true;
            this.rewindSaveAndRecordBtn.Click += new System.EventHandler(this.rewindSaveAndRecordBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.recordBroadbandStatus);
            this.groupBox3.Controls.Add(this.recordBroadbandOutput);
            this.groupBox3.Controls.Add(this.recordBroadbandFormat);
            this.groupBox3.Controls.Add(this.recordBroadbandBtn);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox3.Location = new System.Drawing.Point(3, 161);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(172, 74);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Recording - Baseband";
            // 
            // recordBroadbandStatus
            // 
            this.recordBroadbandStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordBroadbandStatus.Location = new System.Drawing.Point(6, 45);
            this.recordBroadbandStatus.Name = "recordBroadbandStatus";
            this.recordBroadbandStatus.Size = new System.Drawing.Size(109, 23);
            this.recordBroadbandStatus.TabIndex = 4;
            this.recordBroadbandStatus.Text = "00:00:00 - 0 MB";
            this.recordBroadbandStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // recordBroadbandOutput
            // 
            this.recordBroadbandOutput.DisplayMember = "PCM16";
            this.recordBroadbandOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recordBroadbandOutput.FormattingEnabled = true;
            this.recordBroadbandOutput.Items.AddRange(new object[] {
            "Raw",
            "Wav"});
            this.recordBroadbandOutput.Location = new System.Drawing.Point(7, 19);
            this.recordBroadbandOutput.Name = "recordBroadbandOutput";
            this.recordBroadbandOutput.Size = new System.Drawing.Size(71, 21);
            this.recordBroadbandOutput.TabIndex = 3;
            // 
            // recordBroadbandFormat
            // 
            this.recordBroadbandFormat.DisplayMember = "PCM16";
            this.recordBroadbandFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.recordBroadbandFormat.FormattingEnabled = true;
            this.recordBroadbandFormat.Items.AddRange(new object[] {
            "PCM8",
            "PCM16",
            "Float32"});
            this.recordBroadbandFormat.Location = new System.Drawing.Point(83, 19);
            this.recordBroadbandFormat.Name = "recordBroadbandFormat";
            this.recordBroadbandFormat.Size = new System.Drawing.Size(81, 21);
            this.recordBroadbandFormat.TabIndex = 2;
            // 
            // recordBroadbandBtn
            // 
            this.recordBroadbandBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.recordBroadbandBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.recordBroadbandBtn.Location = new System.Drawing.Point(121, 45);
            this.recordBroadbandBtn.Name = "recordBroadbandBtn";
            this.recordBroadbandBtn.Size = new System.Drawing.Size(43, 23);
            this.recordBroadbandBtn.TabIndex = 0;
            this.recordBroadbandBtn.Text = "Start";
            this.recordBroadbandBtn.UseVisualStyleBackColor = true;
            this.recordBroadbandBtn.Click += new System.EventHandler(this.recordBroadbandBtn_Click);
            // 
            // rewindTime
            // 
            this.rewindTime.Location = new System.Drawing.Point(7, 17);
            this.rewindTime.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.rewindTime.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.rewindTime.Name = "rewindTime";
            this.rewindTime.Size = new System.Drawing.Size(46, 20);
            this.rewindTime.TabIndex = 4;
            this.rewindTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.rewindTime.ValueChanged += new System.EventHandler(this.rewindTime_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "PRE-RELEASE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // RomanPortToolsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.MinimumSize = new System.Drawing.Size(200, 244);
            this.Name = "RomanPortToolsPanel";
            this.Size = new System.Drawing.Size(223, 275);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rewindTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button rewindSaveBtn;
        private System.Windows.Forms.ProgressBar rewindUsageBar;
        private System.Windows.Forms.Label rewindStatus;
        private System.Windows.Forms.Button recordAudioBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox recordAudioFormat;
        private System.Windows.Forms.ComboBox recordAudioOutput;
        private System.Windows.Forms.Label recordAudioStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button rewindSaveAndRecordBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label recordBroadbandStatus;
        private System.Windows.Forms.ComboBox recordBroadbandOutput;
        private System.Windows.Forms.ComboBox recordBroadbandFormat;
        private System.Windows.Forms.Button recordBroadbandBtn;
        private System.Windows.Forms.NumericUpDown rewindTime;
        private System.Windows.Forms.Label label1;
    }
}
