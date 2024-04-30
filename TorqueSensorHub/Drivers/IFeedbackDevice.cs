using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorqueSensorHub.Drivers
{
    public interface IFeedbackDevice
    {
        event EventHandler StartedInsertion;
        event EventHandler StoppedInsertion;

        void StopInsertion();
        void StartInsertion();
    }
}
