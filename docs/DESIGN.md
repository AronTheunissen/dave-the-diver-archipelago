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

> **Status as of June 2026:** Phases 1–5 complete. Phase 6 (Jungle DLC) in progress pending wiki data.

### Phase 1: Foundation ✅
- [x] Set up development environment
- [ ] Complete game analysis (items, locations)
- [ ] Define complete item/location lists
- [ ] Create basic APWorld structure

### Phase 2: APWorld Development ✅
- [x] Implement all items (276 items across all categories)
- [x] Implement all locations (1,134 locations across 15 regions)
- [x] Create regions and connections (15 regions with dual-route access)
- [x] Write logic rules (depth gating, key item requirements, region access)
- [x] Add YAML options (25 options covering all systems)
- [x] Unit tests (55/55 passing)

### Phase 3: Client Mod Basics ✅
- [x] Set up BepInEx 6 IL2CPP project
- [x] Implement AP connection with auto-reconnect
- [x] Create 17 Harmony patches (all real class names confirmed via dump.cs)
- [x] Implement save/load integration (SaveData + SaveLoadPatch)

### Phase 4: Feature Implementation ✅
- [x] Fish catch tracking (203 species via FishCatchPatch)
- [x] Recipe/dish upgrade randomization (RecipeUnlockPatch + 549 dish checks)
- [x] Story progress tracking (StoryProgressPatch, ChapterManager)
- [x] Restaurant/Cooksta milestone tracking (CookstaPatch, RestaurantPatch)
- [x] Item granting system (ItemHandler — all game API calls implemented)
- [x] Weapon tracking (WeaponCraftPatch, 79 variants)
- [x] Death Link, hints, goal tracker, progress UI, toast notifications

### Phase 5: Testing & Polish 🔧 (In Progress)
- [ ] Solo playthrough testing (blocked on TID mapping)
- [ ] Fill in weapon/recipe/quest TID mapper dictionaries via UnityExplorer
- [ ] Multiworld testing
- [ ] Balance adjustments
- [ ] Bug fixes

### Phase 6: Jungle DLC ⏳
- [x] Region structure (8 regions) and logic rules
- [x] Placeholder locations (100+) — ready for wiki data
- [ ] Full fish list for all Jungle regions (wiki not yet updated)
- [ ] All Bancho Grill recipes

## Design Decisions

All major design questions have been resolved:

1. **Fish Granularity** — Every fish species is a location (203 base game + DLC), filterable by `fish_checks` option (none / rare_only / all)
2. **Recipe Randomization** — Recipes are AP items; dish research tiers are location checks (549 checks)
3. **Death Link** — Implemented, optional, off by default
4. **Difficulty Options** — 25 YAML options covering starting equipment, fish rarity, content toggles, and DLC flags
5. **Progressive Equipment** — 8-level diving suit (40m→800m), 6 oxygen tanks, 4 harpoon levels
6. **Depth Gating** — Lenient OR logic (suit OR oxygen) so players are never hard-blocked

## Current Status

See **[TODO.md](../TODO.md)** for the full prioritized task list.
