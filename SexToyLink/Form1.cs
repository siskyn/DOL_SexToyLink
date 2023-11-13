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
using CefSharp.DevTools.Page;
using System.Security.Policy;
using CefSharp.DevTools.Network;

namespace SexToyLink
{
    public partial class Form1 : Form
    {
        bool debug = false;
        bool CombatDetected = false;

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
        public 
        volatile Controller mycontroller = new Controller();
        bool loadingSettingsFromFile = true;
        bool recursive = false;
        volatile Stopwatch elapsedTime;
        TimeSpan totalElapsedTime, cycleSpan, totalElapsedTimeReverse;
        long totalElapsePercent = 0;
        bool patternDirectionForward = true;
        bool playingDOL = false;
        float vibrationCurrentIntensity = 0;
        bool haveAnyDevice = false;

        public Form1()
        {
            InitializeComponent();
            if (debug) richTextBox1.BringToFront();
            CefSettings settings = new CefSettings();
            settings.LogSeverity = LogSeverity.Disable;
            //settings.PackLoadingDisabled = true;
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);
            elapsedTime = new Stopwatch();
            totalElapsedTime = new TimeSpan(0, 0, 0);
            totalElapsedTimeReverse = new TimeSpan(0, 0, 0);
        }

        void LoadSettings()
        {
            try
            {
                //checkBox_remember_DOL_path.Checked = mycontroller.mySettings.Get_remember_DOL_path();
                textBox_DOL_min_str.Text = mycontroller.mySettings.Get_DOL_vib_min().ToString();
                textBox_DOL_max_str.Text = mycontroller.mySettings.Get_DOL_vib_max().ToString();
                textBox_DOL_cycle.Text = mycontroller.mySettings.Get_DOL_vib_cycle().ToString();
                textBox_buttplug_port.Text = mycontroller.mySettings.Get_Port().ToString();
                textBox_buttplug_IP.Text = mycontroller.mySettings.Get_IP().ToString();
                textBox_Filepath.Text = mycontroller.mySettings.Get_DOL_path_offline().ToString();
                textBox_WebAddress.Text = mycontroller.mySettings.Get_DOL_path_online().ToString();

                if (mycontroller.mySettings.Get_GameSource() == "online")
                {
                    radioButton_Online.Checked = true;
                    radioButton_offline.Checked = false;
                }
                else
                {
                    radioButton_offline.Checked = true;
                    radioButton_Online.Checked = false;
                }
                loadingSettingsFromFile = false;
            }
            catch(Exception)
            {
                MessageBox.Show(this, "Settings file loaded but values are unexpected. Old settings file may not be compatible with new app version. Resetting to default values.");
                mycontroller.ResetSettings();
            }
        }

            void LoadCycleSpan()
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
            MessageBox.Show(this, "A device was removed while connected. This is not yet handled. Disconnecting to prevent issues. Please reconnect.");
            client.DisconnectAsync();
        }


        void SetStrength(float newValue)
        {



            // MessageBox.Show(this, "str changed to " + text);
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
            UpdateVibration();
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
                this.Text = "Sex Toy Link: " + text;
            }
        }



        async Task UpdateVibration()
        {
            //devicesgenerictemp contains our devices. let's set them all up
            //to do: see how multiple devices are handled and basically vibrate em all to the vibrationCurrentIntensity / 100f value.
            //continue here.
            if(!client.Connected) { return; }

            if (devicesVaginal.Count != 0)
            {//to do: vibrate vaginal device(s)
                haveAnyDevice = true;

            }
            if (devicesAnal.Count != 0)
            {//to do: vibrate anal device(s)
                haveAnyDevice = true;
            }
            if (devicesOral.Count != 0)
            {//to do: vibrate oral device(s)
                haveAnyDevice = true;

            }
            if (devicesGenericTemp.Count != 0)
            {//temp vibrate all devices. later will split to vaginal/anal/oral.
                haveAnyDevice = true;

                //future to do: detect the kind of activity (vibrate or something else) the devices can do, and then do it. For now we'll stick to assuming everything vibrates.
                /*
                 * Console.WriteLine($"{device.Name} supports vibration: ${device.VibrateAttributes.Count > 0}");
                if (device.VibrateAttributes.Count > 0)
                {
                   Console.WriteLine($" - Number of Vibrators: {device.VibrateAttributes.Count}");
                }
                */



                

                foreach (var device in devicesGenericTemp)
                {
                    //Console.WriteLine($"- {device.Name}");
                    try
                    {//let the vibration.... begin.
                        await device.VibrateAsync(vibrationCurrentIntensity / 100f); //This version sets all of the motors on a vibrating device to the same speed.
                    }
                    catch(ButtplugClientConnectorException e)
                    {
                        devicesGenericTemp.Remove(device);
                        MessageBox.Show("Device \"" + device.Name + "\" is no longer connected, we'll stop trying to control it. To control it again, add it to Initiface Central again, then disconnect and reconnect ToyLink to Intiface Central.");
                    }

                }
            }
            if (!haveAnyDevice)
            {
                elapsedTime.Stop();//stop the timer that decides how strong the next vibration should be                    
                timer_DOL_vibrate.Stop();//stop the timer that updates vibration
                vibrationCurrentIntensity = 0;//stop vibration
                client.DisconnectAsync();
                SetStatus("Disconnected");
                MessageBox.Show("No devices are connected to intiface central. Stopping vibration.");                
            }
            
        }

            private void button_Play_Click(object sender, EventArgs e)
        {
            mycontroller.StartGame(this);
            /*
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
            }*/
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
            {//now to do the actual checking.
                CombatDetected = false;
                string script;
                
                //First search directly in the HTML body. This will detect when running offline, or a player's own hosting.
                script = string.Format("document.getElementById('replaceAction').textContent;");
                await chromiumWebBrowser1.EvaluateScriptAsync(script).ContinueWith(x =>
                 {
                     var myresponse = x.Result;

                     if (myresponse.Success && myresponse.Result != null)
                     {
                         CombatDetected = true;
                         combat_Detected();
                     }
                 });


                //skip trying to check inside Iframe because so far we can't so instead we use a workaround where if loading from dolmods, we load the iframe directly, so the search above will detecte it.
                /*
               // if (CombatDetected == false)
               // {
                    //Then search inside a very specific IFrame in case we're playing on dolmods.net. directly in the HTML body. This will detect when running offline, or a player's own hosting.
                    // Get the main frame
                    var mainFrame = chromiumWebBrowser1.GetMainFrame();

                // Try finding the desired frame by name
                try
                {
                    var subframes = chromiumWebBrowser1.GetBrowser().GetFrameIdentifiers();
                    script = string.Format("document.getElementById('replaceAction').textContent;");
                    foreach (var myframe in subframes)
                    {
                       // await myframe. EvaluateScriptAsync(script);


                    }
                    var desiredFrame = chromiumWebBrowser1.GetBrowser().GetFrameIdentifiers()
                        .Select(frameId => chromiumWebBrowser1.GetBrowser().GetFrame(frameId))
                        .FirstOrDefault(f => f.Name == "mod-content");
               
                    // If found, use the desired frame; otherwise, use the main frame
                    var frame = desiredFrame ?? mainFrame;
                    //frame = frame.GetChildFrames().FirstOrDefault(f => f.Name == "mod-content") ?? frame;

                     script = @"
                            var iframe = document.getElementById('mod-content');
                            if (iframe) {
                                var innerDoc = iframe.contentDocument || iframe.contentWindow.document;
                                var elementExists = innerDoc.getElementById('replaceAction') !== null;
                                elementExists;
                            } else {
                                false;
                            }
                        ";

                    script = @"
                                function checkElementExists() {
                                    // Check in the main HTML body
                                    var mainElement = document.getElementById('replaceAction');

                                    if (mainElement) {
                                        return true;
                                    }

                                    // Check in all frames
                                    var frames = document.getElementsByTagName('iframe');
                                    for (var i = 0; i < frames.length; i++) {
                                        var frameDoc = frames[i].contentDocument || frames[i].contentWindow.document;
                                        var frameElement = frameDoc.getElementById('replaceAction');
                                        if (frameElement) {
                                            return true;
                                        }
                                    }

                                    // Element not found in the main body or any frame
                                    return false;
                                }

                                checkElementExists();
                            ";

                    var myresponse = await chromiumWebBrowser1.EvaluateScriptAsync(script);
                    if (myresponse.Success)
                    {
                        bool elementExists = (bool)myresponse.Result;
                        if (elementExists)
                        {
                            CombatDetected = true;
                            combat_Detected();
                        }
                    }
                    
                    /*
                        await chromiumWebBrowser1.EvaluateScriptAsync(script).ContinueWith(x =>
                    //await chromiumWebBrowser1.EvaluateScriptAsync(script, null, frame.Identifier).ContinueWith(x =>
                    {
                        var myresponse = x.Result;

                        if (myresponse.Success && myresponse.Result != null || debug)
                        {
                            CombatDetected = true;
                            combat_Detected();
                        }
                    });
                    // }
                }
                catch (Exception e)//if it throws an error, the page is not loaded. most likely we hit reload game while playing. It will stop throwing it once it finishes loading.
                {
                }*/

                if (CombatDetected == false && playingDOL == true)
                {
                    combat_NOT_Detected();
                }




                }
        }

        void combat_Detected()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    combat_Detected();
                });
            }
            else
            {
                if (!timer_DOL_vibrate.Enabled) timer_DOL_vibrate.Start();
            }
        }

        void combat_NOT_Detected()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    combat_NOT_Detected();
                });
            }
            else
            {
                elapsedTime.Stop();
                timer_DOL_vibrate.Stop();
                SetStrength(0);
            }
        }

        void UpdateVibrationWhilePlaying()
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
            if (selectedTab == 0)
            {
                playingDOL = true;
                if(haveAnyDevice) CheckDOLState();
            }
            else
            {
                if (playingDOL)
                {
                    playingDOL = false;
                    elapsedTime.Stop();//stop the timer that decides how strong the next vibration should be                    
                    timer_DOL_vibrate.Stop();//stop the timer that updates vibration
                    SetStrength(0);//stop vibration
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl1.SelectedIndex;
        }

        private void textBox_DOL_min_str_TextChanged(object sender, EventArgs e)
        {/*
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
                    MessageBox.Show(this, "Value must be a number.");
                    recursive = true;
                    textBox_DOL_min_str.Text = mycontroller.mySettings.Get_DOL_vib_min().ToString();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_DOL_vib_min(temp);
            }*/
        }

        private void textBox_DOL_max_str_TextChanged(object sender, EventArgs e)
        {/*
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
                    MessageBox.Show(this, "Value must be a number.");
                    recursive = true;
                    textBox_DOL_max_str.Text = mycontroller.mySettings.Get_DOL_vib_max().ToString();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_DOL_vib_max(temp);
            }*/
        }

        private void textBox_DOL_cycle_TextChanged(object sender, EventArgs e)
        {/*
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
                    MessageBox.Show(this, "Value must be a number.");
                    recursive = true;
                    textBox_DOL_cycle.Text = mycontroller.mySettings.Get_DOL_vib_cycle().ToString();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_DOL_vib_cycle(temp);
            }*/
        }

        private void timer_DOL_vibrate_Tick(object sender, EventArgs e)
        {
            UpdateVibrationWhilePlaying();
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
            mycontroller.mySettings.Set_GameSource("offline");
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "HTML Files (*.html)|*.html|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1; // Default to HTML files
            openFileDialog.Title = "Select HTML File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_Filepath.Text = openFileDialog.FileName;
            }
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            client.DisconnectAsync();
            SetStatus("Disconnected");
        }

        private async void button_Connect_Click(object sender, EventArgs e)
        {
            haveAnyDevice = false;
            SetStatus("Connecting...");
            try
            {
                myConnector = new ButtplugWebsocketConnector(new Uri("ws://" + mycontroller.mySettings.Get_IP() + ":" + mycontroller.mySettings.Get_Port()));
            }
            catch
            {
                MessageBox.Show(this, "IP or Port not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isConnected = await Connect_InitFace();
            if (isConnected == true)
            {
                //quick and dirty set all connected devices to generic list. need to implement name based separation per user preference later
                devicesGenericTemp = client.Devices.ToList();
                if (devicesGenericTemp.Count > 0)
                {
                    haveAnyDevice = true;
                    controlActive = true;
                    SetStatus("Connected");
                }
                else
                {
                    MessageBox.Show("Connection succesful, but no devices detected. Please connect your devices to Intiface Central first. Disconnecting.");
                    SetStatus("Disconnected");
                }
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
                    MessageBox.Show(this, "Can't connect to Buttplug Server, exiting!" + $"Message: {ex.InnerException.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus("Disconnected");
                }
                catch (ButtplugHandshakeException ex)
                {
                    MessageBox.Show(this, "Mismatch between client (this app) and server (Initface Central) version." + $"Message: {ex.InnerException.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus("Disconnected");
                }
                catch (OperationCanceledException)
                {
                    // Handle the cancellation case
                    MessageBox.Show(this, "Connection attempt timed out.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SetStatus("Disconnected");
                }
            }
            else
            {
                return true;//we were already connected. no need to do anything.
            }
            return false; // Connection unsuccessful
        }

    

    private void textBox_buttplug_port_TextChanged(object sender, EventArgs e)
        {/*
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
                    MessageBox.Show(this, "Value must be a number.");
                    recursive = true;
                    textBox_buttplug_port.Text = mycontroller.mySettings.Get_Port();
                    recursive = false;
                    return;
                }
                mycontroller.mySettings.Set_Port(temp.ToString());
            }*/
        }

        private void textBox_buttplug_IP_TextChanged(object sender, EventArgs e)
        {/*
            if (recursive) return;
            string temp = "";
            if (!loadingSettingsFromFile)
            {
                temp = textBox_buttplug_IP.Text;

                mycontroller.mySettings.Set_IP(textBox_buttplug_IP.Text);
            }*/
        }

        private void textBox_WebAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Filepath_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_Online_CheckedChanged(object sender, EventArgs e)
        {
            mycontroller.mySettings.Set_GameSource("online");
        }

        private void button_game_reload_Click(object sender, EventArgs e)
        {
            mycontroller.StartGame(this);
        }

        private async void chromiumWebBrowser1_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {//check if we're laoding from dolmods, and if yes, load the target of the iframe rather than the original page as a workaround to the iframe of death (can't read inside it)
            //MessageBox.Show("loaded");

            
            var script = string.Format("document.getElementById('mod-content').src;");
            await chromiumWebBrowser1.EvaluateScriptAsync(script).ContinueWith(x =>
            {
                var myresponse = x.Result;

                if (myresponse.Success && myresponse.Result != null)
                {//time to load the content of the iframe instead.
                    //MessageBox.Show(myresponse.Result.ToString());
                    var url = new Uri(myresponse.Result.ToString(), UriKind.Absolute).AbsoluteUri;
                    chromiumWebBrowser1.Load(url);
                }
            }); 

        }

        private void button_save_Settings_Click(object sender, EventArgs e)
        {
            if (mycontroller.SaveSettings(this) == true)
            {
                LoadCycleSpan();
                MessageBox.Show(this, "Settings saved.");
            }
        }

        private void timer_UI_update_wait_Tick(object sender, EventArgs e)
        {
            switch (mycontroller.UI_update_action)
            {
                case "load DOL":
                    {
                        //url = new Uri("https://www.google.com/", UriKind.Absolute).AbsoluteUri;

                        chromiumWebBrowser1.Load(mycontroller.url);
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
















