using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Buttplug.Core.Messages;
using CefSharp;
using CefSharp.WinForms;
using SexToyLink.Classes;
using System.Diagnostics;
using Buttplug.Core;
using Buttplug.Client;
using Buttplug.Core.Messages;
using Buttplug.Client.Connectors.WebsocketConnector;
using System.Net.Http;
using System.Threading;

namespace SexToyLink
{
    public partial class Form1 : Form
    {
        bool debug = false;


        private TaskCompletionSource<bool> stopScanClickWaitTask;
        delegate void SetTextCallback(string text);
        ButtplugWebsocketConnector myConnector;
        ButtplugClient client = new ButtplugClient("SexToyLink Client");
        List<ButtplugClientDevice> devicesOral = new List<ButtplugClientDevice>();
        List<ButtplugClientDevice> devicesVaginal = new List<ButtplugClientDevice>();
        List<ButtplugClientDevice> devicesAnal = new List<ButtplugClientDevice>();
        List<ButtplugClientDevice> devicesGenericTemp = new List<ButtplugClientDevice>();
        List<uint> options = new List<uint>();
        volatile bool controlActive = false;
        volatile int selectedTab = 0;
        volatile string UI_update_action = "", url = "";
        volatile Controller mycontroller = new Controller();
        bool loadingSettingsFromFile = true;
        bool recursive = false;
        volatile Stopwatch elapsedTime;
        TimeSpan totalElapsedTime, cycleSpan, totalElapsedTimeReverse;
        long totalElapsePercent = 0;
        bool patternDirectionForward = true;
        bool playingDOL = false;
        float vibrationCurrentIntensity = 0;

        public Form1()
        {
            InitializeComponent();
            if (debug) richTextBox1.BringToFront();
            CefSettings settings = new CefSettings();
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);
            elapsedTime = new Stopwatch();
            totalElapsedTime = new TimeSpan(0, 0, 0);
            totalElapsedTimeReverse = new TimeSpan(0, 0, 0);
        }

        void LoadSettings()
        {
            //checkBox_remember_DOL_path.Checked = mycontroller.mySettings.Get_remember_DOL_path();
            textBox_DOL_min_str.Text = mycontroller.mySettings.Get_DOL_vib_min().ToString();
            textBox_DOL_max_str.Text = mycontroller.mySettings.Get_DOL_vib_max().ToString();
            textBox_DOL_cycle.Text = mycontroller.mySettings.Get_DOL_vib_cycle().ToString();
            textBox_buttplug_port.Text = mycontroller.mySettings.Get_Port().ToString();
            textBox_buttplug_IP.Text = mycontroller.mySettings.Get_IP().ToString();            
            loadingSettingsFromFile = false;
        }

        void LoadSettingsVar()
        {
            cycleSpan = new TimeSpan(0, 0, mycontroller.mySettings.Get_DOL_vib_cycle());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client.DeviceAdded += HandleDeviceAdded;
            client.DeviceRemoved += HandleDeviceRemoved;
            LoadSettings();
        }


        void HandleDeviceAdded(object aObj, DeviceAddedEventArgs aArgs)
        {//this gets trigered imediately after connecting, giving us a list of all devices. But might not need it? client.devices might be automatically updated.
         //Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Connected!");
         //devicesGenericTemp.Add 
         //devicesVaginal
         //devicesAnal
         //devicesOral


        }

        void HandleDeviceRemoved(object aObj, DeviceRemovedEventArgs aArgs)
        {
            MessageBox.Show("A device was removed while connected. This is not yet handled. Disconnecting to prevent issues. Please reconnect.");
            client.DisconnectAsync();
        }


        void SetStrength(float newValue)
        {
            // MessageBox.Show("str changed to " + text);
            /*
             if (trackBar1.InvokeRequired)
             {
                 SetTextCallback d = new SetTextCallback(SetStrength);
                 this.Invoke(d, new object[] { text });
             }
             else
             {
                 int value = Convert.ToInt32(text);
                 if (trackBar1.Value != value)
                     trackBar1.Value = value;
             }*/

            vibrationCurrentIntensity = newValue;

        }

        void SetStatus(string text)//live verion
        {
            if (this.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.Text = text;
            }
        }



        async Task UpdateVibration()
        {
           
            try
            {
                //List<Task> devicesTasks = new List<Task>(); 
                while (controlActive)
                {
                    //SetVibrationStatus(vibrationCurrentIntensity + "%");

                    foreach (ButtplugClientDevice curDevice in devicesGenericTemp)
                    {
                        //devicesTasks.Add(curDevice.SendVibrateCmd(trackBar1.Value / 100f));
                        //await curDevice.SendVibrateCmd(vibrationCurrentIntensity / 100f);
                        await curDevice.VibrateAsync(vibrationCurrentIntensity / 100f);

                    }
                    //Task.WaitAll(devicesTasks.ToArray());
                    //await Task.Delay(100);
                }

                foreach (ButtplugClientDevice curDevice in devicesGenericTemp)
                {
                    await curDevice.VibrateAsync(0);

                }
                //await device.SendVibrateCmd(0);
                
            }
            catch (ButtplugDeviceException)
            {
                controlActive = false;
                SetStatus("Device disconnected.");
                //ClearDeviceList();
            }

        }

        private void button_Play_Click(object sender, EventArgs e)
        {
            if (!mycontroller.mySettings.Get_remember_DOL_path() || mycontroller.mySettings.Get_DOL_path() == "")
            {

                OpenFileDialog myDialog = new OpenFileDialog();
                myDialog.Filter = "Degrees of Lewdity|*.html";
                myDialog.RestoreDirectory = true;
                if (myDialog.ShowDialog() == DialogResult.OK)
                {
                    Open_DOL_button.Text = "Loading, please wait.";
                    UI_update_action = "load DOL";
                    mycontroller.mySettings.Set_DOL_path(myDialog.FileName);
                    mycontroller.SaveSettingsFile();
                    url = new Uri(myDialog.FileName, UriKind.Absolute).AbsoluteUri;
                    timer_UI_update_wait.Start();
                }
            }
            else
            {
                Open_DOL_button.Text = "Loading, please wait.";
                UI_update_action = "load DOL";
                url = new Uri(mycontroller.mySettings.Get_DOL_path(), UriKind.Absolute).AbsoluteUri;
                timer_UI_update_wait.Start();
            }
        }

        async void CheckDOLState()
        {

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    CheckDOLState();
                });
            }
            else
            {
                //window.alert(document.getElementById('replaceAction').textContent)

                //timer_DOL_vibrate.Start();
                string script = string.Format("document.getElementById('replaceAction').textContent;"); 
                await chromiumWebBrowser1.EvaluateScriptAsync(script).ContinueWith(x =>
                 {
                     var myresponse = x.Result;

                     if (myresponse.Success && myresponse.Result != null || debug)
                     {
                         //MessageBox.Show(myresponse.Result.ToString());
                         //SetStatus((string)myresponse.Result);
                         playingDOL = true;
                         //if (!timer_DOL_vibrate.Enabled) timer_DOL_vibrate.Start();

                         SetStrengthDOL();
                         //SetStrengthDOL();

                     }
                     else
                     {
                         if (playingDOL == true)
                         {
                             elapsedTime.Stop();
                             timer_DOL_vibrate.Stop();
                             SetStrength(0);
                         }
                     }
                 });
            }
        }

        void SetStrengthDOL()
        {// call SetStrength("value") where value is calculated based on settings.
            /*
            *      min              ........             max
            * totalelapse = 0       .........  totalelapse >= cyclespan
            * 
            *  cycle 1000
            * elapsed 100
            * percent 10
            * 
            */

            if (elapsedTime.IsRunning == false)// this is the first moment of the sex scene, start at min.
            {
                SetStrength(mycontroller.mySettings.Get_DOL_vib_min());
                elapsedTime.Start();
                return;
            }

            cycleSpan = new TimeSpan(0, 0, mycontroller.mySettings.Get_DOL_vib_cycle());
            if (patternDirectionForward == true)
            {
                totalElapsedTime += elapsedTime.Elapsed;
            }
            else
            {
                totalElapsedTimeReverse += elapsedTime.Elapsed;
            }
            totalElapsePercent = Convert.ToInt64((totalElapsedTime.TotalMilliseconds - totalElapsedTimeReverse.TotalMilliseconds) / cycleSpan.TotalMilliseconds * 100);


            if (totalElapsePercent >= 100)
            {
                SetStrength(mycontroller.mySettings.Get_DOL_vib_max());
                totalElapsedTime = cycleSpan;
                patternDirectionForward = false;
            }
            else
            {
                if (totalElapsePercent > 0)
                {

                    SetStrength
                    (
                        (
                        mycontroller.mySettings.Get_DOL_vib_min() +
                        (
                            (
                            mycontroller.mySettings.Get_DOL_vib_max() - mycontroller.mySettings.Get_DOL_vib_min()
                            ) *
                            Convert.ToInt32(totalElapsePercent) / 100
                        )
                        )
                    );
                    //SetStrength(Convert.ToInt32(totalElapsePercent).ToString());
                }
                else
                {//we're going into negative % so cycle is complete. time to reset values for next cycle.
                    SetStrength(mycontroller.mySettings.Get_DOL_vib_min());
                    totalElapsedTime = new TimeSpan(0, 0, 0);
                    totalElapsedTimeReverse = totalElapsedTime;
                    patternDirectionForward = true;
                }
            }
            /*
            if (debug)
            {
                SetStatus("_____" +
                     "\nElapsedTime: " + elapsedTime.Elapsed.TotalMilliseconds +
                    "\ntotalElapsedTime: " + totalElapsedTime.TotalMilliseconds +
                    "\ntotalElapsedTimeReverse: " + totalElapsedTimeReverse.TotalMilliseconds +
                    "\ncycleSpan: " + cycleSpan.TotalMilliseconds +
                    "\npatternDirectionForward: " + patternDirectionForward +
                    "\ntotalElapsePercent: " + totalElapsePercent +
                    "\n___________");
            }*/
            elapsedTime.Start();
        }
        private async void timer_DOL_Tick(object sender, EventArgs e)
        {
            if (selectedTab == 1)
            {
                CheckDOLState();
            }
            else
            {
                if (playingDOL)
                {
                    playingDOL = false;
                    SetStrength(0);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl1.SelectedIndex;
        }

        private void textBox_DOL_min_str_TextChanged(object sender, EventArgs e)
        {
            if (recursive) return;
            int temp = 0;
            if (!loadingSettingsFromFile)
            {
                try
                {
                    temp = Convert.ToInt32(textBox_DOL_min_str.Text);
                }
                catch
                {
                    MessageBox.Show("Value must be a number.");
                    recursive = true;
                    textBox_DOL_min_str.Text = mycontroller.mySettings.Get_DOL_vib_min().ToString();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_DOL_vib_min(temp);
            }
        }

        private void textBox_DOL_max_str_TextChanged(object sender, EventArgs e)
        {
            if (recursive) return;
            int temp = 0;
            if (!loadingSettingsFromFile)
            {
                try
                {
                    temp = Convert.ToInt32(textBox_DOL_max_str.Text);
                }
                catch
                {
                    MessageBox.Show("Value must be a number.");
                    recursive = true;
                    textBox_DOL_max_str.Text = mycontroller.mySettings.Get_DOL_vib_max().ToString();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_DOL_vib_max(temp);
            }
        }

        private void textBox_DOL_cycle_TextChanged(object sender, EventArgs e)
        {
            if (recursive) return;
            int temp = 0;
            if (!loadingSettingsFromFile)
            {
                try
                {
                    temp = Convert.ToInt32(textBox_DOL_cycle.Text);
                }
                catch
                {
                    MessageBox.Show("Value must be a number.");
                    recursive = true;
                    textBox_DOL_cycle.Text = mycontroller.mySettings.Get_DOL_vib_cycle().ToString();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_DOL_vib_cycle(temp);
            }
        }

        private void timer_DOL_vibrate_Tick(object sender, EventArgs e)
        {
            SetStrengthDOL();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);//prevent form being disposed before control thread ends resulting in error.
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            // Define the base URL where Intiface is running
            string intifaceBaseUrl = "http://localhost:12345"; // Adjust the URL and port as needed

            // Define the index of the toy you want to control (e.g., 0 for the first toy)
            int toyIndex = 0;

            // Set the intensity level (from 0 to 100)
            int intensity = 50;

            using (HttpClient httpClient = new HttpClient())
            {
                // Create a function to control the toy by index
                async Task ControlToyAsync(int index)
                {
                    string command = $"/devices/{index}/vibrate?intensity={intensity}";
                    HttpResponseMessage response = await httpClient.PostAsync(new Uri(intifaceBaseUrl + command), null);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Toy {index} vibrating at {intensity}% intensity.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to control Toy {index}: {response.StatusCode}");
                    }
                }

                // Call the ControlToyAsync function for the specified toy index
                await ControlToyAsync(toyIndex);
            }
        }

        private void radioButton_offline_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_Browse_Click(object sender, EventArgs e)
        {

        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            client.DisconnectAsync();
            SetStatus("Disconnected");
        }

        private async void button_Connect_Click(object sender, EventArgs e)
        {
            SetStatus("Connecting...");
            try
            {
                myConnector = new ButtplugWebsocketConnector(new Uri("ws://" + mycontroller.mySettings.Get_IP() + ":" + mycontroller.mySettings.Get_Port()));
            }
            catch
            {
                MessageBox.Show("IP or Port not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isConnected = await Connect_InitFace();
            if (isConnected == true)
            {
                //quick and dirty set all connected devices to generic list. need to implement name based separation per user preference later
                devicesGenericTemp = client.Devices.ToList();
                SetStatus("Connected");
            }
            else
            {
                SetStatus("Disconnected");
            }            
        }

        async Task<bool> Connect_InitFace()
        {
            if (!client.Connected)
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromSeconds(5));
                    await client.ConnectAsync(myConnector, cts.Token);

                    // If successful extend the timeout
                    // See https://github.com/buttplugio/buttplug-csharp/issues/685
                    cts.CancelAfter(TimeSpan.FromDays(5));
                    return client.Connected;
                }
                catch (ButtplugClientConnectorException ex)
                {
                    MessageBox.Show("Can't connect to Buttplug Server, exiting!" + $"Message: {ex.InnerException.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus("Disconnected");
                }
                catch (ButtplugHandshakeException ex)
                {
                    MessageBox.Show("Mismatch between client (this app) and server (Initface Central) version." + $"Message: {ex.InnerException.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus("Disconnected");
                }
                catch (OperationCanceledException)
                {
                    // Handle the cancellation case
                    MessageBox.Show("Connection attempt timed out.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SetStatus("Disconnected");
                }
            }
            return false; // Connection unsuccessful
        }

    

    private void textBox_buttplug_port_TextChanged(object sender, EventArgs e)
        {
            if (recursive) return;
            int temp = 0;
            if (!loadingSettingsFromFile)
            {
                try
                {
                    temp = Convert.ToInt32(textBox_buttplug_port.Text);
                }
                catch
                {
                    MessageBox.Show("Value must be a number.");
                    recursive = true;
                    textBox_buttplug_port.Text = mycontroller.mySettings.Get_Port();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_Port(temp.ToString());
            }
        }

        private void textBox_buttplug_IP_TextChanged(object sender, EventArgs e)
        {
            if (recursive) return;
            string temp = "";
            if (!loadingSettingsFromFile)
            {
                temp = textBox_buttplug_IP.Text;

                mycontroller.mySettings.Set_IP(textBox_buttplug_IP.Text);
            }
        }

        private void radioButton_Online_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void button_save_Settings_Click(object sender, EventArgs e)
        {
            if (mycontroller.mySettings.Get_DOL_vib_max() < mycontroller.mySettings.Get_DOL_vib_min())
            {
                MessageBox.Show("Max vibration must be greater or equal to min vibration.\nSettings not saved.");
                return;
            }
            mycontroller.SaveSettingsFile();
            LoadSettingsVar();
            MessageBox.Show("Settings saved.");
        }

        private void timer_UI_update_wait_Tick(object sender, EventArgs e)
        {
            switch (UI_update_action)
            {
                case "load DOL":
                    {
                        //url = new Uri("https://www.google.com/", UriKind.Absolute).AbsoluteUri;

                        chromiumWebBrowser1.Load(url);
                        Open_DOL_button.Visible = false;

                        chromiumWebBrowser1.LoadingStateChanged += (sender2, args) =>
                        {
                            //Wait for the Page to finish loading
                            if (args.IsLoading == false)
                            {
                                timer_DOL_check.Start();
                            }
                        };
                        break;
                    }
            }
            timer_UI_update_wait.Stop();

        }

    }
}
















