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

        // Track whether we've reapplied items since the game was loaded.
        // Reset to false whenever the game save is (re)loaded so that switching
        // saves still triggers a full reapply on the next boat entry.
        private static bool _itemsReapplied = false;

        /// <summary>Called by Plugin when a save file is loaded/reloaded.</summary>
        public static void OnSaveLoaded() => _itemsReapplied = false;

        // ── Boat (safe state — process items here) ────────────────────────────
        // ✅ CONFIRMED via dump.cs: LobbyPlayer is the real class (MonoBehaviour)
        //    LobbyPlayer.LobbyPlayerState enum confirmed:
        //      InBoat = 12  ← items are delivered here
        //      Diving = 5   ← disable
        //      MorningStart = 7, AfternoonStart = 8 ← lobby (boat area but not IN boat)
        //    Method: ChangeLobbyPlayerState(LobbyPlayer.LobbyPlayerState state)
        [HarmonyPatch(typeof(LobbyPlayer), "ChangeLobbyPlayerState")]
        [HarmonyPostfix]
        public static void OnLobbyStateChanged_Postfix(LobbyPlayer.LobbyPlayerState state)
        {
            try
            {
                switch (state)
                {
                    case LobbyPlayer.LobbyPlayerState.InBoat:
                        Log.LogInfo("Game state: IN BOAT — item processing enabled.");
                        ItemQueue.SetGameReady(true);
                        // Reapply all received items on the first boat entry after a save load,
                        // so progressive upgrades, key items, etc. persist across sessions.
                        if (!_itemsReapplied)
                        {
                            _itemsReapplied = true;
                            Log.LogInfo("Game state: First boat entry — reapplying all items.");
                            ItemHandler.ReapplyAllItems();
                        }
                        break;

                    case LobbyPlayer.LobbyPlayerState.Diving:
                        Log.LogInfo("Game state: DIVING — item processing enabled (checks can fire while diving).");
                        ItemQueue.SetGameReady(true);
                        if (!_itemsReapplied)
                        {
                            _itemsReapplied = true;
                            Log.LogInfo("Game state: First dive — reapplying all items.");
                            ItemHandler.ReapplyAllItems();
                        }
                        break;

                    case LobbyPlayer.LobbyPlayerState.MorningStart:
                    case LobbyPlayer.LobbyPlayerState.AfternoonStart:
                        Log.LogInfo($"Game state: {state} — item processing enabled.");
                        ItemQueue.SetGameReady(true);
                        if (!_itemsReapplied)
                        {
                            _itemsReapplied = true;
                            ItemHandler.ReapplyAllItems();
                        }
                        break;

                    // Restaurant, farms, cutscenes, loading — disable item delivery
                    default:
                        ItemQueue.SetGameReady(false);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Log.LogError($"[GameStatePatch] OnLobbyStateChanged_Postfix threw: {ex}");
            }
        }

        // ── Restaurant / SushiBar (additional disable) ────────────────────────
        // ✅ CONFIRMED via dump.cs: SushiBarManager has private void OnStartOpeningSushiBar()
        //    which fires when the restaurant opens for service each night.
        //    Also public void OnEventSushiBarOpened() — the public event callback.
        [HarmonyPatch(typeof(SushiBarManager), "OnEventSushiBarOpened")]
        [HarmonyPostfix]
        public static void OnRestaurantStart_Postfix()
        {
            try
            {
                Log.LogInfo("Game state: RESTAURANT — item processing disabled.");
                ItemQueue.SetGameReady(false);
            }
            catch (System.Exception ex)
            {
                Log.LogError($"[GameStatePatch] OnRestaurantStart_Postfix threw: {ex}");
            }
        }
    }
}
