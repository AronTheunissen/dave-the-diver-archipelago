"""
Dave the Diver Archipelago World Implementation

This module implements Archipelago support for Dave the Diver.
"""

from typing import Dict, Any, List
from BaseClasses import Region, Tutorial
from worlds.AutoWorld import World, WebWorld
from .items import DaveDiverItem, item_table, item_name_to_id
from .locations import location_name_to_id
from .regions import create_regions
from .rules import set_rules
from .options import DaveDiverOptions


class DaveDiverWebWorld(WebWorld):
    """Web interface for Dave the Diver world"""
    
    theme = "ocean"
    
    tutorials = [
        Tutorial(
            "Multiworld Setup Guide",
            "A guide to setting up Dave the Diver for Archipelago",
            "English",
            "setup_en.md",
            "setup/en",
            ["Aron Theunissen"]
        )
    ]


class DaveDiverWorld(World):
    """
    Dave the Diver is a unique adventure game that combines undersea exploration,
    running a sushi restaurant, and various minigames. Dive into the mysterious
    Blue Hole to catch fish, battle sea creatures, and uncover ancient secrets,
    then serve up your catch at Bancho Sushi to keep customers happy!
    
    Supports multiple victory conditions from speedrun to 100% completion!
    """
    
    game = "Dave the Diver"
    web = DaveDiverWebWorld()
    options_dataclass = DaveDiverOptions
    options: DaveDiverOptions
    
    # Data package information
    item_name_to_id = item_name_to_id
    location_name_to_id = location_name_to_id
    
    def __init__(self, world, player: int):
        super().__init__(world, player)
        
    def create_regions(self):
        """Create regions (areas) for the game"""
        create_regions(self)
        
    def create_items(self):
        """Create and place items in the item pool"""
        # Start with all items from item_table
        item_pool = []
        
        for item_name, item_data in item_table.items():
            # Skip if filtered by options
            if not self.should_include_item(item_name):
                continue
                
            # Add multiple copies if needed
            for _ in range(item_data.count):
                item_pool.append(self.create_item(item_name))
        
        # Remove items that are in starting inventory
        # Based on options
        starting_items = self.get_starting_items()
        for item_name in starting_items:
            if item_name in [item.name for item in item_pool]:
                item_pool.remove(next(item for item in item_pool if item.name == item_name))
        
        self.multiworld.itempool += item_pool
        
    def should_include_item(self, item_name: str) -> bool:
        """Check if item should be included based on options"""
        # TODO: Filter based on options (e.g., skip fish if fish_checks = none)
        return True
        
    def get_starting_items(self) -> List[str]:
        """Get list of items player starts with"""
        starting = []
        
        # Progressive oxygen
        for _ in range(self.options.starting_oxygen_level.value):
            starting.append("Progressive Oxygen Tank")
        
        # Progressive harpoon
        for _ in range(self.options.starting_harpoon_level.value):
            starting.append("Progressive Harpoon")
        
        # Progressive suit
        for _ in range(self.options.starting_diving_suit_level.value):
            starting.append("Progressive Diving Suit")
        
        return starting
        
    def set_rules(self):
        """Set logic rules for accessing locations"""
        set_rules(self)
        
    def create_item(self, name: str) -> DaveDiverItem:
        """Create an item by name"""
        item_data = item_table[name]
        return DaveDiverItem(name, item_data.classification, item_data.code, self.player)
        
    def generate_basic(self):
        """Called after create_regions and before set_rules"""
        pass
        
    def fill_slot_data(self) -> Dict[str, Any]:
        """Fill slot data to be sent to the client"""
        return {
            "death_link": self.options.death_link.value,
            "goal": self.options.goal.value,
            "starting_oxygen": self.options.starting_oxygen_level.value,
            "starting_harpoon": self.options.starting_harpoon_level.value,
            "starting_suit": self.options.starting_diving_suit_level.value,
        }
