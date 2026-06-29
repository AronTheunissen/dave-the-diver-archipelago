using System;
using System.Threading.Tasks;
using BepInEx.Logging;
using ImGuiNET;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// In-game Archipelago connection window.
    /// Toggle with F9 key. Uses ImGui for rendering.
    ///
    /// Shows:
    /// - Connection form (server, port, slot name, password)
    /// - Connection status indicator
    /// - Item receive log (last 10 items)
    /// - Location check counter
    /// - Disconnect button
    /// </summary>
    public class ConnectionUI : MonoBehaviour
    {
        // ── UI state ─────────────────────────────────────────────────────────
        private bool _isVisible = false;
        private bool _isConnecting = false;

        // ── Form fields ───────────────────────────────────────────────────────
        private string _server   = "localhost";
        private string _port     = "38281";
        private string _slotName = "Player";
        private string _password = "";

        // ── Status display ────────────────────────────────────────────────────
        private string _statusMessage = "Not connected";
        private bool   _statusIsError = false;

        // ── Item log (last 10 received items) ─────────────────────────────────
        private readonly System.Collections.Generic.Queue<string> _itemLog = new();
        private const int MaxLogEntries = 10;

        // ── Colors ────────────────────────────────────────────────────────────
        private static readonly System.Numerics.Vector4 ColorConnected    = new(0.2f, 0.8f, 0.2f, 1f);
        private static readonly System.Numerics.Vector4 ColorDisconnected = new(0.8f, 0.2f, 0.2f, 1f);
        private static readonly System.Numerics.Vector4 ColorConnecting   = new(0.9f, 0.7f, 0.1f, 1f);
        private static readonly System.Numerics.Vector4 ColorHeader       = new(0.2f, 0.6f, 0.9f, 1f);

        private static ManualLogSource Log => Plugin.Log;

        public void Awake()
        {
            // Load last connection info from save data
            var (url, port, slotName, _) = SaveData.LoadConnectionInfo();
            _server   = url;
            _port     = port.ToString();
            _slotName = slotName;

            // Wire up AP client events
            ArchipelagoClient.OnConnectionStatusChanged += OnStatusChanged;
            ArchipelagoClient.OnItemReceived            += OnItemReceived;
            ArchipelagoClient.OnConnected               += OnConnectedHandler;
            ArchipelagoClient.OnDisconnected            += OnDisconnectedHandler;
        }

        public void OnDestroy()
        {
            ArchipelagoClient.OnConnectionStatusChanged -= OnStatusChanged;
            ArchipelagoClient.OnItemReceived            -= OnItemReceived;
            ArchipelagoClient.OnConnected               -= OnConnectedHandler;
            ArchipelagoClient.OnDisconnected            -= OnDisconnectedHandler;
        }

        public void Update()
        {
            // Toggle UI with F9
            if (UnityEngine.Input.GetKeyDown(KeyCode.F9))
                _isVisible = !_isVisible;
        }

        public void OnGUI()
        {
            if (!_isVisible) return;
            DrawUI();
        }

        // Active tab index
        private int _activeTab = 0;

        private void DrawUI()
        {
            // ── Window setup ──────────────────────────────────────────────────
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(440, 0), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(20, 20), ImGuiCond.FirstUseEver);

            if (!ImGui.Begin("Archipelago - Dave the Diver",
                ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.End();
                return;
            }

            // ── Header ─────────────────────────────────────────────────────────
            ImGui.TextColored(ColorHeader, "Archipelago Multiworld Connection");
            ImGui.Separator();
            ImGui.Spacing();

            // ── Connection status indicator ────────────────────────────────────
            DrawStatusIndicator();
            ImGui.Spacing();
            ImGui.Separator();

            // ── Tab bar ───────────────────────────────────────────────────────
            if (ImGui.BeginTabBar("APTabs"))
            {
                // Connection tab
                if (ImGui.BeginTabItem("Connection"))
                {
                    ImGui.Spacing();
                    if (ArchipelagoClient.IsConnected)
                        DrawConnectedPanel();
                    else
                        DrawConnectionForm();

                    ImGui.Spacing();
                    ImGui.Separator();
                    DrawItemLog();
                    ImGui.EndTabItem();
                }

                // Hints tab (only when connected)
                if (ArchipelagoClient.IsConnected)
                {
                    // Show badge on tab if we have hints
                    int hintCount = HintManager.ReceivedHints.Count;
                    var hintLabel = hintCount > 0 ? $"Hints ({hintCount})" : "Hints";

                    if (ImGui.BeginTabItem(hintLabel))
                    {
                        HintUI.Draw();
                        ImGui.EndTabItem();
                    }

                    // Progress tab
                    if (ImGui.BeginTabItem("Progress"))
                    {
                        ProgressUI.Draw();
                        ImGui.EndTabItem();
                    }
                }

                ImGui.EndTabBar();
            }

            // ── Footer ────────────────────────────────────────────────────────
            ImGui.Spacing();
            ImGui.TextDisabled("Press F9 to toggle this window");

            ImGui.End();
        }

        private void DrawStatusIndicator()
        {
            bool connected = ArchipelagoClient.IsConnected;
            var color = _isConnecting ? ColorConnecting
                      : connected     ? ColorConnected
                      :                 ColorDisconnected;

            var icon   = _isConnecting ? "⏳" : connected ? "✓" : "✗";
            var label  = _isConnecting ? "Connecting..." : _statusMessage;

            ImGui.TextColored(color, $"{icon} {label}");
        }

        private void DrawConnectionForm()
        {
            ImGui.Text("Connect to Archipelago Server");
            ImGui.Spacing();

            // Server address
            ImGui.Text("Server:");
            ImGui.SameLine(80);
            ImGui.SetNextItemWidth(200);
            ImGui.InputText("##server", ref _server, 256);

            // Port
            ImGui.Text("Port:");
            ImGui.SameLine(80);
            ImGui.SetNextItemWidth(80);
            ImGui.InputText("##port", ref _port, 6);

            // Slot name
            ImGui.Text("Slot Name:");
            ImGui.SameLine(80);
            ImGui.SetNextItemWidth(200);
            ImGui.InputText("##slot", ref _slotName, 64);

            // Password (masked)
            ImGui.Text("Password:");
            ImGui.SameLine(80);
            ImGui.SetNextItemWidth(200);
            ImGui.InputText("##password", ref _password, 64, ImGuiInputTextFlags.Password);

            ImGui.Spacing();

            // Connect button
            bool canConnect = !_isConnecting && !string.IsNullOrWhiteSpace(_server)
                              && !string.IsNullOrWhiteSpace(_slotName)
                              && int.TryParse(_port, out _);

            if (!canConnect) ImGui.BeginDisabled();

            if (ImGui.Button("Connect", new System.Numerics.Vector2(120, 0)))
                ConnectAsync();

            if (!canConnect) ImGui.EndDisabled();

            // Show error message if any
            if (_statusIsError && !string.IsNullOrEmpty(_statusMessage))
            {
                ImGui.SameLine();
                ImGui.TextColored(ColorDisconnected, _statusMessage);
            }
        }

        private void DrawConnectedPanel()
        {
            // Slot info
            ImGui.Text($"Game:   Dave the Diver");
            ImGui.Text($"Slot:   {_slotName}");
            ImGui.Text($"Server: {_server}:{_port}");
            ImGui.Spacing();

            // Pending items indicator
            int pending = ItemQueue.PendingCount;
            if (pending > 0)
            {
                ImGui.TextColored(new System.Numerics.Vector4(1f, 0.85f, 0.1f, 1f),
                    $"⏳ {pending} item{(pending == 1 ? "" : "s")} waiting");
                ImGui.TextDisabled("  (items are delivered on the boat)");
                ImGui.Spacing();
            }

            // Disconnect button
            if (ImGui.Button("Disconnect", new System.Numerics.Vector2(120, 0)))
            {
                ArchipelagoClient.Disconnect();
            }
        }

        private void DrawItemLog()
        {
            ImGui.Text("Recent Items Received:");
            ImGui.BeginChild("ItemLog", new System.Numerics.Vector2(0, 120));

            if (_itemLog.Count == 0)
            {
                ImGui.TextDisabled("(no items received yet)");
            }
            else
            {
                foreach (var entry in _itemLog)
                    ImGui.TextUnformatted(entry);
            }

            // Auto-scroll to bottom
            if (ImGui.GetScrollY() >= ImGui.GetScrollMaxY())
                ImGui.SetScrollHereY(1.0f);

            ImGui.EndChild();
        }

        // ── Connection logic ──────────────────────────────────────────────────

        private async void ConnectAsync()
        {
            if (!int.TryParse(_port, out int port))
            {
                _statusMessage = "Invalid port number";
                _statusIsError = true;
                return;
            }

            _isConnecting = true;
            _statusIsError = false;
            _statusMessage = "Connecting...";

            // Save connection info for next session
            SaveData.SaveConnectionInfo(_server, port, _slotName, _password);

            bool success = await ArchipelagoClient.ConnectAsync(_server, port, _slotName, _password);

            _isConnecting = false;

            if (!success)
            {
                _statusIsError = true;
                // Status message is set by ArchipelagoClient via OnStatusChanged event
            }
        }

        // ── Event handlers ────────────────────────────────────────────────────

        private void OnStatusChanged(string status)
        {
            _statusMessage = status;
            _statusIsError = status.StartsWith("Failed") || status.StartsWith("Error");
        }

        private void OnItemReceived(Archipelago.MultiClient.Net.Models.ItemInfo item)
        {
            var entry = $"[{DateTime.Now:HH:mm:ss}] {item.ItemName}";
            _itemLog.Enqueue(entry);
            while (_itemLog.Count > MaxLogEntries)
                _itemLog.Dequeue();
        }

        private void OnConnectedHandler()
        {
            _statusMessage = $"Connected as {_slotName}";
            _statusIsError = false;
            AddLogEntry("✓ Connected to Archipelago!");
        }

        private void OnDisconnectedHandler()
        {
            _statusMessage = "Disconnected";
            _statusIsError = false;
        }

        private void AddLogEntry(string message)
        {
            _itemLog.Enqueue(message);
            while (_itemLog.Count > MaxLogEntries)
                _itemLog.Dequeue();
        }
    }
}
