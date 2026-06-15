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
        // ✅ CONFIRMED via dump.cs: WeaponCraftTreeEvent is a struct fired via DREventManager
        //    Fields: int craftID, int rowIndex, int colIndex
        //    Trigger method: WeaponCraftTreeViewPanel.WeaponCraftTreeEventTrigger(int craftID, int row, int col)
        // We hook WeaponCraftTreeEventTrigger on WeaponCraftTreeViewPanel (the UI panel class)
        [HarmonyPatch(typeof(WeaponCraftTreeViewPanel), "WeaponCraftTreeEventTrigger")]
        [HarmonyPostfix]
        public static void OnWeaponCrafted_Postfix(int craftID, int row, int col)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // craftID maps to the weapon's design sheet TID
            var weaponName = WeaponNameMapper.GetDisplayNameFromCraftID(craftID);
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

        public static string? GetDisplayName(string weaponId) =>
            _map.TryGetValue(weaponId, out var name) ? name : null;

        public static string? GetDisplayNameFromCraftID(int craftID) =>
            _idMap.TryGetValue(craftID, out var name) ? name : null;

        // TODO: Fill in real craftID integers by cross-referencing the weapon design sheet
        // (open the game's design data files or search for WeaponCraftTreeEventTrigger calls in dump.cs)
        private static readonly System.Collections.Generic.Dictionary<int, string> _idMap = new()
        {
            // Example layout — replace with real TIDs from game design sheets:
            // { 10001, "Basic Underwater Rifle" },
            // { 10002, "Underwater Rifle II" },
            // { 10003, "Underwater Rifle III" },
            // ... etc for all 79 weapon variants
        };
    }
}
