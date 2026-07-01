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

            // Register managed MonoBehaviour types with IL2CPP runtime
            try
            {
                Il2CppInterop.Runtime.Injection.ClassInjector.RegisterTypeInIl2Cpp<ConnectionUI>();
                Il2CppInterop.Runtime.Injection.ClassInjector.RegisterTypeInIl2Cpp<NotificationManager>();
                Il2CppInterop.Runtime.Injection.ClassInjector.RegisterTypeInIl2Cpp<ItemQueue>();
                Log.LogInfo("IL2CPP types registered.");
            }
            catch (Exception ex)
            {
                Log.LogWarning($"Failed to register IL2CPP types: {ex.Message}");
            }

            // Apply Harmony patches — each wrapped in try/catch so one failure doesn't crash the mod
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            var patchTypes = new System.Type[]
            {
                typeof(FishCatchPatch),
                typeof(RecipeUnlockPatch),
                typeof(BossDefeatedPatch),
                typeof(StoryProgressPatch),
                typeof(WeaponCraftPatch),
                typeof(PlayerDeathPatch),
                typeof(GameStatePatch),
                typeof(CookstaPatch),
                typeof(PhotographyPatch),
                typeof(EcowatcherPatch),
                typeof(FarmPatch),
                typeof(MinigamePatch),
                typeof(RestaurantPatch),
                typeof(CollectiblePatch),
                typeof(IngredientPatch),
                typeof(CharmPatch),
                typeof(JungleFishingPatch),
                typeof(SaveLoadPatch),
            };
            foreach (var patchType in patchTypes)
            {
                try { _harmony.PatchAll(patchType); }
                catch (Exception ex) { Log.LogWarning($"[Harmony] Failed to patch {patchType.Name}: {ex.Message}"); }
            }

            Log.LogInfo("Harmony patches applied.");

            // Initialize the Archipelago client
            ArchipelagoClient.Initialize();

            // Create persistent UI GameObject (survives scene changes)
            _uiObject = new GameObject("ArchipelagoUI");
            UnityEngine.Object.DontDestroyOnLoad(_uiObject);
            _uiObject.AddComponent<ConnectionUI>();
            _uiObject.AddComponent<NotificationManager>();
            _uiObject.AddComponent<ItemQueue>();

            Log.LogInfo("Connection UI created.");

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
                UnityEngine.Object.Destroy(_uiObject);
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
