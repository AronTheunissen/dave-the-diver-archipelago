using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the farming systems (vegetable farm, chicken farm, fish farm).
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "Farm", "VegetableFarm", "ChickenFarm", "FishFarm", "Harvest", "Crop"
    ///
    /// Known context: Otto is the NPC who manages the farms.
    /// The fish farm was unlocked via "A Noisy Customer" quest.
    /// </summary>
    [HarmonyPatch]
    public static class FarmPatch
    {
        // ── Vegetable Farm ────────────────────────────────────────────────────

        // ✅ CONFIRMED: Farm.FarmPlayerView is the real veg farm class (WhiteMinds mod)
        // ✅ CONFIRMED: Farm.FarmCore is the farm core mechanics class
        // Method names still need confirming via Il2CppDumper — search in Farm.FarmPlayerView / Farm.FarmCore
        // Fires on first harvest of each crop type
        [HarmonyPatch(typeof(Farm.FarmCore), "OnFirstHarvest")]  // class confirmed, method still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnFirstVegetableHarvest_Postfix(string cropName)
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation($"Veg Farm: First Harvest - {cropName}");
            LocationTracker.OnIngredientFirstFound(cropName);
        }

        // Fires when garden tier is upgraded
        [HarmonyPatch(typeof(Farm.FarmCore), "OnTierUpgrade")]  // class confirmed, method still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnGardenTierUpgrade_Postfix(int newTier)
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation($"Veg Farm: Upgrade Garden Tier {newTier}");
        }

        // Fires when total crop harvest count changes
        private static int _totalCrops = 0;
        [HarmonyPatch(typeof(Farm.FarmCore), "OnHarvest")]  // class confirmed, method still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnVegetableHarvest_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalCrops++;
            foreach (var m in new[] { 50, 100, 250 })
                if (_totalCrops == m)
                    ArchipelagoClient.CheckLocation($"Veg Farm: Harvest {m} Total Crops");
        }

        // ── Chicken Farm ──────────────────────────────────────────────────────

        // Fires when coop tier is upgraded
        [HarmonyPatch(typeof(ChickenFarmManager), "OnTierUpgrade")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnCoopTierUpgrade_Postfix(int newTier)
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation($"Chicken Farm: Upgrade Coop Tier {newTier}");
        }

        // Fires when an egg is collected
        private static int _totalEggs = 0;
        [HarmonyPatch(typeof(ChickenFarmManager), "OnEggCollected")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnEggCollected_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalEggs++;
            if (_totalEggs == 1)   ArchipelagoClient.CheckLocation("Chicken Farm: First Egg Collected");
            foreach (var m in new[] { 10, 50, 100 })
                if (_totalEggs == m)
                    ArchipelagoClient.CheckLocation($"Chicken Farm: Collect {m} Eggs");
        }

        // ── Fish Farm ─────────────────────────────────────────────────────────

        // Fires when a fish tank is upgraded
        // ✅ CONFIRMED: FishFarm.FishFarmPlayerView is the real fish farm class (WhiteMinds mod)
        [HarmonyPatch(typeof(FishFarm.FishFarmPlayerView), "OnTankUpgrade")]  // class confirmed, method still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnFishTankUpgrade_Postfix(int tankNumber)
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation($"Fish Farm: Upgrade Tank {tankNumber}");
        }

        // Fires when a fish species is bred for the first time
        [HarmonyPatch(typeof(FishFarm.FishFarmPlayerView), "OnFirstBreed")]  // class confirmed, method still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnFirstBreed_Postfix(string fishName)
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation($"Fish Farm: First Breed - {fishName}");
        }

        // Fires when fish reach adulthood
        private static int _totalAdultFish = 0;
        private static readonly System.Collections.Generic.HashSet<string> _bredSpecies = new();
        [HarmonyPatch(typeof(FishFarm.FishFarmPlayerView), "OnFishReachedAdulthood")]  // class confirmed, method still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnFishAdulthood_Postfix(string fishName)
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalAdultFish++;
            _bredSpecies.Add(fishName);

            foreach (var m in new[] { 10, 25, 50 })
                if (_totalAdultFish == m)
                    ArchipelagoClient.CheckLocation($"Fish Farm: Raise {m} Fish to Adulthood");

            foreach (var m in new[] { 5, 10 })
                if (_bredSpecies.Count == m)
                    ArchipelagoClient.CheckLocation($"Fish Farm: Raise {m} Different Species");
        }
    }
}
