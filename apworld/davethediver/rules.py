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

    # === ICHIBAN DLC: UNLOCK REQUIREMENTS ===
    # The Ichiban DLC requires completing Chapter 5 AND unlocking Cocktails
    # (completing Vincent Yamaoka's 3rd VIP visit). All Ichiban DLC content
    # is gated behind both of these requirements.
    def can_access_ichiban(state) -> bool:
        return (
            state.has("Chapter 5 Complete", player) and
            state.has("Cocktails Unlocked", player)
        )

    # Gate all Ichiban DLC mission/boss locations
    for ichiban_loc in [
        "Ichiban: Complete Operation Sea Blue Eradication",
        "Ichiban: Complete Cold Noodles Mission",
        "Ichiban: Complete Beat 'Em Up Minigame",
        "Ichiban: Complete Karaoke Minigame",
        "Defeat: Torben",
    ]:
        _set_location_rule(multiworld, player, ichiban_loc,
            lambda state: can_access_ichiban(state)
        )

    # Gate Ichiban staff hiring on DLC access too
    for staff_name in ["Hamako", "Etsuko", "Chitose"]:
        _set_location_rule(multiworld, player, f"Staff: Hire {staff_name}",
            lambda state: can_access_ichiban(state)
        )

    # === GODZILLA DLC: EBIRAH + KAIJU FIGURINE RULES ===
    # The Godzilla DLC story triggers the morning after completing Chapter 5.
    # Gate Ebirah's defeat location on having Chapter 5 Complete in inventory.
    _set_location_rule(multiworld, player,
        "Defeat: Ebirah",
        lambda state: state.has("Chapter 5 Complete", player)
    )

    # All 20 Kaiju figurines are collectible after Ebirah is defeated.
    # We gate them on Chapter 5 Complete (same as Ebirah) rather than the
    # location check "Defeat: Ebirah", because state.has() checks items,
    # not completed locations. Chapter 5 Complete is the progression item
    # that naturally precedes the Ebirah encounter.
    # Figurines also inherit their region's depth/area access rules automatically.
    for i in range(1, 21):
        _set_location_rule(multiworld, player,
            f"Kaiju Figurine {i}",
            lambda state: state.has("Chapter 5 Complete", player)
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

    # === JUNGLE DLC RULES ===
    # All jungle content requires the dlc_jungle option — locations are filtered
    # out by should_include_location() when disabled, so rules only need to cover
    # access ordering within the DLC itself.

    # Utara Village — DLC entry point, always accessible once DLC is enabled
    set_rule(
        multiworld.get_entrance("Travel to Utara Village", player),
        lambda state: True  # No extra gate — DLC option toggle handles it
    )

    # Bancho Grill — unlocked during Chapter 1
    set_rule(
        multiworld.get_entrance("Open Bancho Grill", player),
        lambda state: state.has("Jungle Chapter 1 Complete", player)
    )

    # Utara Lake - Lower — requires Purification Filter tier 1
    set_rule(
        multiworld.get_entrance("Dive to Lower Lake", player),
        lambda state: state.has("Progressive Purification Filter", player, 1)
    )

    # Lakebed Sea — requires Chapter 4 + Advanced Filter (tier 3)
    set_rule(
        multiworld.get_entrance("Enter Lakebed Sea", player),
        lambda state: (
            state.has("Jungle Chapter 4 Complete", player) and
            state.has("Progressive Purification Filter", player, 3)
        )
    )

    # Setah Forest — requires Chapter 3
    set_rule(
        multiworld.get_entrance("Enter Setah Forest", player),
        lambda state: state.has("Jungle Chapter 3 Complete", player)
    )

    # Murau Temple — inside Setah Forest (Chapter 3+)
    set_rule(
        multiworld.get_entrance("Enter Murau Temple", player),
        lambda state: state.has("Jungle Chapter 3 Complete", player)
    )

    # Surga Falls — gated behind sub-mission (Cinta quest)
    set_rule(
        multiworld.get_entrance("Reach Surga Falls", player),
        lambda state: state.has("Jungle Chapter 2 Complete", player)
    )

    # Machete-gated area (Pirarucu zone in lower lake)
    _set_location_rule(multiworld, player,
        "Jungle: Unlock Machete Path (Pirarucu Area)",
        lambda state: state.has("Machete", player)
    )
    _set_location_rule(multiworld, player,
        "First Catch: Pirarucu",
        lambda state: state.has("Machete", player)
    )

    # Bug-catching requires Bug Net
    for loc_name in [
        "Jungle Insectagram: First Bug Caught",
        "Jungle Insectagram: 10 Bugs Caught",
        "Jungle Insectagram: 20 Bugs Caught",
        "Jungle Insectagram: 30 Bugs Caught (Complete)",
        "Jungle Insectagram: 50% Complete",
        "Jungle Staff: Unlock Udo",  # Requires 50% Insectagram
    ]:
        _set_location_rule(multiworld, player, loc_name,
            lambda state: state.has("Bug Net", player)
        )

    # Land fishing requires Fishing Rod
    _set_location_rule(multiworld, player,
        "Jungle Minigame: First Land Fishing Catch",
        lambda state: state.has("Fishing Rod", player)
    )

    # Marinca Bloom 50% required for Sato staff + Udo (handled via insectagram above for Udo)
    _set_location_rule(multiworld, player,
        "Jungle Staff: Unlock Sato",
        lambda state: state.has("Bug Net", player)  # Requires 50 Marinca Bloom entries
    )

    # Temple access requires 3 villagers at 3-hearts (Villager Trust items)
    _set_location_rule(multiworld, player,
        "Jungle: Discover Murau Temple",
        lambda state: state.has("Villager Trust", player, 3)
    )
    _set_location_rule(multiworld, player,
        "Jungle: Chapter 3 - Diving Suit of the Sunang Civ",
        lambda state: state.has("Villager Trust", player, 3)
    )

    # Boss sequence gates
    _set_location_rule(multiworld, player,
        "Jungle Boss: Defeat Stethacanthus",
        lambda state: (
            state.has("Jungle Chapter 4 Complete", player) and
            state.has("Progressive Purification Filter", player, 3)
        )
    )
    _set_location_rule(multiworld, player,
        "Jungle Boss: Defeat Xiphactinus",
        lambda state: (
            state.has("Jungle Chapter 4 Complete", player) and
            state.has("Progressive Purification Filter", player, 3)
        )
    )
    _set_location_rule(multiworld, player,
        "Jungle Boss: Defeat Basilosaurus",
        lambda state: (
            state.has("Jungle Chapter 7 Complete", player) and
            state.has("Progressive Purification Filter", player, 3)
        )
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
            defeated_all_bosses(state, player, world.options)
        )

    elif goal == 2:  # Diamond Rank — all Cooksta Diamond requirements
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            state.has("Cooksta: 720 Followers", player) and
            state.has("Cooksta: 375 Best Taste", player) and
            state.has("Cooksta: 32 Researched Recipes", player)
        )

    elif goal == 3:  # Master Diver — catch every fish (= complete MarinCa collection)
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            state.has("Ecowatcher: Complete All Fish", player)
        )

    elif goal == 4:  # 100% Completion
        world.multiworld.completion_condition[player] = lambda state: (
            defeated_yawie(state, player) and
            defeated_all_bosses(state, player, world.options) and
            state.has("Ecowatcher: Complete All Fish", player) and
            state.has("Cooksta: 720 Followers", player) and
            state.has("Cooksta: 375 Best Taste", player) and
            state.has("Cooksta: 32 Researched Recipes", player)
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


def defeated_all_bosses(state: CollectionState, player: int, options=None) -> bool:
    """Check if all bosses (base game + enabled DLC) have been defeated.

    Boss defeat locations are checked via state.can_reach() — this verifies
    the location is both reachable and collected (checked off), which is the
    correct AP primitive for location-based victory conditions.

    DLC bosses are only required when their DLC is enabled in options:
    - Godzilla DLC: Ebirah
    - Ichiban DLC: Torben (note: Torben is NOT a base-game boss)
    - Jungle DLC: the 6 Jungle bosses
    """
    # Helper: safely check if a location exists and has been reached
    def boss_done(loc_name: str) -> bool:
        try:
            return state.can_reach(loc_name, "Location", player)
        except KeyError:
            return True  # Location filtered out (DLC disabled) — skip requirement

    # Base game story bosses
    base_bosses = [
        "Defeat: Giant Squid",
        "Defeat: Clione Queen",
        "Defeat: Truck Hermit Crab",
        "Defeat: Giant Wolf Eel",
        "Defeat: Goblin Shark",
        "Defeat: Phantom Jellyfish",
        "Defeat: Giant Gadon",
        "Defeat: Helicoprion",
        "Defeat: Kronosaurus",
        "Defeat: John Watson",
        # Optional/vortex bosses (base game)
        "Defeat: Great White Shark Klaus",
        "Defeat: Mantis Shrimp",
        "Defeat: Lusca",
    ]

    if not all(boss_done(b) for b in base_bosses):
        return False

    # Godzilla DLC boss
    if options is not None and options.has_godzilla_dlc.value:
        if not boss_done("Defeat: Ebirah"):
            return False

    # Ichiban DLC boss (Torben) — this is an Ichiban-exclusive boss
    if options is not None and options.has_ichiban_dlc.value:
        if not boss_done("Defeat: Torben"):
            return False
    
    # Jungle DLC bosses
    if options is not None and options.has_jungle_dlc.value:
        jungle_bosses = [
            "Jungle Boss: Defeat Giant Snapping Turtle",
            "Jungle Boss: Defeat Sulong",
            "Jungle Boss: Defeat Black Caiman",
            "Jungle Boss: Defeat Stethacanthus",
            "Jungle Boss: Defeat Xiphactinus",
            "Jungle Boss: Defeat Basilosaurus",
        ]
        if not all(boss_done(b) for b in jungle_bosses):
            return False

    return True


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
