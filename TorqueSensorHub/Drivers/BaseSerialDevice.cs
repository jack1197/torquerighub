using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorqueSensorHub.Drivers
{
    public abstract class BaseSerialDevice : IDevice
    {
        public event EventHandler EnableTypeSelect = delegate { };
        public event EventHandler DisableTypeSelect = delegate { };
        public event EventHandler EnablePortSelect = delegate { };
        public event EventHandler DisablePortSelect = delegate { };
        public event EventHandler EnableBaudSelect = delegate { };
        public event EventHandler DisableBaudSelect = delegate { };
        public event EventHandler EnableModeSelect = delegate { };
        public event EventHandler DisableModeSelect = delegate { };
        public event EventHandler EnableTriggerSelect = delegate { };
        public event EventHandler DisableTriggerSelect = delegate { };
        public event EventHandler EnableButton = delegate { };
        public event EventHandler DisableButton = delegate { };
        public event EventHandler<string> SetStatus = delegate { };
        public event EventHandler EnableTriggerUse = delegate { };
        public event EventHandler DisableTriggerUse = delegate { };
        public event EventHandler Trigger = delegate { };
        public event EventHandler Connected = delegate { };
        public event EventHandler Disconnected = delegate { };
        public event EventHandler<List<KeyValuePair<DataPointType, decimal>>> OnDataPoint = delegate { };

        protected event EventHandler ModeChanged = delegate{};

        protected virtual bool PollSupported { get { return false; } }
        protected virtual bool InterruptSupported { get { return false; } }

        protected enum ModeType
        {
            Unconfigured,
            Poll,
            Interrupt
        }

        protected ModeType Mode = ModeType.Unconfigured;
        protected SerialPort Port = new SerialPort();
        bool portSet = false;
        bool baudSet = false;

        protected void SendDataPoint(List<KeyValuePair<DataPointType, decimal>> dataPoint)
        {
            OnDataPoint.Invoke(this, dataPoint);
        }

        public void Connect()
        {
            remainder = "";
            try
            {
                if (!Port.IsOpen)
                {
                    Port.Open();
                }
                Connected.Invoke(this, null);
                DisablePortSelect.Invoke(this, null);
                DisableBaudSelect.Invoke(this, null);
                DisableModeSelect.Invoke(this, null);
                DisableTypeSelect.Invoke(this, null);
                SetStatus.Invoke(this, "Connected");
            }
            catch (System.IO.IOException e) { }
            catch (UnauthorizedAccessException e) 
            { SetStatus.Invoke(this, "Access error, port in use?"); }
            SetupDevice();
            Port.DataReceived += Port_DataReceived;


        }

        protected virtual void SetupDevice()
        {

        }

        private string remainder = "";
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(!Port.IsOpen)
            {
                return;
            }
            string recv = Port.ReadExisting();
            string total = remainder + recv;
            string[] lines = Regex.Split(total, @"\r\n");
            remainder = lines[lines.Length - 1];
            string[] fullLines = lines.Take(lines.Length - 1).ToArray();
            foreach(var line in fullLines)
            {
                if(line != "") ProcessLine(line);
            }
        }

        protected abstract void ProcessLine(string line);

        public void Disconnect()
        {
            StopDevice();
            if(Port.IsOpen)
            {
                Port.Close();
            }
            Port.DataReceived -= Port_DataReceived;
            Disconnected.Invoke(this, null);
            EnablePortSelect.Invoke(this, null);
            EnableBaudSelect.Invoke(this, null);
            EnableTypeSelect.Invoke(this, null);
            if (GetModes().Count > 1) EnableModeSelect.Invoke(this, null);
            SetStatus.Invoke(this, "Disconnected");
        }
        protected virtual void StopDevice()
        {

        }

        public List<string> GetModes()
        {
            List<string> modes = new List<string>();
            if (PollSupported)
            {
                modes.Add("Poll");
            }
            if (InterruptSupported)
            {
                modes.Add("Interrupt");
            }
            return modes;
        }

        public List<string> GetPorts()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public void SetMode(string ModeString)
        {
            if(ModeString == "Poll" && PollSupported)
            {
                Mode = ModeType.Poll;
                DisableTriggerUse.Invoke(this, null);
                EnableTriggerSelect.Invoke(this, null);
                ModeChanged.Invoke(this, null);
            }
            else if(ModeString == "Interrupt" && InterruptSupported)
            {
                Mode = ModeType.Interrupt;
                DisableTriggerSelect.Invoke(this, null);
                EnableTriggerUse.Invoke(this, null);
                ModeChanged.Invoke(this, null);
            }
            else if(Mode != ModeType.Unconfigured)
            {
                Mode = ModeType.Unconfigured;
                ModeChanged.Invoke(this, null);
            }
        }

        public void SetPort(string Port)
        {
            if(SerialPort.GetPortNames().Contains(Port))
            {
                this.Port.PortName = Port;
                portSet = true;
                if(portSet && baudSet)
                {
                    EnableButton.Invoke(this, null);
                    SetStatus.Invoke(this, "Disconnected");
                }
            }
            else
            {
                DisableButton.Invoke(this, null);
                SetStatus.Invoke(this, "Unconfigured");
                portSet = false;
            }
        }

        protected void SendTrigger()
        {
            Trigger.Invoke(this, null);
        }

        ITriggerSource currentTriggerSource = null;
        public void SetTrigger(ITriggerSource triggerSource)
        {
            if(currentTriggerSource != null)
            {
                currentTriggerSource.Trigger -= Triggered;
                currentTriggerSource = null;
            }
            if(triggerSource != null)
            { 
                triggerSource.Trigger += Triggered;
                currentTriggerSource = triggerSource;
            }
        }

        protected virtual void Triggered(object sender, EventArgs e)
        {
            if(!PollSupported)
            {
                throw new NotImplementedException();
            }
        }

        public void SetBaud(int? baud)
        {
            if(baud.HasValue)
            {
                this.Port.BaudRate = baud.Value;
                baudSet = true;
                if (baudSet && portSet)
                {
                    EnableButton.Invoke(this, null);
                }
            }
            else
            {
                DisableButton.Invoke(this, null);
                baudSet = false;
            }
        }

        public List<int> GetBauds()
        {
            return new List<int> {9600, 19200, 38400, 57600, 115200, 125000, 128000, 230400, 250000, 256000, 460800,500000, 921600,1000000,2000000 };
        }

        public void SelectedType()
        {
            SetStatus.Invoke(this, "Unconfigured");
            EnablePortSelect.Invoke(this, null);
            EnableBaudSelect.Invoke(this, null);
            if(GetModes().Count>1)
            {
                EnableModeSelect.Invoke(this, null);
            }
        }

        public void DeselectedType()
        {
            if(Mode == ModeType.Poll)
            {
                DisableTriggerSelect.Invoke(this, null);
            }
            else if(Mode == ModeType.Interrupt)
            {
                DisableTriggerUse.Invoke(this, null);
            }
            DisablePortSelect.Invoke(this, null);
            DisableBaudSelect.Invoke(this, null);
            DisableButton.Invoke(this, null);
            DisableModeSelect.Invoke(this, null);
            Mode = ModeType.Unconfigured;
            Port = new SerialPort();
            portSet = false;
            baudSet = false;

            EnableTypeSelect = delegate { };
            EnableTypeSelect = delegate { };
            DisableTypeSelect = delegate { };
            EnablePortSelect = delegate { };
            DisablePortSelect = delegate { };
            EnableBaudSelect = delegate { };
            DisableBaudSelect = delegate { };
            EnableModeSelect = delegate { };
            DisableModeSelect = delegate { };
            EnableTriggerSelect = delegate { };
            DisableTriggerSelect = delegate { };
            EnableButton = delegate { };
            DisableButton = delegate { };
            SetStatus = delegate { };
            EnableTriggerUse = delegate { };
            DisableTriggerUse = delegate { };
            Trigger = delegate { };
            Connected = delegate { };
            Disconnected = delegate { };
            OnDataPoint = delegate { };
    }

        public virtual int DefaultBaud()
        {
            return 115200;
        }

        public string GetPort()
        {
            if(portSet)
            {
                return Port.PortName;
            }
            else
            {
                return "";
            }
        }



        public abstract List<DataPointType> GetFormat();


        protected void sendStatus(string status)
        {
            SetStatus.Invoke(this, status);
        }

        public virtual void OpenCustomDialog(Control parent)
        {
            if (!SupportCustomDialog())
            {
                throw new NotImplementedException();
            }
        }

        public virtual bool SupportCustomDialog()
        {
            return false;
        }
    }
}
