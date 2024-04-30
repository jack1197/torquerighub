using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TorqueSensorHub.Drivers;

namespace TorqueSensorHub
{
    public partial class InputDeviceDialog : UserControl
    {
        List<Tuple<string, ITriggerSource>> TriggerSources;
        List<Tuple<string, IDevice>> TorqueSources;
        List<Tuple<string, IFeedbackDevice>> FeedbackSources;

        public InputDeviceDialog(List<Tuple<string, ITriggerSource>> TriggerSources, List<Tuple<string, IDevice>> TorqueSources, List<Tuple<string, IFeedbackDevice>> FeedbackSources)
        {
            InitializeComponent();
            this.TriggerSources = TriggerSources;
            this.TorqueSources = TorqueSources;
            this.FeedbackSources = FeedbackSources;
        }

        public IDevice device;
        List<Type> DeviceTypeList;
        public event EventHandler SourceListsUpdateNotifySend;
        public Form1 parent;
        bool connected = false;
        static int counter = 0;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void InputDeviceDialog_Load(object sender, EventArgs e)
        {
            SerialPort.GetPortNames();
            Disposed += InputDeviceDialog_Disposed;
            var DeviceTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                              where t.IsClass && t.Namespace == "TorqueSensorHub.Drivers"
                              select t;
            DeviceTypeList = DeviceTypes.Where(type => type.CustomAttributes.Count(attr => attr.AttributeType == typeof(LoggingDeviceAttribute)) > 0).ToList();
            var DeviceNames = DeviceTypeList.Select(type => type.GetCustomAttribute<LoggingDeviceAttribute>().Name);
            TypeList.Items.AddRange(DeviceNames.ToArray());
            NameBox.Text = "Device " + counter++;
        }

        private void InputDeviceDialog_Disposed(object sender, EventArgs e)
        {
            if (connected) device.Disconnect();
            if (device?.GetFormat().Contains(DataPointType.Torque) ?? false)
            {
                TorqueSources.Remove(TorqueSources.First(i => i.Item2 == device));
                SourceListsUpdateNotifySend.Invoke(this, null);
            }
            if (device != null && device is IFeedbackDevice)
            {
                FeedbackSources.Remove(FeedbackSources.First(i => i.Item2 == device));
                SourceListsUpdateNotifySend.Invoke(this, null);
            }
            device?.DeselectedType();
            device = null;
            try
            {
                parent.SourcesListsUpdated -= Parent_TriggerListUpdated;
            }
            catch
            {

            }
            parent = null;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void TypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (device != null)
            {
                if (device.GetFormat().Contains(DataPointType.Torque))
                {
                    TorqueSources.Remove(TorqueSources.First(i => i.Item2 == device));
                    SourceListsUpdateNotifySend.Invoke(this, null);
                }
                if (device is IFeedbackDevice)
                {
                    FeedbackSources.Remove(FeedbackSources.First(i => i.Item2 == device));
                    SourceListsUpdateNotifySend.Invoke(this, null);
                }
                device.Disconnect();
                device.DeselectedType();
                BaudList.Items.Clear();
                PortList.Items.Clear();
                TriggerList.Items.Clear();
                ModeList.Items.Clear();
            }
            var deviceType = DeviceTypeList.Where(type => type.GetCustomAttribute<LoggingDeviceAttribute>().Name == (TypeList.SelectedItem as string)).First();
            device = Activator.CreateInstance(deviceType) as IDevice;

            device.EnableButton += Device_EnableButton;
            device.DisableButton += Device_DisableButton;
            device.EnableModeSelect += Device_EnableModeSelect;
            device.DisableModeSelect += Device_DisableModeSelect;
            device.EnablePortSelect += Device_EnablePortSelect;
            device.DisablePortSelect += Device_DisablePortSelect;
            device.EnableBaudSelect += Device_EnableBaudSelect;
            device.DisableBaudSelect += Device_DisableBaudSelect;
            device.EnableTriggerSelect += Device_EnableTriggerSelect;
            device.DisableTriggerSelect += Device_DisableTriggerSelect;
            device.EnableTriggerUse += Device_EnableTriggerUse;
            device.DisableTriggerUse += Device_DisableTriggerUse;
            device.EnableTypeSelect += Device_EnableTypeSelect; 
            device.DisableTypeSelect += Device_DisableTypeSelect;
            device.Connected += Device_Connected;
            device.Disconnected += Device_Disconnected;
            device.SetStatus += Device_SetStatus;


            device.SelectedType();
            BaudList.Items.AddRange(device.GetBauds().Select(i => i.ToString()).ToArray());
            if (BaudList.Items.Contains(device.DefaultBaud().ToString())) BaudList.SelectedIndex = BaudList.Items.IndexOf(device.DefaultBaud().ToString());
            else if (BaudList.Items.Count > 0) BaudList.SelectedIndex = 0;
            PortList.Items.AddRange(device.GetPorts().ToArray());
            if(PortList.Items.Count > 0) PortList.SelectedIndex = 0;
            ModeList.Items.AddRange(device.GetModes().ToArray());
            if (ModeList.Items.Count > 0) ModeList.SelectedIndex = 0;

            if(device is IIPDevice)
            {
                IPAddress.Show();
                PortList.Hide();
                RefreshButton.Hide();
                portLabel.Text = "Address:";
                IPAddress.Text = device.GetPort();
            }
            else
            {
                IPAddress.Hide();
                PortList.Show();
                RefreshButton.Show();
                portLabel.Text = "Port:";
            }

            if (device.GetFormat().Contains(DataPointType.Torque))
            {
                var similar = TorqueSources.FindAll(
                    t => t.Item1.StartsWith(
                        (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name
                        )
                    );
                

                if (similar.Count() == 0)
                {
                    string name = (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name + " 1";
                    TorqueSources.Add(new Tuple<string, IDevice>(name, device));
                }
                else
                {
                    int i = 2;
                    do
                    {
                        string name = (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name + " " + i.ToString();
                        if (!similar.Exists(t => t.Item1 == name))
                        {
                            TorqueSources.Add(new Tuple<string, IDevice>(name, device));
                            break;
                        }
                    }
                    while (true);
                }
                SourceListsUpdateNotifySend.Invoke(this, null);
            }


            if (device is IFeedbackDevice)
            {
                var similar = FeedbackSources.FindAll(
                    t => t.Item1.StartsWith(
                        (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name
                        )
                    );
                //IDK what im doing here really, just hoping it works by magic. ~Later comment: not magic

                if (similar.Count() == 0)
                {
                    string name = (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name + " 1";
                    FeedbackSources.Add(new Tuple<string, IFeedbackDevice>(name, device as IFeedbackDevice));
                }
                else
                {
                    int i = 2;
                    do
                    {
                        string name = (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name + " " + i.ToString();
                        if (!similar.Exists(t => t.Item1 == name))
                        {
                            FeedbackSources.Add(new Tuple<string, IFeedbackDevice>(name, device as IFeedbackDevice));
                            break;
                        }
                    }
                    while (true);
                }
                SourceListsUpdateNotifySend.Invoke(this, null);
            }


        }

        private void Device_SetStatus(object sender, string e)
        {
            try
            {
                BeginInvoke((MethodInvoker)delegate { StatusLabel.Text = "Status: " + e; });
            } catch { }
            
        }

        private void Device_Disconnected(object sender, EventArgs e)
        {
            connected = false;
            ConnectButton.Text = "Connect";
            DiagnosticsButton.Enabled = false;
            customDialog.Enabled = false;
        }

        private void Device_Connected(object sender, EventArgs e)
        {
            connected = true;
            ConnectButton.Text = "Disconnect";
            DiagnosticsButton.Enabled = true;
            customDialog.Enabled = device.SupportCustomDialog();
        }

        private void Device_DisableBaudSelect(object sender, EventArgs e)
        {
            BaudList.Enabled = false;
        }

        private void Device_EnableBaudSelect(object sender, EventArgs e)
        {
            BaudList.Enabled = true;
        }

        private void Device_DisableTypeSelect(object sender, EventArgs e)
        {
            TypeList.Enabled = false;
        }

        private void Device_EnableTypeSelect(object sender, EventArgs e)
        {
            TypeList.Enabled = true;
        }

        private void Device_DisableTriggerUse(object sender, EventArgs e)
        {
            TriggerSources.RemoveAll(t => t.Item1 == (TriggerList.SelectedItem as string));
            TriggerList.Items.Clear();
            SourceListsUpdateNotifySend.Invoke(this, null);
        }

        private void Device_EnableTriggerUse(object sender, EventArgs e)
        {
            var similar = TriggerSources.FindAll(
                t => t.Item1.StartsWith(
                    (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name
                    )
                );
            //IDK what im doing here really, just hoping it works by magic
            
            if (similar.Count() == 0)
            {
                string name = (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name + " 1";
                TriggerSources.Add(new Tuple<string, ITriggerSource>(name,device));
                TriggerList.Items.Add(name);
                TriggerList.SelectedItem = name;
            }
            else
            {
                int i = 2;
                do
                {
                    string name = (device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute)) as LoggingDeviceAttribute).Name + " " + i.ToString();
                    if(! similar.Exists(t => t.Item1 == name))
                    {
                        TriggerSources.Add(new Tuple<string, ITriggerSource>(name, device));
                        TriggerList.Items.Add(name);
                        TriggerList.SelectedItem = name;
                        break;
                    }
                }
                while (true);
            }
            SourceListsUpdateNotifySend.Invoke(this, null);
        }

        private void Device_DisableTriggerSelect(object sender, EventArgs e)
        {
            TriggerList.Enabled = false;
            TriggerList.Items.Clear();
            parent.SourcesListsUpdated -= Parent_TriggerListUpdated;

        }

        private void Device_EnableTriggerSelect(object sender, EventArgs e)
        {
            TriggerList.Enabled = true;
            TriggerList.Items.AddRange(TriggerSources.Select(t => t.Item1).ToArray());
            TriggerList.SelectedIndex = 0;
            parent.SourcesListsUpdated += Parent_TriggerListUpdated;
        }


        private void Parent_TriggerListUpdated(object sender, EventArgs e)
        {
            var current = TriggerList.SelectedItem ?? "";
            TriggerList.Items.Clear();
            TriggerList.Items.AddRange(TriggerSources.Select(t => t.Item1).ToArray());
            if(TriggerList.Items.Contains(current))
            {
                TriggerList.SelectedItem = current;
            }
            else
            {
                if(TriggerList.Items.Count > 0)TriggerList.SelectedIndex = 0;
            }
        }

        private void Device_DisablePortSelect(object sender, EventArgs e)
        {
            PortList.Enabled = false;
            RefreshButton.Enabled = false;
            IPAddress.Enabled = false;
        }

        private void Device_EnablePortSelect(object sender, EventArgs e)
        {
            PortList.Enabled = true;
            RefreshButton.Enabled = true;
            IPAddress.Enabled = true;
        }

        private void Device_DisableModeSelect(object sender, EventArgs e)
        {
            ModeList.Enabled = false;
        }

        private void Device_EnableModeSelect(object sender, EventArgs e)
        {
            ModeList.Enabled = true;
        }

        private void Device_DisableButton(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
        }

        private void Device_EnableButton(object sender, EventArgs e)
        {
            ConnectButton.Enabled = true;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            PortList.Items.Clear();
            PortList.Items.AddRange(device.GetPorts().ToArray());
            if (PortList.Items.Count > 0) PortList.SelectedIndex = 0;
        }

        private void PortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            device.SetPort(PortList.SelectedItem as string);
        }

        private void ModeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            device.SetMode(ModeList.SelectedItem as string);
        }

        private void BaudList_SelectedIndexChanged(object sender, EventArgs e)
        {
            device.SetBaud(int.TryParse(BaudList.SelectedItem as string, out var baud) ? baud : default);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if(!connected)
            {
                device.Connect();
            }
            else
            {
                device.Disconnect();
            }
        }

        private void DiagnosticsButton_Click(object sender, EventArgs e)
        {
            Diagnostics diag = new Diagnostics(device);
            diag.Show(this);
        }


        private void TriggerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(TriggerList.Enabled == false)
            {
                return;
            }
            var source = TriggerSources.First(ts => ts.Item1 == TriggerList.Text);

            device.SetTrigger(source.Item2);
        }

        private void customDialog_Click(object sender, EventArgs e)
        {
            device.OpenCustomDialog(this);
        }

        private void IPAddress_TextChanged(object sender, EventArgs e)
        {
            device.SetPort(IPAddress.Text);
        }
    }
}
