// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper v12
// Access via SaveData which stores insect collection state,
// then cross-reference with JungleInsectInfo design sheet.
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");

// Strategy: iterate JungleInsectInfo design sheet via its static DataDic property
// DesignSheetDataHelper<int, JungleInsectInfo> has a static DataDic dictionary

System.Type insectInfoType = typeof(JDLC.JungleInsectInfo);
sb.AppendLine("JungleInsectInfo type: " + insectInfoType.FullName);

// List all static fields and properties
System.Reflection.FieldInfo[] staticFields = insectInfoType.GetFields(
    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
for (int i = 0; i < staticFields.Length; i++)
    sb.AppendLine("StaticField: " + staticFields[i].Name + " : " + staticFields[i].FieldType.Name);

System.Reflection.PropertyInfo[] staticProps = insectInfoType.GetProperties(
    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
for (int i = 0; i < staticProps.Length; i++)
    sb.AppendLine("StaticProp: " + staticProps[i].Name + " : " + staticProps[i].PropertyType.Name);

System.Reflection.MethodInfo[] staticMethods = insectInfoType.GetMethods(
    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
for (int i = 0; i < staticMethods.Length; i++)
    sb.AppendLine("StaticMethod: " + staticMethods[i].Name);

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
