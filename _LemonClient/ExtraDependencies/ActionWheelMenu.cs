using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LemonClient.ExtraDependencies
{
    internal class ActionWheelMenu
    {
        internal static MunchenActionMenu.ActionMenuPage lemonPage;
        internal static MunchenActionMenu.ActionMenuButton mirrorToggle;
        internal static MunchenActionMenu.ActionMenuButton portalButton;

        internal ActionWheelMenu()      //Uses someone else's code to get this working. Never worked from memory. Oh well. Should have tried to do it from scratch
        {
            lemonPage = new MunchenActionMenu.ActionMenuPage(MunchenActionMenu.ActionMenuBaseMenu.MainMenu, "Lemon", Main.LoadTexture("lemonicon"));

            mirrorToggle = new MunchenActionMenu.ActionMenuButton(lemonPage, "Spawn Mirror", delegate
            {
                if(GlobalVariables.mirrorIndex1)
                {
                    Features.MirrorLogic.RemoveMirror(1);
                    GlobalVariables.mirrorIndex1 = false;
                    mirrorToggle.SetButtonText("Spawn Mirror");
                } else
                {
                    Features.MirrorLogic.SpawnMirror(1, GlobalVariables.currentMirrorX, GlobalVariables.currentMirrorY);
                    GlobalVariables.mirrorIndex1 = true;
                    mirrorToggle.SetButtonText("Remove Mirror");
                }
            });

            portalButton = new MunchenActionMenu.ActionMenuButton(lemonPage, "Delete Portals", delegate 
            {
                Features.PortalDeleter.DeleteAllPortals(true);
            });

        }
    }
}
