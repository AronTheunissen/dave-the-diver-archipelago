// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper
// Run this in UnityExplorer's C# Console (F7) while in-game.
// Paste the output here so we can populate the insect list.
//
// Lizards and snails are excluded (they're gift items, not AP checks).
// ============================================================
using System;
using System.Text;
using UnityEngine;

var sb = new StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine($"Generated: {DateTime.Now}");
sb.AppendLine();

// ── Insect Codex: all insects with TID, caught status, battle flag ──
sb.AppendLine("--- JungleInsectCodex.InsectCodexDatas ---");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

try
{
    var codex = JungleInsectCodex.Instance;
    foreach (var data in codex.InsectCodexDatas)
    {
        var info = data.Info;  // JungleInsectInfo
        // Try to get a readable name from CardThumbnail (e.g. "Insect_Dragonfly_01")
        // or from UILocalize if a NameTextID exists
        string name = "unknown";
        try
        {
            // CardThumbnail is like "Insect_Dragonfly_01" — strip prefix for readability
            name = info?.CardThumbnail ?? "?";
        }
        catch { }

        bool isBattle = false;
        try { isBattle = info?.IsBattle ?? false; } catch { }

        sb.AppendLine($"{data.TID} | Unlocked={data.IsUnlocked} | Battle={isBattle} | Card={name}");
    }
}
catch (Exception ex)
{
    sb.AppendLine($"ERROR (InsectCodexDatas): {ex.Message}");

    // Fallback: iterate JungleInsectInfo design sheet directly
    sb.AppendLine("Trying JungleInsectInfo.GetAll() fallback...");
    try
    {
        foreach (var info in JungleInsectInfo.GetAll())
            sb.AppendLine($"{info.TID} | Battle={info.IsBattle} | Quality={info.Quality} | Card={info.CardThumbnail}");
    }
    catch (Exception ex2)
    {
        sb.AppendLine($"ERROR (GetAll fallback): {ex2.Message}");
    }
}

// ── Summary counts ──────────────────────────────────────────
sb.AppendLine();
sb.AppendLine("--- Summary ---");
try
{
    int total = 0, unlocked = 0, battle = 0;
    foreach (var data in JungleInsectCodex.Instance.InsectCodexDatas)
    {
        total++;
        if (data.IsUnlocked) unlocked++;
        if (data.Info?.IsBattle == true) battle++;
    }
    sb.AppendLine($"Total insects: {total}");
    sb.AppendLine($"  - Net-caught (IsBattle=false): {total - battle}");
    sb.AppendLine($"  - Battle insects (IsBattle=true): {battle}");
    sb.AppendLine($"  - Unlocked so far: {unlocked}");
}
catch (Exception ex)
{
    sb.AppendLine($"ERROR (summary): {ex.Message}");
}

// ── Output ───────────────────────────────────────────────────
var result = sb.ToString();
Debug.Log(result);

// Copy to clipboard
GUIUtility.systemCopyBuffer = result;

// Write to file (check BepInEx folder or AppData)
try
{
    string path = System.IO.Path.Combine(Application.persistentDataPath, "insect_dump.txt");
    System.IO.File.WriteAllText(path, result);
    Debug.Log($"Saved to: {path}");
}
catch { }

Debug.Log("=== DONE — check clipboard + BepInEx log ===");
