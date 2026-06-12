using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BepInEx;

// NOTE on game's SaveSystem:
// The game uses SaveSystem → PlayerInfoSave with ObscuredInt encryption.
// Do NOT try to directly read/write PlayerInfoSave values — use the game's
// own setter methods (PlayerInfoSave.set_bei, etc.) found via Il2CppDumper.
// Our SaveData class is SEPARATE from the game's save system — it only
// stores Archipelago state (checked locations, item index, connection info).

namespace DaveDiverAP
{
    /// <summary>
    /// Persists Archipelago state between game sessions.
    /// Stores checked locations, received item index, and connection info.
    /// Saved as JSON in BepInEx/config/DaveDiverAP/
    /// </summary>
    public static class SaveData
    {
        private static readonly string SaveDir = Path.Combine(
            Paths.ConfigPath, "DaveDiverAP");

        private static string SaveFile => Path.Combine(SaveDir, "archipelago_save.json");

        private static SaveState _state = new();

        public static void Load()
        {
            try
            {
                if (!File.Exists(SaveFile)) return;
                var json = File.ReadAllText(SaveFile);
                _state = JsonSerializer.Deserialize<SaveState>(json) ?? new SaveState();
                Plugin.Log.LogInfo($"Save data loaded: {_state.CheckedLocations.Count} locations checked.");
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Failed to load save data: {ex.Message}");
                _state = new SaveState();
            }
        }

        public static void Save()
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                var json = JsonSerializer.Serialize(_state, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SaveFile, json);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Failed to save data: {ex.Message}");
            }
        }

        public static HashSet<long> LoadCheckedLocations()
        {
            Load();
            return new HashSet<long>(_state.CheckedLocations);
        }

        public static int LoadLastItemIndex()
        {
            Load();
            return _state.LastItemIndex;
        }

        public static void AddCheckedLocation(long locationId)
        {
            _state.CheckedLocations.Add(locationId);
            Save();
        }

        public static void SetLastItemIndex(int index)
        {
            _state.LastItemIndex = index;
            Save();
        }

        public static void SaveConnectionInfo(string url, int port, string slotName, string password)
        {
            _state.LastServer   = url;
            _state.LastPort     = port;
            _state.LastSlotName = slotName;
            _state.LastPassword = password;
            Save();
        }

        public static (string url, int port, string slotName, string password) LoadConnectionInfo()
        {
            Load();
            return (_state.LastServer, _state.LastPort, _state.LastSlotName, _state.LastPassword);
        }

        public static void Reset()
        {
            _state = new SaveState
            {
                LastServer   = _state.LastServer,
                LastPort     = _state.LastPort,
                LastSlotName = _state.LastSlotName,
                LastPassword = _state.LastPassword,
            };
            Save();
        }

        private class SaveState
        {
            public HashSet<long> CheckedLocations { get; set; } = new();
            public int LastItemIndex { get; set; } = 0;
            public string LastServer   { get; set; } = "localhost";
            public int    LastPort     { get; set; } = 38281;
            public string LastSlotName { get; set; } = "Player";
            public string LastPassword { get; set; } = "";
        }
    }
}
