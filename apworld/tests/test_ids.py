"""
Tests for item and location ID uniqueness and validity.
No Archipelago mock needed — these test the raw data tables directly.
"""
import unittest
import sys
import os

# Add the apworld directory to the path so we can import davethediver directly
sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

from davethediver.items import item_table, ITEM_BASE as ITEM_BASE_ID
from davethediver.locations import location_table, BASE_ID as LOC_BASE_ID
from davethediver.regions import REGION_NAMES


class TestItemIDs(unittest.TestCase):
    """Verify item IDs are unique, in range, and well-formed."""

    def test_no_duplicate_item_ids(self):
        """No two items should share the same ID."""
        ids = [data.code for data in item_table.values() if data.code is not None]
        self.assertEqual(len(ids), len(set(ids)),
                         f"Duplicate item IDs found! "
                         f"Duplicates: {[x for x in ids if ids.count(x) > 1]}")

    def test_no_duplicate_item_names(self):
        """No two items should share the same name."""
        names = list(item_table.keys())
        self.assertEqual(len(names), len(set(names)),
                         "Duplicate item names found!")

    def test_all_item_ids_positive(self):
        """All item IDs should be positive integers."""
        for name, data in item_table.items():
            if data.code is not None:
                self.assertGreater(data.code, 0,
                                   f"Item '{name}' has non-positive ID {data.code}")

    def test_item_ids_based_on_base_id(self):
        """All item IDs should be >= BASE_ID."""
        for name, data in item_table.items():
            if data.code is not None:
                self.assertGreaterEqual(data.code, ITEM_BASE_ID,
                                        f"Item '{name}' ID {data.code} is below BASE_ID {ITEM_BASE_ID}")

    def test_all_items_have_valid_count(self):
        """All items should have count >= 1."""
        for name, data in item_table.items():
            self.assertGreaterEqual(data.count, 1,
                                    f"Item '{name}' has count {data.count} < 1")

    def test_all_items_have_classification(self):
        """All items should have a non-None classification."""
        for name, data in item_table.items():
            self.assertIsNotNone(data.classification,
                                 f"Item '{name}' has no classification")

    def test_item_count(self):
        """Sanity check: we should have at least 200 items."""
        self.assertGreaterEqual(len(item_table), 200,
                                f"Only {len(item_table)} items — expected 200+")


class TestLocationIDs(unittest.TestCase):
    """Verify location IDs are unique, in range, and well-formed."""

    def test_no_duplicate_location_ids(self):
        """No two locations should share the same ID."""
        ids = [data.code for data in location_table.values() if data.code is not None]
        self.assertEqual(len(ids), len(set(ids)),
                         f"Duplicate location IDs found! "
                         f"Duplicates: {[x for x in ids if ids.count(x) > 1]}")

    def test_no_duplicate_location_names(self):
        """No two locations should share the same name."""
        names = list(location_table.keys())
        self.assertEqual(len(names), len(set(names)),
                         "Duplicate location names found!")

    def test_all_location_ids_positive(self):
        """All location IDs should be positive integers."""
        for name, data in location_table.items():
            if data.code is not None:
                self.assertGreater(data.code, 0,
                                   f"Location '{name}' has non-positive ID {data.code}")

    def test_location_ids_based_on_base_id(self):
        """All location IDs should be >= BASE_ID."""
        for name, data in location_table.items():
            if data.code is not None:
                self.assertGreaterEqual(data.code, LOC_BASE_ID,
                                        f"Location '{name}' ID {data.code} is below BASE_ID {LOC_BASE_ID}")

    def test_all_locations_have_valid_region(self):
        """All locations must reference a region that exists in REGION_NAMES."""
        invalid = [
            f"'{name}' → '{data.region}'"
            for name, data in location_table.items()
            if data.region not in REGION_NAMES
        ]
        self.assertEqual(invalid, [],
                         f"Locations with invalid regions:\n" + "\n".join(invalid))

    def test_location_count(self):
        """Sanity check: we should have at least 1000 locations."""
        self.assertGreaterEqual(len(location_table), 1000,
                                f"Only {len(location_table)} locations — expected 1000+")

    def test_no_item_id_collision_with_location_id(self):
        """Item and location IDs share the same BASE_ID space — check for collisions."""
        item_ids = set(d.code for d in item_table.values() if d.code is not None)
        loc_ids = set(d.code for d in location_table.values() if d.code is not None)
        collisions = item_ids & loc_ids
        self.assertEqual(collisions, set(),
                         f"Item/location ID collisions found: {collisions}")


class TestRegionNames(unittest.TestCase):
    """Verify the region name registry is complete and consistent."""

    def test_expected_regions_present(self):
        """All expected game regions should be defined."""
        expected = {
            "Menu", "Bancho Sushi",
            "Blue Hole - Shallow", "Blue Hole - Mid", "Blue Hole - Deep",
            "Sea People Village", "Glacial Passage", "Glacier Zone",
            "Hydrothermal Vents",
            "Fish Farm", "Vegetable Farm", "Chicken Farm",
            "Jellyfish Basin", "Fog Coast", "Black Cliff",
        }
        for region in expected:
            self.assertIn(region, REGION_NAMES,
                          f"Expected region '{region}' not found in REGION_NAMES")

    def test_all_location_regions_in_registry(self):
        """Every region referenced by a location must be in REGION_NAMES."""
        used_regions = {data.region for data in location_table.values()}
        unknown = used_regions - REGION_NAMES
        self.assertEqual(unknown, set(),
                         f"Locations reference regions not in REGION_NAMES: {unknown}")
