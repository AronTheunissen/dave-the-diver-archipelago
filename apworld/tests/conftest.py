"""
pytest configuration for Dave the Diver APWorld tests.

Since the APWorld code imports from Archipelago's BaseClasses module
(which requires a full Archipelago installation), we mock the minimum
required symbols before any test modules are imported.
"""
import sys
from unittest.mock import MagicMock
from enum import IntEnum


# ── Mock BaseClasses ──────────────────────────────────────────────────────────

class ItemClassification(IntEnum):
    filler      = 0
    progression = 1
    useful      = 2
    trap        = 4


class MockItem:
    def __init__(self, name, classification, code, player):
        self.name           = name
        self.classification = classification
        self.code           = code
        self.player         = player


class MockLocation:
    def __init__(self, player, name, address, parent):
        self.player  = player
        self.name    = name
        self.address = address
        self.parent  = parent
        self.item    = None


class MockRegion:
    def __init__(self, name, player, multiworld):
        self.name       = name
        self.player     = player
        self.multiworld = multiworld
        self.locations  = []
        self.exits      = []

    def connect(self, target, rule=None, name=None):
        self.exits.append(target)


class MockMultiWorld:
    def __init__(self):
        self.regions  = []
        self.itempool = []


class MockWebWorld:
    pass


class MockTutorial:
    def __init__(self, *args, **kwargs):
        pass


class MockWorld:
    web = MockWebWorld()
    def __init__(self, world, player):
        self.multiworld = world
        self.player     = player


# ── Mock Options base classes (need real Python classes for @dataclass + inheritance) ─

class PerGameCommonOptions:
    """Mock base for DaveDiverOptions @dataclass."""
    pass

class Toggle:
    default = 0
    def __init__(self): self.value = self.default

class DefaultOnToggle(Toggle):
    default = 1

class Range:
    range_start = 0
    range_end   = 100
    default     = 0
    def __init__(self): self.value = self.default

class Choice:
    default = 0
    def __init__(self): self.value = self.default

class OptionSet:
    default = frozenset()
    def __init__(self): self.value = set()


# ── Build fake module objects ──────────────────────────────────────────────────

base_classes_mock = MagicMock()
base_classes_mock.ItemClassification = ItemClassification
base_classes_mock.Item               = MockItem
base_classes_mock.Location           = MockLocation
base_classes_mock.Region             = MockRegion
base_classes_mock.MultiWorld         = MockMultiWorld
base_classes_mock.Tutorial           = MockTutorial
base_classes_mock.World              = MockWorld

auto_world_mock = MagicMock()
auto_world_mock.WebWorld = MockWebWorld
auto_world_mock.World    = MockWorld

options_mock = MagicMock()
options_mock.PerGameCommonOptions = PerGameCommonOptions
options_mock.Toggle               = Toggle
options_mock.DefaultOnToggle      = DefaultOnToggle
options_mock.Range                = Range
options_mock.Choice               = Choice
options_mock.OptionSet            = OptionSet

# Register all mocks BEFORE any davethediver imports happen
sys.modules["BaseClasses"]           = base_classes_mock
sys.modules["worlds"]                = MagicMock()
sys.modules["worlds.AutoWorld"]      = auto_world_mock
sys.modules["Options"]               = options_mock
sys.modules["worlds.generic"]        = MagicMock()
sys.modules["worlds.generic.Rules"]  = MagicMock()
sys.modules["Fill"]                  = MagicMock()
