using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buttplug.Client;

namespace SexToyLink.Classes
{
    [Serializable]
    public class Settings
    {
        private int DOL_vibration_min;
        private int DOL_vibration_max;
        private int DOL_vibration_cycle;
        private string gameSource;
        private string DOL_path_online;
        private string DOL_path_offline;
        private string port;
        private string IP;
        public List<MemorizedDevice> memorizedDevicesList;

        public Settings()
        {
            DOL_vibration_min = 0;
            DOL_vibration_max = 100;
            DOL_vibration_cycle = 800;
            gameSource = "online";
            DOL_path_online = "https://dolmods.net/";
            DOL_path_offline = "";
            port = "12345";
            IP = "localhost";
            memorizedDevicesList = new List<MemorizedDevice>();
        }
        public void Set_GameSource(string newSource)
        {
            gameSource = newSource;
        }

        public string Get_GameSource()
        {
            return gameSource;
        }

        public void Set_DOL_path_online(string newPath)
        {
            DOL_path_online = newPath;
        }

        public string Get_DOL_path_online()
        {
            return DOL_path_online;
        }

        public void Set_DOL_path_offline(string newPath)
        {
            DOL_path_offline = newPath;
        }

        public string Get_DOL_path_offline()
        {
            return DOL_path_offline;
        }

        public void Set_IP(string newIP)
        {
            IP = newIP;
        }

        public string Get_IP()
        {
            return IP;
        }

        public void Set_Port(string newPort)
        {
            port = newPort;
        }

        public string Get_Port()
        {
            return port;
        }


        public void Set_DOL_vib_min(int value)
        {
            DOL_vibration_min = value;
        }

        public void Set_DOL_vib_max(int value)
        {
            DOL_vibration_max = value;
        }

        public void Set_DOL_vib_cycle(int value)
        {
            DOL_vibration_cycle = value;
        }

        public int Get_DOL_vib_min()
        {
            return DOL_vibration_min;
        }

        public int Get_DOL_vib_max()
        {
            return DOL_vibration_max;
        }

        public int Get_DOL_vib_cycle()
        {
            return DOL_vibration_cycle;
        }

    }
}
