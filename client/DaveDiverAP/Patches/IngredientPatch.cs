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

        // Fires when a sea plant or ingredient is collected
        [HarmonyPatch(typeof(IngredientObject), "SuccessInteract")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnIngredientCollected_Postfix(object __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // TODO: Get ingredient name from __instance
            // Example: var name = ((IngredientObject)__instance).ingredientData.displayName;
            string? ingredientName = null; // TODO: get from __instance

            if (ingredientName == null) return;
            if (_foundIngredients.Contains(ingredientName)) return;

            _foundIngredients.Add(ingredientName);
            LocationTracker.OnIngredientFirstFound(ingredientName);
        }

        // Fires when Truffle or Rainbow Cap is purchased from vendor (Jango/Mushroomer)
        [HarmonyPatch(typeof(VendorManager), "OnItemPurchased")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnVendorPurchase_Postfix(string itemName)
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (itemName != "Truffle" && itemName != "Rainbow Cap") return;
            if (_foundIngredients.Contains(itemName)) return;

            _foundIngredients.Add(itemName);
            LocationTracker.OnIngredientFirstFound(itemName);
        }
    }
}
