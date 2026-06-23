"""
Tests for should_include_item() and should_include_location() filtering logic.
Uses a lightweight mock of the Archipelago world to avoid needing a full
Archipelago install — only the options object is needed for filtering.
"""
import unittest
import sys
import os

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

from davethediver.items import item_table, ItemData
from davethediver.locations import location_table, LocationData
from davethediver.regions import REGION_NAMES


def make_options(**overrides):
    """
    Create a mock options object with all defaults set to 'on'/'all',
    then apply any overrides. Values are simple ints wrapped in a
    Value object to match the .value attribute access pattern.
    """
    class V:
        def __init__(self, v): self.value = v
        def __bool__(self): return bool(self.value)

    defaults = {
        "fish_checks":              V(2),   # all
        "dish_upgrades":            V(3),   # all
        "recipe_checks":            V(1),   # all
        "include_cooksta":          V(1),
        "include_ecowatcher":       V(1),
        "include_photography":      V(1),
        "include_challenges":       V(1),
        "include_farming":          V(1),
        "include_chicken_farm":     V(1),
        "include_fish_farm":        V(1),
        "include_minigames":        V(1),
        "include_weapon_shop":        V(1),
        "staff_training_depth":       V(2),  # milestones
        "include_ingredient_checks":  V(1),
        "include_sub_missions":       V(1),
        "has_dredge_dlc":             V(1),
        "has_godzilla_dlc":         V(1),
        "has_ichiban_dlc":          V(1),
        "has_jungle_dlc":           V(1),
        "trap_frequency":           V(2),   # common
        "goal":                     V(0),
        "starting_oxygen_level":    V(1),
        "starting_harpoon_level":   V(1),
        "starting_diving_suit_level": V(1),
        "death_link":               V(0),
    }
    for k, v in overrides.items():
        defaults[k] = V(v)

    class Opts:
        pass
    opts = Opts()
    for k, v in defaults.items():
        setattr(opts, k, v)
    return opts


class MockWorld:
    """Minimal mock of DaveDiverWorld for testing filtering methods only."""
    def __init__(self, **option_overrides):
        self.options = make_options(**option_overrides)

    def should_include_location(self, location_name: str, location_data) -> bool:
        # Copied verbatim from __init__.py — kept in sync manually
        from davethediver.__init__ import DaveDiverWorld
        return DaveDiverWorld.should_include_location(self, location_name, location_data)

    def should_include_item(self, item_name: str) -> bool:
        from davethediver.__init__ import DaveDiverWorld
        return DaveDiverWorld.should_include_item(self, item_name)


# ─── helpers ──────────────────────────────────────────────────────────────────

def locs_with_category(category: str):
    return [(n, d) for n, d in location_table.items() if d.category == category]

def items_with_category(category: str):
    return [(n, d) for n, d in item_table.items() if d.category == category]


# ─── Location filtering tests ─────────────────────────────────────────────────

class TestLocationFiltering(unittest.TestCase):

    # ── Fish checks ───────────────────────────────────────────────────────────

    def test_fish_none_excludes_all_fish(self):
        world = MockWorld(fish_checks=0)
        fish_locs = locs_with_category("fish")
        self.assertGreater(len(fish_locs), 0, "No fish locations found to test")
        for name, data in fish_locs:
            self.assertFalse(world.should_include_location(name, data),
                             f"Fish location '{name}' should be excluded when fish_checks=none")

    def test_fish_all_includes_all_fish(self):
        world = MockWorld(fish_checks=2)
        fish_locs = locs_with_category("fish")
        for name, data in fish_locs:
            self.assertTrue(world.should_include_location(name, data),
                            f"Fish location '{name}' should be included when fish_checks=all")

    def test_fish_rare_only_excludes_shallow(self):
        world = MockWorld(fish_checks=1)
        shallow_fish = [(n, d) for n, d in locs_with_category("fish")
                        if d.region == "Blue Hole - Shallow"]
        self.assertGreater(len(shallow_fish), 0, "No shallow fish locations found")
        for name, data in shallow_fish:
            self.assertFalse(world.should_include_location(name, data),
                             f"Shallow fish '{name}' should be excluded when fish_checks=rare_only")

    def test_fish_rare_only_includes_deep_fish(self):
        world = MockWorld(fish_checks=1)
        deep_fish = [(n, d) for n, d in locs_with_category("fish")
                     if d.region != "Blue Hole - Shallow"]
        self.assertGreater(len(deep_fish), 0, "No deep fish locations found")
        for name, data in deep_fish:
            self.assertTrue(world.should_include_location(name, data),
                            f"Deep fish '{name}' should be included when fish_checks=rare_only")

    # ── Dish upgrades ─────────────────────────────────────────────────────────

    def test_dish_upgrades_none_excludes_all(self):
        world = MockWorld(dish_upgrades=0)
        dish_locs = locs_with_category("dish_upgrade")
        self.assertGreater(len(dish_locs), 0, "No dish upgrade locations found")
        for name, data in dish_locs:
            self.assertFalse(world.should_include_location(name, data),
                             f"Dish upgrade '{name}' should be excluded when dish_upgrades=none")

    def test_dish_upgrades_on_includes_all(self):
        world = MockWorld(dish_upgrades=3)
        dish_locs = locs_with_category("dish_upgrade")
        for name, data in dish_locs:
            self.assertTrue(world.should_include_location(name, data),
                            f"Dish upgrade '{name}' should be included when dish_upgrades=all")

    # ── Toggle systems ────────────────────────────────────────────────────────

    def _test_toggle(self, category: str, option_name: str):
        """Generic test for on/off toggle categories."""
        world_off = MockWorld(**{option_name: 0})
        world_on  = MockWorld(**{option_name: 1})
        locs = locs_with_category(category)
        self.assertGreater(len(locs), 0, f"No '{category}' locations found")
        for name, data in locs:
            self.assertFalse(world_off.should_include_location(name, data),
                             f"'{name}' should be excluded when {option_name}=off")
            self.assertTrue(world_on.should_include_location(name, data),
                            f"'{name}' should be included when {option_name}=on")

    def test_cooksta_toggle(self):
        self._test_toggle("cooksta", "include_cooksta")

    def test_ecowatcher_toggle(self):
        self._test_toggle("ecowatcher", "include_ecowatcher")

    def test_photography_toggle(self):
        self._test_toggle("photography", "include_photography")

    def test_challenge_toggle(self):
        self._test_toggle("challenge", "include_challenges")

    def test_farming_toggle(self):
        self._test_toggle("farming", "include_farming")

    def test_chicken_farm_toggle(self):
        self._test_toggle("chicken_farm", "include_chicken_farm")

    def test_fish_farm_toggle(self):
        self._test_toggle("fish_farm", "include_fish_farm")

    def test_minigame_toggle(self):
        self._test_toggle("minigame", "include_minigames")

    def test_weapon_shop_toggle(self):
        self._test_toggle("weapon", "include_weapon_shop")

    # ── DLC categories ────────────────────────────────────────────────────────

    def _test_dlc_toggle(self, category: str, option_name: str):
        world_off = MockWorld(**{option_name: 0})
        world_on  = MockWorld(**{option_name: 1})
        locs = locs_with_category(category)
        self.assertGreater(len(locs), 0, f"No '{category}' locations found")
        for name, data in locs:
            self.assertFalse(world_off.should_include_location(name, data),
                             f"'{name}' should be excluded when {option_name}=off")
            self.assertTrue(world_on.should_include_location(name, data),
                            f"'{name}' should be included when {option_name}=on")

    def test_dredge_dlc_toggle(self):
        self._test_dlc_toggle("dlc_dredge", "has_dredge_dlc")

    def test_godzilla_dlc_toggle(self):
        self._test_dlc_toggle("dlc_godzilla", "has_godzilla_dlc")

    # ── Always-included ───────────────────────────────────────────────────────

    def test_story_locations_always_included(self):
        """Empty-category locations (story, bosses, quests) are always included."""
        world = MockWorld(fish_checks=0, dish_upgrades=0, include_cooksta=0,
                          include_ecowatcher=0, include_photography=0,
                          include_challenges=0, include_farming=0,
                          include_chicken_farm=0, include_fish_farm=0,
                          include_minigames=0, include_weapon_shop=0,
                          has_dredge_dlc=0, has_godzilla_dlc=0)
        always_locs = locs_with_category("")
        self.assertGreater(len(always_locs), 0, "No always-included locations found")
        for name, data in always_locs:
            self.assertTrue(world.should_include_location(name, data),
                            f"Story location '{name}' should always be included")

    def test_restaurant_locations_always_included(self):
        """Restaurant-category locations are always included."""
        world = MockWorld()
        restaurant_locs = locs_with_category("restaurant")
        self.assertGreater(len(restaurant_locs), 0, "No restaurant locations found")
        for name, data in restaurant_locs:
            self.assertTrue(world.should_include_location(name, data),
                            f"Restaurant location '{name}' should always be included")


# ─── Item filtering tests ──────────────────────────────────────────────────────

class TestItemFiltering(unittest.TestCase):

    def test_progression_items_always_included(self):
        """Non-DLC progression items must always be included regardless of options.
        DLC progression items are correctly excluded when their DLC is disabled —
        that is tested separately in test_dredge_dlc_items_excluded_when_dlc_off etc.
        """
        from BaseClasses import ItemClassification
        DLC_CATEGORIES = {"dlc_dredge", "dlc_godzilla", "dlc_ichiban", "dlc_jungle", "restaurant"}
        # Turn everything off
        world = MockWorld(fish_checks=0, dish_upgrades=0, recipe_checks=0,
                          include_cooksta=0, trap_frequency=0,
                          has_dredge_dlc=0, has_godzilla_dlc=0,
                          has_ichiban_dlc=0, has_jungle_dlc=0)
        prog_items = [(n, d) for n, d in item_table.items()
                      if d.classification == ItemClassification.progression
                      and d.category not in DLC_CATEGORIES]
        self.assertGreater(len(prog_items), 0, "No non-DLC progression items found")
        for name, data in prog_items:
            self.assertTrue(world.should_include_item(name),
                            f"Progression item '{name}' must always be included")

    def test_trap_items_excluded_when_frequency_none(self):
        world = MockWorld(trap_frequency=0)
        trap_items = items_with_category("trap")
        for name, data in trap_items:
            self.assertFalse(world.should_include_item(name),
                             f"Trap item '{name}' should be excluded when trap_frequency=none")

    def test_trap_items_included_when_frequency_on(self):
        world = MockWorld(trap_frequency=1)
        trap_items = items_with_category("trap")
        for name, data in trap_items:
            self.assertTrue(world.should_include_item(name),
                            f"Trap item '{name}' should be included when trap_frequency>0")

    def test_dredge_dlc_items_excluded_when_dlc_off(self):
        world = MockWorld(has_dredge_dlc=0)
        dredge_items = items_with_category("dlc_dredge")
        self.assertGreater(len(dredge_items), 0, "No DREDGE DLC items found")
        for name, data in dredge_items:
            self.assertFalse(world.should_include_item(name),
                             f"DREDGE item '{name}' should be excluded when DLC off")

    def test_dredge_dlc_items_included_when_dlc_on(self):
        world = MockWorld(has_dredge_dlc=1)
        dredge_items = items_with_category("dlc_dredge")
        for name, data in dredge_items:
            self.assertTrue(world.should_include_item(name),
                            f"DREDGE item '{name}' should be included when DLC on")

    def test_restaurant_items_excluded_when_no_restaurant_content(self):
        """Restaurant staff/upgrades excluded when dish_upgrades=0 AND recipe_checks=0."""
        world = MockWorld(dish_upgrades=0, recipe_checks=0)
        restaurant_items = items_with_category("restaurant")
        for name, data in restaurant_items:
            self.assertFalse(world.should_include_item(name),
                             f"Restaurant item '{name}' should be excluded when no restaurant content")

    def test_restaurant_items_included_when_dish_upgrades_on(self):
        # Use milestones mode — named staff (not Progressive) are included
        world_milestones = MockWorld(dish_upgrades=1, recipe_checks=0, staff_training_depth=2)
        # Use all_levels mode — Progressive staff are included
        world_all_levels = MockWorld(dish_upgrades=1, recipe_checks=0, staff_training_depth=3)
        restaurant_items = items_with_category("restaurant")
        self.assertGreater(len(restaurant_items), 0, "No restaurant items found")
        for name, data in restaurant_items:
            is_progressive_staff = name.startswith("Progressive ") and name[12:] in (
                "Billy","Carolina","Charlie","Cohh","Davina","Drae","El Nino",
                "Itsuki","James","Jandi","Kyoko","Liu","Maki","Masayoshi","Mitchell",
                "Pai","Raptor","Raul","Tohoku","Yone","Yusuke"
            )
            world = world_all_levels if is_progressive_staff else world_milestones
            self.assertTrue(world.should_include_item(name),
                            f"Restaurant item '{name}' should be included when dish_upgrades>0")

    def test_restaurant_items_included_when_recipe_checks_on(self):
        world_milestones = MockWorld(dish_upgrades=0, recipe_checks=1, staff_training_depth=2)
        world_all_levels = MockWorld(dish_upgrades=0, recipe_checks=1, staff_training_depth=3)
        restaurant_items = items_with_category("restaurant")
        for name, data in restaurant_items:
            is_progressive_staff = name.startswith("Progressive ") and name[12:] in (
                "Billy","Carolina","Charlie","Cohh","Davina","Drae","El Nino",
                "Itsuki","James","Jandi","Kyoko","Liu","Maki","Masayoshi","Mitchell",
                "Pai","Raptor","Raul","Tohoku","Yone","Yusuke"
            )
            world = world_all_levels if is_progressive_staff else world_milestones
            self.assertTrue(world.should_include_item(name),
                            f"Restaurant item '{name}' should be included when recipe_checks>0")

    def test_unknown_item_included_by_default(self):
        """Items not in the table should default to included."""
        world = MockWorld()
        self.assertTrue(world.should_include_item("Nonexistent Item XYZ"))


# ─── Category coverage tests ──────────────────────────────────────────────────

class TestCategoryCompleteness(unittest.TestCase):
    """Ensure every location category is handled by should_include_location."""

    KNOWN_LOCATION_CATEGORIES = {
        "", "fish", "dish_upgrade", "recipe", "restaurant",
        "cooksta", "ecowatcher", "photography", "challenge",
        "farming", "chicken_farm", "fish_farm", "minigame", "weapon",
        "ingredient", "charm", "sub_mission",
        "dlc_dredge", "dlc_godzilla", "dlc_ichiban", "dlc_jungle",
        "staff_all_levels", "staff_all_levels_ichiban",
    }

    KNOWN_ITEM_CATEGORIES = {
        "", "recipe", "restaurant", "trap", "dish_upgrade", "cooksta",
        "dlc_dredge", "dlc_godzilla", "dlc_ichiban", "dlc_jungle",
    }

    def test_no_unknown_location_categories(self):
        """All location categories should be in the known set."""
        used = {d.category for d in location_table.values()}
        unknown = used - self.KNOWN_LOCATION_CATEGORIES
        self.assertEqual(unknown, set(),
                         f"Unknown location categories found: {unknown}")

    def test_no_unknown_item_categories(self):
        """All item categories should be in the known set."""
        used = {d.category for d in item_table.values()}
        unknown = used - self.KNOWN_ITEM_CATEGORIES
        self.assertEqual(unknown, set(),
                         f"Unknown item categories found: {unknown}")


if __name__ == "__main__":
    unittest.main()
