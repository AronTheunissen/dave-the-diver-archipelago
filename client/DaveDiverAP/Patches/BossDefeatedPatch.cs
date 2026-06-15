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
        // ✅ CONFIRMED via dump.cs: BossScene is the real class (public class BossScene : DRMonoBehaviour)
        // ✅ CONFIRMED via dump.cs: FinishBossScene() is the method called when a boss dies
        //    BossScene.Current is the static reference to the active boss scene.
        //    The boss type can be identified from BossScene.bossSceneSO or the class name.
        [HarmonyPatch(typeof(BossScene), "FinishBossScene")]
        [HarmonyPostfix]
        public static void OnBossDefeated_Postfix(BossScene __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // Identify the boss from the BossScene's SO name or the active boss class type.
            // bossSceneSO.name typically matches the boss name in the design data.
            string bossSceneName = __instance.bossSceneSO != null ? __instance.bossSceneSO.name : __instance.gameObject.name;
            var bossName = BossNameMapper.GetDisplayNameFromScene(bossSceneName);
            if (bossName != null)
                LocationTracker.OnBossDefeated(bossName);
        }
    }

    public static class BossNameMapper
    {
        // Maps BossScene SO/GameObject name substrings to AP location display names.
        // Boss class names confirmed via dump.cs (SABossControllerBase subclasses):
        // BossClioneController, BossWolffishController, BossGoblinSharkController,
        // BossGreatWhiteSharkController, BossHelicoprionController, HermitCrabController,
        // BossJW2Controller (John Watson), BossJW3Controller, BossLuscaController,
        // BossMantisShrimpController, BossKronosaurus, SABossAnomalocaris (Yawie),
        // SABossEbirah, BossGiantGardonController
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new(System.StringComparer.OrdinalIgnoreCase)
        {
            { "GiantSquid",      "Boss: Giant Squid" },
            { "Clione",          "Boss: Clione Queen" },
            { "HermitCrab",      "Boss: Truck Hermit Crab" },
            { "Wolffish",        "Boss: Giant Wolf Eel" },
            { "WolfEel",         "Boss: Giant Wolf Eel" },
            { "GoblinShark",     "Boss: Goblin Shark" },
            { "PhantomJelly",    "Boss: Phantom Jellyfish" },
            { "Gardon",          "Boss: Giant Gadon" },
            { "Helicoprion",     "Boss: Helicoprion" },
            { "Kronosaurus",     "Boss: Kronosaurus" },
            { "JW2",             "Boss: John Watson" },
            { "JohnWatson",      "Boss: John Watson" },
            { "JW3",             "Boss: John Watson (Rematch)" },
            { "Ebirah",          "Boss: Ebirah" },
            { "GreatWhite",      "Boss: Klaus" },
            { "Klaus",           "Boss: Klaus" },
            { "MantisShrimp",    "Boss: Mantis Shrimp" },
            { "Lusca",           "Boss: Lusca" },
            { "Torben",          "Boss: Torben" },
            { "Anomalocaris",    "Boss: Yawie" },
            { "Yawie",           "Boss: Yawie" },
        };

        public static string? GetDisplayNameFromScene(string sceneName)
        {
            foreach (var kvp in _map)
                if (sceneName.Contains(kvp.Key, System.StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            return null;
        }

        public static string? GetDisplayName(string bossId) => GetDisplayNameFromScene(bossId);
    }
}
