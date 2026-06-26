// ============================================================
// Dave the Diver — Jungle DLC: Find all grill-related DataManager tables
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== DATAMANAGER JUNGLE RECIPE TABLES ===");

var dataManager = DataManager.Instance;
var dmType = dataManager.GetType();

// List all properties containing "Grill", "Recipe", "Fish", "Jungle"
var props = dmType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
foreach (var p in props)
{
    string name = p.Name;
    if (name.Contains("Grill") || name.Contains("Recipe") || name.Contains("Jungle") || name.Contains("Fish"))
    {
        try
        {
            var val = p.GetValue(dataManager);
            string info = val != null ? val.GetType().Name : "null";
            // If it's a dictionary/list, show count
            if (val != null)
            {
                var countProp = val.GetType().GetProperty("Count");
                if (countProp != null) info = "Count=" + countProp.GetValue(val);
            }
            sb.AppendLine(name + " : " + info);
        }
        catch { sb.AppendLine(name + " : ERROR"); }
    }
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
Debug.Log("=== DONE ===");
