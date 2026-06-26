// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper v11
// Pure reflection on enumerator - no Il2CppSystem generics
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;
object datasObj = typeof(JDLC.JungleInsectCodex).GetProperty("InsectCodexDatas").GetValue(codex);
object rawEnum = datasObj.GetType().GetMethod("GetEnumerator").Invoke(datasObj, null);

// MoveNext is not shown but should exist on the underlying type - check all interfaces
System.Type enumType = rawEnum.GetType();
System.Reflection.MethodInfo moveNextMethod = null;

// Search all interfaces for MoveNext
System.Type[] interfaces = enumType.GetInterfaces();
for (int i = 0; i < interfaces.Length; i++)
{
    System.Reflection.MethodInfo m = interfaces[i].GetMethod("MoveNext");
    if (m != null) { moveNextMethod = m; break; }
}

// Also try direct
if (moveNextMethod == null)
    moveNextMethod = enumType.GetMethod("MoveNext", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy);

sb.AppendLine("MoveNext found: " + (moveNextMethod != null).ToString());

if (moveNextMethod != null)
{
    System.Reflection.PropertyInfo currentProp = enumType.GetProperty("Current");
    int count = 0;
    while ((bool)moveNextMethod.Invoke(rawEnum, null))
    {
        object dataObj = currentProp.GetValue(rawEnum);
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
    sb.AppendLine("Total: " + count.ToString());
}

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try
{
    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "insect_dump.txt"), result);
    Debug.Log("Saved!");
}
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
