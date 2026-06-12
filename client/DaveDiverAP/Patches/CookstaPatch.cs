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
        // ── Post made ─────────────────────────────────────────────────────────
        // PLACEHOLDER: Replace CookstaManager with real class name
        // Look for a method called when the player submits a Cooksta post
        [HarmonyPatch(typeof(CookstaManager), "OnPostSubmitted")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnPostSubmitted_Postfix(object __instance, bool isViral)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnCookstaPostMade(isViral);
        }

        // ── Follower count changed ────────────────────────────────────────────
        // PLACEHOLDER: Hook the follower count setter
        // Fires whenever the follower count increases (after a successful post)
        [HarmonyPatch(typeof(CookstaManager), "set_FollowerCount")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void FollowerCount_Postfix(int value)
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnCookstaFollowersChanged(value);
        }

        // ── Max likes on a post ───────────────────────────────────────────────
        // PLACEHOLDER: Hook the max likes achievement trigger
        // This may fire as part of the post result calculation
        [HarmonyPatch(typeof(CookstaManager), "OnMaxLikesAchieved")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnMaxLikesAchieved_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            LocationTracker.OnCookstaMaxLikes();
        }
    }
}
