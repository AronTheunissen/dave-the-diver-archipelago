using System;
using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// Progress tracker tab — shows goal progress, received items, and category breakdown.
    /// Uses Unity built-in IMGUI (no native DLLs required).
    /// </summary>
    public static class ProgressUI
    {
        private static int _totalChecked   = 0;
        private static int _totalLocations = 0;
        private static readonly Dictionary<string, int> _categoryChecked = new();
        private static int     _itemsTab   = 0;
        private static Vector2 _scrollPos  = Vector2.zero;
        private static Vector2 _catScroll  = Vector2.zero;

        private static readonly (string label, string key, int maxTotal)[] Categories = {
            ("Fish",          "fish",         203),
            ("Bosses",        "boss",          16),
            ("Recipes",       "recipe",        54),
            ("Dish Upgrades", "dish_upgrade", 549),
            ("Weapons",       "weapon",        79),
            ("Ecowatcher",    "ecowatcher",    68),
            ("Cooksta",       "cooksta",       12),
            ("Story / Quests","story",         30),
            ("Farming",       "farming",       37),
            ("Photography",   "photography",   12),
            ("Challenges",    "challenge",      9),
            ("Minigames",     "minigame",       4),
            ("Ingredients",   "ingredient",    25),
            ("Charms",        "charm",          8),
        };

        private static ManualLogSource Log => Plugin.Log;

        public static void Initialize()
        {
            if (ArchipelagoClient.Session == null) return;
            _totalLocations = ArchipelagoClient.Session.Locations.AllLocations.Count;
            _totalChecked   = ArchipelagoClient.Session.Locations.AllLocationsChecked.Count;
            _categoryChecked.Clear();
            foreach (var locId in ArchipelagoClient.Session.Locations.AllLocationsChecked)
            {
                var locName = ArchipelagoClient.Session.Locations.GetLocationNameFromId(locId) ?? "";
                IncrementCategoryForLocation(locName);
            }
            Log.LogInfo($"[ProgressUI] Initialized: {_totalChecked}/{_totalLocations} checked.");
        }

        public static void OnLocationChecked(string locationName)
        {
            _totalChecked++;
            IncrementCategoryForLocation(locationName);
        }

        private static void IncrementCategoryForLocation(string locName)
        {
            var key = locName switch
            {
                _ when locName.StartsWith("First Catch:")   => "fish",
                _ when locName.StartsWith("Defeat:")        => "boss",
                _ when locName.StartsWith("Unlock Recipe:") => "recipe",
                _ when locName.StartsWith("Upgrade ")       => "dish_upgrade",
                _ when locName.StartsWith("Craft:")         => "weapon",
                _ when locName.StartsWith("Ecowatcher:")    => "ecowatcher",
                _ when locName.StartsWith("Cooksta:")       => "cooksta",
                _ when locName.StartsWith("Story:")         => "story",
                _ when locName.StartsWith("Quest:")         => "story",
                _ when locName.StartsWith("Sub-Mission:")   => "story",
                _ when locName.StartsWith("Veg Farm:")      => "farming",
                _ when locName.StartsWith("Chicken Farm:")  => "farming",
                _ when locName.StartsWith("Fish Farm:")     => "farming",
                _ when locName.StartsWith("Photo:")         => "photography",
                _ when locName.StartsWith("Beat ")          => "minigame",
                _ when locName.StartsWith("First Find:")    => "ingredient",
                _ when locName.StartsWith("Charm:")         => "charm",
                _                                           => "other",
            };
            _categoryChecked.TryGetValue(key, out var cur);
            _categoryChecked[key] = cur + 1;
        }

        public static void Draw()
        {
            GUILayout.Space(4);

            if (!ArchipelagoClient.IsConnected)
            {
                GUILayout.Label("Connect to Archipelago to see progress.");
                return;
            }

            // ── Overall ───────────────────────────────────────────────────────
            GUILayout.Label("=== Overall Progress ===");
            float pct = _totalLocations > 0 ? (float)_totalChecked / _totalLocations : 0f;
            DrawBar($"{_totalChecked} / {_totalLocations} locations checked", pct);
            GUILayout.Space(4);

            // ── Goal ──────────────────────────────────────────────────────────
            int goal = ArchipelagoClient.SlotData?.Goal ?? 0;
            string goalName = goal switch
            {
                0 => "Defeat Yawie",
                1 => "Defeat All Bosses",
                2 => "Diamond Rank",
                3 => "Master Diver",
                4 => "100% Completion",
                _ => "Unknown",
            };
            GUILayout.Label($"=== Goal: {goalName} ===");
            DrawGoalRequirements(goal);

            GUILayout.Space(4);

            // ── Received Items ────────────────────────────────────────────────
            GUILayout.Label("=== Received Items ===");
            string[] tabs = { "Equipment", "Key Items", "Charms", "Weapons" };
            GUILayout.BeginHorizontal();
            for (int i = 0; i < tabs.Length; i++)
                if (GUILayout.Toggle(_itemsTab == i, tabs[i], GUI.skin.button, GUILayout.Width(80)))
                    _itemsTab = i;
            GUILayout.EndHorizontal();

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(130));
            switch (_itemsTab)
            {
                case 0: DrawEquipmentTab(); break;
                case 1: DrawKeyItemsTab();  break;
                case 2: DrawCharmsTab();    break;
                case 3: DrawWeaponsTab();   break;
            }
            GUILayout.EndScrollView();

            GUILayout.Space(4);

            // ── Category Breakdown ────────────────────────────────────────────
            GUILayout.Label("=== Location Category Breakdown ===");
            _catScroll = GUILayout.BeginScrollView(_catScroll, GUILayout.Height(160));
            foreach (var (label, key, maxTotal) in Categories)
            {
                int sessionTotal = GetSessionCategoryTotal(key, maxTotal);
                int done = _categoryChecked.TryGetValue(key, out var d) ? d : 0;
                float p = sessionTotal > 0 ? Math.Min(1f, (float)done / sessionTotal) : 0f;
                DrawBar($"{label}: {done}/{sessionTotal}", p);
            }
            GUILayout.EndScrollView();
        }

        private static void DrawGoalRequirements(int goal)
        {
            string yawieStatus = GoalTracker.YawieDefeated ? "✓" : "○";
            GUILayout.Label($"  {yawieStatus} Defeat Yawie (Final Boss)");

            if (!GoalTracker.YawieDefeated)
            {
                GUILayout.Label("    Required to reach Yawie:");
                string btn = SaveData.GetControlRoomButtons() >= 3 ? "✓" : "○";
                GUILayout.Label($"      {btn} Control Room Buttons: {SaveData.GetControlRoomButtons()}/3");
                string laser = ModSaveData.HasLaserDevice ? "✓" : "○";
                GUILayout.Label($"      {laser} Laser Device");
            }

            if (goal >= 1)
            {
                int bossDone = GoalTracker.DefeatedBossCount;
                DrawBar($"Bosses: {bossDone}/16 defeated", bossDone / 16f);
            }

            if (goal == 2 || goal == 4)
            {
                int followers = GoalTracker.CookstaFollowers;
                int taste     = GoalTracker.CookstaBestTaste;
                int recipes   = GoalTracker.CookstaResearchedRecipes;
                DrawBar($"Followers: {followers}/720", Math.Min(1f, followers / 720f));
                DrawBar($"Best Taste: {taste}/375",    Math.Min(1f, taste / 375f));
                DrawBar($"Recipes: {recipes}/32",      Math.Min(1f, recipes / 32f));
            }

            if (goal == 3 || goal == 4)
            {
                string fish = GoalTracker.AllFishComplete ? "✓" : "○";
                GUILayout.Label($"  {fish} All Fish Species Caught");
            }
        }

        private static void DrawEquipmentTab()
        {
            DrawProgressiveItem("Oxygen Tank",    ModSaveData.GetOxygenTankLevel(),   6);
            DrawProgressiveItem("Diving Suit",    ModSaveData.GetDivingSuitLevel(),   8);
            DrawProgressiveItem("Harpoon",        ModSaveData.GetHarpoonLevel(),      4);
            DrawProgressiveItem("Cargo Box",      ModSaveData.GetCargoBoxLevel(),     3);
            DrawProgressiveItem("Tech Suit Parts",        ModSaveData.GetTechSuitParts(),      3);
            DrawProgressiveItem("Control Room Buttons",   SaveData.GetControlRoomButtons(), 3);
            DrawProgressiveItem("Cooksta Rank",           ModSaveData.GetCookstaRank(),        5);
        }

        private static void DrawProgressiveItem(string label, int level, int max)
        {
            float pct = max > 0 ? Math.Min(1f, (float)level / max) : 0f;
            DrawBar($"{label}: {level}/{max}", pct);
        }

        private static void DrawKeyItemsTab()
        {
            var items = new (string label, bool have)[]
            {
                ("Sea People Gloves",     SaveData.HasSeaPeopleGloves),
                ("Sea People Translator", ModSaveData.HasTranslator),
                ("Key to Tenzhin",        ModSaveData.HasKeyToTenzhin),
                ("Laser Device",          ModSaveData.HasLaserDevice),
                ("Sea People's Trust",    ModSaveData.HasSeaPeopleTrust),
                ("Teleport Mirror",       ModSaveData.HasTeleportMirror),
                ("Teleport → SPV",        ModSaveData.HasTeleportSPV),
                ("Teleport → Glacier",    ModSaveData.HasTeleportGlacier),
                ("Teleport → Deep",       ModSaveData.HasTeleportDeep),
                ("Unlock Fish Farm",      ModSaveData.HasFishFarm),
                ("Unlock Veg Farm",       ModSaveData.HasVegetableFarm),
                ("Unlock Chicken Farm",   ModSaveData.HasChickenFarm),
                ("Bug Net",               ModSaveData.HasBugNet),
                ("Night Dive",            ModSaveData.HasNightDive),
                ("iDiver App",            ModSaveData.HasiDiverApp),
                ("Sea People Bracelet",   ModSaveData.HasOxygenGrace),
            };
            foreach (var (label, have) in items)
                GUILayout.Label($"  {(have ? "✓" : "○")} {label}");
        }

        private static void DrawCharmsTab()
        {
            var charms = new[]
            {
                "Dolphin Necklace", "Octopus Bracelet", "Sea People Bracelet",
                "Octopus Weapon Charm", "Sea People Necklace", "Shark Teeth Necklace",
                "Eco Poison Resist Bracelet", "Eco Health Bracelet",
                "Eco Gemstone Bracelet", "Eco Waterproof Bag", "Leo Keychain", "Jimbo Coin",
            };
            int got = 0;
            foreach (var c in charms)
            {
                bool have = ModSaveData.IsCharmAcquired(c);
                if (have) got++;
                GUILayout.Label($"  {(have ? "✓" : "○")} {c}");
            }
            GUILayout.Label($"  {got}/{charms.Length} charms acquired");
        }

        private static void DrawWeaponsTab()
        {
            var trees = new (string name, string[] variants)[]
            {
                ("Underwater Rifle", new[]{ "Basic Underwater Rifle","Underwater Rifle II","Underwater Rifle III","Death Rifle","Flame Rifle I","Flame Rifle II","Explosive Rifle","Tranquilizer Rifle","Poison Rifle I","Poison Rifle II","Hell Poison Rifle","Lightning Rifle I","Lightning Rifle II","Shock Rifle I","Shock Rifle II","Thunderbolt Rifle" }),
                ("Net Gun",          new[]{ "Small Net Gun","Medium Net Gun","Large Net Gun","Steel Net Gun" }),
                ("Hush Dart",        new[]{ "Hush Dart","Enhanced Hush Dart" }),
                ("Triple Axel",      new[]{ "Triple Axel","Quattro Axel","Quattro Axel II","Penta Axel","Flame Triple Axel","Flame Triple Axel II","Explosive Triple Axel","Tranquilizer Triple Axel","Poison Triple Axel","Poison Triple Axel II","Hell Poison Triple Axel","Lightning Triple Axel","Shock Triple Axel","Shock Triple Axel II","Thunderbolt Triple Axel" }),
                ("Red Sniper Rifle", new[]{ "Red Sniper Rifle","Red Sniper Rifle II","Red Sniper Rifle III","Death Sniper Rifle","Flame Sniper Rifle I","Flame Sniper Rifle II","Explosive Sniper Rifle","Tranquilizer Mosin-Nagant","Poison Sniper Rifle I","Poison Sniper Rifle II","Hell Poison Sniper Rifle","Lightning Sniper Rifle I","Lightning Sniper Rifle II","Shock Sniper Rifle I","Shock Sniper Rifle II","Thunderbolt Sniper Rifle" }),
                ("Sticky Bomb Gun",  new[]{ "Sticky Bomb Gun","Sticky Bomb Gun II","Sticky Bomb Gun III","Sticky Mine Launcher I","Sticky Mine Launcher II","Sticky Tranquilizing Bomb Gun","Poison Mine Launcher","Poison Mine Launcher II","Lightning Mine Launcher I","Lightning Mine Launcher II","Shock Mine Launcher I","Shock Mine Launcher II" }),
                ("Grenade Launcher", new[]{ "Grenade Launcher","Grenade Launcher II","Grenade Launcher III","Tranquilizer Gas Bomb Launcher","Poison Launcher","Gravity Launcher","Blackhole Launcher","Flash Grenade Launcher" }),
                ("Ice Gun",          new[]{ "Ice Gun","Enhanced Ice Gun","Ultra Ice Gun" }),
                ("Melee",            new[]{ "Dive Knife","Upgraded Dive Knife" }),
            };
            foreach (var (treeName, variants) in trees)
            {
                int got = 0;
                foreach (var v in variants) if (ModSaveData.IsWeaponUnlocked(v)) got++;
                DrawBar($"{treeName}: {got}/{variants.Length}", (float)got / variants.Length);
            }
        }

        private static void DrawBar(string label, float fraction)
        {
            fraction = Math.Clamp(fraction, 0f, 1f);
            GUILayout.BeginHorizontal();
            // Simple text-based bar using a fixed-width label
            int barWidth = 20;
            int filled = Mathf.RoundToInt(fraction * barWidth);
            string bar = "[" + new string('|', filled) + new string('.', barWidth - filled) + "]";
            GUILayout.Label($"{bar} {label}");
            GUILayout.EndHorizontal();
        }

        private static int GetSessionCategoryTotal(string key, int fallback)
        {
            var session = ArchipelagoClient.Session;
            if (session == null) return fallback;

            string prefix = key switch
            {
                "fish"         => "First Catch:",
                "boss"         => "Defeat:",
                "recipe"       => "Unlock Recipe:",
                "dish_upgrade" => "Upgrade ",
                "weapon"       => "Craft:",
                "ecowatcher"   => "Ecowatcher:",
                "cooksta"      => "Cooksta:",
                "photography"  => "Photo:",
                "ingredient"   => "First Find:",
                "charm"        => "Charm:",
                _              => null,
            };

            if (prefix == null) return fallback;

            int count = 0;
            foreach (var locId in session.Locations.AllLocations)
            {
                var name = session.Locations.GetLocationNameFromId(locId) ?? "";
                if (name.StartsWith(prefix)) count++;
            }
            return count > 0 ? count : fallback;
        }
    }
}
