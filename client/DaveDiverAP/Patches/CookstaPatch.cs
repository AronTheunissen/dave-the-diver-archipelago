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
        // PLACEHOLDER: Replace CookstaManager with real class name
        [HarmonyPatch(typeof(CookstaManager), "set_FollowerCount")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void FollowerCount_Postfix(int value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnCookstaFollowersChanged(value);
        }

        // ── Best Taste score changed ──────────────────────────────────────────
        // Best Taste is the cumulative quality score of dishes served.
        // PLACEHOLDER: Replace with real class/method name from Il2CppDumper
        // Look for: set_BestTaste, OnBestTasteUpdated, or similar in CookstaManager
        [HarmonyPatch(typeof(CookstaManager), "set_BestTaste")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void BestTaste_Postfix(int value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnBestTasteChanged(value);
        }

        // ── Researched recipe count changed ───────────────────────────────────
        // Tracks how many dish recipes have been researched using Artisan's Flame.
        // PLACEHOLDER: Replace with real class/method name from Il2CppDumper
        // Look for: set_ResearchedRecipeCount, OnRecipeResearched, or similar
        [HarmonyPatch(typeof(CookstaManager), "set_ResearchedRecipeCount")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void ResearchedRecipeCount_Postfix(int value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnResearchedRecipesChanged(value);
        }
    }
}
