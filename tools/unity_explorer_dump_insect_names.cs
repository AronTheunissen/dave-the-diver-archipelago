// ============================================================
// Dave the Diver — Jungle DLC: Get localized insect names
// Uses LocalizeManager to resolve NameTextID to display name
// ============================================================

var sb = new System.Text.StringBuilder();
sb.AppendLine("=== INSECT LOCALIZED NAMES ===");
sb.AppendLine("TID | IsBattle | LocalizedName | CardThumbnail");

var dm = DataManager.Instance;
var insectDict = dm.JungleInsectInfoDic;
var vals = insectDict.GetType().GetProperty("Values").GetValue(insectDict);
var e = vals.GetType().GetMethod("GetEnumerator").Invoke(vals, null);
var mn = e.GetType().GetMethod("MoveNext");
var cur = e.GetType().GetProperty("Current");

// Try to find localization method
var localizeType = IngredientsStorage.Instance.GetType().Assembly.GetType("LocalizeManager");
var localizeMethod = localizeType != null ? localizeType.GetMethod("GetLocalizeText",
    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public) : null;
sb.AppendLine("LocalizeManager: " + (localizeType != null ? "found" : "NULL"));
sb.AppendLine("GetLocalizeText: " + (localizeMethod != null ? "found" : "NULL"));

while ((bool)mn.Invoke(e, null))
{
    var entity = cur.GetValue(e);
    if (entity == null) continue;
    var t = entity.GetType();
    int tid = (int)t.GetProperty("TID").GetValue(entity);
    bool isBattle = false; try { isBattle = (bool)t.GetProperty("IsBattle").GetValue(entity); } catch { }
    string card = "?"; try { card = (string)t.GetProperty("CardThumbnail").GetValue(entity) ?? "?"; } catch { }
    string nameTextId = "?"; try { nameTextId = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?"; } catch { }

    // Try to localize
    string localName = nameTextId;
    if (localizeMethod != null)
    {
        try { localName = (string)localizeMethod.Invoke(null, new object[] { nameTextId }) ?? nameTextId; } catch { }
    }

    sb.AppendLine(tid + " | " + isBattle + " | " + localName + " | " + card);
}

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try { System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "insect_names.txt"), result); Debug.Log("Saved!"); }
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
