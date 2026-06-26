// ============================================================
// Dave the Diver — Jungle DLC: Find jungle recipe TID range
// Dumps the highest 50 TIDs in RecipeDataDic to find jungle range
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== RECIPE TID RANGE FINDER ===");

var dm = DataManager.Instance;
var dict = dm.RecipeDataDic;
var dictType = dict.GetType();

var keys = dictType.GetProperty("Keys").GetValue(dict);
var keysType = keys.GetType();
var keysEnum = keysType.GetMethod("GetEnumerator").Invoke(keys, null);
var keysMN = keysEnum.GetType().GetMethod("MoveNext");
var keysCur = keysEnum.GetType().GetProperty("Current");
var dictItem = dictType.GetMethod("get_Item");

// Collect all TIDs and sort them
var allTIDs = new System.Collections.Generic.List<int>();
while ((bool)keysMN.Invoke(keysEnum, null))
{
    int key = (int)keysCur.GetValue(keysEnum);
    allTIDs.Add(key);
}
allTIDs.Sort();

sb.AppendLine("Min TID: " + allTIDs[0]);
sb.AppendLine("Max TID: " + allTIDs[allTIDs.Count - 1]);
sb.AppendLine("Total: " + allTIDs.Count);
sb.AppendLine();

// Show last 80 TIDs (likely jungle) with names
sb.AppendLine("=== LAST 80 TIDs (likely jungle) ===");
sb.AppendLine("TID | NameTextID | UnlockType | UnlockVal");

int startIdx = System.Math.Max(0, allTIDs.Count - 80);
for (int i = startIdx; i < allTIDs.Count; i++)
{
    int key = allTIDs[i];
    var entity = dictItem.Invoke(dict, new object[] { key });
    if (entity == null) continue;
    var t = entity.GetType();
    string nm = "?"; try { nm = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?"; } catch { }
    string ut = "?"; try { ut = (string)t.GetProperty("UnlockType").GetValue(entity) ?? "?"; } catch { }
    int uv = 0; try { uv = (int)t.GetProperty("UnlockTypeValue").GetValue(entity); } catch { }
    sb.AppendLine(key + " | " + nm + " | " + ut + " | " + uv);
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try { System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "fish_recipes.txt"), result); Debug.Log("Saved!"); }
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
