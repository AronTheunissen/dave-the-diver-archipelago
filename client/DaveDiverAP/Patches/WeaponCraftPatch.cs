using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches Duff's Weapon Shop to detect when a weapon is crafted.
    ///
    /// IMPORTANT: Class/method names are PLACEHOLDERS.
    /// Look for: WeaponShopManager, DuffShopManager, CraftingManager.
    /// The craft completion likely fires when the player confirms a craft in the UI.
    /// </summary>
    [HarmonyPatch]
    public static class WeaponCraftPatch
    {
        [HarmonyPatch(typeof(WeaponShopManager), "OnWeaponCrafted")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnWeaponCrafted_Postfix(string weaponId)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var weaponName = WeaponNameMapper.GetDisplayName(weaponId);
            if (weaponName != null)
                LocationTracker.OnWeaponCrafted(weaponName);
        }
    }

    public static class WeaponNameMapper
    {
        // TODO: Map internal weapon IDs to AP location name suffixes (the "Craft: " prefix is added by LocationTracker)
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new()
        {
            // Basic Underwater Rifle tree
            // { "WEAPON_BASIC_RIFLE",      "Basic Underwater Rifle" },
            // { "WEAPON_RIFLE_2",          "Underwater Rifle II" },
            // { "WEAPON_DEATH_RIFLE",      "Death Rifle" },
            // { "WEAPON_FLAME_RIFLE_1",    "Flame Rifle I" },
            // ... add all weapon variants
        };

        public static string? GetDisplayName(string weaponId)
        {
            return _map.TryGetValue(weaponId, out var name) ? name : null;
        }
    }
}
