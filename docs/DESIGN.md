# Dave the Diver - Archipelago Design Document

## Overview

This document outlines the design decisions for implementing Archipelago support in Dave the Diver.

## Game Analysis

### Core Gameplay Loops

1. **Diving Loop**
   - Dive into the Blue Hole
   - Catch fish with various weapons
   - Gather materials and treasures
   - Return to surface before oxygen runs out

2. **Restaurant Loop**
   - Serve customers at Bancho Sushi
   - Prepare dishes from caught fish
   - Earn money and reputation
   - Unlock new recipes and staff

3. **Progression Loop**
   - Purchase/unlock better equipment
   - Access deeper/new areas
   - Complete story chapters
   - Complete side quests and minigames

### Randomization Strategy

#### What to Randomize

**Progression Items** (Must have to complete game):
- Key story items (VIP cards, access passes)
- Major equipment upgrades (depth suits, oxygen tanks)
- Area unlocks (Glacier, Sea People Village)
- Essential weapons/tools

**Useful Items** (Helpful but not required):
- Recipe unlocks
- Equipment enhancements
- Restaurant upgrades
- Additional weapons

**Filler Items** (Common rewards):
- Money (Gold/Bei)
- Crafting materials
- Minor consumables

#### What NOT to Randomize

- Basic movement controls
- Tutorial items
- Core restaurant mechanics
- Dialogue/story content

### Location Categories

1. **Story Progression** (~10-15 checks)
   - Chapter completions
   - Major boss defeats

2. **Fish Catches** (~30-50 checks)
   - Rare fish catches
   - Boss fish
   - Specific fish required for recipes

3. **Restaurant Milestones** (~20-30 checks)
   - Customer count milestones
   - Rating achievements
   - Recipe unlocks
   - Special dish completions

4. **Side Content** (~40-60 checks)
   - Side quests
   - Minigame completions
   - Collectibles
   - Equipment purchases

5. **Achievements** (~20-30 checks)
   - Collection milestones
   - Money milestones
   - Completion goals

**Target Total: 150-200 locations**

## Technical Implementation

### APWorld (Python)

#### Regions

```
Menu
├── Bancho Sushi
│   ├── Restaurant Front
│   └── Equipment Shop
├── Blue Hole - Shallow (0-50m)
├── Blue Hole - Mid (50-100m)
├── Blue Hole - Deep (100m+)
├── Glacier Area
├── Sea People Village
└── Special Events
```

#### Logic Rules

Example rules for progression:
- Blue Hole - Mid: Requires "Oxygen Tank +2" OR "Enhanced Diving Suit"
- Blue Hole - Deep: Requires "Advanced Harpoon" AND "Deep Diving Suit"
- Glacier: Requires "Cold Protection Suit" AND "Story Chapter 4 Complete"
- Sea People Village: Requires "VIP Card" AND "Story Chapter 3 Complete"

### Client Mod (C#)

#### Key Components

1. **ArchipelagoClient.cs**
   - WebSocket connection to AP server
   - Send location checks
   - Receive items
   - Handle commands

2. **GameStateManager.cs**
   - Track unlocked items
   - Track checked locations
   - Persist state in save file

3. **Harmony Patches** (in Patches/)
   - `FishCatchPatch.cs` - Intercept fish catches
   - `RecipeUnlockPatch.cs` - Intercept recipe unlocks
   - `EquipmentPatch.cs` - Inject randomized equipment
   - `StoryProgressPatch.cs` - Track story completion
   - `SaveLoadPatch.cs` - Integrate AP state with saves

4. **ItemGranter.cs**
   - Grant items received from AP
   - Handle different item types appropriately

#### Data Flow

```
Game Event (e.g., catch rare fish)
    ↓
Harmony Patch intercepts
    ↓
Check if location already checked
    ↓
Send LocationCheck to AP server
    ↓
AP server processes, sends item(s)
    ↓
Client receives item notification
    ↓
ItemGranter adds item to player
    ↓
Update save file
```

## Implementation Phases

### Phase 1: Foundation (Week 1-2)
- [x] Set up development environment
- [ ] Complete game analysis (items, locations)
- [ ] Define complete item/location lists
- [ ] Create basic APWorld structure

### Phase 2: APWorld Development (Week 3-4)
- [ ] Implement all items
- [ ] Implement all locations
- [ ] Create regions and connections
- [ ] Write logic rules
- [ ] Add YAML options
- [ ] Test generation

### Phase 3: Client Mod Basics (Week 5-6)
- [ ] Set up BepInEx project
- [ ] Implement AP connection
- [ ] Create basic Harmony patches
- [ ] Implement save/load integration

### Phase 4: Feature Implementation (Week 7-10)
- [ ] Fish catch tracking
- [ ] Recipe/equipment randomization
- [ ] Story progress tracking
- [ ] Restaurant milestone tracking
- [ ] Item granting system

### Phase 5: Testing & Polish (Week 11-12)
- [ ] Solo playthrough testing
- [ ] Multiworld testing
- [ ] Balance adjustments
- [ ] Bug fixes
- [ ] Documentation

## Open Questions

1. **Fish Granularity**: Should every fish species be a location, or only rare/story ones?
   - **Decision**: Only rare, boss, and story-required fish

2. **Recipe Randomization**: Should recipes be items or auto-unlock on catching fish?
   - **Decision**: Recipes as items for better progression control

3. **Death Link**: Should the game support death link?
   - **Decision**: Optional, off by default

4. **Difficulty Options**: Should YAML include difficulty modifiers?
   - **Decision**: Yes - include options for starting equipment, fish rarity, etc.

## Next Steps

1. Play through the entire game while documenting:
   - All equipment and upgrades
   - All recipes
   - All story checkpoints
   - All side quests
   - All minigames

2. Create comprehensive spreadsheets for items and locations

3. Begin APWorld implementation
