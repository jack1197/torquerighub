using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TorqueSensorHub.Drivers;

namespace TorqueSensorHub.CustomDialog
{
    public partial class BoneScrewTestRigSettings : Form
    {
        BoneScrewTestRigDevice device;

        Dictionary<string, string> settingList = new Dictionary<string, string> {
            {"Motor Enabled", "ena" },
            {"Insert Distance", "ind" },
            {"Reverse Distance", "rvd" },
            {"Jog Distance", "jgd" },
            {"Full Insertion Pause", "pau" },
            {"Trapezoidal Enabled", "trp" },
            {"Insert Speed", "ins" },
            {"Ramp Time(Trapez.)", "rmp" },
            {"High Dwell Time(Trapez.)", "hdt" },
            {"Low Dwell Time(Trapez.)", "ldt" },
            {"Distance Per Trapezoid(Trapez.)", "dpt" },
            {"Torque Limit", "tlm" },
            {"Reverse Speed (Override)", "rvs" },
            {"Invert Rotation", "inv" },
        };
        string currentSetting = "ena";

        public BoneScrewTestRigSettings(BoneScrewTestRigDevice device)
        {
            this.device = device;
            InitializeComponent();
        }

        private void Device_StatusMessageRecieved(object sender, string e)
        {
            try
            {
                BeginInvoke((MethodInvoker)delegate 
                {
                    statusText.Text = e;
                    if (e.StartsWith("get: ") || e.StartsWith("set: "))
                    {
                        var parts = e.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if(parts.Count() == 3 && parts[1] == currentSetting)
                        {
                            settingValue.Value = int.Parse(parts[2]);
                        }
                    }
                });
            }
            catch { }
        }

        private void serialSendButton_Click(object sender, EventArgs e)
        {
            device.SendCustomCommand(serialSendBox.Text);
        }


        private void serialSendBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                serialSendButton.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }


        private void JacksTestRigSettings_Load(object sender, EventArgs e)
        {
            device.StatusMessageRecieved += Device_StatusMessageRecieved;
            device.OnDataPoint += Device_OnDataPoint;

            settingSelector.Items.AddRange(settingList.Keys.ToArray());
            settingSelector.SelectedIndex = 0;

            device.Disconnected += Device_Disconnected;
        }

        private void Device_Disconnected(object sender, EventArgs e)
        {
            try
            {
                BeginInvoke((MethodInvoker)delegate { this.Close(); });
            } catch{ }
        }

        private void Device_OnDataPoint(object sender, List<KeyValuePair<DataPointType, decimal>> e)
        {
            BeginInvoke((MethodInvoker)delegate {
                foreach (var point in e)
                {

                    switch (point.Key)
                    {
                        case DataPointType.Unknown:
                            break;
                        case DataPointType.LinearDisplacment:
                            LinearDisplayBox.Text = $"{point.Value-linOffset:F3}";
                            break;
                        case DataPointType.LinearVelocity:
                            break;
                        case DataPointType.AngularDisplacment:
                            RotationDisplayBox.Text = $"{point.Value-rotOffset:F2}";
                            break;
                        case DataPointType.AngularVelocity:
                            break;
                        case DataPointType.Torque:
                            TorqueDisplayBox.Text = $"{point.Value:F3}";
                            break;
                        case DataPointType.AxialForce:
                            break;
                        default:
                            break;
                    }
                }
            });
            
        }

        private void JacksTestRigSettings_FormClosing(object sender, FormClosingEventArgs e)
        {

            device.StatusMessageRecieved -= Device_StatusMessageRecieved;
            device.OnDataPoint -= Device_OnDataPoint;
        }

        private void settingSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSetting = settingList[(string)settingSelector.SelectedItem];
            GetSettingButton.PerformClick();
        }

        private void GetSettingButton_Click(object sender, EventArgs e)
        {
            device.SendCustomCommand($"get {currentSetting}");
        }

        private void setSettingButton_Click(object sender, EventArgs e)
        {
            device.SendCustomCommand($"set {currentSetting} {settingValue.Value}");
        }

        private void saveEepromButton_Click(object sender, EventArgs e)
        {
            device.SendCustomCommand($"save_eeprom");

        }

        private void loadEepromButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"load_eeprom");
        }

        private void resetAllButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"reset_eeprom");
            device.SendCustomCommand($"load_eeprom");
        }

        private void StartInsertButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"start");
        }

        private void StopInsertButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"stop");
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"pause");
        }

        private void SelfcalButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"selfcal");
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"sysrst");
            Thread.Sleep(1000);
            device.SendCustomCommand($"rawmode 1");
        }

        private void JogCWButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"jog cw {jogAmountBox.Value}");
        }

        private void JogCCWButton_Click(object sender, EventArgs e)
        {
            device.SendCustomCommand($"jog ccw {jogAmountBox.Value}");

        }

        decimal linOffset = 0;
        decimal rotOffset = 0;


        private void zeroRotButton_Click(object sender, EventArgs e)
        {
            float linOffset = 0;
            float rotOffset = 0;
            float.TryParse(LinZero.Text, out linOffset);
            float.TryParse(RotZero.Text, out rotOffset);
            device.SendCustomCommand($"zero lin {Math.Round(linOffset * 40)}");
            Thread.Sleep(20);
            device.SendCustomCommand($"zero rot {Math.Round(rotOffset * 4)}");
        }

        private void motorOnButton_Click(object sender, EventArgs e)
        {

            device.SendCustomCommand($"set ena 1");
        }

        private void motorOffButton_Click(object sender, EventArgs e)
        {
            device.SendCustomCommand($"set ena 0");

        }

        bool UILocked = false;

        private void uiLock_MouseUp(object sender, MouseEventArgs e)
        {
            List<Control> keepEnabled = new List<Control> { StartInsertButton, statusText, TorqueDisplayBox, RotationDisplayBox, LinearDisplayBox, uiLock };
            if (UILocked)
            {
                if (uiLock.Value == uiLock.Minimum)
                {
                    UILocked = false;
                    this.ForAllControls((c) => { if (!(keepEnabled.Contains(c) || c is GroupBox || c is Label)) c.Enabled = true; ; });
                }
                else
                {
                    uiLock.Value = uiLock.Maximum;
                }
            }
            else
            {
                if (uiLock.Value == uiLock.Maximum)
                {
                    UILocked = true;
                    this.ForAllControls((c) => { if (!(keepEnabled.Contains(c) || c is GroupBox || c is Label)) c.Enabled = false; ; });
                }
                else
                {
                    uiLock.Value = uiLock.Minimum;
                }
            }
        }
    }
}
