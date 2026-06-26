// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v7
// Use Il2CppSystem.Type for FindObjectsOfType
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

// Use Il2CppSystem.Type for FindObjectsOfType
var il2cppType = Il2CppSystem.Type.GetType("SaveSystemGameDataManager, Assembly-CSharp");
sb.AppendLine("Il2cppType: " + (il2cppType != null ? il2cppType.Name : "NULL"));

var managers = GameObject.FindObjectsOfType(il2cppType);
sb.AppendLine("Managers: " + managers.Length.ToString());

if (managers.Length > 0)
{
    var manager = managers[0];
    // Use System.Type for reflection
    var managerSysType = System.Type.GetType("SaveSystemGameDataManager, Assembly-CSharp");
    var gameSaveProp = managerSysType.GetProperty("GameSave");
    sb.AppendLine("GameSave prop: " + (gameSaveProp != null).ToString());

    var saveData = gameSaveProp.GetValue(manager);
    sb.AppendLine("SaveData: " + (saveData != null).ToString());

    if (saveData != null)
    {
        var jungleProp = saveData.GetType().GetProperty("JDLCContents");
        var jungle = jungleProp.GetValue(saveData);
        sb.AppendLine("Jungle: " + (jungle != null).ToString());

        if (jungle != null)
        {
            var sushiBarField = jungle.GetType().GetField("jungleSushiBarSave",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            sb.AppendLine("SushiBarField: " + (sushiBarField != null).ToString());
            var sushiBar = sushiBarField != null ? sushiBarField.GetValue(jungle) : null;
            sb.AppendLine("SushiBar: " + (sushiBar != null).ToString());

            if (sushiBar != null)
            {
                var dictProp = sushiBar.GetType().GetProperty("GrillRecipeDataDic");
                var dict = dictProp != null ? dictProp.GetValue(sushiBar) : null;
                sb.AppendLine("Dict: " + (dict != null).ToString());

                if (dict != null)
                {
                    int count = (int)dict.GetType().GetProperty("Count").GetValue(dict);
                    sb.AppendLine("Count: " + count.ToString());
                    sb.AppendLine("TID | Unlocked | Cat | NameTextID | Icon");

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
                            bool unlocked = false;
                            try { unlocked = (bool)t.GetMethod("IsUnlocked").Invoke(entity, null); } catch { }
                            sb.AppendLine(tid + " | " + unlocked + " | " + cat + " | " + nm + " | " + ic);
                        }
                    }
                    sb.AppendLine("Total: " + idx);
                }
            }
        }
    }
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try { System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "grill_dump.txt"), result); Debug.Log("Saved!"); }
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
