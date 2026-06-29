using System.Collections.Generic;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Tracks which AP locations have been completed and maps
    /// in-game events to their corresponding location IDs/names.
    /// 
    /// Location names must exactly match the names defined in locations.py.
    /// The Archipelago server maps these names to IDs automatically.
    /// </summary>
    public static class LocationTracker
    {
        private static ManualLogSource Log => Plugin.Log;

        // ── Fish catches ─────────────────────────────────────────────────────

        /// <summary>
        /// Called when the player catches a fish species for the first time.
        /// </summary>
        public static void OnFirstFishCatch(string fishName)
        {
            var locationName = $"First Catch: {fishName}";
            Log.LogInfo($"First catch: {fishName}");
            ArchipelagoClient.CheckLocation(locationName);
        }

        // ── Story progression ─────────────────────────────────────────────────

        private static readonly Dictionary<int, string> ChapterLocations = new()
        {
            { 0, "Story: Complete Prologue" },
            { 1, "Story: Complete Chapter 1 (Traces of the Sea People)" },
            { 2, "Story: Complete Chapter 2 (Into the Deep)" },
            { 3, "Story: Complete Chapter 3 (A Request from the Sea People)" },
            { 4, "Story: Complete Chapter 4 (Abandoned Cave)" },
            { 5, "Story: Complete Chapter 5 (Frozen Passage)" },
            { 6, "Story: Complete Chapter 6 (Melting Glacier)" },
            { 7, "Story: Complete Chapter 7 (Broken Control Room)" },
        };

        public static void OnChapterComplete(int chapterNumber)
        {
            if (ChapterLocations.TryGetValue(chapterNumber, out var locationName))
            {
                Log.LogInfo($"Chapter {chapterNumber} complete");
                ArchipelagoClient.CheckLocation(locationName);
            }

            // Check if this completes the goal
            if (chapterNumber == 7 && ArchipelagoClient.SlotData?.Goal == 0)
                GoalTracker.OnYawieDefeated();
        }

        public static void OnSeaPeopleVillageDiscovered()
        {
            ArchipelagoClient.CheckLocation("Story: Discover Sea People Village");
        }

        public static void OnGlacierPassageDiscovered()
        {
            ArchipelagoClient.CheckLocation("Story: Discover Glacier Passage");
        }

        // ── Boss defeats ─────────────────────────────────────────────────────

        private static readonly Dictionary<string, string> BossLocations = new()
        {
            { "Giant Squid",          "Defeat: Giant Squid" },
            { "Clione Queen",         "Defeat: Clione Queen" },
            { "Truck Hermit Crab",    "Defeat: Truck Hermit Crab" },
            { "Giant Wolf Eel",       "Defeat: Giant Wolf Eel" },
            { "Goblin Shark",         "Defeat: Goblin Shark" },
            { "Phantom Jellyfish",    "Defeat: Phantom Jellyfish" },
            { "Giant Gadon",          "Defeat: Giant Gadon" },
            { "Helicoprion",          "Defeat: Helicoprion" },
            { "Kronosaurus",          "Defeat: Kronosaurus" },
            { "John Watson",          "Defeat: John Watson" },
            { "Ebirah",               "Defeat: Ebirah" },
            { "Klaus",                "Defeat: Great White Shark Klaus" },
            { "Mantis Shrimp",        "Defeat: Mantis Shrimp" },
            { "Lusca",                "Defeat: Lusca" },
            { "Torben",               "Defeat: Torben" },
            { "Yawie",                "Defeat: Yawie (Final Boss)" },
        };

        public static void OnBossDefeated(string bossName)
        {
            if (BossLocations.TryGetValue(bossName, out var locationName))
            {
                Log.LogInfo($"Boss defeated: {bossName}");
                ArchipelagoClient.CheckLocation(locationName);
            }

            // Update goal tracker for all bosses (including optional ones not in BossLocations)
            GoalTracker.OnBossDefeated(bossName);

            // Special handling for Yawie (final boss)
            if (bossName == "Yawie")
                GoalTracker.OnYawieDefeated();
        }

        // ── Recipe unlocks ────────────────────────────────────────────────────

        public static void OnRecipeUnlocked(string recipeName)
        {
            ArchipelagoClient.CheckLocation($"Recipe Unlock: {recipeName}");
        }

        // ── Dish upgrades ─────────────────────────────────────────────────────

        public static void OnDishUpgraded(string dishName, int newLevel)
        {
            if (newLevel >= 2)
                ArchipelagoClient.CheckLocation($"Dish Research: {dishName} Level {newLevel}");
        }

        // ── Restaurant milestones ─────────────────────────────────────────────

        public static void OnCustomersServed(int totalServed)
        {
            var milestones = new[] { 10, 50, 100, 250, 500 };
            foreach (var m in milestones)
                if (totalServed == m)
                    ArchipelagoClient.CheckLocation($"Serve {m} Customers");
        }

        public static void OnRestaurantRatingAchieved(int stars)
        {
            if (stars >= 3)
                ArchipelagoClient.CheckLocation($"Restaurant Rating: {stars} Stars");

            // Update goal tracker
            GoalTracker.OnRestaurantRatingAchieved(stars);
        }

        // ── Weapon crafting ───────────────────────────────────────────────────

        public static void OnWeaponCrafted(string weaponName)
        {
            ArchipelagoClient.CheckLocation($"Craft: {weaponName}");
        }

        // ── Quest completion ──────────────────────────────────────────────────

        public static void OnQuestCompleted(string questName)
        {
            // Map game quest names to AP location names
            var locationName = $"Quest: {questName}";
            ArchipelagoClient.CheckLocation(locationName);
        }

        // ── Ecowatcher missions ───────────────────────────────────────────────

        public static void OnEcowatcherMissionCompleted(string missionName)
        {
            ArchipelagoClient.CheckLocation($"Ecowatcher: {missionName}");

            // Check if this completes the full collections
            if (missionName == "Complete All Fish")
                GoalTracker.OnAllFishComplete();
            if (missionName == "Complete All Marinca")
                GoalTracker.OnAllMarincaComplete();
        }

        public static void OnEcowatcherLevelUp(int newLevel)
        {
            // Award charm locations for ecowatcher level ups
            var charmLocations = new Dictionary<int, string>
            {
                { 2, "Charm: Eco Poison Resist Bracelet (Ecowatcher Level 2)" },
                { 3, "Charm: Eco Health Bracelet (Ecowatcher Level 3)" },
                { 4, "Charm: Eco Gemstone Bracelet (Ecowatcher Level 4)" },
                { 5, "Charm: Eco Waterproof Bag (Ecowatcher Level 5)" },
            };
            if (charmLocations.TryGetValue(newLevel, out var loc))
                ArchipelagoClient.CheckLocation(loc);
        }

        // ── Cooksta rank milestones ───────────────────────────────────────────
        // Real Cooksta rank thresholds from the game:
        // Bronze=10, Silver=20, Gold=100, Platinum=200, Diamond=720

        private static int _lastCookstaRank = -1; // -1 = no rank yet

        // Follower milestone locations — each is a separate check
        private static readonly (int followers, string locationName)[] CookstaFollowerMilestones =
        {
            (10,  "Cooksta: 10 Followers"),
            (20,  "Cooksta: 20 Followers"),
            (100, "Cooksta: 100 Followers"),
            (200, "Cooksta: 200 Followers"),
            (720, "Cooksta: 720 Followers"),
        };

        public static void OnCookstaFollowersChanged(int followers)
        {
            // Check each follower milestone
            foreach (var (threshold, locationName) in CookstaFollowerMilestones)
                if (followers >= threshold)
                    ArchipelagoClient.CheckLocation(locationName);

            // Update goal tracker
            GoalTracker.OnCookstaFollowersChanged(followers);
        }

        // Best Taste score milestones
        public static void OnBestTasteChanged(int bestTaste)
        {
            var milestones = new[] { (125, "Cooksta: 125 Best Taste"),
                                     (250, "Cooksta: 250 Best Taste"),
                                     (375, "Cooksta: 375 Best Taste") };
            foreach (var (threshold, loc) in milestones)
                if (bestTaste >= threshold)
                    ArchipelagoClient.CheckLocation(loc);

            // Update goal tracker
            GoalTracker.OnBestTasteChanged(bestTaste);
        }

        // Researched recipe count milestones
        public static void OnResearchedRecipesChanged(int count)
        {
            var milestones = new[] { (2,  "Cooksta: 2 Researched Recipes"),
                                     (5,  "Cooksta: 5 Researched Recipes"),
                                     (19, "Cooksta: 19 Researched Recipes"),
                                     (32, "Cooksta: 32 Researched Recipes") };
            foreach (var (threshold, loc) in milestones)
                if (count >= threshold)
                    ArchipelagoClient.CheckLocation(loc);

            // Update goal tracker
            GoalTracker.OnResearchedRecipesChanged(count);
        }

        // ── Ingredient first finds ────────────────────────────────────────────

        public static void OnIngredientFirstFound(string ingredientName)
        {
            ArchipelagoClient.CheckLocation($"Ingredient: First {ingredientName}");
        }

        // ── Charm acquisition ─────────────────────────────────────────────────

        public static void OnCharmAcquired(string charmName, string sourceMission)
        {
            ArchipelagoClient.CheckLocation($"Charm: {charmName} ({sourceMission})");
        }

    }
}
