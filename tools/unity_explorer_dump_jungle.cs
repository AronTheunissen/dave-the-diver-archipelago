// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper v8
// Uses UnhollowerBaseLib style iteration via Il2CppSystem
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;

// Get the enumerator via reflection on the Il2Cpp object
System.Type codexType = typeof(JDLC.JungleInsectCodex);
object datasObj = codexType.GetProperty("InsectCodexDatas").GetValue(codex);

// Use reflection to call GetEnumerator and iterate
System.Reflection.MethodInfo getEnumMethod = datasObj.GetType().GetMethod("GetEnumerator");
object rawEnum = getEnumMethod.Invoke(datasObj, null);

// The enumerator returned is an Il2CppSystem wrapper — get its pointer and wrap it
Il2CppSystem.Object il2cppEnum = rawEnum as Il2CppSystem.Object;

// Cast to IEnumerator via IntPtr
System.IntPtr ptr = il2cppEnum.Pointer;
Il2CppSystem.Collections.IEnumerator enumerator =
    new Il2CppSystem.Collections.IEnumerator(ptr);

int count = 0;
while (enumerator.MoveNext())
{
    Il2CppSystem.Object currentObj = enumerator.Current;
    if (currentObj == null) continue;

    JDLC.JungleInsectCodexData data = currentObj.TryCast<JDLC.JungleInsectCodexData>();
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
