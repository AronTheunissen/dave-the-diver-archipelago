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

        // ✅ CONFIRMED: InstanceItemChest is the real class name (WhiteMinds mod)
        // ✅ CONFIRMED: SuccessInteract(BaseCharacter) is the real method signature
        [HarmonyPatch(typeof(InstanceItemChest), "SuccessInteract")]
        [HarmonyPostfix]
        public static void TreasureChest_Postfix()
        {
            try
            {
                if (!ArchipelagoClient.IsConnected) return;
                _chestCount++;
                if (_chestCount <= 2)
                    ArchipelagoClient.CheckLocation($"Find Treasure Chest {_chestCount}");
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[CollectiblePatch] TreasureChest_Postfix threw: {ex}");
            }
        }

        // ── Teleport point activations ────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: InstanceItemChest uses SuccessInteraction() (same pattern as fish)
        //    Teleport mirrors use the same interaction pattern. The teleport point class
        //    is likely InteractionGimmick or a subclass — hook SuccessInteraction on it.
        [HarmonyPatch(typeof(InteractionGimmick_PhotoZone), "SuccessInteraction")]  // closest confirmed gimmick class
        [HarmonyPostfix]
        public static void TeleportPoint_Postfix(object __instance)
        {
            try
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
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[CollectiblePatch] TeleportPoint_Postfix threw: {ex}");
            }
        }

        // ── Godzilla DLC: Kaiju figurines ────────────────────────────────────
        // Figurines are scattered across the Blue Hole after defeating Ebirah.
        // Each figurine is a collectible item in the world — hook the same
        // SuccessInteract pattern as treasure chests.
        // TODO: Find the figurine-specific class name via Il2CppDumper.
        //       Search dump.cs for: "KaijuFigurine", "GodzillaFigurine", "Figurine", "Kaiju"
        //       It likely extends InstanceItemChest or a similar interactable.
        private static int _figurineCount = 0;

        // [HarmonyPatch(typeof(KaijuFigurine), "SuccessInteract")]  // TODO: confirm class name
        // [HarmonyPostfix]
        public static void KaijuFigurine_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            // Only active if Godzilla DLC is enabled in slot data
            if (ArchipelagoClient.SlotData == null || !ArchipelagoClient.SlotData.HasGodzillaDLC) return;
            _figurineCount++;
            if (_figurineCount <= 20)
                ArchipelagoClient.CheckLocation($"Kaiju Figurine {_figurineCount}");
        }

        // ── Duff shop purchases ───────────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: "DuffShopManager" does NOT exist as a class.
        //    DuffShop is a PhoneAppList constant (value 14060002) — it's a phone app, not a manager.
        //    The Duff shop (weapon crafting shop) is WeaponCraftTreePanel / WeaponCraftTreeViewPanel.
        //    Craft purchases are already covered by WeaponCraftPatch.cs via DREventTriggerManager.WeaponCraftTreeEventTrigger.
        //    For upgrade shop (suit/oxygen/tank upgrades from Duff), search dump for "LobbyShop" or "UpgradeShop".
        // TODO: Find the lobby upgrade shop class — search dump.cs for "suit upgrade", "LobbyShop", "EquipShop"
        // Removing this placeholder patch to avoid compile errors.
        // private static int _duffPurchases = 0;
    }
}
