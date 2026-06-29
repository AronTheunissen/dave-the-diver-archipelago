using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Manages Archipelago hints — lets players spend hint points to reveal
    /// where items are or what's at a location.
    ///
    /// Two hint modes:
    /// 1. Item hint:    "Where is my [Progressive Oxygen Tank]?" → reveals which game/location has it
    /// 2. Location hint: "What's at [First Catch: Bluefin Tuna]?" → reveals which item is there
    ///
    /// Hint cost is configured server-side (typically 10 hint points per hint).
    /// Hint points are earned by completing checks.
    /// </summary>
    public static class HintManager
    {
        private static ManualLogSource Log => Plugin.Log;

        // Cached hints received from server
        public static List<Hint> ReceivedHints { get; } = new();

        // Hint points available (received from server)
        public static int HintPoints { get; private set; } = 0;

        // Event fired when hints are updated
        public static event Action? OnHintsUpdated;

        public static void Initialize()
        {
            if (ArchipelagoClient.Session == null) return;

            // Subscribe to hint updates from server
            ArchipelagoClient.Session.DataStorage.TrackHints(OnHintsReceived);

            // Get current hint points
            RefreshHintPoints();

            Log.LogInfo("HintManager initialized.");
        }

        // ── Request hints ─────────────────────────────────────────────────────

        /// <summary>
        /// Request a hint for a specific item by name.
        /// Server responds with location + game where the item can be found.
        /// </summary>
        public static async Task<bool> RequestItemHintAsync(string itemName)
        {
            if (ArchipelagoClient.Session == null) return false;

            if (HintPoints <= 0)
            {
                Log.LogWarning("Not enough hint points!");
                UI.NotificationManager.ShowNotification(
                    "💡 No Hint Points",
                    "Complete more checks to earn hint points.",
                    UI.NotificationManager.NotificationType.Info
                );
                return false;
            }

            try
            {
                Log.LogInfo($"Requesting hint for item: {itemName}");

                // Send hint request packet
                var packet = new SayPacket { Text = $"!hint {itemName}" };
                ArchipelagoClient.Session.Socket.SendPacket(packet);

                Log.LogInfo($"Hint requested for: {itemName}");
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError($"Hint request failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Request a hint for a specific location by name.
        /// Server responds with which item is at that location.
        /// </summary>
        public static async Task<bool> RequestLocationHintAsync(string locationName)
        {
            if (ArchipelagoClient.Session == null) return false;

            if (HintPoints <= 0)
            {
                UI.NotificationManager.ShowNotification(
                    "💡 No Hint Points",
                    "Complete more checks to earn hint points.",
                    UI.NotificationManager.NotificationType.Info
                );
                return false;
            }

            try
            {
                Log.LogInfo($"Requesting hint for location: {locationName}");

                var packet = new SayPacket { Text = $"!hint_location {locationName}" };
                ArchipelagoClient.Session.Socket.SendPacket(packet);

                return true;
            }
            catch (Exception ex)
            {
                Log.LogError($"Location hint request failed: {ex.Message}");
                return false;
            }
        }

        // ── Refresh data ──────────────────────────────────────────────────────

        public static void RefreshHintPoints()
        {
            if (ArchipelagoClient.Session == null) return;

            // Hint points = checked locations / hint cost (server-defined)
            // The AP library exposes this via the DataStorage
            // TODO: Read from session data when AP library exposes it directly
            // For now we track it via PrintJSON messages from the server
        }

        // ── Callbacks ─────────────────────────────────────────────────────────

        private static void OnHintsReceived(Archipelago.MultiClient.Net.Models.Hint[] hints)
        {
            ReceivedHints.Clear();
            ReceivedHints.AddRange(hints);
            Log.LogInfo($"Hints updated: {hints.Length} total hints.");
            OnHintsUpdated?.Invoke();
        }
    }
}
