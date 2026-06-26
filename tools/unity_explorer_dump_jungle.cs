// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper
// Uses Il2CppSystem.Collections.Generic enumerator directly.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;

// Get InsectCodexDatas via reflection on the property
System.Type codexType = typeof(JDLC.JungleInsectCodex);
System.Reflection.PropertyInfo prop = codexType.GetProperty("InsectCodexDatas");
object datasObj = prop.GetValue(codex);

// Use the Il2Cpp enumerator via reflection
System.Type datasType = datasObj.GetType();
System.Reflection.MethodInfo getEnumMethod = datasType.GetMethod("GetEnumerator");
object enumerator = getEnumMethod.Invoke(datasObj, null);
System.Type enumType = enumerator.GetType();
System.Reflection.MethodInfo moveNext = enumType.GetMethod("MoveNext");
System.Reflection.PropertyInfo currentProp = enumType.GetProperty("Current");

int count = 0;
while ((bool)moveNext.Invoke(enumerator, null))
{
    object dataObj = currentProp.GetValue(enumerator);
    if (dataObj == null) continue;
    count++;

    System.Type dataType = dataObj.GetType();
    int tid = (int)dataType.GetProperty("TID").GetValue(dataObj);
    bool isUnlocked = (bool)dataType.GetProperty("IsUnlocked").GetValue(dataObj);

    object infoObj = dataType.GetProperty("Info").GetValue(dataObj);
    string cardName = "?";
    bool isBattle = false;
    if (infoObj != null)
    {
        System.Type infoType = infoObj.GetType();
        object card = infoType.GetProperty("CardThumbnail").GetValue(infoObj);
        if (card != null) cardName = card.ToString();
        isBattle = (bool)infoType.GetProperty("IsBattle").GetValue(infoObj);
    }

    sb.AppendLine(tid.ToString() + " | Unlocked=" + isUnlocked.ToString() + " | Battle=" + isBattle.ToString() + " | Card=" + cardName);
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
