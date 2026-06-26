// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v12
// Cast manager to Il2CppObjectBase before calling GetValue
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

var il2cppType = Il2CppSystem.Type.GetType("DR.Save.SaveSystemGameDataManager, Assembly-CSharp");
var managers = il2cppType != null ? GameObject.FindObjectsOfType(il2cppType) : null;
sb.AppendLine("Manager found: " + (managers != null && managers.Length > 0).ToString());

if (managers != null && managers.Length > 0)
{
    // The manager is returned as UnityEngine.Object — cast to Il2CppObjectBase for reflection
    var managerObj = managers[0];
    var managerSysType = System.Type.GetType("DR.Save.SaveSystemGameDataManager, Assembly-CSharp");
    
    // Use method invoke on the pointer directly
    var gameSaveMethod = managerSysType.GetMethod("get_GameSave",
        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
    sb.AppendLine("get_GameSave method: " + (gameSaveMethod != null ? "found" : "NULL"));

    // Invoke via the Il2CppObjectBase pointer approach
    var saveData = gameSaveMethod != null ? gameSaveMethod.Invoke(managerObj, null) : null;
    sb.AppendLine("SaveData: " + (saveData != null ? saveData.GetType().Name : "NULL"));

    if (saveData != null)
    {
        var jungleMethod = saveData.GetType().GetMethod("get_JDLCContents",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var jungle = jungleMethod != null ? jungleMethod.Invoke(saveData, null) : null;
        sb.AppendLine("Jungle: " + (jungle != null ? jungle.GetType().Name : "NULL"));

        if (jungle != null)
        {
            var sushiBarField = jungle.GetType().GetField("jungleSushiBarSave",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var sushiBar = sushiBarField != null ? sushiBarField.GetValue(jungle) : null;
            sb.AppendLine("SushiBar: " + (sushiBar != null ? sushiBar.GetType().Name : "NULL"));

            if (sushiBar != null)
            {
                var dictMethod = sushiBar.GetType().GetMethod("get_GrillRecipeDataDic",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var dict = dictMethod != null ? dictMethod.Invoke(sushiBar, null) : null;
                sb.AppendLine("Dict: " + (dict != null ? "count=" + dict.GetType().GetProperty("Count").GetValue(dict) : "NULL"));

                if (dict != null)
                {
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
