// ============================================================
// Dave the Diver — Jungle DLC: Fish Recipe Dumper
// Dumps jungle recipes (TID >= 48000000) from DataManager.RecipeDataDic
// Uses Keys list to avoid IL2Cpp iterator issues
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE FISH RECIPES ===");
sb.AppendLine("TID | NameTextID | Category | UnlockType | UnlockTypeValue");

var dm = DataManager.Instance;
var dict = dm.RecipeDataDic;
var dictType = dict.GetType();

// Get Keys (List of ints) and iterate by index
var keys = dictType.GetProperty("Keys").GetValue(dict);
var keysType = keys.GetType();
int count = (int)keysType.GetProperty("Count").GetValue(keys);
sb.AppendLine("Total recipes in RecipeDataDic: " + count);

// Get the indexer on Keys
var keysToArray = keysType.GetMethod("ToArray") ?? keysType.GetMethod("get_Item");

// Use GetEnumerator on Keys which returns int values
var keysEnum = keysType.GetMethod("GetEnumerator").Invoke(keys, null);
var keysMN = keysEnum.GetType().GetMethod("MoveNext");
var keysCur = keysEnum.GetType().GetProperty("Current");

// Also get the indexer on the dictionary
var dictItem = dictType.GetMethod("get_Item");

int jungleCount = 0;
while ((bool)keysMN.Invoke(keysEnum, null))
{
    int key = (int)keysCur.GetValue(keysEnum);
    if (key < 48000000) continue;  // Only jungle recipes
    
    var entity = dictItem.Invoke(dict, new object[] { key });
    if (entity == null) continue;
    jungleCount++;
    
    var t = entity.GetType();
    string nm = "?"; try { nm = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?"; } catch { }
    int cat = 0; try { cat = (int)t.GetProperty("Category").GetValue(entity); } catch { }
    string ut = "?"; try { ut = (string)t.GetProperty("UnlockType").GetValue(entity) ?? "?"; } catch { }
    int uv = 0; try { uv = (int)t.GetProperty("UnlockTypeValue").GetValue(entity); } catch { }
    sb.AppendLine(key + " | " + nm + " | " + cat + " | " + ut + " | " + uv);
}
sb.AppendLine("Total jungle recipes: " + jungleCount);

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try { System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "fish_recipes.txt"), result); Debug.Log("Saved!"); }
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
