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
        // Fires when any challenge is completed
        [HarmonyPatch(typeof(ChallengeManager), "OnChallengeComplete")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnChallengeComplete_Postfix(string challengeId)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var locationName = ChallengeNameMapper.GetLocationName(challengeId);
            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
        }
    }

    public static class ChallengeNameMapper
    {
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new()
        {
            // TODO: Fill in with real challenge IDs from Il2CppDumper
            // { "CHALLENGE_CATCH_5_FISH", "Challenge: Catch 5 Fish in 60 Seconds" },
            // { "CHALLENGE_EARN_1000G",   "Challenge: Earn 1000g in One Dive" },
            // { "CHALLENGE_DEFEAT_SHARKS","Challenge: Defeat 3 Sharks Without Taking Damage" },
            // { "CHALLENGE_HARPOON_10",   "Challenge: Kill 10 Fish with Harpoon Only" },
            // { "CHALLENGE_MELEE_10",     "Challenge: Kill 10 Fish with Melee Only" },
            // { "CHALLENGE_NET_20",       "Challenge: Net Gun 20 Fish Alive" },
            // { "CHALLENGE_SERVE_10",     "Challenge: Serve 10 Customers with Perfect Timing" },
            // { "CHALLENGE_NO_OXYGEN",    "Challenge: Complete a Dive Without Using Oxygen Refills" },
            // { "CHALLENGE_MAX_DEPTH",    "Challenge: Reach Max Depth Without Equipment Damage" },
        };

        public static string? GetLocationName(string challengeId) =>
            _map.TryGetValue(challengeId, out var name) ? name : null;
    }
}
