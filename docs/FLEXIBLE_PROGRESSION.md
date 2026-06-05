# Flexible Progression - Alternative Route Design

## 🎯 Your Vision: Maximum Player Freedom

**Goal:** Players can visit areas in ANY order based purely on equipment, not forced story progression.

**Example:**
- Get Cold Protection Suit → Go to Glacier immediately
- Get Sea People Gloves → Go to Sea People Village immediately
- **Order doesn't matter!**

---

## ✅ Solution: Remove Teleport Mirror Requirement

### The New Approach

**Glacier Access:**
```python
# OLD (strict, requires Sea People Village first):
def can_access_glacier(state, player):
    return (
        state.has("Cold Protection Suit", player) and
        state.has("Teleport Mirror", player)  # Forces Sea People Village visit
    )

# NEW (flexible, equipment only):
def can_access_glacier(state, player):
    return state.has("Cold Protection Suit", player)  # That's it!
```

**Sea People Village Access:**
```python
def can_access_sea_people_village(state, player):
    return state.has("Sea People Gloves", player)  # Just gloves, no VIP card
```

---

## 🗺️ Updated Region Structure

```
Menu
└─> Bancho Sushi (Surface)
    ├─> Blue Hole - Shallow (no requirements)
    │   └─> Blue Hole - Mid (Progressive Oxygen ×2)
    │       └─> Blue Hole - Deep (Progressive Oxygen ×4 + Progressive Suit ×3)
    │
    ├─> Sea People Village (Sea People Gloves only)
    │   └─> Contains: Quests, shops, unique fish
    │
    └─> Glacier (Cold Protection Suit only)
        └─> Contains: Ice fishing, glacier quests, boss
```

**All parallel access!** No forced order.

---

## 🎮 Victory Condition: Complete All 6 Chapters

### Implementation

```python
# In __init__.py (world class)
def set_completion_condition(world):
    """Player must complete all 6 chapters to win"""
    world.multiworld.completion_condition[world.player] = lambda state: (
        state.has("Chapter 1 Complete", player) and
        state.has("Chapter 2 Complete", player) and
        state.has("Chapter 3 Complete", player) and
        state.has("Chapter 4 Complete", player) and
        state.has("Chapter 5 Complete", player) and
        state.has("Chapter 6 Complete", player)
    )
```

**Each chapter can be completed in ANY order** (as long as you have access to the areas).

---

## 📋 Revised Area Requirements

| Area | Requirements | Contains |
|------|-------------|----------|
| **Bancho Sushi** | None | Restaurant, shop, home base |
| **Blue Hole - Shallow** | None | Common fish, early quests |
| **Blue Hole - Mid** | Progressive Oxygen ×2 | Rare fish, better loot |
| **Blue Hole - Deep** | Progressive Oxygen ×4 + Progressive Suit ×3 + Progressive Harpoon ×2 | Boss fish, end-game content |
| **Sea People Village** | **Sea People Gloves** | Unique quests, special fish, shops |
| **Glacier** | **Cold Protection Suit** | Ice fishing, glacier quests, boss |

**No inter-area dependencies!** Each area is independent.

---

## 🎯 Chapter Progression

### Chapters can be completed in parallel areas:

| Chapter | Primary Location | Requirements |
|---------|-----------------|--------------|
| Chapter 1 | Blue Hole - Shallow | None (tutorial) |
| Chapter 2 | Blue Hole - Mid | Mid-depth access |
| Chapter 3 | Blue Hole - Deep | Deep access |
| Chapter 4 | Glacier | Cold Protection Suit |
| Chapter 5 | Sea People Village | Sea People Gloves |
| Chapter 6 | Blue Hole - Deep | Deep access + all previous chapters |

**Flexible order:**
- Get Cold Suit first? → Do Chapter 4 before Chapter 5
- Get Gloves first? → Do Chapter 5 before Chapter 4
- Get both? → Do them in any order!

---

## 🔄 Alternative Access Methods

### Option 1: Single Equipment Item (RECOMMENDED for your vision)

**Just the suit for Glacier:**
```python
area_unlock_items: Dict[str, ItemData] = {
    "Cold Protection Suit": ItemData(BASE_ID + 100, ItemClassification.progression),
    "Sea People Gloves": ItemData(BASE_ID + 105, ItemClassification.progression),
}
```

**No teleport mirror needed!** Client mod can handle this:
```csharp
// In areas.cs
public bool CanAccessGlacier()
{
    return GameState.HasItem("Cold Protection Suit");
    // No mirror check - just unlock glacier on the map
}
```

---

### Option 2: Direct Travel Unlocks

**Add "travel unlock" items:**
```python
area_unlock_items: Dict[str, ItemData] = {
    "Cold Protection Suit": ItemData(BASE_ID + 100, ItemClassification.progression),
    "Sea People Gloves": ItemData(BASE_ID + 105, ItemClassification.progression),
    
    # Travel methods (optional, for lore/flavor)
    "Glacier Boat Charter": ItemData(BASE_ID + 106, ItemClassification.useful),  # Alternative to suit
    "Sea People Translator": ItemData(BASE_ID + 107, ItemClassification.useful),  # Alternative to gloves
}
```

**Logic with alternatives:**
```python
def can_access_glacier(state, player):
    return (
        state.has("Cold Protection Suit", player) or
        state.has("Glacier Boat Charter", player)  # Can pay for boat if no suit
    )
```

---

## 💡 What About the Teleport Mirror?

### Three Options:

### 1. Remove it entirely (SIMPLEST)
- Just delete Teleport Mirror from items and locations
- Glacier is accessible directly with Cold Suit
- Client mod handles making Glacier appear on map

### 2. Make it optional QoL (RECOMMENDED)
```python
# Teleport Mirror is NOT required, but useful
"Teleport Mirror": ItemData(BASE_ID + 102, ItemClassification.useful),  # Changed from progression!
```

**What it does:**
- **Without mirror:** Can still reach Glacier (by boat or other means)
- **With mirror:** Fast travel between areas (quality of life)

**Location check:**
```python
"Sea People Village: Obtain Teleport Mirror": LocationData(BASE_ID + 386, "Sea People Village")
```

### 3. Make it enable fast travel only
- Glacier is accessible without it
- Having it unlocks fast travel system
- Client mod adds teleport option when you have the mirror

---

## 🎮 Implementation Changes

### items.py
```python
# === AREA UNLOCK ITEMS (Specific Items) ===
area_unlock_items: Dict[str, ItemData] = {
    # Glacier access
    "Cold Protection Suit": ItemData(BASE_ID + 100, ItemClassification.progression),
    
    # Sea People Village access
    "Sea People Gloves": ItemData(BASE_ID + 105, ItemClassification.progression),
    
    # Optional QoL (not required for progression)
    "Teleport Mirror": ItemData(BASE_ID + 102, ItemClassification.useful),  # Fast travel only
}
```

### rules.py
```python
def set_rules(world):
    player = world.player
    
    # Simple, independent access rules
    set_rule(
        world.get_entrance("Bancho Sushi -> Glacier", player),
        lambda state: state.has("Cold Protection Suit", player)
    )
    
    set_rule(
        world.get_entrance("Blue Hole -> Sea People Village", player),
        lambda state: state.has("Sea People Gloves", player)
    )
    
    # Victory condition: All 6 chapters
    world.multiworld.completion_condition[player] = lambda state: (
        state.has("Chapter 1 Complete", player) and
        state.has("Chapter 2 Complete", player) and
        state.has("Chapter 3 Complete", player) and
        state.has("Chapter 4 Complete", player) and
        state.has("Chapter 5 Complete", player) and
        state.has("Chapter 6 Complete", player)
    )
```

---

## 📊 Progression Scenarios

### Scenario 1: Glacier First
1. ✅ Receive **Cold Protection Suit** from Pokemon player
2. ✅ **Immediately go to Glacier!**
3. ✅ Complete Chapter 4 (Glacier chapter)
4. ✅ Later: Get Sea People Gloves
5. ✅ Go to Sea People Village
6. ✅ Complete Chapter 5
7. ✅ Complete remaining chapters
8. ✅ Victory!

### Scenario 2: Sea People First
1. ✅ Receive **Sea People Gloves** from Zelda player
2. ✅ **Immediately go to Sea People Village!**
3. ✅ Complete Chapter 5
4. ✅ Later: Get Cold Protection Suit
5. ✅ Go to Glacier
6. ✅ Complete Chapter 4
7. ✅ Complete remaining chapters
8. ✅ Victory!

### Scenario 3: Parallel Progression
1. ✅ Get **both** Cold Suit and Sea People Gloves early
2. ✅ Access **both** Glacier and Sea People Village
3. ✅ Complete chapters 4 and 5 in **any order**
4. ✅ Maximum freedom!

---

## 🎯 Victory Condition Options (YAML)

```yaml
dave_the_diver:
  goal:
    all_chapters: 0       # Complete all 6 chapters (default)
    chapter_6_only: 1     # Just complete final chapter
    specific_count: 2     # Complete X chapters (configurable)
    
  chapters_required: 6    # If using specific_count goal
  
  # Optional requirements
  require_all_recipes: false
  require_all_fish: false
  require_all_ecowatcher: false
```

**Default: all_chapters** - Must complete all 6, but in any order!

---

## 🔧 Client Mod Changes Needed

### Make areas accessible directly:

```csharp
public class AreaAccessManager
{
    public void UpdateAreaAccess()
    {
        // Glacier: Just need suit
        if (GameState.HasItem("Cold Protection Suit"))
        {
            UnlockGlacierOnMap();  // Make it appear/accessible
        }
        
        // Sea People Village: Just need gloves
        if (GameState.HasItem("Sea People Gloves"))
        {
            UnlockSeaPeopleVillageOnMap();
        }
        
        // Teleport Mirror: Optional fast travel
        if (GameState.HasItem("Teleport Mirror"))
        {
            EnableFastTravelSystem();  // QoL feature
        }
    }
    
    private void UnlockGlacierOnMap()
    {
        // Add Glacier to accessible locations list
        // Could add a boat dock option, or direct map marker
        // Player can now travel to Glacier from surface
    }
}
```

---

## ✅ Final Recommendation

### For Maximum Freedom (Your Vision):

1. ✅ **Remove Teleport Mirror requirement** for Glacier
2. ✅ **Glacier requires:** Cold Protection Suit only
3. ✅ **Sea People Village requires:** Sea People Gloves only
4. ✅ **Keep Teleport Mirror** as optional fast travel (useful item)
5. ✅ **Victory condition:** Complete all 6 chapters (any order)

### Items to implement:
```python
# Progression items (required)
"Cold Protection Suit": ItemData(BASE_ID + 100, ItemClassification.progression),
"Sea People Gloves": ItemData(BASE_ID + 105, ItemClassification.progression),

# Useful items (optional)
"Teleport Mirror": ItemData(BASE_ID + 102, ItemClassification.useful),  # Fast travel
```

### Victory condition:
```python
# Must have all chapter completion items
completion_condition = has_all_chapters(state, player)
```

---

## 💡 Why This Works Better

**Your original concern:**
> "Cold Protection Suit might be sent earlier, but can't go to glacier without Sea People Village"

**This solution:**
✅ Cold Protection Suit → **Immediately access Glacier!**  
✅ Sea People Gloves → **Immediately access Sea People Village!**  
✅ **No forced order!**  
✅ **Maximum player freedom!**  
✅ **True Archipelago philosophy!**

---

## 🎮 The Archipelago Experience

**Classic randomizers:** Linear progression, fixed route  
**Archipelago with this design:** 
- Items from **any game** unlock **your areas**
- Access areas in **any order**
- Complete chapters in **any order**
- **True multiworld freedom!**

---

**Should I implement this flexible approach instead? It aligns much better with the Archipelago philosophy of player freedom!** 🎮🌊

Your choice:
1. **Flexible** (Cold Suit → Glacier, Gloves → Village, any order) ⭐ RECOMMENDED
2. **Strict** (Must visit Sea People Village first to get mirror)

Which approach do you prefer?