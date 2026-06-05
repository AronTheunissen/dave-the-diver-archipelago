"""
Dave the Diver - Location Definitions

This file defines all check locations in Dave the Diver.
Locations are places where the player can receive items.
"""

from typing import Dict, NamedTuple, Optional


class LocationData(NamedTuple):
    """Data for a location definition"""
    code: Optional[int]
    region: str  # Which region this location belongs to


# Base location ID
BASE_ID = 0x444400


# === STORY PROGRESSION ===
story_locations: Dict[str, LocationData] = {
    "Complete Chapter 1": LocationData(BASE_ID + 0, "Blue Hole - Shallow"),
    "Complete Chapter 2": LocationData(BASE_ID + 1, "Blue Hole - Mid"),
    "Complete Chapter 3": LocationData(BASE_ID + 2, "Blue Hole - Deep"),
    "Complete Chapter 4": LocationData(BASE_ID + 3, "Blue Hole - Deep"),
    "Complete Chapter 5": LocationData(BASE_ID + 4, "Glacier"),
    "Complete Chapter 6": LocationData(BASE_ID + 5, "Sea People Village"),
    # TODO: Add all story checkpoints
}

# === FISH CATCHING ===
# Catching each fish species for the FIRST TIME

# Common Fish (First Catch)
common_fish_locations: Dict[str, LocationData] = {
    # Shallow water fish (0-50m)
    "First Catch: Anchovy": LocationData(BASE_ID + 100, "Blue Hole - Shallow"),
    "First Catch: Sea Bream": LocationData(BASE_ID + 101, "Blue Hole - Shallow"),
    "First Catch: Striped Bass": LocationData(BASE_ID + 102, "Blue Hole - Shallow"),
    "First Catch: Flatfish": LocationData(BASE_ID + 103, "Blue Hole - Shallow"),
    "First Catch: Squid": LocationData(BASE_ID + 104, "Blue Hole - Shallow"),
    "First Catch: Octopus": LocationData(BASE_ID + 105, "Blue Hole - Shallow"),
    # TODO: Add all common fish species (probably 20-30 species)
}

# Rare Fish (First Catch) - Higher value fish
rare_fish_locations: Dict[str, LocationData] = {
    # Mid depth fish (50-100m)
    "First Catch: Tuna": LocationData(BASE_ID + 150, "Blue Hole - Mid"),
    "First Catch: Salmon": LocationData(BASE_ID + 151, "Blue Hole - Mid"),
    "First Catch: Manta Ray": LocationData(BASE_ID + 152, "Blue Hole - Mid"),
    "First Catch: Blue Marlin": LocationData(BASE_ID + 153, "Blue Hole - Mid"),
    "First Catch: Swordfish": LocationData(BASE_ID + 154, "Blue Hole - Mid"),
    
    # Deep water fish (100m+)
    "First Catch: Giant Squid": LocationData(BASE_ID + 160, "Blue Hole - Deep"),
    "First Catch: Great White Shark": LocationData(BASE_ID + 161, "Blue Hole - Deep"),
    "First Catch: Hammerhead Shark": LocationData(BASE_ID + 162, "Blue Hole - Deep"),
    "First Catch: Whale Shark": LocationData(BASE_ID + 163, "Blue Hole - Deep"),
    
    # Special location fish
    "First Catch: Ice Fish": LocationData(BASE_ID + 170, "Glacier"),
    "First Catch: Glacial Squid": LocationData(BASE_ID + 171, "Glacier"),
    "First Catch: Sea People Fish": LocationData(BASE_ID + 175, "Sea People Village"),
    
    # TODO: Add all rare/special fish species (probably 30-50 species)
}

# Boss/Legendary Fish (First Catch) - Major encounters
boss_fish_locations: Dict[str, LocationData] = {
    "First Catch: Volcanic Viper Moray": LocationData(BASE_ID + 190, "Volcanic Area"),
    "First Catch: Giant Humphead Fish": LocationData(BASE_ID + 191, "Blue Hole - Deep"),
    "First Catch: Klaus (Legendary)": LocationData(BASE_ID + 192, "Blue Hole - Deep"),
    # TODO: Add all boss/legendary fish encounters
}

# === RESTAURANT MILESTONES ===
restaurant_milestones: Dict[str, LocationData] = {
    # Customer count
    "Serve 10 Customers": LocationData(BASE_ID + 200, "Bancho Sushi"),
    "Serve 50 Customers": LocationData(BASE_ID + 201, "Bancho Sushi"),
    "Serve 100 Customers": LocationData(BASE_ID + 202, "Bancho Sushi"),
    "Serve 250 Customers": LocationData(BASE_ID + 203, "Bancho Sushi"),
    "Serve 500 Customers": LocationData(BASE_ID + 204, "Bancho Sushi"),
    
    # Restaurant rating
    "Restaurant Rating: 3 Stars": LocationData(BASE_ID + 220, "Bancho Sushi"),
    "Restaurant Rating: 4 Stars": LocationData(BASE_ID + 221, "Bancho Sushi"),
    "Restaurant Rating: 5 Stars": LocationData(BASE_ID + 222, "Bancho Sushi"),
}

# === DISH UPGRADES ===
# Leveling up sushi dishes (each dish can be upgraded multiple times)
dish_upgrade_locations: Dict[str, LocationData] = {
    # Nigiri dishes
    "Upgrade Tuna Nigiri to Level 2": LocationData(BASE_ID + 250, "Bancho Sushi"),
    "Upgrade Tuna Nigiri to Level 3": LocationData(BASE_ID + 251, "Bancho Sushi"),
    "Upgrade Tuna Nigiri to Level 4": LocationData(BASE_ID + 252, "Bancho Sushi"),
    "Upgrade Tuna Nigiri to Level 5": LocationData(BASE_ID + 253, "Bancho Sushi"),
    
    "Upgrade Salmon Nigiri to Level 2": LocationData(BASE_ID + 254, "Bancho Sushi"),
    "Upgrade Salmon Nigiri to Level 3": LocationData(BASE_ID + 255, "Bancho Sushi"),
    "Upgrade Salmon Nigiri to Level 4": LocationData(BASE_ID + 256, "Bancho Sushi"),
    "Upgrade Salmon Nigiri to Level 5": LocationData(BASE_ID + 257, "Bancho Sushi"),
    
    # Roll dishes
    "Upgrade California Roll to Level 2": LocationData(BASE_ID + 260, "Bancho Sushi"),
    "Upgrade California Roll to Level 3": LocationData(BASE_ID + 261, "Bancho Sushi"),
    "Upgrade California Roll to Level 4": LocationData(BASE_ID + 262, "Bancho Sushi"),
    "Upgrade California Roll to Level 5": LocationData(BASE_ID + 263, "Bancho Sushi"),
    
    # Special dishes
    "Upgrade Premium Sushi Set to Level 2": LocationData(BASE_ID + 270, "Bancho Sushi"),
    "Upgrade Premium Sushi Set to Level 3": LocationData(BASE_ID + 271, "Bancho Sushi"),
    "Upgrade Premium Sushi Set to Level 4": LocationData(BASE_ID + 272, "Bancho Sushi"),
    "Upgrade Premium Sushi Set to Level 5": LocationData(BASE_ID + 273, "Bancho Sushi"),
    
    # TODO: Add upgrades for ALL dishes in the game (100+ dishes x 4 upgrades each = 400+ locations!)
    # Note: You might want to only include upgrades for key dishes, or make this optional via YAML
}

# === RECIPE UNLOCKS ===
recipe_unlock_locations: Dict[str, LocationData] = {
    # Basic recipes (unlock by catching fish)
    "Unlock Recipe: Tuna Nigiri": LocationData(BASE_ID + 300, "Bancho Sushi"),
    "Unlock Recipe: Salmon Nigiri": LocationData(BASE_ID + 301, "Bancho Sushi"),
    "Unlock Recipe: Sea Bream Nigiri": LocationData(BASE_ID + 302, "Bancho Sushi"),
    
    # Advanced recipes (unlock through story/quests)
    "Unlock Recipe: Premium Sushi Set": LocationData(BASE_ID + 350, "Bancho Sushi"),
    "Unlock Recipe: Special Seafood Platter": LocationData(BASE_ID + 351, "Bancho Sushi"),
    
    # TODO: Add all recipe unlocks (100+ recipes in the game)
}

# === BOSS BATTLES ===
boss_locations: Dict[str, LocationData] = {
    "Defeat Giant Squid Boss": LocationData(BASE_ID + 300, "Blue Hole - Deep"),
    "Defeat Sea People Boss": LocationData(BASE_ID + 301, "Sea People Village"),
    "Defeat Glacier Boss": LocationData(BASE_ID + 302, "Glacier"),
    # TODO: Add all boss fights
}

# === QUEST COMPLETION ===
quest_locations: Dict[str, LocationData] = {
    "Complete Duff's First Request": LocationData(BASE_ID + 400, "Blue Hole"),
    "Complete Dr. Bacon's Quest": LocationData(BASE_ID + 401, "Blue Hole"),
    "Complete Sea People Quest 1": LocationData(BASE_ID + 410, "Sea People Village"),
    # TODO: Add all side quests
}

# === COLLECTIBLES & UPGRADES ===
collectible_locations: Dict[str, LocationData] = {
    "Find Treasure Chest 1": LocationData(BASE_ID + 500, "Blue Hole - Shallow"),
    "Find Treasure Chest 2": LocationData(BASE_ID + 501, "Blue Hole - Mid"),
    "Purchase Upgrade from Duff 1": LocationData(BASE_ID + 510, "Bancho Sushi"),
    "Purchase Upgrade from Duff 2": LocationData(BASE_ID + 511, "Bancho Sushi"),
    # TODO: Add all treasure chests, shop purchases, etc.
}

# === MINIGAMES ===
minigame_locations: Dict[str, LocationData] = {
    "Beat Seahorse Racing - Easy": LocationData(BASE_ID + 600, "Sea People Village"),
    "Beat Seahorse Racing - Medium": LocationData(BASE_ID + 601, "Sea People Village"),
    "Beat Seahorse Racing - Hard": LocationData(BASE_ID + 602, "Sea People Village"),
    "Complete All Card Mini-games": LocationData(BASE_ID + 610, "Bancho Sushi"),
    # TODO: Add other minigames
}

# === ACHIEVEMENTS / MILESTONES ===
achievement_locations: Dict[str, LocationData] = {
    "Catch 50 Different Fish Species": LocationData(BASE_ID + 700, "Blue Hole"),
    "Catch 100 Different Fish Species": LocationData(BASE_ID + 701, "Blue Hole"),
    "Earn 10,000 Gold": LocationData(BASE_ID + 710, "Bancho Sushi"),
    "Earn 50,000 Gold": LocationData(BASE_ID + 711, "Bancho Sushi"),
    "Max Upgrade All Equipment": LocationData(BASE_ID + 720, "Bancho Sushi"),
    # TODO: Add achievement-style milestones
}

# Combine all locations
location_table: Dict[str, LocationData] = {
    **story_locations,
    **common_fish_locations,
    **rare_fish_locations,
    **boss_fish_locations,
    **restaurant_milestones,
    **dish_upgrade_locations,
    **recipe_unlock_locations,
    **boss_locations,
    **quest_locations,
    **collectible_locations,
    **minigame_locations,
    **achievement_locations,
}

# Create lookup dictionary
location_name_to_id: Dict[str, int] = {
    name: data.code for name, data in location_table.items() if data.code is not None
}

# TODO: This needs significant expansion with actual game analysis
# Current count: ~40 locations, target: 150-300 locations
# Goal is to match the number of items roughly
