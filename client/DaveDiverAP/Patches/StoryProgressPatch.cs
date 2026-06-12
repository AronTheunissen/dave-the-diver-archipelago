using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the story/mission completion system to detect chapter completions
    /// and key story milestones.
    ///
    /// IMPORTANT: Class/method names are PLACEHOLDERS.
    /// Look for: MissionManager, StoryManager, ChapterManager, QuestManager.
    /// Chapter completion likely fires when the final mission of a chapter is done.
    /// </summary>
    [HarmonyPatch]
    public static class StoryProgressPatch
    {
        // ── Chapter completion ────────────────────────────────────────────────
        [HarmonyPatch(typeof(MissionManager), "OnChapterComplete")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnChapterComplete_Postfix(int chapterIndex)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnChapterComplete(chapterIndex);
        }

        // ── Key story milestones ──────────────────────────────────────────────
        [HarmonyPatch(typeof(MissionManager), "OnMissionComplete")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnMissionComplete_Postfix(string missionId)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // Map key mission IDs to their story locations
            switch (missionId)
            {
                case "MISSION_DISCOVER_VILLAGE":    // TODO: real ID
                    LocationTracker.OnSeaPeopleVillageDiscovered();
                    break;
                case "MISSION_DISCOVER_GLACIER":    // TODO: real ID
                    LocationTracker.OnGlacierPassageDiscovered();
                    break;
                case "MISSION_GAIN_SEA_PEOPLE_TRUST": // TODO: real ID
                    ArchipelagoClient.CheckLocation("Story: Gain Sea People Trust");
                    break;
                default:
                    // Route to quest tracker for other missions
                    var questLocation = QuestNameMapper.GetLocationName(missionId);
                    if (questLocation != null)
                        LocationTracker.OnQuestCompleted(questLocation);
                    break;
            }
        }
    }

    public static class QuestNameMapper
    {
        // TODO: Map internal mission IDs to AP location name suffixes
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new()
        {
            // { "MISSION_DUFF_FIRST",        "Complete Duff's First Request" },
            // { "MISSION_DR_BACON_FIRST",    "Complete Dr. Bacon's First Request" },
            // { "MISSION_COBRA_VIP",         "Complete Cobra's VIP Challenge" },
            // Add all quests here
        };

        public static string? GetLocationName(string missionId)
        {
            return _map.TryGetValue(missionId, out var name) ? name : null;
        }
    }
}
