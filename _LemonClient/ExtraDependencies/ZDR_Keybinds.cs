using System;
using System.Collections;
using MelonLoader;
using UnityEngine;
using VRC.SDKBase;

namespace _LemonClient.ExtraDependencies
{
    class ZDR_Keybinds
    {
		//Let me make this clear,
		//I 100% stole this from ZeroDay to make their fly system work
		//I am waaaaaaaaaaaaaaaay too lazy to get this working myself
		//Welp, hope that helps clear up an confusion

		public static IEnumerator Fly()
		{
			for (; ; )
			{
				ZDR_Keybinds.CameraTransform = Camera.main.transform;
				bool keyDown = Input.GetKeyDown((KeyCode)304);
				if (keyDown)
				{
					GlobalVariables.flySpeed *= 2f;
				}
				bool keyUp = Input.GetKeyUp((KeyCode)304);
				if (keyUp)
				{
					GlobalVariables.flySpeed /= 2f;
				}
				bool key = Input.GetKey((KeyCode)101);
				if (key)
				{
					VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += ZDR_Keybinds.CameraTransform.up * GlobalVariables.flySpeed * Time.deltaTime;
				}
				bool key2 = Input.GetKey((KeyCode)113);
				if (key2)
				{
					VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position -= ZDR_Keybinds.CameraTransform.up * GlobalVariables.flySpeed * Time.deltaTime;
				}
				bool key3 = Input.GetKey((KeyCode)119);
				if (key3)
				{
					VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += ZDR_Keybinds.CameraTransform.forward * GlobalVariables.flySpeed * Time.deltaTime;
				}
				bool key4 = Input.GetKey((KeyCode)97);
				if (key4)
				{
					VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position -= ZDR_Keybinds.CameraTransform.right * GlobalVariables.flySpeed * Time.deltaTime;
				}
				bool key5 = Input.GetKey((KeyCode)100);
				if (key5)
				{
					VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += ZDR_Keybinds.CameraTransform.right * GlobalVariables.flySpeed * Time.deltaTime;
				}
				bool key6 = Input.GetKey((KeyCode)115);
				if (key6)
				{
					VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position -= ZDR_Keybinds.CameraTransform.forward * GlobalVariables.flySpeed * Time.deltaTime;
				}
				bool flag = Networking.LocalPlayer.IsUserInVR();
				if (flag)
				{
					bool flag2 = Math.Abs(Input.GetAxis("Vertical")) != 0f;
					if (flag2)
					{
						VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += ZDR_Keybinds.CameraTransform.forward * GlobalVariables.flySpeed * Time.deltaTime * Input.GetAxis("Vertical");
					}
					bool flag3 = Math.Abs(Input.GetAxis("Horizontal")) != 0f;
					if (flag3)
					{
						VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += ZDR_Keybinds.CameraTransform.right * GlobalVariables.flySpeed * Time.deltaTime * Input.GetAxis("Horizontal");
					}
					bool flag4 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f;
					if (flag4)
					{
						VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += ZDR_Keybinds.CameraTransform.up * GlobalVariables.flySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
					}
					bool flag5 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f;
					if (flag5)
					{
						VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += ZDR_Keybinds.CameraTransform.up * GlobalVariables.flySpeed * Time.deltaTime * Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryThumbstickVertical");
					}
				}
				Networking.LocalPlayer.SetVelocity(Vector3.zero);
				yield return null;
			}
		}

		public static void Initialize()
		{
			bool flag = Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)102);
			if (flag)
			{
				bool flag2 = !Exploits.FlightControl.flightEnabledZDM;
				if (flag2)
				{
					Exploits.FlightControl.flightEnabledZDM = true;
					ZDR_Keybinds.flycor = MelonCoroutines.Start(ZDR_Keybinds.Fly());
					VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = false;
				}
				else
				{
					Exploits.FlightControl.flightEnabledZDM = false;
					MelonCoroutines.Stop(ZDR_Keybinds.flycor);
					VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<CharacterController>().enabled = true;
				}
			}
		}

		private static Transform CameraTransform;

		public static object flycor;

	}
}
