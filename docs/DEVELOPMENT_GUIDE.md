# Development Guide

Detailed technical guide for contributing to the Dave the Diver Archipelago randomizer.

---

## Architecture Overview

The project has two main components:

```
APWorld (Python)          C# Client Mod (BepInEx)
─────────────────         ────────────────────────
items.py                  ItemHandler.cs
locations.py              LocationTracker.cs
regions.py                Patches/ (17 files)
rules.py                  ArchipelagoClient.cs
options.py                GoalTracker.cs
__init__.py               UI/ (3 tabs)
```

**APWorld** runs on the Archipelago server and defines what items/locations exist,
how they connect, and what options players can set in their YAML.

**C# Client** runs inside Dave the Diver via BepInEx, connects to the AP server,
detects in-game events (fish catches, boss kills, etc.) and sends location checks,
and applies items received from the multiworld.

---

## Development Status

### ✅ Phase 1 — Game Analysis (Complete)
- All fish species catalogued by depth/region (203 species)
- All recipes documented with max upgrade levels
- All weapons and upgrade trees mapped (79 variants)
- All Ecowatcher missions recorded from wiki
- All aberration fish and their vortex regions documented
- Chapter structure, boss list, region access requirements confirmed

### ✅ Phase 2 — APWorld Implementation (Complete)
- **1,134+ locations** across 23 regions (including all DLC)
- **276 items** with correct IDs, counts, and categories
- **Logic rules** with game-accurate region gating
- **5 victory conditions** implemented
- **25 YAML options** with full filtering in `should_include_item/location()`
- **DLC support** for 4 DLC packs
- **`fill_slot_data()`** passes all options to the client

### ✅ Phase 2.5 — Unit Tests (Complete, 55/55 passing)
- `apworld/tests/conftest.py` — Archipelago BaseClasses mock (no full install needed)
- `apworld/tests/test_ids.py` — ID uniqueness, region validity, item/location collision detection
- `apworld/tests/test_filtering.py` — all `should_include_location()` and `should_include_item()` logic
- `apworld/tests/test_slot_data.py` — `fill_slot_data()` key coverage and value types
- Run with: `cd apworld && python -m pytest tests/ -v`

### ✅ Phase 3 — C# Client Skeleton (Complete)
- BepInEx 6 IL2CPP plugin structure
- Archipelago.MultiClient.Net connection with auto-reconnect
- 17 Harmony patch skeletons covering all location categories
- Thread-safe item queue (boat-only delivery)
- Death Link, hint system, goal tracker
- 3-tab in-game UI (F9): Connection · Hints · Progress
- BepInEx config file, save/restore session state

### ✅ Phase 4 — Wire Up Game Internals (Complete)
All 17 Harmony patches now use real class/method names confirmed via dump.cs (Il2CppDumper).
See `docs/CLASS_NAME_CHEAT_SHEET.md` for the full reference.

### 🔧 Phase 5 — TID Mapping & ItemHandler (Substantially Complete)
TID mappers mostly complete; StoryProgressPatch redesigned with auto-logging for quest TIDs
dictionaries, and `ItemHandler.cs` stubs need real `SaveData` API calls implemented.

**Step 1 — Fill in class names (partially done)**
Many class names are now confirmed from existing mod research:
- ✅ `FishInteractionBody` (fish catching)
- ✅ `PlayerBreathHandler` (oxygen system)
- ✅ `InstanceItemChest` (treasure chests)
- ✅ `SeahorseRaceSessionPlay` (seahorse racing)
- ✅ `Farm.FarmCore`, `FishFarm.FishFarmPlayerView` (farms)
- ✅ `SushiBarManager`, `SushiBarCustomer` (restaurant)

Still needed (method names + remaining class names):
→ See `docs/CLASS_NAME_CHEAT_SHEET.md` for exactly what to search for.
→ Requires: `BepInEx/interop/Assembly-CSharp.dll` opened in dnSpy/ILSpy

**Step 2 — Implement ItemHandler game API calls**
All `ItemHandler.cs` methods are stubs. Need SaveSystem API calls to actually
give items to the player. Key stubs to implement:
- `GiveWeapon()` — unlock weapon in Duff's shop
- `UnlockRecipe()` — unlock recipe in restaurant
- `UpgradeDivingSuit()` / `UpgradeOxygenTank()` / `UpgradeHarpoon()`
- `GiveIngredient()` — add ingredient to inventory
- `UnlockRegion()` — enable area access / teleport destination

**Step 3 — Fill ID mapper dictionaries**
Each patch has a `*NameMapper` dictionary mapping internal game IDs → AP location names.
These are currently empty and need real IDs from the decompiler.
See `docs/CLASS_NAME_CHEAT_SHEET.md` for guidance.

### 📋 Phase 5 — Testing & Polish (Pending)
- Write unit tests for APWorld logic (`apworld/tests/`)
- Generate test seeds and verify logic
- Test all 5 victory conditions end-to-end
- Full playthrough verification

### ✅ Phase 6 — Jungle DLC Implementation (Substantially Complete)
- ✅ 57 lake fish (Utara Lake Upper/Lower, Lakebed Sea)
- New regions (Jungle Lake, Bancho Grill, Utara Village)
- New items and recipes
- Wire up `has_jungle_dlc` option to new content

---

## APWorld Development

### Adding a new item

```python
# In items.py, add to the appropriate dict:
"My New Item": ItemData(BASE_ID + <unique_id>, ItemClassification.useful, category="")

# Categories: "", "recipe", "restaurant", "trap", "dish_upgrade", "dlc_dredge", etc.
```

### Adding a new location

```python
# In locations.py, add to the appropriate dict:
"My New Location": LocationData(BASE_ID + <unique_id>, "Region Name", category="")

# Region names must match REGION_NAMES in regions.py
# Categories: "", "fish", "recipe", "dish_upgrade", "cooksta", "ecowatcher", etc.
```

### Adding a new region

```python
# In regions.py, add to REGION_NAMES and create_regions():
REGION_NAMES = {..., "My New Region"}

# In create_regions():
new_region = Region("My New Region", self.player, self.multiworld)
self.multiworld.regions.append(new_region)
parent_region.connect(new_region, "Rule Name",
    lambda state: state.has("Required Item", self.player))
```

### Testing APWorld logic

```powershell
cd apworld
python -c "
from davethediver import DaveDiverWorld
print(f'Items: {len(DaveDiverWorld.item_name_to_id)}')
print(f'Locations: {len(DaveDiverWorld.location_name_to_id)}')
# Check for duplicate IDs
item_ids = list(DaveDiverWorld.item_name_to_id.values())
assert len(item_ids) == len(set(item_ids)), 'Duplicate item IDs!'
loc_ids = list(DaveDiverWorld.location_name_to_id.values())
assert len(loc_ids) == len(set(loc_ids)), 'Duplicate location IDs!'
print('All IDs unique — OK')
"
```

---

## C# Client Development

### Project structure

```
client/DaveDiverAP/
├── Plugin.cs               — BepInEx entry, loads all components
├── ArchipelagoClient.cs    — AP server connection, sends checks, receives items
├── SlotData.cs             — Parses fill_slot_data() output from server
├── SaveData.cs             — JSON persistence (last server, received items, etc.)
├── ItemHandler.cs          — Routes received items to game effects (stubs)
├── LocationTracker.cs      — Maps game events to AP location names
├── GoalTracker.cs          — Tracks victory condition progress
├── DeathLinkHandler.cs     — Death Link send/receive
├── ItemQueue.cs            — Buffers items until boat scene
├── ModConfig.cs            — BepInEx config file (server, port, slot)
├── Patches/                — 17 HarmonyPatch classes
└── UI/
    ├── ConnectionUI.cs     — Main window (F9 toggle), Connection tab
    ├── HintUI.cs           — Hints tab
    ├── NotificationManager.cs — Toast notifications
    └── ProgressUI.cs       — Progress tab
```

### Building

```powershell
cd client

# Copy GamePath.props.example -> GamePath.props and set your game path
# Copy BepInEx interop DLLs to client/lib/ (see client/lib/README.md)

dotnet build DaveDiverAP/DaveDiverAP.csproj
# Output: DaveDiverAP/bin/Debug/net6.0/DaveDiverAP.dll
# Auto-copies to BepInEx/plugins/ if GamePath.props is set
```

### Writing a Harmony patch

```csharp
[HarmonyPatch(typeof(RealGameClass), "RealMethodName")]
[HarmonyPostfix]
public static void MyPatch_Postfix(object __instance)
{
    if (!ArchipelagoClient.IsConnected) return;

    // Read data from __instance
    // Call ArchipelagoClient.CheckLocation("Location Name");
}
```

**Key rules:**
- Always check `ArchipelagoClient.IsConnected` first
- Use `[HarmonyPostfix]` so the original method runs first
- Never write to ObscuredInt fields directly — use SaveSystem setters
- Items are delivered on the boat — don't give items during cutscenes

---

## Design Decisions

### Goals & Victory Conditions
- **5 goals**: Defeat Yawie (default) → Defeat All Bosses → Diamond Rank → Master Diver → 100%
- Defeat All Bosses is DLC-aware: Ebirah (Godzilla), Torben (Ichiban), Jungle bosses all required when DLC enabled
- Diamond Rank requires 720 followers + 375 Best Taste + 32 researched recipes (all tracked live in ProgressUI)

### Region & Depth Gating
- Depth uses **lenient OR logic** — suit level OR oxygen tanks, so players are never hard-blocked
- Diving Suit: 8 levels (40m → 800m), levels 7-8 = cold-resistant tiers that gate Glacial Passage and Glacier Zone
- Sea People Village: two routes (Sea People Gloves OR Teleport Mirror), both require Translator
- Glacier: two routes (Key to Tenzhin swim path OR Teleport), both require suit level 7+
- DLC regions (vortex zones, Jungle) only connected when DLC is enabled in options

### Region Map
```
Menu
└── Bancho Sushi (always accessible)
    ├── Blue Hole - Shallow (0–50m, always accessible)
    │   ├── Blue Hole - Mid (50–130m) ← suit lvl 2+ OR 2 oxygen tanks
    │   │   └── Blue Hole - Deep (130–250m) ← suit lvl 3+ OR 3 O2 tanks + harpoon
    │   │       ├── Glacial Passage ← Key to Tenzhin + suit lvl 7+
    │   │       │   └── Glacier Zone ← suit lvl 8 + Tech Suit Parts ×3
    │   │       │       └── Hydrothermal Vents (deepest)
    │   │       ├── Jellyfish Basin  ⟩ DREDGE DLC only,
    │   │       ├── Fog Coast        ⟩ require Vortex Entry
    │   │       └── Black Cliff      ⟩
    └── Sea People Village ← (Sea People Gloves OR Teleport) AND Translator
        └── Fish Farm ← Unlock Fish Farm
    ├── Vegetable Farm ← Unlock Vegetable Farm
    ├── Chicken Farm   ← Unlock Chicken Farm
    └── Utara Village  ← Jungle DLC only (Chapter 5 + travels to jungle)
        └── [8 Jungle sub-regions]
```

### DLC Isolation (end-to-end)
1. **Locations** — filtered by `should_include_location()` with DLC category check
2. **Items** — filtered by `should_include_item()` with DLC check before progression blanket rule
3. **Region connections** — only created when DLC option is enabled
4. **Goal conditions** — DLC bosses only required when DLC is enabled

### Staff System
- 24 named staff (21 base + 3 Ichiban DLC), each is a progression item
- `staff_training_depth` option controls check depth: none / hire_only / milestones / all_levels
- In `all_levels` mode, items become `Progressive [Name]` ×20 — finding Maki a 3rd time trains her to level 3

### Key Design Decisions (Summary)

- **Boat-only item delivery** — prevents items arriving mid-dive or during cutscenes
- **Lenient depth gating** — OR logic between suit/oxygen so no single item blocks depth
- **Progressive Diving Suit (8 levels)** — levels 7-8 are the cold-resistant tiers, naturally gating glacier areas
- **Teleport as bypass** — Teleport Mirror provides an alternative route to Sea People Village and Glacier, preventing bottlenecks
- **Chapters not used as gates** — physical item requirements naturally pace the story
- **DLC disabled by default** — players must opt in via YAML to get DLC content

See **[docs/REGION_ACCESS_DESIGN.md](docs/REGION_ACCESS_DESIGN.md)** for the full region graph and gating logic.
