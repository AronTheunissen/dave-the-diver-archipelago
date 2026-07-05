using HarmonyLib;
// ChapterInfo, ChapterManager are game types in the global namespace (Assembly-CSharp)
using BepInEx.Logging;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the story/mission completion system to detect chapter completions
    /// and key story/quest milestones.
    ///
    /// ✅ CONFIRMED via dump.cs:
    ///   - ChapterManager.set_currentChapterInfo fires on chapter change
    ///   - MissionManager.GetClearMissionDialogData(MissionData, bool) fires when any
    ///     mission is cleared — the MissionData has .TID and .NameTextID
    ///
    /// TID DISCOVERY MODE: On first playthrough, enable BepInEx logging and watch
    /// for "[MissionCleared] TID=XXXXX NameTextID=YYYYYYY" in the log. Use those
    /// values to populate QuestNameMapper._map below.
    /// </summary>
    /// <summary>
    /// Detects mission completions via manual Harmony patching (same approach as ScenarioSkipPatch).
    /// MissionManager.GetClearMissionDialogData fires when any mission is fully cleared.
    /// </summary>
    public static class StoryProgressPatch
    {
        private static readonly ManualLogSource Log =
            BepInEx.Logging.Logger.CreateLogSource("DaveDiverAP.Missions");

        public static void Apply(HarmonyLib.Harmony harmony)
        {
            // Try patching both ApplyMissionClear and ClearMission — one of them must fire.
            // Confirmed from Unity Explorer both exist with (int) parameter.
            int patchCount = 0;
            foreach (var methodName in new[] { "ApplyMissionClear", "ClearMission" })
            {
                var target = typeof(MissionManager).GetMethod(methodName,
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance,
                    null,
                    new System.Type[] { typeof(int) },
                    null);

                if (target == null)
                {
                    Plugin.Log.LogWarning($"[StoryProgress] Could not find {methodName}(int)");
                    continue;
                }

                var postfix = new HarmonyLib.HarmonyMethod(
                    typeof(StoryProgressPatch).GetMethod(nameof(OnMissionCleared_Postfix)));
                harmony.Patch(target, postfix: postfix);
                Plugin.Log.LogInfo($"[StoryProgress] Successfully patched {methodName}");
                patchCount++;
            }
            if (patchCount == 0)
                Plugin.Log.LogWarning("[StoryProgress] No mission clear methods patched — mission checks won't fire");
        }

        private static readonly System.Collections.Generic.HashSet<int> _loggedMissions = new();

        public static void OnMissionCleared_Postfix(int missionID)
        {
            try
            {
                if (missionID == 0) return;
                // Deduplicate — both ApplyMissionClear and ClearMission may fire for same mission
                if (!_loggedMissions.Add(missionID)) return;

                // Look up mission data from MissionManager to get title/type for logging
                string title = "";
                string missionType = "";
                try
                {
                    var mm = MissionManager.Instance;
                    var missionData = mm?.GetMissionData(missionID);
                    if (missionData != null)
                    {
                        title = missionData.Title ?? "";
                        missionType = missionData.Type.ToString();
                        // Skip internal state machine missions
                        if (missionType == "JungleRankReward" || missionType == "JungleRelationEvent") return;
                    }
                }
                catch { }

                // Always log — invaluable for mapping TIDs to AP locations
                Log.LogInfo($"[MissionCleared] TID={missionID} Type={missionType} Title=\"{title}\"");

                if (!ArchipelagoClient.IsConnected) return;

                // Route to AP location check if this TID is mapped
                var locationName = QuestNameMapper.GetLocationName(missionID);
                if (locationName != null)
                    LocationTracker.OnQuestCompleted(locationName);
            }
            catch (System.Exception ex)
            {
                Log.LogError($"[StoryProgressPatch] OnMissionCleared_Postfix threw: {ex}");
            }
        }
    }

    /// <summary>
    /// Skips tutorial/prologue scenario cutscenes when connected to AP.
    /// Uses manual Harmony patching via AccessTools to avoid IL2CPP generic type matching issues.
    /// </summary>
    public static class ScenarioSkipPatch
    {
        // Scenario name prefixes to skip when connected to AP.
        // We skip boat/lobby/restaurant cutscenes but NOT in-dive cutscenes.
        private static readonly string[] _skipPrefixes = {
            // Tutorial & prologue
            "Tutorial_Mission",
            "Tutorial_IDiver",
            "Tutorial07",
            "Tutorial08",
            "BanchoSushi_upgrade",
            "BanchoSushi_upgrade_boat",
            // Main story missions (boat conversations)
            "Main_Mission",
            // Side missions (boat/lobby only — in-dive ones excluded below)
            "Side_",
            // In-restaurant side mission dialogues
            "dialog_Side_",
            // NPC unlock cutscenes (Sato/Marinca, etc.)
            "Fishcard_Contents_Unlock",
        };

        // Scenario name substrings that must NEVER be skipped (interactive in-dive events).
        // These take priority over _skipPrefixes.
        private static readonly string[] _neverSkip = {
            "Dolphin",   // dolphin missions have rope-cutting QTEs
            "Whale",     // whale missions
            "Cutscene",  // all in-game cutscenes
            "Boss",      // boss encounter triggers
            "Escape",    // escape pod sequences
        };

        public static void Apply(HarmonyLib.Harmony harmony)
        {
            // Find StartScenarioInternal by parameter count: (string, Action<bool>, bool, bool, bool) = 5 params
            var methods = typeof(ScenarioManager).GetMethods(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            System.Reflection.MethodInfo? target = null;
            foreach (var m in methods)
            {
                if (m.Name != "StartScenarioInternal") continue;
                var parms = m.GetParameters();
                if (parms.Length == 5 &&
                    parms[0].ParameterType == typeof(string) &&
                    parms[1].ParameterType.Name.Contains("Action") &&
                    parms[2].ParameterType == typeof(bool))
                {
                    target = m;
                    break;
                }
            }

            if (target == null)
            {
                Plugin.Log.LogWarning("[ScenarioSkip] Could not find StartScenarioInternal(string, Action<bool>, bool, bool, bool) — skipping patch");
                return;
            }

            var prefix = new HarmonyLib.HarmonyMethod(
                typeof(ScenarioSkipPatch).GetMethod(nameof(StartScenarioInternal_Prefix)));
            harmony.Patch(target, prefix: prefix);
            Plugin.Log.LogInfo("[ScenarioSkip] Successfully patched StartScenarioInternal for tutorial skipping");
        }

        public static bool StartScenarioInternal_Prefix(object[] __args)
        {
            if (!ArchipelagoClient.IsConnected) return true;
            var dialogueBundleID = __args?[0] as string;
            if (dialogueBundleID == null) return true;

            // Never skip scenarios that fire while diving — in-water cutscenes advance
            // mission state (e.g. dolphin missions, boss intros) and must play through.
            try { if (InGameManager.Instance != null) return true; }
            catch { }

            // Check never-skip list — these are always interactive and must play
            foreach (var never in _neverSkip)
                if (dialogueBundleID.Contains(never)) return true;

            foreach (var prefix in _skipPrefixes)
            {
                if (dialogueBundleID.StartsWith(prefix))
                {
                    Plugin.Log.LogInfo($"[ScenarioSkip] Skipping scenario: {dialogueBundleID}");

                    // Check if this scenario name maps to an AP location
                    if (ArchipelagoClient.IsConnected)
                    {
                        var locationName = QuestNameMapper.GetLocationNameFromScenario(dialogueBundleID);
                        if (locationName != null)
                            LocationTracker.OnQuestCompleted(locationName);
                    }

                    // Try invoking the onFinish callback so state machine continues
                    try
                    {
                        var cb = __args[1];
                        if (cb != null)
                        {
                            var invokeMethod = cb.GetType().GetMethod("Invoke");
                            invokeMethod?.Invoke(cb, new object[] { true });
                        }
                    }
                    catch { }
                    return false;
                }
            }
            return true;
        }
    }

    public static class QuestNameMapper
    {
        // Maps mission TID → AP location name suffix.
        // HOW TO FILL THIS IN:
        //   1. Install mod, play the game normally
        //   2. Watch BepInEx/LogOutput.log for lines like:
        //      [MissionCleared] TID=10012001 NameTextID=MISSION_10012001_NAME
        //   3. Match the TID to the quest you just completed, add it here
        //
        // Known TIDs from dump.cs analysis:
        //   10010002 = Tutorial (End Sushi Bar setup)
        //   10015001 = Dolphin mission task
        //   10012014 = Kazhine book mission
        //   10019805 = First merchant arrival
        //   10061002 = Godzilla DLC earthquake trigger (Ebirah)
        //   10030235 = Investigate Mural (Ch4)
        //   10030236 = Solve Puzzle (Ch4)
        //   10030237 = Open Pressure (Ch4/5)
        //   10030238 = Run from Pirates (Ch5)
        // TIDs confirmed via UnityExplorer MissionDictionary dump (2026-06-27).
        // All 513 cleared missions dumped — only those mapping to AP locations are listed here.
        // Types: Main, Side, OceanLab, Intermission, JungleRankReward, JungleRelationEvent
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // ── Story key milestones ──────────────────────────────────────────
            { 10010012, "Story: Complete The Leahs-chan Rescue" },       // Ch1 — The Leahs-chan Rescue
            { 10010021, "Story: Complete Deliver Key to Tenzhin" },      // Ch4 — Deliver Key to Tenzhin (first step)
            { 10019025, "Story: Complete Cobra's Lost Crowbar" },        // Ch5 — Cobra's Lost Crowbar
            { 10010028, "Story: Obtain Sea People Mirror (Teleport)" },  // Ch5 — Huge Sea People Mirror

            // ── VIP quests ───────────────────────────────────────────────────
            // NOTE: Some TIDs serve double duty (same mission = both a quest and sub-mission check)
            // In those cases we map to the Quest name (more specific) and handle sub-mission via title match
            { 10010003, "Quest: Complete Duff's First Request" },        // Side — Weaponsmith Duff
            { 10010004, "Quest: Help Duff Investigate Blue Hole" },      // Main — Tracking the Sea People
            { 10012004, "Quest: Complete Dr. Bacon's First Request" },   // Side — Red Ecological Data
            { 10010010, "Quest: Obtain Sea People Bracelet from Dr. Bacon" }, // Main — Beyond the Rock Pile
            { 10010007, "Quest: Obtain Bug Net from Dr. Bacon" },        // Side — Otto's Gift? (also Otto's Moray Eel)
            { 10016001, "Quest: Complete Cobra's First Request" },       // Side — Hunt Spider Crab 1
            { 10016002, "Quest: Complete Cobra's VIP Challenge" },       // Side — Hunt Spider Crab 2
            { 10010001, "Quest: Complete Bancho's Training" },           // Main — Prepare Sushi Ingredients
            { 10010005, "Quest: Complete A Noisy Customer (Unlock Fish Farm)" }, // Side — A Noisy Customer
            { 10013004, "Quest: Serve Vincent Yamaoka - Visit 1" },      // Side — Gourmet Vincent's Challenge
            { 10013006, "Quest: Serve Vincent Yamaoka - Visit 2" },      // Side — Gourmet Vincent's Challenge
            { 10013017, "Quest: Serve Vincent Yamaoka - Visit 3" },      // Side — Gourmet Vincent's Challenge
            { 10013009, "Quest: Complete Good Ol' Vegetable Sushi!" },   // Side — Good Ol' Vegetable Sushi!
            { 10013007, "Quest: Complete Michael Bang's Inspiration" },  // Side — Michael Bang's Inspiration
            { 10013032, "Quest: Complete Jango's Secret Recipe" },       // Side — Make Jango Warm!
            { 10013033, "Quest: Serve Mxmtoon" },                        // Side — A Penny for Sammy's Thoughts
            { 10010018, "Quest: Gain Trust of Sea People" },             // Main — The Sea People Village's Trust
            { 10010016, "Quest: Complete Niamo's Request" },             // Main — Treat Ramo (shared TID with Ramo's Request)
            { 10012015, "Quest: Complete Linchen's Request" },           // Side — Grow Sea People Plants

            // ── Sub-missions (base game) ──────────────────────────────────────
            { 10015001, "Sub-Mission: A Dolphin's Request" },            // Side — A Dolphin's Request
            { 10012903, "Sub-Mission: Not Enough Workers" },             // Side — Not Enough Workers
            { 10012003, "Sub-Mission: A Scolding from Yoshie" },        // Side — A Scolding from Yoshie
            { 10015003, "Sub-Mission: What Happened to the Dolphins?" }, // Side — What Happened to the Dolphins?
            { 10012007, "Sub-Mission: Assisting Ellie" },                // Side — Assisting Ellie
            { 10015004, "Sub-Mission: Defeat Pirates" },                 // Side — Defeat Pirates
            { 10012008, "Sub-Mission: Reticent Girl" },                  // Side — Reticent Girl
            { 10012009, "Sub-Mission: Catch Clione" },                   // Side — Catch Clione
            { 10012909, "Sub-Mission: Defeat the Clione Queen" },        // Side — Defeat the Clione Queen
            { 10012028, "Sub-Mission: Giant Stingray at Night" },        // Side — Giant Stingray at Night
            { 10012029, "Sub-Mission: Take Pictures of Manta Ray" },     // Side — Take Pictures of Manta Ray
            { 10015005, "Sub-Mission: Whale Cry" },                      // Side — Whale Cry
            { 10015006, "Sub-Mission: Finding the Baby Whale" },         // Side — Finding The Baby Whale
            { 10015007, "Sub-Mission: Stormy Night" },                   // Side — Stormy Night
            { 10012013, "Sub-Mission: Offer Flowers to King Long's Statue" }, // Side — Offer Flowers to Kinglong's Statue
            { 10012010, "Sub-Mission: Deliver Mima's Lunch Boxes" },     // Side — Deliver Mima's Lunch Boxes
            { 10012019, "Sub-Mission: Catch the Runaway Seahorses" },    // Side — Catch The Runaway Seahorses
            { 10012020, "Sub-Mission: Talk to Yami at the Game Parlor" },// Side — Talk to Yami at The Game Parlor
            { 10012021, "Sub-Mission: Pet Squid Selgio" },               // Side — Pet Squid Selgio
            { 10012026, "Sub-Mission: Daphne's Whistle" },               // Side — Daphne's Whistle
            { 10012016, "Sub-Mission: Find the Children's Ball" },       // Side — Find the Children's Ball
            { 10012023, "Sub-Mission: Sea Person at the Workshop" },     // Side — Sea Person at the Workshop
            { 10012017, "Sub-Mission: Wedding Song Record" },            // Side — Wedding Song Record
            { 10012018, "Sub-Mission: Repair Kinglong's Statue" },      // Side — Repair Kinglong's Statue
            { 10012022, "Sub-Mission: Curious Child" },                  // Side — Curious Child
            { 10012030, "Sub-Mission: Lost Baby Manatee" },              // Side — Lost Baby Manatee
            { 10012033, "Sub-Mission: Trapped in the Glacial Cave" },    // Side — Trapped in the Glacial Cave
            { 10017002, "Sub-Mission: Clara's Omani (Klaus Quest)" },    // Side — Revenge Time!

            // ── Cooking competitions ──────────────────────────────────────────
            { 10013011, "Competition: Defeat Vincent Yamaoka" },         // Side — Chinese Cuisine Contest!
            { 10013013, "Competition: Defeat Wang Pang" },               // Side — Whose Fried food is the Best?
            { 10013012, "Competition: Defeat Alex Cooper" },             // Side — Let Us Begin the Contest
            { 10013015, "Competition: Defeat Pastro Antogiovani" },      // Side — Bancho's Ordeal? Pasta Contest!

            // ── Ecowatcher missions (OceanLab type) ──────────────────────────
            { 10018001, "Ecowatcher: Research Starfish 1" },
            { 10018002, "Ecowatcher: Research Starfish 2" },
            { 10018003, "Ecowatcher: Research Starfish 3" },
            { 10018004, "Ecowatcher: Research Starfish 4" },
            { 10018005, "Ecowatcher: Research Starfish 5" },
            { 10018011, "Ecowatcher: Research Shell 1" },
            { 10018012, "Ecowatcher: Research Shell 2" },
            { 10018013, "Ecowatcher: Research Shell 3" },
            { 10018014, "Ecowatcher: Research Shell 4" },
            { 10018015, "Ecowatcher: Research Shell 5" },
            { 10018021, "Ecowatcher: Research Marine Plants 1" },
            { 10018022, "Ecowatcher: Research Marine Plants 2" },
            { 10018023, "Ecowatcher: Research Marine Plants 3" },
            { 10018024, "Ecowatcher: Research Marine Plants 4" },
            { 10018025, "Ecowatcher: Research Marine Plants 5" },
            { 10018031, "Ecowatcher: Research Fossils 1" },
            { 10018032, "Ecowatcher: Research Fossils 2" },
            { 10018033, "Ecowatcher: Research Fossils 3" },
            { 10018041, "Ecowatcher: Investigate Glacial Marine Plants 1" },
            { 10018042, "Ecowatcher: Investigate Glacial Marine Plants 2" },
            { 10018043, "Ecowatcher: Investigate Glacial Marine Plants 3" },
            { 10018051, "Ecowatcher: Collect Glacial Clams 1" },
            { 10018052, "Ecowatcher: Collect Glacial Clams 2" },
            { 10018061, "Ecowatcher: Defeat Invasive Starfish 1" },
            { 10018062, "Ecowatcher: Defeat Invasive Starfish 2" },
            { 10018071, "Ecowatcher: Investigate Sea People's Artifact 1" },
            { 10018072, "Ecowatcher: Investigate Sea People's Artifact 2" },
            { 10018081, "Ecowatcher: Investigate Dangerous Gemstones 1" },
            { 10018082, "Ecowatcher: Investigate Dangerous Gemstones 2" },
            { 10018083, "Ecowatcher: Investigate Dangerous Gemstones 3" },
            { 10018101, "Ecowatcher: Remove Jellyfish 1" },
            { 10018102, "Ecowatcher: Remove Jellyfish 2" },
            { 10018103, "Ecowatcher: Remove Jellyfish 3" },
            { 10018104, "Ecowatcher: Remove Jellyfish 4" },
            { 10018105, "Ecowatcher: Remove Jellyfish 5" },  // Extra tier confirmed in dump
            { 10018111, "Ecowatcher: Overpopulated Invasive Fish 1" },
            { 10018112, "Ecowatcher: Overpopulated Invasive Fish 2" },
            { 10018113, "Ecowatcher: Overpopulated Invasive Fish 3" },
            { 10018114, "Ecowatcher: Overpopulated Invasive Fish 4" },
            { 10018115, "Ecowatcher: Overpopulated Invasive Fish 5" },
            { 10018116, "Ecowatcher: Overpopulated Invasive Fish 6" },  // Extra tier confirmed in dump
            { 10018121, "Ecowatcher: Investigate Regional Ecology 1" },
            { 10018122, "Ecowatcher: Investigate Regional Ecology 2" },
            { 10018123, "Ecowatcher: Investigate Regional Ecology 3" },

            // ── Godzilla DLC missions ─────────────────────────────────────────
            { 10011000, "Godzilla: Go to Bancho Sushi" },
            { 10011001, "Godzilla: Kaiju's Hideout" },
            { 10011002, "Godzilla: Godzilla In Crisis!" },
            { 10011003, "Godzilla: Lost Kaiju Figurines" },
            { 10011011, "Godzilla: Operation Sea Blue Eradication" },
            { 10011012, "Godzilla: Bartender's Favorite Meal!" },
            { 10011013, "Godzilla: Deliver Cold Noodles to Bartender" },
            { 10011015, "Godzilla: Buckwheat required" },

            // ── Jungle DLC main story ─────────────────────────────────────────
            { 410010000, "Jungle: To a New Place" },
            { 410010001, "Jungle: Let's Head to the Jungle!" },
            { 410010002, "Jungle: Welcome to the Jungle!" },
            { 410010003, "Jungle: Jungle Chef" },
            { 410010005, "Jungle: First Day in the Jungle" },
            { 410010006, "Jungle: The Old House" },
            { 410010007, "Jungle: Bancho's Cooking Tools" },
            { 410010008, "Jungle: Muna's Research" },
            { 410010009, "Jungle: The Path to the Forest" },
            { 410010010, "Jungle: To Setah Forest" },
            { 410010011, "Jungle: Find the Puppy" },
            { 410010012, "Jungle: To Murau Temple" },
            { 410010013, "Jungle: Into the Depths of the Lake" },
            { 410010014, "Jungle: The Cause of the Strange Phenomenon" },
            { 410010015, "Jungle: Back to the Temple" },
            { 410010016, "Jungle: Find the blue Divine Tree Fruit!" },
            { 410010017, "Jungle: Return to the Village!" },
            { 410010018, "Jungle: Another Sea People Tribe" },
            { 410010019, "Jungle: The Tyrant Xiphactinus" },
            { 410010020, "Jungle: Bombs Away" },

            // ── Jungle DLC side missions ──────────────────────────────────────
            { 410012001, "Jungle Quest: Bamboo Shoots" },
            { 410012002, "Jungle Quest: The Red Feather Charm" },
            { 410012003, "Jungle Quest: The Walking Catfish in the Mud" },
            { 410012004, "Jungle Quest: Lipah's Fireflies" },
            { 410012005, "Jungle Quest: The Effects of Hornwort" },
            { 410012006, "Jungle Quest: Hide and Seek!" },
            { 410012007, "Jungle Quest: Gesang's Request" },
            { 410012008, "Jungle Quest: The Kissing Fish" },
            { 410012009, "Jungle Quest: The Night-Blooming Flower" },
            { 410012010, "Jungle Quest: Omila's Floral Plate" },
            { 410012011, "Jungle Quest: The Star-Shaped Fruit" },
            { 410012012, "Jungle Quest: Harta's Gambling Debt" },
            { 410012013, "Jungle Quest: Chandra's Suspicion" },
            { 410012014, "Jungle Quest: Strong Teeth" },
            { 410012015, "Jungle Quest: Eating fish?!" },
            { 410012017, "Jungle Quest: Snail in the Rain" },
            { 410012018, "Jungle Quest: How to Store Skewers" },
            { 410012019, "Jungle Quest: Gesang's Crisis!" },
            { 410012020, "Jungle Quest: Gathering Coconuts" },
            { 410012021, "Jungle Quest: Mysterious Utara Crystal!" },
            { 410012022, "Jungle Quest: Vicious Catfish" },
            { 410012023, "Jungle Quest: Clumsy Slime?" },
            { 410012024, "Jungle Quest: Perak's Gift" },
            { 410012025, "Jungle Quest: Unihornus Bones" },
            { 410012725, "Jungle Quest: Explorer in the Forest" },
            { 410012825, "Jungle Quest: Horn of a Unihornus" },
            { 410012026, "Jungle Quest: The Midnight Candy Thief" },
            { 410012027, "Jungle Quest: The Candy Thief's Footprints" },
            { 410012028, "Jungle Quest: Catch the Candy Thief!" },
            { 410012029, "Jungle Quest: Overgrowing Weeds" },
            { 410012030, "Jungle Quest: Shoo, Monkeys!" },
            { 410012031, "Jungle Quest: The Mysterious Garden" },
            { 410012032, "Jungle Quest: The Bull Shark's Carcass" },
            { 410012033, "Jungle Quest: Operation: Sulong Hunt" },
            { 410012101, "Jungle Quest: The Torn Bug Net" },
            { 410012102, "Jungle Quest: Traces of the UFO" },
            { 410012104, "Jungle Quest: Lake Purification" },
            { 410012105, "Jungle Quest: The Musician of Nashville" },
            { 410012109, "Jungle Quest: The Village's Traditional Game" },
            { 410012110, "Jungle Quest: Animal Block Stacking Showdown!" },
            { 410012111, "Jungle Quest: Bird Hunting with Marone!" },
            { 410012113, "Jungle Quest: A Monster Snapping Turtle?" },
            { 410012813, "Jungle Quest: Beat the Snapping Turtle!" },
            { 410012201, "Jungle Quest: The Foldable Mouse" },
            { 410012202, "Jungle Quest: A Big-Eyed Lizard" },
            { 410016101, "Jungle Quest: Cinta, Food of Memories" },
            { 410016102, "Jungle Quest: Something for Cinta" },
            { 410016110, "Jungle Quest: The Taste of Fish" },
            { 410016111, "Jungle Quest: Crocodile Tail Cuisine" },
        };

        public static string? GetLocationName(int missionTID) =>
            _map.TryGetValue(missionTID, out var name) ? name : null;

        // Maps scenario completion names to AP location names.
        // Scenario names ending in "_Complete" indicate mission completion.
        // TODO: Fill in the correct mappings by playing through the game and
        // watching for "[ScenarioSkip] Unknown completion scenario: X" in the log.
        // The scenario name numbering does NOT correspond to chapter numbers.
        private static readonly System.Collections.Generic.Dictionary<string, string> _scenarioMap = new()
        {
            // ── Sub-missions (confirmed from actual gameplay) ─────────────────
            { "Side_Dolphin01_Complete",    "Sub-Mission: A Dolphin's Request" },
            { "Side_Dolphin02_Complete",    "Sub-Mission: What Happened to the Dolphins?" },
            { "Side_Ellie_01_Complete",     "Sub-Mission: Assisting Ellie" },
            { "Side_Duff01_Complete",       "Sub-Mission: Weaponsmith Duff" },
            // TODO: Add more as you play through and see the scenario names in the log
        };

        public static string? GetLocationNameFromScenario(string scenarioName)
        {
            if (scenarioName == null) return null;

            // Direct lookup first
            if (_scenarioMap.TryGetValue(scenarioName, out var name))
                return name;

            // Log unknown _Complete scenarios so we can map them later
            if (scenarioName.EndsWith("_Complete"))
                Plugin.Log.LogInfo($"[ScenarioSkip] Unknown completion scenario: {scenarioName} — add to QuestNameMapper");

            return null;
        }
    }
}
