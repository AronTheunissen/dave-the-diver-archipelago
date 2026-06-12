using HarmonyLib;
using BepInEx.Logging;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the game's fish catch system to detect first catches.
    /// 
    /// IMPORTANT: The exact class and method names below are PLACEHOLDERS.
    /// Use dnSpy or ILSpy to decompile Assembly-CSharp.dll and find the
    /// actual names for the fish catch/collection tracking methods.
    /// 
    /// Look for methods related to:
    /// - Fish encyclopedia / Marinca entry registration
    /// - First catch bonus / achievement trigger
    /// - Fish collection data save
    /// </summary>
    [HarmonyPatch]
    public static class FishCatchPatch
    {
        // TODO: Replace with actual class name from Assembly-CSharp.dll
        // Likely candidates: FishManager, FishDataManager, EncyclopediaManager, MarincaManager
        [HarmonyPatch(typeof(FishManager), "OnFishCaught")]   // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnFishCaught_Postfix(object __instance, string fishId, bool isFirstCatch)
        {
            if (!isFirstCatch) return;
            if (!ArchipelagoClient.IsConnected) return;

            // TODO: Convert fishId (internal game ID) to the display name used in AP
            // The fish display name must match exactly what's in locations.py
            var fishName = FishNameMapper.GetDisplayName(fishId);
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
