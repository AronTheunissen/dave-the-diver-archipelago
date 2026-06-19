using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the game's boss defeat system to detect boss kills.
    ///
    /// ✅ CONFIRMED via dump.cs:
    /// - CommonBossDead.DoJob() fires for ALL bosses via BossSceneSO job system
    /// - BossScene.Current is the static reference to the active boss scene
    /// - BossScene.bossType is EnumBossFishType enum (confirmed values below)
    /// - FinishBossScene() only exists on EbirahBattleScene (NOT base BossScene)
    ///
    /// EnumBossFishType confirmed values:
    ///   GiantSquid=1, HermitCrab=2, WolfFish=3, Clione=4, JW2=5, Gardon=6,
    ///   MantisShrimp=7, GoblinShark=8, Helicoprion=9, GreatWhiteShark=10,
    ///   Anomalocaris=11, Lusca=12, Ebirah=100,
    ///   Jungle DLC: Stethacanthus=201, Xiphactinus=202, Sulong=203, SnappingTurtle=204
    ///
    /// NOTE: Kronosaurus and Phantom Jellyfish are missing from EnumBossFishType!
    /// They may use bossSceneSO.name fallback instead.
    /// </summary>
    [HarmonyPatch]
    public static class BossDefeatedPatch
    {
        // Hook CommonBossDead.DoJob() — fires for ALL bosses via the BossSceneSO job system
        // Read BossScene.Current.bossType (EnumBossFishType) to identify which boss died
        [HarmonyPatch(typeof(CommonBossDead), "DoJob")]
        [HarmonyPostfix]
        public static void OnBossDefeated_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;

            var scene = BossScene.Current;
            if (scene == null) return;

            // Primary: use EnumBossFishType enum (confirmed exact values)
            var locationName = BossNameMapper.GetLocationName(scene.bossType);

            // Fallback: use bossSceneSO.name substring match for bosses not in enum
            if (locationName == null && scene.bossSceneSO != null)
                locationName = BossNameMapper.GetDisplayNameFromScene(scene.bossSceneSO.name);

            if (locationName != null)
                LocationTracker.OnBossDefeated(locationName);
            else
                Plugin.Log.LogWarning($"[Boss] Unknown boss type: {scene.bossType} / SO: {scene.bossSceneSO?.name}");
        }
    }

    public static class BossNameMapper
    {
        // ✅ CONFIRMED via dump.cs: EnumBossFishType exact integer values
        // Primary lookup — maps enum int to AP location name
        private static readonly System.Collections.Generic.Dictionary<int, string> _enumMap = new()
        {
            { 1,   "Defeat: Giant Squid" },
            { 2,   "Defeat: Truck Hermit Crab" },
            { 3,   "Defeat: Giant Wolf Eel" },
            { 4,   "Defeat: Clione Queen" },
            { 5,   "Defeat: John Watson" },
            { 6,   "Defeat: Giant Gadon" },
            { 7,   "Defeat: Mantis Shrimp" },
            { 8,   "Defeat: Goblin Shark" },
            { 9,   "Defeat: Helicoprion" },
            { 10,  "Defeat: Great White Shark Klaus" },
            { 11,  "Defeat: Yawie (Final Boss)" },
            { 12,  "Defeat: Lusca" },
            { 100, "Defeat: Ebirah" },
            // Jungle DLC bosses (201-204) — names TBD
            // { 201, "Defeat: Stethacanthus" },
            // { 202, "Defeat: Xiphactinus" },
            // { 203, "Defeat: Sulong" },
            // { 204, "Defeat: Snapping Turtle" },
        };

        // Fallback: substring match on bossSceneSO.name for bosses not in EnumBossFishType
        // (Phantom Jellyfish, Kronosaurus, John Watson Rematch, Torben are missing from enum)
        private static readonly System.Collections.Generic.Dictionary<string, string> _nameMap = new(System.StringComparer.OrdinalIgnoreCase)
        {
            { "PhantomJelly",    "Defeat: Phantom Jellyfish" },
            { "Kronosaurus",     "Defeat: Kronosaurus" },
            { "JW3",             "Defeat: John Watson" },   // rematch — same AP location
            { "Torben",          "Defeat: Torben" },
        };

        public static string? GetLocationName(EnumBossFishType bossType) =>
            _enumMap.TryGetValue((int)bossType, out var name) ? name : null;

        public static string? GetDisplayNameFromScene(string sceneName)
        {
            foreach (var kvp in _nameMap)
                if (sceneName.Contains(kvp.Key, System.StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            return null;
        }

        public static string? GetDisplayName(string bossId) => GetDisplayNameFromScene(bossId);
    }
}
