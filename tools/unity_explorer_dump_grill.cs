// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v10
// Try JDLC namespace prefix for all types
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

// Try various type name combinations
string[] typeNames = new string[] {
    "JDLC.JungleSushiBarManagerSystem, Assembly-CSharp",
    "JDLC.GrillRecipeEntity, Assembly-CSharp",
    "JDLC.GrillRecipe, Assembly-CSharp",
    "JDLC.SaveSystemGameDataManager, Assembly-CSharp",
    "JDLC.JungleSushiBarSave, Assembly-CSharp",
    "SaveSystemGameDataManager, Assembly-CSharp",
    "GrillRecipeEntity, Assembly-CSharp",
};

foreach (var typeName in typeNames)
{
    var t = System.Type.GetType(typeName);
    sb.AppendLine(typeName.Split(',')[0] + ": " + (t != null ? "FOUND" : "null"));
}

// Also try using the working ingredient approach to get to SaveData
// IngredientsStorage.Instance works - let's see what assembly it's in
var ingredType = IngredientsStorage.Instance.GetType();
sb.AppendLine("IngredientsStorage assembly: " + ingredType.Assembly.GetName().Name);

// Now try to find SaveSystemGameDataManager in the same assembly
var asm = ingredType.Assembly;
foreach (var asmType in asm.GetTypes())
{
    if (asmType.Name.Contains("SaveSystem") || asmType.Name.Contains("GrillRecipe"))
        sb.AppendLine("Found: " + asmType.FullName);
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
