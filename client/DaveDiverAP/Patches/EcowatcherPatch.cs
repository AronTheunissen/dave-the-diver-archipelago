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
        // ✅ CONFIRMED via dump.cs: MissionManager.UpdateMission(MissionClearType type, int target, int count)
        //    is the central hub for ALL mission updates including Ecowatcher tasks.
        //    EcoWatcher missions use MissionClearType values to categorize the task type.
        //    We hook EcoWatcherDeliverPopup which fires when the player delivers items
        //    to complete an Ecowatcher task (the "deliver" confirmation popup).
        [HarmonyPatch(typeof(EcoWatcherDeliverPopup), "OnDREvent")]  // fires on deliver confirm
        [HarmonyPostfix]
        public static void OnEcowatcherDeliver_Postfix(EcoWatcherDeliverPopup __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;
            // Get the mission name from the cell data
            if (__instance.cell?.CellData != null)
            {
                var missionName = __instance.cell.CellData.ToString();
                LocationTracker.OnEcowatcherMissionCompleted(missionName);
            }
        }

        // Fires when Ecowatcher research level increases (Level 2-5 grant charms)
        // ✅ CONFIRMED via dump.cs: EcoWatcherResearchRankUpPopup fires on rank up
        [HarmonyPatch(typeof(EcoWatcherResearchRankUpPopup), "OnDREvent")]
        [HarmonyPostfix]
        public static void OnEcowatcherLevelUp_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            // Level-up detected — notify tracker (it tracks current level internally)
            // TODO: GetEcowatcherLevel not yet implemented in LocationTracker
            // LocationTracker.OnEcowatcherLevelUp(LocationTracker.GetEcowatcherLevel());
        }
    }
}
