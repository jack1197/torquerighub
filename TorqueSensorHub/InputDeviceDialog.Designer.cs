
namespace TorqueSensorHub
{
    partial class InputDeviceDialog
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
            this.TypeList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PortList = new System.Windows.Forms.ComboBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ModeList = new System.Windows.Forms.ComboBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.TriggerList = new System.Windows.Forms.ComboBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.BaudList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.DiagnosticsButton = new System.Windows.Forms.Button();
            this.customDialog = new System.Windows.Forms.Button();
            this.IPAddress = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TypeList
            // 
            this.TypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeList.FormattingEnabled = true;
            this.TypeList.Location = new System.Drawing.Point(73, 29);
            this.TypeList.Name = "TypeList";
            this.TypeList.Size = new System.Drawing.Size(142, 21);
            this.TypeList.TabIndex = 0;
            this.TypeList.SelectedIndexChanged += new System.EventHandler(this.TypeList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // PortList
            // 
            this.PortList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PortList.Enabled = false;
            this.PortList.FormattingEnabled = true;
            this.PortList.Location = new System.Drawing.Point(73, 56);
            this.PortList.Name = "PortList";
            this.PortList.Size = new System.Drawing.Size(70, 21);
            this.PortList.TabIndex = 2;
            this.PortList.SelectedIndexChanged += new System.EventHandler(this.PortList_SelectedIndexChanged);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(3, 60);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 3;
            this.portLabel.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Mode:";
            // 
            // ModeList
            // 
            this.ModeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeList.Enabled = false;
            this.ModeList.FormattingEnabled = true;
            this.ModeList.Location = new System.Drawing.Point(73, 110);
            this.ModeList.Name = "ModeList";
            this.ModeList.Size = new System.Drawing.Size(142, 21);
            this.ModeList.TabIndex = 5;
            this.ModeList.SelectedIndexChanged += new System.EventHandler(this.ModeList_SelectedIndexChanged);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(163, 56);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(52, 21);
            this.RefreshButton.TabIndex = 6;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Enabled = false;
            this.ConnectButton.Location = new System.Drawing.Point(127, 215);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(87, 23);
            this.ConnectButton.TabIndex = 7;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Trigger:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // TriggerList
            // 
            this.TriggerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TriggerList.Enabled = false;
            this.TriggerList.FormattingEnabled = true;
            this.TriggerList.Location = new System.Drawing.Point(73, 137);
            this.TriggerList.Name = "TriggerList";
            this.TriggerList.Size = new System.Drawing.Size(142, 21);
            this.TriggerList.TabIndex = 9;
            this.TriggerList.SelectedIndexChanged += new System.EventHandler(this.TriggerList_SelectedIndexChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(1, 215);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(68, 23);
            this.DeleteButton.TabIndex = 10;
            this.DeleteButton.Text = "Remove";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(0, 199);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(109, 13);
            this.StatusLabel.TabIndex = 11;
            this.StatusLabel.Text = "Status: Disconnected";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BaudList
            // 
            this.BaudList.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.BaudList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.BaudList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BaudList.Enabled = false;
            this.BaudList.FormattingEnabled = true;
            this.BaudList.Location = new System.Drawing.Point(73, 83);
            this.BaudList.Name = "BaudList";
            this.BaudList.Size = new System.Drawing.Size(142, 21);
            this.BaudList.TabIndex = 12;
            this.BaudList.SelectedIndexChanged += new System.EventHandler(this.BaudList_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Baud:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Name/Id:";
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(73, 3);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(142, 20);
            this.NameBox.TabIndex = 15;
            // 
            // DiagnosticsButton
            // 
            this.DiagnosticsButton.Enabled = false;
            this.DiagnosticsButton.Location = new System.Drawing.Point(75, 215);
            this.DiagnosticsButton.Name = "DiagnosticsButton";
            this.DiagnosticsButton.Size = new System.Drawing.Size(46, 23);
            this.DiagnosticsButton.TabIndex = 16;
            this.DiagnosticsButton.Text = "Diag";
            this.DiagnosticsButton.UseVisualStyleBackColor = true;
            this.DiagnosticsButton.Click += new System.EventHandler(this.DiagnosticsButton_Click);
            // 
            // customDialog
            // 
            this.customDialog.Enabled = false;
            this.customDialog.Location = new System.Drawing.Point(73, 164);
            this.customDialog.Name = "customDialog";
            this.customDialog.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.customDialog.Size = new System.Drawing.Size(141, 24);
            this.customDialog.TabIndex = 19;
            this.customDialog.Text = "Open Custom Settings";
            this.customDialog.UseVisualStyleBackColor = true;
            this.customDialog.Click += new System.EventHandler(this.customDialog_Click);
            // 
            // IPAddress
            // 
            this.IPAddress.Location = new System.Drawing.Point(73, 57);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Size = new System.Drawing.Size(142, 20);
            this.IPAddress.TabIndex = 20;
            this.IPAddress.Visible = false;
            this.IPAddress.TextChanged += new System.EventHandler(this.IPAddress_TextChanged);
            // 
            // InputDeviceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.IPAddress);
            this.Controls.Add(this.customDialog);
            this.Controls.Add(this.DiagnosticsButton);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BaudList);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.TriggerList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.ModeList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.PortList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TypeList);
            this.Name = "InputDeviceDialog";
            this.Size = new System.Drawing.Size(218, 242);
            this.Load += new System.EventHandler(this.InputDeviceDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox TypeList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox PortList;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ModeList;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox TriggerList;
        public System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox BaudList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button DiagnosticsButton;
        public System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Button customDialog;
        public System.Windows.Forms.TextBox IPAddress;
    }
}
