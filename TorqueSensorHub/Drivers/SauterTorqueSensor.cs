using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TorqueSensorHub.Drivers
{
    [LoggingDevice("Sauter Torque Sensor")]
    class SauterTorqueSensor : BaseSerialDevice
    {
        public override List<DataPointType> GetFormat()
        {
            return new List<DataPointType> { DataPointType.Torque };
        }

        protected override void ProcessLine(string line)
        {
            line = Regex.Replace(line, @".*:\s*(\+|\-)?\s*([0-9]*\.[0-9]*).*", "$1$2");
            decimal value;
            bool success = decimal.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
            if(success)
            {
                SendDataPoint(new List<KeyValuePair<DataPointType, decimal>>{ new KeyValuePair<DataPointType, decimal>(DataPointType.Torque,value) });
            }

        }

        protected override bool PollSupported { get { return true; } }

        protected override void Triggered(object sender, EventArgs e)
        {
            base.Triggered(sender, e);
            if(Port.IsOpen)
            {
                Port.Write("l");
            }
        }

        public override int DefaultBaud()
        {
            return 9600;
        }
    }
}
