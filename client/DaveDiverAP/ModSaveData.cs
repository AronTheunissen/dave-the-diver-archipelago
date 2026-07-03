using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BepInEx;

// NOTE on game's SaveSystem:
// The game uses SaveSystem → PlayerInfoSave with ObscuredInt encryption.
// Do NOT try to directly read/write PlayerInfoSave values — use the game's
// own setter methods (PlayerInfoSave.set_bei, etc.) found via Il2CppDumper.
// Our ModSaveData class is SEPARATE from the game's save system — it only
// stores Archipelago state (checked locations, item index, connection info).

namespace DaveDiverAP
{
    /// <summary>
    /// Persists Archipelago state between game sessions.
    /// Stores checked locations, received item index, and connection info.
    /// Saved as JSON in BepInEx/config/DaveDiverAP/
    /// </summary>
    public static class ModSaveData
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

            // Progressive item counts — how many copies of each progressive item received
            public int OxygenTankLevel   { get; set; } = 0;
            public int HarpoonLevel      { get; set; } = 0;
            public int DivingSuitLevel   { get; set; } = 0;
            public int CookstaRank       { get; set; } = 0;  // 0=Coal, 1=Bronze … 5=Diamond
            public int CargoBoxLevel     { get; set; } = 0;
            public int TechSuitParts     { get; set; } = 0;  // 0–3
            public int ControlRoomButtons{ get; set; } = 0;  // 0–3
            public int VortexEntries     { get; set; } = 0;

            // Boolean key item flags
            public bool HasSeaPeopleGloves  { get; set; } = false;
            public bool HasTranslator       { get; set; } = false;
            public bool HasKeyToTenzhin     { get; set; } = false;
            public bool HasLaserDevice      { get; set; } = false;
            public bool HasSeaPeopleTrust   { get; set; } = false;
            public bool HasTeleportMirror   { get; set; } = false;
            public bool HasTeleportSPV      { get; set; } = false;
            public bool HasTeleportGlacier  { get; set; } = false;
            public bool HasTeleportDeep     { get; set; } = false;
            public bool HasFishFarm         { get; set; } = false;
            public bool HasVegetableFarm    { get; set; } = false;
            public bool HasChickenFarm      { get; set; } = false;
            public bool HasBugNet           { get; set; } = false;
            public bool HasNightDive        { get; set; } = false;
            public bool HasiDiverApp        { get; set; } = false;
            public bool HasOxygenGrace      { get; set; } = false;  // Sea People Bracelet

            // Chapters completed (bitmask: bit 0 = ch1, bit 6 = ch7)
            public int CompletedChapters { get; set; } = 0;

            // Dish research levels — maps dish name → current research level received
            public Dictionary<string, int> DishResearchLevels { get; set; } = new();

            // Unlocked recipes (recipe name set)
            public HashSet<string> UnlockedRecipes { get; set; } = new();

            // Unlocked weapons (weapon name set)
            public HashSet<string> UnlockedWeapons { get; set; } = new();

            // Acquired charms (charm name set)
            public HashSet<string> AcquiredCharms { get; set; } = new();

            // Found ingredients (ingredient name set) — persists first-find dedup across restarts
            public HashSet<string> FoundIngredients { get; set; } = new();
        }

        // ── Accessors for progressive/flag state ────────────────────────────

        public static int GetOxygenTankLevel()    => _state.OxygenTankLevel;
        public static int IncrementOxygenTank()   => ++_state.OxygenTankLevel;
        public static int GetHarpoonLevel()       => _state.HarpoonLevel;
        public static int IncrementHarpoon()      => ++_state.HarpoonLevel;
        public static int GetDivingSuitLevel()    => _state.DivingSuitLevel;
        public static int IncrementDivingSuit()   => ++_state.DivingSuitLevel;
        public static int GetCookstaRank()        => _state.CookstaRank;
        public static int IncrementCookstaRank()  => ++_state.CookstaRank;
        public static int GetCargoBoxLevel()      => _state.CargoBoxLevel;
        public static int IncrementCargoBox()     => ++_state.CargoBoxLevel;
        public static int GetTechSuitParts()      => _state.TechSuitParts;
        public static int IncrementTechSuitParts()=> ++_state.TechSuitParts;
        public static int GetControlRoomButtons() => _state.ControlRoomButtons;
        public static int IncrementControlRoomButtons() => ++_state.ControlRoomButtons;
        public static int GetVortexEntries()      => _state.VortexEntries;
        public static int IncrementVortexEntries()=> ++_state.VortexEntries;

        public static bool HasSeaPeopleGloves  { get => _state.HasSeaPeopleGloves;  set { _state.HasSeaPeopleGloves  = value; Save(); } }
        public static bool HasTranslator       { get => _state.HasTranslator;       set { _state.HasTranslator       = value; Save(); } }
        public static bool HasKeyToTenzhin     { get => _state.HasKeyToTenzhin;     set { _state.HasKeyToTenzhin     = value; Save(); } }
        public static bool HasLaserDevice      { get => _state.HasLaserDevice;      set { _state.HasLaserDevice      = value; Save(); } }
        public static bool HasSeaPeopleTrust   { get => _state.HasSeaPeopleTrust;   set { _state.HasSeaPeopleTrust   = value; Save(); } }
        public static bool HasTeleportMirror   { get => _state.HasTeleportMirror;   set { _state.HasTeleportMirror   = value; Save(); } }
        public static bool HasTeleportSPV      { get => _state.HasTeleportSPV;      set { _state.HasTeleportSPV      = value; Save(); } }
        public static bool HasTeleportGlacier  { get => _state.HasTeleportGlacier;  set { _state.HasTeleportGlacier  = value; Save(); } }
        public static bool HasTeleportDeep     { get => _state.HasTeleportDeep;     set { _state.HasTeleportDeep     = value; Save(); } }
        public static bool HasFishFarm         { get => _state.HasFishFarm;         set { _state.HasFishFarm         = value; Save(); } }
        public static bool HasVegetableFarm    { get => _state.HasVegetableFarm;    set { _state.HasVegetableFarm    = value; Save(); } }
        public static bool HasChickenFarm      { get => _state.HasChickenFarm;      set { _state.HasChickenFarm      = value; Save(); } }
        public static bool HasBugNet           { get => _state.HasBugNet;           set { _state.HasBugNet           = value; Save(); } }
        public static bool HasNightDive        { get => _state.HasNightDive;        set { _state.HasNightDive        = value; Save(); } }
        public static bool HasiDiverApp        { get => _state.HasiDiverApp;        set { _state.HasiDiverApp        = value; Save(); } }
        public static bool HasOxygenGrace      { get => _state.HasOxygenGrace;      set { _state.HasOxygenGrace      = value; Save(); } }
        public static int  CompletedChapters   { get => _state.CompletedChapters;   set { _state.CompletedChapters   = value; Save(); } }

        public static int GetDishResearchLevel(string dish)
        {
            _state.DishResearchLevels.TryGetValue(dish, out var level);
            return level;
        }
        public static int IncrementDishResearchLevel(string dish)
        {
            _state.DishResearchLevels.TryGetValue(dish, out var level);
            level++;
            _state.DishResearchLevels[dish] = level;
            return level;
        }

        public static bool IsRecipeUnlocked(string recipe) => _state.UnlockedRecipes.Contains(recipe);
        public static void MarkRecipeUnlocked(string recipe) { _state.UnlockedRecipes.Add(recipe); Save(); }

        public static bool IsWeaponUnlocked(string weapon) => _state.UnlockedWeapons.Contains(weapon);
        public static void MarkWeaponUnlocked(string weapon) { _state.UnlockedWeapons.Add(weapon); Save(); }

        public static bool IsCharmAcquired(string charm) => _state.AcquiredCharms.Contains(charm);
        public static void MarkCharmAcquired(string charm) { _state.AcquiredCharms.Add(charm); Save(); }

        public static bool IsIngredientFound(string ingredient) => _state.FoundIngredients.Contains(ingredient);
        public static void MarkIngredientFound(string ingredient) { _state.FoundIngredients.Add(ingredient); Save(); }

        // Enumerable accessors for ReapplyAllItems
        public static IEnumerable<string> GetUnlockedWeapons()  => _state.UnlockedWeapons;
        public static IEnumerable<string> GetAcquiredCharms()   => _state.AcquiredCharms;
        public static IEnumerable<string> GetUnlockedRecipes()  => _state.UnlockedRecipes;
        public static IEnumerable<string> GetFoundIngredients() => _state.FoundIngredients;
        public static IEnumerable<(string dish, int level)> GetDishResearchLevels()
        {
            foreach (var kv in _state.DishResearchLevels)
                yield return (kv.Key, kv.Value);
        }
    }
}
