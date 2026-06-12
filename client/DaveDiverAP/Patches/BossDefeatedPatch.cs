using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the game's boss defeat system to detect boss kills.
    ///
    /// IMPORTANT: Class/method names are PLACEHOLDERS.
    /// Decompile Assembly-CSharp.dll to find the actual names.
    /// Look for: BossManager, BossController, EnemyDeathHandler, or similar.
    /// The boss death callback likely fires when HP reaches 0 and a cutscene triggers.
    /// </summary>
    [HarmonyPatch]
    public static class BossDefeatedPatch
    {
        // TODO: Replace with actual class and method names
        [HarmonyPatch(typeof(BossManager), "OnBossDefeated")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnBossDefeated_Postfix(string bossId)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var bossName = BossNameMapper.GetDisplayName(bossId);
            if (bossName != null)
                LocationTracker.OnBossDefeated(bossName);
        }
    }

    public static class BossNameMapper
    {
        // TODO: Fill in with real internal boss IDs from decompiled game
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new()
        {
            // { "BOSS_GIANT_SQUID",       "Giant Squid" },
            // { "BOSS_CLIONE_QUEEN",       "Clione Queen" },
            // { "BOSS_HERMIT_CRAB",        "Truck Hermit Crab" },
            // { "BOSS_WOLF_EEL",           "Giant Wolf Eel" },
            // { "BOSS_GOBLIN_SHARK",       "Goblin Shark" },
            // { "BOSS_PHANTOM_JELLYFISH",  "Phantom Jellyfish" },
            // { "BOSS_GIANT_GADON",        "Giant Gadon" },
            // { "BOSS_HELICOPRION",        "Helicoprion" },
            // { "BOSS_KRONOSAURUS",        "Kronosaurus" },
            // { "BOSS_JOHN_WATSON",        "John Watson" },
            // { "BOSS_EBIRAH",             "Ebirah" },
            // { "BOSS_KLAUS",              "Klaus" },
            // { "BOSS_MANTIS_SHRIMP",      "Mantis Shrimp" },
            // { "BOSS_LUSCA",              "Lusca" },
            // { "BOSS_TORBEN",             "Torben" },
            // { "BOSS_YAWIE",              "Yawie" },
        };

        public static string? GetDisplayName(string bossId)
        {
            return _map.TryGetValue(bossId, out var name) ? name : null;
        }
    }
}
