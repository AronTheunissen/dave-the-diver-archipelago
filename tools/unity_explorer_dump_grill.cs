// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v16
// GrillRecipeDataDic is on DataManager : Singleton<DataManager>
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");
sb.AppendLine("TID | Cat | NameTextID | Icon | UnlockType | UnlockTypeValue");

// DataManager is Singleton<DataManager> which extends MonoBehaviour
var dataManager = DataManager.Instance;
sb.AppendLine("DataManager: " + (dataManager != null ? "FOUND" : "NULL"));

if (dataManager != null)
{
    var dictProp = dataManager.GetType().GetProperty("GrillRecipeDataDic");
    sb.AppendLine("GrillRecipeDataDic: " + (dictProp != null ? "found" : "NULL"));

    if (dictProp != null)
    {
        var dict = dictProp.GetValue(dataManager);
        sb.AppendLine("Dict: " + (dict != null ? "count=" + dict.GetType().GetProperty("Count").GetValue(dict) : "NULL"));

        if (dict != null)
        {
            var vals = dict.GetType().GetProperty("Values").GetValue(dict);
            var e = vals.GetType().GetMethod("GetEnumerator").Invoke(vals, null);
            var mn = e.GetType().GetMethod("MoveNext");
            var cur = e.GetType().GetProperty("Current");
            int idx = 0;

            while ((bool)mn.Invoke(e, null))
            {
                var entity = cur.GetValue(e);
                if (entity != null)
                {
                    idx++;
                    var t = entity.GetType();
                    int tid = (int)t.GetProperty("TID").GetValue(entity);
                    string nm = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?";
                    string ic = (string)t.GetProperty("Icon").GetValue(entity) ?? "?";
                    int cat = (int)t.GetProperty("Category").GetValue(entity);
                    string unlockType = "?";
                    int unlockVal = 0;
                    try { unlockType = (string)t.GetProperty("UnlockType").GetValue(entity) ?? "?"; } catch { }
                    try { unlockVal = (int)t.GetProperty("UnlockTypeValue").GetValue(entity); } catch { }
                    sb.AppendLine(tid + " | " + cat + " | " + nm + " | " + ic + " | " + unlockType + " | " + unlockVal);
                }
            }
            sb.AppendLine("Total: " + idx);
        }
    }
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try { System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "grill_dump.txt"), result); Debug.Log("Saved!"); }
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
