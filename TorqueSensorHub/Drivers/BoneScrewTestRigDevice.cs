using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TorqueSensorHub.CustomDialog;

namespace TorqueSensorHub.Drivers
{
    [LoggingDevice("Bone Screw Test Rig")]
    public class BoneScrewTestRigDevice : BaseSerialDevice, IFeedbackDevice
    {
        public event EventHandler<string> StatusMessageRecieved = delegate { };
        public event EventHandler StartedInsertion = delegate { };
        public event EventHandler StoppedInsertion = delegate { };

        public BoneScrewTestRigDevice()
        {

        }
        protected override bool InterruptSupported { get { return true;  } }

        protected override void ProcessLine(string line)
        {
            //Console.WriteLine(line);
            if (line.StartsWith("!"))
            {
                StatusMessageRecieved.Invoke(this, line.Substring(1));
            }
            else
            {
                var values = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<KeyValuePair<DataPointType, decimal>> datapoints = new List<KeyValuePair<DataPointType, decimal>>();
                foreach (var value in values)
                {
                    var parts = value.Split(new char[] { ':' });
                    if (parts.Length == 2)
                    {
                        decimal number;
                        decimal.TryParse(parts[1], out number);
                        DataPointType type = DataPointType.Unknown;
                        switch (parts[0])
                        {
                            case "T":
                                type = DataPointType.Torque;
                                number /= 1000.0m;//to Nm
                                break;
                            case "R":
                                type = DataPointType.AngularDisplacment;
                                number /= 4.0m; //to degrees
                                break;
                            case "D":
                                type = DataPointType.LinearDisplacment;
                                number /= 40.0m;//To mm
                                break;
                        }
                        KeyValuePair<DataPointType, decimal> datapoint = new KeyValuePair<DataPointType, decimal>(type, number);
                        datapoints.Add(datapoint);
                    }
                }
                SendDataPoint(datapoints);
                SendTrigger();
            }
        }

        protected override void SetupDevice()
        {
            base.SetupDevice();
            if (Port.IsOpen)
            {
                Port.Write("\r\nrawmode 1\r\n");
            }
        }

        protected override void StopDevice()
        {
            base.StopDevice();
            if (Port.IsOpen)
            {
                Port.Write("\r\nrawmode 0\r\n");
            }
        }

        public override int DefaultBaud()
        {
            return 2000000;
        }

        public override List<DataPointType> GetFormat()
        {
            return new List<DataPointType> { DataPointType.Torque, DataPointType.LinearDisplacment, DataPointType.AngularDisplacment };
        }

        public void SendCustomCommand(string cmd)
        {
            Port.Write("\r\n" + cmd + "\r\n");
            if(cmd.Trim().Split(' ')[0].Trim() == "start")
            {
                StartedInsertion.Invoke(this, null);
            }
            else if(cmd.Trim().Split(' ')[0].Trim() == "stop")
            {
                StartedInsertion.Invoke(this, null);
            }
        }

        public override bool SupportCustomDialog()
        {
            return true;
        }

        public override void OpenCustomDialog(Control parent)
        {
            base.OpenCustomDialog(parent);
            var dialog = new BoneScrewTestRigSettings(this);
            dialog.Show(parent);
        }

        public void StopInsertion()
        {
            SendCustomCommand($"stop");
        }

        public void StartInsertion()
        {
            SendCustomCommand($"start");
        }
    }
}
