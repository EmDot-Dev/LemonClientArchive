using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LemonClient.Features
{
    class SwitchToAvatarID
    {
        public static string storedID = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11"; //Default VRChat Robot
        public static string prevID = "";
        public static bool activeState = false;
        public static void SwitchID(string avatarID)
        {
            prevID = ExtraDependencies.PlayerWrapper.GetAPIAvatar(ExtraDependencies.PlayerWrapper.LocalVRCPlayer()).id; //Grab local player ID
            ExtraDependencies.PlayerWrapper.ChangeAvatar(avatarID); //Change into stored avatar ID
        }
    }
}
