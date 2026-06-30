# Session Notes — 2026-06-29

## Problem 1: `CommonBossDead::DoJob` NullReferenceException Loop

### Status: ✅ Understood, not yet fixed (noisy but not game-breaking)

**What happens:**
The game's own `DoJob()` method crashes in a tight loop (50+ times) when `BossScene.Current` is null.
This occurs during save/load or scene teardown — the `BossSceneSO` job system keeps retrying.
Our postfix is fine and unaffected; the crash is entirely in the game's own code.

**Attempted fix:**
Added a `[HarmonyPrefix]` returning `bool` to guard against null `BossScene.Current`.
This caused the **game to crash on the loading screen** — likely because:
- `DoJob()` is inherited from `BossSceneSO.JobStuff`, not declared directly on `CommonBossDead`
- Harmony has trouble attaching a `bool`-returning prefix to an inherited IL2CPP method

**Current state:** Reverted to original postfix-only. Loop is noisy in the log but non-fatal.

**Next steps:**
- Use UnityExplorer in-game to confirm whether `DoJob()` is declared on `CommonBossDead`
  directly or inherited from `BossSceneSO.JobStuff`
- If inherited, patch `BossSceneSO.JobStuff` instead, or find a different hook point on `BossScene`

---

## Problem 2: Silent Crash on "Continue" (Save Load)

### Status: 🔴 Not yet fixed

**What happens:**
Game silently closes when pressing "Continue" on the main menu to load a save file.
No useful error in the BepInEx log (drowned out by the NullRef loop above).

**Root cause identified:**
Several patches hook methods on save data classes that fire during **save deserialization on load**,
not just during gameplay. These cause an unhandled exception that silently kills the game.

### Patches disabled so far:

| Patch | Hook | Reason disabled |
|---|---|---|
| `IngredientPatch` | `global::SaveData.AddIngredientsSaveData` | Fires during load replay |
| `RecipeUnlockPatch` | `global::SaveData.AddUnlockRecipeSaveData` | Fires during load replay |
| `RecipeUnlockPatch` | `global::SaveData.UpdateUnlockRecipeSave` | Fires during load replay |
| `CookstaPatch` | `SNSInfoSave.set_followerCount` | Fires during save deserialization |
| `CookstaPatch` | `SNSInfoSave.set_grade` | Fires during save deserialization |

### Still crashing — remaining active patches to investigate:

- `CookstaPatch` → `SNSInfoManager.CheckGradeConditionMessage`
- `StoryProgressPatch` → `MissionManager.GetClearMissionDialogData`
- `GameStatePatch` → `LobbyPlayer.ChangeLobbyPlayerState`
- `GameStatePatch` → `SushiBarManager.OnEventSushiBarOpened`
- `CollectiblePatch` → `InstanceItemChest.SuccessInteract`
- `CollectiblePatch` → `InteractionGimmick_PhotoZone.SuccessInteraction`
- `CharmPatch` → `LobbyCharmSwapPanel.AutoEquipCharmItem`
- `FarmPatch` → `MVFarmFieldController.DoHarvest`
- `RestaurantPatch` → `SushiBarAnalyticsReportSequenceCookStar.DoSequence`
- `PhotographyPatch` → `LobbyPostRoutine.PhotoRewardSequence`

### Suggested debug approach:
To quickly narrow down which patch is the culprit, temporarily disable patches one by one
(or in halves — binary search) and rebuild until the crash stops.

Alternatively, wrap all postfix bodies in try/catch with `Plugin.Log.LogError` so exceptions
surface in the log rather than killing the game silently.

---

---

## Session 2 Updates — 2026-06-30

### Additional patches disabled (save-load crash)
- `CharmPatch` → `LobbyCharmSwapPanel.AutoEquipCharmItem` — auto-equips on load

### Try/catch added to ALL active patch methods
All postfix methods now catch exceptions and log via `Plugin.Log.LogError()` so future
crashes surface in the BepInEx log instead of silently closing the game.

### Better hook points identified for all disabled patches

| Disabled Patch | Old Hook | Better Hook |
|---|---|---|
| `IngredientPatch` | `SaveData.AddIngredientsSaveData` | `IngredientsStorage.AddIngredients(int, int, Place)` — SingletonNoMono, gameplay only |
| `RecipeUnlockPatch` | `SaveData.AddUnlockRecipeSaveData` | `MissionManager.UpdateMission()` filtered for recipe missions, OR recipe unlock UI popup |
| `CookstaPatch` | `SNSInfoSave.set_followerCount/grade` | `SNSInfoManager.CheckGradeConditionMessage()` — already partially active |
| `CharmPatch` | `LobbyCharmSwapPanel.AutoEquipCharmItem` | `MissionManager.UpdateMission()` filtered for charm-granting mission TIDs |

### IL2CPP Bool-Returning Prefix Crash — Root Cause Found

**Why `[HarmonyPrefix] public static bool Method()` crashed on startup:**
- Bool-returning prefixes require HarmonyX to generate IL branching code (store result → branch → invoke original)
- On IL2CPP inherited methods, this IL generation produces **invalid IL code** → `InvalidProgramException` at startup
- Postfixes work because they only append code after the method (simpler IL, no branching needed)
- This is a documented HarmonyX issue (#93, #129, #87)

**The fix for the NullRef loop going forward:**
- Do NOT use `[HarmonyPrefix] static bool` on IL2CPP inherited methods
- Use a **void-returning prefix** instead (no bool, no branching IL generated)
- Or use a state variable set in a void prefix, checked in the postfix

**Working pattern for void prefix guard:**
```csharp
private static bool _skipOriginal = false;

[HarmonyPatch(typeof(CommonBossDead), "DoJob")]
[HarmonyPrefix]
public static void OnBossDefeated_Prefix()  // void, not bool!
{
    _skipOriginal = (BossScene.Current == null);
    if (_skipOriginal)
        Plugin.Log.LogWarning("[Boss] DoJob called with no active BossScene.");
}

[HarmonyPatch(typeof(CommonBossDead), "DoJob")]
[HarmonyPostfix]
public static void OnBossDefeated_Postfix()
{
    if (_skipOriginal) return;  // original already ran and crashed — but at least we know
    // ... rest of postfix
}
```
Note: A void prefix cannot prevent the original from running, so the NullRef will still
fire once — but the loop should stop since we're not breaking the game's internal state.
The real fix is to patch the base class `BossSceneSO.JobStuff` if possible, or find a
different hook point on `BossScene` itself.

---

## Key Lesson Learned

> **Never hook `*Save` class setters or `global::SaveData` methods.**
> They fire during save deserialization on load, not just during gameplay.

**Safe hook points:**
- ✅ Manager singletons (e.g. `SNSInfoManager`, `MissionManager`) — only active during gameplay
- ✅ UI classes (e.g. unlock popups, result panels) — only shown for new events
- ✅ Interaction classes (e.g. `FishInteractionBody.SuccessInteract`) — only fire on player action

**Dangerous hook points:**
- ❌ `global::SaveData.*` methods — replay on load
- ❌ `*Save` class property setters — fire during deserialization
- ❌ Any class whose name ends in `Save` or `SaveData`
