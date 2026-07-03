// Unity Explorer C# Console Script
// Open UnityExplorer (F7), go to C# Console tab, paste and click Run
// Outputs to the BepInEx log AND the Unity Explorer log panel

using System.Reflection;
using System.Linq;

// ── 1. Find InGameManager fish/cargo/pickup methods ───────────────────────
UnityEngine.Debug.Log("=== InGameManager: fish/cargo/pickup methods ===");
foreach (var m in typeof(InGameManager).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
{
    var n = m.Name.ToLower();
    if (n.Contains("fish") || n.Contains("cargo") || n.Contains("pickup") || n.Contains("carry"))
        UnityEngine.Debug.Log($"  {m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})");
}

// ── 2. Find CarryItemManager methods ─────────────────────────────────────
UnityEngine.Debug.Log("=== CarryItemManager: all methods ===");
foreach (var m in typeof(CarryItemManager).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
    UnityEngine.Debug.Log($"  {m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})");

// ── 3. Find PickupInstanceItem methods ───────────────────────────────────
UnityEngine.Debug.Log("=== PickupInstanceItem: all methods ===");
foreach (var m in typeof(PickupInstanceItem).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
    UnityEngine.Debug.Log($"  {m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})");

// ── 4. Search MissionManager for Sato/FishCard/Prologue TIDs ─────────────
UnityEngine.Debug.Log("=== MissionManager: Sato/Fish/Prologue missions ===");
var mm = MissionManager.Instance;
if (mm != null)
{
    var allMissions = mm.GetAllMissionData();
    if (allMissions != null)
    {
        foreach (var mission in allMissions)
        {
            if (mission == null) continue;
            var name = mission.missionName ?? "";
            var tid = mission.TID;
            if (name.ToLower().Contains("sato") || name.ToLower().Contains("fish") ||
                name.ToLower().Contains("card") || name.ToLower().Contains("prologue") ||
                name.ToLower().Contains("tutorial") || name.ToLower().Contains("bancho") ||
                tid == 10010001 || tid == 10010002 || tid == 10010003)
                UnityEngine.Debug.Log($"  TID={tid} Name={name}");
        }
    }
}

UnityEngine.Debug.Log("=== DONE — check BepInEx log for output ===");
