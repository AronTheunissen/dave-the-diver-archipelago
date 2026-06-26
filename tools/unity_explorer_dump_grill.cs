// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v6
// Uses var (which works in UE REPL) and Type.GetType for SaveSystemGameDataManager
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

// Find SaveSystemGameDataManager by type name since it may not be in REPL scope
var managerType = System.Type.GetType("SaveSystemGameDataManager, Assembly-CSharp");
sb.AppendLine("ManagerType: " + (managerType != null ? managerType.Name : "NULL"));

var managers = GameObject.FindObjectsOfType(managerType);
sb.AppendLine("Managers found: " + managers.Length.ToString());

if (managers.Length > 0)
{
    var manager = managers[0];
    var gameSaveProp = managerType.GetProperty("GameSave");
    sb.AppendLine("GameSave prop: " + (gameSaveProp != null).ToString());
    
    var saveData = gameSaveProp.GetValue(manager);
    sb.AppendLine("SaveData: " + (saveData != null).ToString());
    
    var jungleProp = saveData.GetType().GetProperty("JDLCContents");
    sb.AppendLine("JDLCContents prop: " + (jungleProp != null).ToString());
    
    var jungle = jungleProp.GetValue(saveData);
    sb.AppendLine("Jungle: " + (jungle != null).ToString());
    
    if (jungle != null)
    {
        var sushiBarField = jungle.GetType().GetField("jungleSushiBarSave",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        sb.AppendLine("SushiBarField: " + (sushiBarField != null).ToString());
        
        var sushiBar = sushiBarField.GetValue(jungle);
        sb.AppendLine("SushiBar: " + (sushiBar != null).ToString());
        
        if (sushiBar != null)
        {
            var dictProp = sushiBar.GetType().GetProperty("GrillRecipeDataDic");
            sb.AppendLine("DictProp: " + (dictProp != null).ToString());
            
            var dict = dictProp.GetValue(sushiBar);
            sb.AppendLine("Dict: " + (dict != null).ToString());
            
            if (dict != null)
            {
                int count = (int)dict.GetType().GetProperty("Count").GetValue(dict);
                sb.AppendLine("Count: " + count.ToString());
                sb.AppendLine("TID | Unlocked | Cat | NameTextID | Icon");
                
                var vals = dict.GetType().GetProperty("Values").GetValue(dict);
                var getE = vals.GetType().GetMethod("GetEnumerator");
                var e = getE.Invoke(vals, null);
                var mn = e.GetType().GetMethod("MoveNext");
                var cur = e.GetType().GetProperty("Current");
                int i = 0;
                
                while ((bool)mn.Invoke(e, null))
                {
                    var entity = cur.GetValue(e);
                    if (entity != null)
                    {
                        i++;
                        var t = entity.GetType();
                        int tid = (int)t.GetProperty("TID").GetValue(entity);
                        string name = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?";
                        string icon = (string)t.GetProperty("Icon").GetValue(entity) ?? "?";
                        int cat = (int)t.GetProperty("Category").GetValue(entity);
                        bool unlocked = false;
                        try { unlocked = (bool)t.GetMethod("IsUnlocked").Invoke(entity, null); } catch { }
                        sb.AppendLine(tid + " | " + unlocked + " | " + cat + " | " + name + " | " + icon);
                    }
                }
                sb.AppendLine("Total: " + i);
            }
        }
    }
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try
{
    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "grill_dump.txt"), result);
    Debug.Log("Saved!");
}
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
