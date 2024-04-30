using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.UI.DataVisualization.Charting;

namespace TorqueSensorHub.Drivers
{
    [LoggingDevice("Simulated Device")]
    class SimDevice : IDevice, IFeedbackDevice
    {

        Stopwatch timeKeeper = new Stopwatch();
        Random noiseGen = new Random();
        Chart chart = new Chart(); // Microsoft, this is dumb

        public SimDevice()
        {

        }

        int count = 0;

        public event EventHandler StartedInsertion = delegate { };
        public event EventHandler StoppedInsertion = delegate { };
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

        public decimal InsertionSpeed { get; set; } = 30M; //RPM
        public decimal MaxRotations { get; set; } = 12M;
        public decimal Pitch { get; set; } = 2.75M;
        public decimal MaxInsertionTorque { get; set; } = 1.5M;
        public decimal MaxStrippingTorque { get; set; } = 3.5M;
        public decimal StrippingRotations { get; set; } = 0.2M;
        public decimal AfterStrippingTorque { get; set; } = 2.5M;
        public decimal AfterStrippingRotations { get; set; } = 0.5M;
        public decimal NoiseSD { get; set; } = 0.01M;

        public List<DataPointType> GetFormat()
        {
            return new List<DataPointType> {
                DataPointType.Torque,
                DataPointType.LinearDisplacment,
                DataPointType.AngularDisplacment,
                };
        }

        void Triggered(object sender, EventArgs e)
        {


            decimal angularDisplacment = (decimal)timeKeeper.Elapsed.TotalMinutes * InsertionSpeed * 360;
            decimal linearDisplacment = angularDisplacment / 360 * Pitch;
            decimal torque = 0;

            if (angularDisplacment / 360 < MaxRotations)
            {
                torque = angularDisplacment / 360 / MaxRotations * MaxInsertionTorque;
            }
            else if (angularDisplacment / 360 < MaxRotations + StrippingRotations)
            {
                torque = MaxInsertionTorque + (MaxStrippingTorque - MaxInsertionTorque) * (angularDisplacment / 360 - MaxRotations) / StrippingRotations;
            }
            else if (angularDisplacment / 360 < MaxRotations + StrippingRotations + AfterStrippingRotations)
            {
                torque = MaxStrippingTorque + (AfterStrippingTorque - MaxStrippingTorque) * (angularDisplacment / 360 - MaxRotations - StrippingRotations) / AfterStrippingRotations;
            }
            else if (angularDisplacment / 360 >= MaxRotations + StrippingRotations + AfterStrippingRotations)
            {
                torque = AfterStrippingTorque;
            }

            torque += (decimal)chart.DataManipulator.Statistics.InverseNormalDistribution(Math.Max(Math.Min(noiseGen.NextDouble(),0.9999), 0.0001)) * NoiseSD;

            OnDataPoint.Invoke(this, new List<KeyValuePair<DataPointType, decimal>> {
                new KeyValuePair<DataPointType, decimal>(DataPointType.Torque, torque),
                new KeyValuePair<DataPointType, decimal>(DataPointType.LinearDisplacment, -linearDisplacment),
                new KeyValuePair<DataPointType, decimal>(DataPointType.AngularDisplacment, angularDisplacment),
            });

        }

        public void StopInsertion()
        {
            timeKeeper.Reset();
        }

        public void StartInsertion()
        {
            timeKeeper.Start();
        }

        public List<string> GetPorts()
        {
            return new List<string> { "Simulation" };
        }

        public List<string> GetModes()
        { 
            return new List<string> { "Poll" };
        }

        public List<int> GetBauds()
        {
            return new List<int> { 1 };
        }

        public int DefaultBaud()
        {
            return GetBauds()[0];
        }

        public void SetPort(string Port)
        {
            return;
        }

        public string GetPort()
        {
            return GetPorts()[0];
        }

        public void SetBaud(int? baud)
        {
            return;
        }

        public void SetMode(string Mode)
        {
            return;
        }

        ITriggerSource currentTriggerSource = null;
        public void SetTrigger(ITriggerSource triggerSource)
        {
            if (currentTriggerSource != null)
            {
                currentTriggerSource.Trigger -= Triggered;
                currentTriggerSource = null;
            }
            if (triggerSource != null)
            {
                triggerSource.Trigger += Triggered;
                currentTriggerSource = triggerSource;
            }
        }

        public void Connect()
        {
            Connected.Invoke(this, null);
            SetStatus.Invoke(this, "Connected");
            DisableTypeSelect.Invoke(this, null);
            DisableTriggerSelect.Invoke(this, null);
        }

        public void Disconnect()
        {
            Disconnected.Invoke(this, null);
            SetStatus.Invoke(this, "Disconnected");
            EnableTypeSelect.Invoke(this, null);
            EnableTriggerSelect.Invoke(this, null);
        }

        public void SelectedType()
        {
            DisablePortSelect.Invoke(this, null);
            DisableBaudSelect.Invoke(this, null);
            DisableModeSelect.Invoke(this, null);
            EnableTriggerSelect.Invoke(this, null);
            EnableButton.Invoke(this, null);
        }

        public void DeselectedType()
        {
            EnableTypeSelect.Invoke(this, null);
            DisableTriggerSelect.Invoke(this, null);
        }

        public void OpenCustomDialog(Control parent)
        {
            throw new NotImplementedException();
        }

        public bool SupportCustomDialog()
        {
            return false;
        }
    }
}
