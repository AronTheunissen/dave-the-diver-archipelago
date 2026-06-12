using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Manages the connection to the Archipelago server and handles
    /// sending/receiving items and locations.
    /// </summary>
    public static class ArchipelagoClient
    {
        // ── Connection state ────────────────────────────────────────────────
        public static ArchipelagoSession? Session { get; private set; }
        public static bool IsConnected => Session?.Socket.Connected ?? false;
        public static SlotData? SlotData { get; private set; }

        // ── Connection config (set from in-game UI or config file) ──────────
        public static string ServerUrl { get; set; } = "localhost";
        public static int    ServerPort { get; set; } = 38281;
        public static string SlotName   { get; set; } = "Player";
        public static string Password   { get; set; } = "";

        // ── Events ──────────────────────────────────────────────────────────
        public static event Action<NetworkItem>? OnItemReceived;
        public static event Action<string>?      OnConnectionStatusChanged;
        public static event Action?              OnConnected;
        public static event Action?              OnDisconnected;

        // ── Internal state ──────────────────────────────────────────────────
        private static readonly HashSet<long> _checkedLocations = new();
        private static int _lastItemIndex = 0;
        private static ManualLogSource Log => Plugin.Log;

        public static void Initialize()
        {
            Log.LogInfo("ArchipelagoClient initialized. Waiting for connection request.");
        }

        /// <summary>
        /// Attempt to connect to the Archipelago server.
        /// Called when the player enters connection details in the UI.
        /// </summary>
        public static async Task<bool> ConnectAsync(string url, int port, string slotName, string password = "")
        {
            ServerUrl = url;
            ServerPort = port;
            SlotName = slotName;
            Password = password;

            Log.LogInfo($"Connecting to {url}:{port} as {slotName}...");
            OnConnectionStatusChanged?.Invoke("Connecting...");

            try
            {
                Session = ArchipelagoSessionFactory.CreateSession(url, port);

                // Wire up events
                Session.Items.ItemReceived += OnItemReceivedHandler;
                Session.Socket.ErrorReceived += OnErrorReceived;
                Session.Socket.SocketClosed += OnSocketClosed;

                var result = await Session.ConnectAsync();

                if (result.Successful)
                {
                    Log.LogInfo($"Connected to Archipelago! Game: {result.SlotInfo?.Game}");

                    // Parse slot data from server
                    SlotData = new SlotData(result.SlotData);

                    // Sync already-checked locations
                    SyncCheckedLocations();

                    // Replay any items we haven't received yet
                    ReplayMissedItems();

                    OnConnected?.Invoke();
                    OnConnectionStatusChanged?.Invoke("Connected");
                    return true;
                }
                else
                {
                    var errors = string.Join(", ", result.ErrorMessage);
                    Log.LogError($"Connection failed: {errors}");
                    OnConnectionStatusChanged?.Invoke($"Failed: {errors}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Connection exception: {ex.Message}");
                OnConnectionStatusChanged?.Invoke($"Error: {ex.Message}");
                return false;
            }
        }

        public static void Disconnect()
        {
            if (Session != null)
            {
                Session.Socket.DisconnectAsync();
                Session = null;
                OnDisconnected?.Invoke();
                OnConnectionStatusChanged?.Invoke("Disconnected");
                Log.LogInfo("Disconnected from Archipelago.");
            }
        }

        // ── Location checking ────────────────────────────────────────────────

        /// <summary>
        /// Send a completed location check to the Archipelago server.
        /// Call this when the player completes a tracked in-game action.
        /// </summary>
        public static void CheckLocation(long locationId)
        {
            if (!IsConnected)
            {
                Log.LogWarning($"Cannot check location {locationId}: not connected.");
                return;
            }

            if (_checkedLocations.Contains(locationId))
                return; // Already checked

            _checkedLocations.Add(locationId);
            Session!.Locations.CompleteLocationChecks(locationId);
            Log.LogInfo($"Location checked: {locationId}");
        }

        /// <summary>
        /// Check a location by name. Looks up the ID from the session's location table.
        /// </summary>
        public static void CheckLocation(string locationName)
        {
            if (!IsConnected) return;

            var locationId = Session!.Locations.GetLocationIdFromName("Dave the Diver", locationName);
            if (locationId == -1)
            {
                Log.LogWarning($"Unknown location name: {locationName}");
                return;
            }

            CheckLocation(locationId);
        }

        /// <summary>
        /// Notify the server that the goal has been completed.
        /// </summary>
        public static void CompleteGoal()
        {
            if (!IsConnected) return;

            var statusUpdate = new StatusUpdatePacket { Status = ArchipelagoClientState.ClientGoal };
            Session!.Socket.SendPacket(statusUpdate);
            Log.LogInfo("Goal completed! Sent to Archipelago server.");
        }

        // ── Item receiving ───────────────────────────────────────────────────

        private static void OnItemReceivedHandler(ReceivedItemsHelper helper)
        {
            while (helper.Any())
            {
                var item = helper.DequeueItem();

                if (item.ItemIndex <= _lastItemIndex)
                {
                    // Already processed this item in a previous session
                    continue;
                }

                _lastItemIndex = item.ItemIndex;
                Log.LogInfo($"Item received: {item.ItemName} (ID: {item.ItemId}) from {item.Player}");
                OnItemReceived?.Invoke(item);

                // Route to ItemHandler for actual game effect
                ItemHandler.ApplyItem(item);
            }
        }

        private static void ReplayMissedItems()
        {
            if (Session == null) return;

            // The Archipelago library handles replaying items automatically,
            // but we track _lastItemIndex to avoid double-applying.
            _lastItemIndex = SaveData.LoadLastItemIndex();
            Log.LogInfo($"Resuming from item index {_lastItemIndex}");
        }

        private static void SyncCheckedLocations()
        {
            if (Session == null) return;

            // Load previously checked locations from save data
            var saved = SaveData.LoadCheckedLocations();
            foreach (var id in saved)
                _checkedLocations.Add(id);

            Log.LogInfo($"Loaded {_checkedLocations.Count} previously checked locations.");
        }

        // ── Error handling ───────────────────────────────────────────────────

        private static void OnErrorReceived(Exception e, string message)
        {
            Log.LogError($"Archipelago socket error: {message} ({e?.Message})");
            OnConnectionStatusChanged?.Invoke("Connection error");
        }

        private static void OnSocketClosed(string reason)
        {
            Log.LogWarning($"Archipelago connection closed: {reason}");
            OnDisconnected?.Invoke();
            OnConnectionStatusChanged?.Invoke("Disconnected");
        }
    }
}
