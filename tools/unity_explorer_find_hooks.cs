// Paste each block separately in UnityExplorer C# Console

// BLOCK 1: FishInteractionBody methods
var sb = new System.Text.StringBuilder();
var bf = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static;
sb.AppendLine("=== FishInteractionBody ===");
foreach (var m in typeof(FishInteractionBody).GetMethods(bf))
    sb.AppendLine(m.Name + "(" + string.Join(", ", System.Array.ConvertAll(m.GetParameters(), p => p.ParameterType.Name + " " + p.Name)) + ")");
UnityEngine.Debug.Log(sb.ToString());
