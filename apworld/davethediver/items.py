"""
Dave the Diver - Item Definitions

This file defines all items that can be randomized in Dave the Diver.
"""

from typing import Dict, NamedTuple, Optional
from BaseClasses import Item, ItemClassification


class DaveDiverItem(Item):
    """An item in Dave the Diver"""
    game: str = "Dave the Diver"


class ItemData(NamedTuple):
    """Data for an item definition"""
    code: Optional[int]
    classification: ItemClassification
    count: int = 1        # How many of this item exist
    category: str = ""    # Used for option-based filtering (e.g. "recipe", "trap")


# Item classifications:
# - progression: Required to complete the game
# - useful: Helpful but not required
# - filler: Common items to fill remaining locations
# - trap: Negative effects

# Base item ID - Archipelago uses ranges for each game
# We'll use 0x444400 (DvD in hex-ish) as our base
BASE_ID = 0x444400

# Item IDs are offset from ITEM_BASE to avoid collisions with location IDs.
# Location IDs use ITEM_BASE + 0 to ITEM_BASE + ~3500.
# Item IDs use ITEM_BASE + 0 upward (i.e. ITEM_BASE + 5000+).
ITEM_BASE = BASE_ID + 5000


# === PROGRESSIVE EQUIPMENT ===
# These use a single item with multiple copies to represent depth upgrades.
# Counts are intentionally generous so the player is never completely blocked by depth —
# the thresholds in rules.py require only a fraction of the total copies.
progressive_equipment: Dict[str, ItemData] = {
    # 6 copies: rules only require 1 (mid) or 3 (deep) — plenty of slack
    "Progressive Oxygen Tank": ItemData(ITEM_BASE + 300, ItemClassification.progression, count=6),
    # 4 copies: rules only require 1 (mid) or 2 (deep)
    "Progressive Harpoon": ItemData(ITEM_BASE + 301, ItemClassification.progression, count=4),
    # 8 copies — one per suit level (including cold-resistant tiers):
    #   Level 1: max 40m  (from start, but progressive copies unlock deeper)
    #   Level 2: max 80m
    #   Level 3: max 150m
    #   Level 4: max 230m
    #   Level 5: max 375m
    #   Level 6: max 540m
    #   Level 7: max 560m  (Cold-Resistant tier 1 — required for Glacial Passage)
    #   Level 8: max 800m  (Cold-Resistant tier 2 — required for full Glacier Zone)
    # Rules only require a subset of these for each depth gate.
    "Progressive Diving Suit": ItemData(ITEM_BASE + 302, ItemClassification.progression, count=8),
}

# === WEAPONS & EQUIPMENT ===
# Each craftable weapon variant is its own item — received as AP reward when sent.
# Classification: useful (better weapons help but aren't required for progression).
# IDs allocated in blocks starting at ITEM_BASE + 0.
_W = ITEM_BASE  # weapon items start at ITEM_BASE + 0

weapon_items: Dict[str, ItemData] = {
    # --- Basic Underwater Rifle tree ---
    "Basic Underwater Rifle":           ItemData(_W + 0,  ItemClassification.progression),  # Starting weapon
    "Underwater Rifle II":              ItemData(_W + 1,  ItemClassification.useful),
    "Underwater Rifle III":             ItemData(_W + 2,  ItemClassification.useful),
    "Death Rifle":                      ItemData(_W + 3,  ItemClassification.useful),
    "Flame Rifle I":                    ItemData(_W + 4,  ItemClassification.useful),
    "Flame Rifle II":                   ItemData(_W + 5,  ItemClassification.useful),
    "Explosive Rifle":                  ItemData(_W + 6,  ItemClassification.useful),
    "Tranquilizer Rifle":               ItemData(_W + 7,  ItemClassification.useful),
    "Poison Rifle I":                   ItemData(_W + 8,  ItemClassification.useful),
    "Poison Rifle II":                  ItemData(_W + 9,  ItemClassification.useful),
    "Hell Poison Rifle":                ItemData(_W + 10, ItemClassification.useful),
    "Lightning Rifle I":                ItemData(_W + 11, ItemClassification.useful),
    "Lightning Rifle II":               ItemData(_W + 12, ItemClassification.useful),
    "Shock Rifle I":                    ItemData(_W + 13, ItemClassification.useful),
    "Shock Rifle II":                   ItemData(_W + 14, ItemClassification.useful),
    "Thunderbolt Rifle":                ItemData(_W + 15, ItemClassification.useful),

    # --- Small Net Gun tree ---
    "Small Net Gun":                    ItemData(_W + 16, ItemClassification.useful),
    "Medium Net Gun":                   ItemData(_W + 17, ItemClassification.useful),
    "Large Net Gun":                    ItemData(_W + 18, ItemClassification.useful),
    "Steel Net Gun":                    ItemData(_W + 19, ItemClassification.useful),

    # --- Hush Dart tree ---
    "Hush Dart":                        ItemData(_W + 20, ItemClassification.useful),
    "Enhanced Hush Dart":               ItemData(_W + 21, ItemClassification.useful),

    # --- Triple Axel tree ---
    "Triple Axel":                      ItemData(_W + 22, ItemClassification.useful),
    "Quattro Axel":                     ItemData(_W + 23, ItemClassification.useful),
    "Quattro Axel II":                  ItemData(_W + 24, ItemClassification.useful),
    "Penta Axel":                       ItemData(_W + 25, ItemClassification.useful),
    "Flame Triple Axel":                ItemData(_W + 26, ItemClassification.useful),
    "Flame Triple Axel II":             ItemData(_W + 27, ItemClassification.useful),
    "Explosive Triple Axel":            ItemData(_W + 28, ItemClassification.useful),
    "Tranquilizer Triple Axel":         ItemData(_W + 29, ItemClassification.useful),
    "Poison Triple Axel":               ItemData(_W + 30, ItemClassification.useful),
    "Poison Triple Axel II":            ItemData(_W + 31, ItemClassification.useful),
    "Hell Poison Triple Axel":          ItemData(_W + 32, ItemClassification.useful),
    "Lightning Triple Axel":            ItemData(_W + 33, ItemClassification.useful),
    "Shock Triple Axel":                ItemData(_W + 34, ItemClassification.useful),
    "Shock Triple Axel II":             ItemData(_W + 35, ItemClassification.useful),
    "Thunderbolt Triple Axel":          ItemData(_W + 36, ItemClassification.useful),

    # --- Red Sniper Rifle tree ---
    "Red Sniper Rifle":                 ItemData(_W + 37, ItemClassification.useful),
    "Red Sniper Rifle II":              ItemData(_W + 38, ItemClassification.useful),
    "Red Sniper Rifle III":             ItemData(_W + 39, ItemClassification.useful),
    "Death Sniper Rifle":               ItemData(_W + 40, ItemClassification.useful),
    "Flame Sniper Rifle I":             ItemData(_W + 41, ItemClassification.useful),
    "Flame Sniper Rifle II":            ItemData(_W + 42, ItemClassification.useful),
    "Explosive Sniper Rifle":           ItemData(_W + 43, ItemClassification.useful),
    "Tranquilizer Mosin-Nagant":        ItemData(_W + 44, ItemClassification.useful),
    "Poison Sniper Rifle I":            ItemData(_W + 45, ItemClassification.useful),
    "Poison Sniper Rifle II":           ItemData(_W + 46, ItemClassification.useful),
    "Hell Poison Sniper Rifle":         ItemData(_W + 47, ItemClassification.useful),
    "Lightning Sniper Rifle I":         ItemData(_W + 48, ItemClassification.useful),
    "Lightning Sniper Rifle II":        ItemData(_W + 49, ItemClassification.useful),
    "Shock Sniper Rifle I":             ItemData(_W + 50, ItemClassification.useful),
    "Shock Sniper Rifle II":            ItemData(_W + 51, ItemClassification.useful),
    "Thunderbolt Sniper Rifle":         ItemData(_W + 52, ItemClassification.useful),

    # --- Sticky Bomb Gun tree ---
    "Sticky Bomb Gun":                  ItemData(_W + 53, ItemClassification.useful),
    "Sticky Bomb Gun II":               ItemData(_W + 54, ItemClassification.useful),
    "Sticky Bomb Gun III":              ItemData(_W + 55, ItemClassification.useful),
    "Sticky Mine Launcher I":           ItemData(_W + 56, ItemClassification.useful),
    "Sticky Mine Launcher II":          ItemData(_W + 57, ItemClassification.useful),
    "Sticky Tranquilizing Bomb Gun":    ItemData(_W + 58, ItemClassification.useful),
    "Poison Mine Launcher":             ItemData(_W + 59, ItemClassification.useful),
    "Poison Mine Launcher II":          ItemData(_W + 60, ItemClassification.useful),
    "Lightning Mine Launcher I":        ItemData(_W + 61, ItemClassification.useful),
    "Lightning Mine Launcher II":       ItemData(_W + 62, ItemClassification.useful),
    "Shock Mine Launcher I":            ItemData(_W + 63, ItemClassification.useful),
    "Shock Mine Launcher II":           ItemData(_W + 64, ItemClassification.useful),

    # --- Grenade Launcher tree ---
    "Grenade Launcher":                 ItemData(_W + 65, ItemClassification.useful),
    "Grenade Launcher II":              ItemData(_W + 66, ItemClassification.useful),
    "Grenade Launcher III":             ItemData(_W + 67, ItemClassification.useful),
    "Tranquilizer Gas Bomb Launcher":   ItemData(_W + 68, ItemClassification.useful),
    "Poison Launcher":                  ItemData(_W + 69, ItemClassification.useful),
    "Gravity Launcher":                 ItemData(_W + 70, ItemClassification.useful),
    "Blackhole Launcher":               ItemData(_W + 71, ItemClassification.useful),
    "Flash Grenade Launcher":           ItemData(_W + 72, ItemClassification.useful),

    # --- Ice Gun tree ---
    "Ice Gun":                          ItemData(_W + 73, ItemClassification.useful),
    "Enhanced Ice Gun":                 ItemData(_W + 74, ItemClassification.useful),
    "Ultra Ice Gun":                    ItemData(_W + 75, ItemClassification.useful),

    # --- Drain Gun tree (DREDGE DLC only) ---
    "Drain Gun":                        ItemData(_W + 76, ItemClassification.useful, category="dlc_dredge"),
    "Enhanced Drain Gun":               ItemData(_W + 77, ItemClassification.useful, category="dlc_dredge"),
    "Power Drain Gun":                  ItemData(_W + 78, ItemClassification.useful, category="dlc_dredge"),

    # --- Melee weapons ---
    "Dive Knife":                       ItemData(_W + 79, ItemClassification.progression),
    "Upgraded Dive Knife":              ItemData(_W + 80, ItemClassification.useful),

}

# === AREA UNLOCK ITEMS (Specific Items) ===
# These are NOT progressive - you get them once and unlock specific areas
area_unlock_items: Dict[str, ItemData] = {
    # Physical access items
    "Sea People Gloves": ItemData(ITEM_BASE + 100, ItemClassification.progression),  # Lets you swim to Sea People Village
    "Sea People Translator": ItemData(ITEM_BASE + 101, ItemClassification.progression),  # Required to interact in village
    # Note: Cold-Resistant suit tiers are now levels 7 and 8 of Progressive Diving Suit
    "Key to Tenzhin": ItemData(ITEM_BASE + 103, ItemClassification.progression),  # Opens Glacial Passage gate
    "Tech Suit Parts": ItemData(ITEM_BASE + 104, ItemClassification.progression, count=3),  # Needed for Glacier Zone
    "Laser Device": ItemData(ITEM_BASE + 105, ItemClassification.progression),  # Required to open Broken Control Room
    "Control Room Button": ItemData(ITEM_BASE + 106, ItemClassification.progression, count=3),  # 3 needed for Ch7 finale
    "Sea People's Trust":   ItemData(ITEM_BASE + 107, ItemClassification.progression),  # Unlocks Duff's Dream Concert + Ch4
    "Underwater Camera":    ItemData(ITEM_BASE + 113, ItemClassification.progression),   # Given by Dr. Bacon after Beyond the Rock Pile — gates photography
    "Gas Cutter":               ItemData(ITEM_BASE + 250, ItemClassification.progression),   # Required for Giant Squid fight (150m)
    "Beluga Whale Ride Whistle":ItemData(ITEM_BASE + 254, ItemClassification.progression),  # Required for Phantom Jellyfish fight
    "Marinca Completion Trophy":ItemData(ITEM_BASE + 255, ItemClassification.progression),  # Required for Lusca — complete Marinca collection
    "Cobra's Lost Crowbar":     ItemData(ITEM_BASE + 256, ItemClassification.progression),  # Required for Giant Gadon — from Cobra's Lost Crowbar mission
    "Headlamp":             ItemData(ITEM_BASE + 251, ItemClassification.progression),   # Required for Giant Wolf Eel fight (250m)
    "Heat-Resistant Gloves":ItemData(ITEM_BASE + 252, ItemClassification.progression),  # Required for Kronosaurus / Hydrothermal Vents
    "Salvage Drone":        ItemData(ITEM_BASE + 253, ItemClassification.progression),  # Required for Goblin Shark (Yellow Shipwreck)
    "Cocktails Unlocked":   ItemData(ITEM_BASE + 112, ItemClassification.progression),  # Vincent Visit 3 — gates Ichiban DLC
    "Vortex Entry": ItemData(ITEM_BASE + 119, ItemClassification.progression, count=5, category="dlc_dredge"),  # DREDGE DLC vortex regions

    # Teleport system - allows bypassing physical routes
    "Teleport Mirror": ItemData(ITEM_BASE + 108, ItemClassification.progression),  # Base teleport ability
    "Teleport to Sea People Village": ItemData(ITEM_BASE + 109, ItemClassification.progression),  # Alt route to village
    "Teleport to Glacier": ItemData(ITEM_BASE + 110, ItemClassification.progression),  # Direct access to Glacier Zone
    "Teleport to Deep Blue Hole": ItemData(ITEM_BASE + 111, ItemClassification.useful),  # Backtracking QoL

    # Farm unlock items — each farm is a separate system that must be unlocked
    "Unlock Fish Farm": ItemData(ITEM_BASE + 116, ItemClassification.progression),       # Otto's quest "A Noisy Customer"
    "Unlock Vegetable Farm": ItemData(ITEM_BASE + 117, ItemClassification.progression),  # Unlocked via story/quest
    "Unlock Chicken Farm": ItemData(ITEM_BASE + 118, ItemClassification.progression),    # Same location as veg farm, separate system

    # Other key items
    "VIP Card": ItemData(ITEM_BASE + 115, ItemClassification.useful),  # Unlocks VIP restaurant events
}

# === DIVING EQUIPMENT (Non-Progressive) ===
diving_equipment: Dict[str, ItemData] = {
    # Oxygen efficiency (multiplies oxygen duration)
    "Oxygen Efficiency Upgrade": ItemData(ITEM_BASE + 120, ItemClassification.useful, count=2),
    
    # Durability
    "Diving Suit Durability +1": ItemData(ITEM_BASE + 125, ItemClassification.useful, count=3),
    
    # Tools
    "Fish Radar": ItemData(ITEM_BASE + 130, ItemClassification.useful),
    "Enhanced Night Vision": ItemData(ITEM_BASE + 131, ItemClassification.useful),
    "Crab Trap": ItemData(ITEM_BASE + 135, ItemClassification.useful),
    "Enhanced Crab Trap": ItemData(ITEM_BASE + 136, ItemClassification.useful),
    # TODO: Add more tools
}

# === RESTAURANT & RECIPES ===
restaurant_items: Dict[str, ItemData] = {
    # Named staff members — base game (21 staff, each is a unique progression item)
    # Receiving "[Name]" unlocks that staff member for Bancho Sushi.
    # Training milestones are location checks; staff items gate recipe unlocks.
    "Billy":      ItemData(ITEM_BASE + 850, ItemClassification.progression, category="restaurant"),
    "Carolina":   ItemData(ITEM_BASE + 851, ItemClassification.progression, category="restaurant"),
    "Charlie":    ItemData(ITEM_BASE + 852, ItemClassification.progression, category="restaurant"),
    "Cohh":       ItemData(ITEM_BASE + 853, ItemClassification.progression, category="restaurant"),
    "Davina":     ItemData(ITEM_BASE + 854, ItemClassification.progression, category="restaurant"),
    "Drae":       ItemData(ITEM_BASE + 855, ItemClassification.progression, category="restaurant"),
    "El Nino":    ItemData(ITEM_BASE + 856, ItemClassification.progression, category="restaurant"),
    "Itsuki":     ItemData(ITEM_BASE + 857, ItemClassification.progression, category="restaurant"),
    "James":      ItemData(ITEM_BASE + 858, ItemClassification.progression, category="restaurant"),
    "Jandi":      ItemData(ITEM_BASE + 859, ItemClassification.progression, category="restaurant"),
    "Kyoko":      ItemData(ITEM_BASE + 860, ItemClassification.progression, category="restaurant"),
    "Liu":        ItemData(ITEM_BASE + 861, ItemClassification.progression, category="restaurant"),
    "Maki":       ItemData(ITEM_BASE + 862, ItemClassification.progression, category="restaurant"),
    "Masayoshi":  ItemData(ITEM_BASE + 863, ItemClassification.progression, category="restaurant"),
    "Mitchell":   ItemData(ITEM_BASE + 864, ItemClassification.progression, category="restaurant"),
    "Pai":        ItemData(ITEM_BASE + 865, ItemClassification.progression, category="restaurant"),
    "Raptor":     ItemData(ITEM_BASE + 866, ItemClassification.progression, category="restaurant"),
    "Raul":       ItemData(ITEM_BASE + 867, ItemClassification.progression, category="restaurant"),
    "Tohoku":     ItemData(ITEM_BASE + 868, ItemClassification.progression, category="restaurant"),
    "Yone":       ItemData(ITEM_BASE + 869, ItemClassification.progression, category="restaurant"),
    "Yusuke":     ItemData(ITEM_BASE + 870, ItemClassification.progression, category="restaurant"),
    # Ichiban DLC staff (single-copy, milestone mode)
    "Hamako":     ItemData(ITEM_BASE + 871, ItemClassification.progression, category="dlc_ichiban"),
    "Etsuko":     ItemData(ITEM_BASE + 872, ItemClassification.progression, category="dlc_ichiban"),
    "Chitose":    ItemData(ITEM_BASE + 873, ItemClassification.progression, category="dlc_ichiban"),

    # Progressive staff items (×20 each) for staff_training_depth=all_levels
    # Finding "Progressive Maki" the Nth time trains her to level N.
    # IDs at ITEM_BASE + 900 + idx*1 (21 base game + 3 Ichiban DLC = 24 items)
    "Progressive Billy":      ItemData(ITEM_BASE + 900, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Carolina":   ItemData(ITEM_BASE + 901, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Charlie":    ItemData(ITEM_BASE + 902, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Cohh":       ItemData(ITEM_BASE + 903, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Davina":     ItemData(ITEM_BASE + 904, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Drae":       ItemData(ITEM_BASE + 905, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive El Nino":    ItemData(ITEM_BASE + 906, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Itsuki":     ItemData(ITEM_BASE + 907, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive James":      ItemData(ITEM_BASE + 908, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Jandi":      ItemData(ITEM_BASE + 909, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Kyoko":      ItemData(ITEM_BASE + 910, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Liu":        ItemData(ITEM_BASE + 911, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Maki":       ItemData(ITEM_BASE + 912, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Masayoshi":  ItemData(ITEM_BASE + 913, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Mitchell":   ItemData(ITEM_BASE + 914, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Pai":        ItemData(ITEM_BASE + 915, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Raptor":     ItemData(ITEM_BASE + 916, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Raul":       ItemData(ITEM_BASE + 917, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Tohoku":     ItemData(ITEM_BASE + 918, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Yone":       ItemData(ITEM_BASE + 919, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Yusuke":     ItemData(ITEM_BASE + 920, ItemClassification.progression, count=20, category="restaurant"),
    "Progressive Hamako":     ItemData(ITEM_BASE + 921, ItemClassification.progression, count=20, category="dlc_ichiban"),
    "Progressive Etsuko":     ItemData(ITEM_BASE + 922, ItemClassification.progression, count=20, category="dlc_ichiban"),
    "Progressive Chitose":    ItemData(ITEM_BASE + 923, ItemClassification.progression, count=20, category="dlc_ichiban"),

    # Restaurant Upgrades
    "Dining Area Expansion": ItemData(ITEM_BASE + 320, ItemClassification.useful, count=3, category="restaurant"),
    "Kitchen Upgrade": ItemData(ITEM_BASE + 330, ItemClassification.useful, count=3, category="restaurant"),

    # Key Recipes (as randomizable items - given to player as reward)
    # Basic fish sushi recipes
    "Recipe: Yellowfin Tuna Akami Sushi": ItemData(ITEM_BASE + 210, ItemClassification.useful, category="recipe"),
    "Recipe: Great Barracuda Sushi": ItemData(ITEM_BASE + 211, ItemClassification.useful, category="recipe"),
    "Recipe: Humboldt Squid Sushi": ItemData(ITEM_BASE + 212, ItemClassification.useful, category="recipe"),
    "Recipe: Greenland Shark Sushi": ItemData(ITEM_BASE + 213, ItemClassification.useful, category="recipe"),
    "Recipe: Blobfish Sushi": ItemData(ITEM_BASE + 214, ItemClassification.useful, category="recipe"),
    "Recipe: Narwhal Sushi": ItemData(ITEM_BASE + 215, ItemClassification.useful, category="recipe"),
    "Recipe: Vampire Squid Sushi": ItemData(ITEM_BASE + 216, ItemClassification.useful, category="recipe"),
    # VIP recipes
    "Recipe: Seagrapes Jellyfish Sushi": ItemData(ITEM_BASE + 220, ItemClassification.useful, category="recipe"),
    "Recipe: Tropical Fish Sushi Set": ItemData(ITEM_BASE + 221, ItemClassification.useful, category="recipe"),
    "Recipe: Vegetable Sushi": ItemData(ITEM_BASE + 222, ItemClassification.useful, category="recipe"),
    "Recipe: Humboldt Ink Pasta": ItemData(ITEM_BASE + 223, ItemClassification.useful, category="recipe"),
    "Recipe: Antarctic Octopus Carpaccio": ItemData(ITEM_BASE + 224, ItemClassification.useful, category="recipe"),
    "Recipe: Arctic Cod Risotto": ItemData(ITEM_BASE + 225, ItemClassification.useful, category="recipe"),
    "Recipe: Deep Fish Tempura": ItemData(ITEM_BASE + 226, ItemClassification.useful, category="recipe"),
    # Boss recipes
    "Recipe: White Shark Omelet": ItemData(ITEM_BASE + 230, ItemClassification.useful, category="recipe"),
    "Recipe: Clione Queen Soup": ItemData(ITEM_BASE + 231, ItemClassification.useful, category="recipe"),
    "Recipe: Steamed Wolf Eel": ItemData(ITEM_BASE + 232, ItemClassification.useful, category="recipe"),
    "Recipe: Goblin Shark Belly Roast": ItemData(ITEM_BASE + 233, ItemClassification.useful, category="recipe"),
    "Recipe: Phantom Jellyfish Jelly": ItemData(ITEM_BASE + 234, ItemClassification.useful, category="recipe"),
    "Recipe: Roasted Helicoprion Tail": ItemData(ITEM_BASE + 235, ItemClassification.useful, category="recipe"),
    "Recipe: Yawie Steamed Meat": ItemData(ITEM_BASE + 236, ItemClassification.useful, category="recipe"),
    # Cooksta rank recipes
    "Recipe: Seahorse Udon": ItemData(ITEM_BASE + 240, ItemClassification.useful, category="recipe"),
    "Recipe: Atlantic Bonito Curry": ItemData(ITEM_BASE + 241, ItemClassification.useful, category="recipe"),
    "Recipe: Humphead Parrotfish Curry": ItemData(ITEM_BASE + 242, ItemClassification.useful, category="recipe"),
    "Recipe: Dumbo Takoyaki": ItemData(ITEM_BASE + 243, ItemClassification.useful, category="recipe"),
    "Recipe: Great Barracuda Canape": ItemData(ITEM_BASE + 244, ItemClassification.useful, category="recipe"),
}

# === STORY KEY ITEMS ===
# Key items and progression unlocks tied to story milestones
story_key_items: Dict[str, ItemData] = {
    # Chapter completion flags (7 chapters)
    "Chapter 1 Complete": ItemData(ITEM_BASE + 400, ItemClassification.progression),
    "Chapter 2 Complete": ItemData(ITEM_BASE + 401, ItemClassification.progression),
    "Chapter 3 Complete": ItemData(ITEM_BASE + 402, ItemClassification.progression),
    "Chapter 4 Complete": ItemData(ITEM_BASE + 403, ItemClassification.progression),
    "Chapter 5 Complete": ItemData(ITEM_BASE + 404, ItemClassification.progression),
    "Chapter 6 Complete": ItemData(ITEM_BASE + 405, ItemClassification.progression),
    "Chapter 7 Complete": ItemData(ITEM_BASE + 406, ItemClassification.progression),
    # Key story items given by NPCs
    "Sea People Bracelet": ItemData(ITEM_BASE + 410, ItemClassification.useful),    # Survive out of oxygen briefly
    "Bug Net": ItemData(ITEM_BASE + 411, ItemClassification.useful),                # Catch small fish/seahorses
    "Cargo Box Upgrade": ItemData(ITEM_BASE + 412, ItemClassification.useful, count=3),  # Carry more loot
    "Night Dive Unlock": ItemData(ITEM_BASE + 413, ItemClassification.progression), # Unlocks night diving
    "iDiver App": ItemData(ITEM_BASE + 414, ItemClassification.progression),        # Equipment upgrade app
}

# === PROGRESSIVE DISH ITEMS ===
# Each Menu dish that has upgrade tiers gets a progressive item.
# count = max_level - 1 (level 1 is always available once recipe is unlocked).
# These are sent as AP items when a player completes a dish research check.
# Classification: useful (they improve restaurant income but aren't progression gates).

def _prog_dish(base_id: int, max_level: int, category: str = "dish_upgrade") -> ItemData:
    return ItemData(base_id, ItemClassification.useful, count=max_level - 1, category=category)

_PD = ITEM_BASE + 3000  # Start at 3000 to avoid all other item ID ranges

dish_upgrade_items: Dict[str, ItemData] = {
    "Progressive Agar Tokoroten":                          _prog_dish(_PD + 0,   7),
    "Progressive Antarctic Octopus Carpaccio":             _prog_dish(_PD + 1,   7),
    "Progressive Arctic Cod Risotto":                      _prog_dish(_PD + 2,   9),
    "Progressive Atlantic Bonito Curry":                   _prog_dish(_PD + 3,  12),
    "Progressive Batfish Ricebowl":                        _prog_dish(_PD + 4,   7),
    "Progressive Big-Eyed Scad and Soybean Paste Roast":   _prog_dish(_PD + 5,   7),
    "Progressive Black Vinegar Braised Parrotfish":        _prog_dish(_PD + 6,   6),
    "Progressive Blobfish Spring Roll":                    _prog_dish(_PD + 7,  10),
    "Progressive Boiled Porbeagle Shark":                  _prog_dish(_PD + 8,   7),
    "Progressive Boiled Sailfish and Seaweed":             _prog_dish(_PD + 9,   9),
    "Progressive Boiled Yellowback Fusilier":              _prog_dish(_PD + 10,  7),
    "Progressive Boiled and Deep-Fried White Shrimp":      _prog_dish(_PD + 11, 10),
    "Progressive Bluefin Tuna Rice Bowl":                  _prog_dish(_PD + 12,  9),
    "Progressive Comber Sandwich":                         _prog_dish(_PD + 13,  6),
    "Progressive Crimson Fish Roll":                       _prog_dish(_PD + 14,  9),
    "Progressive Crystal Lobster Roll":                    _prog_dish(_PD + 15,  9),
    "Progressive Deep Fish Tempura":                       _prog_dish(_PD + 16,  7),
    "Progressive Deep Sea Kaiju Ramen":                    _prog_dish(_PD + 17,  6, "dlc_godzilla"),
    "Progressive Godzilla vs. Ebirah Curry":               _prog_dish(_PD + 96,  9, "dlc_godzilla"),
    "Progressive Ebirah Chasing Sashimi":                  _prog_dish(_PD + 97,  9, "dlc_godzilla"),
    "Progressive Deep-Fried Eggplant Shrimp Meatballs":   _prog_dish(_PD + 18,  7),
    "Progressive Deep-Fried Red Lionfish":                 _prog_dish(_PD + 19,  4),
    "Progressive Deep-Fried Vegetables":                   _prog_dish(_PD + 20,  3),
    "Progressive Dried Stingray":                          _prog_dish(_PD + 21, 12),
    "Progressive Dumbo Takoyaki":                          _prog_dish(_PD + 22,  9),
    "Progressive Dusky Grouper Steak":                     _prog_dish(_PD + 23,  7),
    "Progressive Eggplant Soba Oyaki":                     _prog_dish(_PD + 24,  9),
    "Progressive Falcatus Soybean Paste Soup":             _prog_dish(_PD + 25,  7),
    "Progressive Fried Habanero Fangtooth":                _prog_dish(_PD + 26,  7),
    "Progressive Fried Onion Cuttlefish":                  _prog_dish(_PD + 27,  7),
    "Progressive Fried Rice with Sally Lightfoot Crab":    _prog_dish(_PD + 28, 10),
    "Progressive Fried Seahorses":                         _prog_dish(_PD + 29,  4),
    "Progressive Fried Tomato and Snailfish":              _prog_dish(_PD + 30,  7),
    "Progressive Great Barracuda Canape":                  _prog_dish(_PD + 31,  6),
    "Progressive Great Spider Crab Curry":                 _prog_dish(_PD + 32,  9),
    "Progressive Hawaiian Poke":                           _prog_dish(_PD + 33,  9),
    "Progressive Hot Pepper Tuna":                         _prog_dish(_PD + 34,  7),
    "Progressive Humboldt Ink Pasta":                      _prog_dish(_PD + 35, 10),
    "Progressive Humphead Parrotfish Curry":               _prog_dish(_PD + 36,  6),
    "Progressive Ice Fish Curry":                          _prog_dish(_PD + 37,  9),
    "Progressive Latok Omelet":                            _prog_dish(_PD + 38,  9),
    "Progressive Mackerel Scad Hotdog":                    _prog_dish(_PD + 39,  6),
    "Progressive Marlin and Soybean Paste Roast":          _prog_dish(_PD + 40,  9),
    "Progressive Mianbao Xia":                             _prog_dish(_PD + 41, 10),
    "Progressive Moray Eel Curry":                         _prog_dish(_PD + 42,  6),
    "Progressive Narrow-barred Spanish Mackerel Arancini": _prog_dish(_PD + 43,  7),
    "Progressive Narwhal Miso Soup":                       _prog_dish(_PD + 44, 12),
    "Progressive Nasu Dengaku":                            _prog_dish(_PD + 45,  4),
    "Progressive Peacock Squid Ripieni":                   _prog_dish(_PD + 46,  7),
    "Progressive Pelican Eel Jelly":                       _prog_dish(_PD + 47,  7),
    "Progressive Pickled Vegetables":                      _prog_dish(_PD + 48,  3),
    "Progressive Pikaia Ramen":                            _prog_dish(_PD + 49, 10),
    "Progressive Plotosid Pie":                            _prog_dish(_PD + 50,  7),
    "Progressive Rice with Great Spider Crab Meat":        _prog_dish(_PD + 51,  7),
    "Progressive Rice with Purple Sea Urchin Sushi":       _prog_dish(_PD + 52,  4),
    "Progressive Rice with White Shrimp Meat":             _prog_dish(_PD + 53,  9),
    "Progressive Roasted Capelin":                         _prog_dish(_PD + 54, 12),
    "Progressive Roasted Tropical Fish and Garlic":        _prog_dish(_PD + 55,  9),
    "Progressive Salt-grilled Redtoothed Triggerfish":     _prog_dish(_PD + 56,  6),
    "Progressive Seahorse Salad":                          _prog_dish(_PD + 57,  6),
    "Progressive Seahorse Udon":                           _prog_dish(_PD + 58,  4),
    "Progressive Seasoned Jellyfish":                      _prog_dish(_PD + 59,  6),
    "Progressive Seasoned Kajime":                         _prog_dish(_PD + 60,  6),
    "Progressive Seasoned Long-spine Porcupinefish Skin":  _prog_dish(_PD + 61,  7),
    "Progressive Seasoned Waptia Fieldensis":              _prog_dish(_PD + 62,  7),
    "Progressive Seaweed Rolled Omelet":                   _prog_dish(_PD + 63,  9),
    "Progressive Shark Karaage":                           _prog_dish(_PD + 64,  9),
    "Progressive Smallspotted Dart Kajime Soup":           _prog_dish(_PD + 65,  7),
    "Progressive Smoked Atlantic Mackerel Scramble":       _prog_dish(_PD + 66,  6),
    "Progressive Spear Squid Soba Futomaki":               _prog_dish(_PD + 67,  9),
    "Progressive Stellate Puffer Nicogori":                _prog_dish(_PD + 68,  7),
    "Progressive Stingray Sashimi Cold Noodles":           _prog_dish(_PD + 69,  9),
    "Progressive Stir-fried Habanero Lobster":             _prog_dish(_PD + 70,  7),
    "Progressive Striped Red Mullet Tangle Roll":          _prog_dish(_PD + 71,  7),
    "Progressive Sweet and Sour Stargazer":                _prog_dish(_PD + 72,  6),
    "Progressive Three-Colored Squid Roast":               _prog_dish(_PD + 73, 12),
    "Progressive Tomato Egg Soup":                         _prog_dish(_PD + 74, 12),
    "Progressive Trevally Nanbanzuke":                     _prog_dish(_PD + 75,  7),
    "Progressive Trevally Sandwich":                       _prog_dish(_PD + 76,  7),
    "Progressive Tropical Fish Sushi Set":                 _prog_dish(_PD + 77,  9),
    "Progressive Trout Sea Grapes Ricebowl":               _prog_dish(_PD + 78,  7),
    "Progressive White Trevally Kombu Ochazuke":           _prog_dish(_PD + 79,  7),
    "Progressive Whole-Roasted Shark Head":                _prog_dish(_PD + 80,  7),
    "Progressive Wrasse Curry":                            _prog_dish(_PD + 81,  6),
    "Progressive Yellowfin Tuna Steak":                    _prog_dish(_PD + 82,  9),
}

# === COOKSTA RANK ITEMS ===
# Progressive Cooksta Rank: 5 copies (Coal→Bronze→Silver→Gold→Platinum→Diamond)
# Each copy received = one rank up in Cooksta, unlocking new features.
# Starting rank is Coal (no item needed), so 5 progressive items = 5 rank-ups.
cooksta_rank_items: Dict[str, ItemData] = {
    "Progressive Cooksta Rank": ItemData(ITEM_BASE + 535, ItemClassification.useful, count=5, category="cooksta"),
}

# === CHARMS ===
# 12 charms total — each grants a passive bonus effect when equipped.
# Obtained from story missions or Ecowatcher level-ups.
charm_items: Dict[str, ItemData] = {
    "Dolphin Necklace":         ItemData(ITEM_BASE + 500, ItemClassification.useful),  # +30% dash speed
    "Octopus Bracelet":         ItemData(ITEM_BASE + 501, ItemClassification.useful),  # Short dash ability
    "Sea People Bracelet":      ItemData(ITEM_BASE + 502, ItemClassification.useful),  # Survive ~10s after oxygen out
    "Octopus Weapon Charm":     ItemData(ITEM_BASE + 503, ItemClassification.useful),  # +15% gun damage
    "Sea People Necklace":      ItemData(ITEM_BASE + 504, ItemClassification.useful),  # Travel through tubeworm tunnels
    "Shark Teeth Necklace":     ItemData(ITEM_BASE + 505, ItemClassification.useful),  # +15% harpoon damage
    "Eco Poison Resist Bracelet": ItemData(ITEM_BASE + 506, ItemClassification.useful), # Poison resistance
    "Eco Health Bracelet":      ItemData(ITEM_BASE + 507, ItemClassification.useful),  # -10% damage taken
    "Eco Gemstone Bracelet":    ItemData(ITEM_BASE + 508, ItemClassification.useful),  # +1 mineral from mining
    "Eco Waterproof Bag":       ItemData(ITEM_BASE + 509, ItemClassification.useful),  # +30kg weight limit
    "Leo Keychain":             ItemData(ITEM_BASE + 510, ItemClassification.useful, category="dlc_dredge"),  # DREDGE DLC
    "Jimbo Coin":               ItemData(ITEM_BASE + 511, ItemClassification.useful),  # Base game — Jimbo's Game Craze! mission
}

# === ABILITIES & UPGRADES ===
ability_items: Dict[str, ItemData] = {
    "Enhanced Vision":          ItemData(ITEM_BASE + 520, ItemClassification.useful),
    "Swimming Speed +1":        ItemData(ITEM_BASE + 521, ItemClassification.useful, count=3),
    # TODO: Add more abilities
}

# === FILLER ITEMS ===
# Ingredients are filler — receiving one means a supply drops in your inventory.
# Quantities reflect rarity: common farm items get large stacks, rare items get small stacks.
filler_items: Dict[str, ItemData] = {
    # Currency
    "Gold (Small)": ItemData(ITEM_BASE + 600, ItemClassification.filler),
    "Gold (Medium)": ItemData(ITEM_BASE + 601, ItemClassification.filler),
    "Gold (Large)": ItemData(ITEM_BASE + 602, ItemClassification.filler),
    "Bei (Small)": ItemData(ITEM_BASE + 610, ItemClassification.filler),
    "Bei (Medium)": ItemData(ITEM_BASE + 611, ItemClassification.filler),

    # Sea plants — common (×10 per item received)
    "Agar x10":               ItemData(ITEM_BASE + 620, ItemClassification.filler),
    "Kajime x10":             ItemData(ITEM_BASE + 621, ItemClassification.filler),
    "Seaweed x10":            ItemData(ITEM_BASE + 622, ItemClassification.filler),
    "Kelp x10":               ItemData(ITEM_BASE + 623, ItemClassification.filler),

    # Sea plants — uncommon (×5 per item received)
    "Sea Grape x5":           ItemData(ITEM_BASE + 624, ItemClassification.filler),
    "Southern Bull Kelp x5":  ItemData(ITEM_BASE + 625, ItemClassification.filler),
    "Black Coral x5":         ItemData(ITEM_BASE + 626, ItemClassification.filler),

    # Sea plants — rare (×2 per item received)
    "Bladderwrack x2":        ItemData(ITEM_BASE + 627, ItemClassification.filler),
    "Hyalonema x2":           ItemData(ITEM_BASE + 628, ItemClassification.filler),
    "Buckbean x2":            ItemData(ITEM_BASE + 629, ItemClassification.filler),

    # Rare forageables (×1 per item received)
    "Truffle x1":             ItemData(ITEM_BASE + 630, ItemClassification.filler),
    "Rainbow Cap x1":         ItemData(ITEM_BASE + 631, ItemClassification.filler),

    # Farm ingredients — common (×10 per item received)
    "Rice x10":               ItemData(ITEM_BASE + 640, ItemClassification.filler),
    "Wheat x10":              ItemData(ITEM_BASE + 641, ItemClassification.filler),
    "Egg x10":                ItemData(ITEM_BASE + 642, ItemClassification.filler),
    "Cucumber x10":           ItemData(ITEM_BASE + 643, ItemClassification.filler),

    # Farm ingredients — uncommon (×5 per item received)
    "Bean x5":                ItemData(ITEM_BASE + 644, ItemClassification.filler),
    "Buckwheat x5":           ItemData(ITEM_BASE + 645, ItemClassification.filler, category="dlc_ichiban"),
    "Carrot x5":              ItemData(ITEM_BASE + 646, ItemClassification.filler),
    "Cherry Tomato x5":       ItemData(ITEM_BASE + 647, ItemClassification.filler),
    "Eggplant x5":            ItemData(ITEM_BASE + 648, ItemClassification.filler),
    "Garlic x5":              ItemData(ITEM_BASE + 649, ItemClassification.filler),
    "Onion x5":               ItemData(ITEM_BASE + 650, ItemClassification.filler),

    # Farm ingredients — rare/spicy (×2 per item received)
    "Habanero x2":            ItemData(ITEM_BASE + 651, ItemClassification.filler),
    "Grade A Egg x2":         ItemData(ITEM_BASE + 652, ItemClassification.filler),

    # === JUNGLE DLC FILLER ITEMS ===
    # Jungle herbs/spices
    "Thai Chili x5":          ItemData(ITEM_BASE + 660, ItemClassification.filler, category="dlc_jungle"),
    "Palm Sugar x5":          ItemData(ITEM_BASE + 661, ItemClassification.filler, category="dlc_jungle"),
    "Calamansi x5":           ItemData(ITEM_BASE + 662, ItemClassification.filler, category="dlc_jungle"),
    "Lemongrass x5":          ItemData(ITEM_BASE + 663, ItemClassification.filler, category="dlc_jungle"),
    # Jungle fruits (farmed)
    "Pineapple x3":           ItemData(ITEM_BASE + 664, ItemClassification.filler, category="dlc_jungle"),
    "Watermelon x3":          ItemData(ITEM_BASE + 665, ItemClassification.filler, category="dlc_jungle"),
    "Honeydew x3":            ItemData(ITEM_BASE + 666, ItemClassification.filler, category="dlc_jungle"),
    "Dragon Fruit x3":        ItemData(ITEM_BASE + 667, ItemClassification.filler, category="dlc_jungle"),
    "Banana x5":              ItemData(ITEM_BASE + 668, ItemClassification.filler, category="dlc_jungle"),
    # Jungle rare ingredients
    "Sunang Stone x1":        ItemData(ITEM_BASE + 669, ItemClassification.filler, category="dlc_jungle"),
}

# === JUNGLE DLC ITEMS ===
# Progression items for the Jungle DLC (dlc_jungle)

jungle_progression_items: Dict[str, ItemData] = {
    # --- Purification Filter (progressive, 3 tiers) ---
    # Allows diving deeper into Utara Lake hazard zones
    # Tier 1: Crude (basic), Tier 2: Improved (to 55m), Tier 3: Advanced (to 75m + Lakebed Sea)
    # IDs start at ITEM_BASE + 800 to avoid all existing item ranges (max existing is ~700)
    "Progressive Purification Filter": ItemData(ITEM_BASE + 800, ItemClassification.progression, count=3, category="dlc_jungle"),

    # --- Jungle story key items ---
    "Jungle Chapter 1 Complete":  ItemData(ITEM_BASE + 801, ItemClassification.progression, category="dlc_jungle"),
    "Jungle Chapter 2 Complete":  ItemData(ITEM_BASE + 802, ItemClassification.progression, category="dlc_jungle"),
    "Jungle Chapter 3 Complete":  ItemData(ITEM_BASE + 803, ItemClassification.progression, category="dlc_jungle"),
    "Jungle Chapter 4 Complete":  ItemData(ITEM_BASE + 804, ItemClassification.progression, category="dlc_jungle"),
    "Jungle Chapter 5 Complete":  ItemData(ITEM_BASE + 805, ItemClassification.progression, category="dlc_jungle"),
    "Jungle Chapter 6 Complete":  ItemData(ITEM_BASE + 806, ItemClassification.progression, category="dlc_jungle"),
    "Jungle Chapter 7 Complete":  ItemData(ITEM_BASE + 807, ItemClassification.progression, category="dlc_jungle"),

    # --- Access/tool items ---
    "Machete":                    ItemData(ITEM_BASE + 810, ItemClassification.progression, category="dlc_jungle"),  # Opens vine-blocked areas
    "Bug Net":                    ItemData(ITEM_BASE + 811, ItemClassification.progression, category="dlc_jungle"),  # Required for insect catching
    "Fishing Rod":                ItemData(ITEM_BASE + 812, ItemClassification.progression, category="dlc_jungle"),  # Land fishing + night fish
    "Pickaxe":                    ItemData(ITEM_BASE + 813, ItemClassification.useful,      category="dlc_jungle"),  # Mining ore
    "Axe":                        ItemData(ITEM_BASE + 814, ItemClassification.useful,      category="dlc_jungle"),  # Chopping trees for ingredients
    "Laser Emitter":              ItemData(ITEM_BASE + 815, ItemClassification.useful,      category="dlc_jungle"),  # Collecting Sunang Stones
    "Ancient Breathing Apparatus":ItemData(ITEM_BASE + 816, ItemClassification.progression, category="dlc_jungle"),  # Deep lake diving (Ch2)

    # --- Jungle Gun (4 weapon modes, each progressive up to level 6) ---
    # Each mode starts at level 1 and has 5 upgrades → 6 levels total.
    # Picking a branch locks out the other branch, so each mode is fully Progressive.
    # Jungle Rifle is progression (needed for combat), others are useful.
    "Progressive Jungle Rifle":   ItemData(ITEM_BASE + 820, ItemClassification.progression, count=6, category="dlc_jungle"),
    "Progressive Jungle Shotgun": ItemData(ITEM_BASE + 821, ItemClassification.useful,      count=6, category="dlc_jungle"),
    "Progressive Jungle Sniper":  ItemData(ITEM_BASE + 822, ItemClassification.useful,      count=6, category="dlc_jungle"),
    "Progressive Jungle Net Gun": ItemData(ITEM_BASE + 823, ItemClassification.progression, count=6, category="dlc_jungle"),  # Needed for live captures

    # --- Jungle villager friendship milestones (items received from NPC rewards) ---
    # 3-heart rewards from key NPCs that gate content
    # --- Jungle Bancho Grill staff (9 members, all unlocked via quests) ---
    # These are progression items — each staff member expands what Bancho Grill can do.
    "Yasuto":            ItemData(ITEM_BASE + 840, ItemClassification.progression, category="dlc_jungle"),
    "Martin Tweed":      ItemData(ITEM_BASE + 841, ItemClassification.progression, category="dlc_jungle"),
    "Rover":             ItemData(ITEM_BASE + 842, ItemClassification.progression, category="dlc_jungle"),
    "Om Nom":            ItemData(ITEM_BASE + 843, ItemClassification.progression, category="dlc_jungle"),
    "Charlie Bonnet III":ItemData(ITEM_BASE + 844, ItemClassification.progression, category="dlc_jungle"),
    "William Longbottom":ItemData(ITEM_BASE + 845, ItemClassification.progression, category="dlc_jungle"),
    "Mita":              ItemData(ITEM_BASE + 846, ItemClassification.progression, category="dlc_jungle"),
    "Udo":               ItemData(ITEM_BASE + 847, ItemClassification.progression, category="dlc_jungle"),
    "Sato":              ItemData(ITEM_BASE + 848, ItemClassification.progression, category="dlc_jungle"),

    "Villager Trust":             ItemData(ITEM_BASE + 830, ItemClassification.progression, count=3, category="dlc_jungle"),  # 3 needed to enter temple
}

# === TRAP ITEMS (Optional) ===
trap_items: Dict[str, ItemData] = {
    # "Broken Equipment": ItemData(ITEM_BASE + 700, ItemClassification.trap, category="trap"),
    # TODO: Add trap items if desired
}

# Combine all items
item_table: Dict[str, ItemData] = {
    **progressive_equipment,
    **weapon_items,
    **area_unlock_items,
    **diving_equipment,
    **restaurant_items,
    **story_key_items,
    **cooksta_rank_items,
    **charm_items,
    **ability_items,
    **dish_upgrade_items,
    **filler_items,
    **trap_items,
    **jungle_progression_items,
}

# Create lookup dictionaries
item_name_to_id: Dict[str, int] = {
    name: data.code for name, data in item_table.items() if data.code is not None
}

# TODO: This needs to be expanded significantly with actual game analysis
# Current count: ~50 items, target: 150-300 items
