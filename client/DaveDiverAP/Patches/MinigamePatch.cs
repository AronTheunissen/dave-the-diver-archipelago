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
        // ✅ CONFIRMED: SeahorseRaceSessionPlay is the real class (WhiteMinds mod)
        // Method name still needs confirming — search for "OnRaceEnd", "OnRaceComplete", "RaceResult" in SeahorseRaceSessionPlay
        [HarmonyPatch(typeof(SeahorseRaceSessionPlay), "OnRaceWon")]  // class confirmed, method still PLACEHOLDER
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
