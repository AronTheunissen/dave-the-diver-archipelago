using System.Collections.Generic;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Tracks progress toward the player's chosen victory condition and
    /// sends the goal completion signal to the Archipelago server when met.
    ///
    /// Goals (matching options.py Goal values):
    ///   0 = Defeat Yawie
    ///   1 = Defeat All Bosses (Yawie + all 15 optional/story bosses)
    ///   2 = Defeat Yawie + Cooksta 10,000 followers
    ///   3 = Defeat Yawie + 5-star restaurant rating
    ///   4 = Defeat Yawie + all Ecowatcher (Complete All Fish + Complete All Marinca)
    ///   5 = Defeat Yawie + Complete MarinCa Collection
    ///   6 = 100% (Yawie + all bosses + max Cooksta + 5-star + all Ecowatcher)
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
        private static int  _restaurantStars         = 0;
        private static bool _allFishComplete         = false;
        private static bool _allMarincaComplete      = false;

        private static readonly HashSet<string> _defeatedBosses = new();

        // All 15 story + optional bosses required for goal 1
        private static readonly HashSet<string> AllBossNames = new()
        {
            "Giant Squid", "Clione Queen", "Truck Hermit Crab",
            "Giant Wolf Eel", "Goblin Shark", "Phantom Jellyfish",
            "Giant Gadon", "Helicoprion", "Kronosaurus",
            "John Watson", "Ebirah",
            "Great White Shark Klaus", "Mantis Shrimp", "Lusca", "Torben",
            "Yawie",
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

        public static void OnRestaurantRatingAchieved(int stars)
        {
            if (stars > _restaurantStars)
            {
                _restaurantStars = stars;
                Log.LogInfo($"GoalTracker: Restaurant rating {stars} stars.");
                CheckGoal();
            }
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
                NotificationManager.ShowNotification(
                    "🏆 Goal Complete!",
                    GetGoalName(goal),
                    NotificationManager.NotificationType.Goal,
                    duration: 10f
                );

                ArchipelagoClient.CompleteGoal();
            }
        }

        private static bool IsGoalMet(int goal) => goal switch
        {
            0 => _yawieDefeated,
            1 => _yawieDefeated && _allBossesDefeated,
            2 => _yawieDefeated && _cookstaFollowers >= 10000,
            3 => _yawieDefeated && _restaurantStars >= 5,
            4 => _yawieDefeated && _allFishComplete && _allMarincaComplete,
            5 => _yawieDefeated && _allMarincaComplete,
            6 => _yawieDefeated && _allBossesDefeated
                 && _cookstaFollowers >= 10000
                 && _restaurantStars >= 5
                 && _allFishComplete && _allMarincaComplete,
            _ => false,
        };

        private static string GetGoalName(int goal) => goal switch
        {
            0 => "Defeat Yawie",
            1 => "Defeat All Bosses",
            2 => "Defeat Yawie + 10,000 Cooksta Followers",
            3 => "Defeat Yawie + 5-Star Restaurant",
            4 => "Master Diver",
            5 => "Complete MarinCa Collection",
            6 => "100% Completion",
            _ => "Unknown Goal",
        };

        /// <summary>
        /// Reset tracker state (e.g., on new connection or new game).
        /// </summary>
        public static void Reset()
        {
            _yawieDefeated     = false;
            _allBossesDefeated = false;
            _cookstaFollowers  = 0;
            _restaurantStars   = 0;
            _allFishComplete   = false;
            _allMarincaComplete = false;
            _goalCompleted     = false;
            _defeatedBosses.Clear();
            Log.LogInfo("GoalTracker: Reset.");
        }
    }
}
