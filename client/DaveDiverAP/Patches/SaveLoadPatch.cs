using HarmonyLib;
using BepInEx.Logging;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Detects when the player loads a save file so we can reset the
    /// "items reapplied" flag and trigger a full reapply on the next boat entry.
    ///
    /// Hook: SaveSystem.LoadSaveData() — confirmed via dump.cs.
    /// SaveSystem is a singleton that owns PlayerInfoSave and all sub-saves.
    /// It calls LoadSaveData() any time a slot is loaded (new game or continue).
    /// </summary>
    [HarmonyPatch]
    public static class SaveLoadPatch
    {
        private static ManualLogSource Log => Plugin.Log;

        // TODO: SaveSystem not found in current interop DLL.
        // Confirmed via dump.cs: SaveSystem.LoadSaveData() is the central load entry point.
        // Regenerate interop by relaunching game with BepInEx, then copy fresh SaveSystem.dll.
        //
        // [HarmonyPatch(typeof(SaveSystem), "LoadSaveData")]
        // [HarmonyPostfix]
        // public static void OnSaveDataLoaded_Postfix()
        // {
        //     Log.LogInfo("[SaveLoadPatch] Save data loaded — scheduling item reapply on next boat entry.");
        //     GameStatePatch.OnSaveLoaded();
        // }
    }
}
