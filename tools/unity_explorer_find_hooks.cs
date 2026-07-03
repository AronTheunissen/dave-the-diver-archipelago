// Unity Explorer C# Console — paste and Run
// Searches all loaded assemblies for relevant types and methods

var sb = new System.Text.StringBuilder();

foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
{
    foreach (var type in asm.GetTypes())
    {
        var tn = type.Name;
        if (tn == "InGameManager" || tn == "CarryItemManager" || tn == "FishInteractionBody" || tn == "PickupInstanceItem")
        {
            sb.AppendLine("=== " + tn + " ===");
            foreach (var m in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static))
            {
                var n = m.Name.ToLower();
                if (n.Contains("fish") || n.Contains("cargo") || n.Contains("pickup") || n.Contains("carry") || n.Contains("add") || n.Contains("interact") || n.Contains("success"))
                    sb.AppendLine("  " + m.Name + "(" + string.Join(", ", System.Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name + " " + p.Name)) + ")");
            }
        }
        if (tn == "SaveData")
        {
            sb.AppendLine("=== SaveData recipe/unlock methods ===");
            foreach (var m in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static))
            {
                var n = m.Name.ToLower();
                if (n.Contains("recipe") || n.Contains("unlock") || n.Contains("dish") || n.Contains("research") || n.Contains("fish"))
                    sb.AppendLine("  " + m.Name + "(" + string.Join(", ", System.Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name + " " + p.Name)) + ")");
            }
        }
    }
}

UnityEngine.Debug.Log(sb.ToString());
