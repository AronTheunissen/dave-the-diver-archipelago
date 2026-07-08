using System.Collections.Concurrent;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using UnityEngine;

namespace DaveDiverAP
{
    /// <summary>
    /// Buffers received AP items and applies them safely on the Unity main thread.
    ///
    /// Items can arrive from the AP server at any time (including during loading screens
    /// or cutscenes), but game API calls must happen on the Unity main thread.
    /// This queue bridges the gap.
    ///
    /// Usage:
    ///   - AP item callback enqueues items: ItemQueue.Enqueue(item)
    ///   - MonoBehaviour Update() dequeues and applies: ItemQueue.ProcessPending()
    /// </summary>
    public class ItemQueue : MonoBehaviour
    {
        public static ItemQueue? Instance { get; private set; }

        private static readonly ConcurrentQueue<ItemInfo> _queue = new();
        private static ManualLogSource Log => Plugin.Log;

        // Only process items when Dave is in a valid game state.
        // GameStatePatch sets this flag via SetGameReady().
        private static bool _isGameReady = false;

        // True once ANY valid game state has been reached (never resets to false).
        // Use this as a load guard in patches that hook SaveData methods:
        //   - IsGameReady: gates item delivery (toggles with game state)
        //   - IsGameLoaded: gates location checks (true from first valid state onward)
        private static bool _isGameLoaded = false;

        // Track how many items are waiting so the UI can show a badge
        public static int PendingCount => _queue.Count;

        /// <summary>
        /// True when Dave is in an active game state where items can be delivered.
        /// Toggles on/off as game state changes.
        /// </summary>
        public static bool IsGameReady => _isGameReady;

        /// <summary>
        /// True once the game has loaded at least once (never resets to false).
        /// Use as a load guard in location check patches to prevent false triggers
        /// during save deserialization.
        /// </summary>
        public static bool IsGameLoaded => _isGameLoaded;

        public void Awake()
        {
            Instance = this;
        }

        public void OnDestroy()
        {
            Instance = null;
        }

        public void Update()
        {
            // Process items whenever the game has loaded at least once AND we're connected.
            // IsGameLoaded is set true by the first valid game state (AfternoonStart, InBoat, etc.)
            // and never resets — so items can be delivered even after state transitions like
            // Idle, ThumbsUp, or other non-boat states that come after the initial load.
            if (!_isGameLoaded) return;
            if (!ArchipelagoClient.IsConnected) return;
            if (_queue.Count > 0)
                Log.LogInfo($"[ItemQueue] Update: {_queue.Count} items pending, IsGameLoaded={_isGameLoaded}");
            ProcessPending();
        }

        /// <summary>
        /// Enqueue an item for processing on the main thread.
        /// Safe to call from any thread.
        /// Items are processed in Update() — which runs every frame when IsGameReady.
        /// </summary>
        public static void Enqueue(ItemInfo item)
        {
            _queue.Enqueue(item);
            Log.LogInfo($"[ItemQueue] Enqueued: {item.ItemName} (queue size now {_queue.Count}, IsGameReady={_isGameReady})");
        }

        /// <summary>
        /// Process all queued items on the main thread.
        /// </summary>
        public static void ProcessPending()
        {
            while (_queue.TryDequeue(out var item))
            {
                Log.LogInfo($"Processing queued item: {item.ItemName}");
                ItemHandler.ApplyItem(item);

                // Show notification
                var sender = item.Player?.Name ?? "Archipelago";
                UI.NotificationManager.ShowNotification(
                    $"🎁 {item.ItemName}",
                    $"Sent by {sender}",
                    UI.NotificationManager.NotificationType.ItemReceived
                );
            }
        }

        /// <summary>
        /// Call this when the game is in a state where items can be safely applied.
        /// (e.g., when Dave is standing on the boat, or when the dive starts)
        /// </summary>
        public static void SetGameReady(bool ready)
        {
            _isGameReady = ready;
            if (ready)
            {
                _isGameLoaded = true;
                Log.LogInfo("Game ready — processing queued items.");
            }
        }
    }
}
