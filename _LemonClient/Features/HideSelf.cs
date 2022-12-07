using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace _LemonClient.Features
{
    class HideSelf                              //Mainly used for player protection against malicious avatars
    {
        public static void ClearAssets()
        {
            AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.field_Private_Cache_0.ClearCache();
            AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.field_Private_Queue_1_AssetBundleDownload_0.Clear();
            AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.field_Private_Queue_1_AssetBundleDownload_1.Clear();
        }

        public static void PerformHide(bool toggleState)
        {
            if (toggleState)                             //Stopping Avatar Queue
            {
                ClearAssets();
                MelonLogger.Msg("Stopping Avatar Queue");
                AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.gameObject.SetActive(false);
                ClearAssets();
            }
            else
            {
                ClearAssets();
                MelonLogger.Msg("Restarting Avatar Queue");
                AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.gameObject.SetActive(true);
                ClearAssets();
            }
        }
    }
}
