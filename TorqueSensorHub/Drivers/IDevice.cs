using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorqueSensorHub.Drivers
{
    public enum DataPointType
    {
        Unknown,
        LinearDisplacment,
        LinearVelocity,
        AngularDisplacment,
        AngularVelocity,
        Torque,
        AxialForce,
    }

    [AttributeUsage(AttributeTargets.All)]
    public class LoggingDeviceAttribute : Attribute
    {
        private string name;

        public LoggingDeviceAttribute(string name)
        {
            this.name = name;
        }

        public virtual string Name
        {
            get { return name; }
        }
    }

    public interface IIPDevice { };


    public interface IDevice : ITriggerSource
    {
        event EventHandler EnablePortSelect;
        event EventHandler DisablePortSelect;

        event EventHandler EnableModeSelect;
        event EventHandler DisableModeSelect;

        event EventHandler EnableTriggerSelect;
        event EventHandler DisableTriggerSelect;

        event EventHandler EnableTypeSelect;
        event EventHandler DisableTypeSelect;

        event EventHandler EnableBaudSelect;
        event EventHandler DisableBaudSelect;

        event EventHandler EnableButton;
        event EventHandler DisableButton;
        event EventHandler<string> SetStatus;


        event EventHandler EnableTriggerUse;
        event EventHandler DisableTriggerUse;



        event EventHandler Connected;
        event EventHandler Disconnected;

        event EventHandler<List<KeyValuePair<DataPointType,decimal>>> OnDataPoint;

        List<string> GetPorts();
        List<string> GetModes();
        List<int> GetBauds();
        int DefaultBaud();
        void SetPort(string Port);
        string GetPort();
        void SetBaud(int? baud);
        void SetMode(string Mode);
        void SetTrigger(ITriggerSource Trigger);
        void Connect();
        void Disconnect();
        void SelectedType();
        void DeselectedType();
        void OpenCustomDialog(Control parent);
        bool SupportCustomDialog();
        List<DataPointType> GetFormat();


    }
}
