using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using ImGuiNET;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// Hint system UI panel — shown as a tab inside the connection window.
    ///
    /// Features:
    /// - Search box to find items or locations to hint
    /// - Show all received hints with item name, location, game, and found status
    /// - Request new hints (costs hint points)
    /// - Toggle between "hint for item" and "hint for location" modes
    /// - Filter hints by found/unfound
    /// </summary>
    public static class HintUI
    {
        private enum HintMode { Item, Location }

        private static string _searchText   = "";
        private static HintMode _hintMode   = HintMode.Item;
        private static bool _showFoundOnly  = false;
        private static bool _showUnfoundOnly = false;
        private static bool _isRequesting   = false;

        private static readonly System.Numerics.Vector4 ColorFound   = new(0.4f, 0.8f, 0.4f, 1f);
        private static readonly System.Numerics.Vector4 ColorUnfound = new(0.9f, 0.6f, 0.2f, 1f);
        private static readonly System.Numerics.Vector4 ColorMine    = new(0.4f, 0.7f, 1.0f, 1f);
        private static readonly System.Numerics.Vector4 ColorOther   = new(0.8f, 0.8f, 0.8f, 1f);
        private static ManualLogSource Log => Plugin.Log;

        public static void Draw()
        {
            ImGui.Spacing();

            // ── Hint point display ────────────────────────────────────────────
            ImGui.TextColored(new System.Numerics.Vector4(1f, 0.9f, 0.3f, 1f),
                $"💡 Hint Points: {HintManager.HintPoints}");
            ImGui.SameLine();
            ImGui.TextDisabled("(earn by completing checks)");
            ImGui.Separator();
            ImGui.Spacing();

            // ── Request new hint ──────────────────────────────────────────────
            ImGui.Text("Request a hint:");
            ImGui.Spacing();

            // Mode toggle
            bool itemMode = _hintMode == HintMode.Item;
            if (ImGui.RadioButton("Item hint", itemMode))
                _hintMode = HintMode.Item;
            ImGui.SameLine();
            if (ImGui.RadioButton("Location hint", !itemMode))
                _hintMode = HintMode.Location;

            ImGui.Spacing();

            // Search box
            var placeholder = _hintMode == HintMode.Item
                ? "Item name (e.g. Progressive Oxygen Tank)"
                : "Location name (e.g. First Catch: Bluefin Tuna)";
            ImGui.SetNextItemWidth(280);
            ImGui.InputTextWithHint("##hintsearch", placeholder, ref _searchText, 256);

            ImGui.SameLine();

            bool canHint = !_isRequesting
                && ArchipelagoClient.IsConnected
                && !string.IsNullOrWhiteSpace(_searchText)
                && HintManager.HintPoints > 0;

            if (!canHint) ImGui.BeginDisabled();

            if (ImGui.Button(_isRequesting ? "Requesting..." : "Hint!", new System.Numerics.Vector2(80, 0)))
                RequestHintAsync();

            if (!canHint) ImGui.EndDisabled();

            if (!ArchipelagoClient.IsConnected)
            {
                ImGui.TextColored(new System.Numerics.Vector4(0.8f, 0.3f, 0.3f, 1f),
                    "Not connected to Archipelago.");
            }
            else if (HintManager.HintPoints <= 0)
            {
                ImGui.TextColored(new System.Numerics.Vector4(0.8f, 0.6f, 0.2f, 1f),
                    "No hint points available.");
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            // ── Hint list ─────────────────────────────────────────────────────
            DrawHintList();
        }

        private static void DrawHintList()
        {
            var hints = HintManager.ReceivedHints;

            if (hints.Count == 0)
            {
                ImGui.TextDisabled("No hints yet. Request one above!");
                return;
            }

            // Filter controls
            ImGui.Text($"Hints ({hints.Count} total):");
            ImGui.SameLine(200);
            ImGui.Checkbox("Found only", ref _showFoundOnly);
            ImGui.SameLine();
            ImGui.Checkbox("Unfound only", ref _showUnfoundOnly);

            ImGui.Spacing();

            // Scrollable hint list
            ImGui.BeginChild("HintList", new System.Numerics.Vector2(0, 200), true);

            foreach (var hint in hints)
            {
                bool found = hint.Found;

                // Apply filters
                if (_showFoundOnly && !found) continue;
                if (_showUnfoundOnly && found) continue;

                // Determine if this is our item or someone else's
                bool isMyItem = hint.ReceivingPlayer.Name == ArchipelagoClient.SlotName;

                // Status icon
                var statusIcon = found ? "✓" : "○";
                var statusColor = found ? ColorFound : ColorUnfound;
                ImGui.TextColored(statusColor, statusIcon);
                ImGui.SameLine();

                // Item name (colored by owner)
                var itemColor = isMyItem ? ColorMine : ColorOther;
                ImGui.TextColored(itemColor, hint.ItemName ?? "Unknown Item");
                ImGui.SameLine();

                ImGui.TextDisabled("→");
                ImGui.SameLine();

                // Location info
                var locationText = $"{hint.LocationName} ({hint.FindingPlayer.Game})";
                ImGui.Text(locationText);

                // Tooltip with full details
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text($"Item:     {hint.ItemName}");
                    ImGui.Text($"For:      {hint.ReceivingPlayer.Name}");
                    ImGui.Text($"At:       {hint.LocationName}");
                    ImGui.Text($"In game:  {hint.FindingPlayer.Game}");
                    ImGui.Text($"Found:    {(found ? "Yes" : "No")}");
                    ImGui.EndTooltip();
                }
            }

            ImGui.EndChild();
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
                    _searchText = ""; // Clear after successful request
                    NotificationManager.ShowNotification(
                        "💡 Hint Requested",
                        "Check the hint list for results.",
                        NotificationManager.NotificationType.Info
                    );
                }
            }
            finally
            {
                _isRequesting = false;
            }
        }
    }
}
