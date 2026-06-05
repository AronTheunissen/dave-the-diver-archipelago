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
    
    # Blue Hole - Mid: Need oxygen OR suit upgrades
    set_rule(
        multiworld.get_entrance("Dive to Mid Depths", player),
        lambda state: (
            state.has("Progressive Oxygen Tank", player, 2) or
            state.has("Progressive Diving Suit", player, 2)
        )
    )
    
    # Blue Hole - Deep: Need significant upgrades
    set_rule(
        multiworld.get_entrance("Dive to Deep", player),
        lambda state: (
            state.has("Progressive Oxygen Tank", player, 4) and
            state.has("Progressive Diving Suit", player, 3) and
            state.has("Progressive Harpoon", player, 2)
        )
    )
    
    # === SEA PEOPLE VILLAGE ACCESS (TWO ROUTES) ===
    
    # Route 1: Traditional (swim down from deep blue hole)
    set_rule(
        multiworld.get_entrance("Swim to Sea People Village", player),
        lambda state: state.has("Sea People Gloves", player)
    )
    
    # Route 2: Teleport (bypass swimming requirement)
    set_rule(
        multiworld.get_entrance("Teleport to Sea People Village", player),
        lambda state: (
            state.has("Teleport Mirror", player) and
            state.has("Teleport to Sea People Village", player)
        )
    )
    
    # === GLACIER ACCESS (TWO ROUTES) ===
    
    # Route 1: Traditional (swim through Sea People Village)
    set_rule(
        multiworld.get_entrance("Swim to Glacier from Village", player),
        lambda state: state.has("Cold Protection Suit", player)
    )
    
    # Route 2: Direct Teleport (bypass Sea People Village entirely!)
    set_rule(
        multiworld.get_entrance("Teleport to Glacier", player),
        lambda state: (
            state.has("Teleport Mirror", player) and
            state.has("Teleport to Glacier", player) and
            state.has("Cold Protection Suit", player)
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
    
    # === FISH FARM ACCESS ===
    # TODO: Add fish farm unlock requirement (might be story-based)
    # For now, assume always accessible from surface
    
    # === VICTORY CONDITION ===
    # Set based on player's chosen goal
    set_completion_condition(world)


# === HELPER FUNCTIONS ===

def has_depth_access(state: CollectionState, player: int, depth_level: int) -> bool:
    """Check if player can reach a certain depth
    
    Args:
        depth_level: 1 = Shallow, 2 = Mid, 3 = Deep
    """
    if depth_level == 1:
        return True  # Shallow always accessible
    elif depth_level == 2:
        return (
            state.has("Progressive Oxygen Tank", player, 2) or
            state.has("Progressive Diving Suit", player, 2)
        )
    elif depth_level == 3:
        return (
            state.has("Progressive Oxygen Tank", player, 4) and
            state.has("Progressive Diving Suit", player, 3)
        )
    return False


def can_access_sea_people_village(state: CollectionState, player: int) -> bool:
    """Check if player can access Sea People Village via any route"""
    return (
        # Route 1: Swim with gloves
        state.has("Sea People Gloves", player) or
        # Route 2: Teleport
        (state.has("Teleport Mirror", player) and
         state.has("Teleport to Sea People Village", player))
    )


def can_access_glacier(state: CollectionState, player: int) -> bool:
    """Check if player can access Glacier via any route"""
    if not state.has("Cold Protection Suit", player):
        return False  # Always need cold suit to survive
    
    return (
        # Route 1: Swim through Sea People Village
        state.has("Sea People Gloves", player) or
        # Route 2: Direct teleport (bypasses village!)
        (state.has("Teleport Mirror", player) and
         state.has("Teleport to Glacier", player))
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
    
    if goal == 0:  # Final chapter only
        world.multiworld.completion_condition[player] = lambda state: \
            state.has("Chapter 6 Complete", player)
    
    elif goal == 1:  # All chapters (default)
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player)
        )
    
    elif goal == 2:  # Chapters + Cooksta
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            state.has("Cooksta: 10000 Followers", player)
        )
    
    elif goal == 3:  # Restaurant Tycoon
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            state.has("Restaurant Rating: 5 Stars", player)
            # TODO: Add "has all key recipes" check
        )
    
    elif goal == 4:  # Master Diver
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            state.has("Ecowatcher: Complete All Fish", player)
            # TODO: Add "caught all fish species" check
        )
    
    elif goal == 5:  # 100% Completion
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            state.has("Ecowatcher: Complete All Fish", player) and
            state.has("Ecowatcher: Complete All Marinca", player) and
            state.has("Cooksta: 10000 Followers", player) and
            state.has("Restaurant Rating: 5 Stars", player)
            # TODO: Add all completion checks
        )


def has_all_chapters(state: CollectionState, player: int) -> bool:
    """Check if all chapters are complete"""
    return (
        state.has("Chapter 1 Complete", player) and
        state.has("Chapter 2 Complete", player) and
        state.has("Chapter 3 Complete", player) and
        state.has("Chapter 4 Complete", player) and
        state.has("Chapter 5 Complete", player) and
        state.has("Chapter 6 Complete", player)
    )
