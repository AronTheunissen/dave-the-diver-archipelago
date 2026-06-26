// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper
// Run this in UnityExplorer's C# Console (F7) while in-game.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");
sb.AppendLine("TID | IsUnlocked | Category | NameTextID | Icon");

SaveData saveData = SaveData.instance;
System.Type saveType = typeof(SaveData);
System.Reflection.PropertyInfo dictProp = saveType.GetProperty("GrillRecipeDataDic");
object dictObj = dictProp.GetValue(saveData);

sb.AppendLine("Dict type: " + dictObj.GetType().Name);
sb.AppendLine("Count: " + ((int)dictObj.GetType().GetProperty("Count").GetValue(dictObj)).ToString());

System.Reflection.PropertyInfo valuesProp = dictObj.GetType().GetProperty("Values");
object values = valuesProp.GetValue(dictObj);
System.Reflection.MethodInfo getEnum = values.GetType().GetMethod("GetEnumerator");
object enumerator = getEnum.Invoke(values, null);
System.Reflection.MethodInfo moveNext = enumerator.GetType().GetMethod("MoveNext");
System.Reflection.PropertyInfo current = enumerator.GetType().GetProperty("Current");

int i = 0;
while ((bool)moveNext.Invoke(enumerator, null))
{
    object entity = current.GetValue(enumerator);
    if (entity == null) continue;
    i++;

    System.Type entityType = entity.GetType();
    int tid = (int)entityType.GetProperty("TID").GetValue(entity);
    string nameTextId = (string)entityType.GetProperty("NameTextID").GetValue(entity) ?? "?";
    string icon = (string)entityType.GetProperty("Icon").GetValue(entity) ?? "?";
    int category = (int)entityType.GetProperty("Category").GetValue(entity);

    bool isUnlocked = false;
    try
    {
        System.Reflection.MethodInfo isUnlockedMethod = entityType.GetMethod("IsUnlocked");
        if (isUnlockedMethod != null)
            isUnlocked = (bool)isUnlockedMethod.Invoke(entity, null);
    }
    catch { }

    sb.AppendLine(tid.ToString() + " | " + isUnlocked.ToString() + " | Cat=" + category.ToString() + " | " + nameTextId + " | " + icon);
}
sb.AppendLine("Total: " + i.ToString() + " recipes");

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;

try
{
    System.IO.File.WriteAllText(
        System.IO.Path.Combine(Application.persistentDataPath, "grill_dump.txt"),
        result);
    Debug.Log("Saved!");
}
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }

Debug.Log("=== DONE ===");
