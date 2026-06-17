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

        // craftID = Lv1 TID of each weapon variant (fired by WeaponCraftTreeEventTrigger)
        private static readonly System.Collections.Generic.Dictionary<int, string> _idMap = new()
        {
            // ── Underwater Rifle tree ────────────────────────────────────────────
            { 3010001, "Basic Underwater Rifle" },       // Normal_UnderwaterRifle_Lv1
            { 3011001, "Underwater Rifle II" },          // Enhanced_UnderwaterRifle01_Lv1
            { 3011011, "Underwater Rifle III" },         // Enhanced_UnderwaterRifle02_Lv1
            { 3011021, "Death Rifle" },                  // Enhanced_UnderwaterRifle03_Lv1
            { 3011041, "Flame Rifle I" },                // Enhanced_Fire_UnderwaterRifle01_Lv1
            { 3011051, "Flame Rifle II" },               // Enhanced_Fire_UnderwaterRifle02_Lv1
            { 3011031, "Explosive Rifle" },              // Enhanced_FireWork_UnderwaterRifle_Lv1
            { 3011061, "Tranquilizer Rifle" },           // Enhanced_Sleep_UnderwaterRifle01_Lv1
            { 3011071, "Poison Rifle I" },               // Enhanced_Poison_UnderwaterRifle01_Lv1
            { 3011081, "Poison Rifle II" },              // Enhanced_Poison_UnderwaterRifle02_Lv1
            { 3011091, "Hell Poison Rifle" },            // Enhanced_HellPoison_UnderwaterRifle_Lv1
            { 3011121, "Lightning Rifle I" },            // Enhanced_Paralysis_UnderwaterRifle01_Lv1
            { 3011131, "Lightning Rifle II" },           // Enhanced_Paralysis_UnderwaterRifle02_Lv1
            { 3011101, "Shock Rifle I" },                // Enhanced_Chain_UnderwaterRifle01_Lv1
            { 3011111, "Shock Rifle II" },               // Enhanced_Chain_UnderwaterRifle02_Lv1
            { 3011161, "Thunderbolt Rifle" },            // Enhanced_ThunderBolt_UnderwaterRifle_Lv1

            // ── Net Gun tree ─────────────────────────────────────────────────────
            { 3010061, "Small Net Gun" },                // Normal_NetGun_Lv1
            { 3012201, "Medium Net Gun" },               // Enhanced_MNetGun_Lv1
            { 3012211, "Large Net Gun" },                // Enhanced_LNetGun_Lv1
            { 3012221, "Steel Net Gun" },                // Enhanced_Steel_NetGun_Lv1

            // ── Hush Dart tree ───────────────────────────────────────────────────
            { 3010031, "Hush Dart" },                    // Normal_SleepGun_Lv1
            { 3012001, "Enhanced Hush Dart" },           // Enhanced_SleepGun01_Lv1

            // ── Triple Axel tree ─────────────────────────────────────────────────
            { 3010011, "Triple Axel" },                  // Normal_TripleAxel_Lv1
            { 3011201, "Quattro Axel" },                 // Enhanced_QuatroShotGun01_Lv1
            { 3011211, "Quattro Axel II" },              // Enhanced_QuatroShotGun02_Lv1
            { 3011221, "Penta Axel" },                   // Enhanced_CincoShotGun_Lv1
            { 3011241, "Flame Triple Axel" },            // Enhanced_Fire_TripleAxel01_Lv1
            { 3011251, "Flame Triple Axel II" },         // Enhanced_Fire_TripleAxel02_Lv1
            { 3011231, "Explosive Triple Axel" },        // Enhanced_Firework_TripleAxel_Lv1
            { 3011261, "Tranquilizer Triple Axel" },     // Enhanced_Sleep_TripleAxel01_Lv1
            { 3011271, "Poison Triple Axel" },           // Enhanced_Poison_TripleAxel01_Lv1
            { 3011281, "Poison Triple Axel II" },        // Enhanced_Poison_TripleAxel02_Lv1
            { 3011331, "Hell Poison Triple Axel" },      // Enhanced_HellPoison_TripleAxel_Lv1
            { 3011301, "Lightning Triple Axel" },        // Enhanced_Paralysis_TripleAxel01_Lv1
            { 3011291, "Shock Triple Axel" },            // Enhanced_Chain_TripleAxel01_Lv1
            { 3011311, "Shock Triple Axel II" },         // Enhanced_Paralysis_TripleAxel02_Lv1 (note: shares Paralysis02 prefix)
            { 3011321, "Thunderbolt Triple Axel" },      // Enhanced_ThunderBolt_TripleAxel_Lv1

            // ── Red Sniper Rifle tree ────────────────────────────────────────────
            { 3010021, "Red Sniper Rifle" },             // Normal_RedSniper_Lv1
            { 3011401, "Red Sniper Rifle II" },          // Enhanced_RedSniper01_Lv1
            { 3011411, "Red Sniper Rifle III" },         // Enhanced_RedSniper02_Lv1
            { 3011561, "Death Sniper Rifle" },           // Enhanced_DeathSniper_Lv1
            { 3011441, "Flame Sniper Rifle I" },         // Enhanced_Fire_Sniper01_Lv1
            { 3011451, "Flame Sniper Rifle II" },        // Enhanced_Fire_Sniper02_Lv1
            { 3011431, "Explosive Sniper Rifle" },       // Enhanced_FireWork_Sniper_Lv1
            { 3011461, "Tranquilizer Mosin-Nagant" },    // Enhanced_Sleep_Sniper01_Lv1
            { 3011481, "Poison Sniper Rifle I" },        // Enhanced_Poison_Sniper01_Lv1
            { 3011491, "Poison Sniper Rifle II" },       // Enhanced_Poison_Sniper02_Lv1
            { 3011471, "Hell Poison Sniper Rifle" },     // Enhanced_HellPoison_Sniper_Lv1
            { 3011521, "Lightning Sniper Rifle I" },     // Enhanced_Paralysis_Sniper01_Lv1
            { 3011531, "Lightning Sniper Rifle II" },    // Enhanced_Paralysis_Sniper02_Lv1
            { 3011501, "Shock Sniper Rifle I" },         // Enhanced_Chain_Sniper01_Lv1
            { 3011511, "Shock Sniper Rifle II" },        // Enhanced_Chain_Sniper02_Lv1
            { 3011541, "Thunderbolt Sniper Rifle" },     // Enhanced_ThunderBolt_Sniper_Lv1

            // ── Sticky Bomb Gun tree ─────────────────────────────────────────────
            { 3010071, "Sticky Bomb Gun" },              // Normal_StickyBombGun_Lv1
            { 3011801, "Sticky Bomb Gun II" },           // Enhanced_StickyBombGun01_Lv1
            { 3011811, "Sticky Bomb Gun III" },          // Enhanced_StickyBombGun02_Lv1
            { 3011841, "Sticky Mine Launcher I" },       // Enhanced_Mine_StickyBombGun01_Lv1
            { 3011851, "Sticky Mine Launcher II" },      // Enhanced_Mine_StickyBombGun02_Lv1
            { 3011861, "Sticky Tranquilizing Bomb Gun" },// Enhanced_Sleep_StickyBombGun01_Lv1
            { 3011871, "Poison Mine Launcher" },         // Enhanced_Poison_StickyBombGun01_Lv1
            { 3011881, "Poison Mine Launcher II" },      // Enhanced_Poison_StickyBombGun02_Lv1
            { 3011921, "Lightning Mine Launcher I" },    // Enhanced_Paralysis_StickyBombGun01_Lv1
            { 3011931, "Lightning Mine Launcher II" },   // Enhanced_Paralysis_StickyBombGun02_Lv1
            { 3011891, "Shock Mine Launcher I" },        // Enhanced_Chain_StickyBombGun01_Lv1
            { 3011901, "Shock Mine Launcher II" },       // Enhanced_Chain_StickyBombGun02_Lv1

            // ── Grenade Launcher tree ────────────────────────────────────────────
            { 3010051, "Grenade Launcher" },             // Normal_GrenadeLauncher_Lv1
            { 3011601, "Grenade Launcher II" },          // Enhanced_GrenadeLauncher01_Lv1
            { 3011611, "Grenade Launcher III" },         // Enhanced_GrenadeLauncher02_Lv1
            { 3011641, "Tranquilizer Gas Bomb Launcher" },// Enhanced_Sleep_GrenadeLauncher01_Lv1
            { 3011651, "Poison Launcher" },              // Enhanced_Poison_GrenadeLauncher01_Lv1
            { 3011661, "Gravity Launcher" },             // Enhanced_Magnetic_GrenadeLauncher01_Lv1
            { 3011711, "Blackhole Launcher" },           // Enhanced_BlackHole_GrenadeLauncher_Lv1
            { 3011671, "Flash Grenade Launcher" },       // Enhanced_Paralysis_GrenadeLauncher01_Lv1

            // ── Ice Gun tree ─────────────────────────────────────────────────────
            { 3010081, "Ice Gun" },                      // Normal_IceGun_Lv1
            { 3012301, "Enhanced Ice Gun" },             // Empowered_IceGun_Lv1
            { 3012311, "Ultra Ice Gun" },                // Black_IceGun_Lv1

            // ── Drain Gun tree (DREDGE DLC) ──────────────────────────────────────
            { 3010091, "Drain Gun" },                    // Normal_DrainGun_Lv1
            { 3012321, "Enhanced Drain Gun" },           // Empowered_DrainGun_Lv1
            { 3012331, "Power Drain Gun" },              // Power_DrainGun_DrainGun_Lv1
        };
    }
}
