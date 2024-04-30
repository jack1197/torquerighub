using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TorqueSensorHub.Drivers
{
    [LoggingDevice("WiFi Screwdriver")]
    internal class WeberScrewdriverUltimateTCP : IDevice, IIPDevice
    {
        public event EventHandler EnablePortSelect = delegate { };
        public event EventHandler DisablePortSelect = delegate { };
        public event EventHandler EnableModeSelect = delegate { };
        public event EventHandler DisableModeSelect = delegate { };
        public event EventHandler EnableTriggerSelect = delegate { };
        public event EventHandler DisableTriggerSelect = delegate { };
        public event EventHandler EnableTypeSelect = delegate { };
        public event EventHandler DisableTypeSelect = delegate { };
        public event EventHandler EnableBaudSelect = delegate { };
        public event EventHandler DisableBaudSelect = delegate { };
        public event EventHandler EnableButton = delegate { };
        public event EventHandler DisableButton = delegate { };
        public event EventHandler<string> SetStatus = delegate { };
        public event EventHandler EnableTriggerUse = delegate { };
        public event EventHandler DisableTriggerUse = delegate { };
        public event EventHandler Connected = delegate { };
        public event EventHandler Disconnected = delegate { };
        public event EventHandler<List<KeyValuePair<DataPointType, decimal>>> OnDataPoint = delegate { };
        public event EventHandler Trigger = delegate { };

        private string IP = "192.168.137.101";
        private float LastGyro = 0f;
        private float LastTorque = 0f;
        WebClient wc { get; set; }

        private Uri getEventUri()
        {
            Uri uri = new Uri("http://error/");
            Uri.TryCreate("http://" + IP + "/events", UriKind.Absolute, out uri);
            return uri;
        }

        public void Connect()
        {
            wc = new WebClient();
            wc.OpenReadAsync(getEventUri());
            wc.OpenReadCompleted += ServerResponseStarts;
            Connected.Invoke(this, null);
            SetStatus.Invoke(this, "Connecting...");
            DisablePortSelect.Invoke(this, null);
        }

        private void ServerResponseStarts(object sender, OpenReadCompletedEventArgs args)
        {
            Task.Run(() => { ListeningTask(args.Result); });
        }

        private async Task ListeningTask(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                string msg = "";
                while (!sr.EndOfStream)
                {
                    string line = await sr.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine(msg);
                        ParseEvent(msg);
                        msg = "";
                    }
                    else
                    {
                        msg += line + Environment.NewLine;
                    }
                }
            }
        }
        private decimal gyropos = 0;
        private void ParseEvent(string msg)
        {
            var lines =  msg.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                string id_prefix = "id: ";
                var id = int.Parse(lines.Where(s => s.StartsWith(id_prefix)).SingleOrDefault()?.Substring(id_prefix.Length) ?? "-1");

                string event_prefix = "event: ";
                var eventname = lines.Where(s => s.StartsWith(event_prefix)).SingleOrDefault()?.Substring(event_prefix.Length) ?? "";

                string data_prefix = "data: ";
                var datastring = lines.Where(s => s.StartsWith(data_prefix)).SingleOrDefault()?.Substring(data_prefix.Length) ?? "";

                if (eventname == "trq_reading")
                {
                    SetStatus.Invoke(this, "Connected");
                    var dataPoint = new List<KeyValuePair<DataPointType, decimal>> { 
                        new KeyValuePair<DataPointType, decimal>(DataPointType.Torque, decimal.Parse(datastring)),
                        new KeyValuePair<DataPointType, decimal>(DataPointType.AngularDisplacment, gyropos)
                    };
                    OnDataPoint.Invoke(this, dataPoint);
                    Trigger.Invoke(this, null);
                }
                else if (eventname == "gyro_readings")
                {
                    dynamic obj = JObject.Parse(datastring);
                    gyropos = obj.gyroX / Math.PI * 180;
                    
                }
            }
            catch (Exception)
            {
            }


        }

        public void Disconnect()
        {
            if(wc != null)
            {
                wc.CancelAsync();
                wc.OpenReadCompleted -= ServerResponseStarts;
                wc.Dispose();
    
                wc = null;
            }
            Disconnected.Invoke(this, null);
            SetStatus.Invoke(this, "Disconnected");
            EnablePortSelect.Invoke(this, null);
        }


        public int DefaultBaud()
        {
            return 1;
        }


        public List<int> GetBauds()
        {
            return new List<int>{1};
        }

        public List<DataPointType> GetFormat()
        {
            return new List<DataPointType> {
                DataPointType.Torque,
                DataPointType.AngularDisplacment,
                };
        }

        public List<string> GetModes()
        {
            return new List<string> { "Interrupt" };
        }

        public string GetPort()
        {
            return IP;
        }

        public List<string> GetPorts()
        {
            return new List<string> { IP };
        }

        public void OpenCustomDialog(Control parent)
        {
            throw new NotImplementedException();
        }



        public void SelectedType()
        {
            EnablePortSelect.Invoke(this, null);
            DisableBaudSelect.Invoke(this, null);
            DisableModeSelect.Invoke(this, null);
            DisableTriggerSelect.Invoke(this, null);
            EnableButton.Invoke(this, null);
            EnableTriggerUse.Invoke(this, null);
        }

        public void DeselectedType()
        {
            DisablePortSelect.Invoke(this, null);
            DisableTriggerUse.Invoke(this, null);
        }


        public void SetBaud(int? baud)
        {
            return;
        }

        public void SetMode(string Mode)
        {
            return;
        }

        public void SetPort(string Port)
        {
            IP = Port;
        }

        public void SetTrigger(ITriggerSource Trigger)
        {
            return;
        }

        public bool SupportCustomDialog()
        {
            return false;
        }
    }
}
