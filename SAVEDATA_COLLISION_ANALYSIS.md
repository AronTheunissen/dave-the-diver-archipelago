# SaveData Namespace Collision Analysis Report

## Summary
The Dave the Diver Archipelago BepInEx mod has a critical namespace collision issue. The mod defines its own `SaveData` class in the `DaveDiverAP` namespace, which shadows the game's global `SaveData` class. This breaks Harmony patches that attempt to patch game SaveData methods using `typeof(global::SaveData)`.

---

## Problem Description

**The Issue:**
- **Mod's SaveData**: Defined in `client/DaveDiverAP/SaveData.cs` (line 20: `public static class SaveData`)
- **Game's SaveData**: A global game class that should be patched by Harmony
- **The Bug**: When Harmony patches use `typeof(global::SaveData)`, they may resolve to the mod's SaveData instead due to namespace resolution order, causing patches on game SaveData methods to fail silently.

**Current Status:**
- The mod's SaveData class is in the `DaveDiverAP` namespace
- Multiple patches explicitly use `typeof(global::SaveData)` to target the game's class, but this is fragile
- The mod also calls methods on its own SaveData class extensively, creating ambiguity

---

## Files Affected

### 1. **Mod's SaveData Class Definition**
- **File**: `client/DaveDiverAP/SaveData.cs`
- **Namespace**: `DaveDiverAP`
- **Lines**: 13-240
- **Class Definition**: Line 20: `public static class SaveData`
- **Purpose**: Persists mod state (checked locations, item indices, connection info, equipment levels, charms, weapons, recipes, etc.)

### 2. **Files Referencing MOD's SaveData (Static Methods)**
The mod's SaveData class is referenced in **9 files**:

#### **ItemHandler.cs** (25+ references)
- Lines: 253, 343-346, 374-389, 399, 417, 433, 449, 555, 566, 575, 579, 613, 670-671, 701-702, 724, 754-755
- Methods called: `SetLastItemIndex()`, `GetOxygenTankLevel()`, `GetHarpoonLevel()`, `GetDivingSuitLevel()`, `GetCargoBoxLevel()`, `GetCookstaRank()`, `GetUnlockedWeapons()`, `GetAcquiredCharms()`, `GetUnlockedRecipes()`, `GetDishResearchLevels()`, `IncrementOxygenTank()`, `IncrementHarpoon()`, `IncrementDivingSuit()`, `IncrementCargoBox()`, `IncrementTechSuitParts()`, `IncrementControlRoomButtons()`, `IncrementVortexEntries()`, `IncrementCookstaRank()`, `IncrementDishResearchLevel()`, `IsCharmAcquired()`, `MarkCharmAcquired()`, `IsWeaponUnlocked()`, `MarkWeaponUnlocked()`, `IsRecipeUnlocked()`, `MarkRecipeUnlocked()`

#### **UI/ProgressUI.cs** (9 references)
- Lines: 161-162, 192-198, 244, 268
- Methods called: `GetControlRoomButtons()`, `GetOxygenTankLevel()`, `GetDivingSuitLevel()`, `GetHarpoonLevel()`, `GetCargoBoxLevel()`, `GetTechSuitParts()`, `GetCookstaRank()`, `IsCharmAcquired()`, `IsWeaponUnlocked()`

#### **Plugin.cs** (1 reference)
- Line: 89
- Method called: `LoadConnectionInfo()`
- Note: Also calls `SaveData.Save()` (line 449 in ItemHandler)

#### **Patches/IngredientPatch.cs** (2 references)**
- Lines: 40-41 (in OnIngredientAdded_Postfix method)
- Methods called: `IsIngredientFound()`, `MarkIngredientFound()`

#### **Patches/FishCatchPatch.cs** (1 reference)**
- Line: 65 (comment referencing SaveData.AddCaughtFish, but this is the GAME's SaveData)

#### **Patches/CookstaPatch.cs** (Comments only)**
- Lines: 77-78 (comments reference "direct SaveData.Instance access fails due to namespace collision")

#### **Patches/FarmPatch.cs** (Comments only)**
- Lines: 50, 69, 71 (comments reference SaveData.FarmSave, which is the GAME's SaveData)

#### **Patches/CharmPatch.cs** (No direct references)**
- File exists in patches directory but doesn't directly reference SaveData class

---

### 3. **Files Using GAME's SaveData (via typeof(global::SaveData) in HarmonyPatch attributes)**

#### **Patches/FishCatchPatch.cs** (Line 70)
```csharp
[HarmonyPatch(typeof(global::SaveData), "AddCaughtFish")]
```
- This patch attempts to hook game's SaveData.AddCaughtFish() method
- **STATUS**: Likely broken due to namespace collision

#### **Patches/RecipeUnlockPatch.cs** (Lines 23, 51, 82)
```csharp
[HarmonyPatch(typeof(global::SaveData), "AddUnlockRecipeSaveData")]
[HarmonyPatch(typeof(global::SaveData), "AddCookingStudySaveData")]
[HarmonyPatch(typeof(global::SaveData), "UpdateCookingStudySaveData")]
```
- Three patches attempting to hook game's SaveData methods
- **STATUS**: Likely broken due to namespace collision

---

## Root Cause Analysis

**Why This Is a Problem:**
1. When C# resolves `typeof(global::SaveData)` in the context of `DaveDiverAP` namespace, it first checks the current namespace
2. If a `SaveData` class exists in `DaveDiverAP`, the `global::` prefix should force global resolution, BUT:
   - If the interop/Assembly-CSharp.dll doesn't properly export the game's SaveData type
   - Or if there's a namespace confusion in Harmony's type resolution
   - The patches may silently fail to find the game's method

3. The mod then compensates by using workarounds:
   - `ItemHandler.cs` line 738-740: Comments state "Direct SaveData.Instance access fails due to namespace collision"
   - `ItemHandler.cs` line 769: Comments state "Direct SaveData.Instance access fails due to namespace collision with our own SaveData class"
   - These workarounds use `CompleteMission()` instead of directly accessing game's SaveData

---

## Count Summary

| Category | Count | Files |
|----------|-------|-------|
| **Files referencing MOD's SaveData** | 5 | ItemHandler.cs, ProgressUI.cs, Plugin.cs, IngredientPatch.cs, CookstaPatch.cs |
| **Total references to MOD's SaveData methods** | 40+ | Scattered across ItemHandler.cs (25+), ProgressUI.cs (9), Plugin.cs (1), IngredientPatch.cs (2), Comments (3+) |
| **Patch files using game's SaveData** | 2 | FishCatchPatch.cs, RecipeUnlockPatch.cs |
| **Game SaveData patches that likely fail** | 4 | AddCaughtFish, AddUnlockRecipeSaveData, AddCookingStudySaveData, UpdateCookingStudySaveData |

---

## Recommended Fix

**Rename the mod's SaveData class from `SaveData` to `ModSaveData`** (or similar):

1. **File to change**: `client/DaveDiverAP/SaveData.cs`
   - Line 20: Change `public static class SaveData` → `public static class ModSaveData`

2. **Files that need updates**:
   - `ItemHandler.cs`: Replace all `SaveData.` calls with `ModSaveData.` (25+ replacements)
   - `ProgressUI.cs`: Replace all `SaveData.` calls with `ModSaveData.` (9 replacements)
   - `Plugin.cs`: Replace `SaveData.LoadConnectionInfo()` with `ModSaveData.LoadConnectionInfo()` (1 replacement)
   - `Patches/IngredientPatch.cs`: Replace `SaveData.` calls with `ModSaveData.` (2 replacements)

3. **No changes needed**:
   - Patches/FishCatchPatch.cs: Uses `typeof(global::SaveData)` - will work correctly once namespace is clear
   - Patches/RecipeUnlockPatch.cs: Uses `typeof(global::SaveData)` - will work correctly once namespace is clear
   - Comments in other files can be updated but are not critical

---

## Verification Steps After Rename

1. Ensure all `SaveData.` references in code are either:
   - Changed to `ModSaveData.` (if referencing the mod's class)
   - Left as `global::SaveData` (if referencing game's class in HarmonyPatch attributes)

2. Test that Harmony patches on `global::SaveData` methods now resolve correctly:
   - Fish catch detection (FishCatchPatch.AddCaughtFish)
   - Recipe unlock detection (RecipeUnlockPatch methods)

3. Verify no compilation errors and no runtime type resolution failures

---

## Notes

- The `DaveDiverAP` namespace is consistent across all files
- The mod's SaveData is a static class (good design for persistence)
- The game's SaveData is a global game class that should NOT be confused with the mod's
- Comments in ItemHandler.cs explicitly acknowledge the namespace collision issue
