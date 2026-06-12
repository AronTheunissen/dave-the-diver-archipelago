using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the Ecowatcher app to detect mission completions and level-ups.
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "Ecowatcher", "EcoWatcher", "MarineResearch", "ResearchMission"
    /// </summary>
    [HarmonyPatch]
    public static class EcowatcherPatch
    {
        // Fires when an Ecowatcher mission is completed
        [HarmonyPatch(typeof(EcowatcherManager), "OnMissionComplete")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnMissionComplete_Postfix(string missionName)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnEcowatcherMissionCompleted(missionName);
        }

        // Fires when the Ecowatcher app levels up
        [HarmonyPatch(typeof(EcowatcherManager), "set_Level")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnLevelUp_Postfix(int value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnEcowatcherLevelUp(value);
        }
    }
}
