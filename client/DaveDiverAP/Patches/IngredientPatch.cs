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
        // ✅ CONFIRMED via dump.cs: IngredientsStorage.AddIngredients(int ingredientsID, int count, Place place)
        //    IngredientsStorage is a SingletonNoMono — fires only during actual gameplay ingredient
        //    additions (pickup, purchase), NOT during save load replay. Safe hook point.
        //
        // NOTE: The old ModSaveData.AddIngredientsSaveData hook was disabled because it fired during
        //    save loading. This IngredientsStorage hook does NOT have that problem.
        //
        // Dedup uses ModSaveData.IsIngredientFound/MarkIngredientFound — persists across game restarts
        // so a previously checked ingredient won't fire again in a new session.
        [HarmonyPatch(typeof(IngredientsStorage), "AddIngredients")]
        [HarmonyPostfix]
        public static void OnIngredientAdded_Postfix(int ingredientsID, int count, SushiBar.Place place)
        {
            try
            {
                if (!ArchipelagoClient.IsConnected) return;
                if (count <= 0) return;

                var ingredientName = IngredientNameMapper.GetDisplayName(ingredientsID);
                if (ingredientName == null) return;

                // Persistent dedup — survives game restarts (stored in archipelago_save.json)
                if (ModSaveData.IsIngredientFound(ingredientName)) return;
                ModSaveData.MarkIngredientFound(ingredientName);

                Plugin.Log.LogInfo($"[Ingredient] First time collected: {ingredientName} (TID={ingredientsID})");
                LocationTracker.OnIngredientFirstFound(ingredientName);
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[IngredientPatch] OnIngredientAdded_Postfix threw: {ex}");
            }
        }
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
