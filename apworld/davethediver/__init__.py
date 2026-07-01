"""
Dave the Diver Archipelago World Implementation

This module implements Archipelago support for Dave the Diver.
"""

from typing import Dict, Any, List
from BaseClasses import Region, Tutorial
from worlds.AutoWorld import World, WebWorld
from .items import DaveDiverItem, item_table, item_name_to_id, filler_items
from .locations import location_name_to_id
from .regions import create_regions
from .rules import set_rules
from .options import DaveDiverOptions, Goal


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
        
        # Starting items are added to the player's start inventory via push_precollected.
        # We do NOT remove them from the item pool — this ensures the full count of
        # progression items (e.g. Progressive Diving Suit x8) stays in the pool so
        # the player can always reach the maximum level regardless of starting level.
        # The client handles extra progressive copies gracefully (they become no-ops).
        for item_name in self.get_starting_items():
            self.multiworld.push_precollected(self.create_item(item_name))

        self.multiworld.itempool += item_pool

    def create_filler(self) -> "DaveDiverItem":
        """Create a random filler item. Called by Archipelago to pad the item pool."""
        filler_names = [name for name in filler_items if self.should_include_item(name)]
        if not filler_names:
            filler_names = ["Gold (Small)"]
        return self.create_item(self.random.choice(filler_names))
        
    def should_include_location(self, location_name: str, location_data) -> bool:
        """Check if a location should be included based on player options.

        Categories and their controlling options:
        - "fish"        → fish_checks (0=none, 1=rare_only, 2=all)
        - "dish_upgrade"→ dish_upgrades (0=none, 1=key_dishes, 2=popular, 3=all)
        - "recipe"      → recipe_checks (0=key_only, 1=all)
        - "restaurant"  → always included (milestones always make sense)
        - "cooksta"     → include_cooksta toggle
        - "ecowatcher"  → include_ecowatcher toggle
        - "photography" → include_photography toggle
        - "challenge"   → include_challenges toggle
        - "farming"     → include_farming toggle
        - "fish_farm"   → include_fish_farm toggle
        - "minigame"    → include_minigames toggle
        - ""            → always included (story, quests, bosses, teleports, collectibles)
        """
        category = location_data.category

        if category == "fish":
            fish_opt = self.options.fish_checks.value
            if fish_opt == 0:  # none
                return False
            if fish_opt == 1:  # rare_only — exclude common fish (shallow-only)
                # Rare fish are mid/deep/glacier/village; common fish are all shallow
                return location_data.region != "Blue Hole - Shallow"
            return True  # 2 = all

        if category == "dish_upgrade":
            return self.options.dish_upgrades.value > 0  # 0 = none

        if category == "recipe":
            return self.options.recipe_checks.value >= 0  # always include for now; "none" not yet an option

        if category == "cooksta":
            return bool(self.options.include_cooksta.value)

        # Staff filtering — depends on staff_training_depth option:
        # 0=none, 1=hire_only, 2=milestones (Lv5/10/15/20), 3=all_levels (Lv1-20)
        depth = self.options.staff_training_depth.value
        if location_name.startswith("Staff: Hire"):
            return depth >= 1
        if location_name.startswith("Staff: Train"):
            if depth == 0:
                return False
            if depth == 1:  # hire_only — no training
                return False
            # Extract level from name e.g. "Staff: Train Maki to Level 10" → 10
            try:
                lvl = int(location_name.rsplit("Level ", 1)[1])
            except (IndexError, ValueError):
                lvl = 0
            is_milestone = lvl in (5, 10, 15, 20)
            is_ichiban = category == "staff_all_levels_ichiban"
            if is_ichiban and not bool(self.options.has_ichiban_dlc.value):
                return False
            if depth == 2:  # milestones only
                return is_milestone
            if depth == 3:  # all levels
                return True

        # Ingredient checks
        if category == "ingredient":
            return bool(self.options.include_ingredient_checks.value)

        # Sub-missions
        if category == "sub_mission":
            return bool(self.options.include_sub_missions.value)

        if category == "ecowatcher":
            return bool(self.options.include_ecowatcher.value)

        if category == "photography":
            return bool(self.options.include_photography.value)

        if category == "challenge":
            return bool(self.options.include_challenges.value)

        if category == "farming":
            return bool(self.options.include_farming.value)

        if category == "chicken_farm":
            return bool(self.options.include_chicken_farm.value)

        if category == "fish_farm":
            return bool(self.options.include_fish_farm.value)

        if category == "minigame":
            return bool(self.options.include_minigames.value)

        if category == "weapon":
            return bool(self.options.include_weapon_shop.value)

        # --- DLC categories ---
        if category == "dlc_dredge":
            return bool(self.options.has_dredge_dlc.value)

        if category == "dlc_godzilla":
            return bool(self.options.has_godzilla_dlc.value)

        if category == "dlc_ichiban":
            return bool(self.options.has_ichiban_dlc.value)

        if category == "dlc_jungle":
            return bool(self.options.has_jungle_dlc.value)

        # "" and "restaurant" — always include
        return True

    def should_include_item(self, item_name: str) -> bool:
        """Check if item should be included based on options.
        
        Items are filtered out when:
        - Their category is disabled by the player's YAML options
        - Trap items are excluded when trap_frequency is 'none'
        - Recipe items are excluded when recipe_checks is disabled
        - Restaurant items are excluded when dish_upgrades is none AND
          recipe_checks is disabled (i.e. no restaurant content at all)
        
        Progression items (area unlocks, chapters, equipment) are ALWAYS included
        regardless of options, as they may be required for logic.
        """
        from BaseClasses import ItemClassification
        item_data = item_table.get(item_name)
        if item_data is None:
            return True

        category = item_data.category

        # Restaurant/staff items — filtered before the progression blanket rule.
        depth = self.options.staff_training_depth.value
        restaurant_on = bool(self.options.dish_upgrades.value > 0 or self.options.recipe_checks.value > 0)

        # Named staff (single copy) — used in hire_only and milestones modes
        if category == "restaurant" and item_name in (
            "Billy","Carolina","Charlie","Cohh","Davina","Drae","El Nino",
            "Itsuki","James","Jandi","Kyoko","Liu","Maki","Masayoshi","Mitchell",
            "Pai","Raptor","Raul","Tohoku","Yone","Yusuke"
        ):
            return restaurant_on and 1 <= depth <= 2

        # Progressive staff (×20) — used in all_levels mode only
        if category == "restaurant" and item_name.startswith("Progressive ") and item_name[12:] in (
            "Billy","Carolina","Charlie","Cohh","Davina","Drae","El Nino",
            "Itsuki","James","Jandi","Kyoko","Liu","Maki","Masayoshi","Mitchell",
            "Pai","Raptor","Raul","Tohoku","Yone","Yusuke"
        ):
            return restaurant_on and depth == 3

        # Other restaurant items (upgrades, etc.)
        if category == "restaurant":
            return restaurant_on

        # DLC items are filtered by DLC flag FIRST — even if they are progression.
        # A progression item for disabled DLC content should never be in the pool,
        # since its locations don't exist either (filtered by should_include_location).
        if category == "dlc_dredge":
            return bool(self.options.has_dredge_dlc.value)
        if category == "dlc_godzilla":
            return bool(self.options.has_godzilla_dlc.value)
        if category == "dlc_ichiban":
            return bool(self.options.has_ichiban_dlc.value)
        if category == "dlc_jungle":
            return bool(self.options.has_jungle_dlc.value)

        # Non-DLC progression items are always kept — removing them could break logic
        if item_data.classification == ItemClassification.progression:
            return True

        # --- Trap items ---
        if category == "trap":
            return self.options.trap_frequency.value > 0  # 0 = none

        # --- Recipe items ---
        # Exclude when recipe checks are fully disabled
        if category == "recipe":
            return self.options.recipe_checks.value > 0  # 0 = key_only still keeps them; only exclude if truly none
            # Note: recipe_checks has no "none" option currently, so all values include recipes.
            # When a "none" value is added this will automatically filter correctly.

        # --- Restaurant items (staff, upgrades) ---
        # Exclude only when BOTH dish upgrades AND recipe checks are off,
        # meaning the restaurant is essentially out of scope.
        if category == "restaurant":
            has_dish_content = self.options.dish_upgrades.value > 0    # 0 = none
            has_recipe_content = self.options.recipe_checks.value > 0  # 0 = key_only (no recipes as items)
            return has_dish_content or has_recipe_content

        # All other items (diving equipment, abilities, filler, story items) always included
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
        """Pad the item pool with filler to match location count."""
        filler_names = [name for name in filler_items if self.should_include_item(name)]
        if not filler_names:
            filler_names = ["Gold (Small)"]
        unfilled = self.multiworld.get_unfilled_locations(self.player)
        item_count = len([i for i in self.multiworld.itempool if i.player == self.player])
        for _ in range(len(unfilled) - item_count):
            self.multiworld.itempool.append(self.create_item(self.random.choice(filler_names)))
        
    def fill_slot_data(self) -> Dict[str, Any]:
        """Fill slot data to be sent to the client mod (SlotData.cs).
        
        Every key here must have a matching property in the C# SlotData class.
        Keys use snake_case to match the Python option names exactly.
        """
        return {
            # ── Victory condition ────────────────────────────────────────────
            "goal": self.options.goal.value,
            # 0=defeat_yawie, 1=defeat_all_bosses, 2=defeat_yawie_plus_cooksta,
            # 3=restaurant_tycoon, 4=master_diver, 5=complete_marinca_collection,
            # 6=hundred_percent

            # ── Fish checks ──────────────────────────────────────────────────
            "fish_checks": self.options.fish_checks.value,
            # 0=none, 1=rare_only, 2=all

            # ── Restaurant options ───────────────────────────────────────────
            "dish_upgrades": self.options.dish_upgrades.value,
            # 0=none, 1=key_dishes, 2=popular, 3=all
            "recipe_checks": self.options.recipe_checks.value,

            # ── Optional systems (0=off, 1=on) ───────────────────────────────
            "include_cooksta":      self.options.include_cooksta.value,
            "include_ecowatcher":   self.options.include_ecowatcher.value,
            "include_photography":  self.options.include_photography.value,
            "include_challenges":   self.options.include_challenges.value,
            "include_farming":      self.options.include_farming.value,
            "include_chicken_farm": self.options.include_chicken_farm.value,
            "include_fish_farm":    self.options.include_fish_farm.value,
            "include_minigames":    self.options.include_minigames.value,
            "include_weapon_shop":        self.options.include_weapon_shop.value,
            "staff_training_depth":       self.options.staff_training_depth.value,
            "include_ingredient_checks":  self.options.include_ingredient_checks.value,
            "include_sub_missions":       self.options.include_sub_missions.value,

            # ── DLC ownership (0=no, 1=yes) ──────────────────────────────────
            "has_dredge_dlc":   self.options.has_dredge_dlc.value,
            "has_godzilla_dlc": self.options.has_godzilla_dlc.value,
            "has_ichiban_dlc":  self.options.has_ichiban_dlc.value,
            "has_jungle_dlc":   self.options.has_jungle_dlc.value,

            # ── Starting equipment levels ─────────────────────────────────────
            "starting_oxygen_level":      self.options.starting_oxygen_level.value,
            "starting_harpoon_level":     self.options.starting_harpoon_level.value,
            "starting_suit_level":        self.options.starting_diving_suit_level.value,

            # ── Misc ──────────────────────────────────────────────────────────
            "death_link":      self.options.death_link.value,
            "trap_frequency":  self.options.trap_frequency.value,
        }
