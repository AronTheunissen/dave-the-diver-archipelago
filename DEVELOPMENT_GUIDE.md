# Dave the Diver Archipelago - Development Guide

## 📊 Project Status

### ✅ Completed

- [x] Project structure created
- [x] Python virtual environment set up with dependencies
- [x] Archipelago reference repository cloned
- [x] Basic APWorld skeleton implemented
- [x] Example items (~50) and locations (~40) defined
- [x] Design document created
- [x] Development documentation written

### ⏳ In Progress / Not Started

- [ ] .NET SDK 8.0 installation (required for client mod)
- [ ] BepInEx setup in game directory
- [ ] Complete game analysis (items, locations, progression)
- [ ] Expand items/locations to 150-300 each
- [ ] Implement APWorld logic (regions, rules, options)
- [ ] Create C# client mod
- [ ] Testing and refinement

---

## 🎯 Development Workflow

### Phase 1: Game Analysis (Current Phase)

**Goal:** Document everything in Dave the Diver that can be randomized

#### What to Do:

1. **Play through the game** (or watch a complete playthrough)
2. **Create a spreadsheet** with these columns:
   - Name
   - Type (Item/Location)
   - Category (Weapon/Recipe/Equipment/Story/etc.)
   - Classification (Progression/Useful/Filler)
   - Region (where it's found/used)
   - Dependencies (what's needed to get it)

3. **Document everything:**
   - All weapons and harpoon tips
   - All diving equipment (suits, oxygen tanks, etc.)
   - All recipes (there are 100+ in the game)
   - All story checkpoints and chapters
   - All side quests
   - All boss encounters
   - All restaurant upgrades
   - All staff unlocks
   - All minigames
   - Notable fish species

#### Example Spreadsheet Format:

```
Name                    | Type     | Category   | Classification | Region          | Dependencies
------------------------|----------|------------|----------------|-----------------|------------------
Steel Net Gun Tip       | Item     | Weapon     | Useful         | Equipment Shop  | Gold: 500
Complete Chapter 1      | Location | Story      | Progression    | Blue Hole       | Basic Harpoon
Giant Squid             | Location | Fish       | Progression    | Deep Blue Hole  | Depth > 100m
Tuna Nigiri Recipe      | Item     | Recipe     | Useful         | Restaurant      | Catch Tuna
VIP Card                | Item     | Key Item   | Progression    | Story Event     | Chapter 3
```

---

### Phase 2: APWorld Implementation

**Goal:** Create the Python code that defines your randomizer logic

#### File Structure:

```
apworld/davethediver/
├── __init__.py         # Main world class
├── items.py            # All items that can be received
├── locations.py        # All checks/locations
├── regions.py          # Areas/regions of the game
├── rules.py            # Logic rules for access
└── options.py          # YAML configuration options
```

#### Step-by-Step Implementation:

### 1. Define All Items (`items.py`)

**What:** Every item that can be randomized and given to players

**How:** Define items with their ID, classification, and count

**Example from your code:**

```python
from typing import Dict, NamedTuple, Optional
from BaseClasses import Item, ItemClassification

class DaveDiverItem(Item):
    """An item in Dave the Diver"""
    game: str = "Dave the Diver"

class ItemData(NamedTuple):
    code: Optional[int]
    classification: ItemClassification
    count: int = 1

BASE_ID = 0x444400  # Your unique game ID

weapon_items: Dict[str, ItemData] = {
    "Basic Harpoon Gun": ItemData(BASE_ID + 0, ItemClassification.progression),
    "Enhanced Harpoon Gun": ItemData(BASE_ID + 1, ItemClassification.progression),
    "Steel Net Gun Tip": ItemData(BASE_ID + 10, ItemClassification.useful),
}

diving_equipment: Dict[str, ItemData] = {
    "Oxygen Tank +1": ItemData(BASE_ID + 100, ItemClassification.progression, count=5),
    "Cargo Expansion +1": ItemData(BASE_ID + 110, ItemClassification.progression, count=5),
}

# Combine all items
item_table: Dict[str, ItemData] = {
    **weapon_items,
    **diving_equipment,
    # ... more categories
}
```

**Item Classifications:**
- `progression` - Required to beat the game (harpoons, oxygen upgrades, key items)
- `useful` - Helpful but not required (better weapons, extra staff)
- `filler` - Common items to fill locations (money, materials)
- `trap` - Optional negative effects

**Real Example from Stardew Valley:**

```python
# tools/Archipelago/worlds/stardew_valley/items.py
"Progressive Pickaxe": ItemData(1101, ItemClassification.progression),
"Progressive Axe": ItemData(1102, ItemClassification.progression),
"Beach Bridge": ItemData(1210, ItemClassification.progression),
"Backpack (24)": ItemData(1301, ItemClassification.useful),
```

---

### 2. Define All Locations (`locations.py`)

**What:** Every place in the game where you can "check" for an item

**How:** Define locations with their ID and which region they belong to

**Example from your code:**

```python
from typing import Dict, NamedTuple, Optional

class LocationData(NamedTuple):
    code: Optional[int]
    region: str

BASE_ID = 0x444400

story_locations: Dict[str, LocationData] = {
    "Complete Chapter 1": LocationData(BASE_ID + 0, "Blue Hole - Shallow"),
    "Complete Chapter 2": LocationData(BASE_ID + 1, "Blue Hole - Mid"),
}

boss_locations: Dict[str, LocationData] = {
    "Defeat Giant Squid Boss": LocationData(BASE_ID + 300, "Blue Hole - Deep"),
}

restaurant_locations: Dict[str, LocationData] = {
    "Serve 50 Customers": LocationData(BASE_ID + 201, "Bancho Sushi"),
    "Restaurant Rating: 5 Stars": LocationData(BASE_ID + 222, "Bancho Sushi"),
}

location_table: Dict[str, LocationData] = {
    **story_locations,
    **boss_locations,
    **restaurant_locations,
}
```

**Real Example from Subnautica:**

```python
# tools/Archipelago/worlds/subnautica/locations.py
"Cyclops Bridge Fragment": LocationData("Crag Field", 1),
"Seamoth Fragment 1": LocationData("Kelp Forest", 2),
"Modification Station Fragment": LocationData("Grassy Plateaus", 3),
```

---

### 3. Create Regions (`regions.py`)

**What:** Different areas of the game and how they connect

**How:** Define regions and their connections, then add locations to each region

**Example to create:**

```python
# apworld/davethediver/regions.py
from typing import Dict, List, Set
from BaseClasses import Region, Entrance
from .locations import location_table, LocationData

def create_regions(world):
    """Create all regions for Dave the Diver"""
    
    # Define regions
    menu = Region("Menu", world.player, world.multiworld)
    
    bancho_sushi = Region("Bancho Sushi", world.player, world.multiworld)
    
    blue_hole_shallow = Region("Blue Hole - Shallow", world.player, world.multiworld)
    blue_hole_mid = Region("Blue Hole - Mid", world.player, world.multiworld)
    blue_hole_deep = Region("Blue Hole - Deep", world.player, world.multiworld)
    
    glacier = Region("Glacier", world.player, world.multiworld)
    sea_people = Region("Sea People Village", world.player, world.multiworld)
    
    # Add locations to regions
    for location_name, location_data in location_table.items():
        if location_data.region == "Bancho Sushi":
            bancho_sushi.locations.append(
                DaveDiverLocation(world.player, location_name, location_data.code, bancho_sushi)
            )
        elif location_data.region == "Blue Hole - Shallow":
            blue_hole_shallow.locations.append(
                DaveDiverLocation(world.player, location_name, location_data.code, blue_hole_shallow)
            )
        # ... etc for all regions
    
    # Create connections between regions
    menu.connect(bancho_sushi, "Start Game")
    bancho_sushi.connect(blue_hole_shallow, "Go Diving - Shallow")
    blue_hole_shallow.connect(blue_hole_mid, "Dive Deeper - Mid")
    blue_hole_mid.connect(blue_hole_deep, "Dive Deeper - Deep")
    bancho_sushi.connect(glacier, "Travel to Glacier")
    bancho_sushi.connect(sea_people, "Visit Sea People Village")
    
    # Add all regions to multiworld
    world.multiworld.regions += [
        menu, bancho_sushi, blue_hole_shallow, blue_hole_mid, 
        blue_hole_deep, glacier, sea_people
    ]

class DaveDiverLocation(Location):
    game: str = "Dave the Diver"
```

**Real Example from Stardew Valley:**

```python
# tools/Archipelago/worlds/stardew_valley/regions.py
Region("Pelican Town", pelican_town_locations),
Region("Beach", beach_locations),
Region("The Mines", mines_locations),
Region("Skull Cavern", skull_cavern_locations),

# Connection with requirement
pelican_town.connect(beach)  # Always accessible
pelican_town.connect(mines, rule=lambda state: state.has("Mine Key", player))
```

---

### 4. Define Access Rules (`rules.py`)

**What:** Logic that determines what items are needed to reach each location

**How:** Write rules using lambda functions that check the player's state

**Example to create:**

```python
# apworld/davethediver/rules.py
from BaseClasses import CollectionState
from worlds.AutoWorld import LogicMixin

def set_rules(world):
    """Set access rules for regions and locations"""
    
    # Region access rules
    # Blue Hole - Mid requires oxygen upgrade or better diving suit
    set_rule(
        world.multiworld.get_entrance("Dive Deeper - Mid", world.player),
        lambda state: state.has("Oxygen Tank +1", world.player, 2) or 
                     state.has("Enhanced Diving Suit", world.player)
    )
    
    # Blue Hole - Deep requires advanced equipment
    set_rule(
        world.multiworld.get_entrance("Dive Deeper - Deep", world.player),
        lambda state: state.has("Advanced Harpoon Gun", world.player) and
                     state.has("Oxygen Tank +1", world.player, 4) and
                     state.has("Deep Diving Suit", world.player)
    )
    
    # Glacier requires cold protection
    set_rule(
        world.multiworld.get_entrance("Travel to Glacier", world.player),
        lambda state: state.has("Cold Protection Suit", world.player) and
                     has_completed_chapter(state, world.player, 4)
    )
    
    # Sea People Village requires VIP card and story progress
    set_rule(
        world.multiworld.get_entrance("Visit Sea People Village", world.player),
        lambda state: state.has("VIP Card", world.player) and
                     has_completed_chapter(state, world.player, 3)
    )
    
    # Location-specific rules
    # Boss fights require specific weapons
    set_rule(
        world.multiworld.get_location("Defeat Giant Squid Boss", world.player),
        lambda state: state.has("Enhanced Harpoon Gun", world.player) and
                     state.has("Oxygen Tank +1", world.player, 3)
    )
    
    # Victory condition - beat the final chapter
    world.multiworld.completion_condition[world.player] = lambda state: \
        has_completed_chapter(state, world.player, 6)

def has_completed_chapter(state: CollectionState, player: int, chapter: int) -> bool:
    """Helper function to check if a chapter is complete"""
    return state.has(f"Complete Chapter {chapter}", player)

def has_weapon_tier(state: CollectionState, player: int, tier: int) -> bool:
    """Helper function to check weapon tier"""
    if tier <= 1:
        return state.has("Basic Harpoon Gun", player)
    elif tier == 2:
        return state.has("Enhanced Harpoon Gun", player)
    else:
        return state.has("Advanced Harpoon Gun", player)
```

**Real Example from Subnautica:**

```python
# tools/Archipelago/worlds/subnautica/rules.py
def has_seaglide(state, player):
    return state.has("Seaglide Fragment", player, 2)

def can_access_200m(state, player):
    return (state.has("Ultra High Capacity Tank", player) or
            state.has("Lightweight High Capacity Tank", player) or
            (state.has("High Capacity Tank", player) and state.has("Rebreather", player)))

set_rule(world.get_location("Sparse Reef Wreck 1"), 
         lambda state: can_access_200m(state, player))
```

---

### 5. Add Options (`options.py`)

**What:** YAML configuration options players can set when generating a seed

**How:** Define options using Archipelago's option classes

**Example to create:**

```python
# apworld/davethediver/options.py
from dataclasses import dataclass
from Options import PerGameCommonOptions, Toggle, DefaultOnToggle, Range, Choice

class StartingOxygen(Range):
    """How many oxygen tank upgrades to start with"""
    display_name = "Starting Oxygen Upgrades"
    range_start = 0
    range_end = 3
    default = 0

class StartingWeapon(Choice):
    """Which harpoon gun to start with"""
    display_name = "Starting Weapon"
    option_basic = 0
    option_enhanced = 1
    default = 0

class RequireAllRecipes(Toggle):
    """Require unlocking all recipes to complete the game"""
    display_name = "Require All Recipes"
    default = 0

class RequireAllFish(Toggle):
    """Require catching all fish species to complete the game"""
    display_name = "Require All Fish Caught"
    default = 0

class RestaurantDifficulty(Choice):
    """How difficult the restaurant management should be"""
    display_name = "Restaurant Difficulty"
    option_easy = 0
    option_normal = 1
    option_hard = 2
    default = 1

class DeathLink(Toggle):
    """When you die, everyone dies. And when someone else dies, you die."""
    display_name = "Death Link"
    default = 0

@dataclass
class DaveDiverOptions(PerGameCommonOptions):
    starting_oxygen: StartingOxygen
    starting_weapon: StartingWeapon
    require_all_recipes: RequireAllRecipes
    require_all_fish: RequireAllFish
    restaurant_difficulty: RestaurantDifficulty
    death_link: DeathLink
```

**Real Example from Stardew Valley:**

```python
# tools/Archipelago/worlds/stardew_valley/options.py
class Goal(Choice):
    """What's the goal to complete the game?"""
    display_name = "Goal"
    option_community_center = 0
    option_grandpa_evaluation = 1
    option_bottom_of_the_mines = 2
    option_cryptic_note = 3
    default = 0

class StartingMoney(Range):
    """Amount of gold when arriving at the farm"""
    display_name = "Starting Gold"
    range_start = -1
    range_end = 50000
    default = -1
```

---

### 6. Update Main World Class (`__init__.py`)

**What:** Tie everything together in the main world class

**How:** Implement all required methods

**Example to update:**

```python
# apworld/davethediver/__init__.py
from typing import Dict, Any, List
from BaseClasses import Region, Tutorial
from worlds.AutoWorld import World, WebWorld
from .items import DaveDiverItem, item_table, item_name_to_id
from .locations import location_table, location_name_to_id
from .regions import create_regions
from .rules import set_rules
from .options import DaveDiverOptions

class DaveDiverWorld(World):
    """
    Dave the Diver is a unique adventure game combining undersea exploration,
    running a sushi restaurant, and various minigames.
    """
    
    game = "Dave the Diver"
    options_dataclass = DaveDiverOptions
    options: DaveDiverOptions
    
    item_name_to_id = item_name_to_id
    location_name_to_id = location_name_to_id
    
    def create_regions(self):
        """Create all regions and locations"""
        create_regions(self)
        
    def create_items(self):
        """Create the item pool"""
        # Start with all items
        item_pool = []
        
        for item_name, item_data in item_table.items():
            # Add multiple copies if needed
            for _ in range(item_data.count):
                item_pool.append(self.create_item(item_name))
        
        # Remove items that are starting inventory
        if self.options.starting_weapon == 1:  # Enhanced
            item_pool.remove(self.create_item("Enhanced Harpoon Gun"))
        
        # Add starting oxygen if configured
        for _ in range(self.options.starting_oxygen.value):
            item_pool.remove(self.create_item("Oxygen Tank +1"))
        
        self.multiworld.itempool += item_pool
        
    def set_rules(self):
        """Set access rules"""
        set_rules(self)
        
    def create_item(self, name: str) -> DaveDiverItem:
        """Create an item by name"""
        item_data = item_table[name]
        return DaveDiverItem(name, item_data.classification, item_data.code, self.player)
        
    def fill_slot_data(self) -> Dict[str, Any]:
        """Data sent to the client"""
        return {
            "death_link": self.options.death_link.value,
            "starting_weapon": self.options.starting_weapon.value,
            "restaurant_difficulty": self.options.restaurant_difficulty.value,
        }
```

---

### Phase 3: Client Mod Implementation

**Goal:** Create a BepInEx mod that connects the game to Archipelago

#### Prerequisites:

1. Install .NET SDK 8.0
2. Install BepInEx 6 IL2CPP to game directory
3. Run game once to generate interop assemblies

#### File Structure:

```
client/
├── DaveDiverAP/
│   ├── Plugin.cs              # BepInEx plugin entry point
│   ├── ArchipelagoClient.cs   # Connection to AP server
│   ├── GameStateManager.cs    # Track unlocked items/locations
│   ├── ItemGranter.cs         # Grant items to player
│   └── Patches/
│       ├── FishCatchPatch.cs      # Intercept fish catches
│       ├── RecipeUnlockPatch.cs   # Intercept recipe unlocks
│       ├── EquipmentPatch.cs      # Modify equipment availability
│       └── SaveLoadPatch.cs       # Integrate with save system
├── lib/                       # Reference DLLs from game
└── DaveDiverAP.csproj
```

#### Step-by-Step Implementation:

### 1. Create C# Project

```powershell
cd dave-the-diver-archipelago/client
dotnet new classlib -n DaveDiverAP -f net48
cd DaveDiverAP
dotnet add package BepInEx.Core
dotnet add package BepInEx.IL2CPP
dotnet add package HarmonyX
dotnet add package Archipelago.MultiClient.Net
```

### 2. Create Plugin Entry Point

**File:** `client/DaveDiverAP/Plugin.cs`

```csharp
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace DaveDiverAP
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BasePlugin
    {
        public const string GUID = "com.archipelago.davethediver";
        public const string NAME = "Dave the Diver Archipelago";
        public const string VERSION = "1.0.0";
        
        private Harmony _harmony;
        private ArchipelagoClient _apClient;
        
        public override void Load()
        {
            Log.LogInfo($"Plugin {NAME} v{VERSION} is loading!");
            
            // Initialize Archipelago client
            _apClient = new ArchipelagoClient(Log);
            
            // Apply Harmony patches
            _harmony = new Harmony(GUID);
            _harmony.PatchAll();
            
            Log.LogInfo($"Plugin {NAME} loaded successfully!");
        }
        
        public override bool Unload()
        {
            _apClient?.Disconnect();
            _harmony?.UnpatchSelf();
            return base.Unload();
        }
    }
}
```

### 3. Create Archipelago Client

**File:** `client/DaveDiverAP/ArchipelagoClient.cs`

```csharp
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using BepInEx.Logging;
using System;
using System.Threading.Tasks;

namespace DaveDiverAP
{
    public class ArchipelagoClient
    {
        private ArchipelagoSession _session;
        private ManualLogSource _log;
        
        public bool IsConnected => _session?.Socket.Connected ?? false;
        
        public ArchipelagoClient(ManualLogSource log)
        {
            _log = log;
        }
        
        public async Task<bool> Connect(string host, int port, string slotName, string password = "")
        {
            try
            {
                _session = ArchipelagoSessionFactory.CreateSession(host, port);
                
                // Handle connection result
                LoginResult result = await _session.LoginAsync(
                    "Dave the Diver",
                    slotName,
                    ItemsHandlingFlags.AllItems,
                    new Version(0, 4, 4),
                    password: password
                );
                
                if (!result.Successful)
                {
                    _log.LogError($"Failed to connect: {string.Join(", ", result.Errors)}");
                    return false;
                }
                
                // Set up event handlers
                _session.Items.ItemReceived += OnItemReceived;
                _session.Socket.ErrorReceived += OnError;
                _session.Socket.SocketClosed += OnDisconnect;
                
                _log.LogInfo($"Successfully connected to Archipelago as {slotName}");
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError($"Connection error: {ex.Message}");
                return false;
            }
        }
        
        public void CheckLocation(long locationId)
        {
            if (!IsConnected) return;
            
            _session.Locations.CompleteLocationChecks(locationId);
            _log.LogInfo($"Location checked: {locationId}");
        }
        
        private void OnItemReceived(ReceivedItemsHelper helper)
        {
            var item = helper.PeekItem();
            _log.LogInfo($"Received item: {item.ItemName} from {item.PlayerName}");
            
            // Grant item to player
            ItemGranter.GrantItem(item.ItemName);
            
            helper.DequeueItem();
        }
        
        private void OnError(Exception ex, string message)
        {
            _log.LogError($"Socket error: {message}");
        }
        
        private void OnDisconnect(string reason)
        {
            _log.LogWarning($"Disconnected from Archipelago: {reason}");
        }
        
        public void Disconnect()
        {
            _session?.Socket.Disconnect();
        }
    }
}
```

### 4. Create Harmony Patch Example

**File:** `client/DaveDiverAP/Patches/FishCatchPatch.cs`

```csharp
using HarmonyLib;
using UnityEngine;

namespace DaveDiverAP.Patches
{
    [HarmonyPatch(typeof(FishManager), "OnFishCaught")]
    public class FishCatchPatch
    {
        static void Postfix(FishData fish)
        {
            // Check if this is a rare/boss fish that's a location check
            if (IsLocationCheckFish(fish))
            {
                long locationId = GetLocationIdForFish(fish.fishId);
                
                // Check the location in Archipelago
                Plugin.Instance.APClient.CheckLocation(locationId);
                
                Debug.Log($"[DaveDiverAP] Checked location for fish: {fish.fishName}");
            }
        }
        
        static bool IsLocationCheckFish(FishData fish)
        {
            // Only rare/boss fish are location checks
            return fish.rarity >= FishRarity.Rare || fish.isBoss;
        }
        
        static long GetLocationIdForFish(int fishId)
        {
            // Map fish ID to Archipelago location ID
            // This would reference your locations.py
            return 0x444400 + 100 + fishId;
        }
    }
}
```

**Note:** The actual class/method names will differ - you'll need to:
1. Use dnSpy or similar to inspect game DLLs
2. Find the actual methods for fish catching, recipe unlocking, etc.
3. Create patches for each

---

## 🔄 Development Cycle

### Daily Workflow:

1. **Morning: Plan**
   - Review what you worked on yesterday
   - Decide what to tackle today
   - Update your game analysis spreadsheet

2. **Development: Implement**
   - Work on APWorld Python code OR client C# code
   - Test frequently
   - Commit to git regularly

3. **Testing: Verify**
   ```powershell
   # Test APWorld
   cd tools/Archipelago
   cp -r ../../apworld/davethediver ./worlds/
   python Generate.py  # Generate a test seed
   
   # Test Client (after building)
   # Launch Dave the Diver with BepInEx
   # Check console for mod loading
   ```

4. **Evening: Document**
   - Update documentation with what you learned
   - Note any issues or questions
   - Plan tomorrow's work

---

## 📖 Learning Resources

### Understanding Archipelago:

1. **Read existing implementations:**
   ```
   tools/Archipelago/worlds/stardew_valley/    # Best example, similar game
   tools/Archipelago/worlds/subnautica/        # Underwater exploration
   tools/Archipelago/worlds/minecraft/         # Crafting mechanics
   ```

2. **Key files to study:**
   - `BaseClasses.py` - Core Archipelago classes
   - `worlds/AutoWorld.py` - Base world class
   - Any world's `__init__.py` - See how they implement

3. **Official Documentation:**
   - Archipelago Discord - #apworld-development channel
   - GitHub Wiki: https://github.com/ArchipelagoMW/Archipelago/wiki

### Learning BepInEx/Harmony:

1. **Study existing Dave the Diver mods:**
   - https://github.com/WhiteMinds/dave-diver-expansion
   - https://www.nexusmods.com/davethediver

2. **BepInEx Documentation:**
   - https://docs.bepinex.dev/

3. **Harmony Patching Guide:**
   - https://harmony.pardeike.net/

---

## 🐛 Troubleshooting

### Python Issues:

**Import errors:**
```powershell
cd apworld
.\venv\Scripts\python.exe -m pip install -e ../../tools/Archipelago
```

**Testing APWorld:**
```powershell
cd tools/Archipelago
python -m pytest worlds/davethediver/test/
```

### C# Issues:

**BepInEx not loading:**
- Check `BepInEx/LogOutput.log` in game folder
- Verify you ran game once after installing BepInEx
- Check that DLL is in `BepInEx/plugins/`

**Can't find game classes:**
- Use dnSpy to inspect `GameAssembly.dll`
- Copy interop DLLs from `BepInEx/interop/` to `client/lib/`

---

## ✅ Checklist for Going Live

Before releasing your APWorld:

- [ ] 150+ items defined
- [ ] 150+ locations defined
- [ ] All regions implemented
- [ ] Logic rules tested (no impossible seeds)
- [ ] Options tested (all combinations work)
- [ ] Client mod connects successfully
- [ ] All location checks work in-game
- [ ] All item grants work correctly
- [ ] Savegame integration works
- [ ] Solo playthrough completed
- [ ] Multiworld test with 2+ players
- [ ] Documentation complete
- [ ] README for players written
- [ ] Submitted to Archipelago Discord for review

---

## 🎯 Current Priority: Game Analysis

**Your immediate next step is to thoroughly analyze Dave the Diver and expand your item/location lists from ~50 to 150-300 each.**

Create a spreadsheet and document EVERYTHING. This is the foundation of your entire project.

Good luck! 🌊🍣🎮
