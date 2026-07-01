using System;
using System.Threading.Tasks;
using BepInEx.Logging;
using UnityEngine;

namespace DaveDiverAP.UI
{
    /// <summary>
    /// In-game Archipelago connection window.
    /// Uses Unity's built-in IMGUI (no native DLLs required).
    /// Toggle with F9 (TODO: requires InputLegacyModule).
    /// </summary>
    public class ConnectionUI : MonoBehaviour
    {
        // ── UI state ─────────────────────────────────────────────────────────
        private bool _isVisible    = true;
        private bool _isConnecting = false;
        private Rect _windowRect   = new Rect(20, 20, 420, 0);

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
        private Vector2 _logScrollPos = Vector2.zero;

        // ── Tabs ──────────────────────────────────────────────────────────────
        private int _activeTab = 0;

        private static ManualLogSource Log => Plugin.Log;

        public void Awake()
        {
            var (url, port, slotName, _) = SaveData.LoadConnectionInfo();
            _server   = url;
            _port     = port.ToString();
            _slotName = slotName;

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
            // TODO: Toggle UI with F9 — needs InputLegacyModule.dll
        }

        public void OnGUI()
        {
            if (!_isVisible) return;
            _windowRect = GUILayout.Window(
                id:         42424242,
                screenRect: _windowRect,
                func:       DrawWindow,
                text:       "Archipelago — Dave the Diver",
                options:    GUILayout.Width(420));
        }

        private void DrawWindow(int id)
        {
            GUILayout.Label("=== Archipelago Multiworld Connection ===");

            // ── Status bar ────────────────────────────────────────────────────
            bool connected = ArchipelagoClient.IsConnected;
            string statusIcon = _isConnecting ? "[...]" : connected ? "[OK]" : "[X]";
            GUILayout.Label($"{statusIcon} {_statusMessage}");

            GUILayout.Space(4);

            // ── Tab bar ───────────────────────────────────────────────────────
            string[] tabs = connected
                ? new[] { "Connection", $"Hints ({HintManager.ReceivedHints.Count})", "Progress" }
                : new[] { "Connection" };

            _activeTab = Mathf.Clamp(_activeTab, 0, tabs.Length - 1);
            _activeTab = GUILayout.Toolbar(_activeTab, tabs);

            GUILayout.Space(4);

            // ── Tab content ───────────────────────────────────────────────────
            if (_activeTab == 0)
                DrawConnectionTab(connected);
            else if (_activeTab == 1 && connected)
                HintUI.Draw();
            else if (_activeTab == 2 && connected)
                ProgressUI.Draw();

            // ── Footer ────────────────────────────────────────────────────────
            GUILayout.Space(4);
            GUILayout.Label("Press F9 to toggle this window");

            // Make window draggable
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void DrawConnectionTab(bool connected)
        {
            if (connected)
                DrawConnectedPanel();
            else
                DrawConnectionForm();

            GUILayout.Space(4);
            DrawItemLog();
        }

        private void DrawConnectionForm()
        {
            GUILayout.Label("Connect to Archipelago Server");
            GUILayout.Space(4);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Server:", GUILayout.Width(75));
            _server = GUILayout.TextField(_server, 256, GUILayout.Width(220));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Port:", GUILayout.Width(75));
            _port = GUILayout.TextField(_port, 6, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Slot Name:", GUILayout.Width(75));
            _slotName = GUILayout.TextField(_slotName, 64, GUILayout.Width(220));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Password:", GUILayout.Width(75));
            _password = GUILayout.PasswordField(_password, '*', 64, GUILayout.Width(220));
            GUILayout.EndHorizontal();

            GUILayout.Space(6);

            bool canConnect = !_isConnecting
                              && !string.IsNullOrWhiteSpace(_server)
                              && !string.IsNullOrWhiteSpace(_slotName)
                              && int.TryParse(_port, out _);

            GUI.enabled = canConnect;
            if (GUILayout.Button(_isConnecting ? "Connecting..." : "Connect", GUILayout.Width(120)))
                ConnectAsync();
            GUI.enabled = true;

            if (_statusIsError && !string.IsNullOrEmpty(_statusMessage))
                GUILayout.Label($"[Error] {_statusMessage}");
        }

        private void DrawConnectedPanel()
        {
            GUILayout.Label($"Game:   Dave the Diver");
            GUILayout.Label($"Slot:   {_slotName}");
            GUILayout.Label($"Server: {_server}:{_port}");

            int pending = ItemQueue.PendingCount;
            if (pending > 0)
            {
                GUILayout.Space(4);
                GUILayout.Label($"⏳ {pending} item{(pending == 1 ? "" : "s")} waiting (delivered on the boat)");
            }

            GUILayout.Space(6);
            if (GUILayout.Button("Disconnect", GUILayout.Width(120)))
                ArchipelagoClient.Disconnect();
        }

        private void DrawItemLog()
        {
            GUILayout.Label("Recent Items Received:");
            _logScrollPos = GUILayout.BeginScrollView(_logScrollPos, GUILayout.Height(120));

            if (_itemLog.Count == 0)
                GUILayout.Label("(no items received yet)", _dimStyle);
            else
                foreach (var entry in _itemLog)
                    GUILayout.Label(entry);

            GUILayout.EndScrollView();
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

            _isConnecting  = true;
            _statusIsError = false;
            _statusMessage = "Connecting...";

            SaveData.SaveConnectionInfo(_server, port, _slotName, _password);

            bool success = await ArchipelagoClient.ConnectAsync(_server, port, _slotName, _password);

            _isConnecting = false;

            if (!success)
                _statusIsError = true;
        }

        // ── Event handlers ────────────────────────────────────────────────────

        private void OnStatusChanged(string status)
        {
            _statusMessage = status;
            _statusIsError = status.StartsWith("Failed") || status.StartsWith("Error");
        }

        private void OnItemReceived(Archipelago.MultiClient.Net.Models.ItemInfo item)
        {
            AddLogEntry($"[{DateTime.Now:HH:mm:ss}] {item.ItemName}");
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
