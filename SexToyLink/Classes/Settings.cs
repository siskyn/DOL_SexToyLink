using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexToyLink.Classes
{
    [Serializable]
    class Settings
    {
        private int DOL_vibration_min;
        private int DOL_vibration_max;
        private int DOL_vibration_cycle;
        private bool remember_DOL_path;
        private string DOL_path;
        private string port;
        private string IP;

        public Settings()
        {
            DOL_vibration_min = 0;
            DOL_vibration_max = 100;
            DOL_vibration_cycle = 700;
            //remember_DOL_path = true;
            DOL_path = "";
            port = "12345";
            IP = "localhost";
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

        public void Set_DOL_path(string path)
        {
            DOL_path = path;
        }

        public string Get_DOL_path()
        {
            return DOL_path;
        }

        public void Set_remember_DOL_path( bool value)
        {
            remember_DOL_path = value;
        }

        public bool Get_remember_DOL_path()
        {
            return remember_DOL_path;
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
