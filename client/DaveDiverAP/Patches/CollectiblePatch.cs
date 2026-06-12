using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches collectible interactions: treasure chests, Duff shop purchases,
    /// and teleport point activations.
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "TreasureChest", "Chest", "Collectible", "TeleportPoint",
    ///             "DuffShop", "UpgradeShop"
    ///
    /// Note: Uses the same CheckAvailableInteraction/SuccessInteract pattern
    /// as fish catches (confirmed by existing mods).
    /// </summary>
    [HarmonyPatch]
    public static class CollectiblePatch
    {
        // ── Treasure chests ───────────────────────────────────────────────────
        private static int _chestCount = 0;

        [HarmonyPatch(typeof(TreasureChest), "SuccessInteract")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void TreasureChest_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _chestCount++;
            if (_chestCount <= 2)
                ArchipelagoClient.CheckLocation($"Find Treasure Chest {_chestCount}");
        }

        // ── Teleport point activations ────────────────────────────────────────
        [HarmonyPatch(typeof(TeleportPoint), "SuccessInteract")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void TeleportPoint_Postfix(object __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // TODO: Read the teleport point location from __instance
            // Map the point's identifier to the AP location name
            // Example: var pointId = ((TeleportPoint)__instance).pointId;
            string? pointId = null; // TODO: get from __instance

            var locationName = pointId switch
            {
                "TELEPORT_GLACIER"       => "Glacier: Activate Glacier Teleport Point",
                "TELEPORT_VILLAGE"       => "Sea People Village: Activate Village Teleport Point",
                "TELEPORT_DEEP"          => "Deep Blue Hole: Activate Deep Teleport Point",
                _                        => null
            };

            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
        }

        // ── Duff shop purchases ───────────────────────────────────────────────
        private static int _duffPurchases = 0;

        [HarmonyPatch(typeof(DuffShopManager), "OnUpgradePurchased")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnDuffUpgradePurchased_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _duffPurchases++;
            if (_duffPurchases <= 2)
                ArchipelagoClient.CheckLocation($"Purchase Upgrade from Duff {_duffPurchases}");
        }
    }
}
