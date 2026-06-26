// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v8
// Step by step debug to find the null
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

// Try Il2CppSystem.Type lookup
var il2cppType = Il2CppSystem.Type.GetType("SaveSystemGameDataManager, Assembly-CSharp");
sb.AppendLine("Step 1 - Il2cppType: " + (il2cppType != null ? il2cppType.Name : "NULL"));

if (il2cppType != null)
{
    var managers = GameObject.FindObjectsOfType(il2cppType);
    sb.AppendLine("Step 2 - Managers: " + managers.Length.ToString());

    if (managers.Length > 0)
    {
        sb.AppendLine("Step 3 - Manager type: " + managers[0].GetType().Name);
        
        // Try getting GameSave via reflection using System.Type
        var sysType = managers[0].GetType();
        sb.AppendLine("Step 4 - sysType: " + sysType.Name);
        
        var gameSaveProp = sysType.GetProperty("GameSave");
        sb.AppendLine("Step 5 - GameSave prop: " + (gameSaveProp != null ? "found" : "NULL"));
        
        if (gameSaveProp != null)
        {
            var saveData = gameSaveProp.GetValue(managers[0]);
            sb.AppendLine("Step 6 - SaveData: " + (saveData != null ? saveData.GetType().Name : "NULL"));
        }
    }
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
