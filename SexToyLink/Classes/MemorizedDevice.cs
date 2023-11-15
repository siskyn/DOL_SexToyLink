using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexToyLink.Classes
{
    [Serializable]
    public class MemorizedDevice
    {
        public string displayName;
        public string bodypart;

        public MemorizedDevice(string newdisplayName, string newbodyPart) 
        {
            displayName = newdisplayName;
            bodypart = newbodyPart;
        }

    }
}
