using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC;
using VRC.SDKBase;
using VRC.Core;
using VRC.Udon;
using UnityEngine;
using UnhollowerBaseLib;

namespace _LemonClient.ExtraDependencies
{
	internal static class WorldWrapper              //THANKS ZeroDay Remastered!!
	{
		public static Il2CppArrayBase<UdonBehaviour> Behaviours;

		internal static ApiWorld CurrentWorld()
		{
			return RoomManager.field_Internal_Static_ApiWorld_0;
		}


		internal static ApiWorldInstance CurrentInstance()
		{
			return RoomManager.field_Internal_Static_ApiWorldInstance_0;
		}


		internal static IEnumerable<Player> GetPlayers()
		{
			return PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray();
		}


		internal static bool IsInRoom()
		{
			return RoomManager.field_Internal_Static_ApiWorld_0 != null && RoomManager.field_Internal_Static_ApiWorldInstance_0 != null;
		}


		internal static int GetPlayerCount()
		{
			return PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count;
		}


		internal static string GetJoinID()
		{
			return WorldWrapper.CurrentInstance().id;
		}


		internal static void JoinWorld(string worldID)
		{
			Networking.GoToRoom(worldID);
		}

		public static void SendUdonRPC(GameObject Object, string EventName, Player Target = null, bool Local = false)
		{
			bool flag = Object != null;
			if (flag)
			{
				UdonBehaviour component = Object.GetComponent<UdonBehaviour>();
				bool flag2 = Target != null;
				if (flag2)
				{
					Networking.SetOwner(Target.field_Private_VRCPlayerApi_0, Object);
					component.SendCustomNetworkEvent((VRC.Udon.Common.Interfaces.NetworkEventTarget)1, EventName);
				}
				else
				{
					bool flag3 = !Local;
					if (flag3)
					{
						component.SendCustomNetworkEvent(0, EventName);
					}
					else
					{
						component.SendCustomEvent(EventName);
					}
				}
			}
			else
			{
				foreach (UdonBehaviour udonBehaviour in Behaviours)
				{
					bool flag4 = udonBehaviour._eventTable.ContainsKey(EventName);
					if (flag4)
					{
						udonBehaviour.SendCustomNetworkEvent(0, EventName);
					}
				}
			}
		}

	}
}
