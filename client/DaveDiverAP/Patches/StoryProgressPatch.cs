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
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // ── Story key milestones (tracked separately from chapters) ────────
            // These are sub-missions within chapters that give key items/unlocks.
            // Fill TIDs in during gameplay — watch BepInEx log for [MissionCleared] lines.

            // Story milestones (TIDs to find in-game):
            // { ???,  "Story: Complete The Leahs-chan Rescue" },     // Ch1 — gives Gas Cutter
            // { ???,  "Story: Complete Deliver Key to Tenzhin" },    // Ch4 — gives Sea People Necklace
            // { ???,  "Story: Complete Cobra's Lost Crowbar" },      // Ch5 — gives Crowbar

            // ── VIP quests ───────────────────────────────────────────────────
            // { ???,  "Quest: Complete Duff's First Request" },
            // { ???,  "Quest: Help Duff Investigate Blue Hole" },
            // { ???,  "Quest: Complete Dr. Bacon's First Request" },
            // { ???,  "Quest: Obtain Sea People Bracelet from Dr. Bacon" },
            // { ???,  "Quest: Obtain Bug Net from Dr. Bacon" },
            // { ???,  "Quest: Complete Cobra's First Request" },
            // { ???,  "Quest: Complete Cobra's VIP Challenge" },
            // { ???,  "Quest: Complete Bancho's Training" },
            // { ???,  "Quest: Complete A Noisy Customer (Unlock Fish Farm)" },
            // { ???,  "Quest: Serve Vincent Yamaoka - Visit 1" },
            // { ???,  "Quest: Serve Vincent Yamaoka - Visit 2" },
            // { ???,  "Quest: Serve Vincent Yamaoka - Visit 3" },
            // { ???,  "Quest: Complete Good Ol' Vegetable Sushi!" },
            // { ???,  "Quest: Complete Michael Bang's Inspiration" },
            // { ???,  "Quest: Complete Otto's Moray Eel Dish" },
            // { ???,  "Quest: Complete Jango's Secret Recipe" },
            // { ???,  "Quest: Serve Mxmtoon" },
            // { ???,  "Quest: Gain Trust of Sea People" },
            // { ???,  "Quest: Complete Niamo's Request" },
            // { ???,  "Quest: Complete Linchen's Request" },
            // { ???,  "Quest: Complete Ramo's Request" },
            // { ???,  "Quest: Obtain Sea People Mirror (Teleport)" },

            // ── Sub-missions ─────────────────────────────────────────────────
            // { ???,  "Sub-Mission: Red Ecological Data" },
            // { ???,  "Sub-Mission: Weaponsmith Duff" },
            // { ???,  "Sub-Mission: A Dolphin's Request" },
            // { ???,  "Sub-Mission: Not Enough Workers" },
            // { ???,  "Sub-Mission: A Scolding from Yoshie" },
            // { ???,  "Sub-Mission: What Happened to the Dolphins?" },
            // { ???,  "Sub-Mission: Assisting Ellie" },
            // { ???,  "Sub-Mission: Defeat Pirates" },
            // { ???,  "Sub-Mission: Reticent Girl" },
            // { ???,  "Sub-Mission: Catch Clione" },
            // { ???,  "Sub-Mission: Defeat the Clione Queen" },
            // { ???,  "Sub-Mission: Giant Stingray at Night" },
            // { ???,  "Sub-Mission: Take Pictures of Manta Ray" },
            // { ???,  "Sub-Mission: Whale Cry" },
            // { ???,  "Sub-Mission: Finding the Baby Whale" },
            // { ???,  "Sub-Mission: Stormy Night" },
            // { ???,  "Sub-Mission: Offer Flowers to King Long's Statue" },
            // { ???,  "Sub-Mission: Deliver Mima's Lunch Boxes" },
            // { ???,  "Sub-Mission: Catch the Runaway Seahorses" },
            // { ???,  "Sub-Mission: Talk to Yami at the Game Parlor" },
            // { ???,  "Sub-Mission: Pet Squid Selgio" },
            // { ???,  "Sub-Mission: Daphne's Whistle" },
            // { ???,  "Sub-Mission: Find the Children's Ball" },
            // { ???,  "Sub-Mission: Sea Person at the Workshop" },
            // { ???,  "Sub-Mission: Wedding Song Record" },
            // { ???,  "Sub-Mission: Repair Kinglong's Statue" },
            // { ???,  "Sub-Mission: Curious Child" },
            // { ???,  "Sub-Mission: Lost Baby Manatee" },
            // { ???,  "Sub-Mission: Trapped in the Glacial Cave" },
            // { ???,  "Sub-Mission: Clara's Omani (Klaus Quest)" },

            // ── Cooking competitions ──────────────────────────────────────────
            // { ???,  "Competition: Defeat Vincent Yamaoka" },
            // { ???,  "Competition: Defeat Wang Pang" },
            // { ???,  "Competition: Defeat Alex Cooper" },
            // { ???,  "Competition: Defeat Pastro Antogiovani" },
        };

        public static string? GetLocationName(int missionTID) =>
            _map.TryGetValue(missionTID, out var name) ? name : null;
    }
}
