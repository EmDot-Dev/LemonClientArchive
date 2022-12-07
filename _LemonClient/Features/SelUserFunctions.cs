using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.DataModel;

namespace _LemonClient.Features
{
    class SelUserFunctions                          //Personal adaptation of the PlayerWrapper class
    {
        public static void ForceClone(Player player)
        {
            if (player == null)
            {
                return;
            }
            var apiAvatar = CallAPIAvatar(player);
            if (apiAvatar == null) return;
            ExtraDependencies.PlayerWrapper.ChangeAvatar(apiAvatar.id);
        }

        public static void CopyAvatarID(Player player)
        {
            var apiAvatar = CallAPIAvatar(player);
            Clipboard.SetText(apiAvatar.id);
        }

        public static void CopyUserID(Player player)
        {
            var apiPlayer = ExtraDependencies.PlayerWrapper.GetAPIUser(player).id;
            Clipboard.SetText(apiPlayer);
        }

        public static ApiAvatar CallAPIAvatar(Player player)
        {
            var apiAvatar = ExtraDependencies.PlayerWrapper.GetAPIAvatar(player);
            return apiAvatar;
        }

        public static void TeleportToPlayer(Player player)
        {
            if (player == null) return;

            var transform = player.transform;
            var targetPosition = transform.position;

            var localTransform = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform;
            localTransform.position = targetPosition;
        }

    }
}
