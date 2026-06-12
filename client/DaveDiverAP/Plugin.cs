using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using DaveDiverAP.Patches;

namespace DaveDiverAP
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Log { get; private set; } = null!;
        public static Plugin Instance { get; private set; } = null!;

        private Harmony? _harmony;

        public override void Load()
        {
            Instance = this;
            Log = base.Log;

            Log.LogInfo($"Dave the Diver Archipelago v{MyPluginInfo.PLUGIN_VERSION} loading...");

            // Apply Harmony patches
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(typeof(FishCatchPatch));
            _harmony.PatchAll(typeof(RecipeUnlockPatch));
            _harmony.PatchAll(typeof(BossDefeatedPatch));
            _harmony.PatchAll(typeof(StoryProgressPatch));
            _harmony.PatchAll(typeof(WeaponCraftPatch));

            Log.LogInfo("Harmony patches applied.");

            // Initialize the Archipelago client (connects when the player starts a game)
            ArchipelagoClient.Initialize();

            Log.LogInfo("Dave the Diver Archipelago loaded successfully!");
        }

        public override bool Unload()
        {
            _harmony?.UnpatchSelf();
            ArchipelagoClient.Disconnect();
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
