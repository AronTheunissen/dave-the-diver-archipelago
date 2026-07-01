using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the game's boss defeat system to detect boss kills.
    ///
    /// ✅ CONFIRMED via dump.cs:
    /// - BossScene : DRMonoBehaviour — base class for all boss encounters
    /// - BossScene.bossType is EnumBossFishType enum (confirmed values below)
    /// - BossScene.Current is the static ref to the active boss scene
    ///
    /// ⚠️ WHY WE DON'T HOOK CommonBossDead.DoJob:
    /// Any Harmony patch on CommonBossDead.DoJob installs an IL2CPP trampoline.
    /// During scene teardown and save/load, the game calls DoJob with a dangling/freed
    /// object pointer. The trampoline then crashes with AccessViolationException trying
    /// to call il2cpp_object_get_class() on the invalid pointer — before our code runs.
    /// This kills the process. Prefix or postfix makes no difference.
    ///
    /// ✅ SAFE ALTERNATIVE: Hook BossScene.OnDestroy
    /// BossScene is a MonoBehaviour — Unity calls OnDestroy() cleanly when the scene
    /// object is destroyed. At that point, __instance is a valid managed object and
    /// __instance.bossType gives us exactly what we need. This only fires for real
    /// boss scene teardowns, not spurious job system calls.
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
        // Hook BossScene.OnDestroy — fires when a boss scene MonoBehaviour is destroyed.
        // __instance.bossType gives us the boss identity from the valid managed object.
        // This is safe because Unity calls OnDestroy() with a valid this pointer,
        // unlike CommonBossDead.DoJob which can be called with garbage pointers.
        //
        // Caveat: OnDestroy fires on ALL scene teardowns, not just victories.
        // We use ItemQueue.IsGameReady as a coarse guard (not during loading),
        // but we may also fire if the player dies and the boss scene is cleaned up.
        // LocationTracker.OnBossDefeated should be idempotent (already-sent check).

        [HarmonyPatch(typeof(BossScene), "OnDestroy")]
        [HarmonyPostfix]
        public static void OnBossSceneDestroyed_Postfix(BossScene __instance)
        {
            try
            {
                if (!ItemQueue.IsGameReady) return;
                if (!ArchipelagoClient.IsConnected) return;

                var locationName = BossNameMapper.GetLocationName((int)__instance.bossType);

                if (locationName != null)
                    LocationTracker.OnBossDefeated(locationName);
                else
                    Plugin.Log.LogWarning($"[Boss] Unknown boss type: {__instance.bossType}");
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[BossDefeatedPatch] OnBossSceneDestroyed_Postfix threw: {ex}");
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
