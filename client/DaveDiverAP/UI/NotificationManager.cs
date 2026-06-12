using System;
using System.Collections.Generic;
using BepInEx.Logging;
using ImGuiNET;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// Displays non-intrusive toast-style notifications in the top-right corner.
    /// Used for:
    /// - Item received (from Archipelago multiworld)
    /// - Death Link received
    /// - Connection status changes
    /// - Goal completion
    ///
    /// Notifications auto-dismiss after a configurable duration.
    /// </summary>
    public class NotificationManager : MonoBehaviour
    {
        public enum NotificationType
        {
            ItemReceived,
            DeathLink,
            Connection,
            Goal,
            Info,
        }

        private class Notification
        {
            public string Title   { get; init; } = "";
            public string Message { get; init; } = "";
            public NotificationType Type { get; init; }
            public float ExpiresAt { get; set; }
            public float Alpha { get; set; } = 1f;
        }

        // ── Singleton ─────────────────────────────────────────────────────────
        public static NotificationManager? Instance { get; private set; }

        private readonly Queue<Notification> _pending  = new();
        private readonly List<Notification>  _active   = new();
        private const int   MaxActive       = 5;
        private const float DefaultDuration = 5f;   // seconds
        private const float FadeDuration    = 0.5f; // seconds for fade-out

        private static ManualLogSource Log => Plugin.Log;

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
            float now = Time.time;

            // Promote pending to active
            while (_pending.Count > 0 && _active.Count < MaxActive)
                _active.Add(_pending.Dequeue());

            // Fade out and remove expired notifications
            _active.RemoveAll(n =>
            {
                float remaining = n.ExpiresAt - now;
                if (remaining <= 0)     return true;  // fully expired
                if (remaining < FadeDuration)
                    n.Alpha = remaining / FadeDuration;
                return false;
            });
        }

        public void OnGUI()
        {
            if (_active.Count == 0) return;
            ImGuiUn.Layout("Notifications", DrawNotifications);
        }

        private void DrawNotifications()
        {
            float screenWidth  = Screen.width;
            float windowWidth  = 320f;
            float padding      = 10f;
            float windowHeight = 0f; // auto

            float y = padding;
            foreach (var n in _active)
            {
                ImGui.SetNextWindowPos(
                    new System.Numerics.Vector2(screenWidth - windowWidth - padding, y),
                    ImGuiCond.Always);
                ImGui.SetNextWindowSize(
                    new System.Numerics.Vector2(windowWidth, windowHeight),
                    ImGuiCond.Always);

                var bgColor = GetBackgroundColor(n.Type, n.Alpha);
                ImGui.PushStyleColor(ImGuiCol.WindowBg, bgColor);
                ImGui.PushStyleColor(ImGuiCol.Border,   new System.Numerics.Vector4(1,1,1, n.Alpha * 0.3f));

                ImGui.Begin($"##notif_{n.GetHashCode()}",
                    ImGuiWindowFlags.NoDecoration |
                    ImGuiWindowFlags.NoInputs |
                    ImGuiWindowFlags.NoNav |
                    ImGuiWindowFlags.NoMove |
                    ImGuiWindowFlags.AlwaysAutoResize |
                    ImGuiWindowFlags.NoSavedSettings);

                // Icon + title
                var icon = GetIcon(n.Type);
                ImGui.TextColored(new System.Numerics.Vector4(1, 1, 1, n.Alpha), $"{icon} {n.Title}");

                if (!string.IsNullOrEmpty(n.Message))
                {
                    ImGui.TextWrapped(n.Message);
                }

                // Update y for next notification
                y += ImGui.GetWindowHeight() + padding;

                ImGui.End();
                ImGui.PopStyleColor(2);
            }
        }

        // ── Public API ────────────────────────────────────────────────────────

        public static void ShowNotification(string title, string message,
            NotificationType type = NotificationType.Info,
            float duration = DefaultDuration)
        {
            if (Instance == null)
            {
                Log.LogWarning($"NotificationManager not initialized. Notification: {title} - {message}");
                return;
            }

            Instance._pending.Enqueue(new Notification
            {
                Title     = title,
                Message   = message,
                Type      = type,
                ExpiresAt = Time.time + duration,
                Alpha     = 1f,
            });
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static string GetIcon(NotificationType type) => type switch
        {
            NotificationType.ItemReceived => "🎁",
            NotificationType.DeathLink    => "💀",
            NotificationType.Connection   => "🔗",
            NotificationType.Goal         => "🏆",
            _                             => "ℹ️",
        };

        private static System.Numerics.Vector4 GetBackgroundColor(NotificationType type, float alpha) => type switch
        {
            NotificationType.ItemReceived => new(0.1f, 0.3f, 0.5f, alpha * 0.9f),
            NotificationType.DeathLink    => new(0.5f, 0.1f, 0.1f, alpha * 0.9f),
            NotificationType.Connection   => new(0.1f, 0.4f, 0.1f, alpha * 0.9f),
            NotificationType.Goal         => new(0.5f, 0.4f, 0.0f, alpha * 0.9f),
            _                             => new(0.2f, 0.2f, 0.2f, alpha * 0.9f),
        };
    }
}
