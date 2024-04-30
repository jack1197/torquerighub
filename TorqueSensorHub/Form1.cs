using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TorqueSensorHub.Drivers;
using Timer = System.Windows.Forms.Timer;

namespace TorqueSensorHub
{
    public partial class Form1 : Form, ITriggerSource
    {
        enum OutputMode
        {
            MultifileUnsynced,
            SinglefileUnsynced,
            MultifileSynced,
            SinglefileSynced
        }
        OutputMode outputType = OutputMode.SinglefileSynced;
        public Form1()
        {
            InitializeComponent();
        }

        List<Tuple<string, ITriggerSource>> TriggerSources = new List<Tuple<string, ITriggerSource>>();
        List<Tuple<string, IDevice>> TorqueSources = new List<Tuple<string, IDevice>>();
        List<Tuple<string, IFeedbackDevice>> FeedbackSources = new List<Tuple<string, IFeedbackDevice>>();
        public event EventHandler SourcesListsUpdated = delegate { };
        public event EventHandler Trigger = delegate { };
        MultimediaTimer internalTriggerTimer = new MultimediaTimer();
        bool running = false;
        Stopwatch timeKeeper = new Stopwatch();
        List<InputDeviceDialog> inputDevices = new List<InputDeviceDialog>();
        List<Tuple<Delegate, IDevice>> subscribedDataEvents = new List<Tuple<Delegate, IDevice>>();
        List<KeyValuePair<string, FileStream>> openedFiles = new List<KeyValuePair<string, FileStream>>();
        List<List<DataPointType>> simpleHeaders = new List<List<DataPointType>>();
        List<KeyValuePair<string,List<DataPointType>>> complexHeaders = new List<KeyValuePair<string, List<DataPointType>>>();
        List<DataPointType> lumpedHeaders = new List<DataPointType>();
        FileStream singleFileStream = null;
        List<KeyValuePair<string, List<KeyValuePair<DataPointType, decimal>>>> LastDataPoints = new List<KeyValuePair<string, List<KeyValuePair<DataPointType, decimal>>>>();


        private void Form1_Load(object sender, EventArgs e)
        {
            TriggerSources.Add(new Tuple<string, ITriggerSource>("Internal Trigger", this));
            SaveTypeBox.SelectedIndex = 0;
            SourcesListsUpdated += Form1_SourceListsUpdated;
            SourcesListsUpdated.Invoke(this, null);
            internalTriggerTimer.Elapsed += InternalTriggerTimer_Tick;
            internalTriggerTimer.Resolution = 1;
            SyncTriggerBox.SelectedIndex = 0;
            FilePathBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 
                + Path.DirectorySeparatorChar + "TorqueSensorHub" 
                + Path.DirectorySeparatorChar + "Datalog.txt";
            this.Text += " " + (ApplicationDeployment.IsNetworkDeployed
               ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
               : Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void InternalTriggerTimer_Tick(object sender, EventArgs e)
        {
            Trigger.Invoke(this, null);
        }

        private void Form1_SourceListsUpdated(object sender, EventArgs e)
        {
            var currentSync = SyncTriggerBox.SelectedItem ?? "";
            SyncTriggerBox.Items.Clear();
            SyncTriggerBox.Items.AddRange(TriggerSources.Select(t => t.Item1).ToArray());
            if (SyncTriggerBox.Items.Contains(currentSync))
            {
                SyncTriggerBox.SelectedItem = currentSync;
            }
            else
            {
                if (SyncTriggerBox.Items.Count > 0) SyncTriggerBox.SelectedIndex = 0;
            }


            var currentTorque = autoTorqueSource.SelectedItem ?? "";
            autoTorqueSource.Items.Clear();
            autoTorqueSource.Items.AddRange(TorqueSources.Select(t => t.Item1).ToArray());
            if (autoTorqueSource.Items.Contains(currentSync))
            {
                autoTorqueSource.SelectedItem = currentSync;
            }
            else
            {
                if (autoTorqueSource.Items.Count > 0)
                {
                    autoTorqueSource.SelectedIndex = 0;
                }
            }
            autoTorqueSource.Enabled = autoTorqueSource.Items.Count > 0;


            var currentFeedback = autoTorqueSource.SelectedItem ?? "";
            autoFeedbackSource.Items.Clear();
            autoFeedbackSource.Items.AddRange(FeedbackSources.Select(t => t.Item1).ToArray());
            if (autoFeedbackSource.Items.Contains(currentFeedback))
            {
                autoFeedbackSource.SelectedItem = currentFeedback;
            }
            else
            {
                if (autoFeedbackSource.Items.Count > 0) autoFeedbackSource.SelectedIndex = 0;
            }
            autoFeedbackSource.Enabled = autoFeedbackSource.Items.Count > 0;

            AutomationButton.Enabled = (autoFeedbackSource.Enabled) && (autoTorqueSource.Enabled);
            FancyDemoButton.Enabled = AutomationButton.Enabled;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InputDeviceDialog dialog = new InputDeviceDialog(TriggerSources, TorqueSources, FeedbackSources);
            SourcesPanel.Controls.Add(dialog);
            dialog.DeleteButton.Click += DeleteButton_Click;
            dialog.SourceListsUpdateNotifySend += (send, args) => { SourcesListsUpdated.Invoke(this, null); };
            dialog.parent = this;
            inputDevices.Add(dialog);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            InputDeviceDialog control = button.Parent as InputDeviceDialog;
            SourcesPanel.Controls.Remove(control);
            inputDevices.Remove(control);
            control.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(InternalTriggerActive.Checked)
            {
                internalTriggerTimer.Interval = (int)(TimerPeriodSelect.Value);
                internalTriggerTimer.Start();
                TimerPeriodSelect.Enabled = false;
            }
            else
            {
                internalTriggerTimer.Stop();
                TimerPeriodSelect.Enabled = true;
            }
        }

        private void BrowseFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(FilePathBox.Text);
            openFileDialog1.FileName = Path.GetFileName(FilePathBox.Text);
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            FilePathBox.Text = openFileDialog1.FileName;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(SaveTypeBox.SelectedIndex)
            {
                case 0:
                    outputType = OutputMode.SinglefileUnsynced;
                    SyncTriggerBox.Enabled = false;
                    break;
                case 1:
                    outputType = OutputMode.SinglefileSynced;
                    SyncTriggerBox.Enabled = true;
                    break;
                case 2:
                    outputType = OutputMode.MultifileUnsynced;
                    SyncTriggerBox.Enabled = false;
                    break;
                case 3:
                    outputType = OutputMode.MultifileSynced;
                    SyncTriggerBox.Enabled = true;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(running)
            {

                currentTriggerSource.Trigger -= SyncTrigger;

                foreach (var obj in subscribedDataEvents)
                {
                    obj.Item2.OnDataPoint -= obj.Item1 as EventHandler<List<KeyValuePair<DataPointType, decimal>>>;
                }

                foreach (var fileOpened in openedFiles)
                {
                    fileOpened.Value.Flush();
                    fileOpened.Value.Close();
                    fileOpened.Value.Dispose();
                }
                if(singleFileStream != null)
                {
                    singleFileStream.Flush();
                    singleFileStream.Close();
                    singleFileStream.Dispose();
                    singleFileStream = null;

                }
                Thread.Sleep(50);
                simpleHeaders = new List<List<DataPointType>>();
                complexHeaders = new List<KeyValuePair<string, List<DataPointType>>>();
                openedFiles = new List<KeyValuePair<string, FileStream>>();
                lumpedHeaders = new List<DataPointType>();
                LastDataPoints = new List<KeyValuePair<string, List<KeyValuePair<DataPointType, decimal>>>>();

                running = false;
                SourcesPanel.Enabled = true;
                timestampCheckbox.Enabled = true;
                StartStopButton.Text = "Start";
                FilePathBox.Enabled = true;
                BrowseFileButton.Enabled = true;
                SaveTypeBox.Enabled = true;
                SyncTriggerBox.Enabled = outputType == OutputMode.SinglefileSynced || outputType == OutputMode.MultifileSynced;
                TimerPeriodSelect.Enabled = true;
                AddSourceButton.Enabled = true;
                InternalTriggerActive.Enabled = true;
                timeKeeper.Stop();
            }
            else
            {
                running = true;
                SourcesPanel.Enabled = false;
                timestampCheckbox.Enabled = false;
                StartStopButton.Text = "Stop";
                FilePathBox.Enabled = false;
                BrowseFileButton.Enabled = false;
                SaveTypeBox.Enabled = false;
                SyncTriggerBox.Enabled = false;
                TimerPeriodSelect.Enabled = false;
                InternalTriggerActive.Enabled = false;
                AddSourceButton.Enabled = false;
                timeKeeper.Restart();

                var devices = inputDevices.Where(id => id.device != null).Select(id => new { name = id.NameBox.Text, dev = id.device });
                string timestamp = timestampCheckbox.Checked ? DateTime.Now.ToString("_yyyy-MM-ddTHH-mm-ss") : "";

                if (outputType == OutputMode.SinglefileSynced || outputType == OutputMode.SinglefileUnsynced)
                {
                    string path = Path.GetDirectoryName(FilePathBox.Text) + Path.DirectorySeparatorChar +
                               Path.GetFileNameWithoutExtension(FilePathBox.Text) +
                               timestamp +
                               Path.GetExtension(FilePathBox.Text);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    var fs_single = File.Open(path, FileMode.Create);

                    string header = "Timestamp";

                    //headers
                    if (outputType == OutputMode.SinglefileUnsynced)
                    {
                        header += ";DeviceName;DeviceType";
                        HashSet<DataPointType> types = new HashSet<DataPointType>();
                        foreach (var device in devices)
                        {
                            var typesForDevice = device.dev.GetFormat().ToHashSet();
                            types.UnionWith(typesForDevice);
                        }

                        lumpedHeaders = types.ToList();
                        foreach (var type in lumpedHeaders)
                        {
                            header += $";{type:G}";
                        }
                    }
                    else if (outputType == OutputMode.SinglefileSynced)
                    {
                        string prepend = "";
                        int index = 0;
                        foreach (var device in devices)
                        {
                            string devType = ((LoggingDeviceAttribute)device.dev.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute))).Name;
                            prepend += $"#{index};{device.name};{devType}\r\n";


                            var typesForDevice = device.dev.GetFormat();
                            complexHeaders.Add(new KeyValuePair<string, List<DataPointType>>(device.name, typesForDevice));
                            foreach (var headerType in typesForDevice)
                            {
                                header += $";{headerType:G}_{index}";
                            }

                            index++;

                            LastDataPoints.Add(new KeyValuePair<string, List<KeyValuePair<DataPointType, decimal>>>(device.name, null));
                        }
                        header = prepend + header;
                    }
                    header += Environment.NewLine;
                    var data = Encoding.UTF8.GetBytes(header);
                    fs_single.Write(data, 0, data.Length);
                    singleFileStream = fs_single;

                }

                foreach (var device in devices)
                {
                    string devType = ((LoggingDeviceAttribute)device.dev.GetType().GetCustomAttribute(typeof(LoggingDeviceAttribute))).Name;
                    EventHandler<List<KeyValuePair<DataPointType, decimal>>> handler = (send, args) => { OnDataPoint(device.name, args, devType); };
                    
                    //open files

                    if (outputType == OutputMode.MultifileSynced || outputType == OutputMode.MultifileUnsynced)
                    { 
                        string path = Path.GetDirectoryName(FilePathBox.Text) + Path.DirectorySeparatorChar +
                            Path.GetFileNameWithoutExtension(FilePathBox.Text) +
                            timestamp + Path.DirectorySeparatorChar + device.name + "_" + devType +
                            Path.GetExtension(FilePathBox.Text);
                    
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        var fs = File.Open(path, FileMode.Create);

                        openedFiles.Add(new KeyValuePair<string, FileStream>(device.name, fs));
                        List<DataPointType> headerTypes = device.dev.GetFormat();
                        simpleHeaders.Add(headerTypes);
                        string header = "Timestamp";

                        foreach (var type in headerTypes)
                        {
                            header += $";{type:G}";
                        }
                        if(outputType == OutputMode.MultifileSynced)
                        {
                            complexHeaders.Add(new KeyValuePair<string, List<DataPointType>>(device.name, headerTypes));
                        }

                        LastDataPoints.Add(new KeyValuePair<string, List<KeyValuePair<DataPointType, decimal>>>(device.name, null));

                        header += Environment.NewLine;
                        var data = Encoding.UTF8.GetBytes(header);
                        fs.Write(data, 0, data.Length);
                    }


                    device.dev.OnDataPoint += handler;
                    subscribedDataEvents.Add(new Tuple<Delegate, IDevice>(handler, device.dev));



                }
                if (outputType == OutputMode.MultifileSynced || outputType == OutputMode.SinglefileSynced)
                {
                    currentTriggerSource.Trigger += SyncTrigger;
                }



            }
        }

        private void SyncTrigger(object sender, EventArgs e)
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //make sure everything has reported at least one value
            lock (LastDataPoints)
            {
                if (LastDataPoints.Count(LastDataPoints => LastDataPoints.Value == null) > 0)
                {
                    return;
                }

                if (outputType == OutputMode.MultifileSynced)
                {
                    foreach (var lastDataPoint in LastDataPoints)
                    {
                        string dataLine = $"{timeKeeper.Elapsed.TotalSeconds:F5}";

                        string devName = lastDataPoint.Key;
                        var openedFile = openedFiles.Where(of => of.Key == devName).First();
                        var fs = openedFile.Value;

                        var headers = complexHeaders.First(ch => ch.Key == devName).Value;
                        foreach (var header in headers)
                        {
                            var index = lastDataPoint.Value.FindIndex(dp => dp.Key == header);
                            if (index == -1)
                            {
                                dataLine += ";";
                            }
                            else
                            {
                                dataLine += $";{lastDataPoint.Value[index].Value:F5}";
                            }
                        }

                        dataLine += Environment.NewLine;
                        var data = Encoding.UTF8.GetBytes(dataLine);
                        fs.Write(data, 0, data.Length);
                    }
                }
                else if (outputType == OutputMode.SinglefileSynced)
                {
                    string dataLine = $"{timeKeeper.Elapsed.TotalSeconds:F5}";
                    foreach (var lastDataPoint in LastDataPoints)
                    {
                        string devName = lastDataPoint.Key;

                        var headers = complexHeaders.First(ch => ch.Key == devName).Value;
                        foreach (var header in headers)
                        {
                            var index = lastDataPoint.Value.FindIndex(dp => dp.Key == header);
                            if (index == -1)
                            {
                                dataLine += ";";
                            }
                            else
                            {
                                dataLine += $";{lastDataPoint.Value[index].Value:F5}";
                            }
                        }

                    }
                    dataLine += Environment.NewLine;
                    var data = Encoding.UTF8.GetBytes(dataLine);
                    singleFileStream.Write(data, 0, data.Length);

                }
            }
            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        private void OnDataPoint(string devName, List<KeyValuePair<DataPointType, decimal>> e, string devType)
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            if (outputType == OutputMode.MultifileUnsynced || outputType == OutputMode.SinglefileUnsynced)
            {
                string dataLine = $"{timeKeeper.Elapsed.TotalSeconds:F5}";
                FileStream fs;
                if (outputType == OutputMode.MultifileUnsynced)
                {
                    var openedFile = openedFiles.Where(of => of.Key == devName).First();
                    fs = openedFile.Value;
                    var headerTypes = simpleHeaders[openedFiles.IndexOf(openedFile)];


                    foreach (var type in headerTypes)
                    {
                        var index = e.FindIndex(dp => dp.Key == type);
                        if (index == -1)
                        {
                            dataLine += ";";
                        }
                        else
                        {
                            dataLine += $";{e[index].Value:F5}";
                        }
                    }
                }
                else//OutputMode.SinglefileUnsynced
                {
                    fs = singleFileStream;

                    dataLine += $";{devName};{devType}";

                    var headerTypes = lumpedHeaders;
                    foreach (var type in headerTypes)
                    {
                        var index = e.FindIndex(dp => dp.Key == type);
                        if (index == -1)
                        {
                            dataLine += ";";
                        }
                        else
                        {
                            dataLine += $";{e[index].Value:F5}";
                        }
                    }
                }
                dataLine += Environment.NewLine;
                var data = Encoding.UTF8.GetBytes(dataLine);
                fs.Write(data, 0, data.Length);
            }
            else //synced
            {
                var ldpItemIndex = LastDataPoints.FindIndex(LastDataPoints => LastDataPoints.Key == devName);
                lock (LastDataPoints)
                {
                    LastDataPoints[ldpItemIndex] = new KeyValuePair<string, List<KeyValuePair<DataPointType, decimal>>>(devName, e);
                }
            }

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        private void TimerPeriodSelect_ValueChanged(object sender, EventArgs e)
        {
            internalTriggerTimer.Interval = (int)TimerPeriodSelect.Value;
        }

        ITriggerSource currentTriggerSource = null;


        private void SyncTriggerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var source = TriggerSources.First(ts => ts.Item1 == SyncTriggerBox.Text).Item2;

            if (currentTriggerSource != null)
            {
                try { currentTriggerSource.Trigger -= SyncTrigger; } catch { }
                currentTriggerSource = null;

            }
            if (source != null)
            {
                currentTriggerSource = source;
            }
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            string path = Path.GetDirectoryName(FilePathBox.Text);
            Directory.CreateDirectory(path);
            Process.Start(path);
        }

        private void AutomationButton_Click(object sender, EventArgs e)
        {
            TorqueReg torqueReg = new TorqueReg(
                TorqueSources.First(i => i.Item1 == autoTorqueSource.SelectedItem as string).Item2, 
                FeedbackSources.First(i => i.Item1 == autoFeedbackSource.SelectedItem as string).Item2);
            torqueReg.Show();
        }

        private void FancyDemoButton_Click(object sender, EventArgs e)
        {
            TorqueRegNice torqueReg = new TorqueRegNice(
                TorqueSources.First(i => i.Item1 == autoTorqueSource.SelectedItem as string).Item2,
                FeedbackSources.First(i => i.Item1 == autoFeedbackSource.SelectedItem as string).Item2);
            torqueReg.Show();

        }

    }
}
