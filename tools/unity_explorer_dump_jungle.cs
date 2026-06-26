// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper
// Run this in UnityExplorer's C# Console (F7) while in-game.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;
foreach (JDLC.JungleInsectCodexData data in codex.InsectCodexDatas)
{
    JDLC.JungleInsectInfo info = data.Info;
    string cardName = (info != null && info.CardThumbnail != null) ? info.CardThumbnail : "?";
    bool isBattle = (info != null) && info.IsBattle;
    sb.AppendLine(data.TID.ToString() + " | Unlocked=" + data.IsUnlocked.ToString() + " | Battle=" + isBattle.ToString() + " | Card=" + cardName);
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
catch (System.Exception ex)
{
    Debug.Log("File save failed: " + ex.Message);
}

Debug.Log("=== DONE ===");
