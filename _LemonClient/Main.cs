using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;
using Object = UnityEngine.Object;

namespace _LemonClient
{   

    //Before this code gets started, all of this was basically deemed as unusable once the EAC update got pushed to VRChat
    //Thusly, this code will not be being updated. All basic malicious code has been removed, and all personal info
    //should also have been removed. I'm lazy, but I do try a little here and there.

    //Please enjoy looking over a culmination of my own work, other people's work, and a mishmash of everything inbetween

    public class Main : MelonMod
    {
        public static string spriteImage;
        public static Sprite ButtonImage;
        public static object infRevRoutine;
        public static object mNameplateRoutine;
        public static object togglePortalRoutine;
        public static object goldGunRoutine;
        public static Sprite lemonLogo;
        public static Texture2D lemonTexture;
        public static string playerID;
        //public static readonly string assetPath = "LemonClient\\Dependencies\\ClientAssets\\ClientAssets.assetbundle";
        public static readonly string logoPath = "LemonClient\\Dependencies\\ClientAssets\\lemonbundle.assetbundle";
        public override void OnApplicationStart()
        {
            //Starting Issues
            MelonCoroutines.Start(WaitForQM());
            MelonCoroutines.Start(WaitForPlayer());
            //lemonLogo = MakeSpriteFromImage(GetImageFromResources("lemonLogo"));

        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (sceneName == "ui")
            {
                //Game Loaded
                MelonLogger.Msg("Welcome to LemonClient");
                //LoadAssetBundle(assetPath);
                LoadAssetBundle(logoPath);
                if (GlobalVariables.cachedAssetBundle != null)
                {
                    MelonLogger.Msg("Loaded test asset bundle");
                    //GlobalVariables.lemonLogo = MakeSpriteFromAssetBundle(LoadTexture("lemonicon"));
                    //GlobalVariables.lemonTexture = LoadTexture("lemonicon");
                    GlobalVariables.lemonTexture = LoadTexture("lemonicon");
                    GlobalVariables.lemonLogo = MakeSpriteFromAssetBundle(GlobalVariables.lemonTexture);
                    if(GlobalVariables.lemonLogo != null)
                    {
                        MelonLogger.Msg("Logo successfully loaded");
                    } else
                    {
                        MelonLogger.Error("Logo failed");
                    }
                } else
                {
                    MelonLogger.Error("Asset Bundle Not Loaded!");
                }
                ExtraDependencies.PlayerWrapper.playerESP = ExtraDependencies.PlayerWrapper.GetPlayerCamera().gameObject.AddComponent<HighlightsFXStandalone>();
                ExtraDependencies.PlayerWrapper.playerESP.blurIterations = 3;
                ExtraDependencies.PlayerWrapper.playerESP.blurSize = 2f;
                ExtraDependencies.PlayerWrapper.RefreshWallhacks();
                new ExtraDependencies.ActionWheelMenu();
            }
        }

        public override void OnUpdate()
        {
            if(GlobalVariables.playerESP)
            {
                ExtraDependencies.PlayerWrapper.RefreshWallhacks();
            }
        }

        private static IEnumerator WaitForQM()
        {
            while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            QMFunctions();
        }

        private static IEnumerator WaitForPlayer()
        {
            while (Object.FindObjectOfType<VRC.Player>() == null) yield return null;
            yield return new WaitForSeconds(1f);
            playerID = ExtraDependencies.PlayerWrapper.LocalPlayer().prop_String_0;
            MelonLogger.Msg("Local Player ID: " + playerID);
        }

        public static void QMFunctions()
        {
            //New Menu = QMNestedButton(Menu Name / Variable, xPos, yPos, Button Text, Tooltip, Menu Name)
            //New Button = QMSingleButton(Menu Name / Variable, xPos, yPos, Button Text, Action () =>, Tooltip)
            //New Toggle = QMToggleButton(Menu Name / Variable, xPos, yPos, Toggle Text, delegate {Action On}, delegate {Action Off}, Tooltip)
            MelonLogger.Msg("QuickMenu Loaded");
            var tabMenu = new ExtraDependencies.QMTabMenu("Lemon Client", "Lemon Client", GlobalVariables.lemonLogo);

            var lemonIcon = new ExtraDependencies.QMSingleButton(tabMenu, 4, 3, "", delegate {
                ExtraDependencies.APIUtils.ShowAlert(ExtraDependencies.APIUtils.GetQuickMenuInstance(), "All functionality is skidded and (mostly) adjusted by EmDot");
            }, "Just a lemon");
            lemonIcon.SetBackgroundImage(GlobalVariables.lemonLogo);
            //Murder 4 Exploits
            var m4Menu = new ExtraDependencies.QMNestedButton(tabMenu, 1, 0, "Murder 4", "Murder 4 Buttons", "Murder 4", true);

            //Roles
            var m4UdonLogic = new ExtraDependencies.QMNestedButton(m4Menu, 1, 0, "Udon Stuff", "Udon Stuff", "Udon Stuff");

            var bystanderSet = new ExtraDependencies.QMSingleButton(m4UdonLogic, 1, 0, "Set Bystander", delegate { Exploits.MurderExploits.SelfBystander(); }, "Set self to Bystander");
            var murdererSet = new ExtraDependencies.QMSingleButton(m4UdonLogic, 2, 0, "Set Murderer", delegate { Exploits.MurderExploits.SelfMurderer(); }, "Set self to Murderer");
            var getMurderer = new ExtraDependencies.QMSingleButton(m4UdonLogic, 3, 0, "Who is Murderer?", delegate {

                MelonCoroutines.Start(RunCode());
                IEnumerator RunCode()
                {
                    MelonCoroutines.Start(Exploits.MurderExploits.FindKillerEnumerator(0.1f));
                    yield return new WaitForSeconds(3f);
                    ExtraDependencies.APIUtils.ShowAlert(ExtraDependencies.APIUtils.GetQuickMenuInstance(), "Murderer is; "+GlobalVariables.murdererName);
                    yield break;
                }

            }, "Announce Murderer");
            //General Utilities
            var lightsOnBTN = new ExtraDependencies.QMSingleButton(m4Menu, 1, 1, "Lights On", delegate { Exploits.MurderExploits.LightsON(); }, "Turns Lights on");
            var forceStart = new ExtraDependencies.QMSingleButton(m4Menu, 2, 1, "Force Start", delegate { Exploits.MurderExploits.StartGame(); }, "Force Starts Game");
            var forceAbort = new ExtraDependencies.QMSingleButton(m4Menu, 3, 1, "Abort Game", delegate { Exploits.MurderExploits.AbortGame(); }, "Aborts Game");
            var lightsOffBTN = new ExtraDependencies.QMSingleButton(m4Menu, 4, 1, "Lights Off", delegate { Exploits.MurderExploits.LightsOFF(); }, "Turns Lights off");

            //"Cosmetics"
            var antiBlind = new ExtraDependencies.QMToggleButton(m4Menu, 1, 2, "Anti-Blind", delegate { Exploits.MurderExploits.AntiBlind(true); }, delegate { Exploits.MurderExploits.AntiBlind(false); }, "Enable / Disable Antiblind");
            var infShots = new ExtraDependencies.QMToggleButton(m4Menu, 2, 2, "Infinite Shots", delegate {
                infRevRoutine = MelonCoroutines.Start(Exploits.MurderExploits.InfiniteRevolver());
                GlobalVariables.infRevolver = true;
            }, delegate {
                if (GlobalVariables.infRevolver)
                {
                    MelonCoroutines.Stop(infRevRoutine);
                    GlobalVariables.infRevolver = false;
                }
            }, "Enable / Disable Infinite Shots");
            var stabbyPlate = new ExtraDependencies.QMToggleButton(m4Menu, 3, 2, "Murderer Nameplate", delegate {
                mNameplateRoutine = MelonCoroutines.Start(Exploits.MurderExploits.ShowMurdererOnNamePlate());
                GlobalVariables.murderNameplateBool = true;
            }, delegate {
                if (GlobalVariables.murderNameplateBool == true)
                {
                    MelonCoroutines.Stop(mNameplateRoutine);
                    Exploits.MurderExploits.murderNameplateBool = false;
                }
            }, "Enable / Disable Murderer Nameplate");

            var m4CosmeticMenu = new ExtraDependencies.QMNestedButton(m4Menu, 2.5f, 0, "Cosmetics", "Cosmetics", "Cosmetics");
            var goldGun = new ExtraDependencies.QMSingleButton(m4CosmeticMenu, 1, 0, "Local Gold Gun", delegate { MelonCoroutines.Start(Exploits.MurderExploits.GoldenGunForYou()); }, "Enable Gold Gun");
            var laserGunSight = new ExtraDependencies.QMSingleButton(m4CosmeticMenu, 2, 0, "Laser Sight", delegate { GameObject.Find("Game Logic/Weapons/Revolver/Recoil Anim/Recoil/Laser Sight").active = true; }, "Enable Laser Sight");
            var xmasGun = new ExtraDependencies.QMSingleButton(m4CosmeticMenu, 3, 0, "X-mas Gun", delegate { GameObject.Find("Game Logic/Weapons/Revolver/Recoil Anim/Recoil/geo (xmas)").active = true; }, "XMas Gun");
            var goldGunToggle = new ExtraDependencies.QMToggleButton(m4CosmeticMenu, 4, 0, "Global Gold Gun", delegate
            {
                GlobalVariables.globalGoldenGun = true;
                goldGunRoutine = MelonCoroutines.Start(Exploits.MurderExploits.GlobalGoldenGun());

            }, delegate {
                GlobalVariables.globalGoldenGun = false;
                MelonCoroutines.Stop(goldGunRoutine);
                goldGunRoutine = null;
            }, "Gold Gun toggle", false);
            
            var clueESP = new ExtraDependencies.QMSingleButton(m4Menu, 4, 2, "Clue ESP", delegate { MelonCoroutines.Start(Exploits.MurderExploits.ClueEsp()); }, "Enable Clue ESP");
            var respawnPickups = new ExtraDependencies.QMSingleButton(m4Menu, 1, 3, "Respawn Pickups", delegate { Exploits.MurderExploits.RespawnPickups(); }, "Respawn All Pickups");
            var forceGrab = new ExtraDependencies.QMSingleButton(m4Menu, 4, 3, "Force Grab", delegate { Exploits.MurderExploits.ForceGrab(); }, "Enable Force Grab");
            

            //Bring Pickups
            var pickupPage = new ExtraDependencies.QMNestedButton(m4Menu, 4, 0, "Pickups", "Pickups Menu", "Pickups");

            var bringRev = new ExtraDependencies.QMSingleButton(pickupPage, 1, 0, "Bring Revolver", delegate { Exploits.MurderExploits.BringRoleWeapon(1); }, "Bring Revolver");
            var bringluger = new ExtraDependencies.QMSingleButton(pickupPage, 2, 0, "Bring Luger", delegate { Exploits.MurderExploits.BringClueWeapon(2); }, "Bring Luger");
            var bringShotgun = new ExtraDependencies.QMSingleButton(pickupPage, 3, 0, "Bring Shotgun", delegate { Exploits.MurderExploits.BringClueWeapon(3); }, "Bring Shotgun");
            var bringKnife = new ExtraDependencies.QMSingleButton(pickupPage, 4, 0, "Bring Knife", delegate { Exploits.MurderExploits.BringRoleWeapon(4); }, "Bring Knife");
            var bringFrag = new ExtraDependencies.QMSingleButton(pickupPage, 1, 1, "Bring Frag", delegate { Exploits.MurderExploits.BringClueWeapon(5); }, "Bring Frag");
            var bringSmoke = new ExtraDependencies.QMSingleButton(pickupPage, 2, 1, "Bring Smoke", delegate { Exploits.MurderExploits.BringClueWeapon(6); }, "Bring Smoke");


            //Prison Escape
            var pEMenu = new ExtraDependencies.QMNestedButton(tabMenu, 1, 0.5f, "Prison Escape", "Prison Escape Buttons", "Prison Escape", true);

            //Gun Skins
            var globalSkins = new ExtraDependencies.QMNestedButton(pEMenu, 1, 0, "Global Skins", "Global Skins", "Global Skins");

            var pEPurpleSkin = new ExtraDependencies.QMSingleButton(globalSkins, 1, 0, "Purple Skin", delegate { MelonCoroutines.Start(Exploits.PrisonEscapeExploits.PatreonSkin_Select("Purple")); }, "Enable Purple Patron Skin");
            var pERedSkin = new ExtraDependencies.QMSingleButton(globalSkins, 2, 0, "Red Skin", delegate { MelonCoroutines.Start(Exploits.PrisonEscapeExploits.PatreonSkin_Select("Red")); }, "Enable Red Patron Skin");
            var pEBlueSkin = new ExtraDependencies.QMSingleButton(globalSkins, 3, 0, "Blue Skin", delegate { MelonCoroutines.Start(Exploits.PrisonEscapeExploits.PatreonSkin_Select("Blue")); }, "Enable Blue Patron Skin");
            var pEGreenSkin = new ExtraDependencies.QMSingleButton(globalSkins, 4, 0, "Green Skin", delegate { MelonCoroutines.Start(Exploits.PrisonEscapeExploits.PatreonSkin_Select("Green")); }, "Enable Green Patron Skin");
            var pEGoldSkin = new ExtraDependencies.QMSingleButton(globalSkins, 1, 1, "Gold Skin", delegate { MelonCoroutines.Start(Exploits.PrisonEscapeExploits.PatreonSkin_Select("Gold")); }, "Enable Gold Patron Skin");
            var pENoSkin = new ExtraDependencies.QMSingleButton(globalSkins, 2, 1, "No Skin", delegate { MelonCoroutines.Start(Exploits.PrisonEscapeExploits.PatreonSkin_Select("None")); }, "Disable Patron Skin");

            //Removed due to inefficient and lag inducuing code

            //var pEGayGuns = new ExtraDependencies.QMToggleButton(globalSkins, 2.5f, 2, "Gay Guns", delegate { MelonCoroutines.Start(Exploits.PrisonEscapeExploits.RGBPatreonGuns(true, GlobalVariables.pEGayTime)); }, delegate { Exploits.PrisonEscapeExploits.RGBPatreonGuns(false, GlobalVariables.pEGayTime); }, "Enable / Disable Gay Guns");
            //var pEGaySlider = new ExtraDependencies.QMSlider(globalSkins, -510, -800, "Gay Speed", 0.25f, 10f, 1f, delegate (float newValue) { GlobalVariables.pEGayTime = newValue; });

            var pECountdown = new ExtraDependencies.QMSingleButton(pEMenu, 3, 0, "Start Countdown", delegate { Exploits.PrisonEscapeExploits.StartCountdown(); }, "Start game countdown");

            //Bring Guns
            var baseWeapons = new ExtraDependencies.QMNestedButton(pEMenu, 4, 0, "Bring Base Guns", "Bring Base Guns", "Bring Base Guns");

            var bringPistol = new ExtraDependencies.QMSingleButton(baseWeapons, 1, 0, "Bring Pistol", () => Exploits.PrisonEscapeExploits.BringBaseWeapon("Pistol (11)"), "Bring Pistol");
            var bringM4A1 = new ExtraDependencies.QMSingleButton(baseWeapons, 2, 0, "Bring M4A1", () => Exploits.PrisonEscapeExploits.BringBaseWeapon("M4A1 (3)"), "Bring M4A1");
            var bringPEShotgun = new ExtraDependencies.QMSingleButton(baseWeapons, 3, 0, "Bring Shotgun", () => Exploits.PrisonEscapeExploits.BringBaseWeapon("Shotgun (3)"), "Bring Shotgun");
            var bringSMG = new ExtraDependencies.QMSingleButton(baseWeapons, 4, 0, "Bring SMG", () => Exploits.PrisonEscapeExploits.BringBaseWeapon("SMG (3)"), "Bring SMG");
            var bringSniper = new ExtraDependencies.QMSingleButton(baseWeapons, 1, 1, "Bring Sniper", () => Exploits.PrisonEscapeExploits.BringBaseWeapon("Sniper (1)"), "Bring Sniper");

            //Bring Special Weapons
            var pESpecialWeapons = new ExtraDependencies.QMNestedButton(pEMenu, 4, 1, "Bring Special Guns", "Bring Special Guns", "Bring Special Guns");

            var pERPG = new ExtraDependencies.QMSingleButton(pESpecialWeapons, 1, 0, "Bring RPG", () => Exploits.PrisonEscapeExploits.BringRewardGun("Large", "RPG"), "Bring RPG");
            var pEAK = new ExtraDependencies.QMSingleButton(pESpecialWeapons, 2, 0, "Bring AK47", () => Exploits.PrisonEscapeExploits.BringRewardGun("Large", "AK47"), "Bring AK47");
            var pEP90 = new ExtraDependencies.QMSingleButton(pESpecialWeapons, 3, 0, "Bring P90", () => Exploits.PrisonEscapeExploits.BringRewardGun("Large", "P90"), "Bring P90");
            var pEMachete = new ExtraDependencies.QMSingleButton(pESpecialWeapons, 4, 0, "Bring Machete", () => Exploits.PrisonEscapeExploits.BringRewardGun("Large", "Machete"), "Bring Machete");
            var pEBroomstick = new ExtraDependencies.QMSingleButton(pESpecialWeapons, 1, 1, "Bring Broomstick", () => Exploits.PrisonEscapeExploits.BringRewardGun("Large", "Broomstick"), "Bring Broomstick");
            var pELMG = new ExtraDependencies.QMSingleButton(pESpecialWeapons, 2, 1, "Bring LMG", () => Exploits.PrisonEscapeExploits.BringRewardGun("Large", "LMG"), "Bring LMG");

            //World Features
            var worldMenu = new ExtraDependencies.QMNestedButton(tabMenu, 2, 0, "World Menu", "World Functions", "World Functions");
            var joinWorldID = new ExtraDependencies.QMSingleButton(worldMenu, 2, 0, "Join World ID", delegate { ExtraDependencies.WorldWrapper.JoinWorld(Clipboard.GetText()); }, "Join World ID from Clipboard");
            var copyWorldID = new ExtraDependencies.QMSingleButton(worldMenu, 3, 0, "Copy World ID", delegate { Clipboard.SetText(ExtraDependencies.WorldWrapper.GetJoinID()); }, "Copy World ID to Clipboard");
            var playerESP = new ExtraDependencies.QMToggleButton(worldMenu, 2.5f, 1, "Player ESP", delegate
            {
                GlobalVariables.playerESP = true;
                Features.ESPHandler.ApplyAllPlayerWallhack();
            }, delegate {
                GlobalVariables.playerESP = false;
                Features.ESPHandler.RemoveAllPlayerWallhack();
            }, "Player ESP / Wallhack");

            //General Functions
            var functionMenu = new ExtraDependencies.QMNestedButton(tabMenu, 3, 0, "Functions", "Functions", "Functions");
            var flightToggle = new ExtraDependencies.QMToggleButton(functionMenu, 1, 0, "Flight", delegate { Exploits.FlightControl.EnableFlightMethod2(true); }, delegate { Exploits.FlightControl.EnableFlightMethod2(false); }, "Enable / Disable Flight");
            var swapAvatar = new ExtraDependencies.QMSingleButton(functionMenu, 2, 0, "Swap Avatar", delegate { 
                if(Clipboard.GetText().Contains("avtr_"))
                {
                    ExtraDependencies.PlayerWrapper.ChangeAvatar(Clipboard.GetText());
                } else
                {
                    MelonLogger.Msg("Not an avatar");
                }
            }, "Swap into avatar ID from Clipboard");
            var toggleDownloads = new ExtraDependencies.QMToggleButton(functionMenu, 3, 0, "Toggle Avatar Queue", delegate { Features.HideSelf.PerformHide(true); }, delegate { Features.HideSelf.PerformHide(false); }, "Toggle Avatar Queue");
            var flightSpeed = new ExtraDependencies.QMSlider(functionMenu, -510, -750, "Flight Speed", 1f, 100f, 10f, delegate (float newValue) { GlobalVariables.flySpeed = newValue; });

            var mirrorToggle = new ExtraDependencies.QMToggleButton(functionMenu, 4, 0, "Spawn Mirror", delegate { Features.MirrorLogic.SpawnMirror(0, GlobalVariables.currentMirrorX, GlobalVariables.currentMirrorY); }, delegate { Features.MirrorLogic.RemoveMirror(0); }, "Spawn / Remove Mirror");
            var mirrorXSlider = new ExtraDependencies.QMSlider(functionMenu, -510, -850, "Mirror Width", 0.5f, 10f, 5f, delegate (float newValue) { GlobalVariables.currentMirrorX = newValue; });
            var mirrorYSlider = new ExtraDependencies.QMSlider(functionMenu, -510, -950, "Mirror Height", 0.5f, 10f, 5f, delegate (float newValue) { GlobalVariables.currentMirrorY = newValue; });

            var deletePortals = new ExtraDependencies.QMSingleButton(functionMenu, 2f, 1, "Delete Portals", delegate { Features.PortalDeleter.DeleteAllPortals(true); }, "Delete all portals");
            var toggleDelPortals = new ExtraDependencies.QMToggleButton(functionMenu, 3f, 1, "Delete Portals (1s)", delegate {
                GlobalVariables.timerDelPortals = true;
                togglePortalRoutine = MelonCoroutines.Start(Features.PortalDeleter.TimerPortalDelete());
            }, delegate {
                if(GlobalVariables.timerDelPortals)
                {
                    GlobalVariables.timerDelPortals = false;
                    MelonCoroutines.Stop(togglePortalRoutine);
                }
            }, "Toggle Delete portals");

            //Block Ranks
            var blockRanks = new ExtraDependencies.QMNestedButton(tabMenu, 4, 1, "Block by Rank", "Block users of selected ranking", "Rank Blocking Menu");
            var visitorBlock = new ExtraDependencies.QMToggleButton(blockRanks, 1, 0, "Block Visitors", delegate { Exploits.BlockRanks.BlockRankCall("Visitor"); }, delegate { Exploits.BlockRanks.BlockRankCall("Visitor"); }, "Block all visitor rank players in the lobby");
            var newUserBlock = new ExtraDependencies.QMToggleButton(blockRanks, 2, 0, "Block New Users", delegate { Exploits.BlockRanks.BlockRankCall("New User"); }, delegate { Exploits.BlockRanks.BlockRankCall("NewUser"); }, "Block all new user rank players in the lobby");
            var userBlock = new ExtraDependencies.QMToggleButton(blockRanks, 3, 0, "Block Users", delegate { Exploits.BlockRanks.BlockRankCall("User"); }, delegate { Exploits.BlockRanks.BlockRankCall("User"); }, "Block all user rank players in the lobby");
            var knownUserBlock = new ExtraDependencies.QMToggleButton(blockRanks, 4, 0, "Block Known Users", delegate { Exploits.BlockRanks.BlockRankCall("KnownUser"); }, delegate { Exploits.BlockRanks.BlockRankCall("KnownUser"); }, "Block all known user rank players in the lobby");
            var trustedUserBlock = new ExtraDependencies.QMToggleButton(blockRanks, 1, 1, "Block Trusted Users", delegate { Exploits.BlockRanks.BlockRankCall("TrustedUser"); }, delegate { Exploits.BlockRanks.BlockRankCall("TrustedUser"); }, "Block all trusted user rank players in the lobby");
            var isolateSelfToggle = new ExtraDependencies.QMToggleButton(blockRanks, 2, 1, "Isolate Self", delegate { Exploits.BlockRanks.BlockRankCall("ALL"); }, delegate { Exploits.BlockRanks.BlockRankCall("ALL"); }, "Block everyone in the lobby. (6 feet and all that)");

            //Settings
            var settingsMenu = new ExtraDependencies.QMNestedButton(tabMenu, 1, 3, "Settings", "Settings", "Settings");

            var espCol = new ExtraDependencies.QMToggleButton(settingsMenu, 1, 0, "Rainbow ESP", delegate { GlobalVariables.rainbowESP = true; }, delegate { GlobalVariables.rainbowESP = false; }, "Rainbow ESP");

            //Nameplate
            //var updateNameplates = new ExtraDependencies.QMSingleButton(tabMenu, 2.5f, 1, "Update Nameplates", delegate { MelonCoroutines.Start(Features.LemonNameplate.AttachTrustRank()); }, "Update nameplates");

            //Target Menu
            var targetMenu = new ExtraDependencies.QMNestedButton("Menu_SelectedUser_Local", 1, 3, "Test Button", "Test", "Test Menu");
            var selectedUserPlayer = new ExtraDependencies.QMSingleButton("Menu_SelectedUser_Local", 2, -0.5f, "Select User", delegate {
                var selectedIUserQM = ExtraDependencies.PlayerWrapper.GetSelectedUser(GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local").GetComponent<SelectedUserMenuQM>());
                GlobalVariables.selectedPlayer = ExtraDependencies.PlayerWrapper.GetPlayerByName(selectedIUserQM.prop_String_1);
                ExtraDependencies.APIUtils.ShowAlert(ExtraDependencies.APIUtils.GetQuickMenuInstance(), "Selected User: " + GlobalVariables.selectedPlayer.prop_String_0);
                MelonLogger.Msg("Selected User: " + GlobalVariables.selectedPlayer.prop_String_0);

            }, "Select User for SelFucntions", true);

            var targetMurderMenu = new ExtraDependencies.QMNestedButton(targetMenu, 1, 0, "Murder 4", "Murder 4", "Murder 4");
            var m4TargetExplode = new ExtraDependencies.QMSingleButton(targetMurderMenu, 1, 0, "Target Explode", delegate { Exploits.MurderExploits.TargetBoom(); }, "Explode target player");

            var forceCloneButton = new ExtraDependencies.QMSingleButton(targetMenu, 1, 1, "Force Clone", delegate { Features.SelUserFunctions.ForceClone(GlobalVariables.selectedPlayer); }, "Force Clone selected Player");
            var teleportToUser = new ExtraDependencies.QMSingleButton(targetMenu, 2, 1, "Teleport to User", delegate { Features.SelUserFunctions.TeleportToPlayer(GlobalVariables.selectedPlayer); }, "Teleport to selected Player");
            var copyAvatarID = new ExtraDependencies.QMSingleButton(targetMenu, 4, 1, "Copy Avatar ID", delegate { Features.SelUserFunctions.CopyAvatarID(GlobalVariables.selectedPlayer); }, "Copy avatar ID of selected player", true);
            var copyUserID = new ExtraDependencies.QMSingleButton(targetMenu, 4, 1.5f, "Copy User ID", delegate { Features.SelUserFunctions.CopyUserID(GlobalVariables.selectedPlayer); }, "Copy user ID of selected player", true);


        }

        internal static void LoadAssetBundle(string filePath)
        {
            GlobalVariables.cachedAssetBundle = AssetBundle.LoadFromMemory_Internal(File.ReadAllBytes(filePath), 0u);
            GlobalVariables.cachedAssetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }

        internal static Texture2D LoadTexture(string textureName)
        {
            if (GlobalVariables.cachedAssetBundle == null)
            {
                return null;
            }
            string text = "assets/bundledassets/lemonclientbundle/" + textureName + ".png";
            if (GlobalVariables.assetCache.ContainsKey(text))
            {
                return (Texture2D)GlobalVariables.assetCache[text];
            }
            Texture2D texture2D = GlobalVariables.cachedAssetBundle.LoadAsset_Internal(text, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            texture2D.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            GlobalVariables.assetCache.Add(text, texture2D);
            return texture2D;
        }

        internal static Sprite MakeSpriteFromAssetBundle(Texture2D lemonTexture)
        {
            var sprite = Sprite.CreateSprite(lemonTexture, new Rect(0f, 0f, lemonTexture.width, lemonTexture.height), new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, default, false);
            sprite.hideFlags += 32;
            return sprite;
        }

    }
}
