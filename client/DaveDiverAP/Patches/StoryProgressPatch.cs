using HarmonyLib;
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
    [HarmonyPatch]
    public static class StoryProgressPatch
    {
        private static readonly ManualLogSource Log =
            BepInEx.Logging.Logger.CreateLogSource("DaveDiverAP.Missions");

        // ── Chapter completion ────────────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: ChapterManager is the real class (Singleton<ChapterManager>)
        //    Fields: currentChapterInfo, reservedChapterInfo, _chapters (List<ChapterInfo>)
        //    ChapterInfo has chapter number data.
        [HarmonyPatch(typeof(ChapterManager), "set_currentChapterInfo")]
        [HarmonyPostfix]
        public static void OnChapterChanged_Postfix(ChapterInfo value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (value == null) return;
            LocationTracker.OnChapterComplete(value.chapterNumber);
        }

        // ── Mission/Quest completion ──────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: MissionManager.GetClearMissionDialogData(MissionData, bool)
        //    fires when any mission is fully cleared. MissionData.TID is the design-sheet ID.
        //    This is better than hooking UpdateMission because it only fires on CLEAR (not progress).
        [HarmonyPatch(typeof(MissionManager), "GetClearMissionDialogData")]
        [HarmonyPostfix]
        public static void OnMissionCleared_Postfix(MissionData missionData)
        {
            if (missionData == null) return;

            int tid = missionData.TID;
            string nameTextId = missionData.NameTextID ?? "unknown";

            // Always log cleared missions — invaluable for building the TID map
            Log.LogInfo($"[MissionCleared] TID={tid} NameTextID={nameTextId}");

            if (!ArchipelagoClient.IsConnected) return;

            // Route to AP location check if this TID is mapped
            var locationName = QuestNameMapper.GetLocationName(tid);
            if (locationName != null)
                LocationTracker.OnQuestCompleted(locationName);
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
            { 10010003, "Quest: Complete Duff's First Request" },        // Side — Weaponsmith Duff
            { 10010004, "Quest: Help Duff Investigate Blue Hole" },      // Main — Tracking the Sea People
            { 10012004, "Quest: Complete Dr. Bacon's First Request" },   // Side — Red Ecological Data
            { 10010010, "Quest: Obtain Sea People Bracelet from Dr. Bacon" }, // Main — Beyond the Rock Pile
            { 10010007, "Quest: Obtain Bug Net from Dr. Bacon" },        // Side — Otto's Gift?
            { 10016001, "Quest: Complete Cobra's First Request" },       // Side — Hunt Spider Crab 1
            { 10016002, "Quest: Complete Cobra's VIP Challenge" },       // Side — Hunt Spider Crab 2
            { 10010001, "Quest: Complete Bancho's Training" },           // Main — Prepare Sushi Ingredients
            { 10010005, "Quest: Complete A Noisy Customer (Unlock Fish Farm)" }, // Side — A Noisy Customer
            { 10013004, "Quest: Serve Vincent Yamaoka - Visit 1" },      // Side — Gourmet Vincent's Challenge
            { 10013006, "Quest: Serve Vincent Yamaoka - Visit 2" },      // Side — Gourmet Vincent's Challenge
            { 10013017, "Quest: Serve Vincent Yamaoka - Visit 3" },      // Side — Gourmet Vincent's Challenge
            { 10013009, "Quest: Complete Good Ol' Vegetable Sushi!" },   // Side — Good Ol' Vegetable Sushi!
            { 10013007, "Quest: Complete Michael Bang's Inspiration" },  // Side — Michael Bang's Inspiration
            { 10010007, "Quest: Complete Otto's Moray Eel Dish" },       // Side — Otto's Gift?
            { 10013032, "Quest: Complete Jango's Secret Recipe" },       // Side — Make Jango Warm!
            { 10013033, "Quest: Serve Mxmtoon" },                        // Side — A Penny for Sammy's Thoughts
            { 10010018, "Quest: Gain Trust of Sea People" },             // Main — The Sea People Village's Trust
            { 10010016, "Quest: Complete Niamo's Request" },             // Main — Treat Ramo (Niamo area)
            { 10012015, "Quest: Complete Linchen's Request" },           // Side — Grow Sea People Plants
            { 10010016, "Quest: Complete Ramo's Request" },              // Main — Treat Ramo

            // ── Sub-missions (base game) ──────────────────────────────────────
            { 10012004, "Sub-Mission: Red Ecological Data" },            // Side — Red Ecological Data
            { 10010003, "Sub-Mission: Weaponsmith Duff" },               // Side — Weaponsmith Duff
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
    }
}
