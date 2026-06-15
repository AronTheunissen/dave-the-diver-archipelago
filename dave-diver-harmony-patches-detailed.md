# Dave the Diver - Complete Harmony Patch Map

## Overview

This document maps every Harmony patch in the DaveDiverExpansion mod, organized by game class and feature.

**Format**: 
```
[File: filename.cs:line]
Class: ClassName → Method(parameters)
Patch Type: [Prefix/Postfix/Transpiler]
Patch Class: PatchClassName
Purpose: What it does
```

---

## CORE GAME CLASS PATCHES

### PlayerCharacter (Most Patched Class)

#### Patch 1: AutoPickup Feature
- **File**: `Features/AutoPickup.cs:200`
- **Target**: `PlayerCharacter.Update()`
- **Patch Type**: Postfix
- **Patch Class**: `AutoPickupPatch`
- **Code**: 
  ```csharp
  [HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Update))]
  public static class AutoPickupPatch
  {
      private static void Postfix(PlayerCharacter __instance)
      {
          AutoPickup.TryPickupNearby(__instance);
      }
  }
  ```
- **Purpose**: Automatically picks up nearby fish, items, and chests during diving
- **Called Every Frame**: Yes
- **Parameters**: Player instance via `__instance`

#### Patch 2: iDiverExtension Features
- **File**: `Features/iDiverExtension.cs:757`
- **Target**: `PlayerCharacter.Update()`
- **Patch Type**: Prefix + Postfix
- **Patch Class**: `AllEffects_Patch`
- **Code Structure**:
  ```csharp
  [HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Update))]
  static class AllEffects_Patch
  {
      static void Prefix(PlayerCharacter __instance) { ... }
      static void Postfix() { ... }
  }
  ```
- **Purpose**: Apply equipment upgrade effects (move speed, booster, crab trap efficiency, etc.)
- **Scope**: Multiple sub-systems handled in one patch

#### Patch 3: Fish Density System
- **File**: `Features/FishDensity.cs:152`
- **Target**: `PlayerCharacter.Update()`
- **Patch Type**: Postfix
- **Patch Class**: `FishDensityScanPatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Update))]
  static class FishDensityScanPatch
  {
      static void Postfix()
      {
          FishDensity.ScanAllocators();
      }
  }
  ```
- **Purpose**: Scan and spawn additional fish groups to increase density
- **Execution**: Once per frame, scans `InGameManager.FishAllocators`

#### Patch 4: Entity Registry Purging
- **File**: `Helpers/EntityRegistry.cs:164`
- **Target**: `PlayerCharacter.Update()`
- **Patch Type**: Postfix
- **Patch Class**: `EntityRegistryPurgePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Update))]
  static class EntityRegistryPurgePatch
  {
      static void Postfix() => EntityRegistry.Purge();
  }
  ```
- **Purpose**: Clean up null/destroyed objects from entity tracking sets every 2 seconds
- **Frequency**: Every 2.0 seconds (via internal timer in Purge())

#### Patch 5: Save Debug Scanning
- **File**: `Features/SaveDebug.cs:88`
- **Target**: `PlayerCharacter.Update()`
- **Patch Type**: Postfix
- **Patch Class**: `PeriodicScanPatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Update))]
  static class PeriodicScanPatch
  {
      static void Postfix()
      {
          if (_scanned) return;
          var t = Time.time;
          if (t - _lastScanTime < 5f) return;
          _lastScanTime = t;
          if (t < 10f) return;
          _scanned = true;
          ScanAllFigureChests();
      }
  }
  ```
- **Purpose**: Debug scan for Godzilla figurine chests at game start
- **Conditions**: Only runs once after 10+ seconds of gameplay

⚠️ **CRITICAL**: `PlayerCharacter.Update()` has **5 patches** applied. Load order matters!

---

## FISH SYSTEM PATCHES

### FishInteractionBody

#### Patch 1: Fish Registry (Awake)
- **File**: `Helpers/EntityRegistry.cs:100`
- **Target**: `FishInteractionBody.Awake()`
- **Patch Type**: Postfix
- **Patch Class**: `FishAwakePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(FishInteractionBody), nameof(FishInteractionBody.Awake))]
  static class FishAwakePatch
  {
      static void Postfix(FishInteractionBody __instance)
      {
          EntityRegistry.AllFish.Add(__instance);
          if (Plugin.DebugLog?.Value == true)
              Plugin.Log.LogInfo($"[EntityRegistry] Fish+ ...");
      }
  }
  ```
- **Purpose**: Register newly spawned fish into tracking set
- **Frequency**: Once per fish, during Awake lifecycle
- **Registry**: Adds to `EntityRegistry.AllFish` HashSet

---

## ITEM PICKUP SYSTEM PATCHES

### PickupInstanceItem

#### Patch 1: Item OnEnable
- **File**: `Helpers/EntityRegistry.cs:56`
- **Target**: `PickupInstanceItem.OnEnable()`
- **Patch Type**: Postfix
- **Patch Class**: `PickupItemOnEnablePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(PickupInstanceItem), nameof(PickupInstanceItem.OnEnable))]
  static class PickupItemOnEnablePatch
  {
      static void Postfix(PickupInstanceItem __instance)
      {
          EntityRegistry.AllItems.Add(__instance);
      }
  }
  ```
- **Purpose**: Register items when they become active
- **Frequency**: Every time an item is enabled
- **Registry**: Adds to `EntityRegistry.AllItems` HashSet

#### Patch 2: Item OnDisable
- **File**: `Helpers/EntityRegistry.cs:67`
- **Target**: `PickupInstanceItem.OnDisable()`
- **Patch Type**: Postfix
- **Patch Class**: `PickupItemOnDisablePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(PickupInstanceItem), nameof(PickupInstanceItem.OnDisable))]
  static class PickupItemOnDisablePatch
  {
      static void Postfix(PickupInstanceItem __instance)
      {
          EntityRegistry.AllItems.Remove(__instance);
      }
  }
  ```
- **Purpose**: Unregister items when they're disabled/destroyed
- **Frequency**: Every time an item is disabled
- **Registry**: Removes from `EntityRegistry.AllItems` HashSet

---

## CHEST & LOOT SYSTEM PATCHES

### InstanceItemChest

#### Patch 1: Chest OnEnable
- **File**: `Helpers/EntityRegistry.cs:78`
- **Target**: `InstanceItemChest.OnEnable()`
- **Patch Type**: Postfix
- **Patch Class**: `ChestOnEnablePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(InstanceItemChest), nameof(InstanceItemChest.OnEnable))]
  static class ChestOnEnablePatch
  {
      static void Postfix(InstanceItemChest __instance)
      {
          EntityRegistry.AllChests.Add(__instance);
      }
  }
  ```
- **Purpose**: Register chests when they spawn into scene
- **Frequency**: Once per chest, on enable
- **Registry**: Adds to `EntityRegistry.AllChests` HashSet

#### Patch 2: Chest SuccessInteract (OPENING)
- **File**: `Helpers/EntityRegistry.cs:89`
- **Target**: `InstanceItemChest.SuccessInteract(BaseCharacter)`
- **Patch Type**: Postfix
- **Patch Class**: `ChestSuccessInteractPatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(InstanceItemChest), nameof(InstanceItemChest.SuccessInteract))]
  static class ChestSuccessInteractPatch
  {
      static void Postfix(InstanceItemChest __instance)
      {
          EntityRegistry.AllChests.Remove(__instance);
      }
  }
  ```
- **Purpose**: Unregister chest when successfully opened
- **Frequency**: Once per chest, when chest is opened
- **Registry**: Removes from `EntityRegistry.AllChests` HashSet
- **🔑 KEY PATTERN**: This is the **chest opening confirmation** - fires AFTER `SuccessInteract` completes

### SpawnerChestItem_GodzillaFigure

#### Patch 1: Godzilla Chest Start
- **File**: `Features/SaveDebug.cs:43`
- **Target**: `SpawnerChestItem_GodzillaFigure.Start()`
- **Patch Type**: Prefix
- **Patch Class**: `FigureChestStartPatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(SpawnerChestItem_GodzillaFigure), 
                nameof(SpawnerChestItem_GodzillaFigure.Start))]
  static class FigureChestStartPatch
  {
      static void Prefix(SpawnerChestItem_GodzillaFigure __instance)
      {
          var go = __instance.gameObject;
          var pos = __instance.transform.position;
          var uid = __instance.UniqueID;
          var tid = __instance.TargetSpawnItemListTID;
          var scene = go.scene.name;
          Plugin.Log.LogWarning(
              $"[FigureChest] Start: scene={scene} uid={uid} " +
              $"dropListTID={tid} pos=(...) active={go.activeSelf}");
      }
  }
  ```
- **Purpose**: Debug logging when Godzilla figurine chests initialize
- **Frequency**: Once per chest, during Start()
- **Logged Properties**: UniqueID, TargetSpawnItemListTID, position, scene name

---

## MINING & BREAKABLE PATCHES

### BreakableLootObject

#### Patch 1: Ore OnEnable
- **File**: `Helpers/EntityRegistry.cs:111`
- **Target**: `BreakableLootObject.OnEnable()`
- **Patch Type**: Postfix
- **Patch Class**: `BreakableLootOnEnablePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(BreakableLootObject), nameof(BreakableLootObject.OnEnable))]
  static class BreakableLootOnEnablePatch
  {
      static void Postfix(BreakableLootObject __instance)
      {
          if (__instance == null) return;
          try
          {
              var t = __instance.lootObjectType;
              if (t == BreakableLootObject.BreakableLootObjectType.Pile
                  || t == BreakableLootObject.BreakableLootObjectType.SeaWeed)
                  return;
          }
          catch { return; }
          EntityRegistry.AllBreakableOres.Add(__instance);
      }
  }
  ```
- **Purpose**: Register ore objects (filtered to exclude Pile/SeaWeed)
- **Frequency**: Every time ore is enabled
- **Filter**: Only registers types 0-5 (actual ores), skips 6-7
- **Registry**: Adds to `EntityRegistry.AllBreakableOres` HashSet

#### Patch 2: Ore OnDie
- **File**: `Helpers/EntityRegistry.cs:130`
- **Target**: `BreakableLootObject.OnDie()`
- **Patch Type**: Postfix
- **Patch Class**: `BreakableLootOnDiePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(BreakableLootObject), nameof(BreakableLootObject.OnDie))]
  static class BreakableLootOnDiePatch
  {
      static void Postfix(BreakableLootObject __instance)
      {
          if (__instance == null) return;
          EntityRegistry.AllBreakableOres.Remove(__instance);
      }
  }
  ```
- **Purpose**: Unregister ore when destroyed
- **Frequency**: Once per ore, when it dies
- **Registry**: Removes from `EntityRegistry.AllBreakableOres` HashSet

### InteractionGimmick_Mining

#### Patch 1: Mining Node Awake
- **File**: `Helpers/EntityRegistry.cs:140`
- **Target**: `InteractionGimmick_Mining.Awake()`
- **Patch Type**: Postfix
- **Patch Class**: `MiningNodeAwakePatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(InteractionGimmick_Mining), 
                nameof(InteractionGimmick_Mining.Awake))]
  static class MiningNodeAwakePatch
  {
      static void Postfix(InteractionGimmick_Mining __instance)
      {
          if (__instance == null) return;
          EntityRegistry.AllMiningNodes.Add(__instance);
      }
  }
  ```
- **Purpose**: Register mining nodes when they awake
- **Frequency**: Once per mining node, during Awake
- **Registry**: Adds to `EntityRegistry.AllMiningNodes` HashSet

---

## CRAB TRAP PATCHES

### CrabTrapZone

#### Patch 1: Crab Trap Start
- **File**: `Helpers/EntityRegistry.cs:150`
- **Target**: `CrabTrapZone.Start()`
- **Patch Type**: Postfix
- **Patch Class**: `CrabTrapStartPatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(CrabTrapZone), nameof(CrabTrapZone.Start))]
  static class CrabTrapStartPatch
  {
      static void Postfix(CrabTrapZone __instance)
      {
          if (__instance == null) return;
          EntityRegistry.AllCrabTraps.Add(__instance);
      }
  }
  ```
- **Purpose**: Register crab trap zones when they start
- **Frequency**: Once per trap zone, during Start
- **Registry**: Adds to `EntityRegistry.AllCrabTraps` HashSet

### CrabTrapObject

#### Patch 1: Crab Trap Update (Efficiency)
- **File**: `Features/iDiverExtension.cs:1088`
- **Target**: `CrabTrapObject.Update()`
- **Patch Type**: Prefix
- **Patch Class**: `CrabTrapEfficiency_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(CrabTrapObject), nameof(CrabTrapObject.Update))]
  static class CrabTrapEfficiency_Patch
  {
      static void Prefix(CrabTrapObject __instance)
      {
          // Modifies crab trap delay based on equipment level
      }
  }
  ```
- **Purpose**: Reduce crab trap catch time based on equipment upgrades
- **Frequency**: Every frame the trap is active
- **Effect**: Speeds up crab catching based on "Crab Trap Efficiency" upgrade level

---

## SCENE & TRANSITION PATCHES

### SeahorseRacer

#### Patch 1: Racer Update
- **File**: `Features/AutoSeahorseRace.cs:135`
- **Target**: `SeahorseRacer.Update()`
- **Patch Type**: Prefix
- **Patch Class**: `SeahorseRacerUpdate_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(SeahorseRacer), nameof(SeahorseRacer.Update))]
  static class SeahorseRacerUpdate_Patch
  {
      static void Prefix(SeahorseRacer __instance)
      {
          // Auto-control seahorse racing
          CheckHotkey();
          if (enabled)
          {
              // Automatic throttle/dodge control
          }
      }
  }
  ```
- **Purpose**: Auto-play seahorse racing with optimal throttle and dodge control
- **Frequency**: Every frame during seahorse race
- **Logic**: Maintains optimal gauge level (~76%) for maximum speed, dodges obstacles automatically

### SeahorseRaceSessionPlay

#### Patch 1: Race Session Start
- **File**: `Features/AutoSeahorseRace.cs:236`
- **Target**: `SeahorseRaceSessionPlay.Start_Impl()`
- **Patch Type**: Prefix
- **Patch Class**: `SessionStart_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(SeahorseRaceSessionPlay), 
                nameof(SeahorseRaceSessionPlay.Start_Impl))]
  static class SessionStart_Patch
  {
      static void Prefix()
      {
          // Initialize auto-race when session starts
      }
  }
  ```
- **Purpose**: Reset state when race session begins
- **Frequency**: Once per race session
- **Effect**: Resets frame counters and obstacle state for new race

---

## EQUIPMENT & UPGRADE PATCHES

### DataManager

#### Patch 1: GetSubEquipment
- **File**: `Features/iDiverExtension.cs:343`
- **Target**: `DataManager.GetSubEquipment(int tid)`
- **Patch Type**: Postfix
- **Patch Class**: `GetSubEquipment_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(DataManager), nameof(DataManager.GetSubEquipment))]
  static class GetSubEquipment_Patch
  {
      static void Postfix(int tid, ref DR.SubEquipment __result)
      {
          var def = FindBySubEquipTID(tid);
          if (def != null && def.enabled)
          {
              __result = CreateSubEquipment(def, GetLevel(def));
          }
      }
  }
  ```
- **Purpose**: Inject custom equipment items into game data system
- **Frequency**: Every time equipment data is queried
- **Effect**: Replaces/adds custom equipment definitions based on level

#### Patch 2: GetIntegratedItem
- **File**: `Features/iDiverExtension.cs:378`
- **Target**: `DataManager.GetIntegratedItem(int tid)`
- **Patch Type**: Postfix
- **Patch Class**: `GetIntegratedItem_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(DataManager), nameof(DataManager.GetIntegratedItem))]
  static class GetIntegratedItem_Patch
  {
      static void Postfix(int tid, ref IntegratedItem __result)
      {
          var def = FindByIntItemTID(tid);
          if (def != null && def.enabled)
          {
              __result = CreateIntegratedItem(def, GetLevel(def));
          }
      }
  }
  ```
- **Purpose**: Inject custom integrated items into game data system
- **Frequency**: Every time integrated item data is queried
- **Effect**: Replaces/adds custom integrated item definitions based on level

### SubEquipmentManager

#### Patch 1: Manager Init
- **File**: `Features/iDiverExtension.cs:412`
- **Target**: `SubEquipmentManager.Init()`
- **Patch Type**: Postfix
- **Patch Class**: `SubEquipInit_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(SubEquipmentManager), nameof(SubEquipmentManager.Init))]
  static class SubEquipInit_Patch
  {
      static void Postfix(SubEquipmentManager __instance)
      {
          // Initialize custom equipment after manager init
      }
  }
  ```
- **Purpose**: Set up custom equipment system after manager initializes
- **Frequency**: Once on equipment manager startup

#### Patch 2: SetSubEquipUIInfo
- **File**: `Features/iDiverExtension.cs:486`
- **Target**: `SubEquipmentManager.SetSubEquipUIInfo(SubEquipmentType)`
- **Patch Type**: Postfix
- **Patch Class**: `SetSubEquipUIInfo_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(SubEquipmentManager), 
                nameof(SubEquipmentManager.SetSubEquipUIInfo))]
  static class SetSubEquipUIInfo_Patch
  {
      static void Postfix(SubEquipmentManager __instance)
      {
          // Update UI after equipment info changes
      }
  }
  ```
- **Purpose**: Refresh UI when equipment info is updated
- **Frequency**: Every time equipment UI needs refresh

### HarpoonProjectile

#### Patch 1: BuffedProjectileDamage (Property Getter)
- **File**: `Features/iDiverExtension.cs:735`
- **Target**: `HarpoonProjectile.BuffedProjectileDamage` (getter)
- **Patch Type**: Postfix
- **Patch Class**: `HarpoonDamage_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(HarpoonProjectile), 
                nameof(HarpoonProjectile.BuffedProjectileDamage), 
                MethodType.Getter)]
  static class HarpoonDamage_Patch
  {
      static void Postfix(ref int __result)
      {
          // Add bonus damage based on equipment level
          __result += bonusDamage;
      }
  }
  ```
- **Purpose**: Add damage bonus to harpoon based on equipment upgrades
- **Frequency**: Every time projectile damage is calculated
- **Note**: Uses `MethodType.Getter` to patch property getter

---

## LOBBY & UI PATCHES

### LobbyEquipUpgradeScrollPanel

#### Patch 1: AddCellData
- **File**: `Features/iDiverExtension.cs:462`
- **Target**: `LobbyEquipUpgradeScrollPanel.AddCellData()`
- **Patch Type**: Postfix
- **Patch Class**: `AddCellData_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(LobbyEquipUpgradeScrollPanel), 
                nameof(LobbyEquipUpgradeScrollPanel.AddCellData))]
  static class AddCellData_Patch
  {
      static void Postfix(LobbyEquipUpgradeScrollPanel __instance)
      {
          // Add custom equipment cells to scroll panel
      }
  }
  ```
- **Purpose**: Inject custom equipment into the upgrade scroll panel
- **Frequency**: When scroll panel populates cells

### LobbyUpgradeEquipScrollCell

#### Patch 1: SetUIData
- **File**: `Features/iDiverExtension.cs:539`
- **Target**: `LobbyUpgradeEquipScrollCell.SetUIData(LobbyUpgradeEquipScrollCell, SubEquipmentType)`
- **Patch Type**: Postfix
- **Patch Class**: `SetUIData_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(LobbyUpgradeEquipScrollCell), 
                nameof(LobbyUpgradeEquipScrollCell.SetUIData))]
  static class SetUIData_Patch
  {
      static void Postfix(LobbyUpgradeEquipScrollCell __instance)
      {
          // Update cell UI with custom equipment data
      }
  }
  ```
- **Purpose**: Update individual cell UI for custom equipment
- **Frequency**: When cell data is set

### IDiverItemDetailPanel

#### Patch 1: SetItemDetailData
- **File**: `Features/iDiverExtension.cs:576`
- **Target**: `IDiverItemDetailPanel.SetItemDetailData()`
- **Patch Type**: Prefix + Postfix
- **Patch Class**: `SetItemDetailData_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(IDiverItemDetailPanel), 
                nameof(IDiverItemDetailPanel.SetItemDetailData))]
  static class SetItemDetailData_Patch
  {
      static void Prefix(IDiverItemDetailPanel __instance)
      {
          // Store original UI state
      }
      
      static void Postfix(IDiverItemDetailPanel __instance)
      {
          // Override UI with custom equipment details
      }
  }
  ```
- **Purpose**: Override item detail panel display for custom equipment
- **Frequency**: When detail panel shows item info
- **Technique**: Disables UIDataText, directly modifies TMP_Text

---

## CASINO/BETTING PATCHES

### BettingUI

#### Patch 1: RefreshCost
- **File**: `Features/BettingExpansion.cs:47`
- **Target**: `BettingUI.RefreshCost()`
- **Patch Type**: Prefix
- **Patch Class**: `RefreshCost_Patch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(BettingUI), nameof(BettingUI.RefreshCost))]
  static class RefreshCost_Patch
  {
      static void Prefix(BettingUI __instance)
      {
          if (!_enabled.Value) return;
          
          var costs = __instance.bettingCosts;
          if (costs == null) return;
          if (IsExpandedCosts(costs)) return;
          if (!IsDefaultCosts(costs)) return;
          
          var newCosts = new Il2CppStructArray<int>(6)
          {
              [0] = 10,
              [1] = 50,
              [2] = 100,
              [3] = 500,
              [4] = 1000,
              [5] = 5000
          };
          
          int idx = __instance._bettingIndex;
          if (idx >= 6) idx = 0;
          
          __instance.SetBettingCosts(newCosts, idx, false);
      }
  }
  ```
- **Purpose**: Expand casino betting options from [10, 50, 100] to [10, 50, 100, 500, 1000, 5000]
- **Frequency**: Every time betting UI refreshes costs
- **Pattern**: Uses `Il2CppStructArray<int>` for game array interop
- **Safety**: Only modifies default costs, preserves custom betting structures

---

## SAVE DATA PATCHES

### SaveData

#### Patch 1: HaveBeenLooted
- **File**: `Features/SaveDebug.cs:72`
- **Target**: `SaveData.HaveBeenLooted(int id)`
- **Patch Type**: Postfix
- **Patch Class**: `HaveBeenLootedPatch`
- **Code**:
  ```csharp
  [HarmonyPatch(typeof(SaveData), nameof(SaveData.HaveBeenLooted))]
  static class HaveBeenLootedPatch
  {
      static void Postfix(int id, bool __result)
      {
          if (id >= 1010301 && id <= 1010320)
          {
              Plugin.Log.LogInfo($"[FigureChest] HaveBeenLooted({id}) = {__result}");
          }
      }
  }
  ```
- **Purpose**: Debug logging for figurine loot checks
- **Frequency**: Every time game checks if an item has been looted
- **Scope**: Only logs figurine item IDs (1010301-1010320)

---

## SUMMARY TABLE

| Class Name | Method | Patch Type | File | Purpose |
|---|---|---|---|---|
| PlayerCharacter | Update() | Postfix | AutoPickup.cs:200 | Auto-pickup nearby items |
| PlayerCharacter | Update() | Prefix/Postfix | iDiverExtension.cs:757 | Apply equipment effects |
| PlayerCharacter | Update() | Postfix | FishDensity.cs:152 | Spawn extra fish |
| PlayerCharacter | Update() | Postfix | EntityRegistry.cs:164 | Purge dead objects |
| PlayerCharacter | Update() | Postfix | SaveDebug.cs:88 | Debug Godzilla chests |
| FishInteractionBody | Awake() | Postfix | EntityRegistry.cs:100 | Register fish |
| PickupInstanceItem | OnEnable() | Postfix | EntityRegistry.cs:56 | Register items |
| PickupInstanceItem | OnDisable() | Postfix | EntityRegistry.cs:67 | Unregister items |
| InstanceItemChest | OnEnable() | Postfix | EntityRegistry.cs:78 | Register chests |
| InstanceItemChest | SuccessInteract() | Postfix | EntityRegistry.cs:89 | Unregister chests (opened) |
| SpawnerChestItem_GodzillaFigure | Start() | Prefix | SaveDebug.cs:43 | Debug chest spawn |
| BreakableLootObject | OnEnable() | Postfix | EntityRegistry.cs:111 | Register ores |
| BreakableLootObject | OnDie() | Postfix | EntityRegistry.cs:130 | Unregister ores |
| InteractionGimmick_Mining | Awake() | Postfix | EntityRegistry.cs:140 | Register mining nodes |
| CrabTrapZone | Start() | Postfix | EntityRegistry.cs:150 | Register crab traps |
| CrabTrapObject | Update() | Prefix | iDiverExtension.cs:1088 | Speed up trap catching |
| SeahorseRacer | Update() | Prefix | AutoSeahorseRace.cs:135 | Auto-control racer |
| SeahorseRaceSessionPlay | Start_Impl() | Prefix | AutoSeahorseRace.cs:236 | Initialize race |
| DataManager | GetSubEquipment() | Postfix | iDiverExtension.cs:343 | Inject equipment |
| DataManager | GetIntegratedItem() | Postfix | iDiverExtension.cs:378 | Inject items |
| SubEquipmentManager | Init() | Postfix | iDiverExtension.cs:412 | Initialize equipment |
| SubEquipmentManager | SetSubEquipUIInfo() | Postfix | iDiverExtension.cs:486 | Update equipment UI |
| HarpoonProjectile | BuffedProjectileDamage | Postfix | iDiverExtension.cs:735 | Bonus harpoon damage |
| LobbyEquipUpgradeScrollPanel | AddCellData() | Postfix | iDiverExtension.cs:462 | Add equipment cells |
| LobbyUpgradeEquipScrollCell | SetUIData() | Postfix | iDiverExtension.cs:539 | Update cell UI |
| IDiverItemDetailPanel | SetItemDetailData() | Prefix/Postfix | iDiverExtension.cs:576 | Override detail panel |
| BettingUI | RefreshCost() | Prefix | BettingExpansion.cs:47 | Expand betting options |
| SaveData | HaveBeenLooted() | Postfix | SaveDebug.cs:72 | Debug loot checks |

---

## PATCH EXECUTION ORDER NOTES

### Multiple Patches on Same Method

When multiple patches target the same method, execution order is:

**PlayerCharacter.Update()**:
1. AllEffects_Patch **Prefix** (iDiverExtension.cs:757)
2. [Original Method]
3. AutoPickupPatch **Postfix** (AutoPickup.cs:200)
4. AllEffects_Patch **Postfix** (iDiverExtension.cs:757)
5. FishDensityScanPatch **Postfix** (FishDensity.cs:152)
6. EntityRegistryPurgePatch **Postfix** (EntityRegistry.cs:164)
7. PeriodicScanPatch **Postfix** (SaveDebug.cs:88)

⚠️ **Order of Postfix patches** depends on load order in `_harmony.PatchAll()`. If order matters, use Harmony priorities.

---

## HARMONY CONFIGURATION

**Harmony Instance ID**: `com.davediver.expansion` (from Plugin.cs)

**Patch Application**:
```csharp
_harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
_harmony.PatchAll();
```

This auto-discovers all classes with `[HarmonyPatch]` attributes and applies them.

---

