using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the minigame systems (seahorse racing, card games).
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "Minigame", "SeahorseRace", "CardGame", "Race"
    /// The seahorse racing is in Sea People Village.
    /// The card mini-games are in Bancho Sushi.
    /// </summary>
    [HarmonyPatch]
    public static class MinigamePatch
    {
        // ✅ CONFIRMED via dump.cs: SeahorseRaceSessionPlay is real (sealed class, implements ISessionPlay)
        //    public void OnGoal(int lane) — fires when ANY racer reaches the finish line
        //    private IEnumerator OnGoalPlayer() — coroutine called internally (NOT hookable by Harmony)
        //    We hook OnGoal(int lane) and check if lane == playerLane (4, per SeahorseRaceTrackData.playerLane const)
        //
        //    Difficulty chain (all confirmed via dump.cs):
        //    SeahorseRaceSessionPlay._session (offset 0x70) → SeahorseRaceSession
        //    SeahorseRaceSession.trackData → SeahorseRaceTrackData
        //    SeahorseRaceTrackData.trackKey → SeahorseRaceTrackKey
        //    SeahorseRaceTrackKey._division → SeahorseRaceTrackKey.Division enum:
        //        C = 0 (Easy), B = 1 (Medium), A = 2 (Hard), S = 3 (Expert)
        [HarmonyPatch(typeof(SeahorseRaceSessionPlay), "OnGoal")]
        [HarmonyPostfix]
        public static void OnSeahorseRaceWon_Postfix(SeahorseRaceSessionPlay __instance, int lane)
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (lane != 4) return;  // playerLane = 4 (const in SeahorseRaceTrackData)

            // Walk the chain: _session → trackData → trackKey → division
            var session = Traverse.Create(__instance).Field("_session").GetValue<SeahorseRaceSession>();
            var trackData = session?.trackData;
            var trackKey = trackData?.trackKey;
            var division = Traverse.Create(trackKey).Field("_division").GetValue<SeahorseRaceTrackKey.Division>();

            var locationName = division switch
            {
                SeahorseRaceTrackKey.Division.C => "Beat Seahorse Racing - Easy",
                SeahorseRaceTrackKey.Division.B => "Beat Seahorse Racing - Medium",
                SeahorseRaceTrackKey.Division.A => "Beat Seahorse Racing - Hard",
                SeahorseRaceTrackKey.Division.S => "Beat Seahorse Racing - Expert",
                _                               => null
            };

            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
        }

        // TODO: Card mini-games use Balatro-style classes (BalatroGameCardDebuff, etc.)
        // CardGameManager does NOT exist in Assembly-CSharp — find the correct class via Il2CppDumper.
        // Search for: "BalatroGame", "OnAllGamesComplete", "CardComplete", "BangshaCard"
        // [HarmonyPatch(typeof(CardGameManager), "OnAllGamesComplete")]  // PLACEHOLDER — class not found
        // [HarmonyPostfix]
        // public static void OnAllCardGamesComplete_Postfix()
        // {
        //     if (!ArchipelagoClient.IsConnected) return;
        //     ArchipelagoClient.CheckLocation("Complete All Card Mini-games");
        // }
    }
}
