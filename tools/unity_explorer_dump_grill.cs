// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v14
// Use JDLC.JungleSushiBarManagerSystem which was confirmed FOUND
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

var asm = IngredientsStorage.Instance.GetType().Assembly;
var managerType = asm.GetType("JDLC.JungleSushiBarManagerSystem");
sb.AppendLine("JungleSushiBarManagerSystem: " + (managerType != null ? "FOUND" : "NULL"));

if (managerType != null)
{
    // List ALL static fields, props and methods
    var flags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
    
    var fields = managerType.GetFields(flags);
    foreach (var f in fields)
    {
        if (!f.Name.StartsWith("Native"))
            sb.AppendLine("Field: " + f.Name + " : " + f.FieldType.Name);
    }
    
    var props = managerType.GetProperties(flags);
    foreach (var p in props)
        sb.AppendLine("Prop: " + p.Name + " : " + p.PropertyType.Name);
    
    var methods = managerType.GetMethods(flags);
    foreach (var m in methods)
    {
        if (!m.Name.StartsWith("Native") && !m.Name.StartsWith("get_Native") && !m.Name.StartsWith("set_Native"))
            sb.AppendLine("Method: " + m.Name + " -> " + m.ReturnType.Name);
    }
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
