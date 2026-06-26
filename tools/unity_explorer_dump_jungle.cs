// ============================================================
// Dave the Diver — Jungle DLC: Debug - list methods on InsectCodexDatas
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();

JDLC.JungleInsectCodex codex = JDLC.JungleInsectCodex.Instance;
sb.AppendLine("Codex null? " + (codex == null).ToString());

System.Type codexType = typeof(JDLC.JungleInsectCodex);
System.Reflection.PropertyInfo prop = codexType.GetProperty("InsectCodexDatas");
sb.AppendLine("Prop null? " + (prop == null).ToString());

object datasObj = prop.GetValue(codex);
sb.AppendLine("datasObj null? " + (datasObj == null).ToString());

if (datasObj != null)
{
    sb.AppendLine("datasObj type: " + datasObj.GetType().FullName);
    System.Reflection.MethodInfo[] methods = datasObj.GetType().GetMethods();
    for (int i = 0; i < methods.Length; i++)
        sb.AppendLine("  Method: " + methods[i].Name);
}

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
