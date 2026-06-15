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
        // ✅ CONFIRMED via dump.cs: MVFarmFieldController has public void DoHarvest(int laneNum)
        //    This is called when the player harvests a crop lane.
        //    MVFarmFieldController also has RequestSowSeed(int laneNum, int seedTID).
        [HarmonyPatch(typeof(MVFarmFieldController), "DoHarvest")]
        [HarmonyPostfix]
        public static void OnVegetableHarvest_Postfix(MVFarmFieldController __instance, int laneNum)
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalCrops++;

            // Check crop milestones
            foreach (var m in new[] { 50, 100, 250 })
                if (_totalCrops == m)
                    ArchipelagoClient.CheckLocation($"Veg Farm: Harvest {m} Total Crops");

            // Detect first harvest of each crop type via lane seed TID
            LocationTracker.OnVegetableHarvested(laneNum);
        }

        private static int _totalCrops = 0;

        // ── Chicken Farm ──────────────────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: SaveData.FarmSave.FarmAnimalSave tracks animals
        //    FarmAnimalPresenter is the visual component; FarmAnimalNamePlate is the UI.
        //    Hook SaveData.FarmSave interaction — FarmAnimalFeedSave tracks feeding.
        //    The egg collection is best detected via SaveData.FarmSave modifications.
        //    Using MVFarmHarvestPopupCtrler which fires on harvest confirm popup.
        [HarmonyPatch(typeof(MVFarmHarvestPopupCtrler), "OnEggCollected")]
        [HarmonyPostfix]
        public static void OnEggCollected_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalEggs++;
            if (_totalEggs == 1) ArchipelagoClient.CheckLocation("Chicken Farm: First Egg Collected");
            foreach (var m in new[] { 10, 50, 100 })
                if (_totalEggs == m)
                    ArchipelagoClient.CheckLocation($"Chicken Farm: Collect {m} Eggs");
        }

        private static int _totalEggs = 0;

        // ── Fish Farm ─────────────────────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: SaveData.FishFarmSave and SaveData.FishFarmAreaSave
        //    FishFarmDynamicEnvironmentController is a Singleton managing the fish farm.
        //    FishFarmDepthPanel tracks depth/tank upgrades.
        //    Hook FishFarmDynamicEnvironmentController for fish farm milestones.
        [HarmonyPatch(typeof(FishFarmDynamicEnvironmentController), "OnFishFarmUpgraded")]
        [HarmonyPostfix]
        public static void OnFishFarmUpgraded_Postfix(int level)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnFishFarmUpgraded(level);
        }

        private static readonly System.Collections.Generic.HashSet<string> _bredSpecies = new();
        private static int _totalAdultFish = 0;
    }
}
