// ============================================================
// Dave the Diver — Jungle DLC: Insect Dumper v9
// Pure reflection - no Il2CppSystem generics at all
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== JUNGLE INSECT DUMP ===");
sb.AppendLine("TID | IsUnlocked | IsBattle | CardThumbnail");

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;

// Get InsectCodexDatas via reflection
object datasObj = typeof(JDLC.JungleInsectCodex).GetProperty("InsectCodexDatas").GetValue(codex);

// Debug: what does GetEnumerator return?
object rawEnum = datasObj.GetType().GetMethod("GetEnumerator").Invoke(datasObj, null);
sb.AppendLine("Enumerator type: " + rawEnum.GetType().FullName);

// List methods on enumerator
System.Reflection.MethodInfo[] enumMethods = rawEnum.GetType().GetMethods();
for (int m = 0; m < enumMethods.Length; m++)
    sb.AppendLine("  EnumMethod: " + enumMethods[m].Name);

// List properties on enumerator  
System.Reflection.PropertyInfo[] enumProps = rawEnum.GetType().GetProperties();
for (int p = 0; p < enumProps.Length; p++)
    sb.AppendLine("  EnumProp: " + enumProps[p].Name);

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
