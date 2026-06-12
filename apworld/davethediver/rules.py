"""
Dave the Diver - Logic Rules

This file defines what items are needed to access each region and location.
"""

from worlds.generic.Rules import set_rule, add_rule
from BaseClasses import CollectionState


def set_rules(world):
    """Set access rules for all regions and locations"""
    
    player = world.player
    multiworld = world.multiworld
    
    # === DEPTH-BASED PROGRESSION ===
    # Suit levels and their max depth:
    #   L1=40m, L2=80m, L3=150m, L4=230m, L5=375m, L6=540m, L7=560m(CR1), L8=800m(CR2)
    # Oxygen tanks: 6 copies available. Both are OR'd so neither alone gates the player.

    # Blue Hole - Mid (50-130m): suit level 2 (80m) OR 2 oxygen tanks
    set_rule(
        multiworld.get_entrance("Dive to Mid Depths", player),
        lambda state: (
            state.has("Progressive Diving Suit", player, 2) or
            state.has("Progressive Oxygen Tank", player, 2)
        )
    )

    # Blue Hole - Deep (130-250m): suit level 3 (150m) OR 3 oxygen tanks, plus harpoon 1
    set_rule(
        multiworld.get_entrance("Dive to Deep", player),
        lambda state: (
            (
                state.has("Progressive Diving Suit", player, 3) or
                state.has("Progressive Oxygen Tank", player, 3)
            ) and
            state.has("Progressive Harpoon", player, 1)
        )
    )

    # === SEA PEOPLE VILLAGE ACCESS (TWO ROUTES) ===
    # Both routes require the Translator to actually interact with villagers.
    
    # Route 1: Swim down from the deep Blue Hole (needs gloves + translator)
    set_rule(
        multiworld.get_entrance("Swim to Sea People Village", player),
        lambda state: (
            state.has("Sea People Gloves", player) and
            state.has("Sea People Translator", player)
        )
    )
    
    # Route 2: Teleport (bypass swimming, but still need translator)
    set_rule(
        multiworld.get_entrance("Teleport to Sea People Village", player),
        lambda state: (
            state.has("Teleport Mirror", player) and
            state.has("Teleport to Sea People Village", player) and
            state.has("Sea People Translator", player)
        )
    )

    # === GLACIAL PASSAGE (Chapter 5 gate) ===
    # Requires suit level 7 (Cold-Resistant tier 1, max 560m) + Key to Tenzhin
    set_rule(
        multiworld.get_entrance("Swim to Glacial Passage", player),
        lambda state: (
            state.has("Key to Tenzhin", player) and
            state.has("Progressive Diving Suit", player, 7)
        )
    )

    # === GLACIER ZONE (Chapter 6) ===
    # Requires suit level 8 (Cold-Resistant tier 2, max 800m) + Tech Suit Parts x3

    # Route 1: Through the Glacial Passage
    set_rule(
        multiworld.get_entrance("Enter Glacier Zone from Passage", player),
        lambda state: (
            state.has("Progressive Diving Suit", player, 8) and
            state.has("Tech Suit Parts", player, 3)
        )
    )

    # Route 2: Direct teleport from surface (bypasses village + passage)
    set_rule(
        multiworld.get_entrance("Teleport to Glacier Zone", player),
        lambda state: (
            state.has("Teleport Mirror", player) and
            state.has("Teleport to Glacier", player) and
            state.has("Progressive Diving Suit", player, 8) and
            state.has("Tech Suit Parts", player, 3)
        )
    )

    # === TELEPORT BACK TO DEEP BLUE HOLE ===
    set_rule(
        multiworld.get_entrance("Teleport to Deep Blue Hole", player),
        lambda state: (
            state.has("Teleport Mirror", player) and
            state.has("Teleport to Deep Blue Hole", player)
        )
    )

    # === LOCATION-LEVEL RULES ===

    # Duff's Dream Concert (Chapter 4: Abandoned Cave gate)
    # Requires Sea People's Trust to be won first.
    _set_location_rule(multiworld, player,
        "Quest: Obtain Sea People Mirror (Teleport)",
        lambda state: state.has("Sea People's Trust", player)
    )

    # Story: Complete Chapter 4 — need Sea People's Trust (same gate)
    _set_location_rule(multiworld, player,
        "Story: Complete Chapter 4 (Abandoned Cave)",
        lambda state: state.has("Sea People's Trust", player)
    )

    # Story: Complete Chapter 7 (Broken Control Room)
    # Requires all 3 Control Room Buttons pressed AND the Laser Device.
    # (Location is in Glacier Zone, so glacier access is already enforced by region rule)
    _set_location_rule(multiworld, player,
        "Story: Complete Chapter 7 (Broken Control Room)",
        lambda state: (
            state.has("Control Room Button", player, 3) and
            state.has("Laser Device", player)
        )
    )

    # Defeat: Yawie (Final Boss) — same requirements as completing Ch7
    _set_location_rule(multiworld, player,
        "Defeat: Yawie (Final Boss)",
        lambda state: (
            state.has("Control Room Button", player, 3) and
            state.has("Laser Device", player)
        )
    )

    # === VORTEX REGION RULES ===
    # Each vortex requires Deep Blue Hole access + a Vortex Entry item.
    for vortex_entrance in [
        "Enter Jellyfish Basin Vortex",
        "Enter Fog Coast Vortex",
        "Enter Black Cliff Vortex",
    ]:
        set_rule(
            multiworld.get_entrance(vortex_entrance, player),
            lambda state: state.has("Vortex Entry", player, 1)
        )

    # === VORTEX BOSS RULES ===
    # Boss aberrations within each vortex — gated by region access (handled above).
    # Lusca is in Sea People Village — needs village access + Vortex Entry
    _set_location_rule(multiworld, player,
        "Defeat: Lusca",
        lambda state: (
            can_access_sea_people_village(state, player) and
            state.has("Vortex Entry", player, 1)
        )
    )

    # === FARM ACCESS RULES ===
    # Each farm requires its unlock item to enter the region

    # Fish Farm — unlocked via "Unlock Fish Farm" item (from Otto's quest)
    set_rule(
        multiworld.get_entrance("Visit Fish Farm", player),
        lambda state: state.has("Unlock Fish Farm", player)
    )

    # Vegetable Farm — unlocked via "Unlock Vegetable Farm" item
    set_rule(
        multiworld.get_entrance("Visit Vegetable Farm", player),
        lambda state: state.has("Unlock Vegetable Farm", player)
    )

    # Chicken Farm — unlocked via "Unlock Chicken Farm" item (separate system)
    set_rule(
        multiworld.get_entrance("Visit Chicken Farm", player),
        lambda state: state.has("Unlock Chicken Farm", player)
    )

    # Otto's quest itself requires Sea People's Trust to trigger
    _set_location_rule(multiworld, player,
        "Quest: Complete A Noisy Customer (Unlock Fish Farm)",
        lambda state: state.has("Sea People's Trust", player)
    )

    # === VICTORY CONDITION ===
    set_completion_condition(world)


def _set_location_rule(multiworld, player: int, location_name: str, rule) -> None:
    """Safely set a rule on a location — skips if location doesn't exist (filtered out)."""
    try:
        loc = multiworld.get_location(location_name, player)
        set_rule(loc, rule)
    except KeyError:
        pass  # Location was filtered out by player options — that's fine


# === HELPER FUNCTIONS ===

def has_depth_access(state: CollectionState, player: int, depth_level: int) -> bool:
    """Check if player can reach a certain depth.

    Suit levels map to max depth: L2=80m, L3=150m, L7=560m(CR1), L8=800m(CR2).
    Oxygen tanks (6 total) are used as an OR alternative so neither alone gates the player.

    Args:
        depth_level: 1 = Shallow (0-40m, always), 2 = Mid (80-130m), 3 = Deep (130-250m)
    """
    if depth_level == 1:
        return True  # Shallow always accessible (suit level 1 from start)
    elif depth_level == 2:
        # Need suit level 2 (80m) OR 2 oxygen tanks
        return (
            state.has("Progressive Diving Suit", player, 2) or
            state.has("Progressive Oxygen Tank", player, 2)
        )
    elif depth_level == 3:
        # Need suit level 3 (150m) OR 3 oxygen tanks, plus at least harpoon level 1
        return (
            (
                state.has("Progressive Diving Suit", player, 3) or
                state.has("Progressive Oxygen Tank", player, 3)
            ) and
            state.has("Progressive Harpoon", player, 1)
        )
    return False


def can_access_sea_people_village(state: CollectionState, player: int) -> bool:
    """Check if player can access Sea People Village via any route.
    
    Both routes require the Sea People Translator to interact with villagers.
    Route 1: Swim with Sea People Gloves (from deep Blue Hole)
    Route 2: Teleport Mirror + Teleport to Sea People Village destination
    """
    has_translator = state.has("Sea People Translator", player)
    if not has_translator:
        return False

    return (
        # Route 1: Swim with gloves
        state.has("Sea People Gloves", player) or
        # Route 2: Teleport
        (state.has("Teleport Mirror", player) and
         state.has("Teleport to Sea People Village", player))
    )


def can_access_glacial_passage(state: CollectionState, player: int) -> bool:
    """Check if player can access the Glacial Passage (Chapter 5 gate).

    Hard gate: requires Sea People Village access + Key to Tenzhin +
    Cold-Resistant Diving Suit (a separate unlock from the depth progression suit).
    """
    return (
        can_access_sea_people_village(state, player) and
        state.has("Key to Tenzhin", player) and
        state.has("Progressive Diving Suit", player, 7)
    )


def can_access_glacier_zone(state: CollectionState, player: int) -> bool:
    """Check if player can access the full Glacier Zone (Chapter 6).
    
    Requires Cold-Resistant Diving Suit + all 3 Tech Suit Parts, and either:
    Route 1: Through Sea People Village + Glacial Passage
    Route 2: Direct teleport to Glacier (bypasses village entirely)
    """
    if not state.has("Progressive Diving Suit", player, 8):
        return False
    if not state.has("Tech Suit Parts", player, 3):
        return False

    return (
        # Route 1: Through the village and passage
        can_access_glacial_passage(state, player) or
        # Route 2: Direct teleport
        (state.has("Teleport Mirror", player) and
         state.has("Teleport to Glacier", player))
    )


def can_complete_chapter_7(state: CollectionState, player: int) -> bool:
    """Check if player can complete Chapter 7 (Broken Control Room).
    
    Requires access to the Glacier Zone, all 3 Control Room Buttons pressed,
    and the Laser Device to open the control room.
    """
    return (
        can_access_glacier_zone(state, player) and
        state.has("Control Room Button", player, 3) and
        state.has("Laser Device", player)
    )


def has_weapon_tier(state: CollectionState, player: int, tier: int) -> bool:
    """Check if player has a certain weapon tier
    
    Args:
        tier: 1 = Basic, 2 = Enhanced, 3 = Advanced
    """
    return state.has("Progressive Harpoon", player, tier)


def set_completion_condition(world):
    """Set victory condition based on player's goal option"""
    player = world.player
    goal = world.options.goal.value

    if goal == 0:  # Defeat Yawie (default)
        world.multiworld.completion_condition[player] = lambda state: \
            defeated_yawie(state, player)

    elif goal == 1:  # Defeat All Bosses
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            defeated_all_bosses(state, player)
        )

    elif goal == 2:  # Defeat Yawie + Cooksta
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            state.has("Cooksta: 720 Followers", player)
        )

    elif goal == 3:  # Restaurant Tycoon
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            state.has("Restaurant Rating: 5 Stars", player)
        )

    elif goal == 4:  # Master Diver
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            state.has("Ecowatcher: Complete All Fish", player) and
            state.has("Ecowatcher: Complete All Marinca", player)
        )

    elif goal == 5:  # Complete MarinCa Collection
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            state.has("Ecowatcher: Complete All Marinca", player)
        )

    elif goal == 6:  # 100% Completion
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            defeated_all_bosses(state, player) and
            state.has("Ecowatcher: Complete All Fish", player) and
            state.has("Ecowatcher: Complete All Marinca", player) and
            state.has("Cooksta: 720 Followers", player) and
            state.has("Restaurant Rating: 5 Stars", player)
        )


def defeated_yawie(state: CollectionState, player: int) -> bool:
    """Check if the final boss Yawie has been defeated.
    
    Requires Glacier Zone access + all 3 Control Room Buttons + Laser Device.
    """
    return (
        state.has("Control Room Button", player, 3) and
        state.has("Laser Device", player) and
        # Glacier Zone access is implied by suit level 8 + tech suit parts
        state.has("Progressive Diving Suit", player, 8) and
        state.has("Tech Suit Parts", player, 3)
    )


def defeated_all_bosses(state: CollectionState, player: int) -> bool:
    """Check if all bosses have been defeated, including optional vortex bosses.
    
    The vortex bosses (Klaus, Mantis Shrimp, Torben, Lusca, etc.) require
    a Vortex Entry item to initiate their encounters.
    """
    return (
        # Story bosses — covered by story/chapter progression
        state.has("Defeat: Giant Squid", player) and
        state.has("Defeat: Clione Queen", player) and
        state.has("Defeat: Truck Hermit Crab", player) and
        state.has("Defeat: Giant Wolf Eel", player) and
        state.has("Defeat: Goblin Shark", player) and
        state.has("Defeat: Phantom Jellyfish", player) and
        state.has("Defeat: Giant Gadon", player) and
        state.has("Defeat: Helicoprion", player) and
        state.has("Defeat: Kronosaurus", player) and
        state.has("Defeat: John Watson", player) and
        state.has("Defeat: Ebirah", player) and
        # Optional/vortex bosses — require Vortex Entry items
        state.has("Defeat: Great White Shark Klaus", player) and
        state.has("Defeat: Mantis Shrimp", player) and
        state.has("Defeat: Lusca", player) and
        state.has("Defeat: Torben", player)
    )


def has_all_chapters(state: CollectionState, player: int) -> bool:
    """Check if all 7 main chapters are complete"""
    return (
        state.has("Chapter 1 Complete", player) and
        state.has("Chapter 2 Complete", player) and
        state.has("Chapter 3 Complete", player) and
        state.has("Chapter 4 Complete", player) and
        state.has("Chapter 5 Complete", player) and
        state.has("Chapter 6 Complete", player) and
        state.has("Chapter 7 Complete", player)
    )
