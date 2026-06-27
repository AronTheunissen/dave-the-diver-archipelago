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
        //    Trigger method: DREventTriggerManager.WeaponCraftTreeEventTrigger(int craftID, int row, int col)
        //    DREventTriggerManager is a static class (TypeDefIndex: 2463) confirmed at line 94078 of dump.cs
        [HarmonyPatch(typeof(DREventTriggerManager), "WeaponCraftTreeEventTrigger")]
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

        // craftID = weapon TID confirmed via SubEquipmentData dump (2026-06-27)
        // TID pattern: 3060xxx where the hundreds digit = weapon tree
        // 3060_0xx = Basic Rifle tree, 3060_1xx = Net Gun tree, 3060_2xx = Triple Axel tree
        // 3060_3xx = Red Sniper tree, 3060_4xx = Sticky Bomb tree, 3060_5xx = Grenade Launcher tree
        // 3060_6xx = Ice Gun tree, 3060_7xx = Hush Dart tree, 3060_8xx = Drain Gun tree
        // 3060_9xx = unknown (901, 903 seen in dump — likely melee/Jungle weapons)
        // NOTE: craftID fired by WeaponCraftTreeEventTrigger may be the tree root TID, not individual variant TID
        // Full weapon TID list from SubEquipmentData (48 weapons owned):
        private static readonly System.Collections.Generic.Dictionary<int, string> _idMap = new()
        {
            // ── Basic Underwater Rifle tree (3060_0xx) ───────────────────────────
            { 3060001, "Basic Underwater Rifle" },
            { 3060002, "Underwater Rifle II" },
            { 3060003, "Underwater Rifle III" },
            { 3060004, "Death Rifle" },
            { 3060005, "Flame Rifle I" },
            { 3060006, "Flame Rifle II" },
            { 3060007, "Explosive Rifle" },
            { 3060008, "Tranquilizer Rifle" },
            { 3060009, "Poison Rifle I" },
            { 3060010, "Poison Rifle II" },
            { 3060012, "Hell Poison Rifle" },
            // Lightning/Shock/Thunderbolt Rifle TIDs not yet in dump (not crafted)

            // ── Small Net Gun tree (3060_1xx) ────────────────────────────────────
            { 3060101, "Small Net Gun" },
            { 3060102, "Medium Net Gun" },
            { 3060103, "Large Net Gun" },
            { 3060104, "Steel Net Gun" },
            { 3060105, "Enhanced Hush Dart" },  // Note: Hush Dart may share 1xx range
            { 3060106, "Small Net Gun" },        // duplicate — needs verification

            // ── Triple Axel tree (3060_2xx) ──────────────────────────────────────
            { 3060201, "Triple Axel" },
            { 3060202, "Quattro Axel" },
            { 3060203, "Quattro Axel II" },
            { 3060204, "Penta Axel" },
            { 3060205, "Flame Triple Axel" },
            { 3060206, "Flame Triple Axel II" },
            { 3060207, "Explosive Triple Axel" },
            { 3060208, "Tranquilizer Triple Axel" },
            { 3060209, "Poison Triple Axel" },
            { 3060210, "Poison Triple Axel II" },
            // Hell Poison/Lightning/Shock/Thunderbolt Triple Axel TIDs not in dump

            // ── Red Sniper Rifle tree (3060_3xx) ────────────────────────────────
            { 3060301, "Red Sniper Rifle" },
            { 3060302, "Red Sniper Rifle II" },
            { 3060303, "Red Sniper Rifle III" },
            { 3060304, "Death Sniper Rifle" },
            { 3060305, "Flame Sniper Rifle I" },
            { 3060306, "Flame Sniper Rifle II" },
            // Remaining Sniper variants not in dump

            // ── Sticky Bomb Gun tree (3060_4xx) ─────────────────────────────────
            { 3060401, "Sticky Bomb Gun" },
            { 3060402, "Sticky Bomb Gun II" },
            { 3060403, "Sticky Bomb Gun III" },
            // Remaining Sticky variants not in dump

            // ── Grenade Launcher tree (3060_5xx) ────────────────────────────────
            { 3060501, "Grenade Launcher" },
            { 3060502, "Grenade Launcher II" },
            // Remaining Grenade variants not in dump

            // ── Ice Gun tree (3060_6xx) ──────────────────────────────────────────
            { 3060601, "Ice Gun" },
            { 3060602, "Enhanced Ice Gun" },
            { 3060603, "Ultra Ice Gun" },

            // ── Hush Dart tree (3060_7xx) ────────────────────────────────────────
            { 3060701, "Hush Dart" },
            { 3060702, "Enhanced Hush Dart" },
            { 3060703, "Hush Dart III" },  // May not exist — needs verification

            // ── Drain Gun tree (3060_8xx, DREDGE DLC) ───────────────────────────
            { 3060801, "Drain Gun" },
            { 3060803, "Enhanced Drain Gun" },
            // Power Drain Gun not in dump

            // ── Unknown tree (3060_9xx) ───────────────────────────────────────────
            // 3060901 and 3060903 seen in dump — likely Jungle DLC weapons or melee
            { 3060901, "Unknown Weapon 901" },  // TODO: identify
            { 3060903, "Unknown Weapon 903" },  // TODO: identify
        };
    }
}
