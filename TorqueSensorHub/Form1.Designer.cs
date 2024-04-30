
namespace TorqueSensorHub
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SourcesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AddSourceButton = new System.Windows.Forms.Button();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timestampCheckbox = new System.Windows.Forms.CheckBox();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.SyncTriggerBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SaveTypeBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BrowseFileButton = new System.Windows.Forms.Button();
            this.FilePathBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.InternalTriggerActive = new System.Windows.Forms.CheckBox();
            this.TimerPeriodSelect = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.AutomationButton = new System.Windows.Forms.Button();
            this.autoFeedbackSource = new System.Windows.Forms.ComboBox();
            this.autoTorqueSource = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.FancyDemoButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimerPeriodSelect)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SourcesPanel
            // 
            this.SourcesPanel.AutoScroll = true;
            this.SourcesPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SourcesPanel.Location = new System.Drawing.Point(12, 41);
            this.SourcesPanel.Name = "SourcesPanel";
            this.SourcesPanel.Size = new System.Drawing.Size(245, 397);
            this.SourcesPanel.TabIndex = 2;
            // 
            // AddSourceButton
            // 
            this.AddSourceButton.Location = new System.Drawing.Point(182, 12);
            this.AddSourceButton.Name = "AddSourceButton";
            this.AddSourceButton.Size = new System.Drawing.Size(75, 23);
            this.AddSourceButton.TabIndex = 3;
            this.AddSourceButton.Text = "Add Source";
            this.AddSourceButton.UseVisualStyleBackColor = true;
            this.AddSourceButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // StartStopButton
            // 
            this.StartStopButton.Location = new System.Drawing.Point(168, 183);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(75, 23);
            this.StartStopButton.TabIndex = 4;
            this.StartStopButton.Text = "Start";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.timestampCheckbox);
            this.groupBox1.Controls.Add(this.openFolderButton);
            this.groupBox1.Controls.Add(this.SyncTriggerBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.SaveTypeBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.BrowseFileButton);
            this.groupBox1.Controls.Add(this.FilePathBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.StartStopButton);
            this.groupBox1.Location = new System.Drawing.Point(263, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(249, 212);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // timestampCheckbox
            // 
            this.timestampCheckbox.AutoSize = true;
            this.timestampCheckbox.Checked = true;
            this.timestampCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.timestampCheckbox.Location = new System.Drawing.Point(144, 19);
            this.timestampCheckbox.Name = "timestampCheckbox";
            this.timestampCheckbox.Size = new System.Drawing.Size(99, 17);
            this.timestampCheckbox.TabIndex = 3;
            this.timestampCheckbox.Text = "Add Timestamp";
            this.timestampCheckbox.UseVisualStyleBackColor = true;
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(92, 62);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(84, 24);
            this.openFolderButton.TabIndex = 12;
            this.openFolderButton.Text = "Open Folder";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // SyncTriggerBox
            // 
            this.SyncTriggerBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SyncTriggerBox.FormattingEnabled = true;
            this.SyncTriggerBox.Items.AddRange(new object[] {
            "Unsyncronised, single-file",
            "Syncronised, single-file",
            "Unsyncronised, multi-file",
            "Syncronised, multi-file"});
            this.SyncTriggerBox.Location = new System.Drawing.Point(6, 156);
            this.SyncTriggerBox.Name = "SyncTriggerBox";
            this.SyncTriggerBox.Size = new System.Drawing.Size(237, 21);
            this.SyncTriggerBox.TabIndex = 11;
            this.SyncTriggerBox.SelectedIndexChanged += new System.EventHandler(this.SyncTriggerBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Sync trigger:";
            // 
            // SaveTypeBox
            // 
            this.SaveTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SaveTypeBox.FormattingEnabled = true;
            this.SaveTypeBox.Items.AddRange(new object[] {
            "Unsyncronised, single-file",
            "Syncronised, single-file",
            "Unsyncronised, multi-file",
            "Syncronised, multi-file"});
            this.SaveTypeBox.Location = new System.Drawing.Point(6, 116);
            this.SaveTypeBox.Name = "SaveTypeBox";
            this.SaveTypeBox.Size = new System.Drawing.Size(237, 21);
            this.SaveTypeBox.TabIndex = 9;
            this.SaveTypeBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Save type:";
            // 
            // BrowseFileButton
            // 
            this.BrowseFileButton.Location = new System.Drawing.Point(182, 62);
            this.BrowseFileButton.Name = "BrowseFileButton";
            this.BrowseFileButton.Size = new System.Drawing.Size(61, 24);
            this.BrowseFileButton.TabIndex = 7;
            this.BrowseFileButton.Text = "Browse";
            this.BrowseFileButton.UseVisualStyleBackColor = true;
            this.BrowseFileButton.Click += new System.EventHandler(this.BrowseFileButton_Click);
            // 
            // FilePathBox
            // 
            this.FilePathBox.Location = new System.Drawing.Point(6, 36);
            this.FilePathBox.Name = "FilePathBox";
            this.FilePathBox.Size = new System.Drawing.Size(237, 20);
            this.FilePathBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Save location (Base path):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.InternalTriggerActive);
            this.groupBox2.Controls.Add(this.TimerPeriodSelect);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(263, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(249, 64);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Internal Trigger";
            // 
            // InternalTriggerActive
            // 
            this.InternalTriggerActive.AutoSize = true;
            this.InternalTriggerActive.Location = new System.Drawing.Point(178, 42);
            this.InternalTriggerActive.Name = "InternalTriggerActive";
            this.InternalTriggerActive.Size = new System.Drawing.Size(65, 17);
            this.InternalTriggerActive.TabIndex = 2;
            this.InternalTriggerActive.Text = "Enabled";
            this.InternalTriggerActive.UseVisualStyleBackColor = true;
            this.InternalTriggerActive.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // TimerPeriodSelect
            // 
            this.TimerPeriodSelect.Location = new System.Drawing.Point(123, 16);
            this.TimerPeriodSelect.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TimerPeriodSelect.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TimerPeriodSelect.Name = "TimerPeriodSelect";
            this.TimerPeriodSelect.Size = new System.Drawing.Size(120, 20);
            this.TimerPeriodSelect.TabIndex = 1;
            this.TimerPeriodSelect.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TimerPeriodSelect.ValueChanged += new System.EventHandler(this.TimerPeriodSelect_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Period (ms)";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.CheckFileExists = false;
            this.openFileDialog1.DefaultExt = "txt";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // AutomationButton
            // 
            this.AutomationButton.Enabled = false;
            this.AutomationButton.Location = new System.Drawing.Point(86, 73);
            this.AutomationButton.Name = "AutomationButton";
            this.AutomationButton.Size = new System.Drawing.Size(157, 23);
            this.AutomationButton.TabIndex = 7;
            this.AutomationButton.Text = "Open Automation Demo";
            this.AutomationButton.UseVisualStyleBackColor = true;
            this.AutomationButton.Click += new System.EventHandler(this.AutomationButton_Click);
            // 
            // autoFeedbackSource
            // 
            this.autoFeedbackSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoFeedbackSource.Enabled = false;
            this.autoFeedbackSource.FormattingEnabled = true;
            this.autoFeedbackSource.Items.AddRange(new object[] {
            "Unsyncronised, single-file",
            "Syncronised, single-file",
            "Unsyncronised, multi-file",
            "Syncronised, multi-file"});
            this.autoFeedbackSource.Location = new System.Drawing.Point(86, 46);
            this.autoFeedbackSource.Name = "autoFeedbackSource";
            this.autoFeedbackSource.Size = new System.Drawing.Size(157, 21);
            this.autoFeedbackSource.TabIndex = 13;
            // 
            // autoTorqueSource
            // 
            this.autoTorqueSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoTorqueSource.Enabled = false;
            this.autoTorqueSource.FormattingEnabled = true;
            this.autoTorqueSource.Items.AddRange(new object[] {
            "Unsyncronised, single-file",
            "Syncronised, single-file",
            "Unsyncronised, multi-file",
            "Syncronised, multi-file"});
            this.autoTorqueSource.Location = new System.Drawing.Point(86, 19);
            this.autoTorqueSource.Name = "autoTorqueSource";
            this.autoTorqueSource.Size = new System.Drawing.Size(157, 21);
            this.autoTorqueSource.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.FancyDemoButton);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.autoTorqueSource);
            this.groupBox3.Controls.Add(this.AutomationButton);
            this.groupBox3.Controls.Add(this.autoFeedbackSource);
            this.groupBox3.Location = new System.Drawing.Point(263, 300);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(249, 125);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Automation";
            // 
            // FancyDemoButton
            // 
            this.FancyDemoButton.Enabled = false;
            this.FancyDemoButton.Location = new System.Drawing.Point(86, 99);
            this.FancyDemoButton.Name = "FancyDemoButton";
            this.FancyDemoButton.Size = new System.Drawing.Size(157, 23);
            this.FancyDemoButton.TabIndex = 16;
            this.FancyDemoButton.Text = "Open Fancy Demo";
            this.FancyDemoButton.UseVisualStyleBackColor = true;
            this.FancyDemoButton.Click += new System.EventHandler(this.FancyDemoButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Feedback:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Torque:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 450);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.AddSourceButton);
            this.Controls.Add(this.SourcesPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Torque Sensor Hub";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimerPeriodSelect)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel SourcesPanel;
        private System.Windows.Forms.Button AddSourceButton;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown TimerPeriodSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox InternalTriggerActive;
        private System.Windows.Forms.Button BrowseFileButton;
        private System.Windows.Forms.TextBox FilePathBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox SaveTypeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SyncTriggerBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.CheckBox timestampCheckbox;
        private System.Windows.Forms.Button AutomationButton;
        private System.Windows.Forms.ComboBox autoFeedbackSource;
        private System.Windows.Forms.ComboBox autoTorqueSource;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button FancyDemoButton;
    }
}

