// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper
// Run this in UnityExplorer's C# Console (F7) while in-game.
// All JDLC classes need the JDLC. namespace prefix.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

try
{
    JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;
    System.Collections.Generic.IEnumerable<JDLC.JungleInsectCodexData> datas = codex.InsectCodexDatas;
    foreach (JDLC.JungleInsectCodexData data in datas)
    {
        JDLC.JungleInsectInfo info = data.Info;
        string name = (info != null && info.CardThumbnail != null) ? info.CardThumbnail : "?";
        bool isBattle = (info != null) ? info.IsBattle : false;
        sb.AppendLine(data.TID.ToString() + " | Unlocked=" + data.IsUnlocked.ToString() + " | Battle=" + isBattle.ToString() + " | Card=" + name);
    }
}
catch (System.Exception ex)
{
    sb.AppendLine("ERROR (codex): " + ex.Message);
    sb.AppendLine("Trying JungleInsectInfo.GetAll() fallback...");
    try
    {
        foreach (JDLC.JungleInsectInfo infoFallback in JDLC.JungleInsectInfo.GetAll())
        {
            sb.AppendLine(infoFallback.TID.ToString() + " | Battle=" + infoFallback.IsBattle.ToString() + " | Quality=" + infoFallback.Quality.ToString() + " | Card=" + (infoFallback.CardThumbnail ?? "?"));
        }
    }
    catch (System.Exception ex2)
    {
        sb.AppendLine("ERROR (GetAll fallback): " + ex2.Message);
    }
}

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;

try
{
    string path = System.IO.Path.Combine(Application.persistentDataPath, "insect_dump.txt");
    System.IO.File.WriteAllText(path, result);
    Debug.Log("Saved to: " + path);
}
catch (System.Exception ex3)
{
    Debug.Log("Could not save file: " + ex3.Message);
}

Debug.Log("=== DONE — check clipboard ===");
