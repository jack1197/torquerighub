using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorqueSensorHub.Drivers
{
    [LoggingDevice("Test Example Device")]
    class TestDevice : BaseSerialDevice
    {
        protected override bool InterruptSupported { get { return true; } }
        protected override bool PollSupported { get { return true; } }


        public TestDevice()
        {
              
        }

        int count = 0;
        protected override void ProcessLine(string line)
        {
            Console.WriteLine($"{count++}:{line}");
            SendDataPoint(new List<KeyValuePair<DataPointType, decimal>>());
        }

        public override List<DataPointType> GetFormat()
        {
            return new List<DataPointType>();
        }

        protected override void Triggered(object sender, EventArgs e)
        {

            base.Triggered(sender, e);
            if(Port.IsOpen)
                Console.WriteLine("Triggered");
        }
    }
}
