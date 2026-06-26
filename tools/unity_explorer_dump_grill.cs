// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v3
// SaveSystemGameDataManager is a MonoBehaviour - use FindObjectOfType
// Then: .GameSave -> .JDLCContents -> jungleSushiBarSave -> GrillRecipeDataDic
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");
sb.AppendLine("TID | IsUnlocked | Category | NameTextID | Icon");

// Get SaveSystemGameDataManager via FindObjectOfType
SaveSystemGameDataManager saveManager = GameObject.FindObjectOfType<SaveSystemGameDataManager>();
sb.AppendLine("SaveManager found: " + (saveManager != null).ToString());
if (saveManager == null) { Debug.Log(sb.ToString()); return; }

SaveData saveData = saveManager.GameSave;
sb.AppendLine("SaveData found: " + (saveData != null).ToString());
if (saveData == null) { Debug.Log(sb.ToString()); return; }

JDLC.SaveDataJungle jungle = saveData.JDLCContents;
sb.AppendLine("JDLCContents found: " + (jungle != null).ToString());
if (jungle == null) { Debug.Log(sb.ToString()); return; }

// Get private jungleSushiBarSave field via reflection
System.Reflection.FieldInfo sushiBarField = typeof(JDLC.SaveDataJungle).GetField(
    "jungleSushiBarSave",
    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
sb.AppendLine("SushiBarSave field: " + (sushiBarField != null).ToString());
if (sushiBarField == null) { Debug.Log(sb.ToString()); return; }

object sushiBarSave = sushiBarField.GetValue(jungle);
sb.AppendLine("SushiBarSave value null: " + (sushiBarSave == null).ToString());
if (sushiBarSave == null) { Debug.Log(sb.ToString()); return; }

System.Reflection.PropertyInfo dictProp = sushiBarSave.GetType().GetProperty("GrillRecipeDataDic");
sb.AppendLine("GrillRecipeDataDic found: " + (dictProp != null).ToString());
if (dictProp == null) { Debug.Log(sb.ToString()); return; }

object dictObj = dictProp.GetValue(sushiBarSave);
sb.AppendLine("Dict null: " + (dictObj == null).ToString());
if (dictObj == null) { Debug.Log(sb.ToString()); return; }

int count = (int)dictObj.GetType().GetProperty("Count").GetValue(dictObj);
sb.AppendLine("Recipe count: " + count.ToString());

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
