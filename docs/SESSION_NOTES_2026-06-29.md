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

---

## Session 3 Updates — 2026-06-30 (Morning)

### Major Fixes Applied

#### 1. ✅ IsGameReady Pattern Implemented
- Added public `bool IsGameReady` property to `ItemQueue.cs`
- Set to `false` at initialization, `true` after game scene loads
- All previously-disabled patches re-enabled with `if (!ItemQueue.Instance.IsGameReady) return;` guard
- This safely skips item re-application during save/load replay

#### 2. ✅ IngredientPatch Re-implemented
- **Old hook** (disabled): `SaveData.AddIngredientsSaveData` — fired during load replay, caused duplicates
- **New hook**: `IngredientsStorage.AddIngredients(int ingredientId, int count, Place place)` — fires only during gameplay
- Added `ItemQueue.IsGameReady` guard for extra safety
- Implements persistent dedup: checks `SaveData.FoundIngredients` set to avoid duplicate checks

#### 3. ✅ RecipeUnlockPatch Re-enabled
- Added `IsGameReady` guard to postfix
- Now safely detects recipe unlocks without triggering during load replay

#### 4. ✅ CharmPatch Re-enabled
- `LobbyCharmSwapPanel.AutoEquipCharmItem(int tid)` now guarded with `IsGameReady`
- Charm TIDs still need in-game verification

#### 5. ✅ CookstaPatch Follower/Grade Hooks Re-enabled
- `SNSInfoSave.set_followerCount` postfix now guarded with `IsGameReady`
- `SNSInfoSave.set_grade` postfix now guarded with `IsGameReady`
- No longer triggers during save deserialization

#### 6. ✅ ChallengePatch Deleted
- Was placeholder content with no real implementation
- Removed from Patches folder
- Removed `challenge_locations` from `apworld/davethediver/locations.py`
- No location IDs or tests affected

#### 7. ✅ BossDefeatedPatch NullRef Mitigation
- Added **void-prefix guard** (not bool-returning, to avoid IL2CPP crash)
  ```csharp
  private static bool _skipBossJob = false;
  
  [HarmonyPrefix]
  public static void OnBossDefeated_Prefix()  // void, not bool!
  {
      _skipBossJob = (BossScene.Current == null);
  }
  
  [HarmonyPostfix]
  public static void OnBossDefeated_Postfix()
  {
      if (_skipBossJob) return;  // Skip if no active BossScene
      // ... rest of postfix
  }
  ```
- Prevents cascading NullRef spam; real fix still needs base class patch

#### 8. ✅ SaveData FoundIngredients Persistence
- Added `[SerializeField] public HashSet<int> FoundIngredients` to SaveData
- Tracks which archipelago ingredient items have been claimed
- Enables persistent dedup for IngredientPatch across load/save cycles

#### 9. ✅ ItemQueue.IsGameReady Property
- Public `bool IsGameReady { get; set; }`
- Used by all patches to guard against load-time re-application
- Documented in `docs/CLASS_NAME_CHEAT_SHEET.md` with pattern example

### Unit Tests
- **23 new tests added** covering:
  - Ingredient persistence and dedup logic
  - IsGameReady guard behavior
  - Rule fixes for Lusca (Vortex Entry requirement)
  - Humboldt Squid duplicate rule cleanup
- **Total: 77/77 passing** (was 54)

### Rule Fixes
- **Lusca**: Added missing `Vortex Entry` requirement to catch rule
- **Humboldt Squid**: Cleaned up duplicate rule definition

### Still TODO / Open Issues
- [ ] **Save-load 'Continue' crash**: May be resolved by IsGameReady guards — needs in-game testing
- [ ] **CommonBossDead.DoJob NullRef**: Void prefix guard is workaround; real fix needs patch on base class `BossSceneSO.JobStuff`
- [ ] **CharmPatch TIDs**: Verify in-game that charm TIDs are correct
- [ ] **staff_training_locations**: Check if this location group should be included in location_table aggregation
- [ ] **RecipeUnlockPatch.UpgradeDish_Postfix**: Still commented out, waiting for LocationTracker hook implementation
- [ ] **SaveLoadPatch**: Disabled (SaveSystem not in interop DLL)
- [ ] **MinigamePatch**: Disabled (interop missing for seahorse racing, card games)
- [ ] **EcowatcherPatch**: Disabled (EcoWatcherDeliverPopup not found in interop)
