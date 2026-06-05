"""
Dave the Diver - Region Definitions

This file defines all regions (areas) in the game and how they connect.
"""

from typing import Dict
from BaseClasses import Region, Entrance
from .locations import location_table


def create_regions(world):
    """Create all regions for Dave the Diver"""
    
    player = world.player
    multiworld = world.multiworld
    
    # Create all regions
    menu = Region("Menu", player, multiworld)
    
    bancho_sushi = Region("Bancho Sushi", player, multiworld)
    
    blue_hole_shallow = Region("Blue Hole - Shallow", player, multiworld)
    blue_hole_mid = Region("Blue Hole - Mid", player, multiworld)
    blue_hole_deep = Region("Blue Hole - Deep", player, multiworld)
    
    sea_people_village = Region("Sea People Village", player, multiworld)
    glacier = Region("Glacier", player, multiworld)
    fish_farm = Region("Fish Farm", player, multiworld)
    
    # Add locations to each region
    for location_name, location_data in location_table.items():
        region = get_region_by_name(location_data.region, locals())
        if region:
            region.locations.append(
                create_location(player, location_name, location_data.code, region)
            )
    
    # Create connections between regions
    # Menu -> Bancho Sushi (always accessible)
    menu.connect(bancho_sushi, "Start Game")
    
    # Bancho Sushi -> Blue Hole Shallow (always accessible)
    bancho_sushi.connect(blue_hole_shallow, "Dive to Shallow")
    
    # Blue Hole Shallow -> Mid (requires oxygen/suit upgrades)
    blue_hole_shallow.connect(blue_hole_mid, "Dive to Mid Depths")
    
    # Blue Hole Mid -> Deep (requires more upgrades)
    blue_hole_mid.connect(blue_hole_deep, "Dive to Deep")
    
    # Blue Hole Deep -> Sea People Village (ROUTE 1: requires gloves)
    blue_hole_deep.connect(sea_people_village, "Swim to Sea People Village")
    
    # Bancho Sushi -> Sea People Village (ROUTE 2: teleport)
    bancho_sushi.connect(sea_people_village, "Teleport to Sea People Village", "teleport")
    
    # Sea People Village -> Glacier (ROUTE 1: swim through village)
    sea_people_village.connect(glacier, "Swim to Glacier from Village")
    
    # Bancho Sushi -> Glacier (ROUTE 2: direct teleport - bypasses village!)
    bancho_sushi.connect(glacier, "Teleport to Glacier", "teleport")
    
    # Bancho Sushi -> Deep Blue Hole (teleport back for convenience)
    bancho_sushi.connect(blue_hole_deep, "Teleport to Deep Blue Hole", "teleport")
    
    # Fish Farm (accessible from surface)
    bancho_sushi.connect(fish_farm, "Visit Fish Farm")
    
    # Add all regions to multiworld
    multiworld.regions += [
        menu,
        bancho_sushi,
        blue_hole_shallow,
        blue_hole_mid,
        blue_hole_deep,
        sea_people_village,
        glacier,
        fish_farm,
    ]


def get_region_by_name(region_name: str, regions_dict: Dict) -> Region:
    """Helper to get region object by name"""
    # Convert "Blue Hole - Shallow" to "blue_hole_shallow"
    var_name = region_name.lower().replace(" - ", "_").replace(" ", "_")
    return regions_dict.get(var_name)


def create_location(player: int, name: str, code: int, region: Region):
    """Create a location object"""
    from .locations import DaveDiverLocation
    return DaveDiverLocation(player, name, code, region)


# Location class
from BaseClasses import Location

class DaveDiverLocation(Location):
    """A location in Dave the Diver"""
    game: str = "Dave the Diver"
