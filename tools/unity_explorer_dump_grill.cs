// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v15
// Use JDLC.JungleSushiBarSave directly via Resources
// + list its members to find GrillRecipeDataDic
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

var asm = IngredientsStorage.Instance.GetType().Assembly;

// Check JDLC.JungleSushiBarSave members
var sushiBarSaveType = asm.GetType("JDLC.JungleSushiBarSave");
sb.AppendLine("JungleSushiBarSave: " + (sushiBarSaveType != null ? "FOUND" : "NULL"));

if (sushiBarSaveType != null)
{
    var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
    var props = sushiBarSaveType.GetProperties(flags);
    foreach (var p in props)
        if (!p.Name.StartsWith("Native"))
            sb.AppendLine("Prop: " + p.Name + " : " + p.PropertyType.Name);
    
    var fields = sushiBarSaveType.GetFields(flags);
    foreach (var f in fields)
        if (!f.Name.StartsWith("Native") && !f.Name.StartsWith("isWrapped") && !f.Name.StartsWith("pooled"))
            sb.AppendLine("Field: " + f.Name + " : " + f.FieldType.Name);
}

// Try to find JungleSushiBarSave instance via PhoneAppManager or similar
// Also try accessing InGameManager which might have save data
var inGameManagerType = asm.GetType("InGameManager");
sb.AppendLine("InGameManager: " + (inGameManagerType != null ? "FOUND" : "NULL"));
if (inGameManagerType != null)
{
    // Check if it has a static Instance or singleton
    var instProp = inGameManagerType.GetProperty("Instance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
    sb.AppendLine("InGameManager.Instance: " + (instProp != null ? "found" : "NULL"));
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
