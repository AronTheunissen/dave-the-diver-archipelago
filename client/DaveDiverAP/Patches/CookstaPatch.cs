using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the Cooksta social media app to detect follower changes and posts.
    ///
    /// IMPORTANT: Class/method names are PLACEHOLDERS.
    /// Decompile GameAssembly.dll with Il2CppDumper to find real names.
    ///
    /// ## What to search for in Il2CppDumper output:
    /// - "Cooksta", "SocialMedia", "SNS", "Instagram" (the game's name for it)
    /// - Methods related to: posting, followers, likes, viral
    /// - Look in the Cheat Engine table — SushiBarCustomer is known, Cooksta
    ///   likely lives near the restaurant management classes
    ///
    /// ## Known classes nearby (from Cheat Engine v1.0.5.1749):
    /// - SushiBarCustomer — restaurant customer class
    /// - StaffBancho, StaffData — restaurant staff
    /// - These suggest restaurant/Cooksta system is in the same namespace
    ///
    /// ## Tracking approach:
    /// - Hook the "post made" callback to count posts and detect viral posts
    /// - Hook the follower count setter to detect milestone crossings
    /// - Hook the "max likes" achievement trigger if it exists
    /// </summary>
    [HarmonyPatch]
    public static class CookstaPatch
    {
        // ── Follower count changed ────────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: SNSInfoSave has ObscuredInt followerCount with getter/setter
        //    set_followerCount(ObscuredInt value) is the property setter
        //    SNSInfoSave is the save data class; SNSInfoManager is the manager singleton
        [HarmonyPatch(typeof(SNSInfoSave), "set_followerCount")]
        [HarmonyPostfix]
        public static void FollowerCount_Postfix(SNSInfoSave __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnCookstaFollowersChanged((int)__instance.followerCount);
        }

        // ── Best Taste score changed ──────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: SNSInfoSave has ObscuredInt m_LikeCount (at 0x24)
        //    This tracks cumulative "best taste" / like score across all posts.
        [HarmonyPatch(typeof(SNSInfoSave), "set_grade")]
        [HarmonyPostfix]
        public static void Grade_Postfix(SNSInfoSave __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;
            // Grade change triggers rank check — also re-check taste score milestones
            LocationTracker.OnBestTasteChanged((int)__instance.followerCount);
        }

        // ── Researched recipe count changed ───────────────────────────────────
        // ✅ CONFIRMED via dump.cs: AchievementEventType.ResearchRecipeCnt = 2
        //    const string ResearchRecipeCnt = "ResearchRecipeCnt" used as achievement key
        //    Hook SaveData.UpdateUnlockRecipeSave (already in RecipeUnlockPatch) and
        //    cross-check total researched count from SaveData.unlockRecipeData dictionary.
        // We hook SNSInfoManager.RankupRoutine as it checks all rank-up conditions including recipes.
        [HarmonyPatch(typeof(SNSInfoManager), "CheckGradeConditionMessage")]
        [HarmonyPostfix]
        public static void CheckGrade_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            // Re-evaluate researched recipe count from save data
            LocationTracker.OnResearchedRecipesChanged(LocationTracker.GetResearchedRecipeCount());
        }
    }
}
