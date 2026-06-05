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
    count: int = 1  # How many of this item exist


# Item classifications:
# - progression: Required to complete the game
# - useful: Helpful but not required
# - filler: Common items to fill remaining locations
# - trap: Negative effects

# Base item ID - Archipelago uses ranges for each game
# We'll use 0x444400 (DvD in hex-ish) as our base
BASE_ID = 0x444400


# === WEAPONS & EQUIPMENT ===
weapon_items: Dict[str, ItemData] = {
    # Harpoon Guns
    "Basic Harpoon Gun": ItemData(BASE_ID + 0, ItemClassification.progression),
    "Enhanced Harpoon Gun": ItemData(BASE_ID + 1, ItemClassification.progression),
    "Advanced Harpoon Gun": ItemData(BASE_ID + 2, ItemClassification.progression),
    
    # Harpoon Tips
    "Steel Net Gun Tip": ItemData(BASE_ID + 10, ItemClassification.useful),
    "Tranquilizer Rifle Tip": ItemData(BASE_ID + 11, ItemClassification.useful),
    "Poison Harpoon Tip": ItemData(BASE_ID + 12, ItemClassification.useful),
    "Triple Axe Harpoon Tip": ItemData(BASE_ID + 13, ItemClassification.useful),
    "Explosive Harpoon Tip": ItemData(BASE_ID + 14, ItemClassification.useful),
    
    # Melee Weapons
    "Dive Knife": ItemData(BASE_ID + 20, ItemClassification.progression),
    "Upgraded Dive Knife": ItemData(BASE_ID + 21, ItemClassification.useful),
    # TODO: Add all melee weapons
}

# === AREA UNLOCK ITEMS (Specific Items) ===
# These are NOT progressive - you get them once and unlock specific areas
area_unlock_items: Dict[str, ItemData] = {
    # Glacier access (requires ALL of these)
    "Cold Protection Suit": ItemData(BASE_ID + 100, ItemClassification.progression),
    "Teleport Mirror": ItemData(BASE_ID + 102, ItemClassification.progression),  # From Sea People Village quest
    
    # Sea People Village access
    "Sea People Gloves": ItemData(BASE_ID + 105, ItemClassification.progression),
    "Mermaid Suit": ItemData(BASE_ID + 106, ItemClassification.progression),  # Alternative to gloves
    
    # Key items
    "VIP Card": ItemData(BASE_ID + 110, ItemClassification.progression),
}

# === DIVING EQUIPMENT (Non-Progressive) ===
diving_equipment: Dict[str, ItemData] = {
    # Oxygen efficiency (multiplies oxygen duration)
    "Oxygen Efficiency Upgrade": ItemData(BASE_ID + 120, ItemClassification.useful, count=2),
    
    # Durability
    "Diving Suit Durability +1": ItemData(BASE_ID + 125, ItemClassification.useful, count=3),
    
    # Tools
    "Fish Radar": ItemData(BASE_ID + 130, ItemClassification.useful),
    "Enhanced Night Vision": ItemData(BASE_ID + 131, ItemClassification.useful),
    "Crab Trap": ItemData(BASE_ID + 135, ItemClassification.useful),
    "Enhanced Crab Trap": ItemData(BASE_ID + 136, ItemClassification.useful),
    # TODO: Add more tools
}

# === RESTAURANT & RECIPES ===
restaurant_items: Dict[str, ItemData] = {
    # Staff
    "Hire Waiter": ItemData(BASE_ID + 200, ItemClassification.useful, count=3),
    "Hire Chef": ItemData(BASE_ID + 205, ItemClassification.useful, count=2),
    
    # Recipes (examples - there are MANY in the game)
    "Sushi Recipe: Tuna Nigiri": ItemData(BASE_ID + 250, ItemClassification.useful),
    "Sushi Recipe: Salmon Roll": ItemData(BASE_ID + 251, ItemClassification.useful),
    # TODO: Add all recipes
    
    # Restaurant Upgrades
    "Dining Area Expansion": ItemData(BASE_ID + 300, ItemClassification.useful, count=3),
    "Kitchen Upgrade": ItemData(BASE_ID + 310, ItemClassification.useful, count=3),
}

# === STORY KEY ITEMS ===
# Items granted by completing story chapters
story_key_items: Dict[str, ItemData] = {
    "Chapter 1 Complete": ItemData(BASE_ID + 400, ItemClassification.progression),
    "Chapter 2 Complete": ItemData(BASE_ID + 401, ItemClassification.progression),
    "Chapter 3 Complete": ItemData(BASE_ID + 402, ItemClassification.progression),
    "Chapter 4 Complete": ItemData(BASE_ID + 403, ItemClassification.progression),
    "Chapter 5 Complete": ItemData(BASE_ID + 404, ItemClassification.progression),
    "Chapter 6 Complete": ItemData(BASE_ID + 405, ItemClassification.progression),
}

# === ABILITIES & UPGRADES ===
ability_items: Dict[str, ItemData] = {
    "Fish Radar": ItemData(BASE_ID + 500, ItemClassification.useful),
    "Enhanced Vision": ItemData(BASE_ID + 501, ItemClassification.useful),
    "Swimming Speed +1": ItemData(BASE_ID + 510, ItemClassification.useful, count=3),
    # TODO: Add abilities
}

# === FILLER ITEMS ===
filler_items: Dict[str, ItemData] = {
    "Gold (Small)": ItemData(BASE_ID + 600, ItemClassification.filler),
    "Gold (Medium)": ItemData(BASE_ID + 601, ItemClassification.filler),
    "Gold (Large)": ItemData(BASE_ID + 602, ItemClassification.filler),
    "Bei (Small)": ItemData(BASE_ID + 610, ItemClassification.filler),
    "Bei (Medium)": ItemData(BASE_ID + 611, ItemClassification.filler),
    # TODO: Add crafting materials as filler
}

# === TRAP ITEMS (Optional) ===
trap_items: Dict[str, ItemData] = {
    # "Broken Equipment": ItemData(BASE_ID + 700, ItemClassification.trap),
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
    **ability_items,
    **filler_items,
    **trap_items,
}

# Create lookup dictionaries
item_name_to_id: Dict[str, int] = {
    name: data.code for name, data in item_table.items() if data.code is not None
}

# TODO: This needs to be expanded significantly with actual game analysis
# Current count: ~50 items, target: 150-300 items
