// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper
// Run this in UnityExplorer's C# Console (F7) while in-game.
// Access path: SaveData -> JDLCContents -> SushiBarSave -> GrillRecipeDataDic
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

// Step 1: Get SaveData instance via static field
System.Type saveDataType = typeof(SaveData);
System.Reflection.FieldInfo[] saveFields = saveDataType.GetFields(
    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
sb.AppendLine("SaveData static fields:");
for (int f = 0; f < saveFields.Length; f++)
    sb.AppendLine("  " + saveFields[f].Name + " : " + saveFields[f].FieldType.Name);

// Step 2: Get JDLCContents property
System.Reflection.PropertyInfo jdlcProp = saveDataType.GetProperty("JDLCContents",
    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
sb.AppendLine("JDLCContents prop found: " + (jdlcProp != null).ToString());

// Step 3: List static properties to find how to get SaveData
System.Reflection.PropertyInfo[] staticProps = saveDataType.GetProperties(
    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
sb.AppendLine("SaveData static props:");
for (int p = 0; p < staticProps.Length; p++)
    sb.AppendLine("  " + staticProps[p].Name + " : " + staticProps[p].PropertyType.Name);

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
