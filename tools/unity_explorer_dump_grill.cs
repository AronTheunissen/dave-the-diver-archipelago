// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v11
// Correct namespaces: DR.Save.SaveSystemGameDataManager, DR.GrillRecipeEntity
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");
sb.AppendLine("TID | Unlocked | Cat | NameTextID | Icon");

// Get SaveSystemGameDataManager via Il2CppSystem type
var il2cppType = Il2CppSystem.Type.GetType("DR.Save.SaveSystemGameDataManager, Assembly-CSharp");
sb.AppendLine("Manager type: " + (il2cppType != null ? "FOUND" : "NULL"));

var managers = il2cppType != null ? GameObject.FindObjectsOfType(il2cppType) : null;
sb.AppendLine("Managers: " + (managers != null ? managers.Length.ToString() : "0"));

if (managers != null && managers.Length > 0)
{
    var manager = managers[0];
    var managerSysType = System.Type.GetType("DR.Save.SaveSystemGameDataManager, Assembly-CSharp");
    var gameSaveProp = managerSysType.GetProperty("GameSave");
    sb.AppendLine("GameSave: " + (gameSaveProp != null ? "found" : "NULL"));

    var saveData = gameSaveProp != null ? gameSaveProp.GetValue(manager) : null;
    sb.AppendLine("SaveData: " + (saveData != null ? saveData.GetType().Name : "NULL"));

    if (saveData != null)
    {
        var jungleProp = saveData.GetType().GetProperty("JDLCContents");
        var jungle = jungleProp != null ? jungleProp.GetValue(saveData) : null;
        sb.AppendLine("Jungle: " + (jungle != null ? jungle.GetType().Name : "NULL"));

        if (jungle != null)
        {
            var sushiBarField = jungle.GetType().GetField("jungleSushiBarSave",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var sushiBar = sushiBarField != null ? sushiBarField.GetValue(jungle) : null;
            sb.AppendLine("SushiBar: " + (sushiBar != null ? sushiBar.GetType().Name : "NULL"));

            if (sushiBar != null)
            {
                var dictProp = sushiBar.GetType().GetProperty("GrillRecipeDataDic");
                var dict = dictProp != null ? dictProp.GetValue(sushiBar) : null;
                sb.AppendLine("Dict: " + (dict != null ? "found, count=" + dict.GetType().GetProperty("Count").GetValue(dict) : "NULL"));

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
