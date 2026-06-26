// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper v13
// Find JungleInsectCodex via GameObject search, then use
// UnityExplorer's Il2CppSystem to read the dictionary via
// the codex's save data keys.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

// Access via SaveData - the insect save stores collected TIDs
// SaveData has JungleSave which has insect collection state
object saveData = null;
try
{
    System.Type saveType = System.Type.GetType("SaveData, Assembly-CSharp");
    if (saveType != null)
    {
        System.Reflection.PropertyInfo instanceProp = saveType.GetProperty("instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        if (instanceProp != null) saveData = instanceProp.GetValue(null);
    }
}
catch (System.Exception ex) { sb.AppendLine("SaveData error: " + ex.Message); }

sb.AppendLine("SaveData found: " + (saveData != null).ToString());
if (saveData != null)
{
    // List all properties on SaveData to find jungle-related ones
    System.Reflection.PropertyInfo[] props = saveData.GetType().GetProperties(
        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
    for (int i = 0; i < props.Length; i++)
    {
        string name = props[i].Name;
        if (name.ToLower().Contains("jungle") || name.ToLower().Contains("insect"))
            sb.AppendLine("  SaveData prop: " + name + " : " + props[i].PropertyType.Name);
    }
}

// Alternative: search for JungleInsectCodex via FindObjectOfType workaround
// Try getting it from Il2CppSystem type search
sb.AppendLine();
sb.AppendLine("Trying Il2CppSystem type lookup...");
try
{
    System.Type[] allTypes = System.AppDomain.CurrentDomain.GetAssemblies()[0].GetTypes();
    sb.AppendLine("Assembly-CSharp types count: " + allTypes.Length.ToString());
}
catch (System.Exception ex) { sb.AppendLine("Type lookup error: " + ex.Message); }

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
