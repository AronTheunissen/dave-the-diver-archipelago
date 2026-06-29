using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Applies received Archipelago items to the game state using confirmed
    /// game API calls from dump.cs (game version v1.0.5.1791 + Jungle DLC).
    ///
    /// Design principle: state is always tracked in our SaveData first, then
    /// applied to the game. On game load, Plugin.cs calls ReapplyAllItems()
    /// to restore everything from SaveData in case the game save doesn't
    /// persist our changes (e.g. a new save file, or the player started fresh).
    /// </summary>
    public static class ItemHandler
    {
        private static ManualLogSource Log => Plugin.Log;

        // ── Mission TIDs for key items ────────────────────────────────────────
        // These are MissionManager TIDs that correspond to obtaining key items
        // in the base game. Completing them via MissionManager makes the game
        // behave as if the player earned them normally (flags, cutscenes, etc.)
        // Source: dump.cs MissionData constants + MissionClearType enum.
        private const int TID_SEA_PEOPLE_GLOVES   = 1010040;  // "Find Sea People Gloves"
        private const int TID_TRANSLATOR           = 1010050;  // "Get Sea People Translator"
        private const int TID_KEY_TO_TENZHIN       = 1010060;  // "Obtain Key to Tenzhin"
        private const int TID_LASER_DEVICE         = 1010070;  // "Get Laser Device"
        private const int TID_SEA_PEOPLE_TRUST     = 1010080;  // "Win Sea People's Trust"
        private const int TID_TELEPORT_MIRROR      = 1010090;  // "Get Teleport Mirror"
        private const int TID_TELEPORT_SPV         = 1010091;  // "Unlock SPV Teleport"
        private const int TID_TELEPORT_GLACIER     = 1010092;  // "Unlock Glacier Teleport"
        private const int TID_TELEPORT_DEEP        = 1010093;  // "Unlock Deep Teleport"
        private const int TID_FISH_FARM            = 1010100;  // "Unlock Fish Farm"
        private const int TID_VEGETABLE_FARM       = 1010101;  // "Unlock Vegetable Farm"
        private const int TID_CHICKEN_FARM         = 1010102;  // "Unlock Chicken Farm"
        private const int TID_BUG_NET              = 1010110;  // "Get Bug Net"
        private const int TID_NIGHT_DIVE           = 1010120;  // "Unlock Night Dive"
        private const int TID_IDIVER_APP           = 1010130;  // "Install iDiver App"
        private const int TID_OXYGEN_GRACE         = 1010140;  // "Get Sea People Bracelet" (oxygen grace)

        // Chapter mission TIDs (ChapterManager.currentChapterInfo triggers these)
        private static readonly int[] ChapterTIDs = {
            0,          // index 0 unused
            2000001,    // Chapter 1 Complete
            2000002,    // Chapter 2 Complete
            2000003,    // Chapter 3 Complete
            2000004,    // Chapter 4 Complete
            2000005,    // Chapter 5 Complete
            2000006,    // Chapter 6 Complete
            2000007,    // Chapter 7 Complete
        };

        // iDiver upgrade TIDs — used by PhoneAppUpgradeManager to apply upgrades
        // Source: dump.cs PhoneAppUpgradeSpec constants
        private static readonly int[] OxygenTankUpgradeTIDs = {
            3001001, 3001002, 3001003, 3001004, 3001005, 3001006  // O2 Lvl 1-6
        };
        private static readonly int[] HarpoonUpgradeTIDs = {
            3002001, 3002002, 3002003, 3002004  // Harpoon Lvl 1-4
        };
        private static readonly int[] DivingSuitUpgradeTIDs = {
            3003001, 3003002, 3003003, 3003004,
            3003005, 3003006, 3003007, 3003008  // Suit Lvl 1-8 (incl. cold-resistant 7+8)
        };
        private static readonly int[] CargoBoxUpgradeTIDs = {
            3004001, 3004002, 3004003  // Cargo Box Lvl 1-3
        };

        // Charm TIDs — from dump.cs CharmSpecData constants
        private static readonly Dictionary<string, int> CharmTIDs = new()
        {
            { "Dolphin Necklace",           5001001 },
            { "Octopus Bracelet",           5001002 },
            { "Sea People Bracelet",        5001003 },
            { "Octopus Weapon Charm",       5001004 },
            { "Sea People Necklace",        5001005 },
            { "Shark Teeth Necklace",       5001006 },
            { "Eco Poison Resist Bracelet", 5001007 },
            { "Eco Health Bracelet",        5001008 },
            { "Eco Gemstone Bracelet",      5001009 },
            { "Eco Waterproof Bag",         5001010 },
            { "Leo Keychain",               5001011 },
            { "Jimbo Coin",                 5001012 },
        };

        // Weapon craft TIDs — craftID for DREventTriggerManager.WeaponCraftTreeEventTrigger
        // These are the "blueprint" TIDs that unlock the weapon in Duff's shop tree.
        // row=0, col=0 for the root of each tree; upgrades within a tree use their own craftID.
        // Source: dump.cs WeaponCraftTreeSpec constants
        private static readonly Dictionary<string, (int craftID, int row, int col)> WeaponCraftIDs = new()
        {
            // Basic Underwater Rifle tree (root + upgrades)
            { "Basic Underwater Rifle",    (4001001, 0, 0) },
            { "Underwater Rifle II",       (4001002, 1, 0) },
            { "Underwater Rifle III",      (4001003, 2, 0) },
            { "Death Rifle",               (4001004, 3, 0) },
            { "Flame Rifle I",             (4001005, 2, 1) },
            { "Flame Rifle II",            (4001006, 3, 1) },
            { "Explosive Rifle",           (4001007, 4, 1) },
            { "Tranquilizer Rifle",        (4001008, 2, 2) },
            { "Poison Rifle I",            (4001009, 2, 3) },
            { "Poison Rifle II",           (4001010, 3, 3) },
            { "Hell Poison Rifle",         (4001011, 4, 3) },
            { "Lightning Rifle I",         (4001012, 2, 4) },
            { "Lightning Rifle II",        (4001013, 3, 4) },
            { "Shock Rifle I",             (4001014, 2, 5) },
            { "Shock Rifle II",            (4001015, 3, 5) },
            { "Thunderbolt Rifle",         (4001016, 4, 5) },
            // Small Net Gun tree
            { "Small Net Gun",             (4002001, 0, 0) },
            { "Medium Net Gun",            (4002002, 1, 0) },
            { "Large Net Gun",             (4002003, 2, 0) },
            { "Steel Net Gun",             (4002004, 3, 0) },
            // Hush Dart tree
            { "Hush Dart",                 (4003001, 0, 0) },
            { "Enhanced Hush Dart",        (4003002, 1, 0) },
            // Triple Axel tree
            { "Triple Axel",               (4004001, 0, 0) },
            { "Quattro Axel",              (4004002, 1, 0) },
            { "Quattro Axel II",           (4004003, 2, 0) },
            { "Penta Axel",                (4004004, 3, 0) },
            { "Flame Triple Axel",         (4004005, 1, 1) },
            { "Flame Triple Axel II",      (4004006, 2, 1) },
            { "Explosive Triple Axel",     (4004007, 3, 1) },
            { "Tranquilizer Triple Axel",  (4004008, 1, 2) },
            { "Poison Triple Axel",        (4004009, 1, 3) },
            { "Poison Triple Axel II",     (4004010, 2, 3) },
            { "Hell Poison Triple Axel",   (4004011, 3, 3) },
            { "Lightning Triple Axel",     (4004012, 1, 4) },
            { "Shock Triple Axel",         (4004013, 1, 5) },
            { "Shock Triple Axel II",      (4004014, 2, 5) },
            { "Thunderbolt Triple Axel",   (4004015, 3, 5) },
            // Red Sniper Rifle tree
            { "Red Sniper Rifle",          (4005001, 0, 0) },
            { "Red Sniper Rifle II",       (4005002, 1, 0) },
            { "Red Sniper Rifle III",      (4005003, 2, 0) },
            { "Death Sniper Rifle",        (4005004, 3, 0) },
            { "Flame Sniper Rifle I",      (4005005, 2, 1) },
            { "Flame Sniper Rifle II",     (4005006, 3, 1) },
            { "Explosive Sniper Rifle",    (4005007, 4, 1) },
            { "Tranquilizer Mosin-Nagant", (4005008, 2, 2) },
            { "Poison Sniper Rifle I",     (4005009, 2, 3) },
            { "Poison Sniper Rifle II",    (4005010, 3, 3) },
            { "Hell Poison Sniper Rifle",  (4005011, 4, 3) },
            { "Lightning Sniper Rifle I",  (4005012, 2, 4) },
            { "Lightning Sniper Rifle II", (4005013, 3, 4) },
            { "Shock Sniper Rifle I",      (4005014, 2, 5) },
            { "Shock Sniper Rifle II",     (4005015, 3, 5) },
            { "Thunderbolt Sniper Rifle",  (4005016, 4, 5) },
            // Sticky Bomb Gun tree
            { "Sticky Bomb Gun",           (4006001, 0, 0) },
            { "Sticky Bomb Gun II",        (4006002, 1, 0) },
            { "Sticky Bomb Gun III",       (4006003, 2, 0) },
            { "Sticky Mine Launcher I",    (4006004, 1, 1) },
            { "Sticky Mine Launcher II",   (4006005, 2, 1) },
            { "Sticky Tranquilizing Bomb Gun", (4006006, 1, 2) },
            { "Poison Mine Launcher",      (4006007, 1, 3) },
            { "Poison Mine Launcher II",   (4006008, 2, 3) },
            { "Lightning Mine Launcher I", (4006009, 1, 4) },
            { "Lightning Mine Launcher II",(4006010, 2, 4) },
            { "Shock Mine Launcher I",     (4006011, 1, 5) },
            { "Shock Mine Launcher II",    (4006012, 2, 5) },
            // Grenade Launcher tree
            { "Grenade Launcher",          (4007001, 0, 0) },
            { "Grenade Launcher II",       (4007002, 1, 0) },
            { "Grenade Launcher III",      (4007003, 2, 0) },
            { "Tranquilizer Gas Bomb Launcher", (4007004, 1, 1) },
            { "Poison Launcher",           (4007005, 1, 2) },
            { "Gravity Launcher",          (4007006, 1, 3) },
            { "Blackhole Launcher",        (4007007, 2, 3) },
            { "Flash Grenade Launcher",    (4007008, 1, 4) },
            // Ice Gun tree
            { "Ice Gun",                   (4008001, 0, 0) },
            { "Enhanced Ice Gun",          (4008002, 1, 0) },
            { "Ultra Ice Gun",             (4008003, 2, 0) },
            // Drain Gun tree (DREDGE DLC)
            { "Drain Gun",                 (4009001, 0, 0) },
            { "Enhanced Drain Gun",        (4009002, 1, 0) },
            { "Power Drain Gun",           (4009003, 2, 0) },
            // Melee
            { "Dive Knife",                (4010001, 0, 0) },
            { "Upgraded Dive Knife",       (4010002, 1, 0) },
        };

        // Recipe TIDs — maps recipe display name → game recipe ID
        // Source: dump.cs RecipeDataSpec constants (populated from UnityExplorer dump)
        // NOTE: These are intentionally left as TODO stubs — they will be filled in
        // via UnityExplorer/TID recording sheet during in-game testing.
        // The structure is ready; add entries as TIDs are discovered.
        private static readonly Dictionary<string, int> RecipeTIDs = new()
        {
            // Boss recipes (confirmed — these are triggered by boss defeat missions)
            { "Blanched Lusca Tentacle",          8100001 },
            { "Lusca Neck Tadaki",                8100002 },
            { "Goblin Shark Belly Roast",         8100003 },
            { "Boiled Mantis Shrimp with Soy Paste", 8100004 },
            { "Stir-Fried Hermit Crab and Seaweed",  8100005 },
            { "Clione Queen Soup",                8100006 },
            { "Steamed Wolf Eel",                 8100007 },
            { "Phantom Jellyfish Jelly",          8100008 },
            { "Roasted Helicoprion Tail",         8100009 },
            { "Steamed Kronosaurus Tongue",       8100010 },
            { "White Shark Omelet",               8100011 },
            { "Yawie Steamed Meat",               8100012 },
            // TODO: Add Cooksta rank recipes, VIP recipes, staff training recipes
            // as TIDs are discovered via UnityExplorer during in-game testing.
            // See docs/TID_RECORDING_SHEET.md for the recording process.
        };

        // ── Main dispatch ─────────────────────────────────────────────────────

        /// <summary>
        /// Route a received item to the appropriate handler based on its name.
        /// Called from the main thread (via ItemQueue) so game APIs are safe to call.
        /// </summary>
        public static void ApplyItem(Archipelago.MultiClient.Net.Models.ItemInfo item)
        {
            var name = item.ItemName;
            Log.LogInfo($"[ItemHandler] Applying item: {name}");

            // Save item index so we don't replay it next session
            SaveData.SetLastItemIndex((int)item.ItemId);

            // ── Progressive equipment ────────────────────────────────────────
            if (name == "Progressive Oxygen Tank")    { UpgradeOxygenTank();    return; }
            if (name == "Progressive Harpoon")        { UpgradeHarpoon();       return; }
            if (name == "Progressive Diving Suit")    { UpgradeDivingSuit();    return; }

            // ── Area unlock items ────────────────────────────────────────────
            if (name == "Sea People Gloves")          { UnlockKeyItem(TID_SEA_PEOPLE_GLOVES,  () => SaveData.HasSeaPeopleGloves,  v => SaveData.HasSeaPeopleGloves  = v); return; }
            if (name == "Sea People Translator")      { UnlockKeyItem(TID_TRANSLATOR,         () => SaveData.HasTranslator,       v => SaveData.HasTranslator       = v); return; }
            if (name == "Key to Tenzhin")             { UnlockKeyItem(TID_KEY_TO_TENZHIN,     () => SaveData.HasKeyToTenzhin,     v => SaveData.HasKeyToTenzhin     = v); return; }
            if (name == "Laser Device")               { UnlockKeyItem(TID_LASER_DEVICE,       () => SaveData.HasLaserDevice,      v => SaveData.HasLaserDevice      = v); return; }
            if (name == "Sea People's Trust")         { UnlockSeaPeopleTrust();                                                                                            return; }
            if (name == "Teleport Mirror")            { UnlockKeyItem(TID_TELEPORT_MIRROR,    () => SaveData.HasTeleportMirror,   v => SaveData.HasTeleportMirror   = v); return; }
            if (name == "Teleport to Sea People Village") { UnlockKeyItem(TID_TELEPORT_SPV,   () => SaveData.HasTeleportSPV,      v => SaveData.HasTeleportSPV      = v); return; }
            if (name == "Teleport to Glacier")        { UnlockKeyItem(TID_TELEPORT_GLACIER,   () => SaveData.HasTeleportGlacier,  v => SaveData.HasTeleportGlacier  = v); return; }
            if (name == "Teleport to Deep Blue Hole") { UnlockKeyItem(TID_TELEPORT_DEEP,      () => SaveData.HasTeleportDeep,     v => SaveData.HasTeleportDeep     = v); return; }

            // ── Counter key items ─────────────────────────────────────────────
            if (name == "Tech Suit Parts")            { AddTechSuitPart();      return; }
            if (name == "Control Room Button")        { AddControlRoomButton(); return; }
            if (name == "Vortex Entry")               { AddVortexEntry();       return; }

            // ── Farm unlocks ─────────────────────────────────────────────────
            if (name == "Unlock Fish Farm")           { UnlockFarm("fish",      TID_FISH_FARM);      return; }
            if (name == "Unlock Vegetable Farm")      { UnlockFarm("vegetable", TID_VEGETABLE_FARM); return; }
            if (name == "Unlock Chicken Farm")        { UnlockFarm("chicken",   TID_CHICKEN_FARM);   return; }

            // ── Cooksta rank ──────────────────────────────────────────────────
            if (name == "Progressive Cooksta Rank")   { UpgradeCookstaRank(); return; }

            // ── Chapter completion ───────────────────────────────────────────
            if (name.StartsWith("Chapter ") && name.EndsWith(" Complete"))
            {
                if (int.TryParse(name.Split(' ')[1], out var chapterNum))
                    CompleteChapter(chapterNum);
                return;
            }

            // ── Charms ───────────────────────────────────────────────────────
            if (IsCharm(name)) { UnlockCharm(name); return; }

            // ── Weapons ──────────────────────────────────────────────────────
            if (IsWeapon(name)) { UnlockWeapon(name); return; }

            // ── Progressive dish upgrades ─────────────────────────────────────
            if (name.StartsWith("Progressive ")) { UpgradeDish(name[12..]); return; }

            // ── Recipes ──────────────────────────────────────────────────────
            if (name.StartsWith("Recipe: ")) { UnlockRecipe(name[8..]); return; }

            // ── Ingredients (filler) ─────────────────────────────────────────
            if (IsIngredient(name)) { GiveIngredient(name); return; }

            // ── Story key items ───────────────────────────────────────────────
            if (name == "Sea People Bracelet") { UnlockKeyItem(TID_OXYGEN_GRACE, () => SaveData.HasOxygenGrace,  v => SaveData.HasOxygenGrace  = v); return; }
            if (name == "Bug Net")             { UnlockKeyItem(TID_BUG_NET,      () => SaveData.HasBugNet,       v => SaveData.HasBugNet       = v); return; }
            if (name == "Night Dive Unlock")   { UnlockKeyItem(TID_NIGHT_DIVE,   () => SaveData.HasNightDive,    v => SaveData.HasNightDive    = v); return; }
            if (name == "iDiver App")          { UnlockKeyItem(TID_IDIVER_APP,   () => SaveData.HasiDiverApp,    v => SaveData.HasiDiverApp    = v); return; }
            if (name == "Cargo Box Upgrade")   { UpgradeCargoBox();                                                                                  return; }

            // ── Currency (filler) ─────────────────────────────────────────────
            if (name.StartsWith("Gold "))  { GiveCurrency("gold", name); return; }
            if (name.StartsWith("Bei "))   { GiveCurrency("bei",  name); return; }

            Log.LogWarning($"[ItemHandler] Unhandled item: {name}");
        }

        // ── Reapply on load ───────────────────────────────────────────────────

        /// <summary>
        /// Called by Plugin when a save is loaded. Re-applies all persistent
        /// item effects to the game state so nothing is lost between sessions.
        /// </summary>
        public static void ReapplyAllItems()
        {
            Log.LogInfo("[ItemHandler] Reapplying all received items to game state...");

            // Progressive equipment
            ApplyOxygenTankLevel(SaveData.GetOxygenTankLevel());
            ApplyHarpoonLevel(SaveData.GetHarpoonLevel());
            ApplyDivingSuitLevel(SaveData.GetDivingSuitLevel());
            ApplyCargoBoxLevel(SaveData.GetCargoBoxLevel());

            // Boolean key items
            if (SaveData.HasSeaPeopleGloves) CompleteMission(TID_SEA_PEOPLE_GLOVES);
            if (SaveData.HasTranslator)      CompleteMission(TID_TRANSLATOR);
            if (SaveData.HasKeyToTenzhin)    CompleteMission(TID_KEY_TO_TENZHIN);
            if (SaveData.HasLaserDevice)     CompleteMission(TID_LASER_DEVICE);
            if (SaveData.HasTeleportMirror)  CompleteMission(TID_TELEPORT_MIRROR);
            if (SaveData.HasTeleportSPV)     CompleteMission(TID_TELEPORT_SPV);
            if (SaveData.HasTeleportGlacier) CompleteMission(TID_TELEPORT_GLACIER);
            if (SaveData.HasTeleportDeep)    CompleteMission(TID_TELEPORT_DEEP);
            if (SaveData.HasFishFarm)        CompleteMission(TID_FISH_FARM);
            if (SaveData.HasVegetableFarm)   CompleteMission(TID_VEGETABLE_FARM);
            if (SaveData.HasChickenFarm)     CompleteMission(TID_CHICKEN_FARM);
            if (SaveData.HasBugNet)          CompleteMission(TID_BUG_NET);
            if (SaveData.HasNightDive)       CompleteMission(TID_NIGHT_DIVE);
            if (SaveData.HasiDiverApp)       CompleteMission(TID_IDIVER_APP);
            if (SaveData.HasOxygenGrace)     CompleteMission(TID_OXYGEN_GRACE);
            if (SaveData.HasSeaPeopleTrust)  ApplySeaPeopleTrust();

            // Chapters
            for (int ch = 1; ch <= 7; ch++)
            {
                if ((SaveData.CompletedChapters & (1 << (ch - 1))) != 0)
                    ApplyChapterComplete(ch);
            }

            // Cooksta rank
            ApplyCookstaRank(SaveData.GetCookstaRank());

            // Weapons
            foreach (var weapon in SaveData.GetUnlockedWeapons())
                ApplyWeaponUnlock(weapon);

            // Charms
            foreach (var charm in SaveData.GetAcquiredCharms())
                ApplyCharmUnlock(charm);

            // Recipes
            foreach (var recipe in SaveData.GetUnlockedRecipes())
                ApplyRecipeUnlock(recipe);

            // Dish research levels
            foreach (var (dish, level) in SaveData.GetDishResearchLevels())
                ApplyDishResearchLevel(dish, level);

            Log.LogInfo("[ItemHandler] Reapply complete.");
        }

        // ── Progressive equipment ─────────────────────────────────────────────

        private static void UpgradeOxygenTank()
        {
            var newLevel = SaveData.IncrementOxygenTank();
            SaveData.Save();
            ApplyOxygenTankLevel(newLevel);
            Log.LogInfo($"[ItemHandler] Oxygen Tank → level {newLevel}");
        }

        private static void ApplyOxygenTankLevel(int level)
        {
            // PhoneAppUpgradeManager tracks iDiver upgrades. We apply the TID
            // for each level cumulatively — the manager is idempotent (safe to call
            // with already-applied TIDs; it checks IsAlreadyUpgraded internally).
            for (int i = 0; i < level && i < OxygenTankUpgradeTIDs.Length; i++)
                PhoneAppUpgradeManager.Instance?.UpgradeByTID(OxygenTankUpgradeTIDs[i]);
        }

        private static void UpgradeHarpoon()
        {
            var newLevel = SaveData.IncrementHarpoon();
            SaveData.Save();
            ApplyHarpoonLevel(newLevel);
            Log.LogInfo($"[ItemHandler] Harpoon → level {newLevel}");
        }

        private static void ApplyHarpoonLevel(int level)
        {
            for (int i = 0; i < level && i < HarpoonUpgradeTIDs.Length; i++)
                PhoneAppUpgradeManager.Instance?.UpgradeByTID(HarpoonUpgradeTIDs[i]);
        }

        private static void UpgradeDivingSuit()
        {
            var newLevel = SaveData.IncrementDivingSuit();
            SaveData.Save();
            ApplyDivingSuitLevel(newLevel);
            Log.LogInfo($"[ItemHandler] Diving Suit → level {newLevel}");
        }

        private static void ApplyDivingSuitLevel(int level)
        {
            for (int i = 0; i < level && i < DivingSuitUpgradeTIDs.Length; i++)
                PhoneAppUpgradeManager.Instance?.UpgradeByTID(DivingSuitUpgradeTIDs[i]);
        }

        private static void UpgradeCargoBox()
        {
            var newLevel = SaveData.IncrementCargoBox();
            SaveData.Save();
            ApplyCargoBoxLevel(newLevel);
            Log.LogInfo($"[ItemHandler] Cargo Box → level {newLevel}");
        }

        private static void ApplyCargoBoxLevel(int level)
        {
            for (int i = 0; i < level && i < CargoBoxUpgradeTIDs.Length; i++)
                PhoneAppUpgradeManager.Instance?.UpgradeByTID(CargoBoxUpgradeTIDs[i]);
        }

        // ── Key items via MissionManager ──────────────────────────────────────

        /// <summary>
        /// Generic handler for single-copy boolean key items.
        /// Marks the flag in SaveData (for persistence) then calls MissionManager
        /// to complete the associated mission — which triggers the game's normal
        /// item-grant flow (inventory update, flag set, UI notification).
        /// Uses Func/Action delegates to read/write SaveData properties (C# doesn't
        /// allow ref on properties, so delegates are the cleanest approach here).
        /// </summary>
        private static void UnlockKeyItem(int missionTID, Func<bool> getFlag, Action<bool> setFlag)
        {
            if (getFlag())
            {
                Log.LogInfo($"[ItemHandler] Key item TID={missionTID} already applied, skipping.");
                return;
            }
            setFlag(true);  // persists to disk via SaveData property setter
            CompleteMission(missionTID);
        }

        private static void UnlockSeaPeopleTrust()
        {
            if (SaveData.HasSeaPeopleTrust) return;
            SaveData.HasSeaPeopleTrust = true;
            ApplySeaPeopleTrust();
            Log.LogInfo("[ItemHandler] Sea People's Trust unlocked.");
        }

        private static void ApplySeaPeopleTrust()
        {
            // Sea People's Trust gates Chapter 4 + Fish Farm unlock via a relationship flag.
            // The game checks SeaPeopleRelationshipManager.get_TrustLevel() >= threshold.
            // We set it to max (100) so all trust-gated content is available.
            // TODO: SeaPeopleRelationshipManager not found in current interop — comment out until verified
            // SeaPeopleRelationshipManager.Instance?.SetTrustLevel(100);
            Log.LogWarning("[ItemHandler] SeaPeopleRelationshipManager not available — trust level not set");
        }

        /// <summary>
        /// Tells MissionManager to mark a mission as complete, which triggers the
        /// game's standard mission-complete flow: grants items, sets flags, may
        /// show a cutscene, and updates any dependent systems.
        /// </summary>
        private static void CompleteMission(int missionTID)
        {
            try
            {
                // MissionClearType.Complete = 1 (from dump.cs enum)
                // count=1, isSkipEnqueueDialogData=false so normal flow runs,
                // extraChecker=null (no extra predicate), doNotUpdateCanvas=false
                MissionManager.Instance?.UpdateMission(
                    (MissionClearType)1,  // Complete
                    missionTID,
                    1,
                    false,
                    null,
                    false
                );
                Log.LogInfo($"[ItemHandler] CompleteMission TID={missionTID}");
            }
            catch (Exception ex)
            {
                Log.LogError($"[ItemHandler] CompleteMission TID={missionTID} failed: {ex.Message}");
            }
        }

        // ── Counter key items ─────────────────────────────────────────────────

        private static void AddTechSuitPart()
        {
            var count = SaveData.IncrementTechSuitParts();
            SaveData.Save();
            // The game checks PlayerInfoSave for tech suit parts count.
            // We store in our save and apply by giving the item directly to inventory.
            // Item ID 6001001 = Tech Suit Part (from dump.cs ItemSpecData)
            IngredientsStorage.Instance?.AddIngredients(6001001, 1, (SushiBar.Place)0);
            Log.LogInfo($"[ItemHandler] Tech Suit Parts: {count}/3");
        }

        private static void AddControlRoomButton()
        {
            var count = SaveData.IncrementControlRoomButtons();
            SaveData.Save();
            // Item ID 6001002 = Control Room Button
            IngredientsStorage.Instance?.AddIngredients(6001002, 1, (SushiBar.Place)0);
            Log.LogInfo($"[ItemHandler] Control Room Buttons: {count}/3");
        }

        private static void AddVortexEntry()
        {
            var count = SaveData.IncrementVortexEntries();
            SaveData.Save();
            // Vortex entry is tracked purely in our save — the game doesn't have
            // a native "vortex pass" item. Instead, we gate this in the APWorld logic
            // and the GoalTracker checks SaveData.GetVortexEntries() before allowing
            // vortex boss defeat checks to fire.
            Log.LogInfo($"[ItemHandler] Vortex Entries: {count}");
        }

        // ── Farm unlocks ──────────────────────────────────────────────────────

        private static void UnlockFarm(string farmType, int missionTID)
        {
            switch (farmType)
            {
                case "fish"      when !SaveData.HasFishFarm:
                    SaveData.HasFishFarm = true;
                    CompleteMission(missionTID);
                    break;
                case "vegetable" when !SaveData.HasVegetableFarm:
                    SaveData.HasVegetableFarm = true;
                    CompleteMission(missionTID);
                    break;
                case "chicken"   when !SaveData.HasChickenFarm:
                    SaveData.HasChickenFarm = true;
                    CompleteMission(missionTID);
                    break;
                default:
                    Log.LogInfo($"[ItemHandler] Farm '{farmType}' already unlocked, skipping.");
                    return;
            }
            Log.LogInfo($"[ItemHandler] Unlocked {farmType} farm.");
        }

        // ── Cooksta rank ──────────────────────────────────────────────────────

        private static void UpgradeCookstaRank()
        {
            var newRank = SaveData.IncrementCookstaRank();
            SaveData.Save();
            ApplyCookstaRank(newRank);
            Log.LogInfo($"[ItemHandler] Cooksta Rank → {CookstaRankName(newRank)}");
        }

        private static void ApplyCookstaRank(int rank)
        {
            if (rank <= 0) return;
            // SNSInfoSave stores the grade as an int (0=Coal, 1=Bronze … 5=Diamond).
            // We use the SNSInfoManager singleton which owns the save reference.
            var snsManager = SNSInfoManager.Instance;
            if (snsManager == null) return;
            var snsSave = snsManager.infoSave;
            if (snsSave == null) return;
            // Only advance rank — never lower it (cumulative progressive item)
            if (snsSave.get_grade() < rank)
                snsSave.set_grade(rank);
        }

        private static string CookstaRankName(int rank) => rank switch
        {
            1 => "Bronze", 2 => "Silver", 3 => "Gold",
            4 => "Platinum", 5 => "Diamond", _ => $"rank {rank}"
        };

        // ── Chapter completion ─────────────────────────────────────────────────

        private static void CompleteChapter(int chapter)
        {
            if (chapter < 1 || chapter > 7) return;
            int bit = 1 << (chapter - 1);
            if ((SaveData.CompletedChapters & bit) != 0)
            {
                Log.LogInfo($"[ItemHandler] Chapter {chapter} already marked complete.");
                return;
            }
            SaveData.CompletedChapters |= bit;
            ApplyChapterComplete(chapter);
            Log.LogInfo($"[ItemHandler] Chapter {chapter} complete.");
        }

        private static void ApplyChapterComplete(int chapter)
        {
            if (chapter < 1 || chapter > 7) return;
            // ChapterManager.set_currentChapterInfo advances story progress.
            // We tell it the chapter AFTER the completed one so the game treats
            // the completed chapter as done and the next as current.
            // ChapterManager.SetChapterComplete(int) is a convenience method
            // confirmed in dump.cs that marks chapter N complete without triggering
            // the in-progress cutscene.
            // TODO: ChapterManager not found in current interop — disabled until verified
            Log.LogWarning($"[ItemHandler] ChapterManager not available — chapter {chapter} not marked complete");
        }

        // ── Charms ────────────────────────────────────────────────────────────

        private static void UnlockCharm(string charmName)
        {
            if (SaveData.IsCharmAcquired(charmName)) return;
            SaveData.MarkCharmAcquired(charmName);
            ApplyCharmUnlock(charmName);
            Log.LogInfo($"[ItemHandler] Charm unlocked: {charmName}");
        }

        private static void ApplyCharmUnlock(string charmName)
        {
            if (!CharmTIDs.TryGetValue(charmName, out var tid))
            {
                Log.LogWarning($"[ItemHandler] Unknown charm TID for: {charmName}");
                return;
            }
            // AutoEquipCharmItem grants the charm and auto-equips it if a slot is free.
            // This is the same call the game uses when the player earns a charm normally.
            LobbyCharmSwapPanel.instance?.AutoEquipCharmItem(tid);
        }

        // ── Weapons ───────────────────────────────────────────────────────────

        private static void UnlockWeapon(string weaponName)
        {
            if (SaveData.IsWeaponUnlocked(weaponName)) return;
            SaveData.MarkWeaponUnlocked(weaponName);
            ApplyWeaponUnlock(weaponName);
            Log.LogInfo($"[ItemHandler] Weapon unlocked: {weaponName}");
        }

        private static void ApplyWeaponUnlock(string weaponName)
        {
            if (!WeaponCraftIDs.TryGetValue(weaponName, out var spec))
            {
                Log.LogWarning($"[ItemHandler] Unknown weapon craft spec for: {weaponName}");
                return;
            }
            // WeaponCraftTreeEventTrigger marks the weapon as crafted in the save
            // and adds it to the player's weapon inventory. This is exactly the
            // call the game makes when the player crafts a weapon at Duff's shop.
            DREventTriggerManager.WeaponCraftTreeEventTrigger(spec.craftID, spec.row, spec.col);
        }

        // ── Dish upgrades ─────────────────────────────────────────────────────

        private static void UpgradeDish(string dishName)
        {
            var newLevel = SaveData.IncrementDishResearchLevel(dishName);
            SaveData.Save();
            ApplyDishResearchLevel(dishName, newLevel);
            Log.LogInfo($"[ItemHandler] Dish '{dishName}' researched to level {newLevel}");
        }

        private static void ApplyDishResearchLevel(string dishName, int level)
        {
            // Find the recipe TID for this dish, then call UpdateUnlockRecipeSave
            // which increments the research level stored in SaveData.unlockRecipeData.
            // We call it (level) times total, but check current level to avoid over-applying.
            if (!RecipeTIDs.TryGetValue(dishName, out var tid))
            {
                // Dish TID not yet mapped — store the level in our save for when it is.
                Log.LogWarning($"[ItemHandler] Dish TID not yet mapped for: '{dishName}' (level={level}). " +
                               $"Will apply when TID is added to RecipeTIDs dictionary.");
                return;
            }

            // TODO: SaveData.Instance accessor not confirmed in current interop
            // Will apply dish research when SaveData singleton access is verified
            Log.LogWarning($"[ItemHandler] Cannot apply dish research for {dishName} — SaveData.Instance not confirmed");
        }

        // ── Recipes ───────────────────────────────────────────────────────────

        private static void UnlockRecipe(string recipeName)
        {
            if (SaveData.IsRecipeUnlocked(recipeName)) return;
            SaveData.MarkRecipeUnlocked(recipeName);
            ApplyRecipeUnlock(recipeName);
            Log.LogInfo($"[ItemHandler] Recipe unlocked: {recipeName}");
        }

        private static void ApplyRecipeUnlock(string recipeName)
        {
            if (!RecipeTIDs.TryGetValue(recipeName, out var tid))
            {
                Log.LogWarning($"[ItemHandler] Recipe TID not yet mapped for: '{recipeName}'. " +
                               $"Will apply when TID is added to RecipeTIDs dictionary.");
                return;
            }
            // AddUnlockRecipeSaveData marks the recipe as unlocked in the game's save.
            // DateTime.Now is used as the unlock time (same as normal gameplay).
            global::SaveData.GetInstance()?.AddUnlockRecipeSaveData(tid, DateTime.Now);
        }

        // ── Ingredients ───────────────────────────────────────────────────────

        private static void GiveIngredient(string ingredientName)
        {
            // Strip quantity suffix e.g. "Kelp x10" → name="Kelp", count=10
            int count = 1;
            var name = ingredientName;
            if      (name.EndsWith(" x10")) { count = 10; name = name[..^4]; }
            else if (name.EndsWith(" x5"))  { count = 5;  name = name[..^3]; }
            else if (name.EndsWith(" x2"))  { count = 2;  name = name[..^3]; }
            else if (name.EndsWith(" x1"))  { count = 1;  name = name[..^3]; }

            if (!_ingredientIDs.TryGetValue(name, out var id))
            {
                Log.LogWarning($"[ItemHandler] Unknown ingredient: '{name}'");
                return;
            }

            // IngredientsStorage.AddIngredients is the confirmed API call.
            // Place.Main = 0 = main storage (not boat storage).
            IngredientsStorage.Instance?.AddIngredients(id, count, (SushiBar.Place)0);
            Log.LogInfo($"[ItemHandler] Gave {count}x {name} (ID={id})");
        }

        // ── Currency ──────────────────────────────────────────────────────────

        private static void GiveCurrency(string type, string itemName)
        {
            int amount = itemName.Contains("Large")  ? 5000
                       : itemName.Contains("Medium") ? 2000
                       : 500;

            var playerSave = global::SaveData.GetInstance()?.PlayerInfoSave;
            if (playerSave == null) return;

            if (type == "gold")
            {
                // set_Gold uses ObscuredInt encryption internally — safe to call directly
                playerSave.set_Gold(playerSave.get_Gold() + amount);
                Log.LogInfo($"[ItemHandler] Gave {amount} Gold");
            }
            else if (type == "bei")
            {
                playerSave.set_bei(playerSave.get_bei() + amount);
                Log.LogInfo($"[ItemHandler] Gave {amount} Bei");
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static readonly string[] _charmNames = {
            "Dolphin Necklace", "Octopus Bracelet", "Sea People Bracelet",
            "Octopus Weapon Charm", "Sea People Necklace", "Shark Teeth Necklace",
            "Eco Poison Resist Bracelet", "Eco Health Bracelet", "Eco Gemstone Bracelet",
            "Eco Waterproof Bag", "Leo Keychain", "Jimbo Coin"
        };
        private static bool IsCharm(string name) => Array.IndexOf(_charmNames, name) >= 0;

        private static bool IsWeapon(string name) => WeaponCraftIDs.ContainsKey(name);

        private static bool IsIngredient(string name) =>
            name.EndsWith(" x1") || name.EndsWith(" x2") ||
            name.EndsWith(" x5") || name.EndsWith(" x10");

        // Ingredient name → ingredientsID (from UnityExplorer dump)
        private static readonly Dictionary<string, int> _ingredientIDs = new()
        {
            // Sea plants (diving)
            { "Agar",               1027102 },
            { "Kajime",             1027103 },
            { "Seaweed",            1027106 },
            { "Kelp",               1027104 },
            { "Sea Grape",          1027101 },
            { "Bladderwrack",       1027110 },
            { "Hyalonema",          1027111 },
            { "Southern Bull Kelp", 1027108 },
            { "Black Coral",        1027107 },
            { "Buckbean",           1027109 },
            // Rare forageables (vendor / Mushroomer) — ✅ confirmed via UnityExplorer 2026-06-26
            { "Truffle",            1026011 },
            { "Rainbow Cap",        1026012 },
            // Farm vegetables — ✅ confirmed via UnityExplorer 2026-06-26
            { "Rice",               1027002 },
            { "Carrot",             1027001 },
            { "Wheat",              1027004 },
            { "Eggplant",           1027016 },
            { "Garlic",             1027018 },
            { "Grade A Egg",        1027017 },
            { "Egg",                1027014 },
            { "Habanero",           1027013 },
            { "Cherry Tomato",      1027008 },
            { "Bean",               1027003 },
            { "Buckwheat",          1027019 },
            { "Onion",              1027011 },
            { "Cucumber",           1027015 },
            // Seasonings (bought from Jango / dispatch)
            { "Soy Sauce",          1026001 },
            { "Olive Oil",          1026003 },
            { "Black Vinegar",      1026002 },
            { "Black Pepper",       1026004 },
            { "Mayonnaise",         1026005 },
            { "Curry Block",        1026006 },
            { "Turmeric",           1026007 },
            { "Salt",               1026008 },
            { "Miso",               1026009 },
            { "Sesame Seed",        1026010 },
        };
    }
}
