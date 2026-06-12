using HarmonyLib;
using BepInEx.Logging;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the game's scene/state transition system to detect when
    /// Dave is standing on the boat (the safe hub state).
    ///
    /// Items are only processed when Dave is on the boat — NOT while:
    /// - Diving in the Blue Hole
    /// - Managing the restaurant (Bancho Sushi)
    /// - Working on the farms (vegetable, chicken, fish)
    /// - In cutscenes or loading screens
    ///
    /// ## How to find the real class names:
    /// Run Il2CppDumper and search for:
    /// - "Boat", "BoatScene", "OverworldScene", "DayStart"
    /// - Scene transition classes: "SceneManager", "GameStateManager", "DayPhaseManager"
    /// - Methods like "OnBoatEnter", "OnDiveStart", "OnNightStart", "OnFarmEnter"
    ///
    /// ## Known game states:
    /// The game cycles through these states daily:
    /// 1. BOAT (daytime on boat) ← ONLY process items here
    /// 2. DIVING (underwater Blue Hole)
    /// 3. RESTAURANT (Bancho Sushi night service)
    /// 4. FARMS (vegetable/chicken/fish farm management)
    ///
    /// ## From existing mods:
    /// - devopsdinosaur/SuperDave uses scene detection for feature toggling
    /// - WhiteMinds/dave-diver-expansion uses scene callbacks for minimap activation
    /// - Both reference a scene/phase manager class (exact name TBD via Il2CppDumper)
    /// </summary>
    [HarmonyPatch]
    public static class GameStatePatch
    {
        private static ManualLogSource Log => Plugin.Log;

        // ── Boat (safe state — process items here) ────────────────────────────

        // PLACEHOLDER: Replace BoatSceneManager with real class name
        // Look for the method called when transitioning TO the boat/overworld state
        // This fires at the start of each in-game day before the dive begins
        [HarmonyPatch(typeof(BoatSceneManager), "OnBoatEnter")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnBoatEnter_Postfix()
        {
            Log.LogInfo("Game state: BOAT — item processing enabled.");
            ItemQueue.SetGameReady(true);
        }

        // ── Diving (disable item processing) ─────────────────────────────────

        // PLACEHOLDER: Replace DiveSceneManager with real class name
        // Look for the method called when the dive starts (player enters water)
        [HarmonyPatch(typeof(DiveSceneManager), "OnDiveStart")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnDiveStart_Postfix()
        {
            Log.LogInfo("Game state: DIVING — item processing disabled.");
            ItemQueue.SetGameReady(false);
        }

        // ── Restaurant (disable item processing) ──────────────────────────────

        // PLACEHOLDER: Replace RestaurantSceneManager with real class name
        // Look for the method called when night service begins at Bancho Sushi
        [HarmonyPatch(typeof(RestaurantSceneManager), "OnRestaurantStart")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnRestaurantStart_Postfix()
        {
            Log.LogInfo("Game state: RESTAURANT — item processing disabled.");
            ItemQueue.SetGameReady(false);
        }

        // ── Farms (disable item processing) ───────────────────────────────────

        // PLACEHOLDER: Replace FarmSceneManager with real class name
        // Look for the method called when entering any farm (vegetable/chicken/fish)
        [HarmonyPatch(typeof(FarmSceneManager), "OnFarmEnter")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnFarmEnter_Postfix()
        {
            Log.LogInfo("Game state: FARM — item processing disabled.");
            ItemQueue.SetGameReady(false);
        }

        // ── Loading / cutscenes (disable item processing) ─────────────────────

        // PLACEHOLDER: Replace LoadingManager with real class name
        // Look for scene loading callbacks
        [HarmonyPatch(typeof(LoadingManager), "OnLoadingStart")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnLoadingStart_Postfix()
        {
            Log.LogInfo("Game state: LOADING — item processing disabled.");
            ItemQueue.SetGameReady(false);
        }
    }
}
