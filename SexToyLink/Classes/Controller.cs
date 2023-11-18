using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using Buttplug.Core;
using CefSharp;
using CefSharp.WinForms;
using SexToyLink.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;


namespace SexToyLink.Classes
{
    public class Controller
    {
        Form_DebugConsole myConsole;
        public bool debug = false;
        public volatile string UI_update_action = "", url = "";
        volatile bool CombatDetected = false;
        private TaskCompletionSource<bool> stopScanClickWaitTask;
        delegate void SetTextCallback(string text);
        delegate void SetStatusCallback(Form_Main myForm, string text);
        ButtplugWebsocketConnector myConnector;
        public ButtplugClient client = new ButtplugClient("SexToyLink Client");
        public List<ButtplugClientDevice> devicesOral = new List<ButtplugClientDevice>();
        public List<ButtplugClientDevice> devicesGenital = new List<ButtplugClientDevice>();
        public List<ButtplugClientDevice> devicesAnal = new List<ButtplugClientDevice>();
        public List<ButtplugClientDevice> devicesBreasts = new List<ButtplugClientDevice>();
        public List<ButtplugClientDevice> devicesUncategorized = new List<ButtplugClientDevice>();
        public List<ButtplugClientDevice> devicesAll = new List<ButtplugClientDevice>();
        public volatile bool controlActive = false;
        public volatile int selectedTab = 0;
        public volatile bool loadingSettingsFromFile = true;
        public volatile bool recursive = false;
        public volatile Stopwatch elapsedTime;
        public TimeSpan totalElapsedTime, cycleSpan, totalElapsedTimeReverse;
        public volatile int totalElapsePercent = 0;
        public volatile bool patternDirectionForward = true;
        public volatile bool playingDOL = false;
        public volatile float vibrationCurrentIntensity = 0;
        public volatile bool haveAnyDevice = false;
        bool oralAction, breastAction, genitalAction, analAction;
        List<string> oralTriggers, breastTriggers, genitalTriggers, analTriggers;
        List<ButtplugClientDevice> updatedDevices = new List<ButtplugClientDevice>();
        public Settings mySettings = new Settings();
        string settingsPath = Application.StartupPath + "\\settings";


        public Controller()
        {
            CefSettings settings = new CefSettings();
            settings.LogSeverity = LogSeverity.Disable;
            //settings.PackLoadingDisabled = true;
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);
            elapsedTime = new Stopwatch();
            totalElapsedTime = new TimeSpan(0, 0, 0);
            totalElapsedTimeReverse = new TimeSpan(0, 0, 0);
            LoadTriggers();
            try
            {
                LoadSettingsFromFile();
            }
            catch { }

            //If built as a debug build, automatically open debug console.
#if DEBUG
            toggleDebug(true);
#else
                toggleDebug(false);
#endif
        }

        void LoadTriggers()
        {
            oralTriggers = new List<string>();
            breastTriggers = new List<string>();
            genitalTriggers = new List<string>();
            analTriggers = new List<string>();

            //regex patterns:
            //match up to 3 words               \w+(\s+\w+){0,2}

            string ass = @"\b(?:ass|bum|butt|anus)\b";
            string optionalAdjective = @"(?:\w+\s+)?";//will also match whitespace after
            string optionalDeeper = @"(?:deeper\s+)?";//will also match whitespace after
            string penis = @"\b(?:penis|shaft|glans)\b";
            string pussy = @"\b(?:pussy|pussy)\b";
            string her = @"\b(?:his|her)\b";
            string attackerBodypart = @"\b(?:ass|bum|butt|anus|pussy)\b";

            oralTriggers.Add(@" / down your throat/");
            oralTriggers.Add(@"/presses your lips together in a kiss/");
            oralTriggers.Add(@"/continues to push your mouth against/");
            oralTriggers.Add(@"/pushes your mouth against/");
            oralTriggers.Add(@"/struggles to keep your mouth latched to/");
            oralTriggers.Add(@"/breaths are quick and heavy, jostling your head against/");
            oralTriggers.Add(@"/against your lips/");
            oralTriggers.Add(@"/mouth with "+her+@" tongue/");
            oralTriggers.Add(@"/between your lips/");
            oralTriggers.Add(@"/explores your mouth/");
            




            breastTriggers.Add(@"/kisses and caresses your/");
            breastTriggers.Add(@"/tongue against your/");
            breastTriggers.Add(@"/fingers lingering around your/");




            genitalTriggers.Add(@"/your labia with the/");//exact
            genitalTriggers.Add(@"/gives your clitoris a tweak./");//exact
            genitalTriggers.Add(@"/while toying with your "+pussy+@"/");
            genitalTriggers.Add(@"/rubs your "+penis+@"./");
            genitalTriggers.Add(@"/fingers around your "+penis+@"./");
            genitalTriggers.Add(@"/the length of your/");
            genitalTriggers.Add(@"/hand onto your "+penis+@"./");
            genitalTriggers.Add(@"/teases your "+penis+@" with the/");
            genitalTriggers.Add(@"/while toying with your "+pussy+@"/");
            genitalTriggers.Add(@"/your labia with the/");
            genitalTriggers.Add(@"/your "+penis+@" with the/");
            genitalTriggers.Add(@"/pulsing around your length/");
            genitalTriggers.Add(@"/violates your "+optionalAdjective+penis+@"/");
            genitalTriggers.Add(@"/"+penis+@" is hungrily devoured by/");//exact
            genitalTriggers.Add(@"/kneading and squeezing your length/");
            genitalTriggers.Add(@"/"+penis+@" with "+her+@" pelvis as/");//penis with [her] pelvis as
            genitalTriggers.Add(@"/"+penis+ @" with "+her+" "+attackerBodypart+@"/");
            genitalTriggers.Add(@"/as you pound "+her+@" ass/"); //as you pound [her] ass
            genitalTriggers.Add(@"/rubbing "+her+attackerBodypart+@" against your "+optionalAdjective+@"penis/");
            genitalTriggers.Add(@"/"+penis+@" between "+her+@" cheeks/");
            genitalTriggers.Add(@"/"+penis+@" "+optionalDeeper+"into "+her+" "+attackerBodypart+@"/");
            genitalTriggers.Add(@"/rhythmically pounding your length./");
            genitalTriggers.Add(@"/continues to violate your " + optionalAdjective+optionalAdjective+penis+"/");
            genitalTriggers.Add(@"/twitch around your length as/");


            

            analTriggers.Add(@"/teases your "+optionalAdjective+ass+@" with/");
            analTriggers.Add(@"/slaps your "+optionalAdjective+ass+@" and/");      
            analTriggers.Add(@"/in and out of your "+optionalAdjective+ass+@"/");            
            analTriggers.Add(@"/"+ass+@", giving no regard to your comfort/");            
            analTriggers.Add(@"/thrusting into your "+optionalAdjective+ass+@"/");
            analTriggers.Add(@"/"+ass+@" with increasing power/");
            analTriggers.Add(@"/caresses your  "+optionalAdjective+ass+@"/");
            analTriggers.Add(@"/into your  "+optionalAdjective+ass+@"/");
            analTriggers.Add(@"/violate your "+optionalAdjective+ass+@"/");
            analTriggers.Add(@"/fucks your "+optionalAdjective+ass+@"/");
            

        }

        public void toggleDebug()
        {

            if (debug)
            {
                debug = false;
                myConsole.closeConsole();
            }
            else
            {
                debug = true;
                myConsole = new Form_DebugConsole(this);
                myConsole.Show();
            }
        }
        public void toggleDebug(bool newState)
        {
            if (!newState)
            {
                debug = false;
                try
                {
                    myConsole.closeConsole();
                }
                catch (Exception e) { }
            }
            else
            {
                debug = true;
                myConsole = new Form_DebugConsole(this);
                myConsole.Show();
            }
        }

        public void UpdateDeviceCategories(List<string> oral, List<string> breast, List<string> genital, List<string> anal)
        {
            mySettings.memorizedDevicesList.Clear();
            devicesOral.Clear();
            devicesBreasts.Clear();
            devicesGenital.Clear();
            devicesAnal.Clear();
            updateSingleDeviceCategory(oral, devicesOral, "oral");
            updateSingleDeviceCategory(breast, devicesBreasts, "breasts");
            updateSingleDeviceCategory(genital, devicesGenital, "genital");
            updateSingleDeviceCategory(anal, devicesAnal, "anal");
            oralAction = oralAction;
            oralAction = oralAction;

        }

        void updateSingleDeviceCategory(List<string> deviceIDs, List<ButtplugClientDevice> devicesList, string bodyPart)
        {
            bool found;
            bool toBreak = false;
            foreach (string deviceID in deviceIDs)
            {
                foreach (var device in devicesAll)
                {
                    if (deviceID == device.Index.ToString())// should check device.DisplayName but using index for now due to buttplug IO bug where it remains null
                    {
                        devicesList.Add(device);
                        mySettings.memorizedDevicesList.Add(new MemorizedDevice(deviceID, bodyPart));
                        break;
                    }
                }
            }
        }

        public void timer_UI_update_wait_Tick(Form_Main myForm)
        {
            switch (UI_update_action)
            {
                case "load DOL":
                    {
                        //url = new Uri("https://www.google.com/", UriKind.Absolute).AbsoluteUri;

                        myForm.chromiumWebBrowser1.Load(url);
                        myForm.Open_DOL_button.Visible = false;

                        myForm.chromiumWebBrowser1.LoadingStateChanged += (sender2, args) =>
                        {
                            //Wait for the Page to finish loading
                            if (args.IsLoading == false)
                            {
                                myForm.timer_DOL_check.Start();
                            }
                        };
                        break;
                    }
            }
            myForm.timer_UI_update_wait.Stop();
        }

        public async void browser_page_loaded(Form_Main myForm)
        {
            //check if we're laoding from dolmods, and if yes, load the target of the iframe rather than the original page as a workaround to the iframe of death (can't read inside it)
            //MessageBox.Show("loaded");


            var script = string.Format("document.getElementById('mod-content').src;");
            await myForm.chromiumWebBrowser1.EvaluateScriptAsync(script).ContinueWith(x =>
            {
                var myresponse = x.Result;

                if (myresponse.Success && myresponse.Result != null)
                {//time to load the content of the iframe instead.
                    //MessageBox.Show(myresponse.Result.ToString());
                    var url = new Uri(myresponse.Result.ToString(), UriKind.Absolute).AbsoluteUri;
                    myForm.chromiumWebBrowser1.Load(url);
                }
            });
        }

        public void Disconnect(Form_Main myForm)
        {
            client.DisconnectAsync();
            SetStatus(myForm, "Disconnected");
        }

        public void button_Browse_Click(Form_Main myForm)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "HTML Files (*.html)|*.html|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1; // Default to HTML files
            openFileDialog.Title = "Select HTML File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                myForm.textBox_Filepath.Text = openFileDialog.FileName;
            }
        }

        public void HandleDeviceAdded(object aObj, DeviceAddedEventArgs aArgs)
        {//this gets trigered imediately after connecting, giving us a list of all devices. But might not need it? client.devices might be automatically updated.
         //Console.WriteLine($"Device {aDeviceEventArgs.Device.Name} Connected!");
         //devicesGenericTemp.Add 
         //devicesVaginal
         //devicesAnal
         //devicesOral

            //Oooorrrrr be super lazy and require the player to have the toys already connected to Intiface before connecting the app to Intiface, and we can implement this later.
        }

        public void HandleDeviceRemoved(object aObj, DeviceRemovedEventArgs aArgs)
        {
            MessageBox.Show("A device was removed while connected. This is not yet handled. To use it again, reconnect device to Intiface Central, then reconnect this app to Intiface Central.");
            //client.DisconnectAsync();
        }

        async void CheckDOLState(Form_Main myForm)
        {
            if (myForm.InvokeRequired)
            {
                myForm.Invoke((MethodInvoker)delegate
                {
                    CheckDOLState(myForm);
                });
            }
            else
            {
                /*
                 * ==========functional old way of checking if in combat only
                //now to do the actual checking.
                CombatDetected = false;
                string script;

                //First search directly in the HTML body. This will detect when running offline, or a player's own hosting.
                script = string.Format("document.getElementById('replaceAction').textContent;");
                await myForm.chromiumWebBrowser1.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    var myresponse = x.Result;

                    if (myresponse.Success && myresponse.Result != null)
                    {
                        CombatDetected = true;
                        combat_Detected(myForm);
                    }
                });
                //skip trying to check inside Iframe because so far we can't so instead we use a workaround where if loading from dolmods, we load the iframe directly, so the search above will detecte it.
                
                
                if (CombatDetected == false && playingDOL == true)
                {
                    combat_NOT_Detected(myForm);
                }
                */

                oralAction = false;
                breastAction = false;
                genitalAction = false;
                analAction = false;
                if (await CheckTriggerSet(myForm, oralTriggers) == true)
                {//we have oral action
                    oralAction = true;
                    //if (debug) consoleWriteLine("Found oral trigger.");
                }
                if (await CheckTriggerSet(myForm, breastTriggers) == true)
                {//we have breast action
                    breastAction = true;
                    //if (debug) consoleWriteLine("Found breast trigger.");
                }
                if (await CheckTriggerSet(myForm, genitalTriggers) == true)
                {//we have genital action
                    genitalAction = true;
                    //if (debug) consoleWriteLine("Found genital trigger.");
                }
                if (await CheckTriggerSet(myForm, analTriggers) == true)
                {//we have anal action
                    analAction = true;
                    //if (debug) consoleWriteLine("Found anal trigger.");
                }

                if (oralAction || breastAction || genitalAction || analAction)
                {
                    //if (debug) consoleWriteLine("combat detected");
                    combat_Detected(myForm);
                }
                else
                {
                   //if (debug) consoleWriteLine("combat not detected");
                    combat_NOT_Detected(myForm);
                }

            }
        }

        void consoleWriteLine(string message)
        {
            myConsole.writeLine(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local).ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
        }

        string GetJavaScriptArray(List<string> patterns)
        {
            string result = "[" + string.Join(",", patterns.Select(p => p)) + "]";
            return result;
        }
        async Task<bool> CheckTriggerSet(Form_Main myForm, List<string> myTriggers)
        {

            //string script = $"var triggers = {Newtonsoft.Json.JsonConvert.SerializeObject(myTriggers)}; triggers.some(trigger => document.body.innerText.includes(trigger));";

            List<string> patterns = new List<string>
            {
                @"/Many \w+ you/",
                @"/word1 \w+ \w+ word3/"
            };
            string script = @"
                try
                {
                    let text = 'Many of you'; 
                    let patterns = " + GetJavaScriptArray(myTriggers) + @";
                    let patternsv2 = [/Many \w+ you/, /word1 \w+ \w+ word3/];
                    //console.log(""patterns: type is "", console.log(typeof patterns), "" and content is: "", patterns);
                    //console.log(""patternsv2: type is "", console.log(typeof patternsv2), "" and content is: "", patternsv2);
                    let found = patterns.find(pattern => new RegExp(pattern).test(document.body.innerText));
                    if(found)
                    {
                        console.log(""Found trigger:"", found.toString());
                    }
                    found ? 'found' : 'not found';
                    //'found'

                }
                catch(error)
                {
                    error.message
                }
            ";
             



            var result = await myForm.chromiumWebBrowser1.EvaluateScriptAsync(script);
            //if (debug) consoleWriteLine("script result: " + result.Result.ToString());
            if (result.Success && result.Result is string)
            {//got a result
                if (result.Result.ToString() == "found")
                {//got a trigger
                    //if (debug) consoleWriteLine("trigger found");
                    return true;
                }
                else
                {
                    if (result.Result.ToString() == "not found")
                    {//ran succesfully but no match
                        //if (debug) consoleWriteLine("trigger not found");
                        return false;
                    }
                    else
                    {//unexpected result so it's an error
                        if (debug) consoleWriteLine("Javascript error or unexpected reply. Error message: " + result.Message + " and  reply message: " + result.Result);
                    }
                }
            }
            else
            {
                if (debug) consoleWriteLine("Javascript error or unexpected reply. Error message: " + result.Message + " and  reply message: " + result.Result);
            }

            return false;
        }

        public void receivedBrowserConsoleMessage(string message)
        {
            if (debug) consoleWriteLine("Browser console: " + message);
        }
        public void Timer_DOL_Tick_Update(Form_Main myForm)
        {
            //if (debug) consoleWriteLine("Timer_DOL_Tick_Update cycle started.");
            if (selectedTab == 0)
            {
                //if (debug) consoleWriteLine("Game tab active, checking for triggers.");
                playingDOL = true;
                if (haveAnyDevice) CheckDOLState(myForm);
            }
            else
            {
                //if (debug) consoleWriteLine("Game tab not active.");
                if (playingDOL)
                {
                    playingDOL = false;
                    elapsedTime.Stop();//stop the timer that decides how strong the next vibration should be                    
                    myForm.timer_DOL_vibrate.Stop();//stop the timer that updates vibration
                    if (debug) consoleWriteLine("Switched out of game tab, stopping devices.");
                    SetStrength(myForm, 0);//stop vibration

                }
            }
            //if (debug) consoleWriteLine("Timer_DOL_Tick_Update cycle ended.");
        }

        void combat_Detected(Form_Main myForm)
        {
            
            if (myForm.InvokeRequired)
            {
                myForm.Invoke((MethodInvoker)delegate
                {
                    combat_Detected(myForm);
                });
            }
            else
            {
                CombatDetected = true;
                //if(debug) consoleWriteLine("combat detected, checking if vibrate timer is on");
                if (!myForm.timer_DOL_vibrate.Enabled)
                {
                    //if(debug) consoleWriteLine("combat detected, vibrate timer was off, starting it");
                    myForm.timer_DOL_vibrate.Start();
                }
                else
                {
                    //if(debug) consoleWriteLine("combat detected, vibrate timer was on. Let it be.");
                }
            }
        }

        void combat_NOT_Detected(Form_Main myForm)
        {
            
            if (myForm.InvokeRequired)
            {
                myForm.Invoke((MethodInvoker)delegate
                {
                    combat_NOT_Detected(myForm);
                });
            }
            else
            {
                if (CombatDetected)
                {
                    CombatDetected = false;
                    elapsedTime.Stop();
                    myForm.timer_DOL_vibrate.Stop();
                    consoleWriteLine("combat not detected, stopping vibration");
                    SetStrength(myForm, 0);
                }
            }
        }

        void SetStrength(Form_Main myForm,  float newValue)
        {
            //if (debug) consoleWriteLine("SetStrength(" + newValue.ToString() + ")");
            vibrationCurrentIntensity = newValue;
            UpdateVibration(myForm);
        }

        public void button_Save_Click(Form_Main myForm)
        {
            if (SaveSettings(myForm) == true)
            {
                LoadCycleSpan();
                MessageBox.Show(myForm, "Settings saved.");
            }
        }

        public void UpdateVibrationWhilePlaying(Form_Main myForm)
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

            //if (debug) consoleWriteLine("Starting vibration update.");
            if (elapsedTime.IsRunning == false)// this is the first moment of the sex scene, start at min.
            {
                if (debug) consoleWriteLine("Starting Scene at min vibration");
                SetStrength(myForm, mySettings.Get_DOL_vib_min());
                elapsedTime.Start();
                return;
            }
            //divided by 2 because we ramp up for duration of cycleSpan, then ramp down for duration of cycleSpan, so in effect, we use twice the time for a complete cycle
            cycleSpan = new TimeSpan(0,0, 0, 0, mySettings.Get_DOL_vib_cycle()/2); 
            
            if (patternDirectionForward == true)
            {
                totalElapsedTime += elapsedTime.Elapsed;
            }
            else
            {
                totalElapsedTimeReverse += elapsedTime.Elapsed;
            }
            totalElapsePercent = Convert.ToInt32((totalElapsedTime.TotalMilliseconds - totalElapsedTimeReverse.TotalMilliseconds) / cycleSpan.TotalMilliseconds * 100);
            //if (debug) consoleWriteLine("Pattern direction: " + patternDirectionForward + "   Time from last update: (ms): " + elapsedTime.Elapsed.TotalMilliseconds + "   Cycle progress (0-100): " + totalElapsePercent);

            if (totalElapsePercent >= 100)
            {
                SetStrength(myForm, mySettings.Get_DOL_vib_max());
                totalElapsedTime = cycleSpan;
                patternDirectionForward = false;
            }
            else
            {
                if (totalElapsePercent > 0)
                {

                    SetStrength
                    (myForm,
                        (
                        mySettings.Get_DOL_vib_min() +
                        (
                            (
                            mySettings.Get_DOL_vib_max() - mySettings.Get_DOL_vib_min()
                            ) *
                            Convert.ToInt32(totalElapsePercent) / 100
                        )
                        )
                    );
                }
                else
                {//we're going into negative % so cycle is complete. time to reset values for next cycle.
                    SetStrength(myForm, mySettings.Get_DOL_vib_min());
                    totalElapsedTime = new TimeSpan(0, 0, 0);
                    totalElapsedTimeReverse = totalElapsedTime;
                    patternDirectionForward = true;
                }
            }
            elapsedTime.Reset();
            elapsedTime.Start();
            //if (debug) consoleWriteLine("Vibration update complete");
        }

        void LoadCycleSpan()
        {
            cycleSpan = new TimeSpan(0, 0, mySettings.Get_DOL_vib_cycle());
        }

        public void LoadSettings(Form_Main myForm)
        {
            try
            {
                //checkBox_remember_DOL_path.Checked = mycontroller.mySettings.Get_remember_DOL_path();
                myForm.textBox_DOL_min_str.Text = mySettings.Get_DOL_vib_min().ToString();
                myForm.textBox_DOL_max_str.Text = mySettings.Get_DOL_vib_max().ToString();
                myForm.textBox_DOL_cycle.Text = mySettings.Get_DOL_vib_cycle().ToString();
                myForm.textBox_buttplug_port.Text = mySettings.Get_Port().ToString();
                myForm.textBox_buttplug_IP.Text = mySettings.Get_IP().ToString();
                myForm.textBox_Filepath.Text = mySettings.Get_DOL_path_offline().ToString();
                myForm.textBox_WebAddress.Text = mySettings.Get_DOL_path_online().ToString();

                if (mySettings.Get_GameSource() == "online")
                {
                    myForm.radioButton_Online.Checked = true;
                    myForm.radioButton_offline.Checked = false;
                }
                else
                {
                    myForm.radioButton_offline.Checked = true;
                    myForm.radioButton_Online.Checked = false;
                }
                loadingSettingsFromFile = false;
            }
            catch (Exception)
            {
                MessageBox.Show(myForm, "Settings file loaded but values are unexpected. Old settings file may not be compatible with new app version. Resetting to default values.");
                ResetSettings();
            }
        }

        async Task UpdateVibration(Form_Main myForm)
        {
            /* QA with Intiface Devs:
             * 
             * Q: How fast are commands pushed out?
             * A:Just assume 100ms or 10hz
             * 
             * Q: How does it handle receiving too many messages too fast?
             * A: OS usually queues things up, meaning if you send WAY too fast the OS will just queue and it can sometimes be sending for like 20-30s after you stop sending commands. 
             * Bluetooth LE also has guaranteed delivery so it'll send every. fucking. message.
             */


            if (!client.Connected) { return; }

            updatedDevices.Clear();

            if (devicesGenital.Count != 0)
            {//vibrate vaginal & penis device(s)
                haveAnyDevice = true;
                foreach (var device in devicesGenital)
                {
                    if (updatedDevices.Contains(device))
                    {//we already sent vibration on this cycle from another bodypart category. skip.
                        break;
                    }
                    try
                    {//let the vibration.... begin.
                        if (genitalAction)
                        {
                            //if (debug) consoleWriteLine("UpdateVibration>Genital>Vibrate(" + vibrationCurrentIntensity / 100f + ")");
                            updatedDevices.Add(device);
                            await device.VibrateAsync(vibrationCurrentIntensity / 100f); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                        else
                        {
                            //if (debug) consoleWriteLine("UpdateVibration>Genital>Vibrate(0)");
                            await device.VibrateAsync(0); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                    }
                    catch (ButtplugClientConnectorException e)
                    {
                        
                        removeConnectedDevice(device);
                        MessageBox.Show("Device \"" + device.Name + "\" is no longer connected, we'll stop trying to control it. To control it again, add it to Initiface Central again, then disconnect and reconnect ToyLink to Intiface Central.");
                    }
                }

            }

            if (devicesAnal.Count != 0)
            {//vibrate anal device(s)
                haveAnyDevice = true;
                foreach (var device in devicesAnal)
                {
                    if (updatedDevices.Contains(device))
                    {//we already sent vibration on this cycle from another bodypart category. skip.
                        break;
                    }
                    //Console.WriteLine($"- {device.Name}");
                    try
                    {//let the vibration.... begin.
                        if (analAction)
                        {
                            //if (debug) consoleWriteLine("UpdateVibration>Anal>Vibrate(" + vibrationCurrentIntensity / 100f + ")");
                            updatedDevices.Add(device);
                            await device.VibrateAsync(vibrationCurrentIntensity / 100f); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                        else
                        {
                            //if (debug) consoleWriteLine("UpdateVibration>Anal>Vibrate(0)");
                            await device.VibrateAsync(0); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                    }
                    catch (ButtplugClientConnectorException e)
                    {
                        devicesAll.Remove(device);
                        MessageBox.Show("Device \"" + device.Name + "\" is no longer connected, we'll stop trying to control it. To control it again, add it to Initiface Central again, then disconnect and reconnect ToyLink to Intiface Central.");
                    }
                }
            }
            if (devicesOral.Count != 0)
            {//vibrate oral device(s)
                haveAnyDevice = true;
                foreach (var device in devicesOral)
                {
                    if (updatedDevices.Contains(device))
                    {//we already sent vibration on this cycle from another bodypart category. skip.
                        break;
                    }
                    //Console.WriteLine($"- {device.Name}");
                    try
                    {//let the vibration.... begin.
                        if (oralAction)
                        {
                            //if (debug) consoleWriteLine("UpdateVibration>Oral>Vibrate(" + vibrationCurrentIntensity / 100f + ")");
                            updatedDevices.Add(device);
                            await device.VibrateAsync(vibrationCurrentIntensity / 100f); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                        else
                        {
                            //if (debug) consoleWriteLine("UpdateVibration>Genital>Vibrate(0)");
                            await device.VibrateAsync(0); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                    }
                    catch (ButtplugClientConnectorException e)
                    {
                        devicesAll.Remove(device);
                        MessageBox.Show("Device \"" + device.Name + "\" is no longer connected, we'll stop trying to control it. To control it again, add it to Initiface Central again, then disconnect and reconnect ToyLink to Intiface Central.");
                    }

                }
            }
            if (devicesBreasts.Count != 0)
            {//vibrate oral device(s)
                haveAnyDevice = true;
                foreach (var device in devicesBreasts)
                {
                    if (updatedDevices.Contains(device))
                    {//we already sent vibration on this cycle from another bodypart category. skip.
                        break;
                    }
                    //Console.WriteLine($"- {device.Name}");
                    try
                    {//let the vibration.... begin.
                        if (breastAction)
                        {
                            updatedDevices.Add(device);
                            await device.VibrateAsync(vibrationCurrentIntensity / 100f); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                        else
                        {
                            await device.VibrateAsync(0); //This version sets all of the motors on a vibrating device to the same speed.
                        }
                    }
                    catch (ButtplugClientConnectorException e)
                    {
                        devicesAll.Remove(device);
                        MessageBox.Show("Device \"" + device.Name + "\" is no longer connected, we'll stop trying to control it. To control it again, add it to Initiface Central again, then disconnect and reconnect ToyLink to Intiface Central.");
                    }

                }
            }


            if (devicesAll.Count != 0 && 1==2)//disable vibrating all.
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

                foreach (var device in devicesAll)
                {
                    if (updatedDevices.Contains(device))
                    {//we already sent vibration on this cycle from another bodypart category. skip.
                        break;
                    }
                    //Console.WriteLine($"- {device.Name}");
                    try
                    {//let the vibration.... begin.
                        await device.VibrateAsync(vibrationCurrentIntensity / 100f); //This version sets all of the motors on a vibrating device to the same speed.
                    }
                    catch (ButtplugClientConnectorException e)
                    {
                        devicesAll.Remove(device);
                        MessageBox.Show("Device \"" + device.Name + "\" is no longer connected, we'll stop trying to control it. To control it again, add it to Initiface Central again, then disconnect and reconnect ToyLink to Intiface Central.");
                    }
                }
            }


            if (!haveAnyDevice)
            {
                elapsedTime.Stop();//stop the timer that decides how strong the next vibration should be                    
                myForm.timer_DOL_vibrate.Stop();//stop the timer that updates vibration
                vibrationCurrentIntensity = 0;//stop vibration
                client.DisconnectAsync();
                SetStatus(myForm, "Disconnected");
                MessageBox.Show("No devices are connected to intiface central. Stopping vibration.");
            }

        }

        void removeConnectedDevice(ButtplugClientDevice myDevice)
        {
            devicesAll.Remove(myDevice);
            devicesOral.Remove(myDevice);
            devicesBreasts.Remove(myDevice);
            devicesAnal.Remove(myDevice);
            devicesGenital.Remove(myDevice);
        }
        async Task<bool> Connect_InitFace(Form_Main myForm)
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
                    SetStatus(myForm, "Disconnected");
                }
                catch (ButtplugHandshakeException ex)
                {
                    MessageBox.Show( "Mismatch between client (this app) and server (Initface Central) version." + $"Message: {ex.InnerException.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetStatus(myForm, "Disconnected");
                }
                catch (OperationCanceledException)
                {
                    // Handle the cancellation case
                    MessageBox.Show("Connection attempt timed out.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SetStatus(myForm, "Disconnected");
                }
            }
            else
            {
                return true;//we were already connected. no need to do anything.
            }
            return false; // Connection unsuccessful
        }

        public void button_Devices_Details_Clicked(Form_Main myfForm)
        {
            if (client.Connected)
            {
                Form_Device_Details myForm = new Form_Device_Details(this);//classes are references so any changes will propagate back, no need to return it.
                myForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please connect to Intiface Central first.");
            }
        }

        public void CategorizeDevices()
        {
            devicesOral.Clear();
            devicesAnal.Clear();
            devicesGenital.Clear();
            devicesBreasts.Clear();
            devicesUncategorized.Clear();

            bool found;
            foreach (var connectedDevice in devicesAll)
            {
                found = false;
                foreach (var memorizedDevice in mySettings.memorizedDevicesList)
                {
                    //if (connectedDevice.DisplayName == memorizedDevice.displayName)
                    if (connectedDevice.Index.ToString() == memorizedDevice.displayName)//temporarily using index until displayname bug is resolved
                    {
                        switch (memorizedDevice.bodypart)
                        {
                            case "oral":
                            {
                                devicesOral.Add(connectedDevice);
                                found = true;
                                break;
                            }
                            case "anal":
                            {
                                devicesAnal.Add(connectedDevice);
                                found = true;
                                break;
                            }
                            case "genital":
                            {
                                devicesGenital.Add(connectedDevice);
                                found = true;
                                break;
                            }
                            case "breasts":
                            {
                                devicesBreasts.Add(connectedDevice);
                                found = true;
                                break;
                            }
                            default:
                            {
                                MessageBox.Show("Something is wrong. Device \"" + connectedDevice.DisplayName + "\" is memorized and has bodypart \"" + memorizedDevice.bodypart + "\" and failed to match the defined categories.");
                                break;
                            }
                        }
                    }
                }
                if (found == false)
                {
                    devicesUncategorized.Add(connectedDevice);
                }
            }
        }

        public async void button_Connect_Click(Form_Main myForm)
        {
            haveAnyDevice = false;
            SetStatus(myForm, "Connecting...");
            //try
            //{
                myConnector = new ButtplugWebsocketConnector(new Uri("ws://" + mySettings.Get_IP() + ":" + mySettings.Get_Port()));
            //}
            //catch
            //{
               // MessageBox.Show("IP or Port not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               // return;
            //}
            if (debug) consoleWriteLine("Connecting to " + mySettings.Get_IP() + ":" + mySettings.Get_Port());
            bool isConnected = await Connect_InitFace(myForm);
            if (isConnected == true)
            {
                if (debug) consoleWriteLine("Connection established.");
                //quick and dirty set all connected devices to generic list. need to implement name based separation per user preference later
                devicesAll = client.Devices.ToList();
                if (devicesAll.Count > 0)
                {
                    CategorizeDevices();
                    haveAnyDevice = true;
                    controlActive = true;
                    SetStatus(myForm, "Connected");
                }
                else
                {
                    Disconnect(myForm);
                    MessageBox.Show("Connection succesful, but no devices detected. Please connect your devices to Intiface Central first. Disconnecting.");
                }
            }
            else
            {
                SetStatus(myForm, "Disconnected");
            }
        }

        public void SetStatus(Form_Main myForm, string text)//live verion
        {
            if (myForm.InvokeRequired)
            {
                //SetTextCallback d = new SetTextCallback(SetStatus);
               // myForm.Invoke(d, new object[] { text });

                SetStatusCallback d = new SetStatusCallback(SetStatus);

                // Call SetStatus using Invoke and pass the parameters
                myForm.Invoke(d, new object[] { myForm, text });
            }
            else
            {
                myForm.Text = "Sex Toy Link: " + text;
            }
        }

        public void StartGame(Form_Main myWindow)
        {
            myWindow.WindowState = FormWindowState.Maximized;
            if (mySettings.Get_GameSource() == "online")
            {//online
                if (mySettings.Get_DOL_path_online() == "")
                {
                    MessageBox.Show("Web address missing. Go to Settings and either set a web address, or change mode to Offline.");
                }
                else
                {//let the game begin...
                    myWindow.Open_DOL_button.Text = "Loading, please wait.";
                    UI_update_action = "load DOL";
                    url = new Uri(mySettings.Get_DOL_path_online(), UriKind.Absolute).AbsoluteUri;
                    myWindow.button_game_reload.Visible = true;
                    myWindow.timer_UI_update_wait.Start();
                }
            }
            else
            {//offline
                if (mySettings.Get_DOL_path_offline() == "")
                {
                    MessageBox.Show("File path is missing. Go to Settings and either set a file path, or change mode to Online.");
                }
                else
                {//let the game begin...
                    myWindow.Open_DOL_button.Text = "Loading, please wait.";
                    UI_update_action = "load DOL";
                    url = new Uri(mySettings.Get_DOL_path_offline(), UriKind.Absolute).AbsoluteUri;
                    myWindow.button_game_reload.Visible = true;
                    myWindow.timer_UI_update_wait.Start();
                }
            }
        }

        public void ResetSettings()
        {
            mySettings = new Settings();
            SaveSettingsFile();
        }

        public void LoadSettingsFromFile()
        {//todo: deserialize settings from settingsPath
            try
            {

                IFormatter myFormatter = new BinaryFormatter();
                Stream myStream = new FileStream(settingsPath, FileMode.Open, FileAccess.Read);
                mySettings = (Settings)myFormatter.Deserialize(myStream);                
                myStream.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load settings. Resetting to default values.");
                SaveSettingsFile();//create settings file with default values.
            }
        }

        public bool SaveSettings(Form_Main myWindow)
        {
            //check if values are valid before comiting to anything
            int temp = 0, temp1=0;
            try
            {
                temp = Convert.ToInt32(myWindow.textBox_DOL_min_str.Text);
            }
            catch
            {
                MessageBox.Show("Min strength must be a number (0-100).");
                return false;
            }

            try
            {
                temp = Convert.ToInt32(myWindow.textBox_DOL_max_str.Text);
            }
            catch
            {
                MessageBox.Show("Max strength must be a number (0-100).");
                return false;
            }
            temp = Convert.ToInt32(myWindow.textBox_DOL_max_str.Text);
            temp1 = Convert.ToInt32(myWindow.textBox_DOL_min_str.Text);
            if (temp1 > temp)
            {
                MessageBox.Show("Min strength must be less or equal to Max strength.");
                return false;
            }


            try
            {
                temp = Convert.ToInt32(myWindow.textBox_DOL_cycle.Text);
            }
            catch
            {
                MessageBox.Show("Cycle duration must be a number (0-100).");
                return false;
            }

            //beyond this point values are valid, save them.


            //skip setting gameSource, that's set when radiobutton is clicked.
            mySettings.Set_DOL_path_offline(myWindow.textBox_Filepath.Text);
            mySettings.Set_DOL_path_online(myWindow.textBox_WebAddress.Text);
            mySettings.Set_IP(myWindow.textBox_buttplug_IP.Text);
            mySettings.Set_Port(myWindow.textBox_buttplug_port.Text);
            mySettings.Set_DOL_vib_min(Convert.ToInt32(myWindow.textBox_DOL_min_str.Text));
            mySettings.Set_DOL_vib_max(Convert.ToInt32(myWindow.textBox_DOL_max_str.Text));
            mySettings.Set_DOL_vib_cycle(Convert.ToInt32(myWindow.textBox_DOL_cycle.Text));

            SaveSettingsFile();
            return true;
        }
        public void SaveSettingsFile()
        {
            try
            {
                IFormatter myFormatter = new BinaryFormatter();
                Stream myStream = new FileStream(settingsPath, FileMode.Create, FileAccess.Write);
                myFormatter.Serialize(myStream, mySettings);
                myStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nStack trace:\n" + e.StackTrace);
            }
        }
    }
}
