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
# These represent catching specific rare/important fish
fish_locations: Dict[str, LocationData] = {
    "Catch Giant Squid": LocationData(BASE_ID + 100, "Blue Hole - Deep"),
    "Catch Great White Shark": LocationData(BASE_ID + 101, "Blue Hole - Deep"),
    "Catch Manta Ray": LocationData(BASE_ID + 102, "Blue Hole - Mid"),
    "Catch Blue Marlin": LocationData(BASE_ID + 103, "Blue Hole - Mid"),
    # TODO: Add important fish catches
    # Note: Not every fish needs to be a location, focus on rare/progression ones
}

# === RESTAURANT MILESTONES ===
restaurant_locations: Dict[str, LocationData] = {
    "Serve 10 Customers": LocationData(BASE_ID + 200, "Bancho Sushi"),
    "Serve 50 Customers": LocationData(BASE_ID + 201, "Bancho Sushi"),
    "Serve 100 Customers": LocationData(BASE_ID + 202, "Bancho Sushi"),
    "Unlock Recipe: Premium Sushi Set": LocationData(BASE_ID + 210, "Bancho Sushi"),
    "Restaurant Rating: 3 Stars": LocationData(BASE_ID + 220, "Bancho Sushi"),
    "Restaurant Rating: 4 Stars": LocationData(BASE_ID + 221, "Bancho Sushi"),
    "Restaurant Rating: 5 Stars": LocationData(BASE_ID + 222, "Bancho Sushi"),
    # TODO: Add recipe unlocks, special dishes, etc.
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
    **fish_locations,
    **restaurant_locations,
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
