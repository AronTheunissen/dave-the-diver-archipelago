# Dave the Diver - Real Unity Class Names and Methods (IL2CPP)

This document lists real class names and method names found in Dave the Diver by analyzing Harmony patch files from two mod repositories:
- https://github.com/devopsdinosaur/dave-the-diver-mods (Super Dave mod)
- https://github.com/Arutsuyo/SuperDave2.0 (SuperDave 2.0 mod)

## Fish/Diving System

### Fish Catching & Items
- **BuffHandler** - Method: `Start()`
  - File: Diving.cs (Both repos)
  - Related to buff/status effects on fish/items

- **PickupInstanceItem** - auto_pickup_callback_PickupInstanceItem() references this
  - File: Diving.cs
  - Handles picking up items from the environment

- **IntegratedItem** - Method: `BuildItem(Items itemBase)`
  - File: Diving.cs
  - Constructs items

- **Items** - Parameter class for item building
  - File: Diving.cs
  - Base item class

### Item Information & UI
- **GetInfoPanelUI** - Method: `WaitOnPopup(GetInfoPanelUI.GetItemInfo info)`
  - File: Diving.cs
  - Shows item information popup
  - Contains nested class: `GetItemInfo`

### Player Diving System
- **PlayerCharacter** - Core player class
  - Methods:
    - `IsCrabTrapAvailable` (Property getter)
    - `SetHPDamage()` - Takes damage in diving
    - `Awake()` - Initialization
  - File: Diving.cs, Singletons.cs
  - References in Singletons.cs indicate this is the main player character class

- **PlayerBreathHandler** - Method: `Update()`
  - File: Diving.cs
  - Handles oxygen/breath mechanics when diving
  - When patched with infinite oxygen mod, player never runs out of breath

- **CharacterController2D** - Method: `Awake()`
  - File: Singletons.cs
  - 2D character movement controller
  - Referenced as singleton for player movement

### Weapons & Equipment
- **SetUPCrabTrapCommand_SO** - ScriptableObject for crab traps
  - Methods:
    - `HoldExecute()`
    - `Start()`
  - File: SuperDavePlugin.cs (repo2)
  - Handles crab trap deployment/execution

## Scene & Environment Management

### Lobby/Home Area
- **LobbyPlayer** - Player in lobby scene
  - Methods:
    - `LateUpdate()`
    - `FixedUpdate()`
  - File: SuperDavePlugin.cs
  - Controls player movement in lobby/boat area

### Diving Locations/Areas
- **DelayedDisappear** - Method: `Update()`
  - File: SuperDavePlugin.cs (repo2)
  - Handles delayed disappearance of objects (likely chests/items after pickup)

## Farm Management

### Farm System
- **Farm.FarmPlayerView** - Nested class for farm player view
  - Methods:
    - `Setup()`
    - `Move()`
    - `FixedUpdate()`
  - File: SuperDavePlugin.cs
  - Handles Dave's movement and interaction on the farm

- **Farm.FarmCore** - Core farm mechanics
  - Method: `UpExecute()`
  - File: SuperDavePlugin.cs (repo2)
  - Executes farm-related commands/updates

### Fish Farm System
- **FishFarm.FishFarmPlayerView** - Nested class for fish farm player view
  - Method: `Move()`
  - File: SuperDavePlugin.cs
  - Handles movement in fish farm area

## Restaurant/Sushi Bar Management

### Sushi Bar Characters & Customers
- **SushiBarCustomer** - Method: `LateUpdate()`
  - File: SuperDavePlugin.cs
  - Handles customer behavior and patience

- **SushiBarStaffBase** - Base class for sushi bar staff
  - Methods:
    - `CalcCookingTime()` - Calculates cooking speed
    - `CalcMoveSpeed()` - Calculates staff movement speed
  - File: SuperDavePlugin.cs
  - Used by chefs/kitchen staff

### Sushi Bar Context & Management
- **SushiBarContext.WasabiGratersData** - Nested class for wasabi system
  - Method: `UpdateWasabiCount(int count)`
  - File: SuperDavePlugin.cs
  - Tracks and updates wasabi availability
  - Can be patched for infinite wasabi

- **SushiBarManager** - Method: `SetTime()`
  - File: SuperDavePlugin.cs (repo2)
  - Manages overall sushi bar state and timing

### Player in Restaurant
- **DaveMoveValue** - Method: `speedMultiplier` (Property getter)
  - File: SuperDavePlugin.cs
  - Controls Dave's movement speed multiplier when in sushi bar/restaurant

## Player Stats & Progression

### Player Stats (referenced through Singletons.cs)
- **PlayerCharacter** - Core player statistics and state
  - Contains oxygen/breath stats
  - Contains health/HP
  - Contains inventory/carry capacity
  - Contains crab trap availability status

### Watering/Farm Interaction
- **PlayerCharacter.Watering()** - Method
  - File: SuperDavePlugin.cs (repo2)
  - Handles player watering action on farm

## iDiver Upgrades

While specific upgrade classes weren't explicitly found in the patches, the following are implied:

### Oxygen Upgrade
- Controlled via **PlayerBreathHandler.Update()** patch
- Infinite oxygen mod patches this to prevent oxygen depletion

### Harpoon/Weapon Upgrades
- Referenced in **Diving.cs** comments:
  - `set_harpoon_head(InstanceItemInventory inventory)` - Method in Diving.cs (repo2)
  - Harpoon types mentioned: Old, Iron, Pump, Merman, NewMV, Alloy
  - Harpoon head types: Normal, Electric, Poison, Chain, Sleep, Paralysis, Strong, Fire, Ice
  - **InstanceItemInventory** - Class referenced in equipment management

- **SetUPCrabTrapCommand_SO** - Related to trap upgrades

### Suit/Armor
- Not explicitly found in patches but likely stored in PlayerCharacter

## Chest & Item Pickup

### Chest Opening
- **PickupInstanceItem** - Handles pickup of all ground items including chests
  - Used in auto-pickup functionality
- **DelayedDisappear** - Handles chest disappearance after opening
  - Method: `Update()`

## Save System

### Player Data & Saves
Based on the code references, the following game management classes exist:

- **PlayerCharacter** - Stores player state (health, items, equipped gear)
- **Farm.FarmCore** - Stores farm state
- **SushiBarContext** + **SushiBarManager** - Stores sushi bar state

Save system classes referenced in comments but not directly patched:
- `SaveSystem` - Implied but not found in patches
- `PlayerInfoSave` - Implied but not found in patches

## Additional Game System Classes Found

### UI & Information
- **GetInfoPanelUI** - Information panel UI for items
- **BuffHandler** - Status/buff UI handler

### Movement & Physics
- **CharacterController2D** - 2D character controller (physics-based movement)
- **DaveMoveValue** - Movement speed calculations

### Interactable Objects
- **DelayedDisappear** - Timed object disappearance
- **IntegratedItem** - Item object integration

### Game Commands/Actions
- **SetUPCrabTrapCommand_SO** - ScriptableObject for crab trap commands

## Source Files & Locations

### Repository 1: devopsdinosaur/dave-the-diver-mods
- **super_dave/Diving.cs** - Main diving/underwater mechanics
- **super_dave/SuperDavePlugin.cs** - Plugin entry point and farm/sushi patches
- **super_dave/Singletons.cs** - Singleton instances for PlayerCharacter and CharacterController2D
- **shared/dd_utils.cs** - Utility classes (DDPlugin, ReflectionUtils, UnityUtils)

### Repository 2: Arutsuyo/SuperDave2.0
- **super_dave/Diving.cs** - Extended diving mechanics with additional methods:
  - `HealPlayer()`
  - `IncreasePlayerWeapon()`
  - `DecreasePlayerWeapon()`
  - `GivePlayerTranq()`
  - `GivePlayerNet()`
  - `GivePlayerSnipe()`
  - `set_harpoon_head(InstanceItemInventory inventory)`

## Summary by Game Feature

| Feature | Class Name | Key Methods |
|---------|-----------|------------|
| **Fish Catching** | BuffHandler, PickupInstanceItem, IntegratedItem | Start(), BuildItem() |
| **Oxygen/Diving** | PlayerBreathHandler, PlayerCharacter | Update(), IsCrabTrapAvailable |
| **Player Movement** | CharacterController2D, DaveMoveValue | Awake(), speedMultiplier |
| **Farm Management** | Farm.FarmPlayerView, Farm.FarmCore | Setup(), Move(), UpExecute() |
| **Fish Farm** | FishFarm.FishFarmPlayerView | Move() |
| **Sushi Bar** | SushiBarCustomer, SushiBarStaffBase, SushiBarContext | LateUpdate(), CalcCookingTime(), UpdateWasabiCount() |
| **Crab Traps** | SetUPCrabTrapCommand_SO, PlayerCharacter | HoldExecute(), IsCrabTrapAvailable |
| **Chest Opening** | PickupInstanceItem, DelayedDisappear | auto_pickup_callback, Update() |
| **Item Pickup** | PickupInstanceItem, GetInfoPanelUI | auto_pickup callback, WaitOnPopup() |
| **Player Stats** | PlayerCharacter | SetHPDamage(), various properties |
| **UI/Info** | GetInfoPanelUI, BuffHandler | WaitOnPopup(), Start() |
| **Weapons** | SetUPCrabTrapCommand_SO, InstanceItemInventory | HoldExecute(), set_harpoon_head() |

## Notes

1. **IL2CPP Architecture**: All these classes are compiled as IL2CPP assemblies, which is why Harmony patching with BepInEx is used to modify behavior at runtime.

2. **Nested Classes**: Some classes use nested namespaces like `Farm.FarmPlayerView` and `SushiBarContext.WasabiGratersData` indicating internal structure organization.

3. **ScriptableObjects (SO)**: Classes ending in `_SO` suffix are Unity ScriptableObjects used for game configuration/commands.

4. **Property Getters/Setters**: Methods like `IsCrabTrapAvailable` and `speedMultiplier` are properties accessed via MethodType.Getter in Harmony patches.

5. **Missing Classes**: The actual `SaveSystem` and `PlayerInfoSave` classes were not found in the analyzed patches, suggesting they may not be directly modified by these mods or use reflection-based access instead.

6. **Weapons System**: The repo2 version (SuperDave2.0) includes additional weapon-related methods not found in repo1, suggesting expanded weapon/equipment control.

