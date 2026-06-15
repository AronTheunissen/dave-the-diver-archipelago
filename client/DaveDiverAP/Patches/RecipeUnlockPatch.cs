using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the recipe unlock and dish upgrade system.
    ///
    /// IMPORTANT: Class/method names are PLACEHOLDERS.
    /// Look for: RecipeManager, CookingManager, MenuManager, DishUpgradeManager.
    /// The recipe unlock likely fires when a new recipe becomes available in the menu.
    /// The dish upgrade fires when research is completed using Artisan's Flame.
    /// </summary>
    [HarmonyPatch]
    public static class RecipeUnlockPatch
    {
        // ── Recipe unlock (new recipe becomes available) ─────────────────────
        // ✅ CONFIRMED via dump.cs: SaveData has AddUnlockRecipeSaveData(int id, DateTime unlockTime)
        // Hooking AddUnlockRecipeSaveData fires exactly when a recipe is unlocked and persisted.
        // The int id is the recipe's design sheet TID.
        [HarmonyPatch(typeof(SaveData), "AddUnlockRecipeSaveData")]
        [HarmonyPostfix]
        public static void UnlockRecipe_Postfix(int id)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var recipeName = RecipeNameMapper.GetDisplayName(id);
            if (recipeName != null)
                LocationTracker.OnRecipeUnlocked(recipeName);
        }

        // ── Dish upgrade (research complete using Artisan's Flame) ───────────
        // ✅ CONFIRMED via dump.cs: SaveData has UpdateUnlockRecipeSave() and
        //    UnlockRecipeSave has ObscuredInt m_UnlockRecipeID and level data.
        // Hook UpdateUnlockRecipeSave to catch research level-ups.
        [HarmonyPatch(typeof(SaveData), "UpdateUnlockRecipeSave")]
        [HarmonyPostfix]
        public static void UpgradeDish_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            // Enumerate all unlock recipes and check for new level-ups
            LocationTracker.OnDishResearchUpdated();
        }
    }

    public static class RecipeNameMapper
    {
        // TODO: Fill in with real internal recipe/dish IDs from decompiled game
        // Maps recipe TID (design sheet integer ID) to AP location display name.
        // TODO: Fill in by cross-referencing the game's recipe design sheet data
        // (search dump.cs for "RecipeTID" or open design tables in UnityExplorer)
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // Example layout — replace with real TIDs:
            // { 20001, "Yellowfin Tuna Akami Sushi" },
            // { 20002, "Atlantic Bonito Curry" },
            // ... etc for all sushi and menu recipes
        };

        public static string? GetDisplayName(int recipeId) =>
            _map.TryGetValue(recipeId, out var name) ? name : null;
    }
}
