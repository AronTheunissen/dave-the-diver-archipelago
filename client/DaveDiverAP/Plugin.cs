using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using DaveDiverAP.Patches;
using DaveDiverAP.UI;
using System;

namespace DaveDiverAP
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Log { get; private set; } = null!;
        public static Plugin Instance { get; private set; } = null!;

        private Harmony? _harmony;
        private GameObject? _uiObject;

        public override void Load()
        {
            Instance = this;
            Log = base.Log;

            Log.LogInfo($"Dave the Diver Archipelago v{MyPluginInfo.PLUGIN_VERSION} loading...");

            // Load BepInEx config file
            ModConfig.Initialize(Config);

            // Apply Harmony patches
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(typeof(FishCatchPatch));
            _harmony.PatchAll(typeof(RecipeUnlockPatch));
            _harmony.PatchAll(typeof(BossDefeatedPatch));
            _harmony.PatchAll(typeof(StoryProgressPatch));
            _harmony.PatchAll(typeof(WeaponCraftPatch));
            _harmony.PatchAll(typeof(PlayerDeathPatch));
            _harmony.PatchAll(typeof(GameStatePatch));
            _harmony.PatchAll(typeof(CookstaPatch));
            _harmony.PatchAll(typeof(PhotographyPatch));
            _harmony.PatchAll(typeof(ChallengePatch));
            _harmony.PatchAll(typeof(EcowatcherPatch));
            _harmony.PatchAll(typeof(FarmPatch));
            _harmony.PatchAll(typeof(MinigamePatch));
            _harmony.PatchAll(typeof(RestaurantPatch));
            _harmony.PatchAll(typeof(CollectiblePatch));
            _harmony.PatchAll(typeof(IngredientPatch));
            _harmony.PatchAll(typeof(CharmPatch));
            _harmony.PatchAll(typeof(SaveLoadPatch));

            Log.LogInfo("Harmony patches applied.");

            // Initialize the Archipelago client
            ArchipelagoClient.Initialize();

            // Create persistent UI GameObject (survives scene changes)
            // All MonoBehaviours go on this single object
            _uiObject = new GameObject("ArchipelagoUI");
            Object.DontDestroyOnLoad(_uiObject);
            _uiObject.AddComponent<ConnectionUI>();
            _uiObject.AddComponent<NotificationManager>();
            _uiObject.AddComponent<ItemQueue>();

            Log.LogInfo("Connection UI created. Press F9 to open.");

            // Auto-connect if configured
            if (ModConfig.AutoConnectOnLaunch.Value)
            {
                var (url, port, slot, pass) = SaveData.LoadConnectionInfo();
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(slot))
                {
                    Log.LogInfo("Auto-connecting to Archipelago...");
                    _ = ArchipelagoClient.ConnectAsync(url, port, slot, pass);
                }
            }

            Log.LogInfo("Dave the Diver Archipelago loaded successfully!");
        }

        public override bool Unload()
        {
            _harmony?.UnpatchSelf();
            ArchipelagoClient.Disconnect();
            DeathLinkHandler.Dispose();
            if (_uiObject != null)
                Object.Destroy(_uiObject);
            return base.Unload();
        }
    }

    internal static class MyPluginInfo
    {
        public const string PLUGIN_GUID = "com.davethediver.archipelago";
        public const string PLUGIN_NAME = "Dave the Diver Archipelago";
        public const string PLUGIN_VERSION = "0.1.0";
    }
}
