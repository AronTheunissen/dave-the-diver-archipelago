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
            try
            {
                if (!ArchipelagoClient.IsConnected) return;
                _totalCrops++;

                // Check crop milestones
                foreach (var m in new[] { 50, 100, 250 })
                    if (_totalCrops == m)
                        ArchipelagoClient.CheckLocation($"Veg Farm: Harvest {m} Total Crops");

                // Detect first harvest of each crop type via lane seed TID
                // TODO: LocationTracker.OnVegetableHarvested not yet implemented
                // LocationTracker.OnVegetableHarvested(laneNum);
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[FarmPatch] OnFarmHarvest_Postfix threw: {ex}");
            }
        }

        private static int _totalCrops = 0;

        // ── Chicken Farm (egg harvest) ────────────────────────────────────────
        // TODO: SaveData.FarmSave not found in current interop DLL.
        // Confirmed via dump.cs — SaveData.FarmSave.SetDailyHarvestItems() fires at day start.
        // Regenerate interop by relaunching game with BepInEx.
        //
        // [HarmonyPatch(typeof(SaveData.FarmSave), "SetDailyHarvestItems")]
        // [HarmonyPostfix]
        // public static void OnDailyHarvestSet_Postfix()
        // {
        //     if (!ArchipelagoClient.IsConnected) return;
        //     _totalEggs++;
        //     if (_totalEggs == 1) ArchipelagoClient.CheckLocation("Chicken Farm: First Egg Collected");
        //     foreach (var m in new[] { 10, 50, 100 })
        //         if (_totalEggs == m)
        //             ArchipelagoClient.CheckLocation($"Chicken Farm: Collect {m} Eggs");
        // }

        private static int _totalEggs = 0;

        // ── Fish Farm ─────────────────────────────────────────────────────────
        // TODO: SaveData.FishFarmAreaSave and ObscuredBool not found in current interop DLL.
        // Confirmed via dump.cs — regenerate interop by relaunching game with BepInEx.
        // SaveData.FishFarmAreaSave.set_IsOpen(ObscuredBool) fires when a fish farm area is unlocked.
        // Fields: AreaID (ObscuredInt), IsOpen (ObscuredBool), ExpansionItemBuyCount (ObscuredInt)
        // FishFarmAreaType enum: None=0, A=1, B=2, C=3, D=4, E=5, F=6, G=7, H=8
        //
        // [HarmonyPatch(typeof(SaveData.FishFarmAreaSave), "set_IsOpen")]
        // [HarmonyPostfix]
        // public static void OnFishFarmAreaOpened_Postfix(SaveData.FishFarmAreaSave __instance, ObscuredBool value)
        // {
        //     if (!ArchipelagoClient.IsConnected) return;
        //     if (!(bool)value) return;
        //     int areaId = (int)__instance.AreaID;
        //     var locationName = areaId switch
        //     {
        //         1 => "Fish Farm: Unlock Area A",
        //         2 => "Fish Farm: Unlock Area B",
        //         3 => "Fish Farm: Unlock Area C",
        //         4 => "Fish Farm: Unlock Area D",
        //         5 => "Fish Farm: Unlock Area E",
        //         6 => "Fish Farm: Unlock Area F",
        //         7 => "Fish Farm: Unlock Area G",
        //         8 => "Fish Farm: Unlock Area H",
        //         _ => null
        //     };
        //     if (locationName != null)
        //         ArchipelagoClient.CheckLocation(locationName);
        // }

        private static readonly System.Collections.Generic.HashSet<string> _bredSpecies = new();
        private static int _totalAdultFish = 0;
    }
}
