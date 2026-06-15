using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the diving/combat challenge system.
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "Challenge", "DiveChallenge", "ChallengeManager"
    /// </summary>
    [HarmonyPatch]
    public static class ChallengePatch
    {
        // ✅ CONFIRMED via dump.cs: MissionManager.UpdateMission(MissionClearType type, int target, int count)
        //    is the unified mission update hub. Challenges use a specific MissionClearType.
        //    We hook MissionManager.UpdateMission and filter for challenge-type missions
        //    by checking if the target TID matches our challenge map.
        [HarmonyPatch(typeof(MissionManager), "UpdateMission")]
        [HarmonyPostfix]
        public static void OnChallengeComplete_Postfix(MissionClearType type, int target, int count)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var locationName = ChallengeNameMapper.GetLocationName(target);
            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
        }
    }

    public static class ChallengeNameMapper
    {
        // Maps mission TID integers to AP location names for challenges.
        // TODO: Cross-reference challenge mission TIDs from the game's mission design sheet.
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // Example layout — replace with real TIDs:
            // { 40001, "Challenge: Catch 5 Fish in 60 Seconds" },
            // { 40002, "Challenge: Earn 1000g in One Dive" },
            // { 40003, "Challenge: Defeat 3 Sharks Without Taking Damage" },
            // { 40004, "Challenge: Kill 10 Fish with Harpoon Only" },
            // { 40005, "Challenge: Kill 10 Fish with Melee Only" },
            // { 40006, "Challenge: Net Gun 20 Fish Alive" },
            // { 40007, "Challenge: Serve 10 Customers with Perfect Timing" },
            // { 40008, "Challenge: Complete a Dive Without Using Oxygen Refills" },
            // { 40009, "Challenge: Reach Max Depth Without Equipment Damage" },
        };

        public static string? GetLocationName(int missionTID) =>
            _map.TryGetValue(missionTID, out var name) ? name : null;
    }
}
