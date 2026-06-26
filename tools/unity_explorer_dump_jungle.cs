// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper
// Uses reflection to access _insectCodexData dictionary directly.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;

// Access private _insectCodexData field via reflection
System.Type codexType = codex.GetType();
System.Reflection.FieldInfo field = codexType.GetField("_insectCodexData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

if (field == null)
{
    sb.AppendLine("ERROR: Could not find _insectCodexData field");
}
else
{
    object dictObj = field.GetValue(codex);
    System.Type dictType = dictObj.GetType();

    // Get Values property
    System.Reflection.PropertyInfo valuesProp = dictType.GetProperty("Values");
    object values = valuesProp.GetValue(dictObj);

    // Get enumerator from values
    System.Type valuesType = values.GetType();
    System.Reflection.MethodInfo getEnum = valuesType.GetMethod("GetEnumerator");
    object enumerator = getEnum.Invoke(values, null);
    System.Type enumType = enumerator.GetType();
    System.Reflection.MethodInfo moveNext = enumType.GetMethod("MoveNext");
    System.Reflection.PropertyInfo current = enumType.GetProperty("Current");

    while ((bool)moveNext.Invoke(enumerator, null))
    {
        object dataObj = current.GetValue(enumerator);
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
}

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
