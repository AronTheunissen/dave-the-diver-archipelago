// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper
// Uses Il2CppSystem enumerator with explicit cast.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;

System.Type codexType = typeof(JDLC.JungleInsectCodex);
object datasObj = codexType.GetProperty("InsectCodexDatas").GetValue(codex);

// Cast to Il2CppSystem IEnumerable and get enumerator
Il2CppSystem.Collections.Generic.IEnumerable<JDLC.JungleInsectCodexData> il2cppEnumerable =
    datasObj.Cast<Il2CppSystem.Collections.Generic.IEnumerable<JDLC.JungleInsectCodexData>>();

Il2CppSystem.Collections.Generic.IEnumerator<JDLC.JungleInsectCodexData> enumerator =
    il2cppEnumerable.GetEnumerator();

int count = 0;
while (enumerator.MoveNext())
{
    JDLC.JungleInsectCodexData data = enumerator.Current;
    if (data == null) continue;
    count++;

    JDLC.JungleInsectInfo info = data.Info;
    string cardName = (info != null && info.CardThumbnail != null) ? info.CardThumbnail : "?";
    bool isBattle = (info != null) && info.IsBattle;

    sb.AppendLine(data.TID.ToString() + " | Unlocked=" + data.IsUnlocked.ToString() + " | Battle=" + isBattle.ToString() + " | Card=" + cardName);
}

sb.AppendLine("Total: " + count.ToString() + " insects");

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;

try
{
    System.IO.File.WriteAllText(
        System.IO.Path.Combine(Application.persistentDataPath, "insect_dump.txt"),
        result);
    Debug.Log("Saved!");
}
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }

Debug.Log("=== DONE ===");
