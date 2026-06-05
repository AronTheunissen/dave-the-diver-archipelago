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
    "Complete Duff's First Request": LocationData(BASE_ID + 380, "Blue Hole"),
    "Complete Dr. Bacon's Quest": LocationData(BASE_ID + 381, "Blue Hole"),
    "Complete Sea People Quest 1": LocationData(BASE_ID + 382, "Sea People Village"),
    "Complete Sea People Quest 2": LocationData(BASE_ID + 383, "Sea People Village"),
    "Complete Cobra's Quest": LocationData(BASE_ID + 384, "Blue Hole"),
    "Complete Niamo's Quest": LocationData(BASE_ID + 385, "Sea People Village"),
    
    # IMPORTANT: This check grants Teleport Mirror (base item for teleport system)
    "Sea People Village: Obtain Teleport Mirror": LocationData(BASE_ID + 386, "Sea People Village"),
    
    # TODO: Add all side quests
}

# === TELEPORT POINTS ===
# Activating these unlocks teleport destinations (requires visiting the area first)
teleport_locations: Dict[str, LocationData] = {
    # Unlock glacier teleport (allows bypassing Sea People Village!)
    "Glacier: Activate Glacier Teleport Point": LocationData(BASE_ID + 750, "Glacier"),
    
    # Unlock village teleport (alternative route to village)
    "Sea People Village: Activate Village Teleport Point": LocationData(BASE_ID + 751, "Sea People Village"),
    
    # Unlock deep blue hole teleport (useful for backtracking)
    "Deep Blue Hole: Activate Deep Teleport Point": LocationData(BASE_ID + 752, "Blue Hole - Deep"),
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

# === COOKSTA (Social Media App) ===
# Cooksta posts and follower milestones
cooksta_locations: Dict[str, LocationData] = {
    # Follower milestones
    "Cooksta: 100 Followers": LocationData(BASE_ID + 400, "Bancho Sushi"),
    "Cooksta: 500 Followers": LocationData(BASE_ID + 401, "Bancho Sushi"),
    "Cooksta: 1000 Followers": LocationData(BASE_ID + 402, "Bancho Sushi"),
    "Cooksta: 2500 Followers": LocationData(BASE_ID + 403, "Bancho Sushi"),
    "Cooksta: 5000 Followers": LocationData(BASE_ID + 404, "Bancho Sushi"),
    "Cooksta: 10000 Followers": LocationData(BASE_ID + 405, "Bancho Sushi"),
    
    # Special Cooksta achievements
    "Cooksta: First Viral Post": LocationData(BASE_ID + 410, "Bancho Sushi"),
    "Cooksta: Post 10 Times": LocationData(BASE_ID + 411, "Bancho Sushi"),
    "Cooksta: Post 25 Times": LocationData(BASE_ID + 412, "Bancho Sushi"),
    "Cooksta: Post 50 Times": LocationData(BASE_ID + 413, "Bancho Sushi"),
    "Cooksta: Max Likes on a Post": LocationData(BASE_ID + 414, "Bancho Sushi"),
    
    # TODO: Add more Cooksta milestones
}

# === ECOWATCHER (Marine Life App) ===
# Ecowatcher entries and research completion
ecowatcher_locations: Dict[str, LocationData] = {
    # Research completion by category
    "Ecowatcher: Complete All Shallow Fish": LocationData(BASE_ID + 450, "Blue Hole - Shallow"),
    "Ecowatcher: Complete All Mid Fish": LocationData(BASE_ID + 451, "Blue Hole - Mid"),
    "Ecowatcher: Complete All Deep Fish": LocationData(BASE_ID + 452, "Blue Hole - Deep"),
    "Ecowatcher: Complete All Glacier Fish": LocationData(BASE_ID + 453, "Glacier"),
    
    # Marinca entries (marine life)
    "Ecowatcher: Log 25 Marinca": LocationData(BASE_ID + 460, "Blue Hole"),
    "Ecowatcher: Log 50 Marinca": LocationData(BASE_ID + 461, "Blue Hole"),
    "Ecowatcher: Log 100 Marinca": LocationData(BASE_ID + 462, "Blue Hole"),
    "Ecowatcher: Complete All Marinca": LocationData(BASE_ID + 463, "Blue Hole"),
    
    # Fish entries
    "Ecowatcher: Log 50 Fish Species": LocationData(BASE_ID + 470, "Blue Hole"),
    "Ecowatcher: Log 100 Fish Species": LocationData(BASE_ID + 471, "Blue Hole"),
    "Ecowatcher: Log 150 Fish Species": LocationData(BASE_ID + 472, "Blue Hole"),
    "Ecowatcher: Complete All Fish": LocationData(BASE_ID + 473, "Blue Hole"),
    
    # TODO: Add specific Marinca entries if desired
}

# === PHOTOGRAPHY / PICTURES ===
# Tako's photography missions and special photo spots
photography_locations: Dict[str, LocationData] = {
    # Tako's photography missions
    "Photography: Complete Mission 1": LocationData(BASE_ID + 500, "Blue Hole"),
    "Photography: Complete Mission 2": LocationData(BASE_ID + 501, "Blue Hole"),
    "Photography: Complete Mission 3": LocationData(BASE_ID + 502, "Blue Hole"),
    "Photography: Complete Mission 4": LocationData(BASE_ID + 503, "Blue Hole"),
    "Photography: Complete Mission 5": LocationData(BASE_ID + 504, "Blue Hole"),
    
    # Special photo spots
    "Photo: Giant Squid": LocationData(BASE_ID + 510, "Blue Hole - Deep"),
    "Photo: Whale Shark": LocationData(BASE_ID + 511, "Blue Hole - Deep"),
    "Photo: Sea People Elder": LocationData(BASE_ID + 512, "Sea People Village"),
    "Photo: Glacier Scenery": LocationData(BASE_ID + 513, "Glacier"),
    
    # Photography milestones
    "Photography: Take 50 Photos": LocationData(BASE_ID + 520, "Blue Hole"),
    "Photography: Take 100 Photos": LocationData(BASE_ID + 521, "Blue Hole"),
    "Photography: Perfect Score on 10 Missions": LocationData(BASE_ID + 522, "Blue Hole"),
    
    # TODO: Add all Tako photography missions
}

# === CHALLENGES ===
# In-game challenges and special objectives
challenge_locations: Dict[str, LocationData] = {
    # Time attack challenges
    "Challenge: Catch 5 Fish in 60 Seconds": LocationData(BASE_ID + 550, "Blue Hole"),
    "Challenge: Earn 1000g in One Dive": LocationData(BASE_ID + 551, "Blue Hole"),
    "Challenge: Defeat 3 Sharks Without Taking Damage": LocationData(BASE_ID + 552, "Blue Hole - Deep"),
    
    # Weapon challenges
    "Challenge: Kill 10 Fish with Harpoon Only": LocationData(BASE_ID + 560, "Blue Hole"),
    "Challenge: Kill 10 Fish with Melee Only": LocationData(BASE_ID + 561, "Blue Hole"),
    "Challenge: Net Gun 20 Fish Alive": LocationData(BASE_ID + 562, "Blue Hole"),
    
    # Special challenges
    "Challenge: Serve 10 Customers with Perfect Timing": LocationData(BASE_ID + 570, "Bancho Sushi"),
    "Challenge: Complete a Dive Without Using Oxygen Refills": LocationData(BASE_ID + 571, "Blue Hole"),
    "Challenge: Reach Max Depth Without Equipment Damage": LocationData(BASE_ID + 572, "Blue Hole - Deep"),
    
    # TODO: Add all in-game challenges
}

# === FARMING (VEG GARDEN) ===
# Vegetable garden farming milestones
farming_locations: Dict[str, LocationData] = {
    # Garden unlocks and upgrades
    "Farming: Unlock Vegetable Garden": LocationData(BASE_ID + 600, "Bancho Sushi"),
    "Farming: Upgrade Garden Tier 1": LocationData(BASE_ID + 601, "Bancho Sushi"),
    "Farming: Upgrade Garden Tier 2": LocationData(BASE_ID + 602, "Bancho Sushi"),
    "Farming: Upgrade Garden Tier 3": LocationData(BASE_ID + 603, "Bancho Sushi"),
    
    # Crop unlocks (first harvest of each crop)
    "Farming: First Harvest - Tomato": LocationData(BASE_ID + 610, "Bancho Sushi"),
    "Farming: First Harvest - Lettuce": LocationData(BASE_ID + 611, "Bancho Sushi"),
    "Farming: First Harvest - Cucumber": LocationData(BASE_ID + 612, "Bancho Sushi"),
    "Farming: First Harvest - Onion": LocationData(BASE_ID + 613, "Bancho Sushi"),
    "Farming: First Harvest - Wasabi": LocationData(BASE_ID + 614, "Bancho Sushi"),
    "Farming: First Harvest - Ginger": LocationData(BASE_ID + 615, "Bancho Sushi"),
    "Farming: First Harvest - Seaweed": LocationData(BASE_ID + 616, "Bancho Sushi"),
    
    # Farming milestones
    "Farming: Harvest 50 Total Crops": LocationData(BASE_ID + 620, "Bancho Sushi"),
    "Farming: Harvest 100 Total Crops": LocationData(BASE_ID + 621, "Bancho Sushi"),
    "Farming: Harvest 250 Total Crops": LocationData(BASE_ID + 622, "Bancho Sushi"),
    "Farming: Grow All Crop Types": LocationData(BASE_ID + 623, "Bancho Sushi"),
    
    # TODO: Add all crop types and farming achievements
}

# === FISH FARM ===
# Fish farm management and breeding
fish_farm_locations: Dict[str, LocationData] = {
    # Fish farm unlocks
    "Fish Farm: Unlock Fish Farm": LocationData(BASE_ID + 650, "Fish Farm"),
    "Fish Farm: Upgrade Tank 1": LocationData(BASE_ID + 651, "Fish Farm"),
    "Fish Farm: Upgrade Tank 2": LocationData(BASE_ID + 652, "Fish Farm"),
    "Fish Farm: Upgrade Tank 3": LocationData(BASE_ID + 653, "Fish Farm"),
    
    # Breed/raise specific fish types
    "Fish Farm: First Breed - Tuna": LocationData(BASE_ID + 660, "Fish Farm"),
    "Fish Farm: First Breed - Salmon": LocationData(BASE_ID + 661, "Fish Farm"),
    "Fish Farm: First Breed - Squid": LocationData(BASE_ID + 662, "Fish Farm"),
    "Fish Farm: First Breed - Octopus": LocationData(BASE_ID + 663, "Fish Farm"),
    "Fish Farm: First Breed - Rare Species": LocationData(BASE_ID + 664, "Fish Farm"),
    
    # Fish farm milestones
    "Fish Farm: Raise 10 Fish to Adulthood": LocationData(BASE_ID + 670, "Fish Farm"),
    "Fish Farm: Raise 25 Fish to Adulthood": LocationData(BASE_ID + 671, "Fish Farm"),
    "Fish Farm: Raise 50 Fish to Adulthood": LocationData(BASE_ID + 672, "Fish Farm"),
    "Fish Farm: Raise 5 Different Species": LocationData(BASE_ID + 673, "Fish Farm"),
    "Fish Farm: Raise 10 Different Species": LocationData(BASE_ID + 674, "Fish Farm"),
    "Fish Farm: Max Out Fish Quality": LocationData(BASE_ID + 675, "Fish Farm"),
    
    # TODO: Add all farmable fish species
}

# === ACHIEVEMENTS / MILESTONES ===
achievement_locations: Dict[str, LocationData] = {
    "Catch 50 Different Fish Species": LocationData(BASE_ID + 700, "Blue Hole"),
    "Catch 100 Different Fish Species": LocationData(BASE_ID + 701, "Blue Hole"),
    "Earn 10,000 Gold": LocationData(BASE_ID + 710, "Bancho Sushi"),
    "Earn 50,000 Gold": LocationData(BASE_ID + 711, "Bancho Sushi"),
    "Earn 100,000 Gold": LocationData(BASE_ID + 712, "Bancho Sushi"),
    "Max Upgrade All Equipment": LocationData(BASE_ID + 720, "Bancho Sushi"),
    
    # Gameplay milestones
    "Play for 10 Hours": LocationData(BASE_ID + 730, "Bancho Sushi"),
    "Play for 25 Hours": LocationData(BASE_ID + 731, "Bancho Sushi"),
    "Complete 50 Dives": LocationData(BASE_ID + 732, "Blue Hole"),
    "Complete 100 Dives": LocationData(BASE_ID + 733, "Blue Hole"),
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
    **teleport_locations,
    **cooksta_locations,
    **ecowatcher_locations,
    **photography_locations,
    **challenge_locations,
    **farming_locations,
    **fish_farm_locations,
    **collectible_locations,
    **minigame_locations,
    **achievement_locations,
}

# Create lookup dictionary
location_name_to_id: Dict[str, int] = {
    name: data.code for name, data in location_table.items() if data.code is not None
}

# TODO: This needs significant expansion with actual game analysis
# Current count: ~150 locations defined, target: 300-500+ locations
# With all systems (fish, dishes, Cooksta, farming, etc.) we can easily hit 750+ locations
# 
# Breakdown by category:
# - Fish first catch: 100+ potential
# - Dish upgrades: 400+ potential (if all dishes included)
# - Recipe unlocks: 100+ potential
# - Cooksta: 15 defined
# - Ecowatcher: 12 defined
# - Photography: 12 defined
# - Challenges: 10 defined
# - Farming: 15 defined
# - Fish Farm: 16 defined
# - Story/Bosses/Quests: 20+ potential
# - Minigames/Collectibles: 30+ potential
