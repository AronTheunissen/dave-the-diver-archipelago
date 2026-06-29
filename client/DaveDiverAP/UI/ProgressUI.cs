using System;
using System.Collections.Generic;
using BepInEx.Logging;
using ImGuiNET;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// Progress tracker tab — shows goal progress, received items state,
    /// and per-category location check breakdown.
    ///
    /// Three sections:
    ///   1. Overall + goal-specific requirements (live from GoalTracker + SaveData)
    ///   2. Received items summary (progressive levels, key items, charms, weapons)
    ///   3. Location category breakdown (how many checks done per category)
    /// </summary>
    public static class ProgressUI
    {
        // ── State ─────────────────────────────────────────────────────────────
        private static int _totalChecked   = 0;
        private static int _totalLocations = 0;

        // Per-category: how many checks completed. Populated from the AP session
        // on connect and incremented as checks come in.
        private static readonly Dictionary<string, int> _categoryChecked = new();

        // Which tab is open in the items section
        private static int _itemsTab = 0;

        // ── Colors ────────────────────────────────────────────────────────────
        private static readonly System.Numerics.Vector4 ColDone       = new(0.2f, 0.8f, 0.2f, 1f);
        private static readonly System.Numerics.Vector4 ColProgress   = new(0.9f, 0.7f, 0.1f, 1f);
        private static readonly System.Numerics.Vector4 ColNotStarted = new(0.45f, 0.45f, 0.45f, 1f);
        private static readonly System.Numerics.Vector4 ColHeader     = new(0.2f, 0.6f, 0.9f, 1f);
        private static readonly System.Numerics.Vector4 ColSubheader  = new(0.7f, 0.85f, 1f, 1f);
        private static readonly System.Numerics.Vector4 ColHave       = new(0.3f, 0.9f, 0.3f, 1f);
        private static readonly System.Numerics.Vector4 ColMissing    = new(0.6f, 0.6f, 0.6f, 1f);

        // Category totals from the APWorld (static — these are the max possible
        // when all options are enabled; individual runs may have fewer if options
        // are toggled off, but the server's AllLocations is the ground truth).
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

        // ── Initialization ────────────────────────────────────────────────────

        /// <summary>
        /// Called after a successful AP connection to populate initial state
        /// from the server's checked-location list.
        /// </summary>
        public static void Initialize()
        {
            if (ArchipelagoClient.Session == null) return;

            _totalLocations = ArchipelagoClient.Session.Locations.AllLocations.Count;
            _totalChecked   = ArchipelagoClient.Session.Locations.AllLocationsChecked.Count;
            _categoryChecked.Clear();

            // Seed per-category counts from location names of already-checked locations
            foreach (var locId in ArchipelagoClient.Session.Locations.AllLocationsChecked)
            {
                var locName = ArchipelagoClient.Session.Locations.GetLocationNameFromId(locId) ?? "";
                IncrementCategoryForLocation(locName);
            }

            Log.LogInfo($"[ProgressUI] Initialized: {_totalChecked}/{_totalLocations} checked.");
        }

        /// <summary>Call when a new location check fires during play.</summary>
        public static void OnLocationChecked(string locationName)
        {
            _totalChecked++;
            IncrementCategoryForLocation(locationName);
        }

        private static void IncrementCategoryForLocation(string locName)
        {
            // Derive category from location name prefix conventions used in locations.py
            var key = locName switch
            {
                _ when locName.StartsWith("First Catch:")      => "fish",
                _ when locName.StartsWith("Boss:")             => "boss",
                _ when locName.StartsWith("Recipe Unlock:")    => "recipe",
                _ when locName.StartsWith("Dish Research:")    => "dish_upgrade",
                _ when locName.StartsWith("Craft:")            => "weapon",
                _ when locName.StartsWith("Ecowatcher:")       => "ecowatcher",
                _ when locName.StartsWith("Cooksta:")          => "cooksta",
                _ when locName.StartsWith("Story:")            => "story",
                _ when locName.StartsWith("Quest:")            => "story",
                _ when locName.StartsWith("Veg Farm:")         => "farming",
                _ when locName.StartsWith("Chicken Farm:")     => "farming",
                _ when locName.StartsWith("Fish Farm:")        => "farming",
                _ when locName.StartsWith("Photography:")      => "photography",
                _ when locName.StartsWith("Challenge:")        => "challenge",
                _ when locName.StartsWith("Minigame:")         => "minigame",
                _ when locName.StartsWith("Ingredient:")       => "ingredient",
                _ when locName.StartsWith("Charm:")            => "charm",
                _                                              => "other",
            };
            _categoryChecked.TryGetValue(key, out var cur);
            _categoryChecked[key] = cur + 1;
        }

        // ── Draw ──────────────────────────────────────────────────────────────

        public static void Draw()
        {
            ImGui.Spacing();

            if (!ArchipelagoClient.IsConnected)
            {
                ImGui.TextDisabled("Connect to Archipelago to see progress.");
                return;
            }

            // ── 1. Overall + Goal ─────────────────────────────────────────────
            DrawOverallSection();

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            // ── 2. Received Items ─────────────────────────────────────────────
            DrawReceivedItemsSection();

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            // ── 3. Category Breakdown ─────────────────────────────────────────
            DrawCategorySection();
        }

        // ── Section 1: Overall + Goal ─────────────────────────────────────────

        private static void DrawOverallSection()
        {
            ImGui.TextColored(ColHeader, "Overall Progress");
            ImGui.Separator();
            ImGui.Spacing();

            float pct = _totalLocations > 0 ? (float)_totalChecked / _totalLocations : 0f;
            DrawBar($"{_totalChecked} / {_totalLocations} locations checked", pct, ColProgress);

            ImGui.Spacing();

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

            ImGui.TextColored(ColHeader, $"Goal: {goalName}");
            ImGui.Separator();
            ImGui.Spacing();

            DrawGoalRequirements(goal);
        }

        private static void DrawGoalRequirements(int goal)
        {
            // All goals require defeating Yawie
            DrawCheck("Defeat Yawie (Final Boss)", GoalTracker.YawieDefeated);

            // Show what key items are still needed to reach Yawie
            if (!GoalTracker.YawieDefeated)
            {
                ImGui.Indent(16);
                ImGui.TextColored(ColSubheader, "Required to reach Yawie:");
                DrawItemCheck("Glacier Zone access", CanAccessGlacier());
                DrawItemCheck("Control Room Buttons (3)",
                    $"{SaveData.GetControlRoomButtons()}/3",
                    SaveData.GetControlRoomButtons() >= 3);
                DrawItemCheck("Laser Device", SaveData.HasLaserDevice);
                ImGui.Unindent(16);
                ImGui.Spacing();
            }

            if (goal >= 1)
            {
                int bossDone = GoalTracker.DefeatedBossCount;
                DrawBar($"Bosses: {bossDone} / 16 defeated", bossDone / 16f,
                    bossDone >= 16 ? ColDone : ColProgress);
            }

            if (goal == 2 || goal == 4)
            {
                ImGui.Spacing();
                ImGui.TextColored(ColSubheader, "Diamond Rank:");
                ImGui.Indent(8);

                int followers = GoalTracker.CookstaFollowers;
                string rank = GetCookstaRankName(followers);
                DrawBar($"Followers: {followers} / 720  (Rank: {rank})",
                    Math.Min(1f, followers / 720f),
                    followers >= 720 ? ColDone : ColProgress);

                int taste = GoalTracker.CookstaBestTaste;
                DrawBar($"Best Taste: {taste} / 375",
                    Math.Min(1f, taste / 375f),
                    taste >= 375 ? ColDone : ColProgress);

                int recipes = GoalTracker.CookstaResearchedRecipes;
                DrawBar($"Researched Recipes: {recipes} / 32",
                    Math.Min(1f, recipes / 32f),
                    recipes >= 32 ? ColDone : ColProgress);

                ImGui.Unindent(8);
            }

            if (goal == 3 || goal == 4)
            {
                ImGui.Spacing();
                DrawCheck("All Fish Species Caught", GoalTracker.AllFishComplete);
            }
        }

        private static bool CanAccessGlacier()
        {
            // Mirrors APWorld logic: needs Glacier access route + cold suit (suit level 7+)
            bool hasTeleport = SaveData.HasTeleportMirror && SaveData.HasTeleportGlacier;
            bool hasSwimRoute = SaveData.GetDivingSuitLevel() >= 7; // cold-resistant = level 7+
            bool hasTechParts = SaveData.GetTechSuitParts() >= 3;
            return (hasTeleport || hasSwimRoute) && hasTechParts;
        }

        private static string GetCookstaRankName(int followers)
        {
            if (followers >= 720) return "Diamond";
            if (followers >= 200) return "Platinum";
            if (followers >= 100) return "Gold";
            if (followers >= 20)  return "Silver";
            if (followers >= 10)  return "Bronze";
            return "Coal";
        }

        // ── Section 2: Received Items ─────────────────────────────────────────

        private static void DrawReceivedItemsSection()
        {
            ImGui.TextColored(ColHeader, "Received Items");
            ImGui.Separator();
            ImGui.Spacing();

            // Sub-tabs: Equipment | Key Items | Charms | Weapons
            string[] tabs = { "Equipment", "Key Items", "Charms", "Weapons" };
            for (int i = 0; i < tabs.Length; i++)
            {
                if (i > 0) ImGui.SameLine();
                if (ImGui.RadioButton(tabs[i], _itemsTab == i))
                    _itemsTab = i;
            }
            ImGui.Spacing();

            ImGui.BeginChild("ItemsContent", new System.Numerics.Vector2(0, 160), ImGuiChildFlags.None);

            switch (_itemsTab)
            {
                case 0: DrawEquipmentTab();  break;
                case 1: DrawKeyItemsTab();   break;
                case 2: DrawCharmsTab();     break;
                case 3: DrawWeaponsTab();    break;
            }

            ImGui.EndChild();
        }

        private static void DrawEquipmentTab()
        {
            // Progressive items — show current level vs max
            DrawProgressiveItem("Oxygen Tank",   SaveData.GetOxygenTankLevel(),  6);
            DrawProgressiveItem("Diving Suit",   SaveData.GetDivingSuitLevel(),  8,
                suffix: SaveData.GetDivingSuitLevel() switch {
                    0    => " (Starting suit)",
                    1    => " (40m)",
                    2    => " (80m)",
                    3    => " (150m)",
                    4    => " (230m)",
                    5    => " (375m)",
                    6    => " (540m)",
                    7    => " (560m – Cold-Resistant)",
                    >= 8 => " (800m – Cold-Resistant II)",
                    _    => ""
                });
            DrawProgressiveItem("Harpoon",       SaveData.GetHarpoonLevel(),     4);
            DrawProgressiveItem("Cargo Box",     SaveData.GetCargoBoxLevel(),    3);
            ImGui.Spacing();
            DrawProgressiveItem("Tech Suit Parts",     SaveData.GetTechSuitParts(),       3);
            DrawProgressiveItem("Control Room Buttons",SaveData.GetControlRoomButtons(),  3);
            DrawProgressiveItem("Vortex Entries",      SaveData.GetVortexEntries(),        5);
            ImGui.Spacing();
            DrawProgressiveItem("Cooksta Rank", SaveData.GetCookstaRank(), 5,
                suffix: " – " + (SaveData.GetCookstaRank() switch {
                    0 => "Coal", 1 => "Bronze", 2 => "Silver",
                    3 => "Gold", 4 => "Platinum", 5 => "Diamond", _ => "?"
                }));
        }

        private static void DrawProgressiveItem(string label, int level, int max, string suffix = "")
        {
            float pct = max > 0 ? Math.Min(1f, (float)level / max) : 0f;
            var color = pct >= 1f ? ColDone : pct > 0f ? ColProgress : ColNotStarted;
            DrawBar($"{label}: {level}/{max}{suffix}", pct, color, height: 14);
        }

        private static void DrawKeyItemsTab()
        {
            // Two columns of key items
            var items = new[]
            {
                ("Sea People Gloves",        SaveData.HasSeaPeopleGloves),
                ("Sea People Translator",    SaveData.HasTranslator),
                ("Key to Tenzhin",           SaveData.HasKeyToTenzhin),
                ("Laser Device",             SaveData.HasLaserDevice),
                ("Sea People's Trust",       SaveData.HasSeaPeopleTrust),
                ("Teleport Mirror",          SaveData.HasTeleportMirror),
                ("Teleport → Sea People V.", SaveData.HasTeleportSPV),
                ("Teleport → Glacier",       SaveData.HasTeleportGlacier),
                ("Teleport → Deep",          SaveData.HasTeleportDeep),
                ("Unlock Fish Farm",         SaveData.HasFishFarm),
                ("Unlock Vegetable Farm",    SaveData.HasVegetableFarm),
                ("Unlock Chicken Farm",      SaveData.HasChickenFarm),
                ("Bug Net",                  SaveData.HasBugNet),
                ("Night Dive",               SaveData.HasNightDive),
                ("iDiver App",               SaveData.HasiDiverApp),
                ("Sea People Bracelet",      SaveData.HasOxygenGrace),
            };

            int mid = items.Length / 2;
            if (ImGui.BeginTable("KeyItemsTable", 2))
            {
                for (int i = 0; i < mid; i++)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    DrawItemCheck(items[i].Item1, items[i].Item2);
                    if (i + mid < items.Length)
                    {
                        ImGui.TableSetColumnIndex(1);
                        DrawItemCheck(items[i + mid].Item1, items[i + mid].Item2);
                    }
                }
                ImGui.EndTable();
            }
        }

        private static void DrawCharmsTab()
        {
            var charms = new[]
            {
                "Dolphin Necklace", "Octopus Bracelet", "Sea People Bracelet",
                "Octopus Weapon Charm", "Sea People Necklace", "Shark Teeth Necklace",
                "Eco Poison Resist Bracelet", "Eco Health Bracelet",
                "Eco Gemstone Bracelet", "Eco Waterproof Bag",
                "Leo Keychain", "Jimbo Coin",
            };

            if (ImGui.BeginTable("CharmsTable", 2))
            {
                for (int i = 0; i < charms.Length; i += 2)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    DrawItemCheck(charms[i], SaveData.IsCharmAcquired(charms[i]));
                    if (i + 1 < charms.Length)
                    {
                        ImGui.TableSetColumnIndex(1);
                        DrawItemCheck(charms[i + 1], SaveData.IsCharmAcquired(charms[i + 1]));
                    }
                }
                ImGui.EndTable();
            }

            int total  = charms.Length;
            int got    = 0;
            foreach (var c in charms) if (SaveData.IsCharmAcquired(c)) got++;
            ImGui.Spacing();
            ImGui.TextColored(ColSubheader, $"{got} / {total} charms acquired");
        }

        private static void DrawWeaponsTab()
        {
            // Show tree roots and how many upgrades received per tree
            var trees = new[]
            {
                ("Underwater Rifle",   new[]{ "Basic Underwater Rifle","Underwater Rifle II","Underwater Rifle III","Death Rifle","Flame Rifle I","Flame Rifle II","Explosive Rifle","Tranquilizer Rifle","Poison Rifle I","Poison Rifle II","Hell Poison Rifle","Lightning Rifle I","Lightning Rifle II","Shock Rifle I","Shock Rifle II","Thunderbolt Rifle" }),
                ("Net Gun",            new[]{ "Small Net Gun","Medium Net Gun","Large Net Gun","Steel Net Gun" }),
                ("Hush Dart",          new[]{ "Hush Dart","Enhanced Hush Dart" }),
                ("Triple Axel",        new[]{ "Triple Axel","Quattro Axel","Quattro Axel II","Penta Axel","Flame Triple Axel","Flame Triple Axel II","Explosive Triple Axel","Tranquilizer Triple Axel","Poison Triple Axel","Poison Triple Axel II","Hell Poison Triple Axel","Lightning Triple Axel","Shock Triple Axel","Shock Triple Axel II","Thunderbolt Triple Axel" }),
                ("Red Sniper Rifle",   new[]{ "Red Sniper Rifle","Red Sniper Rifle II","Red Sniper Rifle III","Death Sniper Rifle","Flame Sniper Rifle I","Flame Sniper Rifle II","Explosive Sniper Rifle","Tranquilizer Mosin-Nagant","Poison Sniper Rifle I","Poison Sniper Rifle II","Hell Poison Sniper Rifle","Lightning Sniper Rifle I","Lightning Sniper Rifle II","Shock Sniper Rifle I","Shock Sniper Rifle II","Thunderbolt Sniper Rifle" }),
                ("Sticky Bomb Gun",    new[]{ "Sticky Bomb Gun","Sticky Bomb Gun II","Sticky Bomb Gun III","Sticky Mine Launcher I","Sticky Mine Launcher II","Sticky Tranquilizing Bomb Gun","Poison Mine Launcher","Poison Mine Launcher II","Lightning Mine Launcher I","Lightning Mine Launcher II","Shock Mine Launcher I","Shock Mine Launcher II" }),
                ("Grenade Launcher",   new[]{ "Grenade Launcher","Grenade Launcher II","Grenade Launcher III","Tranquilizer Gas Bomb Launcher","Poison Launcher","Gravity Launcher","Blackhole Launcher","Flash Grenade Launcher" }),
                ("Ice Gun",            new[]{ "Ice Gun","Enhanced Ice Gun","Ultra Ice Gun" }),
                ("Drain Gun",          new[]{ "Drain Gun","Enhanced Drain Gun","Power Drain Gun" }),
                ("Melee",              new[]{ "Dive Knife","Upgraded Dive Knife" }),
            };

            ImGui.BeginChild("WeaponsScroll", new System.Numerics.Vector2(0, 130), ImGuiChildFlags.None);
            foreach (var (treeName, variants) in trees)
            {
                int got = 0;
                foreach (var v in variants) if (SaveData.IsWeaponUnlocked(v)) got++;
                float pct = (float)got / variants.Length;
                DrawBar($"{treeName}: {got}/{variants.Length}", pct,
                    pct >= 1f ? ColDone : pct > 0f ? ColProgress : ColNotStarted, height: 13);
            }
            ImGui.EndChild();
        }

        // ── Section 3: Category Breakdown ────────────────────────────────────

        private static void DrawCategorySection()
        {
            ImGui.TextColored(ColHeader, "Location Category Breakdown");
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.BeginChild("CategoryList", new System.Numerics.Vector2(0, 220), ImGuiChildFlags.None);
            foreach (var (label, key, maxTotal) in Categories)
            {
                // Use server's actual total for this session (may be less than max
                // if options are disabled), falling back to the hardcoded max.
                int sessionTotal = GetSessionCategoryTotal(key, maxTotal);
                int done = _categoryChecked.TryGetValue(key, out var d) ? d : 0;
                float pct = sessionTotal > 0 ? Math.Min(1f, (float)done / sessionTotal) : 0f;
                var color = pct >= 1f ? ColDone : pct > 0f ? ColProgress : ColNotStarted;
                DrawBar($"{label}: {done}/{sessionTotal}", pct, color, height: 14);
            }
            ImGui.EndChild();
        }

        /// <summary>
        /// Try to count how many locations in this category the server actually
        /// has for this seed (respects options like fish_checks, toggles, DLC).
        /// Falls back to the hardcoded max if we can't count.
        /// </summary>
        private static int GetSessionCategoryTotal(string key, int fallback)
        {
            var session = ArchipelagoClient.Session;
            if (session == null) return fallback;

            // Derive the location name prefix for this category
            string prefix = key switch
            {
                "fish"        => "First Catch:",
                "boss"        => "Boss:",
                "recipe"      => "Recipe Unlock:",
                "dish_upgrade"=> "Dish Research:",
                "weapon"      => "Craft:",
                "ecowatcher"  => "Ecowatcher:",
                "cooksta"     => "Cooksta:",
                "story"       => null,   // mix of "Story:" and "Quest:"
                "farming"     => null,   // mix of prefixes
                "photography" => "Photography:",
                "challenge"   => "Challenge:",
                "minigame"    => "Minigame:",
                "ingredient"  => "Ingredient:",
                "charm"       => "Charm:",
                _             => null,
            };

            if (prefix == null) return fallback;  // too complex, use fallback

            int count = 0;
            foreach (var locId in session.Locations.AllLocations)
            {
                var name = session.Locations.GetLocationNameFromId(locId) ?? "";
                if (name.StartsWith(prefix)) count++;
            }
            return count > 0 ? count : fallback;
        }

        // ── Shared drawing helpers ────────────────────────────────────────────

        private static void DrawBar(string label, float fraction, System.Numerics.Vector4 color,
                                    float height = 16)
        {
            fraction = Math.Clamp(fraction, 0f, 1f);
            ImGui.PushStyleColor(ImGuiCol.PlotHistogram, color);
            ImGui.ProgressBar(fraction, new System.Numerics.Vector2(-1, height), "");
            ImGui.PopStyleColor();
            ImGui.SameLine(0, 6);
            ImGui.TextUnformatted(label);
        }

        private static void DrawCheck(string label, bool done)
        {
            ImGui.TextColored(done ? ColDone : ColNotStarted, done ? $"✓ {label}" : $"○ {label}");
        }

        private static void DrawItemCheck(string label, bool have)
        {
            ImGui.TextColored(have ? ColHave : ColMissing, have ? $"✓ {label}" : $"  {label}");
        }

        private static void DrawItemCheck(string label, string value, bool done)
        {
            ImGui.TextColored(done ? ColDone : ColProgress, $"{(done ? "✓" : "·")} {label}: {value}");
        }
    }
}
