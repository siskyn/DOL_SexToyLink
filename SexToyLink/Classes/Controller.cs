using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SexToyLink.Classes
{
    public class Controller
    {

        public volatile string UI_update_action = "", url = "";





        public  Settings mySettings = new Settings();
        string settingsPath = Application.StartupPath +"\\settings";
        public Controller()
        {
            try
            {
                LoadSettingsFromFile();
                
            }
            catch
            {

            }

        }

        public void StartGame(Form1 myWindow)
        {
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

        public bool SaveSettings(Form1 myWindow)
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
