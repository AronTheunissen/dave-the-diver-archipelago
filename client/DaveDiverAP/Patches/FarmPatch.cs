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
        // ── Vegetable Farm & Chicken Farm ─────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: MVFarmFieldController has public void DoHarvest(int laneNum)
        //    This is called when the player harvests a crop lane (vegetables AND chicken eggs).
        //    MVFarmFieldController also has RequestSowSeed(int laneNum, int seedTID).
        //    MVFarmHarvestPopupCtrler has NO OnEggCollected method — only Open/CloseUI/OnDirectHandler.
        //    Chicken egg collection goes through the same DoHarvest path as vegetables.
        // TODO: Distinguish lane types by checking the seed/animal TID on the lane to separate
        //       vegetable harvests from egg collects. For now both are handled together.
        [HarmonyPatch(typeof(MVFarmFieldController), "DoHarvest")]
        [HarmonyPostfix]
        public static void OnFarmHarvest_Postfix(MVFarmFieldController __instance, int laneNum)
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

        // ── Chicken Farm (egg harvest) ────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: SaveData.FarmSave.SetDailyHarvestItems() is called at day start
        //    to populate the daily harvest items including eggs from chickens.
        //    Also confirmed: SaveData.FarmSave.set_HasHarvestItemToClaim(bool) is set when items ready.
        //    Hook set_HasHarvestItemToClaim — when it becomes true, eggs/crops are ready to claim.
        [HarmonyPatch(typeof(SaveData.FarmSave), "SetDailyHarvestItems")]
        [HarmonyPostfix]
        public static void OnDailyHarvestSet_Postfix()
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
        // ✅ CONFIRMED via dump.cs: SaveData.FishFarmAreaSave.set_IsOpen(ObscuredBool) is called
        //    when a fish farm area is opened/unlocked by the player.
        //    Fields confirmed: AreaID (ObscuredInt), IsOpen (ObscuredBool), ExpansionItemBuyCount (ObscuredInt)
        //    SaveData.FishFarmSave.UpdateFishFarmAreaSave(SaveData.FishFarmAreaSave) saves the state.
        //    FishFarmManager is a Singleton<FishFarmManager> — the main manager class.
        //    FishFarmOpenExpandPopup fires the expand confirm popup (UI side).
        //    We hook set_IsOpen — when it becomes true, a new area was unlocked.
        [HarmonyPatch(typeof(SaveData.FishFarmAreaSave), "set_IsOpen")]
        [HarmonyPostfix]
        public static void OnFishFarmAreaOpened_Postfix(SaveData.FishFarmAreaSave __instance, ObscuredBool value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (!(bool)value) return;  // only fire when opening (not closing)

            // ✅ CONFIRMED via dump.cs: FishFarmAreaType enum: None=0, A=1, B=2, C=3, D=4, E=5, F=6, G=7, H=8
            //    The AreaID in save data matches the FishFarmAreaType int value.
            //    TODO: Verify which areas are actually unlockable by the player in-game (likely A-D for base game).
            int areaId = (int)__instance.AreaID;
            var locationName = areaId switch
            {
                1 => "Fish Farm: Unlock Area A",
                2 => "Fish Farm: Unlock Area B",
                3 => "Fish Farm: Unlock Area C",
                4 => "Fish Farm: Unlock Area D",
                5 => "Fish Farm: Unlock Area E",
                6 => "Fish Farm: Unlock Area F",
                7 => "Fish Farm: Unlock Area G",
                8 => "Fish Farm: Unlock Area H",
                _ => null
            };

            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
        }

        private static readonly System.Collections.Generic.HashSet<string> _bredSpecies = new();
        private static int _totalAdultFish = 0;
    }
}
