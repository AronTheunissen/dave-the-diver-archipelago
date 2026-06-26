// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper
// Step 1: List all fields on JungleInsectCodex to find correct name
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JungleInsectCodex FIELDS ===");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;
System.Type codexType = codex.GetType();

System.Reflection.FieldInfo[] fields = codexType.GetFields(
    System.Reflection.BindingFlags.NonPublic |
    System.Reflection.BindingFlags.Public |
    System.Reflection.BindingFlags.Instance);

for (int i = 0; i < fields.Length; i++)
{
    sb.AppendLine(fields[i].Name + " : " + fields[i].FieldType.Name);
}

// Also check base type fields
System.Type baseType = codexType.BaseType;
if (baseType != null)
{
    sb.AppendLine("--- BaseType: " + baseType.Name + " ---");
    System.Reflection.FieldInfo[] baseFields = baseType.GetFields(
        System.Reflection.BindingFlags.NonPublic |
        System.Reflection.BindingFlags.Public |
        System.Reflection.BindingFlags.Instance);
    for (int i = 0; i < baseFields.Length; i++)
        sb.AppendLine(baseFields[i].Name + " : " + baseFields[i].FieldType.Name);
}

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
