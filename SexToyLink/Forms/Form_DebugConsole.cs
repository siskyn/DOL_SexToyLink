using CefSharp.DevTools.Browser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SexToyLink.Forms
{
    
    public partial class Form_DebugConsole : Form
    {
        private Panel titleBarPanel;
        private bool mustClose = false;
        private SexToyLink.Classes.Controller myController;
        private bool monitoring = true;
        public Form_DebugConsole(Classes.Controller myController)
        {
            InitializeComponent();
            this.myController = myController;
        }


        public void writeLine(string timeStamp, string message)
        {

            if (textBox_console.InvokeRequired)
            {
                textBox_console.BeginInvoke((MethodInvoker)delegate
                {
                    if (monitoring) writeLineToTextBox(timeStamp, message);
                });
            }
            else
            {
                if(monitoring) writeLineToTextBox(timeStamp, message);
            }
        }



        private void writeLineToTextBox(string timeStamp, string message)
        {

            //textBox_console.Text = textBox_console.Text + "\n[" + currentTimeLocal + "] " + message;
            textBox_console.AppendText("\r\n[" + timeStamp + "] " + message);

        }

        private void Form_DebugConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mustClose)
            {
                e.Cancel = true;
                myController.toggleDebug(false);
            }                       
        }

        public void closeConsole()
        {
            mustClose = true;
            this.Close();
        }

        private void button_Copy_Click(object sender, EventArgs e)
        {
            try
            {
                if (monitoring)
                {//if we're monitoring, we're likely writing message to this and will fail to copy. stop accepting input, give it time to finish writing, copy, then resume accepting input.
                    monitoring = false;
                    Thread.Sleep(1000);
                    Clipboard.SetText(textBox_console.Text);
                    monitoring = true;
                }
                Clipboard.SetText(textBox_console.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to copy clipboard: Didn't finish writing previous message. Please copy again.");
            }
        }

        private void buttonStopMonitor_Click(object sender, EventArgs e)
        {
            monitoring = false;
        }

        private void button_startMonitor_Click(object sender, EventArgs e)
        {
            monitoring = true;
        }
    }
}
