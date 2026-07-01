using System;
using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// Displays toast-style notifications in the top-right corner.
    /// Uses Unity built-in IMGUI (no native DLLs required).
    /// Auto-dismisses after a configurable duration.
    /// </summary>
    public class NotificationManager : MonoBehaviour
    {
        public enum NotificationType { ItemReceived, DeathLink, Connection, Goal, Info }

        private class Notification
        {
            public string Title   { get; init; } = "";
            public string Message { get; init; } = "";
            public NotificationType Type { get; init; }
            public float ExpiresAt { get; set; }
            public float Alpha { get; set; } = 1f;
        }

        public static NotificationManager? Instance { get; private set; }

        private readonly Queue<Notification> _pending = new();
        private readonly List<Notification>  _active  = new();
        private const int   MaxActive       = 5;
        private const float DefaultDuration = 5f;
        private const float FadeDuration    = 0.5f;

        private static ManualLogSource Log => Plugin.Log;

        public void Awake()  { Instance = this; }
        public void OnDestroy() { Instance = null; }

        public void Update()
        {
            float now = Time.time;
            while (_pending.Count > 0 && _active.Count < MaxActive)
                _active.Add(_pending.Dequeue());

            _active.RemoveAll(n =>
            {
                float remaining = n.ExpiresAt - now;
                if (remaining <= 0) return true;
                if (remaining < FadeDuration)
                    n.Alpha = remaining / FadeDuration;
                return false;
            });
        }

        public void OnGUI()
        {
            if (_active.Count == 0) return;

            float padding     = 10f;
            float windowWidth = 300f;
            float x = Screen.width - windowWidth - padding;
            float y = padding;

            foreach (var n in _active)
            {
                // Use a semi-transparent colored box
                Color bg = GetBackgroundColor(n.Type);
                bg.a = n.Alpha * 0.9f;

                // Calculate content height
                string content = string.IsNullOrEmpty(n.Message)
                    ? $"{GetIcon(n.Type)} {n.Title}"
                    : $"{GetIcon(n.Type)} {n.Title}\n{n.Message}";

                // Draw background box
                var oldColor = GUI.backgroundColor;
                GUI.backgroundColor = bg;
                GUI.Box(new Rect(x, y, windowWidth, string.IsNullOrEmpty(n.Message) ? 36 : 56), "");
                GUI.backgroundColor = oldColor;

                // Draw text
                var oldContent = GUI.color;
                GUI.color = new Color(1, 1, 1, n.Alpha);
                GUI.Label(new Rect(x + 8, y + 8, windowWidth - 16, 48), content);
                GUI.color = oldContent;

                y += (string.IsNullOrEmpty(n.Message) ? 36 : 56) + padding;
            }
        }

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

        private static string GetIcon(NotificationType type) => type switch
        {
            NotificationType.ItemReceived => "[Item]",
            NotificationType.DeathLink    => "[Death]",
            NotificationType.Connection   => "[AP]",
            NotificationType.Goal         => "[Goal]",
            _                             => "[Info]",
        };

        private static Color GetBackgroundColor(NotificationType type) => type switch
        {
            NotificationType.ItemReceived => new Color(0.1f, 0.3f, 0.5f),
            NotificationType.DeathLink    => new Color(0.5f, 0.1f, 0.1f),
            NotificationType.Connection   => new Color(0.1f, 0.4f, 0.1f),
            NotificationType.Goal         => new Color(0.5f, 0.4f, 0.0f),
            _                             => new Color(0.2f, 0.2f, 0.2f),
        };
    }
}
