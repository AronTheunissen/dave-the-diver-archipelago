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
        //    Methods confirmed: OnGoalPlayer (coroutine — fires when player reaches finish line)
        //    SeahorseRacerState_Finish and SeahorseRacerState_Fail are the outcome states.
        //    SeahorseRaceSession.Destination is the finish data class.
        [HarmonyPatch(typeof(SeahorseRaceSessionPlay), "OnGoalPlayer")]
        [HarmonyPostfix]
        public static void OnSeahorseRaceWon_Postfix(string difficulty)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // difficulty should be "Easy", "Medium", or "Hard"
            var locationName = difficulty switch
            {
                "Easy"   => "Beat Seahorse Racing - Easy",
                "Medium" => "Beat Seahorse Racing - Medium",
                "Hard"   => "Beat Seahorse Racing - Hard",
                _        => null
            };

            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
        }

        // Fires when all card mini-games are completed
        [HarmonyPatch(typeof(CardGameManager), "OnAllGamesComplete")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnAllCardGamesComplete_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation("Complete All Card Mini-games");
        }
    }
}
