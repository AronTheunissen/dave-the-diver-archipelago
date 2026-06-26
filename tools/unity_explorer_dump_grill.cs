// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v13
// Access DR.GrillRecipe design sheet static data directly
// via AppDomain assembly search
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

// Get the Assembly-CSharp assembly
var asm = IngredientsStorage.Instance.GetType().Assembly;

// Find DR.GrillRecipe type
var grillRecipeType = asm.GetType("DR.GrillRecipe");
sb.AppendLine("DR.GrillRecipe: " + (grillRecipeType != null ? "FOUND" : "NULL"));

if (grillRecipeType != null)
{
    // List static fields and props to find the data dictionary
    var staticFields = grillRecipeType.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
    foreach (var f in staticFields)
        sb.AppendLine("StaticField: " + f.Name + " : " + f.FieldType.Name);
    
    var staticProps = grillRecipeType.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
    foreach (var p in staticProps)
        sb.AppendLine("StaticProp: " + p.Name + " : " + p.PropertyType.Name);

    var staticMethods = grillRecipeType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
    foreach (var m in staticMethods)
        sb.AppendLine("StaticMethod: " + m.Name);
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
