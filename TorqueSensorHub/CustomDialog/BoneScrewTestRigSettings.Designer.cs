
namespace TorqueSensorHub.CustomDialog
{
    partial class BoneScrewTestRigSettings
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
            this.serialSendBox = new System.Windows.Forms.TextBox();
            this.serialSendButton = new System.Windows.Forms.Button();
            this.statusText = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.settingValue = new System.Windows.Forms.NumericUpDown();
            this.resetAllButton = new System.Windows.Forms.Button();
            this.loadEepromButton = new System.Windows.Forms.Button();
            this.saveEepromButton = new System.Windows.Forms.Button();
            this.setSettingButton = new System.Windows.Forms.Button();
            this.settingSelector = new System.Windows.Forms.ComboBox();
            this.GetSettingButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.uiLock = new System.Windows.Forms.TrackBar();
            this.motorOffButton = new System.Windows.Forms.Button();
            this.motorOnButton = new System.Windows.Forms.Button();
            this.RestartButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.StopInsertButton = new System.Windows.Forms.Button();
            this.StartInsertButton = new System.Windows.Forms.Button();
            this.SelfcalButton = new System.Windows.Forms.Button();
            this.JogCWButton = new System.Windows.Forms.Button();
            this.JogCCWButton = new System.Windows.Forms.Button();
            this.jogAmountBox = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.LinZero = new System.Windows.Forms.TextBox();
            this.RotZero = new System.Windows.Forms.TextBox();
            this.zeroButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LinearDisplayBox = new System.Windows.Forms.TextBox();
            this.RotationDisplayBox = new System.Windows.Forms.TextBox();
            this.TorqueDisplayBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingValue)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.jogAmountBox)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialSendBox
            // 
            this.serialSendBox.Location = new System.Drawing.Point(6, 19);
            this.serialSendBox.Name = "serialSendBox";
            this.serialSendBox.Size = new System.Drawing.Size(350, 20);
            this.serialSendBox.TabIndex = 0;
            this.serialSendBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serialSendBox_KeyDown);
            // 
            // serialSendButton
            // 
            this.serialSendButton.Location = new System.Drawing.Point(362, 18);
            this.serialSendButton.Name = "serialSendButton";
            this.serialSendButton.Size = new System.Drawing.Size(76, 22);
            this.serialSendButton.TabIndex = 1;
            this.serialSendButton.Text = "Send";
            this.serialSendButton.UseVisualStyleBackColor = true;
            this.serialSendButton.Click += new System.EventHandler(this.serialSendButton_Click);
            // 
            // statusText
            // 
            this.statusText.Location = new System.Drawing.Point(6, 46);
            this.statusText.Name = "statusText";
            this.statusText.ReadOnly = true;
            this.statusText.Size = new System.Drawing.Size(432, 20);
            this.statusText.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.statusText);
            this.groupBox1.Controls.Add(this.serialSendBox);
            this.groupBox1.Controls.Add(this.serialSendButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(444, 73);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom serial commands";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.settingValue);
            this.groupBox2.Controls.Add(this.resetAllButton);
            this.groupBox2.Controls.Add(this.loadEepromButton);
            this.groupBox2.Controls.Add(this.saveEepromButton);
            this.groupBox2.Controls.Add(this.setSettingButton);
            this.groupBox2.Controls.Add(this.settingSelector);
            this.groupBox2.Controls.Add(this.GetSettingButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 72);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configure Settings";
            // 
            // settingValue
            // 
            this.settingValue.Location = new System.Drawing.Point(6, 46);
            this.settingValue.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.settingValue.Minimum = new decimal(new int[] {
            1410065408,
            2,
            0,
            -2147483648});
            this.settingValue.Name = "settingValue";
            this.settingValue.Size = new System.Drawing.Size(159, 20);
            this.settingValue.TabIndex = 3;
            // 
            // resetAllButton
            // 
            this.resetAllButton.Location = new System.Drawing.Point(377, 45);
            this.resetAllButton.Name = "resetAllButton";
            this.resetAllButton.Size = new System.Drawing.Size(63, 22);
            this.resetAllButton.TabIndex = 8;
            this.resetAllButton.Text = "Reset All";
            this.resetAllButton.UseVisualStyleBackColor = true;
            this.resetAllButton.Click += new System.EventHandler(this.resetAllButton_Click);
            // 
            // loadEepromButton
            // 
            this.loadEepromButton.Location = new System.Drawing.Point(248, 45);
            this.loadEepromButton.Name = "loadEepromButton";
            this.loadEepromButton.Size = new System.Drawing.Size(96, 22);
            this.loadEepromButton.TabIndex = 6;
            this.loadEepromButton.Text = "Load EEPROM";
            this.loadEepromButton.UseVisualStyleBackColor = true;
            this.loadEepromButton.Click += new System.EventHandler(this.loadEepromButton_Click);
            // 
            // saveEepromButton
            // 
            this.saveEepromButton.Location = new System.Drawing.Point(248, 18);
            this.saveEepromButton.Name = "saveEepromButton";
            this.saveEepromButton.Size = new System.Drawing.Size(96, 23);
            this.saveEepromButton.TabIndex = 5;
            this.saveEepromButton.Text = "Save EEPROM";
            this.saveEepromButton.UseVisualStyleBackColor = true;
            this.saveEepromButton.Click += new System.EventHandler(this.saveEepromButton_Click);
            // 
            // setSettingButton
            // 
            this.setSettingButton.Location = new System.Drawing.Point(171, 45);
            this.setSettingButton.Name = "setSettingButton";
            this.setSettingButton.Size = new System.Drawing.Size(49, 22);
            this.setSettingButton.TabIndex = 4;
            this.setSettingButton.Text = "Set";
            this.setSettingButton.UseVisualStyleBackColor = true;
            this.setSettingButton.Click += new System.EventHandler(this.setSettingButton_Click);
            // 
            // settingSelector
            // 
            this.settingSelector.FormattingEnabled = true;
            this.settingSelector.ItemHeight = 13;
            this.settingSelector.Location = new System.Drawing.Point(6, 19);
            this.settingSelector.Name = "settingSelector";
            this.settingSelector.Size = new System.Drawing.Size(159, 21);
            this.settingSelector.TabIndex = 3;
            this.settingSelector.SelectedIndexChanged += new System.EventHandler(this.settingSelector_SelectedIndexChanged);
            // 
            // GetSettingButton
            // 
            this.GetSettingButton.Location = new System.Drawing.Point(171, 18);
            this.GetSettingButton.Name = "GetSettingButton";
            this.GetSettingButton.Size = new System.Drawing.Size(49, 23);
            this.GetSettingButton.TabIndex = 1;
            this.GetSettingButton.Text = "Get";
            this.GetSettingButton.UseVisualStyleBackColor = true;
            this.GetSettingButton.Click += new System.EventHandler(this.GetSettingButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.uiLock);
            this.groupBox3.Controls.Add(this.motorOffButton);
            this.groupBox3.Controls.Add(this.motorOnButton);
            this.groupBox3.Controls.Add(this.RestartButton);
            this.groupBox3.Controls.Add(this.PauseButton);
            this.groupBox3.Controls.Add(this.StopInsertButton);
            this.groupBox3.Controls.Add(this.StartInsertButton);
            this.groupBox3.Location = new System.Drawing.Point(12, 169);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(444, 78);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Device Commands";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(189, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Lock UI:";
            // 
            // uiLock
            // 
            this.uiLock.AutoSize = false;
            this.uiLock.LargeChange = 0;
            this.uiLock.Location = new System.Drawing.Point(243, 19);
            this.uiLock.Maximum = 100;
            this.uiLock.Name = "uiLock";
            this.uiLock.Size = new System.Drawing.Size(195, 23);
            this.uiLock.SmallChange = 0;
            this.uiLock.TabIndex = 8;
            this.uiLock.TickStyle = System.Windows.Forms.TickStyle.None;
            this.uiLock.MouseUp += new System.Windows.Forms.MouseEventHandler(this.uiLock_MouseUp);
            // 
            // motorOffButton
            // 
            this.motorOffButton.Location = new System.Drawing.Point(310, 48);
            this.motorOffButton.Name = "motorOffButton";
            this.motorOffButton.Size = new System.Drawing.Size(61, 23);
            this.motorOffButton.TabIndex = 7;
            this.motorOffButton.Text = "Motor Off";
            this.motorOffButton.UseVisualStyleBackColor = true;
            this.motorOffButton.Click += new System.EventHandler(this.motorOffButton_Click);
            // 
            // motorOnButton
            // 
            this.motorOnButton.Location = new System.Drawing.Point(243, 48);
            this.motorOnButton.Name = "motorOnButton";
            this.motorOnButton.Size = new System.Drawing.Size(61, 23);
            this.motorOnButton.TabIndex = 6;
            this.motorOnButton.Text = "Motor On";
            this.motorOnButton.UseVisualStyleBackColor = true;
            this.motorOnButton.Click += new System.EventHandler(this.motorOnButton_Click);
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(377, 48);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(61, 23);
            this.RestartButton.TabIndex = 5;
            this.RestartButton.Text = "Restart";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(104, 19);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(61, 23);
            this.PauseButton.TabIndex = 3;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // StopInsertButton
            // 
            this.StopInsertButton.Location = new System.Drawing.Point(6, 48);
            this.StopInsertButton.Name = "StopInsertButton";
            this.StopInsertButton.Size = new System.Drawing.Size(92, 23);
            this.StopInsertButton.TabIndex = 2;
            this.StopInsertButton.Text = "Stop Insertion";
            this.StopInsertButton.UseVisualStyleBackColor = true;
            this.StopInsertButton.Click += new System.EventHandler(this.StopInsertButton_Click);
            // 
            // StartInsertButton
            // 
            this.StartInsertButton.Location = new System.Drawing.Point(6, 19);
            this.StartInsertButton.Name = "StartInsertButton";
            this.StartInsertButton.Size = new System.Drawing.Size(92, 23);
            this.StartInsertButton.TabIndex = 1;
            this.StartInsertButton.Text = "Start Insertion";
            this.StartInsertButton.UseVisualStyleBackColor = true;
            this.StartInsertButton.Click += new System.EventHandler(this.StartInsertButton_Click);
            // 
            // SelfcalButton
            // 
            this.SelfcalButton.Location = new System.Drawing.Point(229, 18);
            this.SelfcalButton.Name = "SelfcalButton";
            this.SelfcalButton.Size = new System.Drawing.Size(113, 22);
            this.SelfcalButton.TabIndex = 4;
            this.SelfcalButton.Text = "Self Cal";
            this.SelfcalButton.UseVisualStyleBackColor = true;
            this.SelfcalButton.Click += new System.EventHandler(this.SelfcalButton_Click);
            // 
            // JogCWButton
            // 
            this.JogCWButton.Location = new System.Drawing.Point(340, 17);
            this.JogCWButton.Name = "JogCWButton";
            this.JogCWButton.Size = new System.Drawing.Size(46, 23);
            this.JogCWButton.TabIndex = 4;
            this.JogCWButton.Text = "CW";
            this.JogCWButton.UseVisualStyleBackColor = true;
            this.JogCWButton.Click += new System.EventHandler(this.JogCWButton_Click);
            // 
            // JogCCWButton
            // 
            this.JogCCWButton.Location = new System.Drawing.Point(392, 17);
            this.JogCCWButton.Name = "JogCCWButton";
            this.JogCCWButton.Size = new System.Drawing.Size(46, 23);
            this.JogCCWButton.TabIndex = 5;
            this.JogCCWButton.Text = "CCW";
            this.JogCCWButton.UseVisualStyleBackColor = true;
            this.JogCCWButton.Click += new System.EventHandler(this.JogCCWButton_Click);
            // 
            // jogAmountBox
            // 
            this.jogAmountBox.Location = new System.Drawing.Point(81, 19);
            this.jogAmountBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.jogAmountBox.Name = "jogAmountBox";
            this.jogAmountBox.Size = new System.Drawing.Size(73, 20);
            this.jogAmountBox.TabIndex = 9;
            this.jogAmountBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.jogAmountBox);
            this.groupBox4.Controls.Add(this.JogCWButton);
            this.groupBox4.Controls.Add(this.JogCCWButton);
            this.groupBox4.Location = new System.Drawing.Point(12, 253);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(444, 46);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Jog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Multiplier:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.LinZero);
            this.groupBox5.Controls.Add(this.RotZero);
            this.groupBox5.Controls.Add(this.zeroButton);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.SelfcalButton);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.LinearDisplayBox);
            this.groupBox5.Controls.Add(this.RotationDisplayBox);
            this.groupBox5.Controls.Add(this.TorqueDisplayBox);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(12, 305);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(444, 101);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Current values";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(348, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Zero points:";
            // 
            // LinZero
            // 
            this.LinZero.Location = new System.Drawing.Point(348, 71);
            this.LinZero.Name = "LinZero";
            this.LinZero.Size = new System.Drawing.Size(90, 20);
            this.LinZero.TabIndex = 20;
            this.LinZero.Text = "0.000";
            // 
            // RotZero
            // 
            this.RotZero.Location = new System.Drawing.Point(348, 45);
            this.RotZero.Name = "RotZero";
            this.RotZero.Size = new System.Drawing.Size(90, 20);
            this.RotZero.TabIndex = 19;
            this.RotZero.Text = "0.00";
            // 
            // zeroButton
            // 
            this.zeroButton.Location = new System.Drawing.Point(229, 44);
            this.zeroButton.Name = "zeroButton";
            this.zeroButton.Size = new System.Drawing.Size(113, 48);
            this.zeroButton.TabIndex = 18;
            this.zeroButton.Text = "Zero Encoders";
            this.zeroButton.UseVisualStyleBackColor = true;
            this.zeroButton.Click += new System.EventHandler(this.zeroRotButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "mm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Degrees";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Nm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Linear:";
            // 
            // LinearDisplayBox
            // 
            this.LinearDisplayBox.Location = new System.Drawing.Point(95, 71);
            this.LinearDisplayBox.Name = "LinearDisplayBox";
            this.LinearDisplayBox.ReadOnly = true;
            this.LinearDisplayBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LinearDisplayBox.Size = new System.Drawing.Size(70, 20);
            this.LinearDisplayBox.TabIndex = 13;
            this.LinearDisplayBox.Text = "0.000";
            this.LinearDisplayBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RotationDisplayBox
            // 
            this.RotationDisplayBox.Location = new System.Drawing.Point(95, 45);
            this.RotationDisplayBox.Name = "RotationDisplayBox";
            this.RotationDisplayBox.ReadOnly = true;
            this.RotationDisplayBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RotationDisplayBox.Size = new System.Drawing.Size(70, 20);
            this.RotationDisplayBox.TabIndex = 12;
            this.RotationDisplayBox.Text = "0.00";
            this.RotationDisplayBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // TorqueDisplayBox
            // 
            this.TorqueDisplayBox.Location = new System.Drawing.Point(95, 19);
            this.TorqueDisplayBox.Name = "TorqueDisplayBox";
            this.TorqueDisplayBox.ReadOnly = true;
            this.TorqueDisplayBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TorqueDisplayBox.Size = new System.Drawing.Size(70, 20);
            this.TorqueDisplayBox.TabIndex = 3;
            this.TorqueDisplayBox.Text = "0.000";
            this.TorqueDisplayBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Rotation:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Torque:";
            // 
            // JacksTestRigSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 415);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "JacksTestRigSettings";
            this.Text = "Bone Screw Test Rig Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JacksTestRigSettings_FormClosing);
            this.Load += new System.EventHandler(this.JacksTestRigSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingValue)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.jogAmountBox)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox serialSendBox;
        private System.Windows.Forms.Button serialSendButton;
        private System.Windows.Forms.TextBox statusText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown settingValue;
        private System.Windows.Forms.Button resetAllButton;
        private System.Windows.Forms.Button loadEepromButton;
        private System.Windows.Forms.Button saveEepromButton;
        private System.Windows.Forms.Button setSettingButton;
        private System.Windows.Forms.ComboBox settingSelector;
        private System.Windows.Forms.Button GetSettingButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button StopInsertButton;
        private System.Windows.Forms.Button StartInsertButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.Button SelfcalButton;
        private System.Windows.Forms.Button JogCWButton;
        private System.Windows.Forms.Button JogCCWButton;
        private System.Windows.Forms.NumericUpDown jogAmountBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox LinearDisplayBox;
        private System.Windows.Forms.TextBox RotationDisplayBox;
        private System.Windows.Forms.TextBox TorqueDisplayBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button zeroButton;
        private System.Windows.Forms.Button motorOffButton;
        private System.Windows.Forms.Button motorOnButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar uiLock;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox LinZero;
        private System.Windows.Forms.TextBox RotZero;
    }
}