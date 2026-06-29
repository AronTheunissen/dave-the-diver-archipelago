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
        // TODO: EcoWatcherDeliverPopup.OnDREvent not found in current interop.
        // Use UnityExplorer to find the correct class/method for ecowatcher deliver confirmation.
        //
        // [HarmonyPatch(typeof(EcoWatcherDeliverPopup), "OnDREvent")]
        // [HarmonyPostfix]
        // public static void OnEcowatcherDeliver_Postfix(EcoWatcherDeliverPopup __instance)
        // {
        //     if (!ArchipelagoClient.IsConnected) return;
        //     if (__instance.cell?.CellData != null)
        //     {
        //         var missionName = __instance.cell.CellData.ToString();
        //         LocationTracker.OnEcowatcherMissionCompleted(missionName);
        //     }
        // }

        // TODO: EcoWatcherResearchRankUpPopup.OnDREvent not found in current interop.
        // Use UnityExplorer to find the correct class/method for ecowatcher rank-up popup.
        //
        // [HarmonyPatch(typeof(EcoWatcherResearchRankUpPopup), "OnDREvent")]
        // [HarmonyPostfix]
        // public static void OnEcowatcherLevelUp_Postfix()
        // {
        //     if (!ArchipelagoClient.IsConnected) return;
        //     // TODO: GetEcowatcherLevel not yet implemented in LocationTracker
        //     // LocationTracker.OnEcowatcherLevelUp(LocationTracker.GetEcowatcherLevel());
        // }
    }
}
