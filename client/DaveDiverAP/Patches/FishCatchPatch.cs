using HarmonyLib;
using BepInEx.Logging;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the game's fish catch system to detect first catches.
    ///
    /// ## How to find the real class names:
    /// 1. Run Il2CppDumper on GameAssembly.dll + global-metadata.dat
    ///    (https://github.com/Perfare/Il2CppDumper)
    /// 2. Load the generated DummyDll/ in ILSpy or dnSpy
    /// 3. Search for "fish", "catch", "marinca", "encyclopedia"
    ///
    /// ## Known patterns from existing mods:
    /// - All fish/item interactions use CheckAvailableInteraction() + SuccessInteract() pattern
    ///   (confirmed by WhiteMinds/dave-diver-expansion)
    /// - Hook SuccessInteract() on the fish interaction class to detect catches
    /// - Use SaveSystem API (not property getter patches) for reading game state
    ///
    /// ## Known classes (from cheat engine + mod analysis):
    /// - SaveSystem → singleton with PlayerInfoSave accessor
    /// - PlayerInfoSave → ObscuredInt gold/bei/ChefFlame, inventory state
    /// - InGameManager → has FishAllocators for fish spawning
    /// - FishInteraction → inferred name, implements CheckAvailableInteraction/SuccessInteract
    ///
    /// ## Next step (on game machine):
    /// Run Il2CppDumper, search for classes containing "SuccessInteract" or "FirstCatch"
    /// and replace the PLACEHOLDER below with the real class name.
    /// </summary>
    [HarmonyPatch]
    public static class FishCatchPatch
    {
        // PLACEHOLDER — replace FishInteraction with real class name from Il2CppDumper output
        // Candidates: FishInteraction, FishObject, CatchableFish, MarincaFish
        // Look for the class that implements SuccessInteract() and tracks first-catch state
        [HarmonyPatch(typeof(FishInteraction), "SuccessInteract")]   // PLACEHOLDER
        [HarmonyPostfix]
        public static void SuccessInteract_Postfix(object __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // TODO: Read the fish name and isFirstCatch flag from __instance
            // These field names must be confirmed via Il2CppDumper output
            // Example: var fishData = ((FishInteraction)__instance).fishData;
            //          bool isFirst = fishData.isFirstCatch;
            //          string fishId = fishData.fishId;

            // Placeholder logic:
            string? fishId = null;    // TODO: get from __instance
            bool isFirstCatch = true; // TODO: get from __instance

            if (!isFirstCatch) return;

            var fishName = FishNameMapper.GetDisplayName(fishId ?? "");
            if (fishName != null)
                LocationTracker.OnFirstFishCatch(fishName);
        }
    }

    /// <summary>
    /// Maps internal game fish IDs to the display names used in AP location names.
    /// Fill these in by cross-referencing the game's fish data files.
    /// </summary>
    public static class FishNameMapper
    {
        // TODO: Build this mapping by decompiling the game and finding
        // the fish ID → display name lookup table.
        // The keys are internal game IDs, values are AP location name suffixes.
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new()
        {
            // Examples (replace with real IDs):
            // { "FISH_CLOWNFISH",        "Clownfish" },
            // { "FISH_BLUEFIN_TUNA",     "Bluefin Tuna" },
            // { "FISH_GREAT_WHITE_SHARK","Great White Shark Klaus" },
            // Add all ~200 species here
        };

        public static string? GetDisplayName(string fishId)
        {
            return _map.TryGetValue(fishId, out var name) ? name : null;
        }
    }
}
