using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using Object = UnityEngine.Object;
using MelonLoader;

namespace _LemonClient.Features
{
    class MirrorLogic				//Inspired and adapted from MunchenClient
    {
		//Vars
		public static Dictionary<int, GameObject> mirrorDictionary = new Dictionary<int, GameObject>();
		public static readonly Dictionary<string, object> assetCache = new Dictionary<string, object>();
		public static AssetBundle cachedAssetBundle = null;

		//Spawn Mirror
		public static IEnumerator SpawnMirror(int id, float sizeX, float sizeY)
		{
			Camera playerCam = VRCVrCamera.field_Private_Static_VRCVrCamera_0.field_Public_Camera_0;
			GameObject mirrorPrefab = Object.Instantiate(LoadGameObject("Mirror"));
			mirrorPrefab.transform.localScale = new Vector3(sizeX, sizeY, 1f);
			mirrorPrefab.transform.position = playerCam.transform.position + playerCam.transform.forward;
			mirrorPrefab.transform.LookAt(playerCam.transform);
			mirrorPrefab.transform.Rotate(0f, 180f, 0f, Space.Self);
			mirrorDictionary[id] = mirrorPrefab;
			return null;
		}

		public static void FlattenMirror()
		{
			Camera playerCam = VRCVrCamera.field_Private_Static_VRCVrCamera_0.field_Public_Camera_0;
			MelonLogger.Msg("Camera X: " + playerCam.transform.rotation.x);
			MelonLogger.Msg("Camera Y: " + playerCam.transform.rotation.y);
			MelonLogger.Msg("Camera Z: " + playerCam.transform.rotation.z);
		}

		public static void RemoveMirror(int id)
		{
			if (mirrorDictionary.ContainsKey(id))
			{
				Object.Destroy(mirrorDictionary[id]);
				mirrorDictionary.Remove(id);
			}
		}

		//Yoinked from Munchen, until I come up with my own solution
		internal static GameObject LoadGameObject(string prefabName)
		{
			if (GlobalVariables.cachedAssetBundle == null)
			{
				return null;
			}
			string text = "assets/bundledassets/lemonclientbundle/" + prefabName + ".prefab";
			if (assetCache.ContainsKey(text))
			{
				return (GameObject)assetCache[text];
			}
			GameObject gameObject = GlobalVariables.cachedAssetBundle.LoadAsset_Internal(text, Il2CppType.Of<GameObject>()).Cast<GameObject>();
			gameObject.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			assetCache.Add(text, gameObject);
			return gameObject;
		}

		//Again, yoinked from Munchen Source, until I find my own method
		internal static void LoadAssetBundle(string filePath)
		{
			GlobalVariables.cachedAssetBundle = AssetBundle.LoadFromMemory_Internal(File.ReadAllBytes(filePath), 0u);
			GlobalVariables.cachedAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
		}
	}
}
