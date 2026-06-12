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
        [HarmonyPatch(typeof(RecipeManager), "UnlockRecipe")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void UnlockRecipe_Postfix(string recipeId)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var recipeName = RecipeNameMapper.GetDisplayName(recipeId);
            if (recipeName != null)
                LocationTracker.OnRecipeUnlocked(recipeName);
        }

        // ── Dish upgrade (research complete using Artisan's Flame) ───────────
        [HarmonyPatch(typeof(RecipeManager), "UpgradeDish")]   // PLACEHOLDER
        [HarmonyPostfix]
        public static void UpgradeDish_Postfix(string dishId, int newLevel)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var dishName = RecipeNameMapper.GetDisplayName(dishId);
            if (dishName != null)
                LocationTracker.OnDishUpgraded(dishName, newLevel);
        }
    }

    public static class RecipeNameMapper
    {
        // TODO: Fill in with real internal recipe/dish IDs from decompiled game
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new()
        {
            // { "RECIPE_YELLOWFIN_TUNA_AKAMI", "Yellowfin Tuna Akami Sushi" },
            // { "RECIPE_ATLANTIC_BONITO_CURRY", "Atlantic Bonito Curry" },
            // Add all recipes here
        };

        public static string? GetDisplayName(string recipeId)
        {
            return _map.TryGetValue(recipeId, out var name) ? name : null;
        }
    }
}
