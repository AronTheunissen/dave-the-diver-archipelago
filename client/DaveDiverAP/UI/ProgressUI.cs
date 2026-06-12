using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using ImGuiNET;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// Progress tracker tab — shows how close the player is to completing their goal
    /// and tracks overall location check progress.
    /// 
    /// Displays:
    /// - Overall checks: X / Y completed (progress bar)
    /// - Goal-specific requirements (with individual progress bars)
    /// - Location category breakdown (fish, bosses, recipes, etc.)
    /// </summary>
    public static class ProgressUI
    {
        // ── Tracked counts ────────────────────────────────────────────────────
        private static int _totalChecked    = 0;
        private static int _totalLocations  = 0;

        // Per-category check counts
        private static readonly Dictionary<string, (int checked_, int total)> _categoryProgress = new();

        // Cached goal name
        private static string _goalName = "Defeat Yawie";

        // ── Colors ────────────────────────────────────────────────────────────
        private static readonly System.Numerics.Vector4 ColorDone       = new(0.2f, 0.8f, 0.2f, 1f);
        private static readonly System.Numerics.Vector4 ColorInProgress = new(0.9f, 0.7f, 0.1f, 1f);
        private static readonly System.Numerics.Vector4 ColorNotStarted = new(0.5f, 0.5f, 0.5f, 1f);
        private static readonly System.Numerics.Vector4 ColorHeader     = new(0.2f, 0.6f, 0.9f, 1f);

        private static ManualLogSource Log => Plugin.Log;

        // ── Initialize ────────────────────────────────────────────────────────

        public static void Initialize()
        {
            if (ArchipelagoClient.Session == null) return;

            // Get total location count from server
            _totalLocations = ArchipelagoClient.Session.Locations.AllLocations.Count;

            // Count already-checked locations
            _totalChecked = ArchipelagoClient.Session.Locations.AllLocationsChecked.Count;

            // Get goal name
            int goal = ArchipelagoClient.SlotData?.Goal ?? 0;
            _goalName = goal switch
            {
                0 => "Defeat Yawie",
                1 => "Defeat All Bosses",
                2 => "Diamond Rank",
                3 => "Master Diver",
                4 => "100% Completion",
                _ => "Unknown Goal",
            };

            Log.LogInfo($"ProgressUI: {_totalChecked}/{_totalLocations} locations checked.");
        }

        /// <summary>
        /// Call this whenever a location is checked to update the counter.
        /// </summary>
        public static void OnLocationChecked(string category)
        {
            _totalChecked++;

            if (!_categoryProgress.ContainsKey(category))
                _categoryProgress[category] = (0, 0);

            var (c, t) = _categoryProgress[category];
            _categoryProgress[category] = (c + 1, t);
        }

        // ── Draw ──────────────────────────────────────────────────────────────

        public static void Draw()
        {
            ImGui.Spacing();

            if (!ArchipelagoClient.IsConnected)
            {
                ImGui.TextDisabled("Connect to Archipelago to see progress.");
                return;
            }

            // ── Overall progress ──────────────────────────────────────────────
            ImGui.TextColored(ColorHeader, "Overall Progress");
            ImGui.Separator();
            ImGui.Spacing();

            float overallPct = _totalLocations > 0
                ? (float)_totalChecked / _totalLocations
                : 0f;

            DrawProgressBar($"{_totalChecked} / {_totalLocations} checks", overallPct, ColorInProgress);
            ImGui.Spacing();

            // ── Goal progress ─────────────────────────────────────────────────
            ImGui.TextColored(ColorHeader, $"Goal: {_goalName}");
            ImGui.Separator();
            ImGui.Spacing();

            int goal = ArchipelagoClient.SlotData?.Goal ?? 0;
            DrawGoalProgress(goal);

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            // ── Category breakdown ────────────────────────────────────────────
            ImGui.TextColored(ColorHeader, "Category Breakdown");
            ImGui.Separator();
            ImGui.Spacing();

            DrawCategoryBreakdown();
        }

        private static void DrawGoalProgress(int goal)
        {
            // Goal 0: Defeat Yawie
            DrawBoolCheck("Defeat Yawie (Final Boss)", GoalTracker.YawieDefeated);

            if (goal >= 1)
            {
                // Goal 1: All Bosses
                int defeatedCount = GoalTracker.DefeatedBossCount;
                float bossPct = defeatedCount / 16f;
                DrawProgressBar($"Bosses defeated: {defeatedCount} / 16", bossPct,
                    bossPct >= 1f ? ColorDone : ColorInProgress);
            }

            if (goal == 2 || goal == 4)
            {
                // Diamond Rank requirements
                ImGui.TextDisabled("Diamond Rank:");
                DrawFollowerProgress(GoalTracker.CookstaFollowers);
                DrawProgressBar($"Best Taste: {GoalTracker.CookstaBestTaste} / 375",
                    Math.Min(1f, GoalTracker.CookstaBestTaste / 375f), ColorInProgress);
                DrawProgressBar($"Researched Recipes: {GoalTracker.CookstaResearchedRecipes} / 32",
                    Math.Min(1f, GoalTracker.CookstaResearchedRecipes / 32f), ColorInProgress);
            }

            if (goal == 3 || goal == 4)
            {
                // Master Diver
                DrawBoolCheck("All Fish Species Caught", GoalTracker.AllFishComplete);
            }
        }

        private static void DrawFollowerProgress(int followers)
        {
            // Show which Cooksta rank has been reached
            var ranks = new[] { (10, "Bronze"), (20, "Silver"), (100, "Gold"),
                                 (200, "Platinum"), (720, "Diamond") };
            string currentRank = "Coal";
            foreach (var (threshold, name) in ranks)
                if (followers >= threshold) currentRank = name;

            float pct = Math.Min(1f, followers / 720f);
            DrawProgressBar($"Cooksta: {followers} / 720 followers (Rank: {currentRank})",
                pct, ColorInProgress);
        }

        private static void DrawCategoryBreakdown()
        {
            // Static category totals (from locations.py counts)
            var categories = new[]
            {
                ("Fish",          "fish",         203),
                ("Bosses",        "boss",           16),
                ("Recipes",       "recipe",         54),
                ("Dish Upgrades", "dish_upgrade",  549),
                ("Weapons",       "weapon",         79),
                ("Ecowatcher",    "ecowatcher",     68),
                ("Cooksta",       "cooksta",        12),
                ("Farming",       "farming",        13),
                ("Fish Farm",     "fish_farm",      15),
                ("Photography",   "photography",    12),
                ("Challenges",    "challenge",       9),
                ("Minigames",     "minigame",        4),
            };

            ImGui.BeginChild("CategoryList", new System.Numerics.Vector2(0, 200), false);

            foreach (var (label, cat, total) in categories)
            {
                int done = _categoryProgress.TryGetValue(cat, out var p) ? p.checked_ : 0;
                float pct = total > 0 ? (float)done / total : 0f;
                var color = pct >= 1f ? ColorDone
                          : pct >  0f ? ColorInProgress
                          :             ColorNotStarted;
                DrawProgressBar($"{label}: {done}/{total}", pct, color);
            }

            ImGui.EndChild();
        }

        private static void DrawProgressBar(string label, float fraction, System.Numerics.Vector4 color)
        {
            fraction = Math.Clamp(fraction, 0f, 1f);
            ImGui.PushStyleColor(ImGuiCol.PlotHistogram, color);
            ImGui.ProgressBar(fraction, new System.Numerics.Vector2(-1, 16), "");
            ImGui.PopStyleColor();
            ImGui.SameLine(0, 6);
            ImGui.TextUnformatted(label);
        }

        private static void DrawBoolCheck(string label, bool done)
        {
            var color = done ? ColorDone : ColorNotStarted;
            var icon  = done ? "✓" : "○";
            ImGui.TextColored(color, $"{icon} {label}");
        }
    }
}
