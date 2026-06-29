using System.Collections.Generic;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Tracks progress toward the player's chosen victory condition and
    /// sends the goal completion signal to the Archipelago server when met.
    ///
    /// Goals (matching options.py Goal values):
    ///   0 = Defeat Yawie              — defeat the final boss
    ///   1 = Defeat All Bosses         — Yawie + all 15 optional/story/vortex bosses
    ///   2 = Diamond Rank              — Yawie + 720 followers + 375 best taste + 32 recipes
    ///   3 = Master Diver              — Yawie + catch every fish species
    ///   4 = 100% Completion           — all of the above combined
    ///
    /// Call the appropriate On*() method whenever the player achieves something.
    /// GoalTracker will check if the overall goal is now satisfied.
    /// </summary>
    public static class GoalTracker
    {
        private static ManualLogSource Log => Plugin.Log;

        // ── Tracked state ─────────────────────────────────────────────────────
        private static bool _yawieDefeated          = false;
        private static bool _allBossesDefeated      = false;
        private static int  _cookstaFollowers        = 0;
        private static int  _cookstaBestTaste        = 0;
        private static int  _cookstaResearchedRecipes = 0;
        private static bool _allFishComplete         = false;
        private static bool _allMarincaComplete      = false;

        private static readonly HashSet<string> _defeatedBosses = new();

        // ── Public read-only properties for ProgressUI ───────────────────────
        public static bool YawieDefeated          => _yawieDefeated;
        public static bool AllFishComplete        => _allFishComplete;
        public static int  DefeatedBossCount      => _defeatedBosses.Count;
        public static int  CookstaFollowers       => _cookstaFollowers;
        public static int  CookstaBestTaste       => _cookstaBestTaste;
        public static int  CookstaResearchedRecipes => _cookstaResearchedRecipes;

        // All 16 bosses required for goal 1 (Defeat All Bosses).
        // Names must match BossNameMapper output in BossDefeatedPatch.cs
        // (prefixed with "Boss: " as returned by GetDisplayNameFromScene).
        private static readonly HashSet<string> AllBossNames = new()
        {
            "Boss: Giant Squid",
            "Boss: Clione Queen",
            "Boss: Truck Hermit Crab",
            "Boss: Giant Wolf Eel",
            "Boss: Goblin Shark",
            "Boss: Phantom Jellyfish",
            "Boss: Giant Gadon",
            "Boss: Helicoprion",
            "Boss: Kronosaurus",
            "Boss: John Watson",
            "Boss: Ebirah",
            "Boss: Klaus",
            "Boss: Mantis Shrimp",
            "Boss: Lusca",
            "Boss: Torben",
            "Boss: Yawie",
        };

        private static bool _goalCompleted = false;

        // ── Event handlers ────────────────────────────────────────────────────

        public static void OnYawieDefeated()
        {
            _yawieDefeated = true;
            OnBossDefeated("Yawie");
            Log.LogInfo("GoalTracker: Yawie defeated!");
            CheckGoal();
        }

        public static void OnBossDefeated(string bossName)
        {
            _defeatedBosses.Add(bossName);
            _allBossesDefeated = _defeatedBosses.IsSupersetOf(AllBossNames);
            if (_allBossesDefeated)
                Log.LogInfo("GoalTracker: All bosses defeated!");
            CheckGoal();
        }

        public static void OnCookstaFollowersChanged(int followers)
        {
            _cookstaFollowers = followers;
            CheckGoal();
        }

        public static void OnBestTasteChanged(int bestTaste)
        {
            if (bestTaste > _cookstaBestTaste)
            {
                _cookstaBestTaste = bestTaste;
                CheckGoal();
            }
        }

        public static void OnResearchedRecipesChanged(int count)
        {
            if (count > _cookstaResearchedRecipes)
            {
                _cookstaResearchedRecipes = count;
                CheckGoal();
            }
        }

        // Restaurant rating no longer used for any goal — method kept for location checks
        public static void OnRestaurantRatingAchieved(int stars)
        {
            Log.LogInfo($"GoalTracker: Restaurant rating {stars} stars (not used for goal).");
        }

        public static void OnAllFishComplete()
        {
            _allFishComplete = true;
            Log.LogInfo("GoalTracker: All fish species logged!");
            CheckGoal();
        }

        public static void OnAllMarincaComplete()
        {
            _allMarincaComplete = true;
            Log.LogInfo("GoalTracker: All Marinca entries complete!");
            CheckGoal();
        }

        // ── Goal evaluation ───────────────────────────────────────────────────

        private static void CheckGoal()
        {
            if (_goalCompleted) return;
            if (!ArchipelagoClient.IsConnected) return;

            int goal = ArchipelagoClient.SlotData?.Goal ?? 0;
            bool met = IsGoalMet(goal);

            if (met)
            {
                _goalCompleted = true;
                Log.LogInfo($"GoalTracker: Goal {goal} completed! Sending completion to server.");

                // Show celebration notification
                UI.NotificationManager.ShowNotification(
                    "Goal Complete!",
                    GetGoalName(goal),
                    UI.NotificationManager.NotificationType.Goal,
                    duration: 10f
                );

                ArchipelagoClient.CompleteGoal();
            }
        }

        private static bool IsGoalMet(int goal) => goal switch
        {
            0 => _yawieDefeated,
            1 => _yawieDefeated && _allBossesDefeated,
            2 => _yawieDefeated                          // Diamond Rank
                 && _cookstaFollowers >= 720
                 && _cookstaBestTaste >= 375
                 && _cookstaResearchedRecipes >= 32,
            3 => _yawieDefeated && _allFishComplete,      // Master Diver (all fish = all Marinca)
            4 => _yawieDefeated && _allBossesDefeated    // 100% Completion
                 && _cookstaFollowers >= 720
                 && _cookstaBestTaste >= 375
                 && _cookstaResearchedRecipes >= 32
                 && _allFishComplete,
            _ => false,
        };

        private static string GetGoalName(int goal) => goal switch
        {
            0 => "Defeat Yawie",
            1 => "Defeat All Bosses",
            2 => "Diamond Rank (720 Followers + 375 Best Taste + 32 Researched Recipes)",
            3 => "Master Diver (Catch Every Fish Species)",
            4 => "100% Completion",
            _ => "Unknown Goal",
        };

        /// <summary>
        /// Reset tracker state (e.g., on new connection or new game).
        /// </summary>
        public static void Reset()
        {
            _yawieDefeated     = false;
            _allBossesDefeated = false;
            _cookstaFollowers        = 0;
            _cookstaBestTaste        = 0;
            _cookstaResearchedRecipes = 0;
            _allFishComplete   = false;
            _allMarincaComplete = false;
            _goalCompleted     = false;
            _defeatedBosses.Clear();
            Log.LogInfo("GoalTracker: Reset.");
        }
    }
}
