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
        //
        // 🛡️ LOAD GUARD: IsGameReady is false during save deserialization, preventing the
        //    silent crash that occurred when these setters fired during "Continue" loading.
        [HarmonyPatch(typeof(SNSInfoSave), "set_followerCount")]
        [HarmonyPostfix]
        public static void FollowerCount_Postfix(SNSInfoSave __instance)
        {
            try
            {
                if (!ItemQueue.IsGameReady) return;
                if (!ArchipelagoClient.IsConnected) return;
                LocationTracker.OnCookstaFollowersChanged((int)__instance.followerCount);
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[CookstaPatch] FollowerCount_Postfix threw: {ex}");
            }
        }

        // ── Best Taste score changed ──────────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: SNSInfoSave has ObscuredInt m_LikeCount (at 0x24)
        //    This tracks cumulative "best taste" / like score across all posts.
        //
        // 🛡️ LOAD GUARD: Same IsGameReady guard as set_followerCount above.
        [HarmonyPatch(typeof(SNSInfoSave), "set_grade")]
        [HarmonyPostfix]
        public static void Grade_Postfix(SNSInfoSave __instance)
        {
            try
            {
                if (!ItemQueue.IsGameReady) return;
                if (!ArchipelagoClient.IsConnected) return;
                // Use grade (not followerCount) since we're patching the grade setter
                LocationTracker.OnBestTasteChanged((int)__instance.grade);
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[CookstaPatch] Grade_Postfix threw: {ex}");
            }
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
            // TODO: GetResearchedRecipeCount not yet implemented in LocationTracker
            // LocationTracker.OnResearchedRecipesChanged(LocationTracker.GetResearchedRecipeCount());
        }
    }
}
