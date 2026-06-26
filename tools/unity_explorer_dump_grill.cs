// ============================================================
// Dave the Diver — Jungle DLC: Bancho Grill Recipe Dumper v4
// No early returns — uses nested if blocks instead
// ============================================================

System.Text.StringBuilder sb = new System.Text.StringBuilder();
sb.AppendLine("=== BANCHO GRILL RECIPE DUMP ===");

SaveSystemGameDataManager saveManager = GameObject.FindObjectOfType<SaveSystemGameDataManager>();
sb.AppendLine("SaveManager: " + (saveManager != null).ToString());

if (saveManager != null)
{
    SaveData saveData = saveManager.GameSave;
    sb.AppendLine("SaveData: " + (saveData != null).ToString());

    if (saveData != null)
    {
        JDLC.SaveDataJungle jungle = saveData.JDLCContents;
        sb.AppendLine("JDLCContents: " + (jungle != null).ToString());

        if (jungle != null)
        {
            System.Reflection.FieldInfo sushiBarField = typeof(JDLC.SaveDataJungle).GetField(
                "jungleSushiBarSave",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            sb.AppendLine("SushiBarField: " + (sushiBarField != null).ToString());

            if (sushiBarField != null)
            {
                object sushiBarSave = sushiBarField.GetValue(jungle);
                sb.AppendLine("SushiBarSave null: " + (sushiBarSave == null).ToString());

                if (sushiBarSave != null)
                {
                    System.Reflection.PropertyInfo dictProp = sushiBarSave.GetType().GetProperty("GrillRecipeDataDic");
                    sb.AppendLine("DictProp: " + (dictProp != null).ToString());

                    if (dictProp != null)
                    {
                        object dictObj = dictProp.GetValue(sushiBarSave);
                        sb.AppendLine("Dict null: " + (dictObj == null).ToString());

                        if (dictObj != null)
                        {
                            int count = (int)dictObj.GetType().GetProperty("Count").GetValue(dictObj);
                            sb.AppendLine("Recipe count: " + count.ToString());
                            sb.AppendLine("TID | IsUnlocked | Cat | NameTextID | Icon");

                            object values = dictObj.GetType().GetProperty("Values").GetValue(dictObj);
                            System.Reflection.MethodInfo getEnum = values.GetType().GetMethod("GetEnumerator");
                            object enumerator = getEnum.Invoke(values, null);
                            System.Reflection.MethodInfo moveNext = enumerator.GetType().GetMethod("MoveNext");
                            System.Reflection.PropertyInfo current = enumerator.GetType().GetProperty("Current");

                            int i = 0;
                            while ((bool)moveNext.Invoke(enumerator, null))
                            {
                                object entity = current.GetValue(enumerator);
                                if (entity != null)
                                {
                                    i++;
                                    System.Type t = entity.GetType();
                                    int tid = (int)t.GetProperty("TID").GetValue(entity);
                                    string name = (string)t.GetProperty("NameTextID").GetValue(entity) ?? "?";
                                    string icon = (string)t.GetProperty("Icon").GetValue(entity) ?? "?";
                                    int cat = (int)t.GetProperty("Category").GetValue(entity);
                                    bool unlocked = false;
                                    try { unlocked = (bool)t.GetMethod("IsUnlocked").Invoke(entity, null); } catch { }
                                    sb.AppendLine(tid.ToString() + " | " + unlocked.ToString() + " | " + cat.ToString() + " | " + name + " | " + icon);
                                }
                            }
                            sb.AppendLine("Total: " + i.ToString());
                        }
                    }
                }
            }
        }
    }
}

string result = sb.ToString();
Debug.Log(result);
GUIUtility.systemCopyBuffer = result;
try
{
    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "grill_dump.txt"), result);
    Debug.Log("Saved!");
}
catch (System.Exception ex) { Debug.Log("Save failed: " + ex.Message); }
Debug.Log("=== DONE ===");
