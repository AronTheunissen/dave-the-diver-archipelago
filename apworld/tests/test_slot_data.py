"""
Tests for fill_slot_data() — verifies all expected keys are present
and values reflect the options correctly.
"""
import unittest
import sys
import os

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))


def make_options(**overrides):
    class V:
        def __init__(self, v): self.value = v
        def __bool__(self): return bool(self.value)

    defaults = {
        "goal":                       V(0),
        "fish_checks":                V(2),
        "dish_upgrades":              V(3),
        "recipe_checks":              V(1),
        "include_cooksta":            V(1),
        "include_ecowatcher":         V(1),
        "include_photography":        V(1),
        "include_challenges":         V(1),
        "include_farming":            V(1),
        "include_chicken_farm":       V(1),
        "include_fish_farm":          V(1),
        "include_minigames":          V(1),
        "include_weapon_shop":        V(1),
        "has_dredge_dlc":             V(0),
        "has_godzilla_dlc":           V(0),
        "has_ichiban_dlc":            V(0),
        "has_jungle_dlc":             V(0),
        "starting_oxygen_level":      V(1),
        "starting_harpoon_level":     V(1),
        "starting_diving_suit_level": V(1),
        "death_link":                 V(0),
        "trap_frequency":             V(0),
        "chapters_required":          V(7),
        "teleport_behavior":          V(0),
        "oxygen_requirement":         V(0),
    }
    for k, v in overrides.items():
        defaults[k] = V(v)

    class Opts:
        pass
    opts = Opts()
    for k, v in defaults.items():
        setattr(opts, k, v)
    return opts


class MockWorldForSlotData:
    """Minimal mock that only has options — enough to call fill_slot_data."""
    def __init__(self, **overrides):
        self.options = make_options(**overrides)

    def fill_slot_data(self):
        from davethediver.__init__ import DaveDiverWorld
        return DaveDiverWorld.fill_slot_data(self)


EXPECTED_KEYS = {
    "goal", "fish_checks", "dish_upgrades", "recipe_checks",
    "include_cooksta", "include_ecowatcher", "include_photography",
    "include_challenges", "include_farming", "include_chicken_farm",
    "include_fish_farm", "include_minigames", "include_weapon_shop",
    "has_dredge_dlc", "has_godzilla_dlc", "has_ichiban_dlc", "has_jungle_dlc",
    "starting_oxygen_level", "starting_harpoon_level", "starting_suit_level",
    "death_link", "trap_frequency",
}


class TestFillSlotData(unittest.TestCase):

    def test_all_expected_keys_present(self):
        """fill_slot_data() must include all expected keys."""
        world = MockWorldForSlotData()
        slot_data = world.fill_slot_data()
        missing = EXPECTED_KEYS - set(slot_data.keys())
        self.assertEqual(missing, set(),
                         f"fill_slot_data() is missing keys: {missing}")

    def test_no_unexpected_keys(self):
        """fill_slot_data() should not contain unknown keys."""
        world = MockWorldForSlotData()
        slot_data = world.fill_slot_data()
        extra = set(slot_data.keys()) - EXPECTED_KEYS
        self.assertEqual(extra, set(),
                         f"fill_slot_data() has unexpected keys: {extra}")

    def test_all_values_are_int_or_bool(self):
        """All slot data values should be serialisable (int or bool)."""
        world = MockWorldForSlotData()
        slot_data = world.fill_slot_data()
        for key, value in slot_data.items():
            self.assertIsInstance(value, (int, bool),
                                  f"Key '{key}' has non-int value: {type(value)}")

    def test_goal_value_reflects_option(self):
        for goal_val in range(5):
            world = MockWorldForSlotData(goal=goal_val)
            self.assertEqual(world.fill_slot_data()["goal"], goal_val)

    def test_fish_checks_value_reflects_option(self):
        for val in [0, 1, 2]:
            world = MockWorldForSlotData(fish_checks=val)
            self.assertEqual(world.fill_slot_data()["fish_checks"], val)

    def test_death_link_value_reflects_option(self):
        world_off = MockWorldForSlotData(death_link=0)
        world_on  = MockWorldForSlotData(death_link=1)
        self.assertEqual(world_off.fill_slot_data()["death_link"], 0)
        self.assertEqual(world_on.fill_slot_data()["death_link"], 1)

    def test_dlc_flags_default_off(self):
        """DLC flags should default to 0 (off)."""
        world = MockWorldForSlotData()
        slot_data = world.fill_slot_data()
        self.assertEqual(slot_data["has_dredge_dlc"], 0)
        self.assertEqual(slot_data["has_godzilla_dlc"], 0)
        self.assertEqual(slot_data["has_ichiban_dlc"], 0)
        self.assertEqual(slot_data["has_jungle_dlc"], 0)

    def test_dlc_flags_when_enabled(self):
        world = MockWorldForSlotData(has_dredge_dlc=1, has_godzilla_dlc=1)
        slot_data = world.fill_slot_data()
        self.assertEqual(slot_data["has_dredge_dlc"], 1)
        self.assertEqual(slot_data["has_godzilla_dlc"], 1)

    def test_starting_levels_reflected(self):
        world = MockWorldForSlotData(
            starting_oxygen_level=3,
            starting_harpoon_level=2,
            starting_diving_suit_level=4,
        )
        slot_data = world.fill_slot_data()
        self.assertEqual(slot_data["starting_oxygen_level"], 3)
        self.assertEqual(slot_data["starting_harpoon_level"], 2)
        self.assertEqual(slot_data["starting_suit_level"], 4)


if __name__ == "__main__":
    unittest.main()
