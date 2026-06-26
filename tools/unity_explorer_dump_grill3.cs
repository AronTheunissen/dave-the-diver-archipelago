// ============================================================
// Dave the Diver — Jungle DLC: Dump all Jungle recipes from RecipeDataDic
// Jungle recipes have TIDs starting with 48 (based on grill TIDs 481500xx)
// Also dump JungleInsectInfoDic and JungleBattleInsectInfoDic for insect names
// ============================================================

var sb = new System.Text.StringBuilder();
var dm = DataManager.Instance;

// ── Section 1: Jungle fish/complex recipes from RecipeDataDic (TID >= 48000000) ──
sb.AppendLine("=== JUNGLE RECIPES (RecipeDataDic, TID>=48000000) ===");
sb.AppendLine("TID | NameTextID | Category | UnlockType | UnlockTypeValue");

var recipeDict = dm.RecipeDataDic;
var recipeVals = recipeDict.GetType().GetProperty("Values").GetValue(recipeDict);
var e1 = recipeVals.GetType().GetMethod("GetEnumerator").Invoke(recipeVals, null);
var mn1 = e1.GetType().GetMethod("MoveNext");
var cur1 = e1.GetType().GetProperty("Current");
int c1 = 0;
while ((bool)mn1.Invoke(e1, null))
{
    var entity = cur1.GetValue(e1);
    if (entity == null) continue;
    var t = entity.GetType();
    int tid = (int)t.GetProperty("TID").GetValue(entity);
    if (tid < 48000000) continue;  // Only jungle recipes
    c1++;
    string nm = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?";
    int cat = 0; try { cat = (int)t.GetProperty("Category").GetValue(entity); } catch { }
    string ut = "?"; try { ut = (string)t.GetProperty("UnlockType").GetValue(entity) ?? "?"; } catch { }
    int uv = 0; try { uv = (int)t.GetProperty("UnlockTypeValue").GetValue(entity); } catch { }
    sb.AppendLine(tid + " | " + nm + " | " + cat + " | " + ut + " | " + uv);
}
sb.AppendLine("Total jungle recipes: " + c1);

// ── Section 2: Jungle Insect names from JungleInsectInfoDic ──
sb.AppendLine();
sb.AppendLine("=== JUNGLE INSECTS (JungleInsectInfoDic) ===");
sb.AppendLine("TID | IsBattle | CardThumbnail");

var insectDict = dm.JungleInsectInfoDic;
var insectVals = insectDict.GetType().GetProperty("Values").GetValue(insectDict);
var e2 = insectVals.GetType().GetMethod("GetEnumerator").Invoke(insectVals, null);
var mn2 = e2.GetType().GetMethod("MoveNext");
var cur2 = e2.GetType().GetProperty("Current");
int c2 = 0;
while ((bool)mn2.Invoke(e2, null))
{
    var entity = cur2.GetValue(e2);
    if (entity == null) continue;
    c2++;
    var t = entity.GetType();
    int tid = (int)t.GetProperty("TID").GetValue(entity);
    bool isBattle = false; try { isBattle = (bool)t.GetProperty("IsBattle").GetValue(entity); } catch { }
    string card = "?"; try { card = (string)t.GetProperty("CardThumbnail").GetValue(entity) ?? "?"; } catch { }
    sb.AppendLine(tid + " | " + isBattle + " | " + card);
}
sb.AppendLine("Total insects: " + c2);

var result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try { System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "jungle_recipes.txt"), result); Debug.Log("Saved!"); }
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
