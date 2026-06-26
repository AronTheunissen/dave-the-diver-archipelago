// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v9
// Access via JungleSushiBarSystem static class instead
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

// JungleSushiBarManagerSystem is a static class with GrillRecipeDataDic
// Try accessing it directly
try
{
    // List all recipes via JungleSushiBarManagerSystem static methods
    var sysType = System.Type.GetType("JungleSushiBarManagerSystem, Assembly-CSharp");
    sb.AppendLine("JungleSushiBarManagerSystem: " + (sysType != null ? "found" : "NULL"));
    
    if (sysType != null)
    {
        // List static properties
        var props = sysType.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        foreach (var p in props)
            sb.AppendLine("StaticProp: " + p.Name + " : " + p.PropertyType.Name);
        
        // List static methods
        var methods = sysType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        foreach (var m in methods)
            sb.AppendLine("StaticMethod: " + m.Name);
    }
}
catch (System.Exception ex) { sb.AppendLine("Error: " + ex.Message); }

// Also try GrillRecipe design sheet directly
try
{
    var grillRecipeType = System.Type.GetType("GrillRecipe, Assembly-CSharp");
    sb.AppendLine("GrillRecipe type: " + (grillRecipeType != null ? "found" : "NULL"));
    
    if (grillRecipeType != null)
    {
        var staticMethods = grillRecipeType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        foreach (var m in staticMethods)
            sb.AppendLine("GrillRecipe.StaticMethod: " + m.Name);
    }
}
catch (System.Exception ex2) { sb.AppendLine("GrillRecipe Error: " + ex2.Message); }

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
