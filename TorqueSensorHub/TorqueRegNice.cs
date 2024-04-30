using MathNet.Numerics;
using System;
using System.Collections.Concurrent;
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
    public partial class TorqueRegNice : Form
    {
        readonly IDevice readDevice;
        readonly IFeedbackDevice feedbackDevice;
        Stopwatch timeKeeper = new Stopwatch();
        List<decimal> torqueArray = new List<decimal>();
        List<decimal> angularArray = new List<decimal>();
        ScrewData currentScrew = null;
        double currentStrength = 0;
        double holeSize = 3.2e-3;
        double alpha = 2 * Math.PI;
        double offset = 2 * Math.PI;
        double mu_thread = 0.17;
        double mu_head = 0.17;
        ConcurrentQueue<Tuple<DataPointCollection, double, double>> pointsToAdd = new ConcurrentQueue<Tuple<DataPointCollection, double, double>>();
        Timer updateCharts = new Timer();
        float calfac = 1.2f;


        float DisplayRange = 5.0f;
        float TorqueValue = 1.0f;
        float MinValue = 2.0f;
        float IdealValue = 2.7f;
        float MaxValue = 3.0f;

        List<Tuple<string, double, float>> knownMaterials = new List<Tuple<string, double, float>>
        {
            new Tuple<string, double, float>("M80", 0.8e6, 0.7f),
            new Tuple<string, double, float>("M150", 1.6e6, 1.5f),
            new Tuple<string, double, float>("M330", 4e6, 3f),
            new Tuple<string, double, float>("M450", 10e6, 5f),
            new Tuple<string, double, float>("M600", 17e6, 5f),
        };

        class ScrewData
        {
            public string Name { get; set; }
            public double Pitch { get; set; }
            public double Major { get; set; }
            public double Minor { get; set; }
            public double Shank { get; set; } = 4;
            public double Head { get; set; } = 10;
            public double Taper { get; set; } = 60;//degrees
            public double Beta { get; set; }//radians
            public ScrewData(string Name, double Pitch, double Major, double Minor, double Beta)
            {
                this.Name = Name;
                this.Pitch = Pitch;
                this.Major = Major;
                this.Minor = Minor;
                this.Beta = Beta;
            }
        }

        List<ScrewData> screws = new List<ScrewData> 
        {
            //Name, Pitch, Major-Dim, Minor-Diam
            //new ScrewData ( "HA 4.5", 1.75e-3, 4.5e-3, 3.0e-3, 0.3316 ),
            new ScrewData ( "HB 6.5", 2.75e-3, 6.5e-3, 3.0e-3, 0.2618 ),
        };

        readonly TimeSpan UpdateInterval = new TimeSpan(0, 0, 0, 0, 100);//100 ms
        TimeSpan NextUpdate = new TimeSpan(0);

        readonly TimeSpan PointInterval = new TimeSpan(0, 0, 0, 0, 10);//10 ms
        TimeSpan NextPoint = new TimeSpan(0);

        public TorqueRegNice(IDevice readDevice, IFeedbackDevice feedbackDevice)
        {
            InitializeComponent();
            this.readDevice = readDevice;
            this.feedbackDevice = feedbackDevice;
            MaterialListBox.Items.AddRange(knownMaterials.Select(item => item.Item1).ToArray());
            MaterialListBox.SelectedItem = "M330";
            ScrewListBox.Items.AddRange(screws.Select(item => item.Name).ToArray());
            ScrewListBox.SelectedIndex = 0;
            //torqueChart.ChartAreas.First().AxisX.Minimum = 0;
            //torqueChart.ChartAreas.First().AxisY.Minimum = -0.5;
            //torqueChart.ChartAreas.First().AxisY.Interval = 0.5;
            //strengthChart.ChartAreas.First().AxisY.Minimum = -5;
            //strengthChart.ChartAreas.First().AxisY.Maximum = 25;
            //strengthChart.ChartAreas.First().AxisY.Interval = 5;
            updateCharts.Interval = 50;
            updateCharts.Tick += UpdateCharts_Tick;
        }

        private void UpdateCharts_Tick(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                while (pointsToAdd.Count > 0)
                {
                    Tuple<DataPointCollection, double, double> point;
                    pointsToAdd.TryDequeue(out point);//it will be there

                    point.Item1.AddXY(point.Item2, point.Item3);
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NextUpdate = new TimeSpan(UpdateInterval.Ticks);
            NextPoint = new TimeSpan(0);
            torqueArray.Clear();
            angularArray.Clear();
            timeKeeper.Reset();
            feedbackDevice.StartInsertion();
            timeKeeper.Start();
            updateCharts.Start();

            //foreach (var series in torqueChart.Series)
            //{
            //    series.Points.Clear();
            //}

            //foreach (var series in strengthChart.Series)
            //{
            //    series.Points.Clear();
            //}

            readDevice.OnDataPoint += ReadDevice_OnDataPoint;
        }

        private void ReadDevice_OnDataPoint(object sender, List<KeyValuePair<DataPointType, decimal>> e)
        {
            if (e.Count(item => item.Key == DataPointType.Torque) < 1 || (e.Count(item => item.Key == DataPointType.AngularDisplacment) < 1 && e.Count(item => item.Key == DataPointType.LinearDisplacment) < 1))
            {
                return;
            }


            if (timeKeeper.Elapsed > NextPoint)
            {
                NextPoint = timeKeeper.Elapsed + PointInterval;
            }
            else
            {
                return;
            }

            var torque = e.First(item => item.Key == DataPointType.Torque).Value;
            decimal angular = 0;
            if (e.Count(item => item.Key == DataPointType.LinearDisplacment) > 0)
            {
                var linear = e.First(item => item.Key == DataPointType.LinearDisplacment).Value;
                var linear_x0 = (holeSize/2) / Math.Tan((currentScrew.Taper / 2)/180*Math.PI);
                var angular_x0 = linear_x0 / currentScrew.Pitch * 2 * Math.PI;
                angular = (-linear / (decimal)currentScrew.Pitch * 2 * (decimal)Math.PI / 1000) - (decimal)angular_x0;
            }
            else
            {
                angular = e.First(item => item.Key == DataPointType.AngularDisplacment).Value / 180L * (decimal)Math.PI;
            }
            torqueArray.Add(torque);
            angularArray.Add(angular);

            if (timeKeeper.Elapsed > NextUpdate)
            {
                NextUpdate = timeKeeper.Elapsed + UpdateInterval;
                updateEstimate();
            }

            //pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(torqueChart.Series.FindByName("Measured Torque").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round((double)torque, 3)));

        }

        private void updateEstimate()
        {
            int length = torqueArray.Count;

            var torqueDoubleArray = torqueArray.Select(t => (double)t).Take(length).ToArray();
            var angularDoubleArray = angularArray.Select(a => ((double)a)).Take(length).ToArray();


            var theta = Math.Atan2(currentScrew.Pitch , (Math.PI * currentScrew.Major));
            var rs = (2 * holeSize + currentScrew.Major) / 6;
            var Ac = Math.Tan(currentScrew.Beta) * Math.Pow(currentScrew.Major - holeSize, 2) / 4;
            var G1 = rs * Ac * Math.Cos(theta);

            var rf = (holeSize + currentScrew.Major) / 4;
            var Kf0 = 
                0.5 
                * (currentScrew.Major - holeSize) 
                * Math.Sqrt((1 + Math.Pow(Math.Tan(currentScrew.Beta), 2)) * (Math.Pow((currentScrew.Major + holeSize) / 4, 2) + Math.Pow(currentScrew.Pitch / (2 * Math.PI), 2)));
            var G2 = 2 * rf * Kf0 * Math.Cos(theta);

            var strength = Fit.LinearCombination(angularDoubleArray, torqueDoubleArray, (phi) =>
            {
                phi = Math.Max(phi - offset, 0);
                var zeta = ((phi > alpha) ? 1 : 0) * (phi - alpha);
                var psi = phi;

                var Tsz = (1 / alpha) * G1 * (psi - zeta);
                var Tfz = mu_thread * G2 * (phi - alpha / 2);

                return Tsz + Tfz;
            })[0];

            var theta_tau = Math.Atan2(currentScrew.Pitch, Math.PI * currentScrew.Shank) + Math.Atan2(1, -mu_thread);
            var tau_ult = strength;// / Math.Sqrt(3);

            var Ac_fail = Math.PI * currentScrew.Major * angularDoubleArray[length - 1] / (2 * Math.PI) * currentScrew.Pitch;
            
            //Console.WriteLine(angularDoubleArray[length - 1]/Math.PI*180);

            var T_fail = tau_ult * Ac_fail * ((1 / 3) * (Math.Pow(currentScrew.Head, 3) - Math.Pow(currentScrew.Shank, 3)) / (Math.Pow(currentScrew.Head, 2) - Math.Pow(currentScrew.Shank, 2)) 
                * mu_head * Math.Sin(theta_tau) - currentScrew.Major / 2 * Math.Cos(theta_tau)) * calfac;

            var l = (currentScrew.Major - currentScrew.Minor) / 2;
            var gamma = 2 * l / currentScrew.Pitch;
            var k_t = l + (1 + 1.5 * Math.Tanh(0.3 * Math.Log(gamma) + 0.7)) * Math.Sqrt(gamma);

            var T_fail_kz = T_fail / k_t;



            //pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Estimated Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(strength / 1e6, 3)));
            //pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(strengthChart.Series.FindByName("Known Value").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(currentStrength / 1e6, 3)));
            //pointsToAdd.Enqueue(new Tuple<DataPointCollection, double, double>(torqueChart.Series.FindByName("Estimated Limit").Points, Math.Round(timeKeeper.Elapsed.TotalSeconds, 2), Math.Round(Math.Max(T_fail, 0), 3)));

            TorqueValue = (float)torqueDoubleArray[length - 1];
            MaxValue = (float)T_fail;
            MinValue = (float)T_fail * 0.7f;
            IdealValue = (float)T_fail * 0.8f;
            pictureBox1.Invalidate();

            if (autoStopBox.Checked && torqueDoubleArray[length-1] > T_fail*0.8 && angularDoubleArray[length - 1] > Math.PI * 20)
            {
                feedbackDevice.StopInsertion();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            feedbackDevice.StopInsertion();
            readDevice.OnDataPoint -= ReadDevice_OnDataPoint;
            updateCharts.Stop();
            while (pointsToAdd.Count > 0)
            {
                Tuple<DataPointCollection, double, double> point;
                pointsToAdd.TryDequeue(out point);
            }
        }

        private void TorqueReg_FormClosing(object sender, FormClosingEventArgs e)
        {
            feedbackDevice.StopInsertion();
            readDevice.OnDataPoint -= ReadDevice_OnDataPoint;
            updateCharts.Stop();
            while (pointsToAdd.Count > 0)
            {
                Tuple<DataPointCollection, double, double> point;
                pointsToAdd.TryDequeue(out point);
            }
        }

        private void ScrewListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentScrew = screws.First(s => s.Name == ScrewListBox.SelectedItem as string);
        }

        private void MaterialListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentStrength = knownMaterials.First(m => m.Item1 == (string)MaterialListBox.SelectedItem).Item2;
            DisplayRange = knownMaterials.First(m => m.Item1 == (string)MaterialListBox.SelectedItem).Item3;
            pictureBox1.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
            }
        }

        private void DrawRotatedImage(Graphics graphics, Image image,  Size targetSize,  float angle)
        {
            graphics.TranslateTransform((float)targetSize.Width / 2, (float)targetSize.Height / 2);
            //rotate
            graphics.RotateTransform(angle);
            //move image back
            graphics.TranslateTransform(-(float)targetSize.Width / 2, -(float)targetSize.Height / 2);
            graphics.DrawImage(image, new Rectangle(new Point(0,0), targetSize));
            graphics.ResetTransform();
        }

        private float scaledAngle(float value)
        {
            float prop = value / DisplayRange;
            prop = Math.Min(Math.Max(prop, 0), 1);
            const float minAngle = -105;
            const float maxAngle = 105;
            return minAngle + prop * (maxAngle - minAngle);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //int scale = 2;
            //var bitmap = new Bitmap(1920 * scale, 1080 * scale);
            Graphics graphics = e.Graphics;

            var MinValue = this.MinValue;
            var MaxValue = this.MaxValue;
            var TorqueValue = this.TorqueValue;
            var IdealValue = this.IdealValue;
            var DisplayRange = this.DisplayRange;

            Type pboxType = pictureBox1.GetType();
            PropertyInfo irProperty = pboxType.GetProperty("ImageRectangle", BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance);
            Rectangle rectangle = (Rectangle)irProperty.GetValue(pictureBox1, null);

            {
                var targetSize = rectangle.Size;
                float scale = targetSize.Width/1920f;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                DrawRotatedImage(graphics, TorqueSensorHub.Dial.InnerDial, targetSize, scaledAngle(TorqueValue));
                if (TorqueValue > DisplayRange / 1.9f)
                {
                    graphics.DrawImage(TorqueSensorHub.Dial.InnerDialFill, new Rectangle(new Point(0, 0), targetSize));
                }

                if(MinValue > DisplayRange / 1.9f)
                {
                    DrawRotatedImage(graphics, TorqueSensorHub.Dial.OuterDialGreen, targetSize, 75);
                }
                else if(MaxValue < DisplayRange / 2.1f)
                {
                    DrawRotatedImage(graphics, TorqueSensorHub.Dial.OuterDialGreen, targetSize, -75);
                }
                else
                {
                    graphics.DrawImage(TorqueSensorHub.Dial.OuterDialGreen, new Rectangle(new Point(0, 0), targetSize));
                }

                DrawRotatedImage(graphics, TorqueSensorHub.Dial.OuterDialYellow, targetSize, scaledAngle(MinValue));
                DrawRotatedImage(graphics, TorqueSensorHub.Dial.OuterDialRed, targetSize, scaledAngle(MaxValue));
                graphics.DrawImage(TorqueSensorHub.Dial.FG, new Rectangle(new Point(0, 0), targetSize));
                DrawRotatedImage(graphics, TorqueSensorHub.Dial.InnerDialMark, targetSize, scaledAngle(TorqueValue));
                DrawRotatedImage(graphics, TorqueSensorHub.Dial.OuterDialMark, targetSize, scaledAngle(IdealValue));
                SolidBrush drawBrush = new SolidBrush(Color.White);
                graphics.DrawString($"{(int)(TorqueValue * 100):D}", new Font("Arial", 140 * scale), drawBrush, new PointF(980 * scale, 890 * scale), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far });
                graphics.DrawString("cNm", new Font("Arial", 90 * scale), drawBrush, new PointF(990 * scale, 860 * scale), new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far });
                graphics.DrawString($"Target: {(int)(IdealValue * 100):D} cNm", new Font("Arial", 64 * scale), drawBrush, new PointF(960 * scale, 990 * scale), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
                graphics.DrawString($"Range: {(int)(MinValue * 100):D}-{(int)(MaxValue * 100):D} cNm", new Font("Arial", 45 * scale), drawBrush, new PointF(960 * scale, 1060 * scale), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
            }



        }
    }
}
