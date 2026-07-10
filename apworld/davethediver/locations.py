"""
Dave the Diver - Location Definitions

This file defines all check locations in Dave the Diver.
Locations are places where the player can receive items.
"""

from typing import Dict, NamedTuple, Optional


class LocationData(NamedTuple):
    """Data for a location definition"""
    code: Optional[int]
    region: str       # Which region this location belongs to
    category: str = ""  # Used for option-based filtering (e.g. "fish", "cooksta")


# Base location ID
BASE_ID = 0x444400


# === STORY PROGRESSION ===
# The game has a Prologue + 7 chapters
story_locations: Dict[str, LocationData] = {
    # Prologue - A Sushi Bar by the Blue Hole (tutorial)
    "Story: Complete Prologue": LocationData(BASE_ID + 0, "Bancho Sushi"),
    # Chapter 1 - Traces of the Sea People
    "Story: Complete Chapter 1 (Traces of the Sea People)": LocationData(BASE_ID + 1, "Blue Hole - Shallow"),
    # Chapter 2 - Into the Deep
    "Story: Complete Chapter 2 (Into the Deep)": LocationData(BASE_ID + 2, "Blue Hole - Deep"),
    # Chapter 3 - A Request from the Sea People
    "Story: Discover Sea People Village": LocationData(BASE_ID + 3, "Sea People Village"),
    "Story: Complete Chapter 3 (A Request from the Sea People)": LocationData(BASE_ID + 4, "Sea People Village"),
    # Chapter 4 - Abandoned Cave
    "Story: Complete Chapter 4 (Abandoned Cave)": LocationData(BASE_ID + 5, "Blue Hole - Deep"),
    # Chapter 5 - Frozen Passage
    "Story: Discover Glacier Passage": LocationData(BASE_ID + 6, "Glacial Passage"),
    "Story: Complete Chapter 5 (Frozen Passage)": LocationData(BASE_ID + 7, "Glacial Passage"),
    # Chapter 6 - Melting Glacier
    "Story: Complete Chapter 6 (Melting Glacier)": LocationData(BASE_ID + 8, "Glacier Zone"),
    # Chapter 7 - Broken Control Room (final chapter)
    "Story: Complete Chapter 7 (Broken Control Room)": LocationData(BASE_ID + 9, "Glacier Zone"),
    # Key story milestones within chapters (not chapter completions, but important events)
    "Story: Complete The Leahs-chan Rescue":    LocationData(BASE_ID + 10, "Blue Hole - Mid"),   # Ch1 — gives Gas Cutter; defeating Giant Squid gives Headlamp
    "Story: Complete Deliver Key to Tenzhin":  LocationData(BASE_ID + 11, "Sea People Village"), # Ch4 — gives Sea People Necklace (tubeworm tunnels)
    "Story: Complete Cobra's Lost Crowbar":    LocationData(BASE_ID + 12, "Glacial Passage"),    # Ch5 — gives Cobra's Lost Crowbar, required for Giant Gadon
    "iDiver App: Upgrade Slot 1":              LocationData(BASE_ID + 13, "Bancho Sushi"),        # iDiver app checks — give AP items to other games
    "iDiver App: Upgrade Slot 2":              LocationData(BASE_ID + 14, "Bancho Sushi"),
    "iDiver App: Upgrade Slot 3":              LocationData(BASE_ID + 15, "Bancho Sushi"),
}

# === FISH CATCHING ===
# Catching each fish species for the FIRST TIME

# === FISH LOCATIONS (First Catch) ===
# Complete species list from game wiki, organized by region.
# fish_checks option: "rare_only" excludes Blue Hole - Shallow species.

# Blue Hole Shallow (0-50m)
common_fish_locations: Dict[str, LocationData] = {
    "First Catch: American Lobster":         LocationData(BASE_ID + 100, "Blue Hole - Shallow", "fish"),
    "First Catch: Barrel Jellyfish":         LocationData(BASE_ID + 101, "Blue Hole - Shallow", "fish"),
    "First Catch: Big-Belly Seahorse":       LocationData(BASE_ID + 102, "Blue Hole - Shallow", "fish"),
    "First Catch: Black and White Snapper":  LocationData(BASE_ID + 103, "Blue Hole - Shallow", "fish"),
    "First Catch: Blacktip Reefshark":       LocationData(BASE_ID + 104, "Blue Hole - Shallow", "fish"),
    "First Catch: Blue Lobster":             LocationData(BASE_ID + 105, "Blue Hole - Shallow", "fish"),
    "First Catch: Blue Tang":                LocationData(BASE_ID + 106, "Blue Hole - Shallow", "fish"),
    "First Catch: Bluefin Tuna":             LocationData(BASE_ID + 107, "Blue Hole - Shallow", "fish"),
    "First Catch: Box Jellyfish":            LocationData(BASE_ID + 108, "Blue Hole - Shallow", "fish"),
    "First Catch: Cardinal Fish":            LocationData(BASE_ID + 109, "Blue Hole - Shallow", "fish"),
    "First Catch: Clearfin Lionfish":        LocationData(BASE_ID + 110, "Blue Hole - Shallow", "fish"),
    "First Catch: Clownfish":                LocationData(BASE_ID + 111, "Blue Hole - Shallow", "fish"),
    "First Catch: Comber":                   LocationData(BASE_ID + 112, "Blue Hole - Shallow", "fish"),
    "First Catch: Copper Shark":             LocationData(BASE_ID + 113, "Blue Hole - Shallow", "fish"),
    "First Catch: Emperor Angelfish":        LocationData(BASE_ID + 114, "Blue Hole - Shallow", "fish"),
    "First Catch: European Lobster":         LocationData(BASE_ID + 115, "Blue Hole - Shallow", "fish"),
    "First Catch: Flame Angelfish":          LocationData(BASE_ID + 116, "Blue Hole - Shallow", "fish"),
    "First Catch: Fried Egg Jellyfish":      LocationData(BASE_ID + 117, "Blue Hole - Shallow", "fish"),
    "First Catch: Great White Shark Klaus":  LocationData(BASE_ID + 118, "Blue Hole - Shallow", "fish"),
    "First Catch: Green Humphead Parrotfish":LocationData(BASE_ID + 119, "Blue Hole - Shallow", "fish"),
    "First Catch: Green Sea Urchin":         LocationData(BASE_ID + 120, "Blue Hole - Shallow", "fish"),
    "First Catch: Jayakar's Seahorse":       LocationData(BASE_ID + 121, "Blue Hole - Shallow", "fish"),
    "First Catch: Lagoon Triggerfish":       LocationData(BASE_ID + 122, "Blue Hole - Shallow", "fish"),
    "First Catch: Long-Snouted Seahorse":    LocationData(BASE_ID + 123, "Blue Hole - Shallow", "fish"),
    "First Catch: Longfin Batfish":          LocationData(BASE_ID + 124, "Blue Hole - Shallow", "fish"),
    "First Catch: Longspine Porcupinefish":  LocationData(BASE_ID + 125, "Blue Hole - Shallow", "fish"),
    "First Catch: Longspine Squirrelfish":   LocationData(BASE_ID + 126, "Blue Hole - Shallow", "fish"),
    "First Catch: Mantis Shrimp":            LocationData(BASE_ID + 127, "Blue Hole - Shallow", "fish"),
    "First Catch: Marbled Electric Ray":     LocationData(BASE_ID + 128, "Blue Hole - Shallow", "fish"),
    "First Catch: Marlin":                   LocationData(BASE_ID + 129, "Blue Hole - Shallow", "fish"),
    "First Catch: Mediterranean Parrotfish": LocationData(BASE_ID + 130, "Blue Hole - Shallow", "fish"),
    "First Catch: Moray Eel":                LocationData(BASE_ID + 131, "Blue Hole - Shallow", "fish"),
    "First Catch: Orbicular Batfish":        LocationData(BASE_ID + 132, "Blue Hole - Shallow", "fish"),
    "First Catch: Ornate Wrasse":            LocationData(BASE_ID + 133, "Blue Hole - Shallow", "fish"),
    "First Catch: Pacific Seahorse":         LocationData(BASE_ID + 134, "Blue Hole - Shallow", "fish"),
    "First Catch: Purple Sea Urchin":        LocationData(BASE_ID + 135, "Blue Hole - Shallow", "fish"),
    "First Catch: Pyramid Butterflyfish":    LocationData(BASE_ID + 136, "Blue Hole - Shallow", "fish"),
    "First Catch: Rainbow Wrasse":           LocationData(BASE_ID + 137, "Blue Hole - Shallow", "fish"),
    "First Catch: Red Lionfish":             LocationData(BASE_ID + 138, "Blue Hole - Shallow", "fish"),
    "First Catch: Red-banded Lobster":       LocationData(BASE_ID + 139, "Blue Hole - Shallow", "fish"),
    "First Catch: Redtoothed Triggerfish":   LocationData(BASE_ID + 140, "Blue Hole - Shallow", "fish"),
    "First Catch: Salema Porgy":             LocationData(BASE_ID + 141, "Blue Hole - Shallow", "fish"),
    "First Catch: Sea Goldie":               LocationData(BASE_ID + 142, "Blue Hole - Shallow", "fish"),
    "First Catch: Sheepshead":               LocationData(BASE_ID + 143, "Blue Hole - Shallow", "fish"),
    "First Catch: Shortfin Mako":            LocationData(BASE_ID + 144, "Blue Hole - Shallow", "fish"),
    "First Catch: Small Spotted Dart":       LocationData(BASE_ID + 145, "Blue Hole - Shallow", "fish"),
    "First Catch: Starry Puffer":            LocationData(BASE_ID + 146, "Blue Hole - Shallow", "fish"),
    "First Catch: Stingray":                 LocationData(BASE_ID + 147, "Blue Hole - Shallow", "fish"),
    "First Catch: Striped Catfish":          LocationData(BASE_ID + 148, "Blue Hole - Shallow", "fish"),
    "First Catch: Thresher Shark":           LocationData(BASE_ID + 149, "Blue Hole - Shallow", "fish"),
    "First Catch: Titan Triggerfish":        LocationData(BASE_ID + 150, "Blue Hole - Shallow", "fish"),
    "First Catch: Truck Hermit Crab":        LocationData(BASE_ID + 151, "Blue Hole - Shallow", "fish"),
    "First Catch: White Shrimp":             LocationData(BASE_ID + 152, "Blue Hole - Shallow", "fish"),
    "First Catch: Whiteleg Shrimp":          LocationData(BASE_ID + 153, "Blue Hole - Shallow", "fish"),
    "First Catch: Whitetip Reefshark":       LocationData(BASE_ID + 154, "Blue Hole - Shallow", "fish"),
    "First Catch: Yellow Tang":              LocationData(BASE_ID + 155, "Blue Hole - Shallow", "fish"),
    "First Catch: Yellowback Fusilier":      LocationData(BASE_ID + 156, "Blue Hole - Shallow", "fish"),
    "First Catch: Yellowfin Tuna":           LocationData(BASE_ID + 157, "Blue Hole - Shallow", "fish"),
    "First Catch: Zebra Shark":              LocationData(BASE_ID + 158, "Blue Hole - Shallow", "fish"),
}

# Blue Hole Mid Depth (50-130m)
rare_fish_locations: Dict[str, LocationData] = {
    "First Catch: Atlantic Anglerfish":            LocationData(BASE_ID + 159, "Blue Hole - Mid", "fish"),
    "First Catch: Atlantic Bonito":                LocationData(BASE_ID + 160, "Blue Hole - Mid", "fish"),
    "First Catch: Atlantic Mackerel":              LocationData(BASE_ID + 161, "Blue Hole - Mid", "fish"),
    "First Catch: Bigeye Scad":                    LocationData(BASE_ID + 162, "Blue Hole - Mid", "fish"),
    "First Catch: Bigeye Trevally":                LocationData(BASE_ID + 163, "Blue Hole - Mid", "fish"),
    "First Catch: Black Tiger Shrimp":             LocationData(BASE_ID + 164, "Blue Hole - Mid", "fish"),
    "First Catch: Blackfin Barracuda":             LocationData(BASE_ID + 165, "Blue Hole - Mid", "fish"),
    "First Catch: Bluehead Tilefish":              LocationData(BASE_ID + 166, "Blue Hole - Mid", "fish"),
    "First Catch: California Spiny Lobster":       LocationData(BASE_ID + 167, "Blue Hole - Mid", "fish"),
    "First Catch: Clown Frogfish":                 LocationData(BASE_ID + 168, "Blue Hole - Mid", "fish"),
    "First Catch: Coral Trout":                    LocationData(BASE_ID + 169, "Blue Hole - Mid", "fish"),
    "First Catch: Crystal Lobster":                LocationData(BASE_ID + 170, "Blue Hole - Mid", "fish"),
    "First Catch: Cuttlefish":                     LocationData(BASE_ID + 171, "Blue Hole - Mid", "fish"),
    "First Catch: Devil Scorpionfish":             LocationData(BASE_ID + 172, "Blue Hole - Mid", "fish"),
    "First Catch: Dusky Grouper":                  LocationData(BASE_ID + 173, "Blue Hole - Mid", "fish"),
    "First Catch: Dwarf Seahorse":                 LocationData(BASE_ID + 174, "Blue Hole - Mid", "fish"),
    "First Catch: Fan Lobster":                    LocationData(BASE_ID + 175, "Blue Hole - Mid", "fish"),
    "First Catch: Giant Squid":                    LocationData(BASE_ID + 176, "Blue Hole - Mid", "fish"),
    "First Catch: Giant Trevally":                 LocationData(BASE_ID + 177, "Blue Hole - Mid", "fish"),
    "First Catch: Giraffe Seahorse":               LocationData(BASE_ID + 178, "Blue Hole - Mid", "fish"),
    "First Catch: Great Barracuda":                LocationData(BASE_ID + 179, "Blue Hole - Mid", "fish"),
    "First Catch: Grey Triggerfish":               LocationData(BASE_ID + 180, "Blue Hole - Mid", "fish"),
    "First Catch: Harlequin Hind":                 LocationData(BASE_ID + 181, "Blue Hole - Mid", "fish"),
    "First Catch: Hedgehog Seahorse":              LocationData(BASE_ID + 182, "Blue Hole - Mid", "fish"),
    "First Catch: Humboldt Squid":                 LocationData(BASE_ID + 183, "Blue Hole - Mid", "fish"),
    "First Catch: Longnose Sawshark":              LocationData(BASE_ID + 184, "Blue Hole - Mid", "fish"),
    "First Catch: Lusca":                          LocationData(BASE_ID + 185, "Blue Hole - Mid", "fish"),
    "First Catch: Mackerel Scad":                  LocationData(BASE_ID + 186, "Blue Hole - Mid", "fish"),
    "First Catch: Narrow-Barred Spanish Mackerel": LocationData(BASE_ID + 187, "Blue Hole - Mid", "fish"),
    "First Catch: Painted Comber":                 LocationData(BASE_ID + 188, "Blue Hole - Mid", "fish"),
    "First Catch: Sailfish":                       LocationData(BASE_ID + 189, "Blue Hole - Mid", "fish"),
    "First Catch: Sally Lightfoot Crab":           LocationData(BASE_ID + 190, "Blue Hole - Mid", "fish"),
    "First Catch: Smooth Hammerhead":              LocationData(BASE_ID + 191, "Blue Hole - Mid", "fish"),
    "First Catch: Spear Squid":                    LocationData(BASE_ID + 192, "Blue Hole - Mid", "fish"),
    "First Catch: Spiny Seahorse":                 LocationData(BASE_ID + 193, "Blue Hole - Mid", "fish"),
    "First Catch: Striped Red Mullet":             LocationData(BASE_ID + 194, "Blue Hole - Mid", "fish"),
    "First Catch: Tiger Shark":                    LocationData(BASE_ID + 195, "Blue Hole - Mid", "fish"),
    "First Catch: Tiger-Tail Seahorse":            LocationData(BASE_ID + 196, "Blue Hole - Mid", "fish"),
    "First Catch: Tropical Rock Lobster":          LocationData(BASE_ID + 197, "Blue Hole - Mid", "fish"),
    "First Catch: White Spotted Jellyfish":        LocationData(BASE_ID + 198, "Blue Hole - Mid", "fish"),
    "First Catch: White Trevally":                 LocationData(BASE_ID + 199, "Blue Hole - Mid", "fish"),
    "First Catch: Zebra Seahorse":                 LocationData(BASE_ID + 1200, "Blue Hole - Mid", "fish"),

    # Blue Hole Deep (130-250m)
    "First Catch: Blood-belly Comb Jellyfish":     LocationData(BASE_ID + 1201, "Blue Hole - Deep", "fish"),
    "First Catch: Bluespotted Stargazer":          LocationData(BASE_ID + 1202, "Blue Hole - Deep", "fish"),
    "First Catch: Chambered Nautilus":             LocationData(BASE_ID + 1203, "Blue Hole - Deep", "fish"),
    "First Catch: Clione":                         LocationData(BASE_ID + 1204, "Blue Hole - Deep", "fish"),
    "First Catch: Clione Queen":                   LocationData(BASE_ID + 1205, "Blue Hole - Deep", "fish"),
    "First Catch: Comb Jelly":                     LocationData(BASE_ID + 1206, "Blue Hole - Deep", "fish"),
    "First Catch: Cookiecutter Shark":             LocationData(BASE_ID + 1207, "Blue Hole - Deep", "fish"),
    "First Catch: Crowned Seahorse":               LocationData(BASE_ID + 1208, "Blue Hole - Deep", "fish"),
    "First Catch: Eastern Rock Lobster":           LocationData(BASE_ID + 1209, "Blue Hole - Deep", "fish"),
    "First Catch: Fangtooth":                      LocationData(BASE_ID + 1210, "Blue Hole - Deep", "fish"),
    "First Catch: Frilled Shark":                  LocationData(BASE_ID + 1211, "Blue Hole - Deep", "fish"),
    "First Catch: Giant Wolf Eel":                 LocationData(BASE_ID + 1212, "Blue Hole - Deep", "fish"),
    "First Catch: Goblin Shark":                   LocationData(BASE_ID + 1213, "Blue Hole - Deep", "fish"),
    "First Catch: Lined Seahorse":                 LocationData(BASE_ID + 1214, "Blue Hole - Deep", "fish"),
    "First Catch: Megamouth Shark":                LocationData(BASE_ID + 1215, "Blue Hole - Deep", "fish"),
    "First Catch: Norway Lobster":                 LocationData(BASE_ID + 1216, "Blue Hole - Deep", "fish"),
    "First Catch: Pacific Fanfish":                LocationData(BASE_ID + 1217, "Blue Hole - Deep", "fish"),
    "First Catch: Red Bream":                      LocationData(BASE_ID + 1218, "Blue Hole - Deep", "fish"),
    "First Catch: Rhinochimaeridae":               LocationData(BASE_ID + 1219, "Blue Hole - Deep", "fish"),
    "First Catch: Salmon Snailfish":               LocationData(BASE_ID + 1220, "Blue Hole - Deep", "fish"),
    "First Catch: Sea Toad":                       LocationData(BASE_ID + 1221, "Blue Hole - Deep", "fish"),
    "First Catch: Spider Crab":                    LocationData(BASE_ID + 1222, "Blue Hole - Deep", "fish"),
    "First Catch: Spotted Seahorse":               LocationData(BASE_ID + 1223, "Blue Hole - Deep", "fish"),
    "First Catch: Threetooth Puffer":              LocationData(BASE_ID + 1224, "Blue Hole - Deep", "fish"),
    "First Catch: White Seahorse":                 LocationData(BASE_ID + 1225, "Blue Hole - Deep", "fish"),

    # Glacial Passage
    "First Catch: Barreleye":                      LocationData(BASE_ID + 1226, "Glacial Passage", "fish"),
    "First Catch: Blobfish":                       LocationData(BASE_ID + 1227, "Glacial Passage", "fish"),
    "First Catch: Dumbo Octopus":                  LocationData(BASE_ID + 1228, "Glacial Passage", "fish"),
    "First Catch: Peacock Squid":                  LocationData(BASE_ID + 1229, "Glacial Passage", "fish"),
    "First Catch: Pelican Eel":                    LocationData(BASE_ID + 1230, "Glacial Passage", "fish"),
    "First Catch: Vampire Squid":                  LocationData(BASE_ID + 1231, "Glacial Passage", "fish"),

    # Glacier Zone
    "First Catch: Alaska Pollock":                 LocationData(BASE_ID + 1232, "Glacier Zone", "fish"),
    "First Catch: Antarctic Octopus":              LocationData(BASE_ID + 1233, "Glacier Zone", "fish"),
    "First Catch: Arctic Cod":                     LocationData(BASE_ID + 1234, "Glacier Zone", "fish"),
    "First Catch: Arctic Telescope Fish":          LocationData(BASE_ID + 1235, "Glacier Zone", "fish"),
    "First Catch: Capelin":                        LocationData(BASE_ID + 1236, "Glacier Zone", "fish"),
    "First Catch: Gelatinous Snailfish":           LocationData(BASE_ID + 1237, "Glacier Zone", "fish"),
    "First Catch: Golden King Crab":               LocationData(BASE_ID + 1238, "Glacier Zone", "fish"),
    "First Catch: Greenland Shark":                LocationData(BASE_ID + 1239, "Glacier Zone", "fish"),
    "First Catch: Haddock":                        LocationData(BASE_ID + 1240, "Glacier Zone", "fish"),
    "First Catch: Horsehair Crab":                 LocationData(BASE_ID + 1241, "Glacier Zone", "fish"),
    "First Catch: Ice Fish":                       LocationData(BASE_ID + 1242, "Glacier Zone", "fish"),
    "First Catch: Leafy Seadragon":                LocationData(BASE_ID + 1243, "Glacier Zone", "fish"),
    "First Catch: Lumpfish":                       LocationData(BASE_ID + 1244, "Glacier Zone", "fish"),
    "First Catch: Narwhal":                        LocationData(BASE_ID + 1245, "Glacier Zone", "fish"),
    "First Catch: Phantom Jellyfish":              LocationData(BASE_ID + 1246, "Glacier Zone", "fish"),
    "First Catch: Polar Eelpout":                  LocationData(BASE_ID + 1247, "Glacier Zone", "fish"),
    "First Catch: Porbeagle Shark":                LocationData(BASE_ID + 1248, "Glacier Zone", "fish"),
    "First Catch: Snow Crab":                      LocationData(BASE_ID + 1249, "Glacier Zone", "fish"),
    "First Catch: Snub-nosed Spiny Eel":           LocationData(BASE_ID + 1250, "Glacier Zone", "fish"),
    "First Catch: Starry Skate":                   LocationData(BASE_ID + 1251, "Glacier Zone", "fish"),
    "First Catch: Weedy Seadragon":                LocationData(BASE_ID + 1252, "Glacier Zone", "fish"),

    # Hydrothermal Vents
    "First Catch: Allenypterus":                   LocationData(BASE_ID + 1253, "Hydrothermal Vents", "fish"),
    "First Catch: Anomalocaris":                   LocationData(BASE_ID + 1254, "Hydrothermal Vents", "fish"),
    "First Catch: Dollocaris Ingens":              LocationData(BASE_ID + 1255, "Hydrothermal Vents", "fish"),
    "First Catch: Drepanaspis":                    LocationData(BASE_ID + 1256, "Hydrothermal Vents", "fish"),
    "First Catch: Dunkleosteus":                   LocationData(BASE_ID + 1257, "Hydrothermal Vents", "fish"),
    "First Catch: Falcatus":                       LocationData(BASE_ID + 1258, "Hydrothermal Vents", "fish"),
    "First Catch: Helicoprion":                    LocationData(BASE_ID + 1259, "Hydrothermal Vents", "fish"),
    "First Catch: Kronosaurus":                    LocationData(BASE_ID + 1260, "Hydrothermal Vents", "fish"),
    "First Catch: Megalograptus":                  LocationData(BASE_ID + 1261, "Hydrothermal Vents", "fish"),
    "First Catch: Pikaia":                         LocationData(BASE_ID + 1262, "Hydrothermal Vents", "fish"),
    "First Catch: Qingmendous":                    LocationData(BASE_ID + 1263, "Hydrothermal Vents", "fish"),
    "First Catch: Ruby Seadragon":                 LocationData(BASE_ID + 1264, "Hydrothermal Vents", "fish"),
    "First Catch: Tokummia Katalepsis":            LocationData(BASE_ID + 1265, "Hydrothermal Vents", "fish"),
    "First Catch: Waptia Fieldensis":              LocationData(BASE_ID + 1266, "Hydrothermal Vents", "fish"),
    "First Catch: Xenacanthus":                    LocationData(BASE_ID + 1267, "Hydrothermal Vents", "fish"),
    "First Catch: Yawie":                          LocationData(BASE_ID + 1268, "Hydrothermal Vents", "fish"),
}

# Aberration Fish (Vortex regions — require Vortex Entry)
boss_fish_locations: Dict[str, LocationData] = {
    # Jellyfish Basin
    "First Catch: Aurora Jellyfish":       LocationData(BASE_ID + 772, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Bursting Anglerfish":    LocationData(BASE_ID + 773, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Concertina Barracuda":   LocationData(BASE_ID + 774, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Entangled Crab":         LocationData(BASE_ID + 775, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Gazing Shark":           LocationData(BASE_ID + 776, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Imperious Lobster":      LocationData(BASE_ID + 777, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Parhelion Jellyfish":    LocationData(BASE_ID + 778, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Perished Loosejaw":      LocationData(BASE_ID + 779, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Radiant Squid":          LocationData(BASE_ID + 780, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Savage Barracuda":       LocationData(BASE_ID + 781, "Jellyfish Basin", "dlc_dredge"),
    "First Catch: Seizing Snailfish":      LocationData(BASE_ID + 782, "Jellyfish Basin", "dlc_dredge"),
    # Fog Coast
    "First Catch: Barbed Eel":             LocationData(BASE_ID + 783, "Fog Coast", "dlc_dredge"),
    "First Catch: Bloodskin Shark":        LocationData(BASE_ID + 784, "Fog Coast", "dlc_dredge"),
    "First Catch: Cerebral Crab":          LocationData(BASE_ID + 785, "Fog Coast", "dlc_dredge"),
    "First Catch: Fanged Cod":             LocationData(BASE_ID + 786, "Fog Coast", "dlc_dredge"),
    "First Catch: Grotesque Mackerel":     LocationData(BASE_ID + 787, "Fog Coast", "dlc_dredge"),
    "First Catch: Host Eel":               LocationData(BASE_ID + 788, "Fog Coast", "dlc_dredge"),
    "First Catch: Malignant Pincer":       LocationData(BASE_ID + 789, "Fog Coast", "dlc_dredge"),
    "First Catch: Many Eyed Mackerel":     LocationData(BASE_ID + 790, "Fog Coast", "dlc_dredge"),
    "First Catch: Sallow Sailfish":        LocationData(BASE_ID + 791, "Fog Coast", "dlc_dredge"),
    "First Catch: Three-Headed Cod":       LocationData(BASE_ID + 792, "Fog Coast", "dlc_dredge"),
    "First Catch: Tusked Grouper":         LocationData(BASE_ID + 793, "Fog Coast", "dlc_dredge"),
    "First Catch: Voltaic Grouper":        LocationData(BASE_ID + 794, "Fog Coast", "dlc_dredge"),
    # Black Cliff
    "First Catch: Bony Wreckfish":         LocationData(BASE_ID + 795, "Black Cliff", "dlc_dredge"),
    "First Catch: Cortex Decorator":       LocationData(BASE_ID + 796, "Black Cliff", "dlc_dredge"),
    "First Catch: Enthralled Stonefish":   LocationData(BASE_ID + 797, "Black Cliff", "dlc_dredge"),
    "First Catch: Gelatinous Stonefish":   LocationData(BASE_ID + 798, "Black Cliff", "dlc_dredge"),
    "First Catch: Gnashing Perch":         LocationData(BASE_ID + 799, "Black Cliff", "dlc_dredge"),
    "First Catch: Scouring Bass":          LocationData(BASE_ID + 1119, "Black Cliff", "dlc_dredge"),
    "First Catch: Shattered Wreckfish":    LocationData(BASE_ID + 1120, "Black Cliff", "dlc_dredge"),
    "First Catch: Splintered Crab":        LocationData(BASE_ID + 1121, "Black Cliff", "dlc_dredge"),
    "First Catch: Sprouting Eel":          LocationData(BASE_ID + 1122, "Black Cliff", "dlc_dredge"),
    "First Catch: Translucent Sturgeon":   LocationData(BASE_ID + 1123, "Black Cliff", "dlc_dredge"),
    "First Catch: Withered Ray":           LocationData(BASE_ID + 1124, "Black Cliff", "dlc_dredge"),
}

# === RESTAURANT MILESTONES ===
restaurant_milestones: Dict[str, LocationData] = {
    # Customer count
    "Serve 10 Customers": LocationData(BASE_ID + 200, "Bancho Sushi", "restaurant"),
    "Serve 50 Customers": LocationData(BASE_ID + 201, "Bancho Sushi", "restaurant"),
    "Serve 100 Customers": LocationData(BASE_ID + 202, "Bancho Sushi", "restaurant"),
    "Serve 250 Customers": LocationData(BASE_ID + 203, "Bancho Sushi", "restaurant"),
    "Serve 500 Customers": LocationData(BASE_ID + 204, "Bancho Sushi", "restaurant"),
    
    # Restaurant rating
    "Restaurant Rating: 3 Stars": LocationData(BASE_ID + 220, "Bancho Sushi", "restaurant"),
    "Restaurant Rating: 4 Stars": LocationData(BASE_ID + 221, "Bancho Sushi", "restaurant"),
    "Restaurant Rating: 5 Stars": LocationData(BASE_ID + 222, "Bancho Sushi", "restaurant"),
}

# === DISH UPGRADES ===
# Only "Menu" dishes have upgrade levels — sushi dishes are always level 1.
# Each dish generates checks from level 2 up to its max level.
# Format: "Upgrade [Dish Name] to Level N"
# Max levels sourced from the game wiki recipe table.

def _dish_upgrades(dish: str, max_level: int, base: int, category: str = "dish_upgrade") -> dict:
    """Generate upgrade locations for a dish from level 2 to max_level."""
    return {
        f"Upgrade {dish} to Level {lvl}": LocationData(base + (lvl - 2), "Bancho Sushi", category)
        for lvl in range(2, max_level + 1)
    }

# Assign base IDs in blocks of 15 (max possible upgrades per dish) starting at BASE_ID+2000
# Existing locations use up to BASE_ID+1118, so BASE_ID+2000 gives plenty of clearance.
# Each dish block: BASE_ID + 2000 + (dish_index * 15), giving room for 96+ dishes.
_D = BASE_ID + 2000
dish_upgrade_locations: Dict[str, LocationData] = {
    **_dish_upgrades("Agar Tokoroten",                        10, _D + 0*15),   # max 10 (was 7)
    **_dish_upgrades("Antarctic Octopus Carpaccio",           10, _D + 1*15),   # max 10 (was 7)
    **_dish_upgrades("Arctic Cod Risotto",                    10, _D + 2*15),   # max 10 (was 9)
    **_dish_upgrades("Atlantic Bonito Curry",                 10, _D + 3*15),   # max 10 (was 12)
    **_dish_upgrades("Batfish Ricebowl",                      10, _D + 4*15),   # max 10 (was 7)
    **_dish_upgrades("Big-Eyed Scad and Soybean Paste Roast", 10, _D + 5*15),  # max 10 (was 7)
    **_dish_upgrades("Black Vinegar Braised Parrotfish",      10, _D + 6*15),   # max 10 (was 6)
    **_dish_upgrades("Blobfish Spring Roll",                  10, _D + 7*15),   # max 10
    # Boiled Mantis Shrimp with Soy Paste: boss recipe, max 1 — skip (slot 8 reserved)
    **_dish_upgrades("Boiled Porbeagle Shark",                10, _D + 9*15),   # max 10 (was 7)
    **_dish_upgrades("Boiled Sailfish and Seaweed",           10, _D + 10*15),  # max 10 (was 9)
    **_dish_upgrades("Boiled Yellowback Fusilier",            10, _D + 11*15),  # max 10 (was 7)
    **_dish_upgrades("Boiled and Deep-Fried White Shrimp",   10, _D + 12*15),  # max 10
    **_dish_upgrades("Bluefin Tuna Rice Bowl",                10, _D + 13*15),  # max 10 (was 9)
    **_dish_upgrades("Comber Sandwich",                       10, _D + 14*15),  # max 10 (was 6)
    **_dish_upgrades("Crimson Fish Roll",                     10, _D + 15*15),  # max 10 (was 9)
    **_dish_upgrades("Crystal Lobster Roll",                  10, _D + 16*15),  # max 10 (was 9)
    **_dish_upgrades("Deep Fish Tempura",                     10, _D + 17*15),  # max 10 (was 7)
    **_dish_upgrades("Deep Sea Kaiju Ramen",                  10, _D + 18*15, "dlc_godzilla"),  # max 10 (was 6) — Godzilla DLC
    **_dish_upgrades("Deep-Fried Eggplant Shrimp Meatballs", 10, _D + 19*15),  # max 10 (was 7)
    **_dish_upgrades("Deep-Fried Red Lionfish",               10, _D + 20*15),  # max 10 (was 4)
    **_dish_upgrades("Deep-Fried Vegetables",                 10, _D + 21*15),  # max 10 (was 3)
    **_dish_upgrades("Dried Stingray",                        10, _D + 22*15),  # max 10 (was 12)
    **_dish_upgrades("Dumbo Takoyaki",                        10, _D + 23*15),  # max 10 (was 9)
    **_dish_upgrades("Dusky Grouper Steak",                   10, _D + 24*15),  # max 10 (was 7)
    **_dish_upgrades("Eggplant Soba Oyaki",                   10, _D + 25*15, "dlc_ichiban"),  # max 10 (was 9)
    **_dish_upgrades("Falcatus Soybean Paste Soup",           10, _D + 26*15),  # max 10 (was 7)
    **_dish_upgrades("Fried Habanero Fangtooth",              10, _D + 27*15),  # max 10 (was 7)
    **_dish_upgrades("Fried Onion Cuttlefish",                10, _D + 28*15),  # max 10 (was 7)
    **_dish_upgrades("Fried Rice with Sally Lightfoot Crab", 10, _D + 29*15),  # max 10
    **_dish_upgrades("Fried Seahorses",                       10, _D + 30*15),  # max 10 (was 4)
    **_dish_upgrades("Fried Tomato and Snailfish",            10, _D + 31*15),  # max 10 (was 7)
    # Goblin Shark Belly Roast: boss recipe, max 1 — skip (slot 32 reserved)
    **_dish_upgrades("Great Barracuda Canape",                10, _D + 33*15),  # max 10 (was 6)
    **_dish_upgrades("Great Spider Crab Curry",               10, _D + 34*15),  # max 10 (was 9)
    **_dish_upgrades("Hawaiian Poke",                         10, _D + 35*15),  # max 10 (was 9)
    **_dish_upgrades("Hot Pepper Tuna",                       10, _D + 36*15),  # max 10 (was 7)
    **_dish_upgrades("Humboldt Ink Pasta",                    10, _D + 37*15),  # max 10
    **_dish_upgrades("Humphead Parrotfish Curry",             10, _D + 38*15),  # max 10 (was 6)
    **_dish_upgrades("Ice Fish Curry",                        10, _D + 39*15),  # max 10 (was 9)
    **_dish_upgrades("Latok Omelet",                          10, _D + 40*15),  # max 10 (was 9)
    **_dish_upgrades("Mackerel Scad Hotdog",                  10, _D + 41*15),  # max 10 (was 6)
    **_dish_upgrades("Marlin and Soybean Paste Roast",        10, _D + 42*15),  # max 10 (was 9)
    **_dish_upgrades("Mianbao Xia",                           10, _D + 43*15),  # max 10
    **_dish_upgrades("Moray Eel Curry",                       10, _D + 44*15),  # max 10 (was 6)
    **_dish_upgrades("Narrow-barred Spanish Mackerel Arancini", 10, _D + 45*15), # max 10 (was 7)
    **_dish_upgrades("Narwhal Miso Soup",                     10, _D + 46*15),  # max 10 (was 12)
    **_dish_upgrades("Nasu Dengaku",                          10, _D + 47*15),  # max 10 (was 4)
    **_dish_upgrades("Peacock Squid Ripieni",                 10, _D + 48*15),  # max 10 (was 7)
    **_dish_upgrades("Pelican Eel Jelly",                     10, _D + 49*15),  # max 10 (was 7)
    # Phantom Jellyfish Jelly: boss recipe, max 1 — skip (slot 50 reserved)
    **_dish_upgrades("Pickled Vegetables",                    10, _D + 51*15),  # max 10 (was 3)
    **_dish_upgrades("Pikaia Ramen",                          10, _D + 52*15),  # max 10
    **_dish_upgrades("Plotosid Pie",                          10, _D + 53*15),  # max 10 (was 7)
    **_dish_upgrades("Rice with Great Spider Crab Meat",      10, _D + 54*15),  # max 10 (was 7)
    **_dish_upgrades("Rice with Purple Sea Urchin Sushi",     10, _D + 55*15),  # max 10 (was 4)
    **_dish_upgrades("Rice with White Shrimp Meat",           10, _D + 56*15),  # max 10 (was 9)
    **_dish_upgrades("Roasted Capelin",                       10, _D + 57*15),  # max 10 (was 12)
    # Roasted Helicoprion Tail: boss recipe, max 1 — skip (slot 58 reserved)
    **_dish_upgrades("Roasted Tropical Fish and Garlic",      10, _D + 59*15),  # max 10 (was 9)
    **_dish_upgrades("Salt-grilled Redtoothed Triggerfish",   10, _D + 60*15),  # max 10 (was 6)
    **_dish_upgrades("Seahorse Salad",                        10, _D + 61*15),  # max 10 (was 6)
    # Seahorse Skewers: max 1 — skip (slot 62 reserved)
    **_dish_upgrades("Seahorse Udon",                         10, _D + 63*15),  # max 10 (was 4)
    **_dish_upgrades("Seasoned Jellyfish",                    10, _D + 64*15),  # max 10 (was 6)
    **_dish_upgrades("Seasoned Kajime",                       10, _D + 65*15),  # max 10 (was 6)
    **_dish_upgrades("Seasoned Long-spine Porcupinefish Skin", 10, _D + 66*15), # max 10 (was 7)
    **_dish_upgrades("Seasoned Waptia Fieldensis",            10, _D + 67*15),  # max 10 (was 7)
    **_dish_upgrades("Seaweed Rolled Omelet",                 10, _D + 68*15),  # max 10 (was 9)
    **_dish_upgrades("Shark Karaage",                         10, _D + 69*15),  # max 10 (was 9)
    **_dish_upgrades("Smallspotted Dart Kajime Soup",         10, _D + 70*15),  # max 10 (was 7)
    **_dish_upgrades("Smoked Atlantic Mackerel Scramble",     10, _D + 71*15),  # max 10 (was 6)
    **_dish_upgrades("Spear Squid Soba Futomaki",             10, _D + 72*15, "dlc_ichiban"),  # max 10 (was 9)
    **_dish_upgrades("Special Fried Shrimp Sushi",            10, _D + 73*15),  # max 10 (was 1 — spreadsheet confirmed)
    # Steamed Kronosaurus Tongue: boss recipe, max 1 — skip (slot 74 reserved)
    # Steamed Wolf Eel: boss recipe, max 1 — skip (slot 75 reserved)
    **_dish_upgrades("Stellate Puffer Nicogori",              10, _D + 76*15),  # max 10 (was 7)
    **_dish_upgrades("Stingray Sashimi Cold Noodles",         10, _D + 77*15, "dlc_ichiban"),  # max 10 (was 9)
    # Stir-Fried Hermit Crab and Seaweed: boss recipe, max 1 — skip (slot 78 reserved)
    **_dish_upgrades("Stir-fried Habanero Lobster",           10, _D + 79*15),  # max 10 (was 7)
    **_dish_upgrades("Striped Red Mullet Tangle Roll",        10, _D + 80*15),  # max 10 (was 7)
    **_dish_upgrades("Sweet and Sour Stargazer",              10, _D + 81*15),  # max 10 (was 6)
    **_dish_upgrades("Three-Colored Squid Roast",             10, _D + 82*15),  # max 10 (was 12)
    **_dish_upgrades("Tomato Egg Soup",                       10, _D + 83*15),  # max 10 (was 12)
    **_dish_upgrades("Trevally Nanbanzuke",                   10, _D + 84*15),  # max 10 (was 7)
    **_dish_upgrades("Trevally Sandwich",                     10, _D + 85*15),  # max 10 (was 7)
    **_dish_upgrades("Tropical Fish Sushi Set",               10, _D + 86*15),  # max 10 (was 9)
    **_dish_upgrades("Trout Sea Grapes Ricebowl",             10, _D + 87*15),  # max 10 (was 7)
    **_dish_upgrades("Vegetable Sushi",                       10, _D + 88*15),  # max 10 (was 1 — spreadsheet confirmed)
    # White Shark Omelet: boss recipe, max 1 — skip (slot 89 reserved)
    **_dish_upgrades("White Trevally Kombu Ochazuke",         10, _D + 90*15),  # max 10 (was 7)
    **_dish_upgrades("Whole-Roasted Shark Head",              10, _D + 91*15),  # max 10 (was 7)
    **_dish_upgrades("Wrasse Curry",                          10, _D + 92*15),  # max 10 (was 6)
    # Yawie Steamed Meat: boss recipe, max 1 — skip (slot 93 reserved)
    **_dish_upgrades("Yellowfin Tuna Steak",                  10, _D + 94*15),  # max 10 (was 9)
    # --- Ichiban DLC dishes ---
    **_dish_upgrades("Warm Atlantic Mackerel Soba",           10, _D + 95*15, "dlc_ichiban"),  # max 10 (was 9)
    # --- Godzilla DLC dishes ---
    **_dish_upgrades("Godzilla vs. Ebirah Curry",             10, _D + 96*15, "dlc_godzilla"),  # max 10 (was 9)
    **_dish_upgrades("Ebirah Chasing Sashimi",                10, _D + 97*15, "dlc_godzilla"),  # max 10 (was 9)
    # --- Missing cooked dishes (added from spreadsheet) ---
    # NOTE: slots 98-99 still fit under _D (end at BASE_ID+3493, _SA starts at BASE_ID+3500).
    # Slots 100+ would collide with _SA (BASE_ID+3500), so they use _D2 = BASE_ID+6000 instead.
    **_dish_upgrades("Great Spider Crab and Cucumber Sushi",  10, _D + 98*15),   # max 10
    **_dish_upgrades("Grilled Eel with Habanero",             10, _D + 99*15),   # max 10
}
# Filter out any entries that somehow have no upgrades (defensive)
dish_upgrade_locations = {k: v for k, v in dish_upgrade_locations.items()}

# === DISH UPGRADES OVERFLOW — _D2 block ===
# _D slots 100+ collide with _SA (BASE_ID+3500), so overflow entries use a higher base.
# Known occupied ranges (all relative to BASE_ID):
#   +2000..+3493  : _D dish upgrade slots 0–99
#   +3500..+3979  : _SA staff all-levels
#   +4000..+~6000 : _J jungle locations
#   +8000..+8097  : dish_upgrade_items in items.py (ITEM_BASE+3000..+3097)
#   +10000..+10118: recipe_unlock_locations extended sushi unlocks
# _D2 must start at BASE_ID+12000 to be safely clear of all the above.
# _D2 range: BASE_ID+12000 .. BASE_ID+12000+151*15+14 = BASE_ID+14279.
# Layout (each slot = 15 IDs wide):
#   Slots   0– 9: remaining cooked dishes (overflow from _D)
#   Slot   10:    reserved (Clione Queen Soup, boss — no upgrades)
#   Slots  11–17: Truffle (VIP) dishes, max level 5
#   Slots  18–143: Base game sushi 8050xxx (~110 dishes, max level 10)
#   Slots 144–151: Tuna Bar sushi 8052xxx (8 dishes, max level 10)
_D2 = BASE_ID + 12000

dish_upgrade_locations.update({
    # --- Remaining cooked dishes (overflow from _D, slots 0–9 of _D2) ---
    **_dish_upgrades("Haddock Acqua Pazza",                   10, _D2 + 0*15),   # max 10
    **_dish_upgrades("Lobster Platter",                       10, _D2 + 1*15),   # max 10
    **_dish_upgrades("Moonlight Bladderwrack Roll",           10, _D2 + 2*15),   # max 10
    **_dish_upgrades("Pufferfish Dumpling Soup",              10, _D2 + 3*15),   # max 10
    **_dish_upgrades("Seagrapes Jellyfish Sushi",             10, _D2 + 4*15),   # max 10
    **_dish_upgrades("Seagrapes Special Sushi",               10, _D2 + 5*15),   # max 10
    **_dish_upgrades("Sea Toad and Cucumber Gunkan Sushi",    10, _D2 + 6*15),   # max 10
    **_dish_upgrades("Skewered Cucumber",                     10, _D2 + 7*15),   # max 10
    **_dish_upgrades("Soy Sauce Marinated Crab",              10, _D2 + 8*15),   # max 10
    **_dish_upgrades("Stellate Puffer Special Sushi",         10, _D2 + 9*15),   # max 10
    # Slot 10 reserved: Clione Queen Soup (boss recipe, max 1 — no upgrades)
    # --- Truffle (VIP) dishes — max level 5 (slots 11–17) ---
    **_dish_upgrades("Boiled Asian Sheepshead Wrasse & Truffle", 5, _D2 + 11*15),  # max 5
    **_dish_upgrades("Grilled Antarctic Octopus & Truffle",    5, _D2 + 12*15),   # max 5
    **_dish_upgrades("Hyalonema Tuna Sashimi",                 5, _D2 + 13*15),   # max 5
    **_dish_upgrades("Steamed Hyalonema Angler Fish",          5, _D2 + 14*15),   # max 5
    **_dish_upgrades("Truffle Blue Lobster Tail Sushi",        5, _D2 + 15*15),   # max 5
    **_dish_upgrades("Truffle Sailfish Tartare",               5, _D2 + 16*15),   # max 5
    **_dish_upgrades("Truffle Shark Sandwich",                 5, _D2 + 17*15),   # max 5
    # --- Base game sushi 8050xxx — max level 10 (slots 18–127) ---
    # Skipped TIDs (not in spreadsheet, no upgrades): 8050052 (Norimaki), 8050101 (Swordfish Sushi),
    #   8050111 (Young Anomalocaris Sushi), 8050121 (Blackfin Barracuda Sushi variant)
    # Also skipped: 8050010 (gap), 8050028 (gap), 8050039-41 (gaps), 8050058 (gap), 8050118 (gap)
    **_dish_upgrades("Clownfish Sushi",                        10, _D2 + 18*15),  # 8050001
    **_dish_upgrades("Comber Sushi",                           10, _D2 + 19*15),  # 8050002
    **_dish_upgrades("Cardinalfish Sushi",                     10, _D2 + 20*15),  # 8050003
    **_dish_upgrades("Sea Goldie Sushi",                       10, _D2 + 21*15),  # 8050004
    **_dish_upgrades("Pyramid Butterflyfish Sushi",            10, _D2 + 22*15),  # 8050005
    **_dish_upgrades("Yellow Tang Sushi",                      10, _D2 + 23*15),  # 8050006
    **_dish_upgrades("Salema Porgy Sushi",                     10, _D2 + 24*15),  # 8050007
    **_dish_upgrades("Orbicular Batfish Fry",                  10, _D2 + 25*15),  # 8050008
    **_dish_upgrades("Blue Tang Sushi",                        10, _D2 + 26*15),  # 8050009
    **_dish_upgrades("Rainbow Wrasse Sushi",                   10, _D2 + 27*15),  # 8050011
    **_dish_upgrades("Lagoon Triggerfish Sushi",               10, _D2 + 28*15),  # 8050012
    **_dish_upgrades("Smallspotted Dart Sushi",                10, _D2 + 29*15),  # 8050013
    **_dish_upgrades("Yellowback Fusilier Sushi",              10, _D2 + 30*15),  # 8050014
    **_dish_upgrades("Ornate Wrasse Sushi",                    10, _D2 + 31*15),  # 8050015
    **_dish_upgrades("Longfin Batfish Sushi",                  10, _D2 + 32*15),  # 8050016
    **_dish_upgrades("Mediterranean Parrotfish Sushi",         10, _D2 + 33*15),  # 8050017
    **_dish_upgrades("Redtoothed Triggerfish Sushi",           10, _D2 + 34*15),  # 8050018
    **_dish_upgrades("B&W Snapper Sushi",                      10, _D2 + 35*15),  # 8050019
    **_dish_upgrades("Green Humphead Parrotfish Sushi",        10, _D2 + 36*15),  # 8050020
    **_dish_upgrades("Red Lionfish Sushi",                     10, _D2 + 37*15),  # 8050021
    **_dish_upgrades("Bluehead Tilefish Sushi",                10, _D2 + 38*15),  # 8050022
    **_dish_upgrades("Clown Frogfish Sushi",                   10, _D2 + 39*15),  # 8050023
    **_dish_upgrades("Painted Comber Sushi",                   10, _D2 + 40*15),  # 8050024
    **_dish_upgrades("Humphead Parrotfish Sushi",              10, _D2 + 41*15),  # 8050025
    **_dish_upgrades("Bigeye Scad Sushi",                      10, _D2 + 42*15),  # 8050026
    **_dish_upgrades("Striped Red Mullet Sushi",               10, _D2 + 43*15),  # 8050027
    **_dish_upgrades("Harlequin Hind Sushi",                   10, _D2 + 44*15),  # 8050029
    **_dish_upgrades("Bigeye Trevally Sushi",                  10, _D2 + 45*15),  # 8050030
    **_dish_upgrades("Coral Trout Sushi",                      10, _D2 + 46*15),  # 8050031
    **_dish_upgrades("Grey Triggerfish Sushi",                 10, _D2 + 47*15),  # 8050032
    **_dish_upgrades("Atlantic Bonito Sushi",                  10, _D2 + 48*15),  # 8050033
    **_dish_upgrades("Atlantic Mackerel Sushi",                10, _D2 + 49*15),  # 8050034
    **_dish_upgrades("White Trevally Sushi",                   10, _D2 + 50*15),  # 8050035
    **_dish_upgrades("Cuttlefish Sushi",                       10, _D2 + 51*15),  # 8050036
    **_dish_upgrades("Dusky Grouper Sushi",                    10, _D2 + 52*15),  # 8050037
    **_dish_upgrades("Narrow-barred Spanish Mackerel Sushi",   10, _D2 + 53*15),  # 8050038
    **_dish_upgrades("Giant Trevally Sushi",                   10, _D2 + 54*15),  # 8050042
    **_dish_upgrades("Blackfin Barracuda Sushi",               10, _D2 + 55*15),  # 8050043
    **_dish_upgrades("Whitetip Reefshark Sushi",               10, _D2 + 56*15),  # 8050044
    **_dish_upgrades("Tiger Shark Sushi",                      10, _D2 + 57*15),  # 8050045
    **_dish_upgrades("Barrel Jellyfish Sushi",                 10, _D2 + 58*15),  # 8050046
    **_dish_upgrades("Fried Egg Jellyfish Sushi",              10, _D2 + 59*15),  # 8050047
    **_dish_upgrades("White Spotted Jellyfish Sushi",          10, _D2 + 60*15),  # 8050048
    **_dish_upgrades("Great Barracuda Sushi",                  10, _D2 + 61*15),  # 8050049
    **_dish_upgrades("Mackerel Scad Sushi",                    10, _D2 + 62*15),  # 8050050
    **_dish_upgrades("Titan Triggerfish Sushi",                10, _D2 + 63*15),  # 8050051
    **_dish_upgrades("Longnose Sawshark Sushi",                10, _D2 + 64*15),  # 8050053
    **_dish_upgrades("Chambered Nautilus Sushi",               10, _D2 + 65*15),  # 8050054
    **_dish_upgrades("Fangtooth Sushi",                        10, _D2 + 66*15),  # 8050055
    **_dish_upgrades("Frilled Shark Sushi",                    10, _D2 + 67*15),  # 8050056
    **_dish_upgrades("Bluespotted Stargazer Sushi",            10, _D2 + 68*15),  # 8050057
    **_dish_upgrades("Rhinochimaeridae Sushi",                 10, _D2 + 69*15),  # 8050059
    **_dish_upgrades("Spider Crab Sushi",                      10, _D2 + 70*15),  # 8050060
    **_dish_upgrades("Megamouth Shark Sushi",                  10, _D2 + 71*15),  # 8050061
    **_dish_upgrades("Cookiecutter Shark Sushi",               10, _D2 + 72*15),  # 8050062
    **_dish_upgrades("Sea Toad Sushi",                         10, _D2 + 73*15),  # 8050063
    **_dish_upgrades("Salmon Snailfish Sushi",                 10, _D2 + 74*15),  # 8050064
    **_dish_upgrades("Pacific Fanfish Sushi",                  10, _D2 + 75*15),  # 8050065
    **_dish_upgrades("Threetooth Puffer Sushi",                10, _D2 + 76*15),  # 8050066
    **_dish_upgrades("Red Bream Sushi",                        10, _D2 + 77*15),  # 8050067
    **_dish_upgrades("Atlantic Anglerfish Sushi",              10, _D2 + 78*15),  # 8050068
    **_dish_upgrades("Comb Jelly Sushi",                       10, _D2 + 79*15),  # 8050069
    **_dish_upgrades("Blood-belly Comb Jelly Sushi",           10, _D2 + 80*15),  # 8050070
    **_dish_upgrades("Blacktip Reefshark Sushi",               10, _D2 + 81*15),  # 8050071
    **_dish_upgrades("Copper Shark Sushi",                     10, _D2 + 82*15),  # 8050072
    **_dish_upgrades("Box Jellyfish Sushi",                    10, _D2 + 83*15),  # 8050073
    **_dish_upgrades("Moray Eel Sushi",                        10, _D2 + 84*15),  # 8050074
    **_dish_upgrades("Sally Lightfoot Crab Sushi",             10, _D2 + 85*15),  # 8050075
    **_dish_upgrades("Peacock Squid Sushi",                    10, _D2 + 86*15),  # 8050076
    **_dish_upgrades("Dumbo Octopus Sushi",                    10, _D2 + 87*15),  # 8050077
    **_dish_upgrades("Barreleye Sushi",                        10, _D2 + 88*15),  # 8050078
    **_dish_upgrades("Blobfish Sushi",                         10, _D2 + 89*15),  # 8050079
    **_dish_upgrades("Vampire Squid Sushi",                    10, _D2 + 90*15),  # 8050080
    **_dish_upgrades("Arctic Cod Sushi",                       10, _D2 + 91*15),  # 8050081
    **_dish_upgrades("Gelatinous Snailfish Sushi",             10, _D2 + 92*15),  # 8050082
    **_dish_upgrades("Antarctic Octopus Sushi",                10, _D2 + 93*15),  # 8050083
    **_dish_upgrades("Greenland Shark Sushi",                  10, _D2 + 94*15),  # 8050084
    **_dish_upgrades("Polar Eelpout Sushi",                    10, _D2 + 95*15),  # 8050085
    **_dish_upgrades("Porbeagle Shark Sushi",                  10, _D2 + 96*15),  # 8050086
    **_dish_upgrades("Ice Fish Sushi",                         10, _D2 + 97*15),  # 8050087
    **_dish_upgrades("Capelin Sushi",                          10, _D2 + 98*15),  # 8050088
    **_dish_upgrades("Narwhal Sushi",                          10, _D2 + 99*15),  # 8050089
    **_dish_upgrades("Haddock Sushi",                          10, _D2 + 100*15), # 8050090
    **_dish_upgrades("Starry Skate Sushi",                     10, _D2 + 101*15), # 8050091
    **_dish_upgrades("Shortfin Mako Sushi",                    10, _D2 + 102*15), # 8050092
    **_dish_upgrades("Thresher Shark Sushi",                   10, _D2 + 103*15), # 8050093
    **_dish_upgrades("Smooth Hammerhead Sushi",                10, _D2 + 104*15), # 8050094
    **_dish_upgrades("Zebra Shark Sushi",                      10, _D2 + 105*15), # 8050095
    **_dish_upgrades("Pelican Eel Sushi",                      10, _D2 + 106*15), # 8050096
    **_dish_upgrades("White Shrimp Sushi",                     10, _D2 + 107*15), # 8050097
    **_dish_upgrades("Humboldt Squid Sushi",                   10, _D2 + 108*15), # 8050098
    **_dish_upgrades("Devil Scorpionfish Sushi",               10, _D2 + 109*15), # 8050099
    **_dish_upgrades("Marlin Sushi",                           10, _D2 + 110*15), # 8050100
    **_dish_upgrades("Sailfish Sushi",                         10, _D2 + 111*15), # 8050102
    **_dish_upgrades("Waptia Sushi",                           10, _D2 + 112*15), # 8050103
    **_dish_upgrades("Pikaia Sushi",                           10, _D2 + 113*15), # 8050104
    **_dish_upgrades("Allenypterus Sushi",                     10, _D2 + 114*15), # 8050105
    **_dish_upgrades("Qingmenodus Sushi",                      10, _D2 + 115*15), # 8050106
    **_dish_upgrades("Falcatus Sushi",                         10, _D2 + 116*15), # 8050107
    **_dish_upgrades("Drepanaspis Sushi",                      10, _D2 + 117*15), # 8050108
    **_dish_upgrades("Dunkleosteus Sushi",                     10, _D2 + 118*15), # 8050109
    **_dish_upgrades("Megalograptus Sushi",                    10, _D2 + 119*15), # 8050110
    **_dish_upgrades("Seadragon Onigiri",                      10, _D2 + 120*15), # 8050112
    **_dish_upgrades("Arctic Telescope Fish Sushi",            10, _D2 + 121*15), # 8050113
    **_dish_upgrades("Alaska Pollock Sushi",                   10, _D2 + 122*15), # 8050114
    **_dish_upgrades("Lumpfish Sushi",                         10, _D2 + 123*15), # 8050115
    **_dish_upgrades("Snub-nosed Spiny Eel Sushi",             10, _D2 + 124*15), # 8050116
    **_dish_upgrades("Xenacanthus Sushi",                      10, _D2 + 125*15), # 8050117
    **_dish_upgrades("Longspine Squirrelfish Sushi",           10, _D2 + 126*15), # 8050119
    **_dish_upgrades("Clearfin Lionfish Sushi",                10, _D2 + 127*15), # 8050120
    **_dish_upgrades("Spear Squid Sushi",                      10, _D2 + 128*15), # 8050122
    **_dish_upgrades("Red-banded Lobster Sushi",               10, _D2 + 129*15), # 8050123
    **_dish_upgrades("American Lobster Sushi",                 10, _D2 + 130*15), # 8050124
    **_dish_upgrades("Blue Lobster Sushi",                     10, _D2 + 131*15), # 8050125
    **_dish_upgrades("California Spiny Lobster Sushi",         10, _D2 + 132*15), # 8050126
    **_dish_upgrades("Fan Lobster Sushi",                      10, _D2 + 133*15), # 8050127
    **_dish_upgrades("Norway Lobster Sushi",                   10, _D2 + 134*15), # 8050128
    **_dish_upgrades("Golden King Crab Sushi",                 10, _D2 + 135*15), # 8050129
    **_dish_upgrades("Snow Crab Sushi",                        10, _D2 + 136*15), # 8050130
    **_dish_upgrades("Horsehair Crab Sushi",                   10, _D2 + 137*15), # 8050131
    **_dish_upgrades("European Lobster Sushi",                 10, _D2 + 138*15), # 8050132
    **_dish_upgrades("Tropical Rock Lobster Sushi",            10, _D2 + 139*15), # 8050133
    **_dish_upgrades("Crystal Lobster Sushi",                  10, _D2 + 140*15), # 8050134
    **_dish_upgrades("Eastern Rock Lobster Sushi",             10, _D2 + 141*15), # 8050135
    **_dish_upgrades("Dollocaris Ingens Sushi",                10, _D2 + 142*15), # 8050136
    **_dish_upgrades("Tokummia Katalepsis Sushi",              10, _D2 + 143*15), # 8050137
    # --- Tuna Bar sushi 8052xxx — max level 10 (slots 144–154) ---
    # Note: 8052007 (Bluefin Tuna Rice Bowl), 8052008 (Hawaiian Poke), 8052009 (Yellowfin Tuna Steak)
    #   are already in dish_upgrade_locations under _D (slots 13, 35, 94).
    **_dish_upgrades("Bluefin Tuna Akami Sushi",               10, _D2 + 144*15), # 8052001
    **_dish_upgrades("Bluefin Tuna Chutoro Sushi",             10, _D2 + 145*15), # 8052002
    **_dish_upgrades("Bluefin Tuna Ootoro Sushi",              10, _D2 + 146*15), # 8052003
    **_dish_upgrades("Yellowfin Tuna Akami Sushi",             10, _D2 + 147*15), # 8052004
    **_dish_upgrades("Yellowfin Tuna Chutoro Sushi",           10, _D2 + 148*15), # 8052005
    **_dish_upgrades("Yellowfin Tuna Ootoro Sushi",            10, _D2 + 149*15), # 8052006
    **_dish_upgrades("Raw Black Tiger Shrimp Sushi",           10, _D2 + 150*15), # 8052011
    **_dish_upgrades("Cooked Whiteleg Shrimp Sushi",           10, _D2 + 151*15), # 8052012
})

# === RECIPE UNLOCKS ===
recipe_unlock_locations: Dict[str, LocationData] = {
    # --- All fish sushi (unlocked by catching the fish, 8050xxx) ---
    # Previously only hand-picked "key" sushi were here; now ALL are included.
    # IDs 800-817 are the original hand-picked ones (kept for backward compat).
    # IDs 900-999 and 1000+ are the remaining sushi dishes.
    "Unlock Recipe: Yellowfin Tuna Akami Sushi": LocationData(BASE_ID + 800, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Alaska Pollock Sushi":        LocationData(BASE_ID + 801, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Antarctic Octopus Sushi":     LocationData(BASE_ID + 802, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Arctic Cod Sushi":            LocationData(BASE_ID + 803, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Atlantic Anglerfish Sushi":   LocationData(BASE_ID + 804, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blobfish Sushi":              LocationData(BASE_ID + 805, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blue Tang Sushi":             LocationData(BASE_ID + 806, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Clownfish Sushi":             LocationData(BASE_ID + 807, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Great Barracuda Sushi":       LocationData(BASE_ID + 808, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Greenland Shark Sushi":       LocationData(BASE_ID + 809, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humboldt Squid Sushi":        LocationData(BASE_ID + 810, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Marlin Sushi":                LocationData(BASE_ID + 811, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Narwhal Sushi":               LocationData(BASE_ID + 812, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Tiger Shark Sushi":           LocationData(BASE_ID + 813, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Vampire Squid Sushi":         LocationData(BASE_ID + 814, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Zebra Shark Sushi":           LocationData(BASE_ID + 815, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Comber Sushi":                LocationData(BASE_ID + 816, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humphead Parrotfish Sushi":   LocationData(BASE_ID + 817, "Bancho Sushi", "recipe"),
    # --- Remaining base game sushi (8050xxx, not previously listed) ---
    "Unlock Recipe: Sea Goldie Sushi":                    LocationData(BASE_ID + 10000, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Cardinalfish Sushi":                  LocationData(BASE_ID + 10001, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Rainbow Wrasse Sushi":                LocationData(BASE_ID + 10002, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Salema Porgy Sushi":                  LocationData(BASE_ID + 10003, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Yellow Tang Sushi":                   LocationData(BASE_ID + 10004, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pyramid Butterflyfish Sushi":         LocationData(BASE_ID + 10005, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Orbicular Batfish Fry":               LocationData(BASE_ID + 10006, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Lagoon Triggerfish Sushi":            LocationData(BASE_ID + 10007, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Smallspotted Dart Sushi":             LocationData(BASE_ID + 10008, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Yellowback Fusilier Sushi":           LocationData(BASE_ID + 10009, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Ornate Wrasse Sushi":                 LocationData(BASE_ID + 10010, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Longfin Batfish Sushi":               LocationData(BASE_ID + 10011, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Mediterranean Parrotfish Sushi":      LocationData(BASE_ID + 10012, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Redtoothed Triggerfish Sushi":        LocationData(BASE_ID + 10013, "Bancho Sushi", "recipe"),
    "Unlock Recipe: B&W Snapper Sushi":                   LocationData(BASE_ID + 10014, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Green Humphead Parrotfish Sushi":     LocationData(BASE_ID + 10015, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Red Lionfish Sushi":                  LocationData(BASE_ID + 10016, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Bluehead Tilefish Sushi":             LocationData(BASE_ID + 10017, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Clown Frogfish Sushi":                LocationData(BASE_ID + 10018, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Painted Comber Sushi":                LocationData(BASE_ID + 10019, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Bigeye Scad Sushi":                   LocationData(BASE_ID + 10020, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Striped Red Mullet Sushi":            LocationData(BASE_ID + 10021, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Harlequin Hind Sushi":                LocationData(BASE_ID + 10022, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Bigeye Trevally Sushi":               LocationData(BASE_ID + 10023, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Coral Trout Sushi":                   LocationData(BASE_ID + 10024, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Grey Triggerfish Sushi":              LocationData(BASE_ID + 10025, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Atlantic Bonito Sushi":               LocationData(BASE_ID + 10026, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Atlantic Mackerel Sushi":             LocationData(BASE_ID + 10027, "Bancho Sushi", "recipe"),
    "Unlock Recipe: White Trevally Sushi":                LocationData(BASE_ID + 10028, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Cuttlefish Sushi":                    LocationData(BASE_ID + 10029, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dusky Grouper Sushi":                 LocationData(BASE_ID + 10030, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Narrow-barred Spanish Mackerel Sushi": LocationData(BASE_ID + 10031, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Giant Trevally Sushi":                LocationData(BASE_ID + 10032, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blackfin Barracuda Sushi":            LocationData(BASE_ID + 10033, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Whitetip Reefshark Sushi":            LocationData(BASE_ID + 10034, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Barrel Jellyfish Sushi":              LocationData(BASE_ID + 10035, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fried Egg Jellyfish Sushi":           LocationData(BASE_ID + 10036, "Bancho Sushi", "recipe"),
    "Unlock Recipe: White Spotted Jellyfish Sushi":       LocationData(BASE_ID + 10037, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Mackerel Scad Sushi":                 LocationData(BASE_ID + 10038, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Titan Triggerfish Sushi":             LocationData(BASE_ID + 10039, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Norimaki":                            LocationData(BASE_ID + 10040, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Longnose Sawshark Sushi":             LocationData(BASE_ID + 10041, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Chambered Nautilus Sushi":            LocationData(BASE_ID + 10042, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fangtooth Sushi":                     LocationData(BASE_ID + 10043, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Frilled Shark Sushi":                 LocationData(BASE_ID + 10044, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Bluespotted Stargazer Sushi":         LocationData(BASE_ID + 10045, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Rhinochimaeridae Sushi":              LocationData(BASE_ID + 10046, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Spider Crab Sushi":                   LocationData(BASE_ID + 10047, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Megamouth Shark Sushi":               LocationData(BASE_ID + 10048, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Cookiecutter Shark Sushi":            LocationData(BASE_ID + 10049, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Sea Toad Sushi":                      LocationData(BASE_ID + 10050, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Salmon Snailfish Sushi":              LocationData(BASE_ID + 10051, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pacific Fanfish Sushi":               LocationData(BASE_ID + 10052, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Threetooth Puffer Sushi":             LocationData(BASE_ID + 10053, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Red Bream Sushi":                     LocationData(BASE_ID + 10054, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Comb Jelly Sushi":                    LocationData(BASE_ID + 10055, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blood-belly Comb Jelly Sushi":        LocationData(BASE_ID + 10056, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blacktip Reefshark Sushi":            LocationData(BASE_ID + 10057, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Copper Shark Sushi":                  LocationData(BASE_ID + 10058, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Box Jellyfish Sushi":                 LocationData(BASE_ID + 10059, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Moray Eel Sushi":                     LocationData(BASE_ID + 10060, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Sally Lightfoot Crab Sushi":          LocationData(BASE_ID + 10061, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Peacock Squid Sushi":                 LocationData(BASE_ID + 10062, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dumbo Octopus Sushi":                 LocationData(BASE_ID + 10063, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Barreleye Sushi":                     LocationData(BASE_ID + 10064, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Gelatinous Snailfish Sushi":          LocationData(BASE_ID + 10065, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Polar Eelpout Sushi":                 LocationData(BASE_ID + 10066, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Porbeagle Shark Sushi":               LocationData(BASE_ID + 10067, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Ice Fish Sushi":                      LocationData(BASE_ID + 10068, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Capelin Sushi":                       LocationData(BASE_ID + 10069, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Haddock Sushi":                       LocationData(BASE_ID + 10070, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Starry Skate Sushi":                  LocationData(BASE_ID + 10071, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Shortfin Mako Sushi":                 LocationData(BASE_ID + 10072, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Thresher Shark Sushi":                LocationData(BASE_ID + 10073, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Smooth Hammerhead Sushi":             LocationData(BASE_ID + 10074, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pelican Eel Sushi":                   LocationData(BASE_ID + 10075, "Bancho Sushi", "recipe"),
    "Unlock Recipe: White Shrimp Sushi":                  LocationData(BASE_ID + 10076, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Devil Scorpionfish Sushi":            LocationData(BASE_ID + 10077, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Swordfish Sushi":                     LocationData(BASE_ID + 10078, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Sailfish Sushi":                      LocationData(BASE_ID + 10079, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Waptia Sushi":                        LocationData(BASE_ID + 10080, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pikaia Sushi":                        LocationData(BASE_ID + 10081, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Allenypterus Sushi":                  LocationData(BASE_ID + 10082, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Qingmenodus Sushi":                   LocationData(BASE_ID + 10083, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Falcatus Sushi":                      LocationData(BASE_ID + 10084, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Drepanaspis Sushi":                   LocationData(BASE_ID + 10085, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dunkleosteus Sushi":                  LocationData(BASE_ID + 10086, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Megalograptus Sushi":                 LocationData(BASE_ID + 10087, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Young Anomalocaris Sushi":            LocationData(BASE_ID + 10088, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seadragon Onigiri":                   LocationData(BASE_ID + 10089, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Arctic Telescope Fish Sushi":         LocationData(BASE_ID + 10090, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Lumpfish Sushi":                      LocationData(BASE_ID + 10091, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Snub-nosed Spiny Eel Sushi":          LocationData(BASE_ID + 10092, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Xenacanthus Sushi":                   LocationData(BASE_ID + 10093, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Longspine Squirrelfish Sushi":        LocationData(BASE_ID + 10094, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Clearfin Lionfish Sushi":             LocationData(BASE_ID + 10095, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Spear Squid Sushi":                   LocationData(BASE_ID + 10096, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Red-banded Lobster Sushi":            LocationData(BASE_ID + 10097, "Bancho Sushi", "recipe"),
    "Unlock Recipe: American Lobster Sushi":              LocationData(BASE_ID + 10098, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blue Lobster Sushi":                  LocationData(BASE_ID + 10099, "Bancho Sushi", "recipe"),
    "Unlock Recipe: California Spiny Lobster Sushi":      LocationData(BASE_ID + 10100, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fan Lobster Sushi":                   LocationData(BASE_ID + 10101, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Norway Lobster Sushi":                LocationData(BASE_ID + 10102, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Golden King Crab Sushi":              LocationData(BASE_ID + 10103, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Snow Crab Sushi":                     LocationData(BASE_ID + 10104, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Horsehair Crab Sushi":                LocationData(BASE_ID + 10105, "Bancho Sushi", "recipe"),
    "Unlock Recipe: European Lobster Sushi":              LocationData(BASE_ID + 10106, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Tropical Rock Lobster Sushi":         LocationData(BASE_ID + 10107, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Crystal Lobster Sushi":               LocationData(BASE_ID + 10108, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Eastern Rock Lobster Sushi":          LocationData(BASE_ID + 10109, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dollocaris Ingens Sushi":             LocationData(BASE_ID + 10110, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Tokummia Katalepsis Sushi":           LocationData(BASE_ID + 10111, "Bancho Sushi", "recipe"),
    # Tuna bar sushi (8052xxx) — not in 8050xxx range
    "Unlock Recipe: Bluefin Tuna Akami Sushi":            LocationData(BASE_ID + 10112, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Bluefin Tuna Chutoro Sushi":          LocationData(BASE_ID + 10113, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Bluefin Tuna Ootoro Sushi":           LocationData(BASE_ID + 10114, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Yellowfin Tuna Chutoro Sushi":        LocationData(BASE_ID + 10115, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Yellowfin Tuna Ootoro Sushi":         LocationData(BASE_ID + 10116, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Raw Black Tiger Shrimp Sushi":        LocationData(BASE_ID + 10117, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Cooked Whiteleg Shrimp Sushi":        LocationData(BASE_ID + 10118, "Bancho Sushi", "recipe"),

    # --- VIP mission recipes ---
    # --- Ichiban DLC recipes ---
    "Unlock Recipe: Stingray Sashimi Cold Noodles": LocationData(BASE_ID + 877, "Bancho Sushi", "dlc_ichiban"),
    "Unlock Recipe: Spear Squid Soba Futomaki":     LocationData(BASE_ID + 878, "Bancho Sushi", "dlc_ichiban"),
    "Unlock Recipe: Eggplant Soba Oyaki":           LocationData(BASE_ID + 879, "Bancho Sushi", "dlc_ichiban"),
    "Unlock Recipe: Warm Atlantic Mackerel Soba":   LocationData(BASE_ID + 880, "Bancho Sushi", "dlc_ichiban"),

    # --- Godzilla DLC recipes (all 3 unlocked via the DLC story) ---
    "Unlock Recipe: Godzilla vs. Ebirah Curry": LocationData(BASE_ID + 855, "Bancho Sushi", "dlc_godzilla"),
    "Unlock Recipe: Ebirah Chasing Sashimi":    LocationData(BASE_ID + 856, "Bancho Sushi", "dlc_godzilla"),
    "Unlock Recipe: Deep Sea Kaiju Ramen":       LocationData(BASE_ID + 885, "Bancho Sushi", "dlc_godzilla"),
}

# Godzilla DLC: Kaiju figurines collectible after defeating Ebirah.
# Figurines are scattered across the Blue Hole and can be collected post-Ebirah fight.
kaiju_figurine_locations: Dict[str, LocationData] = {
    # All 20 Kaiju figurines with confirmed locations (from player research)
    # All gated on Ebirah defeated (Chapter 5 Complete) in rules.py
    "Kaiju Figurine: Godzilla (1965)":         LocationData(BASE_ID + 857, "Blue Hole - Shallow",  "dlc_godzilla"),  # Near surface, far right
    "Kaiju Figurine: Ebirah (1966)":           LocationData(BASE_ID + 858, "Blue Hole - Mid",      "dlc_godzilla"),  # Shipwreck
    "Kaiju Figurine: Minilla (1967)":          LocationData(BASE_ID + 859, "Glacier Zone",         "dlc_godzilla"),  # First Glacial Cave, top right
    "Kaiju Figurine: Hedorah (1971)":          LocationData(BASE_ID + 860, "Hydrothermal Vents",   "dlc_godzilla"),  # Bottom right corner
    "Kaiju Figurine: Gigan (1972)":            LocationData(BASE_ID + 861, "Glacial Passage",      "dlc_godzilla"),  # Left side puzzle room
    "Kaiju Figurine: Jet Jaguar (1973)":       LocationData(BASE_ID + 862, "Sea People Village",   "dlc_godzilla"),  # Right of Game Parlor
    "Kaiju Figurine: King Caesar (1974)":      LocationData(BASE_ID + 863, "Blue Hole - Deep",     "dlc_godzilla"),  # Ramo/Suwam rescue room
    "Kaiju Figurine: Mechagodzilla (1975)":    LocationData(BASE_ID + 864, "Blue Hole - Mid",      "dlc_godzilla"),  # Sea People Record Chamber
    "Kaiju Figurine: Biolante (1989)":         LocationData(BASE_ID + 865, "Blue Hole - Deep",     "dlc_godzilla"),  # Tsuchi's house, turtle left side
    "Kaiju Figurine: King Ghidorah (1991)":    LocationData(BASE_ID + 866, "Sea People Village",   "dlc_godzilla"),  # Tenzhin's House
    "Kaiju Figurine: Mecha-King Ghidorah (1991)":LocationData(BASE_ID + 867, "Blue Hole - Deep",  "dlc_godzilla"),  # Near right side entrance
    "Kaiju Figurine: Rodan (1993)":            LocationData(BASE_ID + 868, "Glacier Zone",         "dlc_godzilla"),  # Ice Maze
    "Kaiju Figurine: Godzilla (1994)":         LocationData(BASE_ID + 869, "Blue Hole - Deep",     "dlc_godzilla"),  # Near whale skeleton
    "Kaiju Figurine: SpaceGodzilla (1994)":    LocationData(BASE_ID + 870, "Glacier Zone",         "dlc_godzilla"),  # Glacial Seaweed Cave, right jetstream
    "Kaiju Figurine: Little Godzilla (1994)":  LocationData(BASE_ID + 871, "Blue Hole - Deep",     "dlc_godzilla"),  # Blue Hole Depths
    "Kaiju Figurine: Destoroyah (1995)":       LocationData(BASE_ID + 872, "Hydrothermal Vents",   "dlc_godzilla"),  # Control room, left volcano puzzle
    "Kaiju Figurine: Godzilla (1995)":         LocationData(BASE_ID + 873, "Blue Hole - Deep",     "dlc_godzilla"),  # Near Sea People Village entrance
    "Kaiju Figurine: Anguirus (2004)":         LocationData(BASE_ID + 874, "Blue Hole - Mid",      "dlc_godzilla"),  # Limestone Caves
    "Kaiju Figurine: Mothra (1961)":           LocationData(BASE_ID + 875, "Blue Hole - Deep",     "dlc_godzilla"),  # Underwater Lake
    "Kaiju Figurine: Godzilla (2016)":         LocationData(BASE_ID + 876, "Glacial Passage",      "dlc_godzilla"),  # Right side puzzle room, 4th depth level
}

# === ICHIBAN DLC LOCATIONS ===
ichiban_locations: Dict[str, LocationData] = {
    # Ichiban DLC has 2 missions (which are the two main story beats of the DLC):
    # - Operation Sea Blue Eradication: culminates in defeating Torben (boss check handles completion)
    # - Cold Noodles: side mission unlocking Stingray Sashimi Cold Noodles recipe
    "Ichiban: Complete Operation Sea Blue Eradication": LocationData(BASE_ID + 881, "Bancho Sushi", "dlc_ichiban"),
    "Ichiban: Complete Cold Noodles Mission":            LocationData(BASE_ID + 886, "Bancho Sushi", "dlc_ichiban"),
    "Ichiban: Complete Beat 'Em Up Minigame":            LocationData(BASE_ID + 882, "Bancho Sushi", "dlc_ichiban"),
    "Ichiban: Complete Karaoke Minigame":                LocationData(BASE_ID + 883, "Bancho Sushi", "dlc_ichiban"),
}

# =====================================================================
# === JUNGLE DLC LOCATIONS (dlc_jungle) ===
# =====================================================================
# IDs allocated in the BASE_ID + 1400–1999 range.
# Fish locations are TODO — to be filled in once the full fish list is
# available from the wiki. Placeholder entries are numbered for now.

_J = BASE_ID + 4000  # Jungle DLC location base (dish upgrades end ~BASE_ID+3425, so 4000 is safe)

# --- Jungle story chapters ---
jungle_story_locations: Dict[str, LocationData] = {
    "Jungle: Prologue - To a New Place":                    LocationData(_J + 0,  "Utara Village",      "dlc_jungle"),
    "Jungle: Chapter 1 - The Village with Bad Food":        LocationData(_J + 1,  "Utara Village",      "dlc_jungle"),
    "Jungle: Chapter 2 - Jungle Life!":                     LocationData(_J + 2,  "Utara Village",      "dlc_jungle"),
    "Jungle: Chapter 3 - Diving Suit of the Sunang Civ":    LocationData(_J + 3,  "Setah Forest",       "dlc_jungle"),
    "Jungle: Chapter 4 - The Sea Beneath the Lake":         LocationData(_J + 4,  "Lakebed Sea",        "dlc_jungle"),
    "Jungle: Chapter 5 - Let's Head To The Jungle!":        LocationData(_J + 5,  "Utara Village",      "dlc_jungle"),
    "Jungle: Chapter 6 - Welcome To The Jungle!":           LocationData(_J + 6,  "Utara Village",      "dlc_jungle"),
    "Jungle: Chapter 7 - Find The Blue Divine Tree Fruit!": LocationData(_J + 7,  "Lakebed Sea",        "dlc_jungle"),
    "Jungle: Epilogue":                                     LocationData(_J + 8,  "Utara Village",      "dlc_jungle"),
}

# --- Jungle boss defeats ---
jungle_boss_locations: Dict[str, LocationData] = {
    "Jungle Boss: Defeat Giant Snapping Turtle":  LocationData(_J + 20, "Utara Lake - Lower", "dlc_jungle"),
    "Jungle Boss: Defeat Sulong":                 LocationData(_J + 21, "Utara Lake - Lower", "dlc_jungle"),
    "Jungle Boss: Defeat Black Caiman":           LocationData(_J + 22, "Utara Lake - Upper", "dlc_jungle"),
    "Jungle Boss: Defeat Stethacanthus":          LocationData(_J + 23, "Lakebed Sea",        "dlc_jungle"),
    "Jungle Boss: Defeat Xiphactinus":            LocationData(_J + 24, "Lakebed Sea",        "dlc_jungle"),
    "Jungle Boss: Defeat Basilosaurus":           LocationData(_J + 25, "Lakebed Sea",        "dlc_jungle"),
}

# --- Jungle staff unlocks ---
# =====================================================================
# STAFF LOCATIONS
# Each staff member has:
#   - "Staff: Hire [Name]"          — 1 check when recruited
#   - "Staff: Train [Name] Lv 5/10/15/20" — 4 training milestone checks
# Training levels 5, 10, 15 gate specific recipes; level 20 is completionist.
# Base offset: BASE_ID + 1500 (hire) and BASE_ID + 1600 (training)
# =====================================================================

_SH = BASE_ID + 1500   # Staff Hire base
_ST = BASE_ID + 1600   # Staff Training base (legacy — superseded by staff_all_levels_locations)
# NOTE: staff_training_locations (using _ST) was replaced by staff_all_levels_locations (_SA).
# staff_all_levels_locations handles both milestone (Lv5/10/15/20) and all_levels (Lv1-20)
# modes via should_include_location() filtering. staff_training_locations is NOT in location_table.

# Staff list with index for ID calculation:
# Base game (0-20): Billy, Carolina, Charlie, Cohh, Davina, Drae, El Nino,
#   Itsuki, James, Jandi, Kyoko, Liu, Maki, Masayoshi, Mitchell, Pai, Raptor,
#   Raul, Tohoku, Yone, Yusuke
# Ichiban DLC (21-23): Hamako, Etsuko, Chitose
# Jungle DLC (24-30): via jungle_staff_locations

def _staff_hire(idx: int, region: str = "Bancho Sushi", category: str = "") -> LocationData:
    return LocationData(_SH + idx, region, category)

def _staff_train(staff_idx: int, level_idx: int, region: str = "Bancho Sushi", category: str = "") -> LocationData:
    """level_idx: 0=Lv5, 1=Lv10, 2=Lv15, 3=Lv20"""
    return LocationData(_ST + staff_idx * 4 + level_idx, region, category)

staff_hire_locations: Dict[str, LocationData] = {
    # Base game staff (idx 0-20)
    "Staff: Hire Billy":      _staff_hire(0),
    "Staff: Hire Carolina":   _staff_hire(1),
    "Staff: Hire Charlie":    _staff_hire(2),
    "Staff: Hire Cohh":       _staff_hire(3),
    "Staff: Hire Davina":     _staff_hire(4),
    "Staff: Hire Drae":       _staff_hire(5),
    "Staff: Hire El Nino":    _staff_hire(6),
    "Staff: Hire Itsuki":     _staff_hire(7),
    "Staff: Hire James":      _staff_hire(8),
    "Staff: Hire Jandi":      _staff_hire(9),
    "Staff: Hire Kyoko":      _staff_hire(10),
    "Staff: Hire Liu":        _staff_hire(11),
    "Staff: Hire Maki":       _staff_hire(12),
    "Staff: Hire Masayoshi":  _staff_hire(13),
    "Staff: Hire Mitchell":   _staff_hire(14),
    "Staff: Hire Pai":        _staff_hire(15),
    "Staff: Hire Raptor":     _staff_hire(16),
    "Staff: Hire Raul":       _staff_hire(17),
    "Staff: Hire Tohoku":     _staff_hire(18),
    "Staff: Hire Yone":       _staff_hire(19),
    "Staff: Hire Yusuke":     _staff_hire(20),
    # Ichiban DLC staff (idx 21-23)
    "Staff: Hire Hamako":     _staff_hire(21, category="dlc_ichiban"),
    "Staff: Hire Etsuko":     _staff_hire(22, category="dlc_ichiban"),
    "Staff: Hire Chitose":    _staff_hire(23, category="dlc_ichiban"),
}

staff_training_locations: Dict[str, LocationData] = {
    # Base game staff training milestones
    "Staff: Train Billy to Level 5":       _staff_train(0, 0),
    "Staff: Train Billy to Level 10":      _staff_train(0, 1),
    "Staff: Train Billy to Level 15":      _staff_train(0, 2),
    "Staff: Train Billy to Level 20":      _staff_train(0, 3),
    "Staff: Train Carolina to Level 5":    _staff_train(1, 0),
    "Staff: Train Carolina to Level 10":   _staff_train(1, 1),
    "Staff: Train Carolina to Level 15":   _staff_train(1, 2),
    "Staff: Train Carolina to Level 20":   _staff_train(1, 3),
    "Staff: Train Charlie to Level 5":     _staff_train(2, 0),
    "Staff: Train Charlie to Level 10":    _staff_train(2, 1),
    "Staff: Train Charlie to Level 15":    _staff_train(2, 2),
    "Staff: Train Charlie to Level 20":    _staff_train(2, 3),
    "Staff: Train Cohh to Level 5":        _staff_train(3, 0),
    "Staff: Train Cohh to Level 10":       _staff_train(3, 1),
    "Staff: Train Cohh to Level 15":       _staff_train(3, 2),
    "Staff: Train Cohh to Level 20":       _staff_train(3, 3),
    "Staff: Train Davina to Level 5":      _staff_train(4, 0),
    "Staff: Train Davina to Level 10":     _staff_train(4, 1),
    "Staff: Train Davina to Level 15":     _staff_train(4, 2),
    "Staff: Train Davina to Level 20":     _staff_train(4, 3),
    "Staff: Train Drae to Level 5":        _staff_train(5, 0),
    "Staff: Train Drae to Level 10":       _staff_train(5, 1),
    "Staff: Train Drae to Level 15":       _staff_train(5, 2),
    "Staff: Train Drae to Level 20":       _staff_train(5, 3),
    "Staff: Train El Nino to Level 5":     _staff_train(6, 0),
    "Staff: Train El Nino to Level 10":    _staff_train(6, 1),
    "Staff: Train El Nino to Level 15":    _staff_train(6, 2),
    "Staff: Train El Nino to Level 20":    _staff_train(6, 3),
    "Staff: Train Itsuki to Level 5":      _staff_train(7, 0),
    "Staff: Train Itsuki to Level 10":     _staff_train(7, 1),
    "Staff: Train Itsuki to Level 15":     _staff_train(7, 2),
    "Staff: Train Itsuki to Level 20":     _staff_train(7, 3),
    "Staff: Train James to Level 5":       _staff_train(8, 0),
    "Staff: Train James to Level 10":      _staff_train(8, 1),
    "Staff: Train James to Level 15":      _staff_train(8, 2),
    "Staff: Train James to Level 20":      _staff_train(8, 3),
    "Staff: Train Jandi to Level 5":       _staff_train(9, 0),
    "Staff: Train Jandi to Level 10":      _staff_train(9, 1),
    "Staff: Train Jandi to Level 15":      _staff_train(9, 2),
    "Staff: Train Jandi to Level 20":      _staff_train(9, 3),
    "Staff: Train Kyoko to Level 5":       _staff_train(10, 0),
    "Staff: Train Kyoko to Level 10":      _staff_train(10, 1),
    "Staff: Train Kyoko to Level 15":      _staff_train(10, 2),
    "Staff: Train Kyoko to Level 20":      _staff_train(10, 3),
    "Staff: Train Liu to Level 5":         _staff_train(11, 0),
    "Staff: Train Liu to Level 10":        _staff_train(11, 1),
    "Staff: Train Liu to Level 15":        _staff_train(11, 2),
    "Staff: Train Liu to Level 20":        _staff_train(11, 3),
    "Staff: Train Maki to Level 5":        _staff_train(12, 0),
    "Staff: Train Maki to Level 10":       _staff_train(12, 1),
    "Staff: Train Maki to Level 15":       _staff_train(12, 2),
    "Staff: Train Maki to Level 20":       _staff_train(12, 3),
    "Staff: Train Masayoshi to Level 5":   _staff_train(13, 0),
    "Staff: Train Masayoshi to Level 10":  _staff_train(13, 1),
    "Staff: Train Masayoshi to Level 15":  _staff_train(13, 2),
    "Staff: Train Masayoshi to Level 20":  _staff_train(13, 3),
    "Staff: Train Mitchell to Level 5":    _staff_train(14, 0),
    "Staff: Train Mitchell to Level 10":   _staff_train(14, 1),
    "Staff: Train Mitchell to Level 15":   _staff_train(14, 2),
    "Staff: Train Mitchell to Level 20":   _staff_train(14, 3),
    "Staff: Train Pai to Level 5":         _staff_train(15, 0),
    "Staff: Train Pai to Level 10":        _staff_train(15, 1),
    "Staff: Train Pai to Level 15":        _staff_train(15, 2),
    "Staff: Train Pai to Level 20":        _staff_train(15, 3),
    "Staff: Train Raptor to Level 5":      _staff_train(16, 0),
    "Staff: Train Raptor to Level 10":     _staff_train(16, 1),
    "Staff: Train Raptor to Level 15":     _staff_train(16, 2),
    "Staff: Train Raptor to Level 20":     _staff_train(16, 3),
    "Staff: Train Raul to Level 5":        _staff_train(17, 0),
    "Staff: Train Raul to Level 10":       _staff_train(17, 1),
    "Staff: Train Raul to Level 15":       _staff_train(17, 2),
    "Staff: Train Raul to Level 20":       _staff_train(17, 3),
    "Staff: Train Tohoku to Level 5":      _staff_train(18, 0),
    "Staff: Train Tohoku to Level 10":     _staff_train(18, 1),
    "Staff: Train Tohoku to Level 15":     _staff_train(18, 2),
    "Staff: Train Tohoku to Level 20":     _staff_train(18, 3),
    "Staff: Train Yone to Level 5":        _staff_train(19, 0),
    "Staff: Train Yone to Level 10":       _staff_train(19, 1),
    "Staff: Train Yone to Level 15":       _staff_train(19, 2),
    "Staff: Train Yone to Level 20":       _staff_train(19, 3),
    "Staff: Train Yusuke to Level 5":      _staff_train(20, 0),
    "Staff: Train Yusuke to Level 10":     _staff_train(20, 1),
    "Staff: Train Yusuke to Level 15":     _staff_train(20, 2),
    "Staff: Train Yusuke to Level 20":     _staff_train(20, 3),
    # Ichiban DLC staff training
    "Staff: Train Hamako to Level 5":      _staff_train(21, 0, category="dlc_ichiban"),
    "Staff: Train Hamako to Level 10":     _staff_train(21, 1, category="dlc_ichiban"),
    "Staff: Train Hamako to Level 15":     _staff_train(21, 2, category="dlc_ichiban"),
    "Staff: Train Hamako to Level 20":     _staff_train(21, 3, category="dlc_ichiban"),
    "Staff: Train Etsuko to Level 5":      _staff_train(22, 0, category="dlc_ichiban"),
    "Staff: Train Etsuko to Level 10":     _staff_train(22, 1, category="dlc_ichiban"),
    "Staff: Train Etsuko to Level 15":     _staff_train(22, 2, category="dlc_ichiban"),
    "Staff: Train Etsuko to Level 20":     _staff_train(22, 3, category="dlc_ichiban"),
    "Staff: Train Chitose to Level 5":     _staff_train(23, 0, category="dlc_ichiban"),
    "Staff: Train Chitose to Level 10":    _staff_train(23, 1, category="dlc_ichiban"),
    "Staff: Train Chitose to Level 15":    _staff_train(23, 2, category="dlc_ichiban"),
    "Staff: Train Chitose to Level 20":    _staff_train(23, 3, category="dlc_ichiban"),
}

# ── All-levels training (Lv1-20, for staff_training_depth=all_levels) ────────
# ID block: BASE_ID + 3500 + staff_idx*20 + (level-1)  →  max offset = 3500+23*20+19 = 3979 (safe before jungle at 4000)
# category "staff_all_levels" (base game) or "staff_all_levels_ichiban" (DLC)
_SA = BASE_ID + 3500  # Staff All-levels base (_D block ends at ~3493, _D2 block at BASE_ID+12000+, jungle starts at 4000)

_BASE_STAFF_NAMES = [
    "Billy", "Carolina", "Charlie", "Cohh", "Davina", "Drae", "El Nino",
    "Itsuki", "James", "Jandi", "Kyoko", "Liu", "Maki", "Masayoshi", "Mitchell",
    "Pai", "Raptor", "Raul", "Tohoku", "Yone", "Yusuke",
]
_ICHIBAN_STAFF_NAMES = ["Hamako", "Etsuko", "Chitose"]

staff_all_levels_locations: Dict[str, LocationData] = {
    f"Staff: Train {name} to Level {lvl}": LocationData(
        _SA + idx * 20 + (lvl - 1), "Bancho Sushi", "staff_all_levels"
    )
    for idx, name in enumerate(_BASE_STAFF_NAMES)
    for lvl in range(1, 21)
} | {
    f"Staff: Train {name} to Level {lvl}": LocationData(
        _SA + (21 + i) * 20 + (lvl - 1), "Bancho Sushi", "staff_all_levels_ichiban"
    )
    for i, name in enumerate(_ICHIBAN_STAFF_NAMES)
    for lvl in range(1, 21)
}

# ── Jungle staff (hire only — training is in the Jungle DLC options block) ──
jungle_staff_locations: Dict[str, LocationData] = {
    "Jungle Staff: Unlock Yasuto":            LocationData(_J + 40, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock Martin Tweed":      LocationData(_J + 41, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock Rover":             LocationData(_J + 42, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock Om Nom":            LocationData(_J + 43, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock Charlie Bonnet III":LocationData(_J + 44, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock William Longbottom":LocationData(_J + 45, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock Mita":              LocationData(_J + 46, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock Udo":               LocationData(_J + 47, "Utara Village", "dlc_jungle"),
    "Jungle Staff: Unlock Sato":              LocationData(_J + 48, "Utara Village", "dlc_jungle"),
}

# --- Jungle villager friendship rewards (3-heart and 4-heart milestones) ---
# 33 villagers with 2 reward tiers each = 66 checks
# NOTE: Exact villager names to be confirmed in-game. Using known names from guides.
# Key named villagers confirmed: Muna, Jaka, Bonita, Gesang, Chandra, Peneb, Lathi,
# Cinta, Lipah, Marone, Eka, Bima, Saniah, Sato (pre-staff), Rimbo
jungle_villager_locations: Dict[str, LocationData] = {
    "Jungle Villager: Muna 3-Heart Reward":    LocationData(_J + 60, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Muna 4-Heart Reward":    LocationData(_J + 61, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Jaka 3-Heart Reward":    LocationData(_J + 62, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Jaka 4-Heart Reward":    LocationData(_J + 63, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Bonita 3-Heart Reward":  LocationData(_J + 64, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Bonita 4-Heart Reward":  LocationData(_J + 65, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Gesang 3-Heart Reward":  LocationData(_J + 66, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Gesang 4-Heart Reward":  LocationData(_J + 67, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Chandra 3-Heart Reward": LocationData(_J + 68, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Chandra 4-Heart Reward": LocationData(_J + 69, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Peneb 3-Heart Reward":   LocationData(_J + 70, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Peneb 4-Heart Reward":   LocationData(_J + 71, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Lathi 3-Heart Reward":   LocationData(_J + 72, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Lathi 4-Heart Reward":   LocationData(_J + 73, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Cinta 3-Heart Reward":   LocationData(_J + 74, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Cinta 4-Heart Reward":   LocationData(_J + 75, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Lipah 3-Heart Reward":   LocationData(_J + 76, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Lipah 4-Heart Reward":   LocationData(_J + 77, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Marone 3-Heart Reward":  LocationData(_J + 78, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Marone 4-Heart Reward":  LocationData(_J + 79, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Eka 3-Heart Reward":     LocationData(_J + 80, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Eka 4-Heart Reward":     LocationData(_J + 81, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Bima 3-Heart Reward":    LocationData(_J + 82, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Bima 4-Heart Reward":    LocationData(_J + 83, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Saniah 3-Heart Reward":  LocationData(_J + 84, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Saniah 4-Heart Reward":  LocationData(_J + 85, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Rimbo 3-Heart Reward":   LocationData(_J + 86, "Utara Village", "dlc_jungle"),
    "Jungle Villager: Rimbo 4-Heart Reward":   LocationData(_J + 87, "Utara Village", "dlc_jungle"),
    # TODO: Add remaining ~18 villagers once confirmed in-game (IDs _J+88 to _J+123 reserved)
}

# --- Jungle minigames ---
jungle_minigame_locations: Dict[str, LocationData] = {
    "Jungle Minigame: Win First Beetle Battle":       LocationData(_J + 130, "Setah Forest",   "dlc_jungle"),
    "Jungle Minigame: Win 5 Beetle Battles":          LocationData(_J + 131, "Setah Forest",   "dlc_jungle"),
    "Jungle Minigame: Win 10 Beetle Battles":         LocationData(_J + 132, "Setah Forest",   "dlc_jungle"),
    "Jungle Minigame: Complete Hide and Seek":        LocationData(_J + 133, "Utara Village",  "dlc_jungle"),
    "Jungle Minigame: Complete Shooting Range":       LocationData(_J + 134, "Utara Village",  "dlc_jungle"),
    "Jungle Minigame: Complete Duck Hunting":         LocationData(_J + 135, "Surga Falls",    "dlc_jungle"),
    "Jungle Minigame: Complete Rope-Cutting Puzzle":  LocationData(_J + 136, "Murau Temple",   "dlc_jungle"),
    "Jungle Minigame: First Land Fishing Catch":      LocationData(_J + 137, "Surga Falls",    "dlc_jungle"),
}

# --- Jungle insect catching (Insectagram) ---
jungle_insect_locations: Dict[str, LocationData] = {
    # === Net-caught insects (20 species, caught with Bug Net) ===
    # TIDs confirmed via UnityExplorer DataManager.JungleInsectInfoDic dump 2026-06-26
    "Insect: Catch Ulysses Swallowtail":              LocationData(_J + 150, "Setah Forest",       "dlc_jungle"),  # TID 40001
    "Insect: Catch Stick Insect":                     LocationData(_J + 151, "Setah Forest",       "dlc_jungle"),  # TID 40002 (Phobaeticus chani)
    "Insect: Catch Gigas Giant Longhorn Beetle":      LocationData(_J + 152, "Setah Forest",       "dlc_jungle"),  # TID 40003
    "Insect: Catch Diving Beetle":                    LocationData(_J + 153, "Utara Lake - Upper", "dlc_jungle"),  # TID 40004
    "Insect: Catch Takua Cicada":                     LocationData(_J + 154, "Setah Forest",       "dlc_jungle"),  # TID 40005
    "Insect: Catch Blue Admiral Butterfly":           LocationData(_J + 155, "Setah Forest",       "dlc_jungle"),  # TID 40007
    "Insect: Catch Common Lascar Butterfly":          LocationData(_J + 156, "Setah Forest",       "dlc_jungle"),  # TID 40008
    "Insect: Catch Striped Blue Crow Butterfly":      LocationData(_J + 157, "Setah Forest",       "dlc_jungle"),  # TID 40009
    "Insect: Catch Paper Kite Butterfly":             LocationData(_J + 158, "Setah Forest",       "dlc_jungle"),  # TID 40010
    "Insect: Catch Common Green Birdwing":            LocationData(_J + 159, "Setah Forest",       "dlc_jungle"),  # TID 40011
    "Insect: Catch Rajah Brooke's Birdwing":          LocationData(_J + 160, "Setah Forest",       "dlc_jungle"),  # TID 40012
    "Insect: Catch Atlas Moth":                       LocationData(_J + 161, "Setah Forest",       "dlc_jungle"),  # TID 40013
    "Insect: Catch Firefly":                          LocationData(_J + 162, "Setah Forest",       "dlc_jungle"),  # TID 40014
    "Insect: Catch Moth":                             LocationData(_J + 163, "Setah Forest",       "dlc_jungle"),  # TID 40015
    "Insect: Catch Sea Green Swallowtail":            LocationData(_J + 164, "Setah Forest",       "dlc_jungle"),  # TID 40017
    "Insect: Catch Gigon Swallowtail":                LocationData(_J + 165, "Setah Forest",       "dlc_jungle"),  # TID 40018
    "Insect: Catch Common Grass Yellow Butterfly":    LocationData(_J + 166, "Setah Forest",       "dlc_jungle"),  # TID 40019
    "Insect: Catch Common Albatross Butterfly":       LocationData(_J + 167, "Setah Forest",       "dlc_jungle"),  # TID 40020
    "Insect: Catch Blanchard's Ghost Butterfly":      LocationData(_J + 168, "Setah Forest",       "dlc_jungle"),  # TID 40021

    # === Battle insects (17 beetles — fight using battle insect minigame) ===
    # TIDs confirmed via UnityExplorer DataManager.JungleInsectInfoDic dump 2026-06-26
    "Insect Battle: Defeat Little Stag Beetle":             LocationData(_J + 170, "Setah Forest",  "dlc_jungle"),  # TID 40016
    "Insect Battle: Defeat Caucasus Beetle":                LocationData(_J + 171, "Setah Forest",  "dlc_jungle"),  # TID 40023
    "Insect Battle: Defeat Atlas Beetle":                   LocationData(_J + 172, "Setah Forest",  "dlc_jungle"),  # TID 40024
    "Insect Battle: Defeat Five-Horned Rhinoceros Beetle":  LocationData(_J + 173, "Setah Forest",  "dlc_jungle"),  # TID 40025
    "Insect Battle: Defeat Siamese Five-Horned Beetle":     LocationData(_J + 174, "Setah Forest",  "dlc_jungle"),  # TID 40026
    "Insect Battle: Defeat Siamese Rhinoceros Beetle":      LocationData(_J + 175, "Setah Forest",  "dlc_jungle"),  # TID 40027
    "Insect Battle: Defeat Femoralis Stag Beetle":          LocationData(_J + 176, "Setah Forest",  "dlc_jungle"),  # TID 40028
    "Insect Battle: Defeat Steveni Stag Beetle":            LocationData(_J + 177, "Setah Forest",  "dlc_jungle"),  # TID 40029
    "Insect Battle: Defeat Giraffe Stag Beetle":            LocationData(_J + 178, "Setah Forest",  "dlc_jungle"),  # TID 40030
    "Insect Battle: Defeat Zebra Stag Beetle":              LocationData(_J + 179, "Setah Forest",  "dlc_jungle"),  # TID 40031
    "Insect Battle: Defeat Giant Stag Beetle":              LocationData(_J + 180, "Setah Forest",  "dlc_jungle"),  # TID 40032
    "Insect Battle: Defeat Antler Stag Beetle":             LocationData(_J + 181, "Setah Forest",  "dlc_jungle"),  # TID 40033
    "Insect Battle: Defeat Metallic Stag Beetle":           LocationData(_J + 182, "Setah Forest",  "dlc_jungle"),  # TID 40034
    "Insect Battle: Defeat Striata Stag Beetle":            LocationData(_J + 183, "Setah Forest",  "dlc_jungle"),  # TID 40035
    "Insect Battle: Defeat Rosenbergi Stag Beetle":         LocationData(_J + 184, "Setah Forest",  "dlc_jungle"),  # TID 40036
    "Insect Battle: Defeat Boss Stag Beetle":               LocationData(_J + 185, "Setah Forest",  "dlc_jungle"),  # TID 40037
    "Insect Battle: Defeat Boss Beetle":                    LocationData(_J + 186, "Setah Forest",  "dlc_jungle"),  # TID 40038

    # === Insectagram milestones ===
    "Jungle Insectagram: 50% Complete":               LocationData(_J + 190, "Utara Village",  "dlc_jungle"),
    "Jungle Insectagram: 100% Complete":              LocationData(_J + 191, "Utara Village",  "dlc_jungle"),
}

# --- Jungle fish (first catch) — PLACEHOLDER ---
# TODO: Fill in exact fish names and regions once wiki tables are available.
# Research shows ~82 fish total across Upper Lake, Lower Lake, Lakebed Sea, and fishing spots.
# Region assignments: Upper Lake (0-35m), Lower Lake (35-75m), Lakebed Sea, Surga Falls (rod)
# Known species: Tilapia, Archerfish, Red/Brown/Green/Blue/Heckel Discus, Walking Catfish,
#   Red-Bellied Piranha, Black Caiman (boss), Giant Freshwater Stingray, Electric Eel,
#   Piraiba Catfish, Asian Arowana, Platinum Alligator Gar, Sockeye Salmon,
#   Giant Snapping Turtle (boss), Fire Eel, Pirarucu, Ammonite, Orthoceras,
#   Stethacanthus (boss), Xiphactinus (boss), Basilosaurus (boss)
# IDs _J+200 to _J+399 reserved for fish first-catch locations.
jungle_fish_locations: Dict[str, LocationData] = {
    # === Utara Lake - Upper (shallow zone) ===
    # Caught by diving / net — no special tool required beyond lake access
    # Confirmed via GO name dump (2026-06-27) — TIDs in FishCatchPatch.cs
    "First Catch: Kissing Gourami":       LocationData(_J + 200, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Walking Catfish":       LocationData(_J + 201, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Clown Loach":           LocationData(_J + 202, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Tilapia":               LocationData(_J + 203, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Red Discus":            LocationData(_J + 204, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Lemon Yellow Lab":      LocationData(_J + 205, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Bluegray Mbuna":        LocationData(_J + 206, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Archerfish":            LocationData(_J + 207, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Chocolate Gourami":     LocationData(_J + 208, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Red-Bellied Piranha":   LocationData(_J + 209, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Bluegill":              LocationData(_J + 210, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Pearl Gourami":         LocationData(_J + 211, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Brown Discus":          LocationData(_J + 212, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Green Discus":          LocationData(_J + 213, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Blue Discus":           LocationData(_J + 214, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Heckel Discus":         LocationData(_J + 215, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Malayan Pikehead":      LocationData(_J + 216, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Pirarucu":              LocationData(_J + 217, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Redeye Piranha":        LocationData(_J + 218, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Goliath Tigerfish":     LocationData(_J + 219, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Black Caiman":          LocationData(_J + 220, "Utara Lake - Upper", "dlc_jungle"),

    # === Utara Lake - Lower (deep zone — requires Purification Filter) ===
    # Confirmed via GO name dump (2026-06-27)
    "First Catch: Giant Freshwater Stingray": LocationData(_J + 250, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Electric Eel":              LocationData(_J + 251, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Nile Perch":                LocationData(_J + 252, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Horse Face Loach":          LocationData(_J + 253, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Giant Snakehead":           LocationData(_J + 254, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Armored Catfish":           LocationData(_J + 255, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Largemouth Bass":           LocationData(_J + 256, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Mud Carp":                  LocationData(_J + 257, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Piraiba Catfish":           LocationData(_J + 258, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Indonesian Tiger Perch":    LocationData(_J + 259, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Grass Carp":                LocationData(_J + 260, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Asian Arowana":             LocationData(_J + 261, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Alligator Gar":             LocationData(_J + 262, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Great Sturgeon":            LocationData(_J + 263, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: King Salmon":               LocationData(_J + 264, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Fire Eel":                  LocationData(_J + 265, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Clown Featherback":         LocationData(_J + 266, "Utara Lake - Lower", "dlc_jungle"),

    # === Lakebed Sea (ancient ecosystem) ===
    # Confirmed via GO name dump (2026-06-27)
    "First Catch: Eagle Shark":            LocationData(_J + 300, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Ophthalmosaurus":        LocationData(_J + 301, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Parameteroraspis":       LocationData(_J + 302, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Paradoxides":            LocationData(_J + 303, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Stylonurus":             LocationData(_J + 304, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Ammonite":               LocationData(_J + 305, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Tullimonstrum":          LocationData(_J + 306, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Promissum":              LocationData(_J + 307, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Hensodon":               LocationData(_J + 308, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Red Feather Starfish":   LocationData(_J + 309, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Eomesodon":              LocationData(_J + 310, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Exellia":                LocationData(_J + 311, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Foreyia":                LocationData(_J + 312, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Orthoceras":             LocationData(_J + 313, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Burgessomedusa":         LocationData(_J + 314, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Gyrodus":                LocationData(_J + 315, "Lakebed Sea", "dlc_jungle"),
    "First Catch: Sacabambaspis":          LocationData(_J + 316, "Lakebed Sea", "dlc_jungle"),

    # === Rod Fishing (Surga Falls / Setah Forest — requires Fishing Rod) ===
    "First Catch: Moonlight Gourami":      LocationData(_J + 350, "Setah Forest", "dlc_jungle"),
    "First Catch: Three Spot Gourami":     LocationData(_J + 351, "Setah Forest", "dlc_jungle"),
    "First Catch: Malayan Leaf Fish":      LocationData(_J + 352, "Setah Forest", "dlc_jungle"),
    "First Catch: Snakeskin Gourami":      LocationData(_J + 353, "Setah Forest", "dlc_jungle"),
    "First Catch: Giant Gourami":          LocationData(_J + 354, "Setah Forest", "dlc_jungle"),
    "First Catch: Emperor Snakehead":      LocationData(_J + 355, "Setah Forest", "dlc_jungle"),
    "First Catch: Striped Snakehead":      LocationData(_J + 356, "Setah Forest", "dlc_jungle"),
    "First Catch: Peacock Bass":           LocationData(_J + 357, "Setah Forest", "dlc_jungle"),
    "First Catch: Tambaqui":               LocationData(_J + 358, "Setah Forest", "dlc_jungle"),
    "First Catch: Malayan Mahseer":        LocationData(_J + 359, "Setah Forest", "dlc_jungle"),
    "First Catch: Redtail Catfish":        LocationData(_J + 360, "Setah Forest", "dlc_jungle"),
    "First Catch: Tapah":                  LocationData(_J + 361, "Setah Forest", "dlc_jungle"),
}

# --- Jungle ingredient first finds ---
jungle_ingredient_locations: Dict[str, LocationData] = {
    "Jungle First Find: Thai Chili":    LocationData(_J + 400, "Utara Village",      "dlc_jungle"),
    "Jungle First Find: Palm Sugar":    LocationData(_J + 401, "Utara Village",      "dlc_jungle"),
    "Jungle First Find: Calamansi":     LocationData(_J + 402, "Utara Lake - Upper", "dlc_jungle"),
    "Jungle First Find: Lemongrass":    LocationData(_J + 403, "Utara Village",      "dlc_jungle"),
    "Jungle First Find: Banana":        LocationData(_J + 404, "Setah Forest",       "dlc_jungle"),
    "Jungle First Find: Pineapple":     LocationData(_J + 405, "Utara Village",      "dlc_jungle"),
    "Jungle First Find: Dragon Fruit":  LocationData(_J + 406, "Utara Village",      "dlc_jungle"),
    "Jungle First Find: Watermelon":    LocationData(_J + 407, "Utara Village",      "dlc_jungle"),
    "Jungle First Find: Honeydew":      LocationData(_J + 408, "Utara Village",      "dlc_jungle"),
    "Jungle First Find: Sunang Stone":  LocationData(_J + 409, "Lakebed Sea",        "dlc_jungle"),
}

# --- Jungle Bancho Grill milestones ---
jungle_restaurant_locations: Dict[str, LocationData] = {
    # === Bancho Grill milestones ===
    "Jungle: Open Bancho Grill":                  LocationData(_J + 420, "Bancho Grill",       "dlc_jungle"),
    "Jungle: Serve 10 Customers":                 LocationData(_J + 421, "Bancho Grill",       "dlc_jungle"),
    "Jungle: Serve 50 Customers":                 LocationData(_J + 422, "Bancho Grill",       "dlc_jungle"),
    "Jungle: Serve 100 Customers":                LocationData(_J + 423, "Bancho Grill",       "dlc_jungle"),

    # === Complex recipe unlocks (Artisan Flame research) — TIDs from UnityExplorer 2026-06-26 ===
    # UnlockType=0 means unlocked by first catch/ingredient
    # UnlockType=1/2/3/4 means villager rank-up reward
    # UnlockType=410012031 = Bird-of-Paradise rank, 410016111 = Monkey rank
    "Grill Recipe: Tilapia with Calamansi":       LocationData(_J + 500, "Bancho Grill",       "dlc_jungle"),  # 8054101
    "Grill Recipe: Tropical Fish Steamed":        LocationData(_J + 501, "Bancho Grill",       "dlc_jungle"),  # 8054102
    "Grill Recipe: Black Caiman Taco":            LocationData(_J + 502, "Bancho Grill",       "dlc_jungle"),  # 8054103
    "Grill Recipe: Gourami Fried":                LocationData(_J + 503, "Bancho Grill",       "dlc_jungle"),  # 8054105
    "Grill Recipe: Largemouth Bass Boiled":       LocationData(_J + 504, "Bancho Grill",       "dlc_jungle"),  # 8054106
    "Grill Recipe: Pirarucu Banana Lasagna":      LocationData(_J + 505, "Bancho Grill",       "dlc_jungle"),  # 8054107
    "Grill Recipe: Lemon Yellow Lab Banana Fried":LocationData(_J + 506, "Bancho Grill",       "dlc_jungle"),  # 8054108
    "Grill Recipe: Piranha Head Soup":            LocationData(_J + 507, "Bancho Grill",       "dlc_jungle"),  # 8054110
    "Grill Recipe: Bamboo Shoot Soup":            LocationData(_J + 508, "Bancho Grill",       "dlc_jungle"),  # 8054111
    "Grill Recipe: King Trumpet Mushroom Stir-fried": LocationData(_J + 509, "Bancho Grill",   "dlc_jungle"),  # 8054112
    "Grill Recipe: Banana Halo-Halo":             LocationData(_J + 510, "Bancho Grill",       "dlc_jungle"),  # 8054113
    "Grill Recipe: Banana Blossom Salad":         LocationData(_J + 511, "Bancho Grill",       "dlc_jungle"),  # 8054114
    "Grill Recipe: Mud Carp Grilled in Banana Leaf": LocationData(_J + 512, "Bancho Grill",    "dlc_jungle"),  # 8054115
    "Grill Recipe: Grass Carp Bamboo Shoot Steamed": LocationData(_J + 513, "Bancho Grill",    "dlc_jungle"),  # 8054116
    "Grill Recipe: Indonesian Tiger Perch Sate":  LocationData(_J + 514, "Bancho Grill",       "dlc_jungle"),  # 8054117
    "Grill Recipe: Piraiba Catfish Tamarind Soup":LocationData(_J + 515, "Bancho Grill",       "dlc_jungle"),  # 8054118
    "Grill Recipe: Electric Eel Sliced":          LocationData(_J + 516, "Bancho Grill",       "dlc_jungle"),  # 8054119
    "Grill Recipe: Giant Freshwater Stingray Barbecued": LocationData(_J + 517, "Bancho Grill", "dlc_jungle"),  # 8054120
    "Grill Recipe: Giant Snakehead Soup":         LocationData(_J + 518, "Bancho Grill",       "dlc_jungle"),  # 8054124
    "Grill Recipe: Crayfish Sambal Stir-fried":   LocationData(_J + 519, "Bancho Grill",       "dlc_jungle"),  # 8054125
    "Grill Recipe: Crayfish Lemongrass Steamed":  LocationData(_J + 520, "Bancho Grill",       "dlc_jungle"),  # 8054126
    "Grill Recipe: Ciurcopterus Stir-fried":      LocationData(_J + 521, "Lakebed Sea",        "dlc_jungle"),  # 8054127
    "Grill Recipe: Tumidocarcinus Tamarind Stir-fried": LocationData(_J + 522, "Lakebed Sea",  "dlc_jungle"),  # 8054128
    "Grill Recipe: Tuzoia Soup":                  LocationData(_J + 523, "Lakebed Sea",        "dlc_jungle"),  # 8054129
    "Grill Recipe: Duck with Water Chestnut":     LocationData(_J + 524, "Bancho Grill",       "dlc_jungle"),  # 8054130
    "Grill Recipe: Bluegill Steamed":             LocationData(_J + 525, "Bancho Grill",       "dlc_jungle"),  # 8054133
    "Grill Recipe: Striped Snakehead Fried":      LocationData(_J + 526, "Bancho Grill",       "dlc_jungle"),  # 8054134
    "Grill Recipe: Tambaqui Grilled":             LocationData(_J + 527, "Bancho Grill",       "dlc_jungle"),  # 8054135
    "Grill Recipe: Ammonite Salad":               LocationData(_J + 528, "Lakebed Sea",        "dlc_jungle"),  # 8054138
    "Grill Recipe: Ophtalmosaurus Grilled":       LocationData(_J + 529, "Lakebed Sea",        "dlc_jungle"),  # 8054139
    "Grill Recipe: Clown Featherback Taro Fried": LocationData(_J + 530, "Bancho Grill",       "dlc_jungle"),  # 8054140
    "Grill Recipe: Goliath Tigerfish Salad":      LocationData(_J + 531, "Bancho Grill",       "dlc_jungle"),  # 8054141
    "Grill Recipe: Giant Sturgeon Steak":         LocationData(_J + 532, "Bancho Grill",       "dlc_jungle"),  # 8054142
    "Grill Recipe: Sacabambaspis Sate":           LocationData(_J + 533, "Lakebed Sea",        "dlc_jungle"),  # 8054143
    "Grill Recipe: Eagle Shark Stew":             LocationData(_J + 534, "Lakebed Sea",        "dlc_jungle"),  # 8054144
    "Grill Recipe: Stir-fried Crocodile Tail":    LocationData(_J + 535, "Bancho Grill",       "dlc_jungle"),  # 8054145 (Bonita VIP)
    "Grill Recipe: Tangsuyuk":                    LocationData(_J + 536, "Bancho Grill",       "dlc_jungle"),  # 8054146 (Monkey rank)
    "Grill Recipe: Walking Catfish Tom Yum":      LocationData(_J + 537, "Bancho Grill",       "dlc_jungle"),  # 8054147
    "Grill Recipe: Tropical Fruit Salad":         LocationData(_J + 538, "Bancho Grill",       "dlc_jungle"),  # 8054148 (Bird-of-Paradise)
    "Grill Recipe: Dragon Fruit Salad":           LocationData(_J + 539, "Bancho Grill",       "dlc_jungle"),  # 8054150 (Bird-of-Paradise)
    "Grill Recipe: Premium Fruit Salad":          LocationData(_J + 540, "Bancho Grill",       "dlc_jungle"),  # 8054151 (Bird-of-Paradise)
    "Grill Recipe: Pink Durian Pudding":          LocationData(_J + 541, "Bancho Grill",       "dlc_jungle"),  # 8054152 (Bird-of-Paradise)
    "Grill Recipe: Salmon Watermelon Salad":      LocationData(_J + 542, "Bancho Grill",       "dlc_jungle"),  # 8054153 (Bird-of-Paradise)
    "Grill Recipe: Alligator Gar Durian Head Curry": LocationData(_J + 543, "Bancho Grill",    "dlc_jungle"),  # 8054154 (Bird-of-Paradise)
    "Grill Recipe: Electric Eel Pineapple Stir-fried": LocationData(_J + 544, "Bancho Grill",  "dlc_jungle"),  # 8054156 (Bird-of-Paradise)
    "Grill Recipe: Parameteraspides Tom Yum":     LocationData(_J + 545, "Lakebed Sea",        "dlc_jungle"),  # 8054159
    "Grill Recipe: Burgessomedusa Pink Honeydew Jelly": LocationData(_J + 546, "Lakebed Sea",  "dlc_jungle"),  # 8054160 (Bird-of-Paradise)
    "Grill Recipe: Black Caiman Red Pineapple Stir-fried": LocationData(_J + 547, "Bancho Grill", "dlc_jungle"),  # 8054161 (Bird-of-Paradise)
    "Grill Recipe: Catfish Mix Fried":            LocationData(_J + 548, "Bancho Grill",       "dlc_jungle"),  # 8054164
    "Grill Recipe: Perch Mix Steamed":            LocationData(_J + 549, "Bancho Grill",       "dlc_jungle"),  # 8054165
    "Grill Recipe: Shellfish Hotpot":             LocationData(_J + 550, "Bancho Grill",       "dlc_jungle"),  # 8054166
    "Grill Recipe: Squid Mix Stir-fried":         LocationData(_J + 551, "Lakebed Sea",        "dlc_jungle"),  # 8054167
    "Grill Recipe: Stylonurus Honeydew Salad":    LocationData(_J + 552, "Lakebed Sea",        "dlc_jungle"),  # 8054168 (Bird-of-Paradise)
    "Grill Recipe: Banana Dragon Fruit Pudding":  LocationData(_J + 553, "Bancho Grill",       "dlc_jungle"),  # 8054169 (Bird-of-Paradise)
    "Grill Recipe: Mangosteen Coconut Dessert":   LocationData(_J + 554, "Bancho Grill",       "dlc_jungle"),  # 8054170
    "Grill Recipe: Bird Mix Seasoned":            LocationData(_J + 555, "Setah Forest",       "dlc_jungle"),  # 8054171
    "Grill Recipe: Snake Mix Spicy Soup":         LocationData(_J + 556, "Setah Forest",       "dlc_jungle"),  # 8054172
    "Grill Recipe: Hindleg Mix Fried":            LocationData(_J + 557, "Setah Forest",       "dlc_jungle"),  # 8054173
    "Grill Recipe: Fried Egg":                    LocationData(_J + 558, "Bancho Grill",       "dlc_jungle"),  # 8054174
    "Grill Recipe: Fiddlehead Fern Boiled":       LocationData(_J + 559, "Setah Forest",       "dlc_jungle"),  # 8054201

    # === Boss recipes (8 — each unlocked by defeating the boss) ===
    "Grill Recipe: Stethacanthus Coconut Stew":   LocationData(_J + 560, "Lakebed Sea",        "dlc_jungle"),  # 8054301
    "Grill Recipe: Sulong Foreleg Seasoned":      LocationData(_J + 561, "Utara Lake - Upper", "dlc_jungle"),  # 8054302
    "Grill Recipe: Giant Turtle Seafood Soup":    LocationData(_J + 562, "Utara Lake - Lower", "dlc_jungle"),  # 8054303
    "Grill Recipe: Xiphactinus Spicy Soup":       LocationData(_J + 563, "Lakebed Sea",        "dlc_jungle"),  # 8054304
    "Grill Recipe: Giant Blenny Herb Roast":      LocationData(_J + 564, "Utara Lake - Lower", "dlc_jungle"),  # 8054305
    "Grill Recipe: Giant Mudskipper Cheek Steamed": LocationData(_J + 565, "Setah Forest",     "dlc_jungle"),  # 8054306
    "Grill Recipe: Giant Snakehead Fin Soup":     LocationData(_J + 566, "Utara Lake - Lower", "dlc_jungle"),  # 8054307
    "Grill Recipe: Basilosaurus Belly Hotpot":    LocationData(_J + 567, "Lakebed Sea",        "dlc_jungle"),  # 8054308
}

# --- Jungle exploration milestones ---
jungle_exploration_locations: Dict[str, LocationData] = {
    "Jungle: Reach Setah Forest":                 LocationData(_J + 440, "Setah Forest",       "dlc_jungle"),
    "Jungle: Discover Murau Temple":              LocationData(_J + 441, "Murau Temple",        "dlc_jungle"),
    "Jungle: Reach Surga Falls":                  LocationData(_J + 442, "Surga Falls",         "dlc_jungle"),
    "Jungle: Discover Lakebed Sea":               LocationData(_J + 443, "Lakebed Sea",         "dlc_jungle"),
    "Jungle: Unlock Machete Path (Pirarucu Area)":LocationData(_J + 444, "Utara Lake - Lower",  "dlc_jungle"),
    "Jungle: Complete Marinca Bloom 50%":         LocationData(_J + 445, "Utara Village",       "dlc_jungle"),
    "Jungle: Complete Marinca Bloom 100%":        LocationData(_J + 446, "Utara Village",       "dlc_jungle"),
    "Jungle: Upgrade Purification Filter Tier 2": LocationData(_J + 447, "Utara Village",       "dlc_jungle"),
    "Jungle: Upgrade Purification Filter Tier 3": LocationData(_J + 448, "Utara Village",       "dlc_jungle"),
}

# --- Jungle Gun weapon upgrade locations ---
# 4 modes × 6 levels = 24 locations (level 1 is the base unlock, levels 2-6 are upgrades)
# Each mode is independent — upgrading one doesn't affect the others.
jungle_weapon_locations: Dict[str, LocationData] = {
    "Jungle Gun: Rifle Level 1":   LocationData(_J + 460, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Rifle Level 2":   LocationData(_J + 461, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Rifle Level 3":   LocationData(_J + 462, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Rifle Level 4":   LocationData(_J + 463, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Rifle Level 5":   LocationData(_J + 464, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Rifle Level 6":   LocationData(_J + 465, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Shotgun Level 1": LocationData(_J + 466, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Shotgun Level 2": LocationData(_J + 467, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Shotgun Level 3": LocationData(_J + 468, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Shotgun Level 4": LocationData(_J + 469, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Shotgun Level 5": LocationData(_J + 470, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Shotgun Level 6": LocationData(_J + 471, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Sniper Level 1":  LocationData(_J + 472, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Sniper Level 2":  LocationData(_J + 473, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Sniper Level 3":  LocationData(_J + 474, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Sniper Level 4":  LocationData(_J + 475, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Sniper Level 5":  LocationData(_J + 476, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Sniper Level 6":  LocationData(_J + 477, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Net Gun Level 1": LocationData(_J + 478, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Net Gun Level 2": LocationData(_J + 479, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Net Gun Level 3": LocationData(_J + 480, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Net Gun Level 4": LocationData(_J + 481, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Net Gun Level 5": LocationData(_J + 482, "Utara Village",  "dlc_jungle"),
    "Jungle Gun: Net Gun Level 6": LocationData(_J + 483, "Utara Village",  "dlc_jungle"),
}

# --- Jungle boss fish (in boss_fish_locations for fish_checks filter compatibility) ---
jungle_boss_fish_locations: Dict[str, LocationData] = {
    "First Catch: Giant Snapping Turtle": LocationData(_J + 490, "Utara Lake - Lower", "dlc_jungle"),
    "First Catch: Black Caiman":          LocationData(_J + 491, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Sulong":                LocationData(_J + 492, "Utara Lake - Upper", "dlc_jungle"),
    "First Catch: Stethacanthus":         LocationData(_J + 493, "Lakebed Sea",        "dlc_jungle"),
    "First Catch: Xiphactinus":           LocationData(_J + 494, "Lakebed Sea",        "dlc_jungle"),
    "First Catch: Basilosaurus":          LocationData(_J + 495, "Lakebed Sea",        "dlc_jungle"),
}

# === COOKED DISH RECIPE UNLOCK LOCATIONS ===
# These fire when a player researches a cooked dish for the first time (level 1).
# All 88 cooked dishes that have upgrade locations but were missing unlock locations.
# IDs: BASE_ID + 11000 .. BASE_ID + 11087
_RU = BASE_ID + 11000
recipe_unlock_locations.update({
    "Unlock Recipe: Agar Tokoroten":                        LocationData(_RU + 0,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Atlantic Bonito Curry":                 LocationData(_RU + 1,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Batfish Ricebowl":                      LocationData(_RU + 2,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Big-Eyed Scad and Soybean Paste Roast": LocationData(_RU + 3,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Black Vinegar Braised Parrotfish":      LocationData(_RU + 4,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Bluefin Tuna Rice Bowl":                LocationData(_RU + 5,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Boiled Asian Sheepshead Wrasse & Truffle": LocationData(_RU + 6, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Boiled Porbeagle Shark":                LocationData(_RU + 7,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Boiled Sailfish and Seaweed":           LocationData(_RU + 8,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Boiled Yellowback Fusilier":            LocationData(_RU + 9,  "Bancho Sushi", "recipe"),
    "Unlock Recipe: Boiled and Deep-Fried White Shrimp":    LocationData(_RU + 10, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Crimson Fish Roll":                     LocationData(_RU + 11, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Crystal Lobster Roll":                  LocationData(_RU + 12, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Deep-Fried Eggplant Shrimp Meatballs":  LocationData(_RU + 13, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Deep-Fried Red Lionfish":               LocationData(_RU + 14, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Deep-Fried Vegetables":                 LocationData(_RU + 15, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dried Stingray":                        LocationData(_RU + 16, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dumbo Takoyaki":                        LocationData(_RU + 17, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dusky Grouper Steak":                   LocationData(_RU + 18, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Falcatus Soybean Paste Soup":           LocationData(_RU + 19, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fried Habanero Fangtooth":              LocationData(_RU + 20, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fried Onion Cuttlefish":                LocationData(_RU + 21, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fried Rice with Sally Lightfoot Crab":  LocationData(_RU + 22, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fried Seahorses":                       LocationData(_RU + 23, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Fried Tomato and Snailfish":            LocationData(_RU + 24, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Great Barracuda Canape":                LocationData(_RU + 25, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Great Spider Crab Curry":               LocationData(_RU + 26, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Great Spider Crab and Cucumber Sushi":  LocationData(_RU + 27, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Grilled Antarctic Octopus & Truffle":   LocationData(_RU + 28, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Grilled Eel with Habanero":             LocationData(_RU + 29, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Hawaiian Poke":                         LocationData(_RU + 30, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Hot Pepper Tuna":                       LocationData(_RU + 31, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humphead Parrotfish Curry":             LocationData(_RU + 32, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Hyalonema Tuna Sashimi":                LocationData(_RU + 33, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Ice Fish Curry":                        LocationData(_RU + 34, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Latok Omelet":                          LocationData(_RU + 35, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Lobster Platter":                       LocationData(_RU + 36, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Mackerel Scad Hotdog":                  LocationData(_RU + 37, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Marlin and Soybean Paste Roast":        LocationData(_RU + 38, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Mianbao Xia":                           LocationData(_RU + 39, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Moonlight Bladderwrack Roll":           LocationData(_RU + 40, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Moray Eel Curry":                       LocationData(_RU + 41, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Narrow-barred Spanish Mackerel Arancini": LocationData(_RU + 42, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Narwhal Miso Soup":                     LocationData(_RU + 43, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Nasu Dengaku":                          LocationData(_RU + 44, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pelican Eel Jelly":                     LocationData(_RU + 45, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pickled Vegetables":                    LocationData(_RU + 46, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pikaia Ramen":                          LocationData(_RU + 47, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Plotosid Pie":                          LocationData(_RU + 48, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Pufferfish Dumpling Soup":              LocationData(_RU + 49, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Rice with Great Spider Crab Meat":      LocationData(_RU + 50, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Rice with Purple Sea Urchin Sushi":     LocationData(_RU + 51, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Rice with White Shrimp Meat":           LocationData(_RU + 52, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Roasted Capelin":                       LocationData(_RU + 53, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Roasted Tropical Fish and Garlic":      LocationData(_RU + 54, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Salt-grilled Redtoothed Triggerfish":   LocationData(_RU + 55, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Sea Toad and Cucumber Gunkan Sushi":    LocationData(_RU + 56, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seagrapes Special Sushi":               LocationData(_RU + 57, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seahorse Salad":                        LocationData(_RU + 58, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seahorse Udon":                         LocationData(_RU + 59, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seasoned Jellyfish":                    LocationData(_RU + 60, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seasoned Kajime":                       LocationData(_RU + 61, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seasoned Long-spine Porcupinefish Skin": LocationData(_RU + 62, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seasoned Waptia Fieldensis":            LocationData(_RU + 63, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seaweed Rolled Omelet":                 LocationData(_RU + 64, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Shark Karaage":                         LocationData(_RU + 65, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Skewered Cucumber":                     LocationData(_RU + 66, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Smallspotted Dart Kajime Soup":         LocationData(_RU + 67, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Smoked Atlantic Mackerel Scramble":     LocationData(_RU + 68, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Soy Sauce Marinated Crab":              LocationData(_RU + 69, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Special Fried Shrimp Sushi":            LocationData(_RU + 70, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Steamed Hyalonema Angler Fish":         LocationData(_RU + 71, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Stellate Puffer Nicogori":              LocationData(_RU + 72, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Stellate Puffer Special Sushi":         LocationData(_RU + 73, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Stir-fried Habanero Lobster":           LocationData(_RU + 74, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Striped Red Mullet Tangle Roll":        LocationData(_RU + 75, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Three-Colored Squid Roast":             LocationData(_RU + 76, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Tomato Egg Soup":                       LocationData(_RU + 77, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Trevally Nanbanzuke":                   LocationData(_RU + 78, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Trevally Sandwich":                     LocationData(_RU + 79, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Trout Sea Grapes Ricebowl":             LocationData(_RU + 80, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Truffle Blue Lobster Tail Sushi":       LocationData(_RU + 81, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Truffle Sailfish Tartare":              LocationData(_RU + 82, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Truffle Shark Sandwich":                LocationData(_RU + 83, "Bancho Sushi", "recipe"),
    "Unlock Recipe: White Trevally Kombu Ochazuke":         LocationData(_RU + 84, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Whole-Roasted Shark Head":              LocationData(_RU + 85, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Wrasse Curry":                          LocationData(_RU + 86, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Yellowfin Tuna Steak":                  LocationData(_RU + 87, "Bancho Sushi", "recipe"),
})

recipe_unlock_locations.update({
    "Unlock Recipe: Seagrapes Jellyfish Sushi": LocationData(BASE_ID + 820, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Tropical Fish Sushi Set": LocationData(BASE_ID + 821, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Vegetable Sushi": LocationData(BASE_ID + 822, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Sweet and Sour Stargazer": LocationData(BASE_ID + 823, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blobfish Spring Roll": LocationData(BASE_ID + 824, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Deep Fish Tempura": LocationData(BASE_ID + 825, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Comber Sandwich": LocationData(BASE_ID + 826, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humboldt Ink Pasta": LocationData(BASE_ID + 827, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Antarctic Octopus Carpaccio": LocationData(BASE_ID + 828, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Haddock Acqua Pazza": LocationData(BASE_ID + 829, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Arctic Cod Risotto": LocationData(BASE_ID + 830, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Peacock Squid Ripieni": LocationData(BASE_ID + 831, "Bancho Sushi", "recipe"),

    # --- Boss defeat recipes ---
    "Unlock Recipe: Stir-Fried Hermit Crab and Seaweed": LocationData(BASE_ID + 835, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Boiled Mantis Shrimp with Soy Paste": LocationData(BASE_ID + 836, "Bancho Sushi", "recipe"),
    "Unlock Recipe: White Shark Omelet": LocationData(BASE_ID + 837, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Clione Queen Soup": LocationData(BASE_ID + 838, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Steamed Wolf Eel": LocationData(BASE_ID + 839, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Goblin Shark Belly Roast": LocationData(BASE_ID + 840, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Phantom Jellyfish Jelly": LocationData(BASE_ID + 841, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Roasted Helicoprion Tail": LocationData(BASE_ID + 842, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Steamed Kronosaurus Tongue": LocationData(BASE_ID + 843, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Yawie Steamed Meat": LocationData(BASE_ID + 844, "Bancho Sushi", "recipe"),

    # --- Cooksta rank recipes ---
    "Unlock Recipe: Stellate Puffer Special Sushi (Cooksta Gold)": LocationData(BASE_ID + 848, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Seahorse Udon (Cooksta Platinum)": LocationData(BASE_ID + 849, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Atlantic Bonito Curry (Cooksta Platinum)": LocationData(BASE_ID + 850, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humphead Parrotfish Curry (Cooksta Platinum)": LocationData(BASE_ID + 851, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Nasu Dengaku (Cooksta Diamond)": LocationData(BASE_ID + 852, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Dumbo Takoyaki (Cooksta Diamond)": LocationData(BASE_ID + 853, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Great Barracuda Canape (Cooksta Diamond)": LocationData(BASE_ID + 854, "Bancho Sushi", "recipe"),
})

# === BOSS BATTLES ===
# All 14 bosses in the game
boss_locations: Dict[str, LocationData] = {
    # Chapter 1 bosses
    "Defeat: Giant Squid": LocationData(BASE_ID + 900, "Blue Hole - Deep"),
    "Defeat: Clione Queen": LocationData(BASE_ID + 901, "Blue Hole - Deep"),
    "Defeat: Truck Hermit Crab": LocationData(BASE_ID + 902, "Blue Hole - Deep"),
    # Chapter 2 bosses
    "Defeat: Giant Wolf Eel": LocationData(BASE_ID + 903, "Blue Hole - Deep"),
    "Defeat: Goblin Shark": LocationData(BASE_ID + 904, "Blue Hole - Deep"),
    "Defeat: Phantom Jellyfish": LocationData(BASE_ID + 905, "Blue Hole - Deep"),
    # Glacier bosses (Chapter 5 passage boss + Chapter 6 zone bosses)
    "Defeat: Giant Gadon": LocationData(BASE_ID + 906, "Glacial Passage"),
    "Defeat: Helicoprion": LocationData(BASE_ID + 907, "Glacier Zone"),
    "Defeat: Kronosaurus": LocationData(BASE_ID + 908, "Glacier Zone"),
    # Sea People Village bosses
    "Defeat: John Watson": LocationData(BASE_ID + 909, "Sea People Village"),
    # Ebirah is a Godzilla DLC exclusive boss — only included when dlc_godzilla is enabled.
    # The DLC story triggers after Chapter 5 (the "earthquake" event the next morning).
    # Placed in Blue Hole - Deep because the submarine fight happens in the deep ocean.
    "Defeat: Ebirah": LocationData(BASE_ID + 910, "Blue Hole - Deep", "dlc_godzilla"),
    # Optional/legendary bosses
    "Defeat: Great White Shark Klaus": LocationData(BASE_ID + 911, "Blue Hole - Deep"),
    "Defeat: Mantis Shrimp": LocationData(BASE_ID + 912, "Blue Hole - Deep"),
    "Defeat: Lusca": LocationData(BASE_ID + 913, "Sea People Village"),
    # Final boss
    "Defeat: Yawie (Final Boss)": LocationData(BASE_ID + 914, "Sea People Village"),
    # Torben is an Ichiban DLC exclusive boss
    "Defeat: Torben": LocationData(BASE_ID + 915, "Blue Hole - Deep", "dlc_ichiban"),
}

# === QUEST COMPLETION ===
quest_locations: Dict[str, LocationData] = {
    # --- Duff (main partner) ---
    "Quest: Complete Duff's First Request": LocationData(BASE_ID + 380, "Blue Hole - Shallow"),
    "Quest: Help Duff Investigate Blue Hole": LocationData(BASE_ID + 381, "Blue Hole - Shallow"),

    # --- Dr. Bacon (archaeologist/engineer) ---
    "Quest: Complete Dr. Bacon's First Request": LocationData(BASE_ID + 382, "Blue Hole - Shallow"),
    "Quest: Obtain Sea People Bracelet from Dr. Bacon": LocationData(BASE_ID + 383, "Blue Hole - Shallow"),
    "Quest: Obtain Bug Net from Dr. Bacon": LocationData(BASE_ID + 384, "Blue Hole - Shallow"),

    # --- Cobra (investor) ---
    "Quest: Complete Cobra's First Request": LocationData(BASE_ID + 385, "Bancho Sushi"),
    "Quest: Complete Cobra's VIP Challenge": LocationData(BASE_ID + 386, "Bancho Sushi"),

    # --- Bancho (head chef) ---
    "Quest: Complete Bancho's Training": LocationData(BASE_ID + 387, "Bancho Sushi"),

    # --- Otto (farmer) ---
    "Quest: Complete A Noisy Customer (Unlock Fish Farm)": LocationData(BASE_ID + 388, "Bancho Sushi"),

    # --- VIP guests ---
    # ── VIP Quests (independent — gated by ingredient access) ────────────────
    # Vincent Yamaoka appears multiple times as a VIP judge — each visit is separate
    "Quest: Serve Vincent Yamaoka - Visit 1":      LocationData(BASE_ID + 389, "Bancho Sushi"),
    "Quest: Serve Vincent Yamaoka - Visit 2":      LocationData(BASE_ID + 420, "Bancho Sushi"),
    "Quest: Serve Vincent Yamaoka - Visit 3":      LocationData(BASE_ID + 421, "Bancho Sushi"),
    # Sammy — needs Vegetable Farm (rice, eggplant, carrot)
    "Quest: Complete Good Ol' Vegetable Sushi!":   LocationData(BASE_ID + 422, "Vegetable Farm"),
    # Michael Bang — needs Shallow + Vegetable Farm (coral trout, titan triggerfish + rice)
    "Quest: Complete Michael Bang's Inspiration":  LocationData(BASE_ID + 423, "Bancho Sushi"),
    # Otto — needs Moray Eel (Shallow) + Turmeric (vendor/dispatch)
    "Quest: Complete Otto's Moray Eel Dish":       LocationData(BASE_ID + 424, "Bancho Sushi"),
    # Jango — needs Bluefin Tuna Chutoro (Mid) + Habanero (farm) + Sea Grape (Mid) + Sesame
    "Quest: Complete Jango's Secret Recipe":       LocationData(BASE_ID + 425, "Blue Hole - Mid"),
    # Mxmtoon — needs Green Sea Urchin (requires Sea People Gloves) + Bluefin Tuna + Cuttlefish
    "Quest: Serve Mxmtoon":                        LocationData(BASE_ID + 426, "Bancho Sushi"),
    # ── Cooking Competition Chain ─────────────────────────────────────────────
    # Each fight unlocks the next. Alex Cooper's defeat grants Cocktails Unlocked.
    # Vincent fight: needs Sea Grape (Limestone Cave = Blue Hole Mid) + White Spotted Jellyfish + Salt
    "Competition: Defeat Vincent Yamaoka":         LocationData(BASE_ID + 427, "Blue Hole - Mid"),
    # Wang Pang: needs Bluespotted Stargazer (Deep) + Egg (Chicken Farm) + Wheat + Olive Oil
    "Competition: Defeat Wang Pang":               LocationData(BASE_ID + 428, "Blue Hole - Deep"),
    # Alex Cooper: needs Cookiecutter Shark + Vampire Squid + Barreleye (all Deep) + Kelp
    "Competition: Defeat Alex Cooper":             LocationData(BASE_ID + 429, "Blue Hole - Deep"),
    # Pastro: needs Humboldt Squid (Glacier or Deep Night) + White Shrimp (Vents/Deep) + Wheat + Garlic
    "Competition: Defeat Pastro Antogiovani":      LocationData(BASE_ID + 430, "Blue Hole - Deep"),
    # ── Sub-Missions (category="sub_mission", toggle via include_sub_missions) ──
    # Prologue sub-missions
    # Sub-missions use BASE_ID+1700 onwards (safe: staff training ends at 1695)
    "Sub-Mission: Red Ecological Data":           LocationData(BASE_ID + 1700, "Blue Hole - Shallow", "sub_mission"),
    "Sub-Mission: Weaponsmith Duff":              LocationData(BASE_ID + 1701, "Bancho Sushi", "sub_mission"),
    # Chapter 1 sub-missions
    "Sub-Mission: A Dolphin's Request":           LocationData(BASE_ID + 1702, "Blue Hole - Shallow", "sub_mission"),
    "Sub-Mission: Not Enough Workers":            LocationData(BASE_ID + 1703, "Bancho Sushi", "sub_mission"),
    "Sub-Mission: A Scolding from Yoshie":        LocationData(BASE_ID + 1704, "Blue Hole - Shallow", "sub_mission"),
    "Sub-Mission: What Happened to the Dolphins?":LocationData(BASE_ID + 1705, "Blue Hole - Shallow", "sub_mission"),
    "Sub-Mission: Assisting Ellie":               LocationData(BASE_ID + 1706, "Blue Hole - Shallow", "sub_mission"),
    "Sub-Mission: Defeat Pirates":                LocationData(BASE_ID + 1707, "Blue Hole - Shallow", "sub_mission"),
    # Chapter 2 sub-missions
    "Sub-Mission: Reticent Girl":                 LocationData(BASE_ID + 1708, "Bancho Sushi", "sub_mission"),
    "Sub-Mission: Catch Clione":                  LocationData(BASE_ID + 1709, "Blue Hole - Deep", "sub_mission"),
    "Sub-Mission: Defeat the Clione Queen":       LocationData(BASE_ID + 1710, "Blue Hole - Deep", "sub_mission"),
    "Sub-Mission: Giant Stingray at Night":       LocationData(BASE_ID + 1711, "Blue Hole - Shallow", "sub_mission"),
    "Sub-Mission: Take Pictures of Manta Ray":    LocationData(BASE_ID + 1712, "Blue Hole - Mid", "sub_mission"),
    "Sub-Mission: Whale Cry":                     LocationData(BASE_ID + 1713, "Blue Hole - Mid", "sub_mission"),
    "Sub-Mission: Finding the Baby Whale":        LocationData(BASE_ID + 1714, "Blue Hole - Mid", "sub_mission"),
    "Sub-Mission: Stormy Night":                  LocationData(BASE_ID + 1715, "Bancho Sushi", "sub_mission"),
    # Chapter 3 sub-missions (Sea People Village)
    "Sub-Mission: Offer Flowers to King Long's Statue": LocationData(BASE_ID + 1716, "Sea People Village", "sub_mission"),
    "Sub-Mission: Deliver Mima's Lunch Boxes":    LocationData(BASE_ID + 1717, "Sea People Village", "sub_mission"),
    "Sub-Mission: Catch the Runaway Seahorses":   LocationData(BASE_ID + 1718, "Sea People Village", "sub_mission"),
    "Sub-Mission: Talk to Yami at the Game Parlor":LocationData(BASE_ID + 1719, "Sea People Village", "sub_mission"),
    "Sub-Mission: Pet Squid Selgio":              LocationData(BASE_ID + 1720, "Sea People Village", "sub_mission"),
    "Sub-Mission: Daphne's Whistle":              LocationData(BASE_ID + 1721, "Sea People Village", "sub_mission"),
    "Sub-Mission: Find the Children's Ball":      LocationData(BASE_ID + 1722, "Sea People Village", "sub_mission"),
    "Sub-Mission: Sea Person at the Workshop":    LocationData(BASE_ID + 1723, "Sea People Village", "sub_mission"),
    "Sub-Mission: Wedding Song Record":           LocationData(BASE_ID + 1724, "Sea People Village", "sub_mission"),
    "Sub-Mission: Repair Kinglong's Statue":      LocationData(BASE_ID + 1725, "Sea People Village", "sub_mission"),
    "Sub-Mission: Curious Child":                 LocationData(BASE_ID + 1726, "Sea People Village", "sub_mission"),
    # Chapter 6 sub-missions (Glacier Zone)
    "Sub-Mission: Lost Baby Manatee":             LocationData(BASE_ID + 1727, "Glacier Zone", "sub_mission"),
    "Sub-Mission: Trapped in the Glacial Cave":   LocationData(BASE_ID + 1728, "Glacial Passage", "sub_mission"),
    "Sub-Mission: Clara's Omani (Klaus Quest)":   LocationData(BASE_ID + 1729, "Blue Hole - Shallow", "sub_mission"),

    # --- Sea People Village ---
    "Quest: Gain Trust of Sea People": LocationData(BASE_ID + 395, "Sea People Village"),
    "Quest: Complete Niamo's Request": LocationData(BASE_ID + 396, "Sea People Village"),
    "Quest: Complete Linchen's Request": LocationData(BASE_ID + 397, "Sea People Village"),
    "Quest: Complete Ramo's Request": LocationData(BASE_ID + 398, "Sea People Village"),

    # IMPORTANT: This check grants Teleport Mirror (base item for teleport system)
    "Quest: Obtain Sea People Mirror (Teleport)": LocationData(BASE_ID + 399, "Sea People Village"),
}

# === TELEPORT POINTS ===
# Activating these unlocks teleport destinations (requires visiting the area first)
teleport_locations: Dict[str, LocationData] = {
    # Unlock glacier teleport (allows bypassing Sea People Village + Glacial Passage!)
    "Glacier: Activate Glacier Teleport Point": LocationData(BASE_ID + 750, "Glacier Zone"),
    
    # Unlock village teleport (alternative route to village)
    "Sea People Village: Activate Village Teleport Point": LocationData(BASE_ID + 751, "Sea People Village"),
    
    # Unlock deep blue hole teleport (useful for backtracking)
    "Deep Blue Hole: Activate Deep Teleport Point": LocationData(BASE_ID + 752, "Blue Hole - Deep"),
}

# === COLLECTIBLES & UPGRADES ===
collectible_locations: Dict[str, LocationData] = {
    # ✅ CONFIRMED via dump.cs: InstanceItemChest.SuccessInteract(BaseCharacter) is the hook
    "Find Treasure Chest 1": LocationData(BASE_ID + 950, "Blue Hole - Shallow"),
    "Find Treasure Chest 2": LocationData(BASE_ID + 951, "Blue Hole - Mid"),
    # NOTE: "Purchase Upgrade from Duff" REMOVED — DuffShopManager class does NOT exist in the game.
    # DuffShop is a PhoneAppList constant (value 14060002). Weapon crafting is already covered
    # by WeaponCraftPatch.cs via DREventTriggerManager.WeaponCraftTreeEventTrigger.
    # TODO: Add all treasure chests once chest locations are mapped
}

# === MINIGAMES ===
minigame_locations: Dict[str, LocationData] = {
    # ✅ CONFIRMED via dump.cs: SeahorseRaceTrackKey.Division enum: C=0 (Easy), B=1 (Medium), A=2 (Hard), S=3 (Expert)
    # Hook: SeahorseRaceSessionPlay.OnGoal(int lane), filter lane==4 (playerLane const), read _session.trackData.trackKey._division
    "Beat Seahorse Racing - Easy":   LocationData(BASE_ID + 600, "Sea People Village", "minigame"),
    "Beat Seahorse Racing - Medium": LocationData(BASE_ID + 601, "Sea People Village", "minigame"),
    "Beat Seahorse Racing - Hard":   LocationData(BASE_ID + 602, "Sea People Village", "minigame"),
    "Beat Seahorse Racing - Expert": LocationData(BASE_ID + 603, "Sea People Village", "minigame"),
    "Complete All Card Mini-games":  LocationData(BASE_ID + 610, "Bancho Sushi", "minigame"),
    # TODO: Add other minigames
}

# === COOKSTA (Social Media App) ===
# Cooksta posts and follower milestones
cooksta_locations: Dict[str, LocationData] = {
    # === COOKSTA RANK REQUIREMENTS ===
    # Each individual requirement for a rank is a separate check.
    # The RANK ITSELF is a Progressive Cooksta Rank item (received as AP reward).
    #
    # Bronze rank requirements:
    "Cooksta: 10 Followers":                  LocationData(BASE_ID + 400, "Bancho Sushi", "cooksta"),

    # Silver rank requirements:
    "Cooksta: 20 Followers":                  LocationData(BASE_ID + 401, "Bancho Sushi", "cooksta"),
    "Cooksta: 2 Researched Recipes":          LocationData(BASE_ID + 402, "Bancho Sushi", "cooksta"),

    # Gold rank requirements:
    "Cooksta: 100 Followers":                 LocationData(BASE_ID + 403, "Bancho Sushi", "cooksta"),
    "Cooksta: 125 Best Taste":                LocationData(BASE_ID + 404, "Bancho Sushi", "cooksta"),
    "Cooksta: 5 Researched Recipes":          LocationData(BASE_ID + 405, "Bancho Sushi", "cooksta"),

    # Platinum rank requirements:
    "Cooksta: 200 Followers":                 LocationData(BASE_ID + 406, "Bancho Sushi", "cooksta"),
    "Cooksta: 250 Best Taste":                LocationData(BASE_ID + 407, "Bancho Sushi", "cooksta"),
    "Cooksta: 19 Researched Recipes":         LocationData(BASE_ID + 408, "Bancho Sushi", "cooksta"),

    # Diamond rank requirements:
    "Cooksta: 720 Followers":                 LocationData(BASE_ID + 409, "Bancho Sushi", "cooksta"),
    "Cooksta: 375 Best Taste":                LocationData(BASE_ID + 410, "Bancho Sushi", "cooksta"),
    "Cooksta: 32 Researched Recipes":         LocationData(BASE_ID + 411, "Bancho Sushi", "cooksta"),
}

# === ECOWATCHER (Marine Life App) ===
# Each mission completion grants an AP check instead of research points.
# Missions unlock progressively by chapter (Ch2 = Blue Hole, Ch5-6 = Glacial/Vents).
ecowatcher_locations: Dict[str, LocationData] = {

    # --- Chapter 2: Blue Hole Missions ---

    # Starfish Research series (5 tiers)
    "Ecowatcher: Research Starfish 1": LocationData(BASE_ID + 450, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Starfish 2": LocationData(BASE_ID + 451, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Starfish 3": LocationData(BASE_ID + 452, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Starfish 4": LocationData(BASE_ID + 453, "Blue Hole - Mid", "ecowatcher"),
    "Ecowatcher: Research Starfish 5": LocationData(BASE_ID + 454, "Blue Hole - Mid", "ecowatcher"),

    # Shell Research series (5 tiers)
    "Ecowatcher: Research Shell 1": LocationData(BASE_ID + 455, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Shell 2": LocationData(BASE_ID + 456, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Shell 3": LocationData(BASE_ID + 457, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Shell 4": LocationData(BASE_ID + 458, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Shell 5": LocationData(BASE_ID + 459, "Blue Hole - Shallow", "ecowatcher"),

    # Marine Plants Research series (5 tiers, last 2 unlock Ch3)
    "Ecowatcher: Research Marine Plants 1": LocationData(BASE_ID + 460, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Marine Plants 2": LocationData(BASE_ID + 461, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Marine Plants 3": LocationData(BASE_ID + 462, "Blue Hole - Mid", "ecowatcher"),
    "Ecowatcher: Research Marine Plants 4": LocationData(BASE_ID + 463, "Blue Hole - Mid", "ecowatcher"),
    "Ecowatcher: Research Marine Plants 5": LocationData(BASE_ID + 464, "Blue Hole - Mid", "ecowatcher"),

    # Fossils Research series (3 tiers)
    "Ecowatcher: Research Fossils 1": LocationData(BASE_ID + 465, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Fossils 2": LocationData(BASE_ID + 466, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Research Fossils 3": LocationData(BASE_ID + 467, "Blue Hole - Mid", "ecowatcher"),

    # Remove Jellyfish series (4 tiers)
    "Ecowatcher: Remove Jellyfish 1": LocationData(BASE_ID + 468, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Remove Jellyfish 2": LocationData(BASE_ID + 469, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Remove Jellyfish 3": LocationData(BASE_ID + 470, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Remove Jellyfish 4": LocationData(BASE_ID + 471, "Blue Hole - Mid", "ecowatcher"),

    # Overpopulated Invasive Fish series (5 tiers)
    "Ecowatcher: Cull Invasive Fish 1": LocationData(BASE_ID + 472, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Cull Invasive Fish 2": LocationData(BASE_ID + 473, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Cull Invasive Fish 3": LocationData(BASE_ID + 474, "Blue Hole - Mid", "ecowatcher"),
    "Ecowatcher: Cull Invasive Fish 4": LocationData(BASE_ID + 475, "Blue Hole - Mid", "ecowatcher"),
    "Ecowatcher: Cull Invasive Fish 5": LocationData(BASE_ID + 476, "Blue Hole - Mid", "ecowatcher"),

    # --- Chapter 3: Blue Hole Seaweed Map ---
    "Ecowatcher: Cull Invasive Fish (Seaweed Map)": LocationData(BASE_ID + 477, "Blue Hole - Mid", "ecowatcher"),

    # --- Chapter 5: Glacial Area ---
    "Ecowatcher: Investigate Regional Ecology 1": LocationData(BASE_ID + 478, "Glacial Passage", "ecowatcher"),

    # --- Chapter 6: Glacial Area ---
    "Ecowatcher: Investigate Glacial Marine Plants 1": LocationData(BASE_ID + 479, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Collect Glacial Clams 1": LocationData(BASE_ID + 480, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Defeat Invasive Starfish 1": LocationData(BASE_ID + 481, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Investigate Sea People's Artifact 1": LocationData(BASE_ID + 482, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Investigate Dangerous Gemstones 1": LocationData(BASE_ID + 483, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Investigate Regional Ecology 2": LocationData(BASE_ID + 484, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Investigate Glacial Marine Plants 2": LocationData(BASE_ID + 485, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Collect Glacial Clams 2": LocationData(BASE_ID + 486, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Defeat Invasive Starfish 2": LocationData(BASE_ID + 487, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Investigate Sea People's Artifact 2": LocationData(BASE_ID + 488, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Investigate Dangerous Gemstones 2": LocationData(BASE_ID + 489, "Glacier Zone", "ecowatcher"),
    "Ecowatcher: Investigate Glacial Marine Plants 3": LocationData(BASE_ID + 490, "Glacier Zone", "ecowatcher"),

    # --- Chapter 6: Hydrothermal Vents ---
    "Ecowatcher: Investigate Regional Ecology 3": LocationData(BASE_ID + 491, "Hydrothermal Vents", "ecowatcher"),
    "Ecowatcher: Investigate Dangerous Gemstones 3": LocationData(BASE_ID + 492, "Hydrothermal Vents", "ecowatcher"),

    # --- Marinca collection: 50% and 100% per page ---
    # (First catch is already a separate fish location check)
    # Shallow Blue Hole Marinca pages
    "Marinca: Starfish Page 50%": LocationData(BASE_ID + 493, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Starfish Page 100%": LocationData(BASE_ID + 494, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Jellyfish Page 50%": LocationData(BASE_ID + 495, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Jellyfish Page 100%": LocationData(BASE_ID + 496, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Shells Page 50%": LocationData(BASE_ID + 497, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Shells Page 100%": LocationData(BASE_ID + 498, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Seahorses Page 50%": LocationData(BASE_ID + 499, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Seahorses Page 100%": LocationData(BASE_ID + 1100, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Crabs Page 50%": LocationData(BASE_ID + 1101, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Crabs Page 100%": LocationData(BASE_ID + 1102, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Marine Plants Page 50%": LocationData(BASE_ID + 1103, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Marine Plants Page 100%": LocationData(BASE_ID + 1104, "Blue Hole - Shallow", "ecowatcher"),
    "Marinca: Fossils Page 50%": LocationData(BASE_ID + 1105, "Blue Hole - Mid", "ecowatcher"),
    "Marinca: Fossils Page 100%": LocationData(BASE_ID + 1106, "Blue Hole - Mid", "ecowatcher"),
    # Glacial Marinca pages
    "Marinca: Glacial Plants Page 50%": LocationData(BASE_ID + 1107, "Glacier Zone", "ecowatcher"),
    "Marinca: Glacial Plants Page 100%": LocationData(BASE_ID + 1108, "Glacier Zone", "ecowatcher"),
    "Marinca: Glacial Clams Page 50%": LocationData(BASE_ID + 1109, "Glacier Zone", "ecowatcher"),
    "Marinca: Glacial Clams Page 100%": LocationData(BASE_ID + 1110, "Glacier Zone", "ecowatcher"),
    "Marinca: Glacial Starfish Page 50%": LocationData(BASE_ID + 1111, "Glacier Zone", "ecowatcher"),
    "Marinca: Glacial Starfish Page 100%": LocationData(BASE_ID + 1112, "Glacier Zone", "ecowatcher"),
    "Marinca: Artifacts Page 50%": LocationData(BASE_ID + 1113, "Glacier Zone", "ecowatcher"),
    "Marinca: Artifacts Page 100%": LocationData(BASE_ID + 1114, "Glacier Zone", "ecowatcher"),
    "Marinca: Gemstones Page 50%": LocationData(BASE_ID + 1115, "Glacier Zone", "ecowatcher"),
    "Marinca: Gemstones Page 100%": LocationData(BASE_ID + 1116, "Glacier Zone", "ecowatcher"),
    # Completion milestones (kept as useful overall goals)
    "Ecowatcher: Complete All Marinca": LocationData(BASE_ID + 1117, "Blue Hole - Shallow", "ecowatcher"),
    "Ecowatcher: Complete All Fish": LocationData(BASE_ID + 1118, "Blue Hole - Shallow", "ecowatcher"),
    # Marinca Completion Trophy — awarded when all Marinca entries are complete
    # Placed in Hydrothermal Vents since the last possible entry (Ruby Seadragon) is there
    # This item gates the Lusca boss fight (secret post-game boss)
    "Marinca: Complete All Entries (Trophy)": LocationData(BASE_ID + 1125, "Hydrothermal Vents", "ecowatcher"),
}

# === PHOTOGRAPHY / PICTURES ===
# Tako's photography missions and special photo spots
photography_locations: Dict[str, LocationData] = {
    # ── Wildlife Photography Spots ──────────────────────────────────────────
    # Each requires the Underwater Camera (gated in rules.py).
    # Spots are contextual — triggered by missions or random map seeds.
    "Photo: Pink Dolphin":                  LocationData(BASE_ID + 500, "Blue Hole - Shallow", "photography"),
    "Photo: Manta Ray":                     LocationData(BASE_ID + 501, "Blue Hole - Shallow", "photography"),  # night mission with lighting
    "Photo: Loggerhead Turtle":             LocationData(BASE_ID + 502, "Blue Hole - Shallow", "photography"),  # after seaweed collector mission
    "Photo: Baby Humpback Whale":           LocationData(BASE_ID + 503, "Blue Hole - Shallow", "photography"),  # whale rescue chain
    "Photo: Underwater Lake":               LocationData(BASE_ID + 504, "Blue Hole - Mid",     "photography"),  # caves during Curious Child
    "Photo: Opah":                          LocationData(BASE_ID + 505, "Blue Hole - Deep",    "photography"),
    "Photo: Bathynomus (Giant Isopod)":     LocationData(BASE_ID + 506, "Glacier Zone",        "photography"),
    "Photo: Lion's Mane Jellyfish":         LocationData(BASE_ID + 507, "Glacier Zone",        "photography"),
    "Photo: Southern Right Whale Dolphin":  LocationData(BASE_ID + 508, "Glacier Zone",        "photography"),
    "Photo: Beluga Whale":                  LocationData(BASE_ID + 509, "Glacier Zone",        "photography"),
    "Photo: Arandaspis":                    LocationData(BASE_ID + 510, "Hydrothermal Vents",  "photography"),
    "Photo: Coelacanth":                    LocationData(BASE_ID + 511, "Hydrothermal Vents",  "photography"),
    # ── Sea People Murals (Dr. Bacon's quest) ──────────────────────────────
    # 8 murals total — completing all triggers a boat cutscene with Dr. Bacon
    "Photo: Sea People Mural 1 (King Long)":     LocationData(BASE_ID + 512, "Blue Hole - Shallow", "photography"),  # Ch1 Beyond the Rock Pile
    "Photo: Sea People Mural 2":                 LocationData(BASE_ID + 513, "Glacial Passage",     "photography"),
    "Photo: Sea People Mural 3":                 LocationData(BASE_ID + 514, "Glacial Passage",     "photography"),
    "Photo: Sea People Mural 4":                 LocationData(BASE_ID + 515, "Glacial Passage",     "photography"),
    "Photo: Sea People Mural 5":                 LocationData(BASE_ID + 516, "Glacial Passage",     "photography"),  # missable
    "Photo: Sea People Mural 6":                 LocationData(BASE_ID + 517, "Glacial Passage",     "photography"),
    "Photo: Sea People Mural 7":                 LocationData(BASE_ID + 518, "Glacial Passage",     "photography"),
    "Photo: Sea People Mural 8":                 LocationData(BASE_ID + 519, "Glacial Passage",     "photography"),  # missable (Giant Gadon room)
}

# === CHALLENGES ===
# Challenge locations intentionally omitted — challenge content was placeholder/invented
# and not verified as real in-game locations. ChallengePatch.cs also deleted.
# Do not re-add without confirming real game challenge TIDs via dump.cs.

# === VEGETABLE FARM ===
# Vegetable garden farming milestones (unlocked via "Unlock Vegetable Farm")
farming_locations: Dict[str, LocationData] = {
    # Garden upgrades
    "Veg Farm: Upgrade Garden Tier 1": LocationData(BASE_ID + 1000, "Vegetable Farm", "farming"),
    "Veg Farm: Upgrade Garden Tier 2": LocationData(BASE_ID + 1001, "Vegetable Farm", "farming"),
    "Veg Farm: Upgrade Garden Tier 3": LocationData(BASE_ID + 1002, "Vegetable Farm", "farming"),

    # Crop unlocks (first harvest of each vegetable crop)
    "Veg Farm: First Harvest - Tomato": LocationData(BASE_ID + 1010, "Vegetable Farm", "farming"),
    "Veg Farm: First Harvest - Lettuce": LocationData(BASE_ID + 1011, "Vegetable Farm", "farming"),
    "Veg Farm: First Harvest - Cucumber": LocationData(BASE_ID + 1012, "Vegetable Farm", "farming"),
    "Veg Farm: First Harvest - Onion": LocationData(BASE_ID + 1013, "Vegetable Farm", "farming"),
    "Veg Farm: First Harvest - Wasabi": LocationData(BASE_ID + 1014, "Vegetable Farm", "farming"),
    "Veg Farm: First Harvest - Ginger": LocationData(BASE_ID + 1015, "Vegetable Farm", "farming"),
    "Veg Farm: First Harvest - Seaweed": LocationData(BASE_ID + 1016, "Vegetable Farm", "farming"),
    "Veg Farm: First Harvest - Buckwheat": LocationData(BASE_ID + 1017, "Vegetable Farm", "dlc_ichiban"),
    "Veg Farm: First Harvest - Perilla": LocationData(BASE_ID + 1018, "Vegetable Farm", "farming"),

    # Vegetable farm milestones
    "Veg Farm: Harvest 50 Total Crops": LocationData(BASE_ID + 1020, "Vegetable Farm", "farming"),
    "Veg Farm: Harvest 100 Total Crops": LocationData(BASE_ID + 1021, "Vegetable Farm", "farming"),
    "Veg Farm: Harvest 250 Total Crops": LocationData(BASE_ID + 1022, "Vegetable Farm", "farming"),
    "Veg Farm: Grow All Crop Types": LocationData(BASE_ID + 1023, "Vegetable Farm", "farming"),
}

# === CHICKEN FARM ===
# Chicken farm milestones (unlocked via "Unlock Chicken Farm", same physical location as veg farm)
chicken_farm_locations: Dict[str, LocationData] = {
    # Coop upgrades
    "Chicken Farm: Upgrade Coop Tier 1": LocationData(BASE_ID + 1030, "Chicken Farm", "chicken_farm"),
    "Chicken Farm: Upgrade Coop Tier 2": LocationData(BASE_ID + 1031, "Chicken Farm", "chicken_farm"),
    "Chicken Farm: Upgrade Coop Tier 3": LocationData(BASE_ID + 1032, "Chicken Farm", "chicken_farm"),

    # First egg collections
    "Chicken Farm: First Egg Collected": LocationData(BASE_ID + 1040, "Chicken Farm", "chicken_farm"),
    "Chicken Farm: Collect 10 Eggs": LocationData(BASE_ID + 1041, "Chicken Farm", "chicken_farm"),
    "Chicken Farm: Collect 50 Eggs": LocationData(BASE_ID + 1042, "Chicken Farm", "chicken_farm"),
    "Chicken Farm: Collect 100 Eggs": LocationData(BASE_ID + 1043, "Chicken Farm", "chicken_farm"),

    # Chicken farm milestones
    "Chicken Farm: Max Out Egg Quality": LocationData(BASE_ID + 1050, "Chicken Farm", "chicken_farm"),
    "Chicken Farm: Raise All Chicken Types": LocationData(BASE_ID + 1051, "Chicken Farm", "chicken_farm"),
}

# === FISH FARM ===
# Fish farm management and breeding
fish_farm_locations: Dict[str, LocationData] = {
    # Fish farm area unlocks
    # ✅ CONFIRMED via dump.cs: FishFarmAreaType enum: None=0, A=1, B=2, C=3, D=4, E=5, F=6, G=7, H=8
    # The first area (A) is unlocked via the "A Noisy Customer" quest (covered in quest_locations).
    # Areas B-H are purchased in-game. Base game likely uses A-D; E-H may be future/DLC content.
    # Hook: SaveData.FishFarmAreaSave.set_IsOpen(ObscuredBool) in FarmPatch.cs
    "Fish Farm: Unlock Area A": LocationData(BASE_ID + 651, "Fish Farm", "fish_farm"),
    "Fish Farm: Unlock Area B": LocationData(BASE_ID + 652, "Fish Farm", "fish_farm"),
    "Fish Farm: Unlock Area C": LocationData(BASE_ID + 653, "Fish Farm", "fish_farm"),
    "Fish Farm: Unlock Area D": LocationData(BASE_ID + 654, "Fish Farm", "fish_farm"),
    "Fish Farm: Unlock Area E": LocationData(BASE_ID + 655, "Fish Farm", "fish_farm"),
    "Fish Farm: Unlock Area F": LocationData(BASE_ID + 656, "Fish Farm", "fish_farm"),
    "Fish Farm: Unlock Area G": LocationData(BASE_ID + 657, "Fish Farm", "fish_farm"),
    "Fish Farm: Unlock Area H": LocationData(BASE_ID + 658, "Fish Farm", "fish_farm"),

    # Fish farm milestones (kept from original design)
    "Fish Farm: Raise 10 Fish to Adulthood": LocationData(BASE_ID + 670, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 25 Fish to Adulthood": LocationData(BASE_ID + 671, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 50 Fish to Adulthood": LocationData(BASE_ID + 672, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 5 Different Species": LocationData(BASE_ID + 673, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 10 Different Species": LocationData(BASE_ID + 674, "Fish Farm", "fish_farm"),
    "Fish Farm: Max Out Fish Quality": LocationData(BASE_ID + 675, "Fish Farm", "fish_farm"),
}

# === INGREDIENT FIRST-FINDS ===
# First time collecting each ingredient = AP check.
# Farm ingredients, sea plants, and rare forageables are included.
# Shop-bought condiments (Soy Sauce, Olive Oil, etc.) are excluded — too trivial.
# Boss-drop ingredients are covered by boss defeat checks.
ingredient_locations: Dict[str, LocationData] = {
    # --- Sea plants (found while diving) ---
    "First Find: Agar":               LocationData(BASE_ID + 1300, "Blue Hole - Shallow", ""),
    "First Find: Kajime":             LocationData(BASE_ID + 1301, "Blue Hole - Shallow", ""),
    "First Find: Seaweed":            LocationData(BASE_ID + 1302, "Blue Hole - Shallow", ""),
    "First Find: Kelp":               LocationData(BASE_ID + 1303, "Blue Hole - Mid", ""),
    "First Find: Sea Grape":          LocationData(BASE_ID + 1304, "Blue Hole - Deep", ""),
    "First Find: Bladderwrack":       LocationData(BASE_ID + 1305, "Glacial Passage", ""),
    "First Find: Hyalonema":          LocationData(BASE_ID + 1306, "Glacial Passage", ""),
    "First Find: Southern Bull Kelp": LocationData(BASE_ID + 1307, "Glacier Zone", ""),
    "First Find: Black Coral":        LocationData(BASE_ID + 1308, "Glacier Zone", ""),
    "First Find: Buckbean":           LocationData(BASE_ID + 1309, "Hydrothermal Vents", ""),

    # --- Rare forageables (from special locations/vendors) ---
    "First Find: Truffle":            LocationData(BASE_ID + 1310, "Bancho Sushi", ""),
    "First Find: Rainbow Cap":        LocationData(BASE_ID + 1311, "Bancho Sushi", ""),

    # --- Farm ingredients (first harvest — separate from farm upgrade checks) ---
    "First Find: Rice":               LocationData(BASE_ID + 1312, "Vegetable Farm", "farming"),
    "First Find: Bean":               LocationData(BASE_ID + 1313, "Vegetable Farm", "farming"),
    "First Find: Buckwheat":          LocationData(BASE_ID + 1314, "Vegetable Farm", "dlc_ichiban"),
    "First Find: Carrot":             LocationData(BASE_ID + 1315, "Vegetable Farm", "farming"),
    "First Find: Cherry Tomato":      LocationData(BASE_ID + 1316, "Vegetable Farm", "farming"),
    "First Find: Cucumber":           LocationData(BASE_ID + 1317, "Vegetable Farm", "farming"),
    "First Find: Eggplant":           LocationData(BASE_ID + 1318, "Vegetable Farm", "farming"),
    "First Find: Garlic":             LocationData(BASE_ID + 1319, "Vegetable Farm", "farming"),
    "First Find: Habanero":           LocationData(BASE_ID + 1320, "Vegetable Farm", "farming"),
    "First Find: Onion":              LocationData(BASE_ID + 1321, "Vegetable Farm", "farming"),
    "First Find: Wheat":              LocationData(BASE_ID + 1322, "Vegetable Farm", "farming"),

    # --- Chicken Farm ingredients ---
    "First Find: Egg":                LocationData(BASE_ID + 1323, "Chicken Farm", "chicken_farm"),
    "First Find: Grade A Egg":        LocationData(BASE_ID + 1324, "Chicken Farm", "chicken_farm"),
}

# === WEAPON CRAFTING (Duff's Weapon Shop) ===
# Each named weapon variant = 1 craft location check.
# Locations are in Bancho Sushi (accessed via the phone app).
# Blueprint unlocks gate which weapons can be crafted.
# Base ID block: 1400-1599 (200 slots for all weapon variants)
weapon_locations: Dict[str, LocationData] = {
    # --- Basic Underwater Rifle tree (16 variants) ---
    "Craft: Basic Underwater Rifle":      LocationData(BASE_ID + 1400, "Bancho Sushi", "weapon"),
    "Craft: Underwater Rifle II":         LocationData(BASE_ID + 1401, "Bancho Sushi", "weapon"),
    "Craft: Underwater Rifle III":        LocationData(BASE_ID + 1402, "Bancho Sushi", "weapon"),
    "Craft: Death Rifle":                 LocationData(BASE_ID + 1403, "Bancho Sushi", "weapon"),
    "Craft: Flame Rifle I":               LocationData(BASE_ID + 1404, "Bancho Sushi", "weapon"),
    "Craft: Flame Rifle II":              LocationData(BASE_ID + 1405, "Bancho Sushi", "weapon"),
    "Craft: Explosive Rifle":             LocationData(BASE_ID + 1406, "Bancho Sushi", "weapon"),
    "Craft: Tranquilizer Rifle":          LocationData(BASE_ID + 1407, "Bancho Sushi", "weapon"),
    "Craft: Poison Rifle I":              LocationData(BASE_ID + 1408, "Bancho Sushi", "weapon"),
    "Craft: Poison Rifle II":             LocationData(BASE_ID + 1409, "Bancho Sushi", "weapon"),
    "Craft: Hell Poison Rifle":           LocationData(BASE_ID + 1410, "Bancho Sushi", "weapon"),
    "Craft: Lightning Rifle I":           LocationData(BASE_ID + 1411, "Bancho Sushi", "weapon"),
    "Craft: Lightning Rifle II":          LocationData(BASE_ID + 1412, "Bancho Sushi", "weapon"),
    "Craft: Shock Rifle I":               LocationData(BASE_ID + 1413, "Bancho Sushi", "weapon"),
    "Craft: Shock Rifle II":              LocationData(BASE_ID + 1414, "Bancho Sushi", "weapon"),
    "Craft: Thunderbolt Rifle":           LocationData(BASE_ID + 1415, "Bancho Sushi", "weapon"),

    # --- Small Net Gun tree (4 variants) ---
    "Craft: Small Net Gun":               LocationData(BASE_ID + 1416, "Bancho Sushi", "weapon"),
    "Craft: Medium Net Gun":              LocationData(BASE_ID + 1417, "Bancho Sushi", "weapon"),
    "Craft: Large Net Gun":               LocationData(BASE_ID + 1418, "Bancho Sushi", "weapon"),
    "Craft: Steel Net Gun":               LocationData(BASE_ID + 1419, "Bancho Sushi", "weapon"),

    # --- Hush Dart tree (2 variants) ---
    "Craft: Hush Dart":                   LocationData(BASE_ID + 1420, "Bancho Sushi", "weapon"),
    "Craft: Enhanced Hush Dart":          LocationData(BASE_ID + 1421, "Bancho Sushi", "weapon"),

    # --- Triple Axel tree (13 variants) ---
    "Craft: Triple Axel":                 LocationData(BASE_ID + 1422, "Bancho Sushi", "weapon"),
    "Craft: Quattro Axel":                LocationData(BASE_ID + 1423, "Bancho Sushi", "weapon"),
    "Craft: Quattro Axel II":             LocationData(BASE_ID + 1424, "Bancho Sushi", "weapon"),
    "Craft: Penta Axel":                  LocationData(BASE_ID + 1425, "Bancho Sushi", "weapon"),
    "Craft: Flame Triple Axel":           LocationData(BASE_ID + 1426, "Bancho Sushi", "weapon"),
    "Craft: Flame Triple Axel II":        LocationData(BASE_ID + 1427, "Bancho Sushi", "weapon"),
    "Craft: Explosive Triple Axel":       LocationData(BASE_ID + 1428, "Bancho Sushi", "weapon"),
    "Craft: Tranquilizer Triple Axel":    LocationData(BASE_ID + 1429, "Bancho Sushi", "weapon"),
    "Craft: Poison Triple Axel":          LocationData(BASE_ID + 1430, "Bancho Sushi", "weapon"),
    "Craft: Poison Triple Axel II":       LocationData(BASE_ID + 1431, "Bancho Sushi", "weapon"),
    "Craft: Hell Poison Triple Axel":     LocationData(BASE_ID + 1432, "Bancho Sushi", "weapon"),
    "Craft: Lightning Triple Axel":       LocationData(BASE_ID + 1433, "Bancho Sushi", "weapon"),
    "Craft: Shock Triple Axel":           LocationData(BASE_ID + 1434, "Bancho Sushi", "weapon"),
    "Craft: Shock Triple Axel II":        LocationData(BASE_ID + 1435, "Bancho Sushi", "weapon"),
    "Craft: Thunderbolt Triple Axel":     LocationData(BASE_ID + 1436, "Bancho Sushi", "weapon"),

    # --- Red Sniper Rifle tree (13 variants) ---
    "Craft: Red Sniper Rifle":            LocationData(BASE_ID + 1437, "Bancho Sushi", "weapon"),
    "Craft: Red Sniper Rifle II":         LocationData(BASE_ID + 1438, "Bancho Sushi", "weapon"),
    "Craft: Red Sniper Rifle III":        LocationData(BASE_ID + 1439, "Bancho Sushi", "weapon"),
    "Craft: Death Sniper Rifle":          LocationData(BASE_ID + 1440, "Bancho Sushi", "weapon"),
    "Craft: Flame Sniper Rifle I":        LocationData(BASE_ID + 1441, "Bancho Sushi", "weapon"),
    "Craft: Flame Sniper Rifle II":       LocationData(BASE_ID + 1442, "Bancho Sushi", "weapon"),
    "Craft: Explosive Sniper Rifle":      LocationData(BASE_ID + 1443, "Bancho Sushi", "weapon"),
    "Craft: Tranquilizer Mosin-Nagant":   LocationData(BASE_ID + 1444, "Bancho Sushi", "weapon"),
    "Craft: Poison Sniper Rifle I":       LocationData(BASE_ID + 1445, "Bancho Sushi", "weapon"),
    "Craft: Poison Sniper Rifle II":      LocationData(BASE_ID + 1446, "Bancho Sushi", "weapon"),
    "Craft: Hell Poison Sniper Rifle":    LocationData(BASE_ID + 1447, "Bancho Sushi", "weapon"),
    "Craft: Lightning Sniper Rifle I":    LocationData(BASE_ID + 1448, "Bancho Sushi", "weapon"),
    "Craft: Lightning Sniper Rifle II":   LocationData(BASE_ID + 1449, "Bancho Sushi", "weapon"),
    "Craft: Shock Sniper Rifle I":        LocationData(BASE_ID + 1450, "Bancho Sushi", "weapon"),
    "Craft: Shock Sniper Rifle II":       LocationData(BASE_ID + 1451, "Bancho Sushi", "weapon"),
    "Craft: Thunderbolt Sniper Rifle":    LocationData(BASE_ID + 1452, "Bancho Sushi", "weapon"),

    # --- Sticky Bomb Gun tree (12 variants) ---
    "Craft: Sticky Bomb Gun":             LocationData(BASE_ID + 1453, "Bancho Sushi", "weapon"),
    "Craft: Sticky Bomb Gun II":          LocationData(BASE_ID + 1454, "Bancho Sushi", "weapon"),
    "Craft: Sticky Bomb Gun III":         LocationData(BASE_ID + 1455, "Bancho Sushi", "weapon"),
    "Craft: Sticky Mine Launcher I":      LocationData(BASE_ID + 1456, "Bancho Sushi", "weapon"),
    "Craft: Sticky Mine Launcher II":     LocationData(BASE_ID + 1457, "Bancho Sushi", "weapon"),
    "Craft: Sticky Tranquilizing Bomb Gun": LocationData(BASE_ID + 1458, "Bancho Sushi", "weapon"),
    "Craft: Poison Mine Launcher":        LocationData(BASE_ID + 1459, "Bancho Sushi", "weapon"),
    "Craft: Poison Mine Launcher II":     LocationData(BASE_ID + 1460, "Bancho Sushi", "weapon"),
    "Craft: Lightning Mine Launcher I":   LocationData(BASE_ID + 1461, "Bancho Sushi", "weapon"),
    "Craft: Lightning Mine Launcher II":  LocationData(BASE_ID + 1462, "Bancho Sushi", "weapon"),
    "Craft: Shock Mine Launcher I":       LocationData(BASE_ID + 1463, "Bancho Sushi", "weapon"),
    "Craft: Shock Mine Launcher II":      LocationData(BASE_ID + 1464, "Bancho Sushi", "weapon"),

    # --- Grenade Launcher tree (9 variants) ---
    "Craft: Grenade Launcher":            LocationData(BASE_ID + 1465, "Bancho Sushi", "weapon"),
    "Craft: Grenade Launcher II":         LocationData(BASE_ID + 1466, "Bancho Sushi", "weapon"),
    "Craft: Grenade Launcher III":        LocationData(BASE_ID + 1467, "Bancho Sushi", "weapon"),
    "Craft: Tranquilizer Gas Bomb Launcher": LocationData(BASE_ID + 1468, "Bancho Sushi", "weapon"),
    "Craft: Poison Launcher":             LocationData(BASE_ID + 1469, "Bancho Sushi", "weapon"),
    "Craft: Gravity Launcher":            LocationData(BASE_ID + 1470, "Bancho Sushi", "weapon"),
    "Craft: Blackhole Launcher":          LocationData(BASE_ID + 1471, "Bancho Sushi", "weapon"),
    "Craft: Flash Grenade Launcher":      LocationData(BASE_ID + 1472, "Bancho Sushi", "weapon"),

    # --- Ice Gun tree (3 variants) ---
    "Craft: Ice Gun":                     LocationData(BASE_ID + 1473, "Bancho Sushi", "weapon"),
    "Craft: Enhanced Ice Gun":            LocationData(BASE_ID + 1474, "Bancho Sushi", "weapon"),
    "Craft: Ultra Ice Gun":               LocationData(BASE_ID + 1475, "Bancho Sushi", "weapon"),

    # --- Drain Gun tree (3 variants, DREDGE DLC only) ---
    "Craft: Drain Gun":                   LocationData(BASE_ID + 1476, "Bancho Sushi", "dlc_dredge"),
    "Craft: Enhanced Drain Gun":          LocationData(BASE_ID + 1477, "Bancho Sushi", "dlc_dredge"),
    "Craft: Power Drain Gun":             LocationData(BASE_ID + 1478, "Bancho Sushi", "dlc_dredge"),
}

# === CHARMS ===
# Each charm's acquisition condition = 1 AP location check.
# Mission charms are in the relevant region; Ecowatcher charms gate on ecowatcher levels.
charm_locations: Dict[str, LocationData] = {
    # ── Mission-acquired charms (base game) ──────────────────────────────────
    "Charm: Dolphin Necklace (Complete Defeat Pirates)":                LocationData(BASE_ID + 1480, "Blue Hole - Shallow", ""),
    "Charm: Octopus Bracelet (Complete Investigate the Strange Coral)": LocationData(BASE_ID + 1481, "Blue Hole - Mid", ""),
    "Charm: Sea People Bracelet (Complete Beyond the Rock Pile)":       LocationData(BASE_ID + 1482, "Blue Hole - Deep", ""),
    "Charm: Octopus Weapon Charm (Complete Octopus Returns)":           LocationData(BASE_ID + 1483, "Blue Hole - Mid", ""),
    "Charm: Sea People Necklace (Complete Deliver Key to Tenzhin)":     LocationData(BASE_ID + 1484, "Sea People Village", ""),
    "Charm: Shark Teeth Necklace (Complete Revenge Time!)":             LocationData(BASE_ID + 1485, "Blue Hole - Shallow", ""),
    # ── DLC charms ───────────────────────────────────────────────────────────
    "Charm: Leo Keychain (Complete EVIL FACTORY Demo)":                 LocationData(BASE_ID + 1486, "Bancho Sushi", "dlc_dredge"),
    "Charm: Jimbo Coin (Complete Jimbo's Game Craze!)":                 LocationData(BASE_ID + 1487, "Bancho Sushi", ""),
    # ── Ecowatcher level-up charms ───────────────────────────────────────────
    "Charm: Eco Poison Resist Bracelet (Ecowatcher Level 2)":           LocationData(BASE_ID + 1488, "Blue Hole - Shallow", "ecowatcher"),
    "Charm: Eco Health Bracelet (Ecowatcher Level 3)":                  LocationData(BASE_ID + 1489, "Blue Hole - Shallow", "ecowatcher"),
    "Charm: Eco Gemstone Bracelet (Ecowatcher Level 4)":                LocationData(BASE_ID + 1490, "Blue Hole - Shallow", "ecowatcher"),
    "Charm: Eco Waterproof Bag (Ecowatcher Level 5)":                   LocationData(BASE_ID + 1491, "Blue Hole - Shallow", "ecowatcher"),
    # ── Jungle DLC charms (max villager friendship rewards) ──────────────────
    "Charm: Crocodile Tooth Necklace (Complete Operation: Sulong Hunt)": LocationData(BASE_ID + 1492, "Utara Village", "dlc_jungle"),
    "Charm: Charm of Abundance (Max Friendship: Panutah)":              LocationData(BASE_ID + 1493, "Utara Village", "dlc_jungle"),
    "Charm: Anti-Gravity Device (Max Friendship: Muna)":                LocationData(BASE_ID + 1494, "Utara Village", "dlc_jungle"),
    "Charm: Gold Necklace of Sloth (Max Friendship: Harta)":            LocationData(BASE_ID + 1495, "Utara Village", "dlc_jungle"),
    "Charm: Bracelet of Strength (Max Friendship: Uzme)":               LocationData(BASE_ID + 1496, "Utara Village", "dlc_jungle"),
    "Charm: Air Resonance Necklace (Max Friendship: Bonita)":           LocationData(BASE_ID + 1497, "Utara Village", "dlc_jungle"),
}

# === ACHIEVEMENTS / MILESTONES ===
achievement_locations: Dict[str, LocationData] = {
    "Catch 50 Different Fish Species": LocationData(BASE_ID + 700, "Blue Hole - Shallow"),
    "Catch 100 Different Fish Species": LocationData(BASE_ID + 701, "Blue Hole - Shallow"),
    "Earn 10,000 Gold": LocationData(BASE_ID + 710, "Bancho Sushi"),
    "Earn 50,000 Gold": LocationData(BASE_ID + 711, "Bancho Sushi"),
    "Earn 100,000 Gold": LocationData(BASE_ID + 712, "Bancho Sushi"),
    "Max Upgrade All Equipment": LocationData(BASE_ID + 720, "Bancho Sushi"),
    
    # Gameplay milestones
    "Play for 10 Hours": LocationData(BASE_ID + 730, "Bancho Sushi"),
    "Play for 25 Hours": LocationData(BASE_ID + 731, "Bancho Sushi"),
    "Complete 50 Dives": LocationData(BASE_ID + 732, "Blue Hole - Shallow"),
    "Complete 100 Dives": LocationData(BASE_ID + 733, "Blue Hole - Shallow"),
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
    **farming_locations,
    **chicken_farm_locations,
    **fish_farm_locations,
    **collectible_locations,
    **kaiju_figurine_locations,
    **ichiban_locations,
    **jungle_story_locations,
    **jungle_boss_locations,
    **staff_hire_locations,
    **staff_all_levels_locations,  # covers both milestone (Lv5/10/15/20) and all_levels (Lv1-20) modes
    **jungle_staff_locations,
    **jungle_villager_locations,
    **jungle_minigame_locations,
    **jungle_insect_locations,
    **jungle_fish_locations,
    **jungle_boss_fish_locations,
    **jungle_ingredient_locations,
    **jungle_restaurant_locations,
    **jungle_exploration_locations,
    **jungle_weapon_locations,
    **minigame_locations,
    **ingredient_locations,
    **weapon_locations,
    **charm_locations,
    **achievement_locations,
}

# Create lookup dictionary
location_name_to_id: Dict[str, int] = {
    name: data.code for name, data in location_table.items() if data.code is not None
}

# Breakdown by category (approximate):
# - Fish first catch: 100+ (common + rare + boss + jungle)
# - Dish upgrades: 400+ (if all dishes included)
# - Recipe unlocks: 36 defined
# - Cooksta: 31 defined
# - Ecowatcher: 106 defined
# - Photography: 33 defined
# - Farming/ChickenFarm/FishFarm: 74 defined
# - Story/Bosses/Quests: 200+ defined
# - Staff hire/train: 150+ defined
# - Jungle DLC: 500+ defined
# - Minigames/Collectibles/Weapons/Charms: 200+ defined
