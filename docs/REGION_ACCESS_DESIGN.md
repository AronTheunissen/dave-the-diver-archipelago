# Region Access & Progression Design

> This document explains how regions are gated in the Dave the Diver randomizer
> and the design decisions behind the progression system.

## Region Map

```
Menu
└── Bancho Sushi (always accessible)
    ├── Blue Hole - Shallow (0–50m, always accessible)
    │   ├── Blue Hole - Mid (50–130m) ← suit lvl 2+ OR 2 oxygen tanks
    │   │   └── Blue Hole - Deep (130–250m) ← suit lvl 3+ OR 3 oxygen tanks + harpoon
    │   │       ├── Glacial Passage (locked by Key to Tenzhin + suit lvl 7+)
    │   │       │   └── Glacier Zone (locked by suit lvl 8 / Tech Suit Parts ×3)
    │   │       │       └── Hydrothermal Vents (deepest area)
    │   │       ├── Jellyfish Basin (vortex, locked by Vortex Entry)
    │   │       ├── Fog Coast      (vortex, locked by Vortex Entry)
    │   │       └── Black Cliff    (vortex, locked by Vortex Entry)
    └── Sea People Village ← (Sea People Gloves OR Teleport Mirror) AND Translator
        └── Fish Farm ← Unlock Fish Farm item
    ├── Vegetable Farm ← Unlock Vegetable Farm item
    └── Chicken Farm   ← Unlock Chicken Farm item
```

## Depth Gating — The Lenient Design

Depth is gated **leniently** — the player should never be hard-blocked from reaching
a depth tier purely by equipment. The OR logic between suit and oxygen means
there are always multiple paths forward.

### Progressive Diving Suit (8 levels)

| Level | Max Depth | Unlocks |
|---|---|---|
| 1 (start) | 40m | Blue Hole Shallow |
| 2 | 80m | **Mid Blue Hole** (OR 2 oxygen tanks) |
| 3 | 150m | **Deep Blue Hole** (OR 3 oxygen tanks + harpoon) |
| 4 | 230m | Useful upgrade, no new gate |
| 5 | 375m | Useful upgrade, no new gate |
| 6 | 540m | Useful upgrade, no new gate |
| 7 | 560m | **Glacial Passage** (cold-resistant tier 1) |
| 8 | 800m | **Glacier Zone** (cold-resistant tier 2) |

Levels 7 and 8 are the former "Cold-Resistant Diving Suit" — merged into the
progressive suit because the depth gating naturally requires cold protection
at those depths anyway.

### Progressive Oxygen Tank (6 levels) — OR alternative for depth

- 1 tank → helps reach Mid Blue Hole (OR suit lvl 2)
- 2 tanks → helps reach Mid Blue Hole (OR suit lvl 2)
- 3 tanks → helps reach Deep Blue Hole (OR suit lvl 3 + harpoon)

### Progressive Harpoon (4 levels)

- Level 1 required (AND, not OR) for Deep Blue Hole — you genuinely need a weapon
  to survive the deeper zones. Only 1 level is needed, extras are useful upgrades.

## Sea People Village Access

Two routes, both require the **Sea People Translator**:

1. **Swim route** — requires `Sea People Gloves` (allows swimming through the currents)
2. **Teleport route** — requires `Teleport Mirror` + `Teleport to Sea People Village`

The Teleport Mirror is a single item that enables all teleport destinations.
Specific teleport destinations are separate items (Teleport to Glacier Zone,
Teleport to Sea People Village).

### Why two routes?

Without two routes, the Sea People Village would be gated exclusively on `Sea People Gloves`,
making it a hard bottleneck that could block large sections of the randomizer.
The teleport alternative provides a bypass if gloves come late in the item pool.

## Glacier Access

Two routes, both require suit level 7+ (Cold-Resistant):

1. **Through Glacial Passage** — requires `Key to Tenzhin` to open the passage gate
2. **Teleport route** — requires `Teleport Mirror` + `Teleport to Glacier Zone`

Additionally, **Glacier Zone** (deeper than Glacial Passage) requires suit level 8
OR `Tech Suit Parts ×3` to survive the extreme cold.

## Vortex Regions (Aberrations)

The three vortex regions (Jellyfish Basin, Fog Coast, Black Cliff) each require
a `Vortex Entry` item. With `fish_checks` enabled, these regions contain 34
aberration fish first-catch checks.

- `Vortex Entry` is a progression item (×5 copies — one more than the 3 regions,
  giving some slack)
- All three vortex regions are connected from Blue Hole - Deep

## Farm Regions

Each farm is an unlockable region, gated by its corresponding item:

| Region | Gate Item | Notes |
|---|---|---|
| Fish Farm | `Unlock Fish Farm` | Otto's quest reward |
| Vegetable Farm | `Unlock Vegetable Farm` | Separate unlock |
| Chicken Farm | `Unlock Chicken Farm` | Same physical location as veg farm, different system |

## Chapter Gating — NOT Implemented

Chapters are **not** used as region gates in this randomizer. Instead, the
physical requirements (items, equipment) naturally pace the player through the
story. Reasons:

1. Chapter progress in Dave the Diver is more linear than most games — the key
   items (Translator, Key to Tenzhin, Tech Suit Parts, etc.) are the real gates
2. Using chapters as gates would create a second layer of gating on top of item
   gates, potentially making the early game too restrictive
3. The final boss (Yawie) is naturally gated by Glacier Zone access +
   Control Room Buttons + Laser Device — no need for an explicit "Chapter 7" check
