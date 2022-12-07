using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using TMPro;
using System.Collections;

namespace _LemonClient.Features
{
    class PortalDeleter									//Adapted from Munchen and ZeroDay clients
    {
        internal static void DeleteAllPortals(bool informHUD)
        {
            PortalInternal[] portals = Resources.FindObjectsOfTypeAll<PortalInternal>();
			int num = 0;
			for (int i = 0; i < portals.Length; i++)
			{
				if (!(portals[i] == null))
				{
					TextMeshPro componentInChildren = portals[i].GetComponentInChildren<TextMeshPro>();
					if ((componentInChildren.text.Contains("public") || componentInChildren.text.Contains("invite") || componentInChildren.text.Contains("friends")) && !componentInChildren.text.Contains(APIUser.CurrentUser.displayName))
					{
						Networking.Destroy(portals[i].gameObject);
						num++;
					}
				}
			}
			if(informHUD)
            {
				if(num == 1)
                {
					ExtraDependencies.APIUtils.ShowAlert(ExtraDependencies.APIUtils.GetQuickMenuInstance(), "Deleted " + num + " portal!");
                } else if (num > 1)
                {
					ExtraDependencies.APIUtils.ShowAlert(ExtraDependencies.APIUtils.GetQuickMenuInstance(), "Deleted " + num + " portals!");
                }
            }
		}

		internal static IEnumerator TimerPortalDelete()
        {
			while (GlobalVariables.timerDelPortals)
            {
				yield return new WaitForSeconds(1f);
				PortalInternal[] portals = Resources.FindObjectsOfTypeAll<PortalInternal>();
				int num = 0;
				for (int i = 0; i < portals.Length; i++)
				{
					if (!(portals[i] == null))
					{
						TextMeshPro componentInChildren = portals[i].GetComponentInChildren<TextMeshPro>();
						if ((componentInChildren.text.Contains("public") || componentInChildren.text.Contains("invite") || componentInChildren.text.Contains("friends")) && !componentInChildren.text.Contains(APIUser.CurrentUser.displayName))
						{
							Networking.Destroy(portals[i].gameObject);
							num++;
						}
					}
				}
			}
        }
    }
}
