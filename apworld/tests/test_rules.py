"""
Dave the Diver - Logic Rules Tests

Tests for option-based filtering, Jungle DLC toggling, goal conditions,
and key logic checks (depth access, night dive, boss gates, etc.).
"""
import unittest
import sys
import os

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from davethediver.locations import location_table, LocationData
from davethediver.items import item_table


def make_options(**overrides):
    """Create a mock options object with sensible defaults."""
    defaults = {
        "goal": 0,                      # Goal.defeat_yawie
        "chapters_required": 7,
        "fish_checks": 2,               # FishChecks.all
        "require_all_fish": 0,
        "dish_upgrades": 1,             # DishUpgrades.on
        "recipe_checks": 1,             # RecipeChecks.on
        "require_all_recipes": 0,
        "restaurant_rating_required": 3,
        "include_cooksta": 1,
        "cooksta_followers_required": 0,
        "include_ecowatcher": 1,
        "include_photography": 1,
        "include_challenges": 0,
        "include_farming": 1,
        "include_chicken_farm": 1,
        "include_fish_farm": 1,
        "include_minigames": 1,
        "include_weapon_shop": 1,
        "staff_training_depth": 0,      # milestones
        "include_ingredient_checks": 1,
        "include_sub_missions": 1,
        "has_dredge_dlc": 0,
        "has_godzilla_dlc": 0,
        "has_ichiban_dlc": 0,
        "has_jungle_dlc": 0,
        "starting_oxygen_level": 1,
        "starting_harpoon_level": 1,
        "starting_suit_level": 1,
        "oxygen_requirement": 0,
        "death_link": 0,
        "teleport_behavior": 0,
        "trap_item_frequency": 0,
    }
    defaults.update(overrides)

    class V:
        def __init__(self, v): self.value = v

    class Opts:
        pass

    opts = Opts()
    for k, v in defaults.items():
        setattr(opts, k, V(v))
    return opts


class TestJungleDLCFiltering(unittest.TestCase):
    """Test that Jungle DLC locations are properly toggled."""

    def _jungle_locs(self):
        return {
            name: data for name, data in location_table.items()
            if data.category in ("dlc_jungle",) or
               name.startswith("Jungle") or
               name.startswith("Insect:") or
               name.startswith("Insect Battle:") or
               name.startswith("Grill Recipe:") or
               "Jungle" in data.region
        }

    def test_jungle_locations_exist(self):
        """Jungle DLC should have a significant number of locations."""
        jungle_locs = self._jungle_locs()
        self.assertGreater(len(jungle_locs), 50,
            f"Expected 50+ jungle locations, got {len(jungle_locs)}")

    def test_jungle_locations_have_jungle_regions(self):
        """All jungle locations should be in jungle-specific regions."""
        jungle_regions = {
            "Utara Village", "Utara Lake - Upper", "Utara Lake - Lower",
            "Setah Forest", "Murau Temple", "Surga Falls", "Lakebed Sea",
            "Bancho Grill"
        }
        for name, data in location_table.items():
            if data.region in jungle_regions:
                self.assertIn(data.category, (
                    "dlc_jungle", "dlc_ichiban", "fish", "ingredient",
                    "weapon", "farming", "", "dish_upgrade", "recipe"
                ), f"Location '{name}' in jungle region has unexpected category '{data.category}'")

    def test_no_challenge_locations(self):
        """challenge_locations should have been removed — no locations with 'challenge' category."""
        challenge_locs = [
            name for name, data in location_table.items()
            if data.category == "challenge"
        ]
        self.assertEqual(challenge_locs, [],
            f"Found challenge category locations that should have been removed: {challenge_locs}")


class TestLocationCategories(unittest.TestCase):
    """Test that location categories are consistent and complete."""

    KNOWN_CATEGORIES = {
        "", "fish", "dish_upgrade", "recipe", "cooksta", "ecowatcher",
        "photography", "farming", "dlc_dredge", "dlc_godzilla",
        "dlc_ichiban", "dlc_jungle", "minigame", "weapon", "ingredient",
        "chicken_farm", "fish_farm", "staff_all_levels", "staff_all_levels_ichiban",
        "restaurant", "sub_mission",
    }

    def test_no_unknown_categories(self):
        """All location categories should be from the known set."""
        unknown = set()
        for name, data in location_table.items():
            if data.category not in self.KNOWN_CATEGORIES:
                unknown.add(data.category)
        self.assertEqual(unknown, set(),
            f"Unknown location categories found: {unknown}")

    def test_story_locations_have_no_category(self):
        """Story locations should have empty category (always included)."""
        story_locs = {name: data for name, data in location_table.items()
                      if name.startswith("Story:")}
        for name, data in story_locs.items():
            self.assertEqual(data.category, "",
                f"Story location '{name}' should have empty category, got '{data.category}'")

    def test_boss_locations_have_no_category(self):
        """Non-DLC boss defeat locations should have empty category (always included).
        DLC bosses (Ebirah=dlc_godzilla, Jungle bosses=dlc_jungle) are allowed DLC categories."""
        dlc_prefixes = ("dlc_",)
        boss_locs = {name: data for name, data in location_table.items()
                     if name.startswith("Defeat:") or name.startswith("Jungle Boss:")}
        for name, data in boss_locs.items():
            if not any(data.category.startswith(p) for p in dlc_prefixes):
                self.assertEqual(data.category, "",
                    f"Non-DLC boss location '{name}' should have empty category, got '{data.category}'")

    def test_fish_locations_have_fish_category(self):
        """All First Catch locations should have 'fish' or a DLC category."""
        fish_locs = {name: data for name, data in location_table.items()
                     if name.startswith("First Catch:") and "Jungle" not in data.region}
        valid_fish_categories = {"fish", "dlc_dredge", "dlc_godzilla", "dlc_jungle"}
        for name, data in fish_locs.items():
            self.assertIn(data.category, valid_fish_categories,
                f"Fish location '{name}' should have a fish/DLC category, got '{data.category}'")


class TestGoalOptions(unittest.TestCase):
    """Test that goal-related options and items exist correctly."""

    def test_defeat_yawie_location_exists(self):
        """The Defeat Yawie location must exist for the default goal."""
        self.assertIn("Defeat: Yawie (Final Boss)", location_table,
            "Defeat: Yawie (Final Boss) location is required for default goal")

    def test_yawie_in_correct_region(self):
        """Yawie (Final Boss) should be in the Sea People Village region
        (the boss fight triggers in the village, not the glacier)."""
        loc = location_table.get("Defeat: Yawie (Final Boss)")
        self.assertIsNotNone(loc)
        self.assertEqual(loc.region, "Sea People Village",
            f"Yawie should be in Sea People Village, got '{loc.region}'")

    def test_progressive_diving_suit_exists(self):
        """Progressive Diving Suit must exist for depth gating."""
        self.assertIn("Progressive Diving Suit", item_table)

    def test_progressive_oxygen_tank_exists(self):
        """Progressive Oxygen Tank must exist for depth gating."""
        self.assertIn("Progressive Oxygen Tank", item_table)

    def test_key_items_exist(self):
        """All key story items must exist in item_table."""
        key_items = [
            "Sea People Gloves",
            "Sea People Translator",
            "Key to Tenzhin",
            "Laser Device",
            "Sea People's Trust",
            "Teleport Mirror",
            "Control Room Button",
            "Tech Suit Parts",
            "Gas Cutter",
            "Headlamp",
            "Bug Net",
            "Night Dive Unlock",
            "Vortex Entry",
        ]
        for item in key_items:
            self.assertIn(item, item_table,
                f"Key item '{item}' is missing from item_table")

    def test_jungle_key_items_exist(self):
        """Jungle DLC progression items must exist."""
        jungle_items = [
            "Jungle Chapter 1 Complete",
            "Jungle Chapter 2 Complete",
            "Jungle Chapter 3 Complete",
            "Jungle Chapter 4 Complete",
            "Progressive Purification Filter",
            "Fishing Rod",
            "Machete",
        ]
        for item in jungle_items:
            self.assertIn(item, item_table,
                f"Jungle item '{item}' is missing from item_table")


class TestDepthAccessItems(unittest.TestCase):
    """Test that depth-gating items have correct counts."""

    def test_progressive_diving_suit_count(self):
        """Progressive Diving Suit should have count >= 8 (max level)."""
        suit = item_table.get("Progressive Diving Suit")
        self.assertIsNotNone(suit)
        self.assertGreaterEqual(suit.count, 8,
            f"Progressive Diving Suit should have count >= 8, got {suit.count}")

    def test_progressive_oxygen_tank_count(self):
        """Progressive Oxygen Tank should have count >= 6."""
        tank = item_table.get("Progressive Oxygen Tank")
        self.assertIsNotNone(tank)
        self.assertGreaterEqual(tank.count, 6,
            f"Progressive Oxygen Tank should have count >= 6, got {tank.count}")

    def test_progressive_harpoon_count(self):
        """Progressive Harpoon should have count >= 1."""
        harpoon = item_table.get("Progressive Harpoon")
        self.assertIsNotNone(harpoon)
        self.assertGreaterEqual(harpoon.count, 1,
            f"Progressive Harpoon should have count >= 1, got {harpoon.count}")


class TestRegionAssignments(unittest.TestCase):
    """Test that locations are assigned to correct regions."""

    def test_glacier_locations_in_glacier_region(self):
        """Glacier zone locations should be in Glacier Zone or Glacial Passage."""
        glacier_regions = {"Glacier Zone", "Glacial Passage", "Hydrothermal Vents"}
        glacier_locs = [name for name, data in location_table.items()
                        if "Glacier" in name or "Glacial" in name]
        for name in glacier_locs:
            data = location_table[name]
            if "Ecowatcher" in name:
                continue  # Ecowatcher missions have varied regions
            self.assertIn(data.region, glacier_regions,
                f"'{name}' should be in a glacier region, got '{data.region}'")

    def test_sea_people_locations_in_village(self):
        """Sea People Village locations should be in the correct region."""
        spv_locs = [name for name, data in location_table.items()
                    if data.region == "Sea People Village"]
        self.assertGreater(len(spv_locs), 5,
            "Expected more than 5 locations in Sea People Village region")

    def test_bancho_sushi_locations_exist(self):
        """Bancho Sushi region should have staff and restaurant locations."""
        bancho_locs = [name for name, data in location_table.items()
                       if data.region == "Bancho Sushi"]
        self.assertGreater(len(bancho_locs), 10,
            "Expected more than 10 locations in Bancho Sushi region")


class TestItemClassifications(unittest.TestCase):
    """Test that items have correct Archipelago classifications."""

    def test_progressive_items_are_progression(self):
        """Core progressive items should be classified as progression.
        Exception: Progressive Cooksta Rank is 'useful' by design (not required to complete the game)."""
        from BaseClasses import ItemClassification
        # These specific progressive items are required for game completion — must be progression
        required_progressive = [
            "Progressive Diving Suit",
            "Progressive Oxygen Tank",
            "Progressive Harpoon",
            "Progressive Purification Filter",  # Jungle DLC
        ]
        for name in required_progressive:
            if name in item_table:
                data = item_table[name]
                self.assertEqual(data.classification, ItemClassification.progression,
                    f"Core progressive item '{name}' should be 'progression', got {data.classification}")

    def test_key_items_are_progression(self):
        """Key story items should be classified as progression."""
        from BaseClasses import ItemClassification
        key_items = [
            "Sea People Gloves", "Sea People Translator", "Key to Tenzhin",
            "Laser Device", "Sea People's Trust", "Gas Cutter", "Bug Net",
        ]
        for name in key_items:
            if name in item_table:
                data = item_table[name]
                self.assertEqual(data.classification, ItemClassification.progression,
                    f"Key item '{name}' should be 'progression', got {data.classification}")

    def test_filler_items_are_filler(self):
        """Filler items (classification=filler) should exist in item_table."""
        from BaseClasses import ItemClassification
        filler_items = [name for name, data in item_table.items()
                        if data.classification == ItemClassification.filler]
        self.assertGreater(len(filler_items), 0, "Should have at least some filler items")

    def test_trap_items_are_trap(self):
        """Trap items should be classified as trap."""
        from BaseClasses import ItemClassification
        trap_items = [name for name, data in item_table.items()
                      if data.category == "trap"]
        for name in trap_items:
            data = item_table[name]
            self.assertEqual(data.classification, ItemClassification.trap,
                f"Trap item '{name}' should be 'trap', got {data.classification}")


if __name__ == "__main__":
    unittest.main()
