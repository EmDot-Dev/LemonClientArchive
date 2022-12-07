using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC;
using VRC.Core;
using VRC.Udon;
using VRC.SDKBase;
using MelonLoader;
using UnityEngine.UI;
using VRC.UI.Elements.Menus;
using System.Collections;
using VRC.SDK3.Components;

namespace _LemonClient.Features
{
    class ESPHandler
    {
        //An attempt at some methods

        public static void ApplyAllPlayerWallhack() {
            foreach (Player player in ExtraDependencies.PlayerWrapper.GetAllPlayers())
            {
                if(player != ExtraDependencies.PlayerWrapper.LocalPlayer())
                {
                    ExtraDependencies.PlayerWrapper.EnableESPState(player._vrcplayer.field_Internal_PlayerSelector_0.GetComponent<Renderer>());
                }
            }
        }

        public static void RemoveAllPlayerWallhack()
        {
            foreach (Player player in ExtraDependencies.PlayerWrapper.GetAllPlayers())
            {
                if (player != ExtraDependencies.PlayerWrapper.LocalPlayer())
                {
                    ExtraDependencies.PlayerWrapper.DisableESPState(player._vrcplayer.field_Internal_PlayerSelector_0.GetComponent<Renderer>());
                }
            }
        }

    }
}
