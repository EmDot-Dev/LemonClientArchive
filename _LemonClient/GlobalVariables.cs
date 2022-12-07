using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;

namespace _LemonClient
{
    public class GlobalVariables
    {
        public static Sprite lemonLogo;
        public static Texture2D lemonTexture;

        public static String murdererName;
        public static float flySpeed = 10;
        public static bool infRevolver;
        public static bool murderNameplateBool;
        public static bool timerDelPortals;

        public static GameObject murderLobbyArea;
        public static GameObject murderGameArea;
        public static bool globalGoldenGun = false;

        public static float pEGayTime = 1;

        public static float currentMirrorX = 5f;
        public static float currentMirrorY = 5f;

        public static Player selectedPlayer = null;

        public static readonly Dictionary<string, object> assetCache = new Dictionary<string, object>();
        public static AssetBundle cachedAssetBundle = null;

        public static bool mirrorIndex1 = false;

        public static bool playerESP = false;
        public static bool rainbowESP = false;
    }
}
