// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper v10
// MoveNext is on base IEnumerator interface - invoke via reflection
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;
object datasObj = typeof(JDLC.JungleInsectCodex).GetProperty("InsectCodexDatas").GetValue(codex);
object rawEnum = datasObj.GetType().GetMethod("GetEnumerator").Invoke(datasObj, null);

// Get the Il2CppSystem enumerator pointer and wrap as generic IEnumerator
System.IntPtr enumPtr = (System.IntPtr)rawEnum.GetType().GetProperty("Pointer").GetValue(rawEnum);
Il2CppSystem.Collections.Generic.IEnumerator<JDLC.JungleInsectCodexData> enumerator =
    new Il2CppSystem.Collections.Generic.IEnumerator<JDLC.JungleInsectCodexData>(enumPtr);

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
