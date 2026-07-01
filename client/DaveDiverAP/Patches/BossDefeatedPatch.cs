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
    public static class BossDefeatedPatch
    {
        // Hook CommonBossDead.DoJob() — fires for ALL bosses via the BossSceneSO job system
        // Read BossScene.Current.bossType (EnumBossFishType) to identify which boss died

        // Guard flag: set by void prefix, read by postfix to skip processing when scene is null.
        // We use a void prefix (NOT bool-returning) because bool-returning prefixes generate
        // invalid IL on IL2CPP inherited methods and crash the game on startup.
        // A void prefix cannot prevent the original from running, so the NullRef in DoJob will
        // still fire ONCE — but _sceneWasNull lets the postfix skip its own logic cleanly,
        // which should also break any retry loop driven by our patch.
        private static bool _sceneWasNull = false;

        [HarmonyPatch(typeof(CommonBossDead), "DoJob")]
        [HarmonyPrefix]
        public static void OnBossDefeated_Prefix()
        {
            _sceneWasNull = (BossScene.Current == null);
            if (_sceneWasNull)
                Plugin.Log.LogWarning("[Boss] CommonBossDead.DoJob called with no active BossScene — will skip postfix.");
        }

        [HarmonyPatch(typeof(CommonBossDead), "DoJob")]  // same method — Harmony deduplicates by method target, this is fine
        [HarmonyPostfix]
        public static void OnBossDefeated_Postfix()
        {
            try
            {
                // Skip if prefix detected no active scene (void prefix can't stop original,
                // but we can skip our own logic here)
                if (_sceneWasNull) return;

                if (!ArchipelagoClient.IsConnected) return;

                var scene = BossScene.Current;
                if (scene == null) return;

                // Primary: use EnumBossFishType enum (confirmed exact values)
                var locationName = BossNameMapper.GetLocationName((int)scene.bossType);

                // Fallback: use bossSceneSO.name substring match for bosses not in enum
                // TODO: bossSceneSO requires Sirenix.Serialization.dll — disabled until added to lib
                // if (locationName == null && scene.bossSceneSO != null)
                //     locationName = BossNameMapper.GetDisplayNameFromScene(scene.bossSceneSO.name);

                if (locationName != null)
                    LocationTracker.OnBossDefeated(locationName);
                else
                    Plugin.Log.LogWarning($"[Boss] Unknown boss type: {scene.bossType}");
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[BossDefeatedPatch] OnBossDefeated_Postfix threw: {ex}");
            }
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
            // Jungle DLC bosses — names confirmed from mission dump (2026-06-27)
            { 201, "Defeat: Stethacanthus" },        // Jungle boss
            { 202, "Defeat: Xiphactinus" },          // "The Tyrant Xiphactinus" main story mission
            { 203, "Defeat: Sulong" },               // "Operation: Sulong Hunt" side mission
            { 204, "Defeat: Snapping Turtle" },      // "A Monster Snapping Turtle?" side mission
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

        public static string? GetLocationName(int bossType) =>
            _enumMap.TryGetValue(bossType, out var name) ? name : null;

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
