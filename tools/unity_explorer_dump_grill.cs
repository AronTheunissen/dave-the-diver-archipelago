// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v2
// Uses Singleton<SaveData>.Instance to get SaveData
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");
sb.AppendLine("TID | IsUnlocked | Category | NameTextID | Icon");

// SaveData extends Singleton<SaveData> - access via Instance property
SaveData saveData = Singleton<SaveData>.Instance;
sb.AppendLine("SaveData found: " + (saveData != null).ToString());
if (saveData == null) { Debug.Log(sb.ToString()); return; }

// Get JDLCContents (SaveDataJungle)
JDLC.SaveDataJungle jungle = saveData.JDLCContents;
sb.AppendLine("JDLCContents found: " + (jungle != null).ToString());
if (jungle == null) { Debug.Log(sb.ToString()); return; }

// Get SushiBarSave via reflection (it's private)
System.Type jungleType = typeof(JDLC.SaveDataJungle);
System.Reflection.FieldInfo sushiBarField = jungleType.GetField("jungleSushiBarSave",
    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
sb.AppendLine("SushiBarSave field found: " + (sushiBarField != null).ToString());
if (sushiBarField == null) { Debug.Log(sb.ToString()); return; }

object sushiBarSave = sushiBarField.GetValue(jungle);
sb.AppendLine("SushiBarSave value null: " + (sushiBarSave == null).ToString());
if (sushiBarSave == null) { Debug.Log(sb.ToString()); return; }

// Get GrillRecipeDataDic from JungleSushiBarSave
System.Reflection.PropertyInfo dictProp = sushiBarSave.GetType().GetProperty("GrillRecipeDataDic");
sb.AppendLine("GrillRecipeDataDic prop found: " + (dictProp != null).ToString());
if (dictProp == null) { Debug.Log(sb.ToString()); return; }

object dictObj = dictProp.GetValue(sushiBarSave);
sb.AppendLine("Dict null: " + (dictObj == null).ToString());
if (dictObj == null) { Debug.Log(sb.ToString()); return; }

int count = (int)dictObj.GetType().GetProperty("Count").GetValue(dictObj);
sb.AppendLine("Recipe count: " + count.ToString());

// Iterate via Values
object values = dictObj.GetType().GetProperty("Values").GetValue(dictObj);
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
    try { isUnlocked = (bool)entityType.GetMethod("IsUnlocked").Invoke(entity, null); } catch { }
    sb.AppendLine(tid.ToString() + " | " + isUnlocked.ToString() + " | Cat=" + category.ToString() + " | " + nameTextId + " | " + icon);
}
sb.AppendLine("Total: " + i.ToString());

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
