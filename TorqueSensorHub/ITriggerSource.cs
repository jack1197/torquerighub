using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorqueSensorHub
{
    public interface ITriggerSource
    {
        event EventHandler Trigger;
    }
}
