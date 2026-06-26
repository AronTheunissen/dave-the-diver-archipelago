# Dave the Diver — TID Recording Sheet

Use this sheet during your UnityExplorer session to record TID numbers.

**How to open UnityExplorer:** Launch the game → press **F7**
**How to use C# console:** UnityExplorer → C# Console tab → paste script → Run

---

## 📋 TID Mapping Checklist

| Mapper | File | Status | Notes |
|---|---|---|---|
| `BossNameMapper` | `BossDefeatedPatch.cs` | ✅ Complete | Uses `EnumBossFishType` enum — no TIDs needed |
| `FishNameMapper` | `FishCatchPatch.cs` | ✅ Complete | Uses GameObject name substrings — no TIDs needed |
| `WeaponNameMapper` | `WeaponCraftPatch.cs` | ✅ Complete | All weapon craft TIDs filled in |
| `RecipeNameMapper` | `RecipeUnlockPatch.cs` | ✅ Complete | 300+ recipe TIDs mapped (8050xxx-8059xxx) |
| `IngredientNameMapper` | `IngredientPatch.cs` | ✅ Complete | 10 sea plants + Truffle mapped |
| `CharmMapper` | `CharmPatch.cs` | ✅ Complete | 8 charms mapped (3017xxx TIDs) |
| `VIPNameMapper` | `RestaurantPatch.cs` | 🟡 Partial | WangPang/Alex/Pastro confirmed — **needs in-game verify** |
| `QuestNameMapper` | `StoryProgressPatch.cs` | 🟡 Ready to fill | Patch now auto-logs TIDs via `GetClearMissionDialogData` hook — just play and watch BepInEx log |
| `ChallengeNameMapper` | `ChallengePatch.cs` | 🔴 Empty | All challenge mission TIDs unknown |
| Rainbow Cap TID | `IngredientPatch.cs` | ✅ Complete | `1026012` — confirmed via UnityExplorer 2026-06-26 |

**Priority order for in-game TID capture:**
1. 🔴 Quest TIDs — complete a quest, read BepInEx log for `MissionManager.UpdateMission` calls
2. 🔴 Challenge TIDs — complete a challenge, same approach
3. 🟡 VIP TID verify — beat Wang Pang, read log for `[VIP] CanProcessVIPShowdownResult: MissionClearTID=XXXX`
4. 🔴 Rainbow Cap — purchase from mushroom vendor, read log for `AddIngredientsSaveData id=XXXX`

---

## ✅ Already Confirmed TIDs (from dump.cs / previous sessions)

### Boss Types (`EnumBossFishType`)
These are confirmed from dump.cs — no UnityExplorer needed:
```
GiantSquid=1, HermitCrab=2, WolfFish=3, Clione=4, JW2=5,
Gardon=6, MantisShrimp=7, GoblinShark=8, Helicoprion=9,
GreatWhiteShark=10, Anomalocaris=11, Lusca=12, Ebirah=100
Jungle DLC: Stethacanthus=201, Xiphactinus=202, Sulong=203, SnappingTurtle=204
```

### VIP Cooking TIDs (`VIPCookingScenarioDataList.VIP_TID`)
```
WangPang=9100017, Alex=9100018, Pastro=9100019
```
Note: Vincent, Michael Bang, Sammy have no cooking challenge — tracked via MissionManager.

### Weapon Craft TIDs (from previous UnityExplorer sessions)
Already filled in `WeaponCraftPatch.cs` — see `WeaponNameMapper._idMap`.

### Recipe TIDs (from previous UnityExplorer sessions)
Already filled in `RecipeUnlockPatch.cs` — see `RecipeNameMapper._map`.
Format: `8050xxx` = raw fish sushi, `8051xxx` = cooked dishes, `8052xxx` = tuna bar.

### Ingredient TIDs (from previous UnityExplorer sessions)
Already filled in `IngredientPatch.cs` and `ItemHandler.cs`:
```
Sea Grape=1027101, Agar=1027102, Kajime=1027103, Kelp=1027104,
Seaweed=1027106, Black Coral=1027107, Southern Bull Kelp=1027108,
Buckbean=1027109, Bladderwrack=1027110, Hyalonema=1027111
Truffle=1026011
```

### Charm TIDs (from previous UnityExplorer sessions)
Already filled in `CharmPatch.cs`:
```
Dolphin Necklace=3017001, Sea People Necklace=3017011,
Octopus Bracelet=3017021, Sea People Bracelet=3017031,
Octopus Weapon Charm=3017042, Shark Teeth Necklace=3017044,
Jimbo Coin=3017049, Leo Keychain=3017101
```

---

## 🔴 Still Needed

### 1. Quest / Story Mission TIDs
**Where to find:** While playing a quest, run this in C# console:
```csharp
// Dump all active missions from MissionManager
var mgr = MissionManager.Instance;
var field = typeof(MissionManager).GetField("m_MissionList",
    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
if (field != null) {
    var list = field.GetValue(mgr) as System.Collections.IEnumerable;
    foreach (var m in list) {
        var tid = m.GetType().GetField("m_ID")?.GetValue(m);
        var name = m.GetType().GetField("m_MissionName")?.GetValue(m);
        MelonLogger.Msg($"Mission TID={tid} Name={name}");
    }
}
```
**Alternative:** Complete a quest and check BepInEx log for:
`[VIP] CanProcessVIPShowdownResult: MissionClearTID=XXXX`

| Quest Name | TID |
|---|---|
| Complete Duff's First Request | ? |
| Help Duff Investigate Blue Hole | ? |
| Complete Dr. Bacon's First Request | ? |
| Obtain Sea People Bracelet from Dr. Bacon | ? |
| Obtain Bug Net from Dr. Bacon | ? |
| Complete Cobra's First Request | ? |
| Complete Cobra's VIP Challenge | ? |
| Complete Bancho's Training | ? |
| Complete A Noisy Customer (Unlock Fish Farm) | ? |
| Gain Trust of Sea People | ? |
| Complete Niamo's Request | ? |
| Complete Linchen's Request | ? |
| Complete Ramo's Request | ? |
| Obtain Sea People Mirror (Teleport) | ? |

### 2. Challenge Mission TIDs
**Where to find:** Same MissionManager dump while at the challenge board.

| Challenge Name | TID |
|---|---|
| (Need to list in-game challenge names) | ? |

### 3. Ecowatcher Mission TIDs
**Where to find:** Open Ecowatcher app, trigger a mission, check MissionManager.

| Mission Name | TID |
|---|---|
| (Need to list in-game mission names) | ? |

### 4. Rainbow Cap TID
**Where to find:** Purchase from the mushroom vendor, check `AddIngredientsSaveData`:
```csharp
// Patch hook will log: OnIngredientCollected_Postfix called with id=XXXX
// Or search in ObjectSearch for IngredientsData with name containing "Rainbow"
```

| Ingredient | TID |
|---|---|
| Rainbow Cap | ? |

---

## 📋 Quick C# Console Scripts

### Dump All Missions (run at any time)
```csharp
// Paste in UnityExplorer C# Console
var saveData = SaveSystem.Instance?.CurrentSaveData;
// MissionManager approach — works if MissionManager is loaded
```

### Find Ingredient TID by Name
```csharp
// While holding the ingredient or after picking it up:
// Check BepInEx log for: [Ingredient] AddIngredientsSaveData id=XXXX
// The IngredientPatch hook logs this automatically
```

### Dump Ecowatcher Missions
```csharp
// Open the Ecowatcher app in-game, then:
// Trigger a mission completion and watch BepInEx log for MissionManager.UpdateMission calls
// The patch logs: type=X target=XXXX count=X
```

---

## 📝 Notes / Session Log

| Date | What was found | TIDs |
|---|---|---|
| 2026-06-19 | VIP cooking TIDs from dump.cs | WangPang=9100017, Alex=9100018, Pastro=9100019 |
| 2026-06-19 | Boss types from EnumBossFishType enum | See above |
| 2026-06-19 | Fish farm areas from FishFarmAreaType enum | A=1, B=2, C=3, D=4, E=5, F=6, G=7, H=8 |
| 2026-06-19 | Seahorse race divisions | C=Easy, B=Medium, A=Hard, S=Expert |
| 2026-06-26 | Rainbow Cap ingredient TID via UnityExplorer | `1026012` |
| 2026-06-26 | Fixed farm vegetable IDs (Bean/Buckwheat/Onion/Cucumber were wrong) | Bean=1027003, Buckwheat=1027019, Onion=1027011, Cucumber=1027015 |
| 2026-06-26 | Full ingredient dump (235 entries) obtained via UnityExplorer | All sea plants, farm veg, seasonings, fish confirmed |
