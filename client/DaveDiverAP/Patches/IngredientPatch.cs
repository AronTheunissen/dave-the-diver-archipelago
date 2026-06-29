using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches ingredient collection to detect first-time finds.
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "Ingredient", "Plant", "SeaPlant", "Collect", "Gather"
    ///
    /// Note: Sea plants (Kelp, Agar, etc.) are gathered during dives.
    /// Farm ingredients are handled by FarmPatch.cs.
    /// Truffle/Rainbow Cap are purchased from vendors (Jango, Mushroomer).
    ///
    /// Uses the same SuccessInteract pattern as fish catches.
    /// </summary>
    [HarmonyPatch]
    public static class IngredientPatch
    {
        // Tracks which ingredients have been collected for the first time
        private static readonly System.Collections.Generic.HashSet<string> _foundIngredients = new();

        // ✅ CONFIRMED via dump.cs: SaveData has AddIngredientsSaveData(IngredientsData data)
        //    This fires whenever an ingredient is added to the save (first pick-up or purchase).
        //    IngredientsData has the ingredient's TID and data.
        //
        // ⚠️ DISABLED: This hook fires during save loading (game replays all saved ingredients),
        //    which caused a silent crash on "Continue" from the main menu.
        //    Re-enable once we confirm IngredientsData.ingredientsID field name and that the hook
        //    does NOT fire during save load (or add a guard to skip load-time replays).
        //
        // [HarmonyPatch(typeof(global::SaveData), "AddIngredientsSaveData")]
        // [HarmonyPostfix]
        // public static void OnIngredientCollected_Postfix(IngredientsData data)
        // {
        //     if (!ArchipelagoClient.IsConnected) return;
        //     if (data == null) return;
        //
        //     // Use the ingredient ID to look up the display name
        //     var ingredientName = IngredientNameMapper.GetDisplayName(data.ingredientsID);
        //     if (ingredientName == null) return;
        //     if (_foundIngredients.Contains(ingredientName)) return;
        //
        //     _foundIngredients.Add(ingredientName);
        //     LocationTracker.OnIngredientFirstFound(ingredientName);
        // }
    }

    public static class IngredientNameMapper
    {
        // Maps ingredient TID to display name for AP location matching.
        // TODO: Cross-reference ingredient TIDs from game design sheets.
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // Sea plants (diving) — Type: Vegetable in game data:
            { 1027102, "Agar" },
            { 1027103, "Kajime" },
            { 1027106, "Seaweed" },
            { 1027104, "Kelp" },
            { 1027101, "Sea Grape" },
            { 1027110, "Bladderwrack" },
            { 1027111, "Hyalonema" },
            { 1027108, "Southern Bull Kelp" },
            { 1027107, "Black Coral" },
            { 1027109, "Buckbean" },
            // Rare forageables (vendor) — ✅ both confirmed via UnityExplorer 2026-06-26
            { 1026011, "Truffle" },
            { 1026012, "Rainbow Cap" },
        };

        public static string? GetDisplayName(int tid) =>
            _map.TryGetValue(tid, out var name) ? name : null;
    }
}
