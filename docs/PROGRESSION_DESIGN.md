# Progression Design - Dave the Diver Archipelago

## The Core Question

**Should areas be locked by story progression, or purely by equipment/items?**

---

## 🎯 Recommended Approach: **Equipment-Based Progression**

**YES - Make the map open from the start, but gate access with items!**

This is the **ideal approach** for Dave the Diver because:

1. ✅ **Fits Archipelago philosophy** - Items from other games unlock your progression
2. ✅ **Player agency** - You can explore freely once you get the right gear
3. ✅ **Natural gating** - The game already uses depth/oxygen as barriers
4. ✅ **No softlocks** - Can't get stuck waiting for story progression
5. ✅ **Replayability** - Different item orders = different routes each time

---

## 🔓 Progressive Item System

### Progressive Oxygen Tank

**Instead of:** "Oxygen Tank +1", "Oxygen Tank +2", etc.  
**Use:** "Progressive Oxygen Tank" (received 5 times)

```python
# items.py
"Progressive Oxygen Tank": ItemData(BASE_ID + 100, ItemClassification.progression, count=5),
```

**How it works:**
- Receive it 1st time → Can dive to 30m
- Receive it 2nd time → Can dive to 50m  
- Receive it 3rd time → Can dive to 75m
- Receive it 4th time → Can dive to 100m
- Receive it 5th time → Can dive to 130m+

**Logic rules:**
```python
# rules.py
def can_reach_mid_depths(state, player):
    return state.has("Progressive Oxygen Tank", player, 2)  # Need 2 upgrades

def can_reach_deep(state, player):
    return state.has("Progressive Oxygen Tank", player, 4)  # Need 4 upgrades
```

---

### Progressive Harpoon Gun

```python
"Progressive Harpoon": ItemData(BASE_ID + 0, ItemClassification.progression, count=3),
```

- Level 1: Basic Harpoon Gun (start with this)
- Level 2: Enhanced Harpoon Gun
- Level 3: Advanced Harpoon Gun

**Logic:**
```python
def can_defeat_mid_tier_enemies(state, player):
    return state.has("Progressive Harpoon", player, 2)

def can_defeat_boss_fish(state, player):
    return state.has("Progressive Harpoon", player, 3)
```

---

### Progressive Diving Suit

```python
"Progressive Diving Suit": ItemData(BASE_ID + 5, ItemClassification.progression, count=4),
```

- Level 1: Basic Suit (50m max)
- Level 2: Enhanced Suit (100m max)
- Level 3: Deep Sea Suit (150m+ max)
- Level 4: Reinforced Suit (extra durability)

---

### Progressive Cargo Capacity

```python
"Progressive Cargo": ItemData(BASE_ID + 10, ItemClassification.progression, count=5),
```

Each upgrade increases weight capacity, allowing more fish/items per dive.

---

## 🗺️ Area Access Requirements

### Bancho Sushi (Restaurant)
**Requirements:** None (always accessible)

### Blue Hole - Shallow (0-50m)
**Requirements:** None (starting area)

### Blue Hole - Mid (50-100m)
**Requirements:**
- Progressive Oxygen Tank × 2 **OR**
- Progressive Diving Suit × 2

**Logic:**
```python
set_rule(
    world.get_entrance("Access Blue Hole Mid", player),
    lambda state: state.has("Progressive Oxygen Tank", player, 2) or
                  state.has("Progressive Diving Suit", player, 2)
)
```

### Blue Hole - Deep (100m+)
**Requirements:**
- Progressive Oxygen Tank × 4 **AND**
- Progressive Diving Suit × 3 **AND**
- Progressive Harpoon × 2 (enemies are tougher)

### Glacier Area
**Requirements:**
- Cold Protection Suit (specific item, not progressive)
- Chapter 4 Complete (story gate - explained below)

**Why story gate?** Some areas are physically locked until story events happen.

### Sea People Village
**Requirements:**
- Sea People Gloves (specific item) **OR** Mermaid Suit
- VIP Card (story item)

**This is your example!** ✅

---

## 🎮 How It Works In Practice

### Example Scenario:

**Early game:**
- Player has: Basic suit, Basic harpoon, 1× Progressive Oxygen
- Can access: Shallow area only (0-50m)
- Finds: Common fish, early recipes

**Mid game:**
- Receives from another player: Progressive Oxygen × 2
- Can now access: Mid depths (50-100m)
- Finds: Rare fish, better treasure chests

**Late game:**
- Receives: Progressive Oxygen × 4, Progressive Suit × 3, Progressive Harpoon × 3
- Can access: Deep areas, can defeat bosses
- Completes end-game content

---

## 📋 All Progressive Items

### Equipment Progression

| Progressive Item | Count | Purpose |
|-----------------|-------|---------|
| Progressive Oxygen Tank | 5 | Dive deeper (30m → 50m → 75m → 100m → 130m+) |
| Progressive Diving Suit | 4 | Depth limit & durability |
| Progressive Harpoon | 3 | Combat power (Basic → Enhanced → Advanced) |
| Progressive Cargo | 5 | Carry capacity |
| Progressive Swimming Speed | 3 | Move faster underwater |

### Ability Unlocks (Non-Progressive)

| Item | Purpose |
|------|---------|
| Cold Protection Suit | Access Glacier |
| Sea People Gloves | Access Sea People Village |
| Drone | Auto-collect items |
| Fish Radar | See fish locations |
| Enhanced Night Vision | See in dark areas |

### Key Items (Story/Area Access)

| Item | Purpose |
|------|---------|
| VIP Card | Required for certain areas |
| Sea People Pass | Alternative to gloves |
| Glacier Access Permit | Story unlock |

---

## 🚫 What About Story Progression?

### Hybrid Approach: Mostly Equipment, Some Story Gates

**Equipment gates:** 90% of progression  
**Story gates:** 10% of progression (unavoidable)

**Why we need some story gates:**
- Certain areas don't exist until story events (e.g., Glacier opens in Chapter 4)
- Some NPCs don't appear until certain chapters
- Certain mechanics unlock through story (e.g., Fish Farm)

**Solution:** Make story completions location checks that grant key items

```python
# Example: Chapter 4 completion grants Glacier Access
"Complete Chapter 4": LocationData(BASE_ID + 4, "Blue Hole - Deep")
# When checked, player receives:
"Glacier Access Permit": ItemData(BASE_ID + 402, ItemClassification.progression)

# Logic:
def can_access_glacier(state, player):
    return (state.has("Cold Protection Suit", player) and
            state.has("Glacier Access Permit", player))
```

---

## ⚖️ Balance Considerations

### Too Many Progressive Items?

**Problem:** If everything is progressive, early game feels slow.

**Solution:** Start with some basic equipment:
```python
# In options.py
class StartingEquipment(DefaultOnToggle):
    """Start with basic diving gear"""
    display_name = "Starting Equipment"
    
# Player starts with:
# - Basic Harpoon Gun
# - Basic Diving Suit (50m)
# - 1× Oxygen Tank
# - Basic Cargo capacity
```

### Alternative Paths

**Don't make everything linear!**

```python
# BAD - Only one path:
def can_reach_mid_depths(state, player):
    return state.has("Progressive Oxygen Tank", player, 2)

# GOOD - Multiple paths:
def can_reach_mid_depths(state, player):
    return (state.has("Progressive Oxygen Tank", player, 2) or
            state.has("Progressive Diving Suit", player, 2) or
            (state.has("Progressive Oxygen Tank", player, 1) and
             state.has("Oxygen Efficiency Upgrade", player)))
```

---

## 🎯 Recommended Progressive Items

### High Priority (Core Progression)

1. ✅ **Progressive Oxygen Tank** (5 levels) - Most important
2. ✅ **Progressive Harpoon** (3 levels) - Combat/catching
3. ✅ **Progressive Diving Suit** (3-4 levels) - Depth & protection
4. ✅ **Progressive Cargo** (3-5 levels) - Carry capacity

### Medium Priority (Useful but Optional)

5. ⭐ **Progressive Drone** (2 levels) - Basic → Enhanced
6. ⭐ **Progressive Crab Trap** (2 levels) - More traps
7. ⭐ **Progressive Swimming Speed** (2-3 levels) - QoL

### Low Priority (Can be Single Items)

- Cold Protection Suit (one-time unlock)
- Sea People Gloves (one-time unlock)
- Fish Radar (one-time unlock)
- VIP Card (story key item)

---

## 🔧 Implementation Example

### items.py
```python
equipment_progressive: Dict[str, ItemData] = {
    # Core progression
    "Progressive Oxygen Tank": ItemData(BASE_ID + 100, ItemClassification.progression, count=5),
    "Progressive Harpoon": ItemData(BASE_ID + 105, ItemClassification.progression, count=3),
    "Progressive Diving Suit": ItemData(BASE_ID + 110, ItemClassification.progression, count=3),
    "Progressive Cargo": ItemData(BASE_ID + 115, ItemClassification.progression, count=4),
    
    # Useful progression
    "Progressive Drone": ItemData(BASE_ID + 120, ItemClassification.useful, count=2),
    
    # Single unlocks
    "Cold Protection Suit": ItemData(BASE_ID + 125, ItemClassification.progression),
    "Sea People Gloves": ItemData(BASE_ID + 126, ItemClassification.progression),
    "Fish Radar": ItemData(BASE_ID + 127, ItemClassification.useful),
}
```

### rules.py
```python
def set_rules(world):
    player = world.player
    
    # Mid depths require oxygen OR suit upgrades
    set_rule(
        world.get_entrance("Blue Hole Shallow -> Mid", player),
        lambda state: (
            state.has("Progressive Oxygen Tank", player, 2) or
            state.has("Progressive Diving Suit", player, 2)
        )
    )
    
    # Deep depths require BOTH oxygen AND suit AND harpoon
    set_rule(
        world.get_entrance("Blue Hole Mid -> Deep", player),
        lambda state: (
            state.has("Progressive Oxygen Tank", player, 4) and
            state.has("Progressive Diving Suit", player, 3) and
            state.has("Progressive Harpoon", player, 2)
        )
    )
    
    # Glacier requires specific items
    set_rule(
        world.get_entrance("Bancho Sushi -> Glacier", player),
        lambda state: (
            state.has("Cold Protection Suit", player) and
            state.has("Glacier Access Permit", player)  # From Chapter 4
        )
    )
    
    # Sea People Village - multiple paths!
    set_rule(
        world.get_entrance("Blue Hole -> Sea People Village", player),
        lambda state: (
            state.has("VIP Card", player) and
            (state.has("Sea People Gloves", player) or
             state.has("Mermaid Suit", player))
        )
    )
```

---

## 💡 Benefits of This Approach

### For Players:
✅ Exploration feels rewarding  
✅ Different routes each seed  
✅ Items from other games feel meaningful  
✅ Can plan progression strategies  

### For Randomizer:
✅ Clear progression logic  
✅ No softlocks (can always progress with right items)  
✅ Scalable difficulty via YAML options  
✅ Works well in multiworld  

### For Dave the Diver:
✅ Respects game's natural progression  
✅ Depth-based exploration still makes sense  
✅ Equipment upgrades feel meaningful  
✅ Story still flows naturally  

---

## 🎮 Client Mod Implementation

### Tracking Progressive Items

```csharp
public class ProgressiveItemTracker
{
    private Dictionary<string, int> progressiveCounts = new Dictionary<string, int>();
    
    public void GrantProgressive(string itemName)
    {
        if (!progressiveCounts.ContainsKey(itemName))
            progressiveCounts[itemName] = 0;
            
        progressiveCounts[itemName]++;
        
        switch (itemName)
        {
            case "Progressive Oxygen Tank":
                ApplyOxygenUpgrade(progressiveCounts[itemName]);
                break;
            case "Progressive Harpoon":
                ApplyHarpoonUpgrade(progressiveCounts[itemName]);
                break;
            // etc.
        }
    }
    
    private void ApplyOxygenUpgrade(int level)
    {
        // Level 1: 30m max
        // Level 2: 50m max
        // Level 3: 75m max
        // Level 4: 100m max
        // Level 5: 130m+ max
        
        int maxDepth = level switch
        {
            1 => 30,
            2 => 50,
            3 => 75,
            4 => 100,
            _ => 150
        };
        
        PlayerStats.SetMaxDivingDepth(maxDepth);
    }
}
```

---

## 📊 YAML Options

```yaml
dave_the_diver:
  # Starting equipment
  starting_oxygen_level: 1  # 0-5 (how many progressive oxygen to start with)
  starting_harpoon_level: 1  # 0-3
  starting_suit_level: 1     # 0-3
  
  # Progression difficulty
  oxygen_requirements:
    lenient: 0    # Only 3 oxygen upgrades needed for deep
    normal: 1     # 4 upgrades needed (default)
    strict: 2     # 5 upgrades needed
    
  # Area access
  require_story_for_glacier: true   # Default: true
  require_story_for_sea_people: false  # Default: false (gloves enough)
```

---

## ✅ Final Recommendation

**Use equipment-based progression with progressive items!**

**Progressive items:**
- Oxygen Tank × 5
- Harpoon × 3
- Diving Suit × 3
- Cargo × 4

**Specific unlock items:**
- Cold Protection Suit (Glacier)
- Sea People Gloves (Sea People Village)
- Fish Radar (useful QoL)
- VIP Card (story key)

**Minor story gates:**
- Glacier Access Permit (from Chapter 4)
- Fish Farm Unlock (from story progression)

**Result:** 90% equipment-driven, 10% story-driven = Perfect balance!

---

**This gives you the open exploration feel of Archipelago while respecting Dave the Diver's natural progression!** 🎮🌊
