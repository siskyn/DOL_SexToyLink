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
using SexToyLink.Forms;
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
    public partial class Form_Main : Form
    {
        public volatile Controller mycontroller;

        public Form_Main()
        {
            InitializeComponent();
            mycontroller = new Controller();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            mycontroller.client.DeviceAdded += mycontroller.HandleDeviceAdded;
            mycontroller.client.DeviceRemoved += mycontroller.HandleDeviceRemoved;
            mycontroller.LoadSettings(this);
        }

        private void button_Play_Click(object sender, EventArgs e)
        {
            mycontroller.StartGame(this);
        }

        private async void timer_DOL_Tick(object sender, EventArgs e)
        {
            mycontroller.Timer_DOL_Tick_Update(this);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mycontroller.selectedTab = tabControl1.SelectedIndex;
        }

        private void timer_DOL_vibrate_Tick(object sender, EventArgs e)
        {
            mycontroller.UpdateVibrationWhilePlaying(this);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);//prevent form being disposed before control thread ends resulting in error.
        }

        private void radioButton_offline_CheckedChanged(object sender, EventArgs e)
        {
            mycontroller.mySettings.Set_GameSource("offline");
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            mycontroller.button_Browse_Click(this);            
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            mycontroller.Disconnect(this);
        }

        private async void button_Connect_Click(object sender, EventArgs e)
        {
            mycontroller.button_Connect_Click(this);
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
        {
            mycontroller.browser_page_loaded(this);
        }

        private void button_save_Settings_Click(object sender, EventArgs e)
        {
            mycontroller.button_Save_Click(this);
        }

        private void timer_UI_update_wait_Tick(object sender, EventArgs e)
        {
            mycontroller.timer_UI_update_wait_Tick(this);
        }

        private void button_Devices_Details_Click(object sender, EventArgs e)
        {
            mycontroller.button_Devices_Details_Clicked(this);
        }
    }
}
















