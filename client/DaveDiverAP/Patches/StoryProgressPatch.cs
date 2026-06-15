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
        // ✅ CONFIRMED via dump.cs: ChapterManager is the real class (Singleton<ChapterManager>)
        //    Fields: currentChapterInfo, reservedChapterInfo, _chapters (List<ChapterInfo>)
        //    ChapterInfo has chapter number data.
        // We hook ChapterManager to detect chapter changes via currentChapterInfo setter.
        [HarmonyPatch(typeof(ChapterManager), "set_currentChapterInfo")]
        [HarmonyPostfix]
        public static void OnChapterChanged_Postfix(ChapterInfo value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (value == null) return;
            // ChapterInfo contains the chapter number — route to location tracker
            LocationTracker.OnChapterComplete(value.chapterNumber);
        }

        // ── Key story milestones ──────────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: MissionManager is the real class (Singleton<MissionManager>)
        //    Static method: UpdateMission(MissionClearType type, int target, int count, ...)
        //    This fires whenever any mission progress is updated/completed.
        [HarmonyPatch(typeof(MissionManager), "UpdateMission")]
        [HarmonyPostfix]
        public static void OnMissionUpdate_Postfix(MissionClearType type, int target, int count)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // Route completed missions (count >= required) to the quest tracker
            var questLocation = QuestNameMapper.GetLocationName(target);
            if (questLocation != null)
                LocationTracker.OnQuestCompleted(questLocation);
        }
    }

    public static class QuestNameMapper
    {
        // Maps mission TID (design sheet integer) to AP location name.
        // MissionManager.UpdateMission takes int target which is the mission TID.
        // TODO: Cross-reference mission TIDs from the game's mission design sheet data.
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // Example layout — replace with real mission TIDs:
            // { 30001, "Quest: Complete Duff's First Request" },
            // { 30002, "Quest: Complete Dr. Bacon's Research Request" },
            // { 30003, "Quest: Complete Cobra's VIP Challenge" },
            // { 30004, "Quest: Complete Otto's Hiring Request" },
            // ... all story quests and VIP missions
        };

        public static string? GetLocationName(int missionTID) =>
            _map.TryGetValue(missionTID, out var name) ? name : null;
    }
}
