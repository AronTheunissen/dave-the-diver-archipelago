# Dave the Diver - Quick Reference for Game Classes

## System Categories Quick Lookup

### 🎮 PLAYER & CHARACTER
- **PlayerCharacter** - Main player class
  - Methods: `Update()`, `IsActionLock`, `IsScenarioPlaying`
  - Properties: `grabHandler`, `CurrentInstanceItemInventory`
  - ⚠️ Most patched class in the mod

### 🐟 FISH & INTERACTION  
- **FishInteractionBody** - All fish components
  - Methods: `Awake()`, `CheckAvailableInteraction(BaseCharacter)`, `SuccessInteract(BaseCharacter)`
  - Properties: `InteractionType`, `isInteractable`, `IsEnableInteraction`
  - Enum: `FishInteractionType` (None=0, Carving=1, **Pickup=2**, Calldrone=3)

- **FishAllocator** - Fish spawner
  - Methods: `DoInstanceFishOrGroup()`, `GetRandomFishGroup()`
  - Properties: `IsInstanced`, `FishPrefabOrGroup`, `instanceType`

### 📦 ITEMS & PICKUP
- **PickupInstanceItem** - All pickupable items base class
  - Methods: `OnEnable()`, `OnDisable()`, `CheckAvailableInteraction()`, `SuccessInteract()`
  - Properties: `isNeedSwapSetID`, `usePreset`, `GetItemID()`

- **PickupInstanceItem_SeaUrchin** - Sea urchin (special case)
  - Field: `_grabLevel` (grab level requirement)

### 🎁 CHESTS (KEY SYSTEM)
- **InstanceItemChest** - Treasure chest
  - Methods: `OnEnable()`, **`SuccessInteract(BaseCharacter)`** ← CHEST OPENING
  - Properties: `IsOpen`, `command`, `m_ItemSpawner`

- **SpawnerChestItem_GodzillaFigure** - Godzilla figurine chest
  - Methods: `Start()`
  - Properties: `UniqueID`, `TargetSpawnItemListTID`

### ⛏️ MINING & BREAKABLE
- **BreakableLootObject** - Ore/rocks/seaweed
  - Methods: `OnEnable()`, `OnDie()`, `IsDead()`
  - Properties: `lootObjectType` (enum with 8 types)

- **InteractionGimmick_Mining** - Mining node
  - Methods: `Awake()`
  - Properties: `isClear`

### 🦀 CRAB TRAPS
- **CrabTrapZone** - Crab trap area
  - Methods: `Start()`, `CheckAvailableInteraction()`, `SetUpCrabTrap()`

- **CrabTrapObject** - Active crab trap
  - Methods: `Update()`

### 🎪 SCENES & TRANSITIONS
- **MoveScenePanel** - Scene change menu
  - Methods: `OnPlayerEnter(bool)`, `ShowList()`, `OnCancel()`
  - Properties: `IsOpened`

- **OrthographicCameraManager** - Camera control
  - Properties: `m_Camera`

### 🏇 SEAHORSE RACING
- **SeahorseRacer** - Race vehicle
  - Methods: `Update()`

- **SeahorseRaceSessionPlay** - Race session
  - Methods: `Start_Impl()`

- **SeahorseRaceTrackObstacle** - Race obstacles
  - (Queried via FindObjectsOfType)

### ⚙️ EQUIPMENT & UPGRADES
- **SubEquipmentManager** - Equipment management
  - Methods: `Init()`, `SetSubEquipUIInfo()`
  - Access: `SingletonNoMono<SubEquipmentManager>.s_Instance`

- **HarpoonProjectile** - Harpoon weapon
  - Properties: `BuffedProjectileDamage` (getter)

- **GrabHandler** - Grab equipment (gloves)
  - Properties: `grabLevel` (get/set)
  - Fields: `_grabLevel`

- **BettingUI** - Casino betting
  - Methods: `RefreshCost()`, `SetBettingCosts()`
  - Properties: `bettingCosts`

### 🎮 GAME MANAGERS
- **InGameManager** - Main game manager
  - Properties: `playerCharacter`, `FishAllocators`, `SubBoundsCollection`
  - Methods: `GetBoundary()`
  - Access: `Singleton<InGameManager>._instance`

- **DataManager** - Data loading
  - Methods: `GetSubEquipment()`, `GetIntegratedItem()`, `GetText()`
  - Access: `Singleton<DataManager>._instance`

- **MainCanvasManager** - UI canvas
  - Properties: `pausePopupPanel`, `quickPausePopup`, `IsQuickPause`

- **ResourceManager** - Resource loading
  - Access: `Singleton<ResourceManager>._instance`

---

## 🔑 CRITICAL INTERACTION PATTERNS

### THE UNIVERSAL INTERACTION PATTERN

ALL interactive objects use this two-step pattern:

```csharp
// Step 1: Check if interaction is available
if (interactiveObject.CheckAvailableInteraction(player))
{
    // Step 2: Trigger the interaction
    interactiveObject.SuccessInteract(player);
}
```

### Classes That Use This Pattern:
1. ✅ `FishInteractionBody.SuccessInteract(BaseCharacter)`
2. ✅ `PickupInstanceItem.SuccessInteract(BaseCharacter)`
3. ✅ `InstanceItemChest.SuccessInteract(BaseCharacter)` ← **CHEST OPENING**
4. ✅ `CrabTrapZone.CheckAvailableInteraction()` + `SetUpCrabTrap()`

---

## 🔧 HARMONY PATCH LOCATIONS

### By Feature:

**AutoPickup.cs**
```
PlayerCharacter.Update() [Postfix] → TryPickupNearby()
```

**AutoSeahorseRace.cs**
```
SeahorseRacer.Update() [Prefix]
SeahorseRaceSessionPlay.Start_Impl() [Prefix]
```

**BettingExpansion.cs**
```
BettingUI.RefreshCost() [Prefix]
```

**FishDensity.cs**
```
PlayerCharacter.Update() [Postfix] → ScanAllocators()
```

**iDiverExtension.cs**
```
DataManager.GetSubEquipment() [Postfix]
DataManager.GetIntegratedItem() [Postfix]
SubEquipmentManager.Init() [Postfix]
SubEquipmentManager.SetSubEquipUIInfo() [Postfix]
LobbyEquipUpgradeScrollPanel.AddCellData() [Postfix]
LobbyUpgradeEquipScrollCell.SetUIData() [Postfix]
IDiverItemDetailPanel.SetItemDetailData() [Prefix/Postfix]
HarpoonProjectile.BuffedProjectileDamage [Postfix] (getter)
PlayerCharacter.Update() [Prefix/Postfix]
CrabTrapObject.Update() [Prefix]
```

**SaveDebug.cs**
```
SpawnerChestItem_GodzillaFigure.Start() [Prefix]
SaveData.HaveBeenLooted() [Postfix]
PlayerCharacter.Update() [Postfix]
```

**EntityRegistry.cs** (Helper)
```
PickupInstanceItem.OnEnable() [Postfix]
PickupInstanceItem.OnDisable() [Postfix]
InstanceItemChest.OnEnable() [Postfix]
InstanceItemChest.SuccessInteract() [Postfix]
FishInteractionBody.Awake() [Postfix]
BreakableLootObject.OnEnable() [Postfix]
BreakableLootObject.OnDie() [Postfix]
InteractionGimmick_Mining.Awake() [Postfix]
CrabTrapZone.Start() [Postfix]
PlayerCharacter.Update() [Postfix]
```

---

## 📋 ENUM VALUES

### FishInteractionType
```
None = 0
Carving = 1
Pickup = 2 ← Most common fish
Calldrone = 3
```

### BreakableLootObjectType
```
Ore_Opal = 0
Ore_Lead = 1
Ore_Copper = 2
Ore_Iron = 3
Ore_Diamond = 4
Ore_Amethyst = 5
Pile = 6 (filtered out)
SeaWeed = 7 (filtered out)
```

### SubHelperType
```
None = 0
Drone = 1
Booster = 2
Net = 3
Spotlight = 4
Cargo = 5
Harpoon = 6
Diver = 7
Oxygen = 8
Sensor = 9
BoosterMk2 = 10
ChargeBattery = 11
Explosive = 12
```

### DR.Save.Languages
```
Chinese = 6
ChineseTraditional = 41
English = 10
```

---

## 🧬 SINGLETON ACCESS PATTERNS

### MonoBehaviour Singletons
```csharp
var instance = Singleton<T>._instance;

// Examples:
var inGameMgr = Singleton<InGameManager>._instance;
var dataMgr = Singleton<DataManager>._instance;
var mainCanvas = Singleton<MainCanvasManager>._instance;
var cameraMgr = Singleton<OrthographicCameraManager>._instance;
```

### Non-MonoBehaviour Singletons
```csharp
var instance = SingletonNoMono<T>.s_Instance;

// Example:
var equipMgr = SingletonNoMono<SubEquipmentManager>.s_Instance;
```

---

## 🎯 COMMON PROPERTY ACCESS PATTERNS

### Player Properties
```csharp
player.IsActionLock            // bool - action locked state
player.IsScenarioPlaying       // bool - cutscene playing
player.grabHandler             // GrabHandler
player.grabHandler.grabLevel   // int - current grab level
player.CurrentInstanceItemInventory  // InstanceItemInventory
```

### Fish Properties
```csharp
fish.InteractionType           // FishInteractionType enum
fish.isInteractable            // bool - can interact
fish.IsEnableInteraction       // bool - enabled for interaction
fish.gameObject.activeInHierarchy    // bool - active in scene
fish.transform.position        // Vector3 - world position
```

### Chest Properties
```csharp
chest.IsOpen                   // bool - chest opened
chest.gameObject.name          // string - prefab name
chest.transform.position       // Vector3
chest.SuccessInteract(player)  // void - open chest
```

### Item Properties
```csharp
item.isNeedSwapSetID           // int - 0 if real item, non-zero if ghost
item.usePreset                 // bool - preset item
item.gameObject.name           // string - prefab name
item.transform.position        // Vector3
```

---

## ⚠️ IMPORTANT GOTCHAS

### 1. Ghost Items (Double Object Syndrome)
```csharp
// Items exist as TWO objects:
if (item.isNeedSwapSetID != 0)
    continue;  // Skip ghost copy
```

### 2. Oxygen Chests Spawn Zones
```csharp
// O2 chests don't give oxygen directly
// They spawn OxygenZone that player must physically enter
// Always use smaller radius (1.0) for O2 chests
```

### 3. Fish InteractionType is STATIC
```csharp
// InteractionType is set in prefab, doesn't change at runtime
// DON'T use it to detect if fish is dead
// Use IsEnableInteraction + activeInHierarchy instead
```

### 4. Grab Level Check Pattern
```csharp
// Sea urchins need grab level check
var seaUrchin = item.TryCast<PickupInstanceItem_SeaUrchin>();
if (seaUrchin != null)
{
    if (player.grabHandler.grabLevel < seaUrchin._grabLevel)
        continue;  // Skip, player doesn't have gloves
}
```

### 5. Weapon Swap Loop Danger
```csharp
// DON'T auto-pickup weapons - causes infinite loop:
// pickup weapon → equip → drop old weapon → pickup dropped → ...
var goName = item.gameObject.name;
if (goName.StartsWith("PickupInstance") || goName.Contains("HarpoonHead"))
    continue;
```

### 6. PlayerCharacter.Update is HIGHLY PATCHED
```csharp
// The following ALL patch PlayerCharacter.Update:
// - AutoPickupPatch (Postfix)
// - AllEffects_Patch (Prefix/Postfix)
// - FishDensityScanPatch (Postfix)
// - EntityRegistryPurgePatch (Postfix)
// - PeriodicScanPatch (Postfix)
// Order matters! Last patch in load order runs last.
```

---

## 📚 REFERENCE BY GAME SYSTEM

### For Fish Catching Mods:
```
Classes:  FishInteractionBody, FishAllocator
Methods:  CheckAvailableInteraction(), SuccessInteract()
Pattern:  FishInteractionType == Pickup (check before interact)
Enum:     FishInteractionType.Pickup = 2
```

### For Item Pickup Mods:
```
Classes:  PickupInstanceItem, PickupInstanceItem_SeaUrchin
Methods:  CheckAvailableInteraction(), SuccessInteract()
Pattern:  Check isNeedSwapSetID == 0 (filter ghosts)
Filter:   Skip "PickupInstance*" and "*HarpoonHead*"
```

### For Chest/Loot Mods:
```
Classes:  InstanceItemChest, SpawnerChestItem_GodzillaFigure
Methods:  SuccessInteract(BaseCharacter) ← KEY METHOD
Pattern:  SuccessInteract(player) to open chest
Oxygen:   Use 1.0 radius, creates OxygenZone trigger
```

### For Scene Transition Mods:
```
Classes:  MoveScenePanel, MoveSceneElement, SceneLoader
Methods:  OnPlayerEnter(bool), ShowList(bool), OnCancel()
Pattern:  Find panel, call OnPlayerEnter(true) to open
Property: IsOpened to check if panel is open
```

### For Equipment/Upgrade Mods:
```
Classes:  SubEquipmentManager, DataManager, HarpoonProjectile
Access:   SingletonNoMono<SubEquipmentManager>.s_Instance
Pattern:  Patch DataManager.GetSubEquipment/GetIntegratedItem
Methods:  GetSubEquipment(), GetIntegratedItem()
```

### For Mining/Resource Mods:
```
Classes:  BreakableLootObject, InteractionGimmick_Mining
Methods:  OnEnable(), OnDie(), Awake()
Enum:     BreakableLootObjectType (8 types, filter Pile/SeaWeed)
Filter:   Only register types 0-5 (actual ores)
```

### For Oxygen System Mods:
```
Classes:  InstanceItemChest (O2 type), OxygenArea
O2 Chests: Spawn OxygenZone at chest location
OxygenArea: OnPlayerEnter(true) direct charging
Threshold: minHP property (default 0.5)
```

---

## 🚀 IL2CPP SPECIFIC PATTERNS

### Array Creation & Population
```csharp
var newArray = new Il2CppStructArray<int>(length);
newArray[0] = value1;
newArray[1] = value2;
// Pass to game code
someMethod.SetArray(newArray);
```

### Type Casting
```csharp
// Safely cast to subtype
var specialized = item.TryCast<PickupInstanceItem_SeaUrchin>();
if (specialized != null)
{
    // Use specialized version
}
```

### Property Getter Patching
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

### Private Field Access
```csharp
var field = obj.GetType().GetField(
    fieldName,
    BindingFlags.Instance | BindingFlags.NonPublic
);
var value = (T)field.GetValue(obj);
```

---

## 📞 VERSION INFO

- **Mod**: DaveDiverExpansion v1.6.1
- **BepInEx**: Version 6 (Bleeding Edge)
- **HarmonyLib**: HarmonyX
- **Game**: Dave the Diver (IL2CPP, Unity 6000.0.52f1)
- **Reference Date**: June 2026

---

All class names, method names, and patterns in this document are verified against the actual DaveDiverExpansion source code repository.
