using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TorqueSensorHub.Drivers;

namespace TorqueSensorHub
{
    public partial class Diagnostics : Form
    {
        public Diagnostics(IDevice device)
        {
            InitializeComponent();
            this.device = device;
        }
        IDevice device;
        Timer updateTimer = new Timer();
        Stopwatch stopwatch = new Stopwatch();
        long lastTimestamp;
        List<long> durations = new List<long>();
        Series dataSeries;
        private void Diagnostics_Load(object sender, EventArgs e)
        {
            dataSeries = new Series("Periods");
            chart1.Series.Add(dataSeries);

            updateTimer.Interval = 1000;
            updateTimer.Start();
            updateTimer.Tick += UpdateTimer_Tick;
            device.OnDataPoint += Device_OnDataPoint;
            lastTimestamp = Stopwatch.GetTimestamp();
            NameText.Text = ((LoggingDeviceAttribute)device.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute))).Name;
            PortText.Text = device.GetPort();
            device.Disconnected += Device_Disconnected;

        }

        private void Device_Disconnected(object sender, EventArgs e)
        {
            updateTimer.Stop();
            this.Close();
            this.Dispose();
        }

        private void Device_OnDataPoint(object sender, List<KeyValuePair<DataPointType, decimal>> e)
        {
            long newTime = Stopwatch.GetTimestamp();
            long duration = newTime - lastTimestamp;
            lock(durations)
            {
                durations.Add(duration);
            }
            lastTimestamp = newTime;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!updateTimer.Enabled || durations.Count == 0)
            {
                return;
            }
            if(this.Disposing)
            {
                return;
            }
            List<long> raw;
            lock (durations)
            { 
                raw = durations.ToList();
                durations = new List<long>();
            }
            dataSeries.Points?.Clear();

            double M = 0.0;
            double S = 0.0;
            int k = 1;

            double Sum = 0;
            List<double> processed = new List<double>();

            double minbin = 0;
            double maxbin = 2.4;
            double binwidth = 0.1;
            int bins = (int)Math.Round((maxbin - minbin) / binwidth);
            int[] binned = new int[bins];
            foreach (var duration in raw)
            {
                double value = ((double)duration / (double)Stopwatch.Frequency)*1000.0;
                processed.Add(value);

                int bin = (int)((value-binwidth/2) / binwidth);
                binned[Math.Min(bin, bins-1)]++;

                //SD calculation
                double tmpM = M;
                M += (value - tmpM) / k;
                S += (value - tmpM) * (value - M);
                k++;

                //Mean calculation
                Sum += value;
            }
            processed.Sort();

            double SD = Math.Sqrt(S / (k - 2));
            double Mean = Sum / raw.Count();

            MeanPeriodText.Text = Mean.ToString("F3");
            SDPeriodText.Text = SD.ToString("F3");
            RateText.Text = (1000.0 / Mean).ToString("F3");
            MaxPeriodText.Text = (processed.Max()).ToString("F3");
            MinPeriodText.Text = (processed.Min()).ToString("F3");

            for(int i = 0; i<bins; i++)
            {
                dataSeries.Points?.AddXY((i+1)*binwidth, (double)binned[i]/raw.Count()*100);
            }
        }

    }
}
