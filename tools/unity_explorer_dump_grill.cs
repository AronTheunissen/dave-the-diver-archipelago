// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v5
// All variables declared at top to avoid REPL scoping issues
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
SaveSystemGameDataManager saveManager = null;
SaveData saveData = null;
JDLC.SaveDataJungle jungle = null;
System.Reflection.FieldInfo sushiBarField = null;
object sushiBarSave = null;
System.Reflection.PropertyInfo dictProp = null;
object dictObj = null;
object values = null;
System.Reflection.MethodInfo getEnum = null;
object enumerator = null;
System.Reflection.MethodInfo moveNext = null;
System.Reflection.PropertyInfo current = null;
int i = 0;

sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

saveManager = GameObject.FindObjectOfType<SaveSystemGameDataManager>();
sb.AppendLine("SaveManager: " + (saveManager != null).ToString());
if (saveManager != null) saveData = saveManager.GameSave;
sb.AppendLine("SaveData: " + (saveData != null).ToString());
if (saveData != null) jungle = saveData.JDLCContents;
sb.AppendLine("JDLCContents: " + (jungle != null).ToString());

if (jungle != null)
{
    sushiBarField = typeof(JDLC.SaveDataJungle).GetField("jungleSushiBarSave",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    sb.AppendLine("SushiBarField: " + (sushiBarField != null).ToString());
    if (sushiBarField != null) sushiBarSave = sushiBarField.GetValue(jungle);
    sb.AppendLine("SushiBarSave null: " + (sushiBarSave == null).ToString());
}

if (sushiBarSave != null)
{
    dictProp = sushiBarSave.GetType().GetProperty("GrillRecipeDataDic");
    sb.AppendLine("DictProp: " + (dictProp != null).ToString());
    if (dictProp != null) dictObj = dictProp.GetValue(sushiBarSave);
    sb.AppendLine("Dict null: " + (dictObj == null).ToString());
}

if (dictObj != null)
{
    int count = (int)dictObj.GetType().GetProperty("Count").GetValue(dictObj);
    sb.AppendLine("Recipe count: " + count.ToString());
    sb.AppendLine("TID | IsUnlocked | Cat | NameTextID | Icon");

    values = dictObj.GetType().GetProperty("Values").GetValue(dictObj);
    getEnum = values.GetType().GetMethod("GetEnumerator");
    enumerator = getEnum.Invoke(values, null);
    moveNext = enumerator.GetType().GetMethod("MoveNext");
    current = enumerator.GetType().GetProperty("Current");

    while ((bool)moveNext.Invoke(enumerator, null))
    {
        object entity = current.GetValue(enumerator);
        if (entity != null)
        {
            i++;
            System.Type t = entity.GetType();
            int tid = (int)t.GetProperty("TID").GetValue(entity);
            string name = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?";
            string icon = (string)t.GetProperty("Icon").GetValue(entity) ?? "?";
            int cat = (int)t.GetProperty("Category").GetValue(entity);
            bool unlocked = false;
            try { unlocked = (bool)t.GetMethod("IsUnlocked").Invoke(entity, null); } catch { }
            sb.AppendLine(tid.ToString() + " | " + unlocked.ToString() + " | " + cat.ToString() + " | " + name + " | " + icon);
        }
    }
    sb.AppendLine("Total: " + i.ToString());
}

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try
{
    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "grill_dump.txt"), result);
    Debug.Log("Saved!");
}
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
