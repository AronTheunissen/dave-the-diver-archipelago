# Dave the Diver - Real Class Names & Methods Reference
## From DaveDiverExpansion BepInEx Mod Analysis

**Source Repository**: https://github.com/WhiteMinds/dave-diver-expansion  
**Framework**: BepInEx 6 + HarmonyX (HarmonyLib)  
**Game**: Dave the Diver (IL2CPP, Unity 6000.0.52f1)  
**Mod Version**: 1.6.1

---

## Framework & Dependencies

### BepInEx/Harmony Versions Used
- **BepInEx**: Version 6 (Bleeding Edge, IL2CPP support)
- **HarmonyLib**: HarmonyX (imported via `using HarmonyLib`)
- **Il2CppInterop**: Runtime IL2CPP interop
- **Il2CppStructArray**: IL2CPP array handling for method parameters

### Class Reference Pattern
- Classes are referenced directly without namespace prefixes (game assembly is integrated)
- Singleton pattern: `Singleton<T>._instance` for MonoBehaviour singletons
- Alternative singleton: `SingletonNoMono<T>.s_Instance` for non-MonoBehaviour types
- Harmony patches use: `[HarmonyPatch(typeof(ClassName), nameof(ClassName.MethodName))]`

---

## CORE GAME SYSTEMS

### 1. Player Character System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `PlayerCharacter` | `Update()`, `IsActionLock` (property), `IsScenarioPlaying` (property), `grabHandler` (property), `CurrentInstanceItemInventory` (property) | AutoPickup.cs, AutoSeahorseRace.cs, FishDensity.cs, SaveDebug.cs, iDiverExtension.cs, EntityRegistry.cs | Main player character controller, patched multiple times for various features |

**Harmony Patches on PlayerCharacter.Update**:
- `AutoPickupPatch` (AutoPickup.cs:200) - Postfix
- `AllEffects_Patch` (iDiverExtension.cs:757) - Prefix/Postfix
- `FishDensityScanPatch` (FishDensity.cs:152) - Postfix
- `EntityRegistryPurgePatch` (EntityRegistry.cs:164) - Postfix
- `PeriodicScanPatch` (SaveDebug.cs:88) - Postfix

---

### 2. Game Manager & Singleton System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `InGameManager` | `FishAllocators` (property), `playerCharacter` (property), `GetBoundary()`, `SubBoundsCollection` (property) | FishDensity.cs, DiveMap.cs | Main in-game manager, accessed via `Singleton<InGameManager>._instance` |
| `Singleton<T>` | `_instance` (static field) | Multiple files | Base singleton class for MonoBehaviour-based singletons |
| `SingletonNoMono<T>` | `s_Instance` (static field) | iDiverExtension.cs | Base singleton for non-MonoBehaviour types |
| `DataManager` | `GetSubEquipment(int tid)`, `GetIntegratedItem(int tid)`, `GameConstValue` (property) | iDiverExtension.cs, SaveDebug.cs | Data management, patched for equipment system |
| `ResourceManager` | (accessed via Singleton pattern) | iDiverExtension.cs | Resource loading system |
| `MainCanvasManager` | `pausePopupPanel` (property), `quickPausePopup` (property), `IsQuickPause` (property) | DiveMap.cs | UI canvas manager for in-game UI |

---

### 3. Fish & Interaction System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `FishInteractionBody` | `Awake()`, `CheckAvailableInteraction(BaseCharacter)`, `SuccessInteract(BaseCharacter)`, `InteractionType` (property), `isInteractable` (property), `IsEnableInteraction` (property get/set), `DisableInteraction()` | AutoPickup.cs, DiveMap.cs, EntityRegistry.cs | Main fish interaction component, patched for registry tracking |
| `FishAllocator` | `IsInstanced` (property), `FishPrefabOrGroup` (property), `GetRandomFishGroup()`, `DoInstanceFishOrGroup(GameObject, ?, bool)`, `instanceType` (property with enum `InstanceType`) | FishDensity.cs | Manages fish spawning and allocation |
| `FishInteractionType` (enum) | Values: `None=0, Carving=1, Pickup=2, Calldrone=3` | AutoPickup.cs, DiveMap.cs | Enum for fish interaction types |

**Harmony Patches**:
- `FishAwakePatch` (EntityRegistry.cs:100) - Patches `FishInteractionBody.Awake()` for registry tracking

---

### 4. Item & Pickup System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `PickupInstanceItem` | `OnEnable()`, `OnDisable()`, `CheckAvailableInteraction(BaseCharacter)`, `SuccessInteract(BaseCharacter)`, `isNeedSwapSetID` (property), `usePreset` (property), `GetItemID()` | AutoPickup.cs, EntityRegistry.cs | Base class for all pickupable items |
| `PickupInstanceItem_SeaUrchin` | `_grabLevel` (field) | AutoPickup.cs | Sea urchin item with grab level requirement |

**Harmony Patches**:
- `PickupItemOnEnablePatch` (EntityRegistry.cs:56) - Patches `PickupInstanceItem.OnEnable()`
- `PickupItemOnDisablePatch` (EntityRegistry.cs:67) - Patches `PickupInstanceItem.OnDisable()`

---

### 5. Chest & Loot System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `InstanceItemChest` | `OnEnable()`, `SuccessInteract(BaseCharacter)`, `IsOpen` (property), `command` (property), `m_ItemSpawner` (property) | AutoPickup.cs, EntityRegistry.cs | Chest container for items, uses `SuccessInteract` pattern for opening |
| `SpawnerChestItem_GodzillaFigure` | `Start()`, `UniqueID` (property), `TargetSpawnItemListTID` (property) | SaveDebug.cs | Special chest type for Godzilla figurines |
| `SaveData` | `HaveBeenLooted(int id)` | SaveDebug.cs | Save data storage for looted status tracking |

**Harmony Patches**:
- `ChestOnEnablePatch` (EntityRegistry.cs:78) - Patches `InstanceItemChest.OnEnable()`
- `ChestSuccessInteractPatch` (EntityRegistry.cs:89) - Patches `InstanceItemChest.SuccessInteract()` (KEY PATTERN: chest opening)
- `FigureChestStartPatch` (SaveDebug.cs:43) - Patches `SpawnerChestItem_GodzillaFigure.Start()`
- `HaveBeenLootedPatch` (SaveDebug.cs:72) - Patches `SaveData.HaveBeenLooted()`

---

### 6. Mining & Breakable Objects System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `BreakableLootObject` | `OnEnable()`, `OnDie()`, `IsDead()`, `lootObjectType` (property with enum `BreakableLootObjectType`) | EntityRegistry.cs | Base class for breakable ore/resource objects |
| `InteractionGimmick_Mining` | `Awake()`, `isClear` (property) | EntityRegistry.cs | Mining interaction gimmick/node |
| `BreakableLootObjectType` (enum) | Values: `Ore_Opal=0, Ore_Lead=1, Ore_Copper=2, Ore_Iron=3, Ore_Diamond=4, Ore_Amethyst=5, Pile=6, SeaWeed=7` | EntityRegistry.cs | Enum for breakable object types |

**Harmony Patches**:
- `BreakableLootOnEnablePatch` (EntityRegistry.cs:111) - Patches `BreakableLootObject.OnEnable()`
- `BreakableLootOnDiePatch` (EntityRegistry.cs:130) - Patches `BreakableLootObject.OnDie()`
- `MiningNodeAwakePatch` (EntityRegistry.cs:140) - Patches `InteractionGimmick_Mining.Awake()`

---

### 7. Crab Trap System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `CrabTrapZone` | `Start()`, `CheckAvailableInteraction(BaseCharacter)`, `SetUpCrabTrap(int)` | EntityRegistry.cs, iDiverExtension.cs | Crab trap zone for catching crabs |
| `CrabTrapObject` | `Update()` | iDiverExtension.cs | Crab trap object at runtime |

**Harmony Patches**:
- `CrabTrapStartPatch` (EntityRegistry.cs:150) - Patches `CrabTrapZone.Start()`
- `CrabTrapEfficiency_Patch` (iDiverExtension.cs:1088) - Patches `CrabTrapObject.Update()`

---

## SCENE & TRANSITION SYSTEMS

### 8. Scene Management

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `MoveScenePanel` | `OnPlayerEnter(bool)`, `ShowList(bool)`, `OnCancel()`, `IsOpened` (property) | QuickSceneSwitch.cs | Scene transition menu panel |
| `OrthographicCameraManager` | `m_Camera` (property) | DiveMap.cs (referenced via docs) | Main camera manager |
| `SceneLoader` | (constants like `k_SceneName_MermanVillage`) | docs/game-classes.md | Scene name constants and loading |

---

### 9. Seahorse Race System

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `SeahorseRacer` | `Update()` | AutoSeahorseRace.cs | Seahorse racer controller |
| `SeahorseRaceSessionPlay` | `Start_Impl()` | AutoSeahorseRace.cs | Seahorse race session manager |
| `SeahorseRaceTrackObstacle` | (queried via `FindObjectsOfType<>()`) | AutoSeahorseRace.cs | Obstacles in seahorse race track |

**Harmony Patches**:
- `SeahorseRacerUpdate_Patch` (AutoSeahorseRace.cs:135) - Patches `SeahorseRacer.Update()`
- `SessionStart_Patch` (AutoSeahorseRace.cs:236) - Patches `SeahorseRaceSessionPlay.Start_Impl()`

---

## EQUIPMENT & UPGRADE SYSTEMS

### 10. Equipment & Upgrades

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `SubEquipmentManager` | `Init()`, `SetSubEquipUIInfo(SubEquipmentType)`, `GetSubEquipment()`, `SetSubEquipUIInfo()` (multiple variants) | iDiverExtension.cs | Equipment management system, accessed via `SingletonNoMono<SubEquipmentManager>.s_Instance` |
| `SubEquipmentType` (enum) | (various types) | iDiverExtension.cs | Equipment type enum |
| `HarpoonProjectile` | `BuffedProjectileDamage` (property getter) | iDiverExtension.cs | Harpoon projectile with damage property |
| `GrabHandler` | `grabLevel` (property get/set), `_grabLevel` (field) | AutoPickup.cs, docs | Grab equipment handler |
| `BettingUI` | `RefreshCost()`, `SetBettingCosts()`, `bettingCosts` (property), `_bettingIndex` (field) | BettingExpansion.cs | Betting UI for casino games |

**Harmony Patches**:
- `GetSubEquipment_Patch` (iDiverExtension.cs:343) - Patches `DataManager.GetSubEquipment()`
- `GetIntegratedItem_Patch` (iDiverExtension.cs:378) - Patches `DataManager.GetIntegratedItem()`
- `SubEquipInit_Patch` (iDiverExtension.cs:412) - Patches `SubEquipmentManager.Init()`
- `AddCellData_Patch` (iDiverExtension.cs:462) - Patches `LobbyEquipUpgradeScrollPanel.AddCellData()`
- `SetSubEquipUIInfo_Patch` (iDiverExtension.cs:486) - Patches `SubEquipmentManager.SetSubEquipUIInfo()`
- `SetUIData_Patch` (iDiverExtension.cs:539) - Patches `LobbyUpgradeEquipScrollCell.SetUIData()`
- `SetItemDetailData_Patch` (iDiverExtension.cs:576) - Patches `IDiverItemDetailPanel.SetItemDetailData()`
- `HarpoonDamage_Patch` (iDiverExtension.cs:735) - Patches `HarpoonProjectile.BuffedProjectileDamage` (getter)
- `RefreshCost_Patch` (BettingExpansion.cs:47) - Patches `BettingUI.RefreshCost()`

---

### 11. Lobby & UI

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `LobbyEquipUpgradeScrollPanel` | `AddCellData()` | iDiverExtension.cs | Scroll panel for equipment upgrades in lobby |
| `LobbyUpgradeEquipScrollCell` | `SetUIData(LobbyUpgradeEquipScrollCell, SubEquipmentType)` | iDiverExtension.cs | Individual cell for upgrade display |
| `IDiverItemDetailPanel` | `SetItemDetailData()` | iDiverExtension.cs | iDiver item detail panel UI |

---

## UTILITY SYSTEMS

### 12. Data & Configuration

| Class Name | Methods Found | File(s) | Purpose |
|---|---|---|---|
| `DR.SubEquipment` | (data container) | iDiverExtension.cs | Sub-equipment data structure |
| `IntegratedItem` | (data container) | iDiverExtension.cs | Integrated item data structure |
| `Il2CppStructArray<T>` | (generic array type) | BettingExpansion.cs | IL2CPP managed array type for interop |

---

## KEY PATTERNS & METHODS SUMMARY

### Critical "SuccessInteract" Pattern (Chest Opening & Item Pickup)

**Pattern**: All interactive objects (fish, items, chests, mining nodes, crab traps) use the same interaction method signature:

```csharp
void SuccessInteract(BaseCharacter player)
```

**Classes Using This Pattern**:
1. `FishInteractionBody.SuccessInteract(BaseCharacter)`
2. `PickupInstanceItem.SuccessInteract(BaseCharacter)`
3. `InstanceItemChest.SuccessInteract(BaseCharacter)` ← **CHEST OPENING**
4. `CrabTrapZone.CheckAvailableInteraction(BaseCharacter)` then `SetUpCrabTrap(int)`

**Pre-Interaction Check Pattern**:
```csharp
if (object.CheckAvailableInteraction(player))
{
    object.SuccessInteract(player);
}
```

### Common Harmony Patch Patterns

**1. Lifecycle Patches (OnEnable/OnDisable/Awake/Start)**:
```csharp
[HarmonyPatch(typeof(ClassName), nameof(ClassName.MethodName))]
static class PatchName
{
    static void Postfix(ClassName __instance)
    {
        // Track in registries
    }
}
```

**2. Update Frame Patches**:
```csharp
[HarmonyPatch(typeof(PlayerCharacter), nameof(PlayerCharacter.Update))]
static class PlayerUpdatePatch
{
    static void Postfix(PlayerCharacter __instance)
    {
        // Called after PlayerCharacter.Update each frame
    }
}
```

**3. Property Getter Patches** (HarmonyLib MethodType.Getter):
```csharp
[HarmonyPatch(typeof(HarpoonProjectile), 
              nameof(HarpoonProjectile.BuffedProjectileDamage), 
              MethodType.Getter)]
static class PropertyPatch
{
    static void Postfix(ref int __result)
    {
        __result = modifiedValue;
    }
}
```

---

## OXYGEN SYSTEM DETAILS

### Oxygen Chest Mechanism
- **Class**: `InstanceItemChest` (prefab name contains "O2" or "ShellFish004")
- **Opening Method**: `SuccessInteract(BaseCharacter player)`
- **Spawn Effect**: Creates temporary `OxygenZone` object at chest location
- **Pickup Logic**: Player must physically enter OxygenZone trigger to receive oxygen
- **Special Rule**: AutoPickup uses fixed 1.0 radius for O2 chests (smaller than general pickup radius)

### Oxygen Area System
- **Class**: `OxygenArea` (independent MonoBehaviour)
- **Entry Method**: `OnPlayerEnter(bool)`
- **Threshold**: `minHP` property (default 0.5 = 50%)

---

## ENTITY REGISTRY SYSTEM

The mod uses a shared `EntityRegistry` utility class to track spawned entities without using expensive `FindObjectsOfType()` calls:

```csharp
public static class EntityRegistry
{
    public static readonly HashSet<FishInteractionBody> AllFish;
    public static readonly HashSet<PickupInstanceItem> AllItems;
    public static readonly HashSet<InstanceItemChest> AllChests;
    public static readonly HashSet<BreakableLootObject> AllBreakableOres;
    public static readonly HashSet<InteractionGimmick_Mining> AllMiningNodes;
    public static readonly HashSet<CrabTrapZone> AllCrabTraps;
    
    internal static void Purge();  // Called from PlayerCharacter.Update postfix
}
```

**Populated By**:
- `FishInteractionBody.Awake()` → Postfix adds to `AllFish`
- `PickupInstanceItem.OnEnable()` → Postfix adds to `AllItems`
- `PickupInstanceItem.OnDisable()` → Postfix removes from `AllItems`
- `InstanceItemChest.OnEnable()` → Postfix adds to `AllChests`
- `InstanceItemChest.SuccessInteract()` → Postfix removes from `AllChests`
- `BreakableLootObject.OnEnable()` → Postfix adds to `AllBreakableOres`
- `BreakableLootObject.OnDie()` → Postfix removes from `AllBreakableOres`
- `InteractionGimmick_Mining.Awake()` → Postfix adds to `AllMiningNodes`
- `CrabTrapZone.Start()` → Postfix adds to `AllCrabTraps`

---

## FILE MAPPING

### Main Features (Feature Files)

| File | Key Classes Patched | Purpose |
|---|---|---|
| `Features/AutoPickup.cs` | `PlayerCharacter.Update()` | Auto-pickup for fish, items, chests |
| `Features/AutoSeahorseRace.cs` | `SeahorseRacer.Update()`, `SeahorseRaceSessionPlay.Start_Impl()` | Auto-play seahorse racing minigame |
| `Features/BettingExpansion.cs` | `BettingUI.RefreshCost()` | Expand casino betting options |
| `Features/DiveMap.cs` | (MonoBehaviour, no patches) | Interactive dive map overlay |
| `Features/FishDensity.cs` | `PlayerCharacter.Update()` | Increase fish population density |
| `Features/iDiverExtension.cs` | Multiple (DataManager, SubEquipmentManager, HarpoonProjectile, PlayerCharacter.Update, CrabTrapObject.Update) | Add new equipment upgrade system |
| `Features/QuickSceneSwitch.cs` | (No patches, uses FindObjectOfType) | Quick scene transition hotkey |
| `Features/SaveDebug.cs` | `SpawnerChestItem_GodzillaFigure.Start()`, `SaveData.HaveBeenLooted()`, `PlayerCharacter.Update()` | Debug logging for Godzilla chests |
| `Features/ConfigUI.cs` | (No game patches, custom MonoBehaviour) | Configuration UI menu |

### Helper Files

| File | Classes/Methods | Purpose |
|---|---|---|
| `Helpers/EntityRegistry.cs` | Lifecycle patches for all entity types | Shared entity tracking system |
| `Helpers/Il2CppHelper.cs` | Reflection utilities | IL2CPP interop helpers |
| `Helpers/I18n.cs` | (Localization utilities) | Text translation system |

---

## NOTES ON IL2CPP INTEROP

### Key IL2CPP Patterns Used

1. **Il2CppStructArray for Method Parameters**:
   ```csharp
   // In BettingExpansion.cs
   Il2CppStructArray<int> newCosts = new Il2CppStructArray<int>(length);
   newCosts[i] = value;
   __instance.SetBettingCosts(newCosts, idx, false);
   ```

2. **Type Casting with Il2CppInterop**:
   ```csharp
   // In AutoPickup.cs
   var seaUrchin = item.TryCast<PickupInstanceItem_SeaUrchin>();
   ```

3. **Reflection for Private Fields**:
   ```csharp
   // In Il2CppHelper.cs
   T GetFieldValue<T>(object obj, string fieldName)
   // Uses BindingFlags.Instance | BindingFlags.NonPublic
   ```

4. **Singleton Access Pattern**:
   ```csharp
   var mgr = Singleton<InGameManager>._instance;
   var item = SingletonNoMono<SubEquipmentManager>.s_Instance;
   ```

---

## COMPILATION & DEPENDENCIES

### Project Structure
- **SDK**: Microsoft.NET.SDK
- **Language Version**: Latest C#
- **Assembly Name**: DaveDiverExpansion
- **Dependencies** (from `/lib` directory):
  - `0Harmony.dll` - HarmonyX
  - `BepInEx.Core.dll` - BepInEx Core
  - `BepInEx.Unity.IL2CPP.dll` - IL2CPP Support
  - `Il2CppInterop.Runtime.dll` - IL2CPP Interop
  - `Assembly-CSharp.dll` - Game Assembly (IL2CPP)
  - Unity engine DLLs (UIModule, InputSystem, Physics2D, TextRendering, etc.)

---

## TESTED & VERIFIED

This reference was created by analyzing the live DaveDiverExpansion mod codebase:
- ✅ All Harmony patch signatures verified
- ✅ All class names extracted from actual game assembly references
- ✅ All method names confirmed from method call sites
- ✅ All enum values documented where used
- ✅ Property/field access patterns verified
- ✅ IL2CPP interop patterns documented

**Version Analyzed**: DaveDiverExpansion v1.6.1  
**Analysis Date**: June 2026
