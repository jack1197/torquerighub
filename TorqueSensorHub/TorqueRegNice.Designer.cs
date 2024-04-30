
namespace TorqueSensorHub
{
    partial class TorqueRegNice
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.autoStopBox = new System.Windows.Forms.CheckBox();
            this.ScrewListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MaterialListBox = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(124, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start Insertion";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Stop Insertion";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // autoStopBox
            // 
            this.autoStopBox.AutoSize = true;
            this.autoStopBox.Location = new System.Drawing.Point(236, 16);
            this.autoStopBox.Name = "autoStopBox";
            this.autoStopBox.Size = new System.Drawing.Size(73, 17);
            this.autoStopBox.TabIndex = 4;
            this.autoStopBox.Text = "Auto Stop";
            this.autoStopBox.UseVisualStyleBackColor = true;
            // 
            // ScrewListBox
            // 
            this.ScrewListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScrewListBox.FormattingEnabled = true;
            this.ScrewListBox.Location = new System.Drawing.Point(361, 13);
            this.ScrewListBox.Name = "ScrewListBox";
            this.ScrewListBox.Size = new System.Drawing.Size(157, 21);
            this.ScrewListBox.TabIndex = 15;
            this.ScrewListBox.SelectedIndexChanged += new System.EventHandler(this.ScrewListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(315, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Screw:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Reference material:";
            // 
            // MaterialListBox
            // 
            this.MaterialListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaterialListBox.FormattingEnabled = true;
            this.MaterialListBox.Location = new System.Drawing.Point(638, 12);
            this.MaterialListBox.Name = "MaterialListBox";
            this.MaterialListBox.Size = new System.Drawing.Size(157, 21);
            this.MaterialListBox.TabIndex = 18;
            this.MaterialListBox.SelectedIndexChanged += new System.EventHandler(this.MaterialListBox_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(801, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Fullscreen";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::TorqueSensorHub.Dial.Ref;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1582, 889);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // TorqueRegNice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 889);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.MaterialListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ScrewListBox);
            this.Controls.Add(this.autoStopBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Name = "TorqueRegNice";
            this.Text = "TorqueReg";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TorqueReg_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox autoStopBox;
        private System.Windows.Forms.ComboBox ScrewListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MaterialListBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}