// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper
// Run this in UnityExplorer's C# Console (F7) while in-game.
// Accesses SaveData.GrillRecipeDataDic directly.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");
sb.AppendLine("TID | IsUnlocked | Category | NameTextID | Icon");

// Access GrillRecipeDataDic via SaveData
SaveData saveData = SaveData.instance;
sb.AppendLine("SaveData found: " + (saveData != null).ToString());

System.Type saveType = typeof(SaveData);
System.Reflection.PropertyInfo dictProp = saveType.GetProperty("GrillRecipeDataDic");
sb.AppendLine("GrillRecipeDataDic prop found: " + (dictProp != null).ToString());

object dictObj = dictProp.GetValue(saveData);
sb.AppendLine("Dict found: " + (dictObj != null).ToString());

if (dictObj != null)
{
    // Dictionary<int, GrillRecipeEntity> - use reflection to get Values
    System.Type dictType = dictObj.GetType();
    sb.AppendLine("Dict type: " + dictType.Name);
    
    // Get Count
    System.Reflection.PropertyInfo countProp = dictType.GetProperty("Count");
    int count = (int)countProp.GetValue(dictObj);
    sb.AppendLine("Count: " + count.ToString());
    
    // Get Values collection
    System.Reflection.PropertyInfo valuesProp = dictType.GetProperty("Values");
    object values = valuesProp.GetValue(dictObj);
    
    // Get enumerator on values
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
        
        // Get base GrillRecipe properties
        int tid = (int)entityType.GetProperty("TID").GetValue(entity);
        string nameTextId = (string)entityType.GetProperty("NameTextID").GetValue(entity) ?? "?";
        string icon = (string)entityType.GetProperty("Icon").GetValue(entity) ?? "?";
        int category = (int)entityType.GetProperty("Category").GetValue(entity);
        
        // Try IsUnlocked()
        bool isUnlocked = false;
        try
        {
            System.Reflection.MethodInfo isUnlockedMethod = entityType.GetMethod("IsUnlocked");
            if (isUnlockedMethod != null)
                isUnlocked = (bool)isUnlockedMethod.Invoke(entity, null);
        }
        catch { }
        
        // Try to get localized name
        string localName = nameTextId;
        try
        {
            System.Reflection.MethodInfo localize = typeof(UILocalize).GetMethod(
                "GetLocalizedText",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                null, new System.Type[] { typeof(string) }, null);
            if (localize != null)
                localName = (string)localize.Invoke(null, new object[] { nameTextId }) ?? nameTextId;
        }
        catch { }
        
        sb.AppendLine(tid.ToString() + " | " + isUnlocked.ToString() + " | Cat=" + category.ToString() + " | " + localName + " | Icon=" + icon);
    }
    sb.AppendLine("Total: " + i.ToString() + " recipes");
}

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;

try
{
    System.IO.File.WriteAllText(
        System.IO.Path.Combine(Application.persistentDataPath, "grill_dump.txt"),
        result);
    Debug.Log("Saved to grill_dump.txt!");
}
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }

Debug.Log("=== DONE ===");
