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

        // Only process items when Dave is standing on the boat.
        // NOT while diving, in the restaurant, on farms, or in loading screens.
        // GameStatePatch sets this flag via SetGameReady().
        // Patches that hook SaveData methods can use IsGameReady as a load guard.
        private static bool _isGameReady = false;

        // Track how many items are waiting so the UI can show a badge
        public static int PendingCount => _queue.Count;

        /// <summary>
        /// True when Dave is in an active game state (on the boat, diving, etc.).
        /// False during loading screens and save deserialization.
        /// Use this as a guard in patches that hook SaveData methods to prevent
        /// firing during save loading.
        /// </summary>
        public static bool IsGameReady => _isGameReady;

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
            if (!_isGameReady) return;
            ProcessPending();
        }

        /// <summary>
        /// Enqueue an item for processing on the main thread.
        /// Safe to call from any thread.
        /// </summary>
        public static void Enqueue(ItemInfo item)
        {
            _queue.Enqueue(item);
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
                Log.LogInfo("Game ready — processing queued items.");
        }
    }
}
