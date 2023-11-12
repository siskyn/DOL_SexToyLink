using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SexToyLink.Classes
{
    class Controller
    {
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

        public void LoadSettingsFromFile()
        {//todo: deserialize settings from settingsPath
            try
            {

                IFormatter myFormatter = new BinaryFormatter();
                Stream myStream = new FileStream(settingsPath, FileMode.Open, FileAccess.Read);
                mySettings = (Settings)myFormatter.Deserialize(myStream);                
                myStream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to load settings. Resetting to default values.");
                SaveSettingsFile();//create settings file with default values.
            }
        }

        public void SaveSettingsFile()
        {//todo: serialize settings to settingsPath
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
