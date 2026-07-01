using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// Hint system UI panel — shown as a tab inside the connection window.
    /// Uses Unity built-in IMGUI (no native DLLs required).
    /// </summary>
    public static class HintUI
    {
        private enum HintMode { Item, Location }

        private static string   _searchText    = "";
        private static HintMode _hintMode      = HintMode.Item;
        private static bool     _showFoundOnly = false;
        private static bool     _showUnfoundOnly = false;
        private static bool     _isRequesting  = false;
        private static Vector2  _scrollPos     = Vector2.zero;

        private static ManualLogSource Log => Plugin.Log;

        public static void Draw()
        {
            GUILayout.Space(4);

            // ── Hint points ───────────────────────────────────────────────────
            GUILayout.Label($"Hint Points: {HintManager.HintPoints}  (earn by completing checks)");

            GUILayout.Space(4);
            GUILayout.Label("Request a hint:");

            // Mode toggle
            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(_hintMode == HintMode.Item, "Item hint", GUI.skin.button, GUILayout.Width(100)))
                _hintMode = HintMode.Item;
            if (GUILayout.Toggle(_hintMode == HintMode.Location, "Location hint", GUI.skin.button, GUILayout.Width(120)))
                _hintMode = HintMode.Location;
            GUILayout.EndHorizontal();

            GUILayout.Space(4);

            // Search box + button
            GUILayout.BeginHorizontal();
            string placeholder = _hintMode == HintMode.Item
                ? "Item name..."
                : "Location name...";
            _searchText = GUILayout.TextField(_searchText.Length == 0 ? placeholder : _searchText, 256, GUILayout.Width(260));
            if (_searchText == placeholder) _searchText = "";

            bool canHint = !_isRequesting
                && ArchipelagoClient.IsConnected
                && !string.IsNullOrWhiteSpace(_searchText)
                && HintManager.HintPoints > 0;

            GUI.enabled = canHint;
            if (GUILayout.Button(_isRequesting ? "..." : "Hint!", GUILayout.Width(60)))
                RequestHintAsync();
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            if (!ArchipelagoClient.IsConnected)
                GUILayout.Label("Not connected to Archipelago.");
            else if (HintManager.HintPoints <= 0)
                GUILayout.Label("No hint points available.");

            GUILayout.Space(4);

            // ── Hint list ─────────────────────────────────────────────────────
            DrawHintList();
        }

        private static void DrawHintList()
        {
            var hints = HintManager.ReceivedHints;

            if (hints.Count == 0)
            {
                GUILayout.Label("No hints yet. Request one above!");
                return;
            }

            // Filter controls
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Hints ({hints.Count}):", GUILayout.Width(80));
            _showFoundOnly   = GUILayout.Toggle(_showFoundOnly,   "Found only",   GUILayout.Width(90));
            _showUnfoundOnly = GUILayout.Toggle(_showUnfoundOnly, "Unfound only", GUILayout.Width(90));
            GUILayout.EndHorizontal();

            GUILayout.Space(4);

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(200));

            foreach (var hint in hints)
            {
                bool found = hint.Found;
                if (_showFoundOnly   && !found) continue;
                if (_showUnfoundOnly &&  found) continue;

                var session = ArchipelagoClient.Session;
                string itemName = session?.Items.GetItemName(hint.ItemId) ?? $"Item {hint.ItemId}";
                string locationName = session?.Locations.GetLocationNameFromId(hint.LocationId) ?? $"Location {hint.LocationId}";
                string receivingPlayer = session?.Players.GetPlayerName(hint.ReceivingPlayer) ?? $"Player {hint.ReceivingPlayer}";
                string findingGame = session?.Players.GetPlayerInfo(hint.FindingPlayer)?.Game ?? "Unknown";

                string status = found ? "[✓]" : "[○]";
                GUILayout.Label($"{status} {itemName}  →  {locationName} ({findingGame})  for: {receivingPlayer}");
            }

            GUILayout.EndScrollView();
        }

        private static async void RequestHintAsync()
        {
            if (string.IsNullOrWhiteSpace(_searchText)) return;

            _isRequesting = true;
            try
            {
                bool success = _hintMode == HintMode.Item
                    ? await HintManager.RequestItemHintAsync(_searchText)
                    : await HintManager.RequestLocationHintAsync(_searchText);

                if (success)
                {
                    _searchText = "";
                    NotificationManager.ShowNotification("Hint Requested", "Check the hint list for results.",
                        NotificationManager.NotificationType.Info);
                }
            }
            finally
            {
                _isRequesting = false;
            }
        }
    }
}
