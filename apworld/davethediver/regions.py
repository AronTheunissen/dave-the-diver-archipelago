"""
Dave the Diver - Region Definitions

Regions represent distinct areas of the game world with different access requirements.
Connections between regions are named entrances, and rules.py sets access rules on them.

Region map:
  Menu
    └─> Bancho Sushi (always open)
          ├─> Blue Hole - Shallow (always open)
          │     └─> Blue Hole - Mid (oxygen/suit upgrades)
          │               └─> Blue Hole - Deep (more upgrades)
          │                     └─> Sea People Village [Route 1: gloves + translator]
          │                               └─> Glacial Passage [Key to Tenzhin + cold suit]
          │                                         └─> Glacier Zone [Tech Suit Parts x3]
          ├─> Sea People Village [Route 2: teleport mirror + dest + translator]
          ├─> Glacier Zone [Route 3: teleport mirror + dest + cold suit + tech suit parts]
          ├─> Blue Hole - Deep [teleport back, QoL]
          └─> Fish Farm (unlocked via Otto's quest)
"""

from BaseClasses import Region
from .locations import location_table, DaveDiverLocation


# All valid region names — used for location assignment lookup
REGION_NAMES = {
    "Menu",
    "Bancho Sushi",
    "Blue Hole - Shallow",
    "Blue Hole - Mid",
    "Blue Hole - Deep",
    "Sea People Village",
    "Glacial Passage",
    "Glacier Zone",
    "Hydrothermal Vents",  # Deepest area, accessed via Glacier Zone (Chapter 6+)
    "Fish Farm",
    "Vegetable Farm",      # Same physical location as Chicken Farm, but separate unlock
    "Chicken Farm",        # Same physical location as Vegetable Farm, but separate unlock
    # Aberration vortex regions — accessed via Vortex Entry items (night only)
    "Jellyfish Basin",     # Vortex 1: jellyfish/squid/crab aberrations
    "Fog Coast",           # Vortex 2: eel/shark/barracuda aberrations
    "Black Cliff",         # Vortex 3: wreckfish/stonefish/sturgeon aberrations
}


def create_regions(world):
    """Create all regions for Dave the Diver and connect them."""

    player = world.player
    multiworld = world.multiworld

    # ── Create all regions ──────────────────────────────────────────────────
    regions = {name: Region(name, player, multiworld) for name in REGION_NAMES}

    # ── Assign locations to regions ─────────────────────────────────────────
    for location_name, location_data in location_table.items():
        # Skip locations disabled by player options
        if not world.should_include_location(location_name, location_data):
            continue

        region_name = location_data.region
        if region_name not in regions:
            # Warn about misconfigured region assignments rather than silently dropping
            import warnings
            warnings.warn(
                f"Location '{location_name}' references unknown region '{region_name}'. "
                f"Valid regions: {sorted(REGION_NAMES)}"
            )
            continue

        region = regions[region_name]
        region.locations.append(
            DaveDiverLocation(player, location_name, location_data.code, region)
        )

    # ── Connect regions ─────────────────────────────────────────────────────

    # Menu → Bancho Sushi (always open — game start)
    regions["Menu"].connect(regions["Bancho Sushi"], "Start Game")

    # Bancho Sushi → Blue Hole - Shallow (always open — basic diving)
    regions["Bancho Sushi"].connect(regions["Blue Hole - Shallow"], "Dive to Shallow")

    # Blue Hole - Shallow → Blue Hole - Mid (oxygen/suit upgrades)
    regions["Blue Hole - Shallow"].connect(regions["Blue Hole - Mid"], "Dive to Mid Depths")

    # Blue Hole - Mid → Blue Hole - Deep (more upgrades)
    regions["Blue Hole - Mid"].connect(regions["Blue Hole - Deep"], "Dive to Deep")

    # ── Sea People Village: two routes ──────────────────────────────────────

    # Route 1: Swim from deep Blue Hole (Sea People Gloves + Translator)
    regions["Blue Hole - Deep"].connect(
        regions["Sea People Village"], "Swim to Sea People Village"
    )

    # Route 2: Teleport from surface (Teleport Mirror + destination + Translator)
    regions["Bancho Sushi"].connect(
        regions["Sea People Village"], "Teleport to Sea People Village"
    )

    # ── Glacial Passage (Chapter 5 gate) ─────────────────────────────────────
    # Only accessible from Sea People Village, requires Key to Tenzhin + Cold Suit
    regions["Sea People Village"].connect(
        regions["Glacial Passage"], "Swim to Glacial Passage"
    )

    # ── Glacier Zone (Chapter 6): two routes ────────────────────────────────

    # Route 1: Through the Glacial Passage (requires Tech Suit Parts x3)
    regions["Glacial Passage"].connect(
        regions["Glacier Zone"], "Enter Glacier Zone from Passage"
    )

    # Route 2: Direct teleport from surface (bypasses Sea People Village + Passage)
    regions["Bancho Sushi"].connect(
        regions["Glacier Zone"], "Teleport to Glacier Zone"
    )

    # ── Hydrothermal Vents (Chapter 6+, accessed via Glacier Zone) ───────────
    regions["Glacier Zone"].connect(
        regions["Hydrothermal Vents"], "Dive to Hydrothermal Vents"
    )

    # ── Teleport back to Deep Blue Hole (QoL, not required) ─────────────────
    regions["Bancho Sushi"].connect(
        regions["Blue Hole - Deep"], "Teleport to Deep Blue Hole"
    )

    # ── Vortex regions (aberrations, night only, require Vortex Entry) ─────────
    regions["Blue Hole - Deep"].connect(regions["Jellyfish Basin"], "Enter Jellyfish Basin Vortex")
    regions["Blue Hole - Deep"].connect(regions["Fog Coast"], "Enter Fog Coast Vortex")
    regions["Blue Hole - Deep"].connect(regions["Black Cliff"], "Enter Black Cliff Vortex")

    # ── Fish Farm (unlocked via Otto's quest + Unlock Fish Farm item) ────────
    regions["Bancho Sushi"].connect(regions["Fish Farm"], "Visit Fish Farm")

    # ── Vegetable Farm (same location as Chicken Farm, separate unlock) ──────
    regions["Bancho Sushi"].connect(regions["Vegetable Farm"], "Visit Vegetable Farm")

    # ── Chicken Farm (same location as Vegetable Farm, separate unlock) ──────
    regions["Bancho Sushi"].connect(regions["Chicken Farm"], "Visit Chicken Farm")

    # ── Register all regions ─────────────────────────────────────────────────
    multiworld.regions += list(regions.values())


# Location class — defined here to avoid circular import issues
from BaseClasses import Location

class DaveDiverLocation(Location):
    """A location in Dave the Diver"""
    game: str = "Dave the Diver"
