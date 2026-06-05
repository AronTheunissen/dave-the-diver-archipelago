# Progression Logic Fix - Teleport Mirror Solution

## 🚨 The Problem You Identified

**In vanilla Dave the Diver:**
```
Blue Hole → Sea People Village → (Get teleport mirror) → Glacier
```

**The issue with randomizer:**
- Player receives "Cold Protection Suit" early (from another game)
- But can't access Glacier yet because...
- Glacier requires teleport mirror
- Teleport mirror is obtained in Sea People Village
- Sea People Village requires Sea People Gloves
- What if gloves haven't been found yet?

**Result:** Player has the right equipment but wrong item order = softlock!

---

## ✅ Solution: Make Teleport Mirror a Progression Item

### Option 1: Teleport Mirror as Item (RECOMMENDED)

**Add to items.py:**
```python
area_unlock_items: Dict[str, ItemData] = {
    # Glacier access
    "Cold Protection Suit": ItemData(BASE_ID + 100, ItemClassification.progression),
    "Teleport Mirror": ItemData(BASE_ID + 102, ItemClassification.progression),  # NEW!
    
    # Sea People Village access
    "Sea People Gloves": ItemData(BASE_ID + 105, ItemClassification.progression),
    "VIP Card": ItemData(BASE_ID + 110, ItemClassification.progression),
}
```

**Updated logic rules:**
```python
# In rules.py

def can_access_sea_people_village(state, player):
    """Sea People Village requires gloves"""
    return (
        state.has("VIP Card", player) and
        state.has("Sea People Gloves", player)
    )

def can_access_glacier(state, player):
    """Glacier requires BOTH suit AND mirror"""
    return (
        state.has("Cold Protection Suit", player) and
        state.has("Teleport Mirror", player)  # Now required!
    )
```

**Where to place Teleport Mirror as a check:**
```python
# In locations.py
quest_locations: Dict[str, LocationData] = {
    "Complete Sea People Quest: Teleport Mirror": LocationData(BASE_ID + 385, "Sea People Village"),
}
```

**Progression flow:**
1. Get Sea People Gloves → Access village
2. Complete quest in village → Receive Teleport Mirror (as Archipelago item)
3. Teleport Mirror is sent to item pool
4. Eventually receive Teleport Mirror
5. If you also have Cold Protection Suit → Can access Glacier!

**This ensures:** You MUST access Sea People Village before Glacier (as in vanilla)

---

### Option 2: Alternative Path System

**Make Glacier accessible via two routes:**

```python
def can_access_glacier(state, player):
    """Two ways to reach Glacier"""
    return state.has("Cold Protection Suit", player) and (
        # Route 1: Traditional (via teleport mirror from Sea People Village)
        state.has("Teleport Mirror", player) or
        
        # Route 2: Alternative (direct boat access from surface)
        state.has("Glacier Boat Access", player)
    )
```

**Add items:**
```python
"Teleport Mirror": ItemData(BASE_ID + 102, ItemClassification.progression),
"Glacier Boat Access": ItemData(BASE_ID + 103, ItemClassification.progression),
```

**This allows:**
- Early Glacier access if you get boat access item
- Traditional route via Sea People Village → Mirror
- More flexibility for randomization

---

### Option 3: Remove Vanilla Restrictions (NOT RECOMMENDED)

**Just let players go anywhere with the right suit:**
```python
def can_access_glacier(state, player):
    return state.has("Cold Protection Suit", player)  # That's it!
```

**Problems:**
- Breaks vanilla progression flow
- Mirror becomes meaningless
- Sea People Village might become optional
- Not faithful to the original game

---

## 🎯 Recommended Solution: Option 1 (Teleport Mirror as Item)

### Implementation

**1. Add Teleport Mirror item:**
```python
# items.py
area_unlock_items: Dict[str, ItemData] = {
    "Cold Protection Suit": ItemData(BASE_ID + 100, ItemClassification.progression),
    "Teleport Mirror": ItemData(BASE_ID + 102, ItemClassification.progression),
    "Glacier Access Permit": ItemData(BASE_ID + 101, ItemClassification.progression),
    "Sea People Gloves": ItemData(BASE_ID + 105, ItemClassification.progression),
    "VIP Card": ItemData(BASE_ID + 110, ItemClassification.progression),
}
```

**2. Create location check for getting the mirror:**
```python
# locations.py
quest_locations: Dict[str, LocationData] = {
    # ... other quests
    "Sea People Village: Obtain Teleport Mirror": LocationData(BASE_ID + 386, "Sea People Village"),
}
```

**3. Update access rules:**
```python
# rules.py
def set_rules(world):
    player = world.player
    
    # Sea People Village requires gloves and VIP card
    set_rule(
        world.get_entrance("Blue Hole -> Sea People Village", player),
        lambda state: (
            state.has("VIP Card", player) and
            state.has("Sea People Gloves", player)
        )
    )
    
    # Glacier requires BOTH suit AND mirror
    set_rule(
        world.get_entrance("Sea People Village -> Glacier", player),
        lambda state: (
            state.has("Cold Protection Suit", player) and
            state.has("Teleport Mirror", player)
        )
    )
    
    # Also require Chapter 4 completion (story reason)
    add_rule(
        world.get_entrance("Sea People Village -> Glacier", player),
        lambda state: state.has("Chapter 4 Complete", player)
    )
```

**4. Client mod implementation:**
```csharp
// In ItemGranter.cs
public void GrantItem(string itemName)
{
    switch (itemName)
    {
        case "Teleport Mirror":
            UnlockTeleportMirror();
            ShowNotification("Received Teleport Mirror! Can now teleport to Glacier.");
            break;
            
        case "Cold Protection Suit":
            UnlockColdSuit();
            ShowNotification("Received Cold Protection Suit! Can survive in Glacier.");
            break;
            
        // When you have BOTH, glacier becomes accessible
    }
}
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
    ├─> Sea People Village (VIP Card + Sea People Gloves)
    │   └─> [Location Check: Obtain Teleport Mirror]
    │
    └─> Glacier (Cold Protection Suit + Teleport Mirror + Chapter 4 Complete)
```

---

## 📋 Complete Item Requirements by Area

### Bancho Sushi
**Requirements:** None (starting location)

### Blue Hole - Shallow
**Requirements:** None

### Blue Hole - Mid
**Requirements:**
- Progressive Oxygen Tank ×2 **OR**
- Progressive Diving Suit ×2

### Blue Hole - Deep
**Requirements:**
- Progressive Oxygen Tank ×4 **AND**
- Progressive Diving Suit ×3 **AND**
- Progressive Harpoon ×2

### Sea People Village
**Requirements:**
- VIP Card **AND**
- Sea People Gloves

**Important check here:** Obtain Teleport Mirror (location check)

### Glacier
**Requirements:**
- Cold Protection Suit **AND**
- Teleport Mirror **AND**
- Chapter 4 Complete

---

## 🎮 How This Plays Out

### Scenario 1: Normal Progression
1. Find VIP Card + Sea People Gloves
2. Access Sea People Village
3. Complete quest → Check "Obtain Teleport Mirror" location
4. Teleport Mirror is in item pool
5. Eventually receive Teleport Mirror
6. Find/receive Cold Protection Suit
7. Complete Chapter 4
8. Can now access Glacier!

### Scenario 2: Early Cold Suit
1. Receive Cold Protection Suit from another player early
2. "Nice! But I can't use it yet..."
3. Still need to get to Sea People Village first
4. Find VIP Card + Sea People Gloves
5. Access village, get Teleport Mirror check
6. Receive Teleport Mirror
7. NOW the Cold Suit is useful → Glacier access!

### Scenario 3: Early Mirror (from another player)
1. Someone else gets "Obtain Teleport Mirror" check in their game
2. Mirror is sent to you
3. "I have a mirror but nowhere to use it yet..."
4. Eventually get Cold Protection Suit
5. Can now access Glacier!

**No softlocks!** Items can arrive in any order, but you need ALL required items.

---

## 🔧 Additional Teleport Mirror Uses

In vanilla Dave the Diver, the mirror is used for fast travel. We can expand this:

**Optional YAML setting:**
```yaml
dave_the_diver:
  teleport_mirror_behavior:
    required_for_glacier: true   # Default: true
    enables_fast_travel: true    # Default: true (QoL when you have it)
    enables_shortcuts: false     # Could unlock shortcuts between areas
```

---

## ✅ Final Recommendation

**Implement Option 1:**
1. ✅ Add "Teleport Mirror" as progression item
2. ✅ Create location check "Sea People Village: Obtain Teleport Mirror"
3. ✅ Require mirror for Glacier access
4. ✅ This maintains vanilla progression flow
5. ✅ Prevents softlocks
6. ✅ Items can arrive in any order but all are needed

**Progression chain:**
```
Sea People Gloves → Sea People Village → Teleport Mirror
                                              ↓
Cold Protection Suit + Teleport Mirror + Chapter 4 → Glacier
```

---

## 💡 Why This Works

1. **Prevents softlocks:** Can't access Glacier without proper progression
2. **Maintains vanilla logic:** Must visit Sea People Village before Glacier
3. **Flexible item order:** Suit and Mirror can arrive in any order
4. **Multiworld friendly:** Items can come from any game
5. **Clear requirements:** Players understand what they need

---

## 🚫 Items That Don't Prevent This Issue

**These alone are NOT enough:**
- ❌ Just "Cold Protection Suit" - Can get it but can't reach Glacier
- ❌ Just "Glacier Access Permit" - Still need mirror to teleport there
- ❌ Just "Chapter 4 Complete" - Story is done but still can't get there

**You need ALL of:**
- ✅ Cold Protection Suit (survive cold)
- ✅ Teleport Mirror (reach glacier location)
- ✅ Chapter 4 Complete (story gate)

---

**Great catch on this progression issue! The Teleport Mirror solution maintains vanilla flow while preventing softlocks.** 🎮🪞

Should I implement this fix now?
