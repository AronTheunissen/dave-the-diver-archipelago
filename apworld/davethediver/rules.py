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

    # === NIGHT DIVE RULES ===
    # Night Dive Unlock is obtained from "Giant Stingray at Night" in vanilla,
    # but in AP it's a receivable item. Night-only fish require it.
    # Night-only fish by region:
    # Shallow: Blue Lobster, Blacktip Reefshark, Box Jellyfish, Clearfin Lionfish,
    #          Crystal Lobster, Devil Scorpionfish, Longspine Porcupinefish,
    #          Longspine Squirrelfish, Red-banded Lobster, Moray Eel
    # Mid: Blackfin Barracuda, Crystal Lobster, Devil Scorpionfish,
    #      Fan Lobster, Giant Squid, Humboldt Squid, Spear Squid
    night_only_fish = [
        # Shallow night fish
        "First Catch: Blue Lobster",
        "First Catch: Blacktip Reefshark",
        "First Catch: Box Jellyfish",
        "First Catch: Clearfin Lionfish",
        "First Catch: Crystal Lobster",
        "First Catch: Devil Scorpionfish",
        "First Catch: Longspine Porcupinefish",
        "First Catch: Longspine Squirrelfish",
        "First Catch: Red-banded Lobster",
        "First Catch: Moray Eel",
        # Mid night fish
        "First Catch: Blackfin Barracuda",
        "First Catch: Fan Lobster",
        "First Catch: Giant Squid",
        "First Catch: Humboldt Squid",
        "First Catch: Spear Squid",
    ]
    for fish_loc in night_only_fish:
        _set_location_rule(multiworld, player, fish_loc,
            lambda state: state.has("Night Dive Unlock", player)
        )

    # Humboldt Squid is Mid depth + Night Dive (not Glacier Zone as previously thought)
    # The Mid region gate already handles depth; we just add Night Dive on top
    _set_location_rule(multiworld, player,
        "First Catch: Humboldt Squid",
        lambda state: (
            has_depth_access(state, player, 2) and  # Mid depth
            state.has("Night Dive Unlock", player)
        )
    )

    # Cooksta App is unlocked after "A Scolding from Yoshie" sub-mission
    # Gate all Cooksta locations on this sub-mission being completeable
    if world.options.include_cooksta.value and world.options.include_sub_missions.value:
        for loc in multiworld.get_locations(player):
            if loc.name.startswith("Cooksta:"):
                add_rule(loc,
                    lambda state: state.can_reach("Sub-Mission: A Scolding from Yoshie", "Location", player)
                )

    # Night Dive Unlock itself is obtained from Giant Stingray at Night
    # (no prerequisite needed — it's a vanilla night encounter that can happen early)

    # === VORTEX REGION RULES (DREDGE DLC) ===
    # The red fog appears on random nights once Sammy's Chicken Farm is unlocked.
    # Each vortex (whirlpool seen from the Dredge boat) also requires a Vortex Entry.
    for vortex_entrance in [
        "Enter Jellyfish Basin Vortex",
        "Enter Fog Coast Vortex",
        "Enter Black Cliff Vortex",
    ]:
        set_rule(
            multiworld.get_entrance(vortex_entrance, player),
            lambda state: (
                state.has("Vortex Entry", player, 1) and
                state.has("Unlock Chicken Farm", player)  # Unlocks red fog nights
            )
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

    # === BOSS FIGHT SPECIFIC RULES ===

    # Gas Cutter comes from "The Leahs-chan Rescue" (Chapter 1 main mission)
    # It's an AP item so can be received from anywhere, but the location that
    # produces it is the Leahs-chan rescue mission check.
    # Gate the Giant Squid on Gas Cutter (which the player must have received).
    # Headlamp is the reward for defeating Giant Squid (completing Leahs-chan).
    # So: Gas Cutter → Giant Squid → Headlamp → Giant Wolf Eel (nice chain!)

    # Giant Squid: needs Gas Cutter + 150m depth (Diving Suit Lv3+ or equiv O2)
    _set_location_rule(multiworld, player,
        "Defeat: Giant Squid",
        lambda state: (
            state.has("Gas Cutter", player) and
            has_depth_access(state, player, 2)  # Mid depth covers 150m
        )
    )

    # Clione Queen: needs Bug Net + 200m depth (Deep)
    _set_location_rule(multiworld, player,
        "Defeat: Clione Queen",
        lambda state: (
            state.has("Bug Net", player) and
            has_depth_access(state, player, 3)  # Deep 130-250m
        )
    )

    # Giant Wolf Eel: needs Sea People Gloves + Headlamp + 250m depth (Deep)
    _set_location_rule(multiworld, player,
        "Defeat: Giant Wolf Eel",
        lambda state: (
            state.has("Sea People Gloves", player) and
            state.has("Headlamp", player) and
            has_depth_access(state, player, 3)  # Deep
        )
    )

    # Goblin Shark: needs Salvage Drone + Underwater Camera (Yellow Shipwreck)
    _set_location_rule(multiworld, player,
        "Defeat: Goblin Shark",
        lambda state: (
            state.has("Salvage Drone", player) and
            state.has("Underwater Camera", player) and
            has_depth_access(state, player, 3)  # Deep
        )
    )

    # John Watson: 2 fights — Fight 1 needs Translator + 130m, Fight 2 needs cold suit
    # John Watson is already in Sea People Village region which requires Translator.
    # We add the cold suit requirement since Fight 2 is at 400m.
    _set_location_rule(multiworld, player,
        "Defeat: John Watson",
        lambda state: (
            state.has("Sea People Translator", player) and
            state.has("Progressive Diving Suit", player, 7)  # Cold-Resistant Suit = Lv7
        )
    )

    # Kronosaurus: needs Heat-Resistant Gloves + Hydrothermal Vents access
    _set_location_rule(multiworld, player,
        "Defeat: Kronosaurus",
        lambda state: state.has("Heat-Resistant Gloves", player)
        # Hydrothermal Vents region gate already handles depth/cold suit
    )

    # Phantom Jellyfish: needs Cold-Resistant Suit (Lv7) + Beluga Whale Ride Whistle
    # Beluga Whale Ride Whistle is awarded by completing Sub-Mission: Daphne's Whistle
    # (Sea People Village mission — already gated by village access)
    _set_location_rule(multiworld, player,
        "Defeat: Phantom Jellyfish",
        lambda state: (
            state.has("Progressive Diving Suit", player, 7) and  # Cold-Resistant
            state.has("Beluga Whale Ride Whistle", player)
        )
    )
    # Beluga Whale Ride Whistle is the reward from Daphne's Whistle mission
    # Gate the Phantom Jellyfish on that sub-mission being completeable
    # (the whistle is an AP item so it could come from anywhere in the multiworld,
    # but we still need the location check itself to produce it)

    # Helicoprion: needs Cold-Resistant Suit (Lv7, 560m) + 450m depth
    _set_location_rule(multiworld, player,
        "Defeat: Helicoprion",
        lambda state: state.has("Progressive Diving Suit", player, 7)
    )

    # Giant Gadon: needs Glacial Passage access + Cobra's Lost Crowbar
    _set_location_rule(multiworld, player,
        "Defeat: Giant Gadon",
        lambda state: state.has("Cobra's Lost Crowbar", player)
    )

    # Lusca: secret post-game optional boss
    # Requires: Marinca Completion Trophy + Stormy Night unlocked + Sea People Village
    # Sea People Village access is already gated by region rules ✅
    _set_location_rule(multiworld, player,
        "Defeat: Lusca",
        lambda state: (
            state.has("Marinca Completion Trophy", player) and
            state.has("Sea People Gloves", player)  # Stormy Night unlock trigger
        )
    )

    # Yawie: needs 550m+ (suit lv7+), 3 buttons, Laser Device, Glacier Zone access
    # Already gated by defeated_yawie() in victory conditions, but add explicit rule
    _set_location_rule(multiworld, player,
        "Defeat: Yawie",
        lambda state: (
            state.has("Control Room Button", player, 3) and
            state.has("Laser Device", player) and
            state.has("Progressive Diving Suit", player, 7)
        )
    )

    # Vortex boss chain: Stormy Night → Mantis Shrimp → Klaus (Clara's Omani side quest)
    _set_location_rule(multiworld, player,
        "Defeat: Mantis Shrimp",
        lambda state: state.can_reach("Defeat: Truck Hermit Crab", "Location", player)
    )
    _set_location_rule(multiworld, player,
        "Defeat: Great White Shark Klaus",
        lambda state: (
            state.can_reach("Defeat: Mantis Shrimp", "Location", player) and
            state.can_reach("Sub-Mission: Clara's Omani (Klaus Quest)", "Location", player)
        )
    )

    # Hydrothermal Vents entrance gate: also needs Heat-Resistant Gloves
    try:
        vents_entrance = multiworld.get_entrance("Enter Hydrothermal Vents", player)
        add_rule(vents_entrance,
            lambda state: state.has("Heat-Resistant Gloves", player)
        )
    except Exception:
        pass  # Entrance may not exist if Vents region is filtered

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

    # === VIP VISIT RULES ===
    # Vincent Yamaoka appears as a recurring VIP judge across all 3 visits.
    # His visits are NOT part of the competition chain — they're independent.
    # All 3 visits require being able to get to Bancho Sushi (always accessible).
    # No special rules needed for Vincent visits — region access handles it.

    # Independent VIP quests — gated by ingredient access:
    # Sammy: rice, eggplant, carrot → Vegetable Farm (already in correct region)
    # Michael Bang: coral trout (Shallow) + farm → Bancho Sushi (Shallow always accessible)
    # Otto: Moray Eel (Shallow) + Turmeric (vendor) → always accessible
    # Jango: Bluefin Tuna Chutoro (Mid) + Habanero (farm) + Sea Grape (Mid)
    _set_location_rule(multiworld, player,
        "Quest: Complete Jango's Secret Recipe",
        lambda state: (
            has_depth_access(state, player, 2) and  # Blue Hole Mid for Bluefin + Sea Grape
            state.has("Unlock Vegetable Farm", player)  # Farm for Habanero
        )
    )
    # Mxmtoon: Green Sea Urchin needs Sea People Gloves + Bluefin Tuna (Mid) + Cuttlefish (Mid)
    _set_location_rule(multiworld, player,
        "Quest: Serve Mxmtoon",
        lambda state: (
            state.has("Sea People Gloves", player) and
            has_depth_access(state, player, 2)  # Mid for Bluefin + Cuttlefish
        )
    )
    # Michael Bang's Inspiration: Coral Trout (Shallow) + Titan Triggerfish (Shallow) + Rice (Farm)
    _set_location_rule(multiworld, player,
        "Quest: Complete Michael Bang's Inspiration",
        lambda state: state.has("Unlock Vegetable Farm", player)
    )

    # === COOKING COMPETITION CHAIN ===
    # Sequential chain: each fight requires beating the previous one.
    # Ingredient access gates each fight; Alex Cooper's defeat grants Cocktails Unlocked.

    # Vincent fight: Sea Grape (Limestone Cave = Mid) + White Spotted Jellyfish (Mid) + Salt
    _set_location_rule(multiworld, player,
        "Competition: Defeat Vincent Yamaoka",
        lambda state: has_depth_access(state, player, 2)  # Blue Hole Mid
    )
    # Wang Pang: beat Vincent + Bluespotted Stargazer (Deep) + Egg (Chicken Farm)
    _set_location_rule(multiworld, player,
        "Competition: Defeat Wang Pang",
        lambda state: (
            state.can_reach("Competition: Defeat Vincent Yamaoka", "Location", player) and
            has_depth_access(state, player, 3) and  # Blue Hole Deep
            state.has("Unlock Chicken Farm", player)  # For Egg
        )
    )
    # Alex Cooper: beat Wang Pang + Cookiecutter Shark + Vampire Squid + Barreleye (all Deep) + Kelp
    _set_location_rule(multiworld, player,
        "Competition: Defeat Alex Cooper",
        lambda state: (
            state.can_reach("Competition: Defeat Wang Pang", "Location", player) and
            has_depth_access(state, player, 3)  # Blue Hole Deep for all fish
        )
    )
    # Pastro: beat Alex + Humboldt Squid (Mid + Night Dive) + White Shrimp (Mid) + farm
    _set_location_rule(multiworld, player,
        "Competition: Defeat Pastro Antogiovani",
        lambda state: (
            state.can_reach("Competition: Defeat Alex Cooper", "Location", player) and
            has_depth_access(state, player, 2) and  # Mid for Humboldt Squid + White Shrimp
            state.has("Night Dive Unlock", player) and  # Humboldt Squid is night-only
            state.has("Unlock Vegetable Farm", player)  # Farm for Wheat + Garlic
        )
    )

    # === STAFF TRAINING RULES ===
    # Training locations require having the staff member first.
    # Works for both milestone mode (item = "Maki") and all_levels mode
    # (item = "Progressive Maki") — we check for either.
    # Training must also be in order: Lv10 requires Lv5 done, etc.
    # We enforce ordering via can_reach on the previous training location.

    _BASE_STAFF = [
        "Billy", "Carolina", "Charlie", "Cohh", "Davina", "Drae", "El Nino",
        "Itsuki", "James", "Jandi", "Kyoko", "Liu", "Maki", "Masayoshi", "Mitchell",
        "Pai", "Raptor", "Raul", "Tohoku", "Yone", "Yusuke",
    ]
    _DLC_STAFF = {"Hamako": "dlc_ichiban", "Etsuko": "dlc_ichiban", "Chitose": "dlc_ichiban"}
    _ALL_STAFF = {name: "" for name in _BASE_STAFF}
    _ALL_STAFF.update(_DLC_STAFF)
    _TRAINING_LEVELS = [5, 10, 15, 20]

    for staff_name in _ALL_STAFF:
        # Gate every training location on having the staff member first.
        # Covers both milestone mode (item = "Maki") and all_levels mode
        # (item = "Progressive Maki" ×20) by checking for either item name.
        # Levels 1-20 covers both milestone (5/10/15/20) and all_levels (1-20) —
        # _set_location_rule is a no-op if the location doesn't exist in the seed.
        for lvl in range(1, 21):
            _set_location_rule(multiworld, player,
                f"Staff: Train {staff_name} to Level {lvl}",
                lambda state, n=staff_name: (
                    state.has(n, player) or
                    state.has(f"Progressive {n}", player, 1)
                )
            )

    # === SUB-MISSION RULES ===
    # Most sub-missions inherit region access rules automatically.
    # The following have explicit prerequisites or chains:

    # Chapter 1: Dolphin chain (request → follow-up → defeat pirates)
    _set_location_rule(multiworld, player,
        "Sub-Mission: What Happened to the Dolphins?",
        lambda state: state.can_reach("Sub-Mission: A Dolphin's Request", "Location", player)
    )
    _set_location_rule(multiworld, player,
        "Sub-Mission: Defeat Pirates",
        lambda state: state.can_reach("Sub-Mission: What Happened to the Dolphins?", "Location", player)
    )

    # Chapter 2: Clione chain (find → defeat queen)
    _set_location_rule(multiworld, player,
        "Sub-Mission: Defeat the Clione Queen",
        lambda state: state.can_reach("Sub-Mission: Catch Clione", "Location", player)
    )

    # Chapter 2: Whale chain (hear cry → find baby)
    _set_location_rule(multiworld, player,
        "Sub-Mission: Finding the Baby Whale",
        lambda state: state.can_reach("Sub-Mission: Whale Cry", "Location", player)
    )

    # Chapter 3 Sea People Village chains:
    # Kinglong's Statue chain (offer flowers → repair statue)
    _set_location_rule(multiworld, player,
        "Sub-Mission: Repair Kinglong's Statue",
        lambda state: state.can_reach("Sub-Mission: Offer Flowers to King Long's Statue", "Location", player)
    )

    # Pet Squid Selgio requires Bug Net (used to catch Selgio)
    _set_location_rule(multiworld, player,
        "Sub-Mission: Pet Squid Selgio",
        lambda state: state.has("Bug Net", player)
    )

    # Curious Child requires Sea People Necklace (to travel through tubeworm tunnels)
    # Sea People Necklace is awarded by completing Deliver Key to Tenzhin (Ch4 mission)
    _set_location_rule(multiworld, player,
        "Sub-Mission: Curious Child",
        lambda state: state.has("Sea People Necklace", player)
    )
    # Gate Sea People Necklace source location on Sea People Village access
    # (Deliver Key to Tenzhin is a Ch4 mission in Sea People Village)
    # The region gate already handles this — no extra rule needed ✅

    # Gate Gas Cutter source on Mid depth access (Leahs-chan Rescue is in Blue Hole Mid)
    # Region gate handles this ✅

    # Catch Runaway Seahorses requires Bug Net
    _set_location_rule(multiworld, player,
        "Sub-Mission: Catch the Runaway Seahorses",
        lambda state: state.has("Bug Net", player)
    )
    # Talk to Yami requires Catch Runaway Seahorses done first
    _set_location_rule(multiworld, player,
        "Sub-Mission: Talk to Yami at the Game Parlor",
        lambda state: (
            state.has("Bug Net", player) and
            state.can_reach("Sub-Mission: Catch the Runaway Seahorses", "Location", player)
        )
    )

    # Stormy Night = Truck Hermit Crab vortex boss.
    # Unlocked after receiving Sea People Gloves (grip gloves) in vanilla.
    # The gloves aren't used to enter but are the unlock trigger.
    _set_location_rule(multiworld, player,
        "Sub-Mission: Stormy Night",
        lambda state: state.has("Sea People Gloves", player)
    )

    # Weaponsmith Duff — always accessible (Bancho Sushi region), no extra gate.
    # Completing it unlocks Duff's Weapon Shop — so all weapon craft locations
    # require this sub-mission to have been completed (or sub-missions to be off,
    # in which case the weapon shop is gated by having any weapon in inventory).
    # Gate all weapon craft locations on Weaponsmith Duff being reachable+collected
    # OR on sub-missions being disabled (in which case no gate needed since
    # Duff's shop would be assumed open from the start).
    if world.options.include_sub_missions.value and world.options.include_weapon_shop.value:
        for loc_name in list(multiworld.get_locations(player)):
            if loc_name.name.startswith("Craft:"):
                add_rule(loc_name,
                    lambda state: state.can_reach("Sub-Mission: Weaponsmith Duff", "Location", player)
                )

    # === GODZILLA DLC: EBIRAH + KAIJU FIGURINE RULES ===
    # The Godzilla DLC story triggers the morning after completing Chapter 5.
    # Gate Ebirah's defeat location on having Chapter 5 Complete in inventory.
    _set_location_rule(multiworld, player,
        "Defeat: Ebirah",
        lambda state: state.has("Chapter 5 Complete", player)
    )

    # Kaiju figurines and Godzilla recipes unlock after defeating Ebirah.
    # We use can_reach("Defeat: Ebirah") — the correct AP primitive for checking
    # that a location has been collected. This naturally chains from Chapter 5 Complete.
    def ebirah_defeated(state) -> bool:
        return state.can_reach("Defeat: Ebirah", "Location", player)

    # Gate all named Kaiju figurines on Ebirah being defeated
    kaiju_names = [
        "Godzilla (1965)", "Ebirah (1966)", "Minilla (1967)", "Hedorah (1971)",
        "Gigan (1972)", "Jet Jaguar (1973)", "King Caesar (1974)", "Mechagodzilla (1975)",
        "Biolante (1989)", "King Ghidorah (1991)", "Mecha-King Ghidorah (1991)",
        "Rodan (1993)", "Godzilla (1994)", "SpaceGodzilla (1994)", "Little Godzilla (1994)",
        "Destoroyah (1995)", "Godzilla (1995)", "Anguirus (2004)", "Mothra (1961)",
        "Godzilla (2016)",
    ]
    for name in kaiju_names:
        _set_location_rule(multiworld, player,
            f"Kaiju Figurine: {name}",
            lambda state: ebirah_defeated(state)
        )

    # Godzilla recipes also unlock after Ebirah is defeated
    for recipe_loc in [
        "Unlock Recipe: Godzilla vs. Ebirah Curry",
        "Unlock Recipe: Ebirah Chasing Sashimi",
        "Unlock Recipe: Deep Sea Kaiju Ramen",
    ]:
        _set_location_rule(multiworld, player, recipe_loc,
            lambda state: ebirah_defeated(state)
        )

    # === ECOWATCHER DEPTH RULES ===
    # Ecowatcher missions require access to the region where their target
    # organisms live. Most Blue Hole missions are already in the correct
    # region, but we add explicit depth gates for missions that need
    # specific depth tiers or special regions.
    if world.options.include_ecowatcher.value:
        # Glacial Area missions require Glacial Passage access
        glacial_missions = [
            "Ecowatcher: Investigate Regional Ecology 1",
            "Ecowatcher: Investigate Regional Ecology 2",
            "Ecowatcher: Investigate Glacial Marine Plants 1",
            "Ecowatcher: Investigate Glacial Marine Plants 2",
            "Ecowatcher: Investigate Glacial Marine Plants 3",
            "Ecowatcher: Collect Glacial Clams 1",
            "Ecowatcher: Collect Glacial Clams 2",
            "Ecowatcher: Defeat Invasive Starfish 1",
            "Ecowatcher: Defeat Invasive Starfish 2",
            "Ecowatcher: Investigate Sea People's Artifact 1",
            "Ecowatcher: Investigate Sea People's Artifact 2",
            "Ecowatcher: Investigate Dangerous Gemstones 1",
            "Ecowatcher: Investigate Dangerous Gemstones 2",
        ]
        for mission in glacial_missions:
            _set_location_rule(multiworld, player, mission,
                # Glacial Passage: needs Key to Tenzhin + Cold-Resistant suit (level 7+)
                lambda state: (
                    state.has("Key to Tenzhin", player) and
                    (state.has("Progressive Diving Suit", player, 7) or
                     (state.has("Teleport Mirror", player) and state.has("Teleport to Glacier", player)))
                )
            )

        # Hydrothermal Vents missions — deepest region, requires Glacier Zone access
        vents_missions = [
            "Ecowatcher: Investigate Regional Ecology 3",
            "Ecowatcher: Investigate Dangerous Gemstones 3",
        ]
        for mission in vents_missions:
            _set_location_rule(multiworld, player, mission,
                lambda state: (
                    state.has("Progressive Diving Suit", player, 8) and
                    state.has("Tech Suit Parts", player, 3) and
                    (state.has("Key to Tenzhin", player) or
                     (state.has("Teleport Mirror", player) and state.has("Teleport to Glacier", player)))
                )
            )

        # Deep Blue Hole missions (Overpopulated Invasive Fish 4-6, Remove Jellyfish 3-4)
        # target deep-sea species like Fangtooth and Bluespotted Stargazer
        deep_missions = [
            "Ecowatcher: Cull Invasive Fish 4",
            "Ecowatcher: Cull Invasive Fish 5",
            "Ecowatcher: Remove Jellyfish 3",
            "Ecowatcher: Remove Jellyfish 4",
            "Ecowatcher: Research Starfish 4",
            "Ecowatcher: Research Starfish 5",
            "Ecowatcher: Research Shell 4",
            "Ecowatcher: Research Shell 5",
        ]
        for mission in deep_missions:
            _set_location_rule(multiworld, player, mission,
                lambda state: has_depth_access(state, player, 3)  # Deep
            )

    # === PHOTOGRAPHY RULES ===
    # Underwater Camera is given by Dr. Bacon after completing "Beyond the Rock Pile"
    # (Chapter 1 main mission). All photography locations require the camera.
    if world.options.include_photography.value:
        for loc in multiworld.get_locations(player):
            if loc.name.startswith("Photo:"):
                add_rule(loc,
                    lambda state: state.has("Underwater Camera", player)
                )

        # Specific photo spots require mission chains to be completed first:
        # Manta Ray — triggered by the night-diving lighting mission + Night Dive required
        _set_location_rule(multiworld, player,
            "Photo: Manta Ray",
            lambda state: (
                state.has("Underwater Camera", player) and
                state.has("Night Dive Unlock", player) and
                state.can_reach("Sub-Mission: Take Pictures of Manta Ray", "Location", player)
            )
        )
        # Take Pictures of Manta Ray sub-mission also needs Night Dive Unlock
        _set_location_rule(multiworld, player,
            "Sub-Mission: Take Pictures of Manta Ray",
            lambda state: state.has("Night Dive Unlock", player)
        )
        # Loggerhead Turtle — spawns after Finding the Seaweed Collector (= Stormy Night chain)
        _set_location_rule(multiworld, player,
            "Photo: Loggerhead Turtle",
            lambda state: (
                state.has("Underwater Camera", player) and
                state.can_reach("Sub-Mission: Stormy Night", "Location", player)
            )
        )
        # Baby Humpback Whale — unlocked during whale rescue chain
        _set_location_rule(multiworld, player,
            "Photo: Baby Humpback Whale",
            lambda state: (
                state.has("Underwater Camera", player) and
                state.can_reach("Sub-Mission: Finding the Baby Whale", "Location", player)
            )
        )
        # Underwater Lake — found during Curious Child mission
        _set_location_rule(multiworld, player,
            "Photo: Underwater Lake",
            lambda state: (
                state.has("Underwater Camera", player) and
                state.can_reach("Sub-Mission: Curious Child", "Location", player)
            )
        )
        # Pink Dolphin — triggered by the dolphin quest chain
        _set_location_rule(multiworld, player,
            "Photo: Pink Dolphin",
            lambda state: (
                state.has("Underwater Camera", player) and
                state.can_reach("Sub-Mission: What Happened to the Dolphins?", "Location", player)
            )
        )

    # === DISH UPGRADE RULES ===
    # All dish research tiers (Level 2+) require the recipe to be unlocked first.
    # We match "Upgrade [Dish] to Level N" with "Unlock Recipe: [Dish]" via name.
    # For base game sushi (auto-unlocked by catching fish), the region gate already
    # handles access — but for Menu dishes gated by VIP/staff/Cooksta, this ensures
    # the player can't research a dish they haven't unlocked the recipe for.
    if world.options.dish_upgrades.value > 0 and world.options.recipe_checks.value > 0:
        recipe_names = {
            # Build set of recipe names that have explicit unlock locations
            loc.name[len("Unlock Recipe: "):] 
            for loc in multiworld.get_locations(player)
            if loc.name.startswith("Unlock Recipe: ")
        }
        for loc in multiworld.get_locations(player):
            if loc.name.startswith("Upgrade ") and " to Level " in loc.name:
                # Extract dish name from "Upgrade [Dish] to Level N"
                dish = loc.name[len("Upgrade "):loc.name.rfind(" to Level ")]
                if dish in recipe_names:
                    unlock_loc = f"Unlock Recipe: {dish}"
                    add_rule(loc,
                        lambda state, ul=unlock_loc: state.can_reach(ul, "Location", player)
                    )

    # === GODZILLA DISH UPGRADE RULES ===
    # Godzilla recipes unlock after Ebirah is defeated — dish upgrades also gated.
    if world.options.has_godzilla_dlc.value:
        for godzilla_dish in [
            "Godzilla vs. Ebirah Curry",
            "Ebirah Chasing Sashimi",
            "Deep Sea Kaiju Ramen",
        ]:
            for loc in multiworld.get_locations(player):
                if loc.name.startswith(f"Upgrade {godzilla_dish} to Level"):
                    add_rule(loc,
                        lambda state: state.can_reach("Defeat: Ebirah", "Location", player)
                    )

    # === OTTO'S GIFT / FISH FARM RULE ===
    # Otto's Gift? is gated by completing A Noisy Customer sub-mission
    # (Otto appears as a noisy customer first, then offers his gift/fish farm)
    _set_location_rule(multiworld, player,
        "Quest: Otto's Gift?",
        lambda state: state.can_reach("Quest: Complete A Noisy Customer (Unlock Fish Farm)", "Location", player)
    )
    # Fish Farm unlock comes from Otto's quest chain
    # (already gated by region/item — Unlock Fish Farm item gates the farm region)

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

    # All insects (net-caught + battle) require Bug Net
    for insect_name in [
        "Insect: Catch Ulysses Swallowtail", "Insect: Catch Stick Insect",
        "Insect: Catch Gigas Giant Longhorn Beetle", "Insect: Catch Diving Beetle",
        "Insect: Catch Takua Cicada", "Insect: Catch Blue Admiral Butterfly",
        "Insect: Catch Common Lascar Butterfly", "Insect: Catch Striped Blue Crow Butterfly",
        "Insect: Catch Paper Kite Butterfly", "Insect: Catch Common Green Birdwing",
        "Insect: Catch Rajah Brooke's Birdwing", "Insect: Catch Atlas Moth",
        "Insect: Catch Firefly", "Insect: Catch Moth",
        "Insect: Catch Sea Green Swallowtail", "Insect: Catch Gigon Swallowtail",
        "Insect: Catch Common Grass Yellow Butterfly", "Insect: Catch Common Albatross Butterfly",
        "Insect: Catch Blanchard's Ghost Butterfly",
        "Insect Battle: Defeat Little Stag Beetle", "Insect Battle: Defeat Caucasus Beetle",
        "Insect Battle: Defeat Atlas Beetle", "Insect Battle: Defeat Five-Horned Rhinoceros Beetle",
        "Insect Battle: Defeat Siamese Five-Horned Beetle", "Insect Battle: Defeat Siamese Rhinoceros Beetle",
        "Insect Battle: Defeat Femoralis Stag Beetle", "Insect Battle: Defeat Steveni Stag Beetle",
        "Insect Battle: Defeat Giraffe Stag Beetle", "Insect Battle: Defeat Zebra Stag Beetle",
        "Insect Battle: Defeat Giant Stag Beetle", "Insect Battle: Defeat Antler Stag Beetle",
        "Insect Battle: Defeat Metallic Stag Beetle", "Insect Battle: Defeat Striata Stag Beetle",
        "Insect Battle: Defeat Rosenbergi Stag Beetle", "Insect Battle: Defeat Boss Stag Beetle",
        "Insect Battle: Defeat Boss Beetle",
        "Jungle Insectagram: 50% Complete", "Jungle Insectagram: 100% Complete",
    ]:
        _set_location_rule(multiworld, player, insect_name,
            lambda state: state.has("Bug Net", player)
        )

    # Land fishing + rod-caught fish require Fishing Rod
    _set_location_rule(multiworld, player,
        "Jungle Minigame: First Land Fishing Catch",
        lambda state: state.has("Fishing Rod", player)
    )
    for loc_name in [
        "First Catch: Moonlight Gourami", "First Catch: Three Spot Gourami",
        "First Catch: Malayan Leaf Fish", "First Catch: Snakeskin Gourami",
        "First Catch: Giant Gourami", "First Catch: Emperor Snakehead",
        "First Catch: Striped Snakehead", "First Catch: Peacock Bass",
        "First Catch: Tambaqui", "First Catch: Malayan Mahseer",
        "First Catch: Redtail Catfish", "First Catch: Tapah",
    ]:
        _set_location_rule(multiworld, player, loc_name,
            lambda state: state.has("Fishing Rod", player)
        )

    # Jungle boss fish — first catch requires defeating the boss first
    _set_location_rule(multiworld, player,
        "First Catch: Giant Snapping Turtle",
        lambda state: state.can_reach("Jungle Boss: Defeat Giant Snapping Turtle", "Location", player)
    )
    _set_location_rule(multiworld, player,
        "First Catch: Black Caiman",
        lambda state: state.can_reach("Jungle Boss: Defeat Black Caiman", "Location", player)
    )
    _set_location_rule(multiworld, player,
        "First Catch: Sulong",
        lambda state: state.can_reach("Jungle Boss: Defeat Sulong", "Location", player)
    )
    _set_location_rule(multiworld, player,
        "First Catch: Stethacanthus",
        lambda state: state.can_reach("Jungle Boss: Defeat Stethacanthus", "Location", player)
    )
    _set_location_rule(multiworld, player,
        "First Catch: Xiphactinus",
        lambda state: state.can_reach("Jungle Boss: Defeat Xiphactinus", "Location", player)
    )
    _set_location_rule(multiworld, player,
        "First Catch: Basilosaurus",
        lambda state: state.can_reach("Jungle Boss: Defeat Basilosaurus", "Location", player)
    )

    # Jungle gun upgrades — each mode requires having at least 1 copy of that weapon
    for i in range(1, 7):
        _set_location_rule(multiworld, player,
            f"Jungle Gun: Rifle Level {i}",
            lambda state, lvl=i: state.has("Progressive Jungle Rifle", player, lvl)
        )
        _set_location_rule(multiworld, player,
            f"Jungle Gun: Shotgun Level {i}",
            lambda state, lvl=i: state.has("Progressive Jungle Shotgun", player, lvl)
        )
        _set_location_rule(multiworld, player,
            f"Jungle Gun: Sniper Level {i}",
            lambda state, lvl=i: state.has("Progressive Jungle Sniper", player, lvl)
        )
        _set_location_rule(multiworld, player,
            f"Jungle Gun: Net Gun Level {i}",
            lambda state, lvl=i: state.has("Progressive Jungle Net Gun", player, lvl)
        )

    # Jungle boss sequence
    _set_location_rule(multiworld, player,
        "Jungle Boss: Defeat Black Caiman",
        lambda state: state.has("Jungle Chapter 2 Complete", player)
    )
    _set_location_rule(multiworld, player,
        "Jungle Boss: Defeat Sulong",
        lambda state: state.has("Jungle Chapter 7 Complete", player)
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
