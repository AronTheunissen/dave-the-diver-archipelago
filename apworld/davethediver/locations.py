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

def _dish_upgrades(dish: str, max_level: int, base: int) -> dict:
    """Generate upgrade locations for a dish from level 2 to max_level."""
    return {
        f"Upgrade {dish} to Level {lvl}": LocationData(base + (lvl - 2), "Bancho Sushi", "dish_upgrade")
        for lvl in range(2, max_level + 1)
    }

# Assign base IDs in blocks of 15 (max possible upgrades per dish) starting at BASE_ID+2000
# Existing locations use up to BASE_ID+1118, so BASE_ID+2000 gives plenty of clearance.
# Each dish block: BASE_ID + 2000 + (dish_index * 15), giving room for 96+ dishes.
_D = BASE_ID + 2000
dish_upgrade_locations: Dict[str, LocationData] = {
    **_dish_upgrades("Agar Tokoroten",                        7,  _D + 0*15),   # max 7
    **_dish_upgrades("Antarctic Octopus Carpaccio",           7,  _D + 1*15),   # max 7
    **_dish_upgrades("Arctic Cod Risotto",                    9,  _D + 2*15),   # max 9
    # Atlantic Bonito Curry: see corrected entry at bottom (max 12)
    **_dish_upgrades("Batfish Ricebowl",                      7,  _D + 4*15),   # max 7
    **_dish_upgrades("Big-Eyed Scad and Soybean Paste Roast", 7, _D + 5*15),   # max 7
    **_dish_upgrades("Black Vinegar Braised Parrotfish",      6,  _D + 6*15),   # max 6
    **_dish_upgrades("Blobfish Spring Roll",                  10, _D + 7*15),   # max 10
    **_dish_upgrades("Boiled Mantis Shrimp with Soy Paste",  1,  _D + 8*15),   # max 1 (boss recipe, no upgrades — skip)
    **_dish_upgrades("Boiled Porbeagle Shark",                7,  _D + 9*15),   # max 7
    **_dish_upgrades("Boiled Sailfish and Seaweed",           9,  _D + 10*15),  # max 9
    **_dish_upgrades("Boiled Yellowback Fusilier",            7,  _D + 11*15),  # max 7
    **_dish_upgrades("Boiled and Deep-Fried White Shrimp",   10, _D + 12*15),  # max 10
    **_dish_upgrades("Bluefin Tuna Rice Bowl",                9,  _D + 13*15),  # max 9
    **_dish_upgrades("Comber Sandwich",                       6,  _D + 14*15),  # max 6
    **_dish_upgrades("Crimson Fish Roll",                     9,  _D + 15*15),  # max 9
    **_dish_upgrades("Crystal Lobster Roll",                  9,  _D + 16*15),  # max 9
    **_dish_upgrades("Deep Fish Tempura",                     7,  _D + 17*15),  # max 7
    **_dish_upgrades("Deep Sea Kaiju Ramen",                  6,  _D + 18*15),  # max 6
    **_dish_upgrades("Deep-Fried Eggplant Shrimp Meatballs", 7,  _D + 19*15),  # max 7
    **_dish_upgrades("Deep-Fried Red Lionfish",               4,  _D + 20*15),  # max 4
    **_dish_upgrades("Deep-Fried Vegetables",                 3,  _D + 21*15),  # max 3
    **_dish_upgrades("Dried Stingray",                        12, _D + 22*15),  # max 12
    **_dish_upgrades("Dumbo Takoyaki",                        9,  _D + 23*15),  # max 9
    **_dish_upgrades("Dusky Grouper Steak",                   7,  _D + 24*15),  # max 7
    **_dish_upgrades("Eggplant Soba Oyaki",                   9,  _D + 25*15),  # max 9
    **_dish_upgrades("Falcatus Soybean Paste Soup",           7,  _D + 26*15),  # max 7
    **_dish_upgrades("Fried Habanero Fangtooth",              7,  _D + 27*15),  # max 7
    **_dish_upgrades("Fried Onion Cuttlefish",                7,  _D + 28*15),  # max 7
    **_dish_upgrades("Fried Rice with Sally Lightfoot Crab", 10, _D + 29*15),  # max 10
    **_dish_upgrades("Fried Seahorses",                       4,  _D + 30*15),  # max 4
    **_dish_upgrades("Fried Tomato and Snailfish",            7,  _D + 31*15),  # max 7
    **_dish_upgrades("Goblin Shark Belly Roast",              1,  _D + 32*15),  # boss — no upgrades
    **_dish_upgrades("Great Barracuda Canape",                6,  _D + 33*15),  # max 6
    **_dish_upgrades("Great Spider Crab Curry",               9,  _D + 34*15),  # max 9
    **_dish_upgrades("Hawaiian Poke",                         9,  _D + 35*15),  # max 9
    **_dish_upgrades("Hot Pepper Tuna",                       7,  _D + 36*15),  # max 7
    **_dish_upgrades("Humboldt Ink Pasta",                    10, _D + 37*15),  # max 10
    **_dish_upgrades("Humphead Parrotfish Curry",             6,  _D + 38*15),  # max 6
    **_dish_upgrades("Ice Fish Curry",                        9,  _D + 39*15),  # max 9
    **_dish_upgrades("Latok Omelet",                          9,  _D + 40*15),  # max 9
    **_dish_upgrades("Mackerel Scad Hotdog",                  6,  _D + 41*15),  # max 6
    **_dish_upgrades("Marlin and Soybean Paste Roast",        9,  _D + 42*15),  # max 9
    **_dish_upgrades("Mianbao Xia",                           10, _D + 43*15),  # max 10
    **_dish_upgrades("Moray Eel Curry",                       6,  _D + 44*15),  # max 6
    **_dish_upgrades("Narrow-barred Spanish Mackerel Arancini", 7, _D + 45*15), # max 7
    **_dish_upgrades("Narwhal Miso Soup",                     12, _D + 46*15),  # max 12
    **_dish_upgrades("Nasu Dengaku",                          4,  _D + 47*15),  # max 4
    **_dish_upgrades("Peacock Squid Ripieni",                 7,  _D + 48*15),  # max 7
    **_dish_upgrades("Pelican Eel Jelly",                     7,  _D + 49*15),  # max 7
    **_dish_upgrades("Phantom Jellyfish Jelly",               1,  _D + 50*15),  # boss — no upgrades
    **_dish_upgrades("Pickled Vegetables",                    3,  _D + 51*15),  # max 3
    **_dish_upgrades("Pikaia Ramen",                          10, _D + 52*15),  # max 10
    **_dish_upgrades("Plotosid Pie",                          7,  _D + 53*15),  # max 7
    **_dish_upgrades("Rice with Great Spider Crab Meat",      7,  _D + 54*15),  # max 7
    **_dish_upgrades("Rice with Purple Sea Urchin Sushi",     4,  _D + 55*15),  # max 4
    **_dish_upgrades("Rice with White Shrimp Meat",           9,  _D + 56*15),  # max 9
    **_dish_upgrades("Roasted Capelin",                       12, _D + 57*15),  # max 12
    **_dish_upgrades("Roasted Helicoprion Tail",              1,  _D + 58*15),  # boss — no upgrades
    **_dish_upgrades("Roasted Tropical Fish and Garlic",      9,  _D + 59*15),  # max 9
    **_dish_upgrades("Salt-grilled Redtoothed Triggerfish",   6,  _D + 60*15),  # max 6
    **_dish_upgrades("Seahorse Salad",                        6,  _D + 61*15),  # max 6
    **_dish_upgrades("Seahorse Skewers",                      1,  _D + 62*15),  # max 1 — no upgrades
    **_dish_upgrades("Seahorse Udon",                         4,  _D + 63*15),  # max 4
    **_dish_upgrades("Seasoned Jellyfish",                    6,  _D + 64*15),  # max 6
    **_dish_upgrades("Seasoned Kajime",                       6,  _D + 65*15),  # max 6
    **_dish_upgrades("Seasoned Long-spine Porcupinefish Skin", 7, _D + 66*15),  # max 7
    **_dish_upgrades("Seasoned Waptia Fieldensis",            7,  _D + 67*15),  # max 7
    **_dish_upgrades("Seaweed Rolled Omelet",                 9,  _D + 68*15),  # max 9
    **_dish_upgrades("Shark Karaage",                         9,  _D + 69*15),  # max 9
    **_dish_upgrades("Smallspotted Dart Kajime Soup",         7,  _D + 70*15),  # max 7
    **_dish_upgrades("Smoked Atlantic Mackerel Scramble",     6,  _D + 71*15),  # max 6
    **_dish_upgrades("Spear Squid Soba Futomaki",             9,  _D + 72*15),  # max 9
    **_dish_upgrades("Special Fried Shrimp Sushi",            1,  _D + 73*15),  # max 1 — no upgrades
    **_dish_upgrades("Steamed Kronosaurus Tongue",            1,  _D + 74*15),  # boss — no upgrades
    **_dish_upgrades("Steamed Wolf Eel",                      1,  _D + 75*15),  # boss — no upgrades
    **_dish_upgrades("Stellate Puffer Nicogori",              7,  _D + 76*15),  # max 7
    **_dish_upgrades("Stingray Sashimi Cold Noodles",         9,  _D + 77*15),  # max 9
    **_dish_upgrades("Stir-Fried Hermit Crab and Seaweed",   1,  _D + 78*15),  # boss — no upgrades
    **_dish_upgrades("Stir-fried Habanero Lobster",          7,  _D + 79*15),  # max 7
    **_dish_upgrades("Striped Red Mullet Tangle Roll",        7,  _D + 80*15),  # max 7
    **_dish_upgrades("Sweet and Sour Stargazer",              6,  _D + 81*15),  # max 6
    **_dish_upgrades("Three-Colored Squid Roast",             12, _D + 82*15),  # max 12
    **_dish_upgrades("Tomato Egg Soup",                       12, _D + 83*15),  # max 12
    **_dish_upgrades("Trevally Nanbanzuke",                   7,  _D + 84*15),  # max 7
    **_dish_upgrades("Trevally Sandwich",                     7,  _D + 85*15),  # max 7
    **_dish_upgrades("Tropical Fish Sushi Set",               9,  _D + 86*15),  # max 9
    **_dish_upgrades("Trout Sea Grapes Ricebowl",             7,  _D + 87*15),  # max 7
    **_dish_upgrades("Vegetable Sushi",                       1,  _D + 88*15),  # max 1 — no upgrades
    **_dish_upgrades("White Shark Omelet",                    1,  _D + 89*15),  # boss — no upgrades
    **_dish_upgrades("White Trevally Kombu Ochazuke",         7,  _D + 90*15),  # max 7
    **_dish_upgrades("Whole-Roasted Shark Head",              7,  _D + 91*15),  # max 7
    **_dish_upgrades("Wrasse Curry",                          6,  _D + 92*15),  # max 6
    **_dish_upgrades("Yawie Steamed Meat",                    1,  _D + 93*15),  # boss — no upgrades
    **_dish_upgrades("Yellowfin Tuna Steak",                  9,  _D + 94*15),  # max 9
    **_dish_upgrades("Atlantic Bonito Curry",                 12, _D + 3*15),   # max 12 (corrected from duplicate)
}
# Filter out any entries that somehow have no upgrades (defensive)
dish_upgrade_locations = {k: v for k, v in dish_upgrade_locations.items()}

# === RECIPE UNLOCKS ===
recipe_unlock_locations: Dict[str, LocationData] = {
    # --- Basic fish sushi (unlocked by catching the fish) ---
    "Unlock Recipe: Yellowfin Tuna Akami Sushi": LocationData(BASE_ID + 800, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Alaska Pollock Sushi": LocationData(BASE_ID + 801, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Antarctic Octopus Sushi": LocationData(BASE_ID + 802, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Arctic Cod Sushi": LocationData(BASE_ID + 803, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Atlantic Anglerfish Sushi": LocationData(BASE_ID + 804, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blobfish Sushi": LocationData(BASE_ID + 805, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blue Tang Sushi": LocationData(BASE_ID + 806, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Clownfish Sushi": LocationData(BASE_ID + 807, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Great Barracuda Sushi": LocationData(BASE_ID + 808, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Greenland Shark Sushi": LocationData(BASE_ID + 809, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humboldt Squid Sushi": LocationData(BASE_ID + 810, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Marlin Sushi": LocationData(BASE_ID + 811, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Narwhal Sushi": LocationData(BASE_ID + 812, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Tiger Shark Sushi": LocationData(BASE_ID + 813, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Vampire Squid Sushi": LocationData(BASE_ID + 814, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Zebra Shark Sushi": LocationData(BASE_ID + 815, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Comber Sushi": LocationData(BASE_ID + 816, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humphead Parrotfish Sushi": LocationData(BASE_ID + 817, "Bancho Sushi", "recipe"),

    # --- VIP mission recipes ---
    # --- Godzilla DLC recipes ---
    "Unlock Recipe: Godzilla vs. Ebirah Curry": LocationData(BASE_ID + 855, "Bancho Sushi", "dlc_godzilla"),
    "Unlock Recipe: Ebirah Chasing Sashimi":    LocationData(BASE_ID + 856, "Bancho Sushi", "dlc_godzilla"),

    "Unlock Recipe: Seagrapes Jellyfish Sushi (Vincent)": LocationData(BASE_ID + 820, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Tropical Fish Sushi Set (Michael Bang)": LocationData(BASE_ID + 821, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Vegetable Sushi (Sammy)": LocationData(BASE_ID + 822, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Sweet and Sour Stargazer (Wang Pang)": LocationData(BASE_ID + 823, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Blobfish Spring Roll (Wang Pang)": LocationData(BASE_ID + 824, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Deep Fish Tempura (Alex)": LocationData(BASE_ID + 825, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Comber Sandwich (Alex)": LocationData(BASE_ID + 826, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Humboldt Ink Pasta (Pastro)": LocationData(BASE_ID + 827, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Antarctic Octopus Carpaccio (Pastro)": LocationData(BASE_ID + 828, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Haddock Acqua Pazza (Pastro)": LocationData(BASE_ID + 829, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Arctic Cod Risotto (Pastro)": LocationData(BASE_ID + 830, "Bancho Sushi", "recipe"),
    "Unlock Recipe: Peacock Squid Ripieni (Pastro)": LocationData(BASE_ID + 831, "Bancho Sushi", "recipe"),

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
}

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
    "Defeat: Ebirah": LocationData(BASE_ID + 910, "Sea People Village"),
    # Optional/legendary bosses
    "Defeat: Great White Shark Klaus": LocationData(BASE_ID + 911, "Blue Hole - Deep"),
    "Defeat: Mantis Shrimp": LocationData(BASE_ID + 912, "Blue Hole - Deep"),
    "Defeat: Lusca": LocationData(BASE_ID + 913, "Sea People Village"),
    # Final boss
    "Defeat: Yawie (Final Boss)": LocationData(BASE_ID + 914, "Sea People Village"),
    # Optional: Torben
    "Defeat: Torben": LocationData(BASE_ID + 915, "Blue Hole - Deep"),
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
    "Quest: Serve Vincent Yamaoka (The Gourmet)": LocationData(BASE_ID + 389, "Bancho Sushi"),
    "Quest: Serve Michael Bang (Movie Director)": LocationData(BASE_ID + 390, "Bancho Sushi"),
    "Quest: Serve Sammy (Rapper)": LocationData(BASE_ID + 391, "Bancho Sushi"),
    "Quest: Serve Wang Pang (Chef Competitor)": LocationData(BASE_ID + 392, "Bancho Sushi"),
    "Quest: Serve Alex Cooper (Chef Competitor)": LocationData(BASE_ID + 393, "Bancho Sushi"),
    "Quest: Serve Pastro Antogiovani (Chef Competitor)": LocationData(BASE_ID + 394, "Bancho Sushi"),

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
    "Find Treasure Chest 1": LocationData(BASE_ID + 950, "Blue Hole - Shallow"),
    "Find Treasure Chest 2": LocationData(BASE_ID + 951, "Blue Hole - Mid"),
    "Purchase Upgrade from Duff 1": LocationData(BASE_ID + 960, "Bancho Sushi"),
    "Purchase Upgrade from Duff 2": LocationData(BASE_ID + 961, "Bancho Sushi"),
    # TODO: Add all treasure chests, shop purchases, etc.
}

# === MINIGAMES ===
minigame_locations: Dict[str, LocationData] = {
    "Beat Seahorse Racing - Easy": LocationData(BASE_ID + 600, "Sea People Village", "minigame"),
    "Beat Seahorse Racing - Medium": LocationData(BASE_ID + 601, "Sea People Village", "minigame"),
    "Beat Seahorse Racing - Hard": LocationData(BASE_ID + 602, "Sea People Village", "minigame"),
    "Complete All Card Mini-games": LocationData(BASE_ID + 610, "Bancho Sushi", "minigame"),
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
}

# === PHOTOGRAPHY / PICTURES ===
# Tako's photography missions and special photo spots
photography_locations: Dict[str, LocationData] = {
    # Tako's photography missions
    "Photography: Complete Mission 1": LocationData(BASE_ID + 500, "Blue Hole - Shallow", "photography"),
    "Photography: Complete Mission 2": LocationData(BASE_ID + 501, "Blue Hole - Shallow", "photography"),
    "Photography: Complete Mission 3": LocationData(BASE_ID + 502, "Blue Hole - Shallow", "photography"),
    "Photography: Complete Mission 4": LocationData(BASE_ID + 503, "Blue Hole - Shallow", "photography"),
    "Photography: Complete Mission 5": LocationData(BASE_ID + 504, "Blue Hole - Shallow", "photography"),
    
    # Special photo spots
    "Photo: Giant Squid": LocationData(BASE_ID + 510, "Blue Hole - Deep", "photography"),
    "Photo: Whale Shark": LocationData(BASE_ID + 511, "Blue Hole - Deep", "photography"),
    "Photo: Sea People Elder": LocationData(BASE_ID + 512, "Sea People Village", "photography"),
    "Photo: Glacier Scenery": LocationData(BASE_ID + 513, "Glacier Zone", "photography"),
    
    # Photography milestones
    "Photography: Take 50 Photos": LocationData(BASE_ID + 520, "Blue Hole - Shallow", "photography"),
    "Photography: Take 100 Photos": LocationData(BASE_ID + 521, "Blue Hole - Shallow", "photography"),
    "Photography: Perfect Score on 10 Missions": LocationData(BASE_ID + 522, "Blue Hole - Shallow", "photography"),
    
    # TODO: Add all Tako photography missions
}

# === CHALLENGES ===
# In-game challenges and special objectives
challenge_locations: Dict[str, LocationData] = {
    # Time attack challenges
    "Challenge: Catch 5 Fish in 60 Seconds": LocationData(BASE_ID + 550, "Blue Hole - Shallow", "challenge"),
    "Challenge: Earn 1000g in One Dive": LocationData(BASE_ID + 551, "Blue Hole - Shallow", "challenge"),
    "Challenge: Defeat 3 Sharks Without Taking Damage": LocationData(BASE_ID + 552, "Blue Hole - Deep", "challenge"),
    
    # Weapon challenges
    "Challenge: Kill 10 Fish with Harpoon Only": LocationData(BASE_ID + 560, "Blue Hole - Shallow", "challenge"),
    "Challenge: Kill 10 Fish with Melee Only": LocationData(BASE_ID + 561, "Blue Hole - Shallow", "challenge"),
    "Challenge: Net Gun 20 Fish Alive": LocationData(BASE_ID + 562, "Blue Hole - Shallow", "challenge"),
    
    # Special challenges
    "Challenge: Serve 10 Customers with Perfect Timing": LocationData(BASE_ID + 570, "Bancho Sushi", "challenge"),
    "Challenge: Complete a Dive Without Using Oxygen Refills": LocationData(BASE_ID + 571, "Blue Hole - Shallow", "challenge"),
    "Challenge: Reach Max Depth Without Equipment Damage": LocationData(BASE_ID + 572, "Blue Hole - Deep", "challenge"),
    
    # TODO: Add all in-game challenges
}

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
    "Veg Farm: First Harvest - Buckwheat": LocationData(BASE_ID + 1017, "Vegetable Farm", "farming"),
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
    # Fish farm unlocks
    "Fish Farm: Unlock Fish Farm": LocationData(BASE_ID + 650, "Fish Farm", "fish_farm"),
    "Fish Farm: Upgrade Tank 1": LocationData(BASE_ID + 651, "Fish Farm", "fish_farm"),
    "Fish Farm: Upgrade Tank 2": LocationData(BASE_ID + 652, "Fish Farm", "fish_farm"),
    "Fish Farm: Upgrade Tank 3": LocationData(BASE_ID + 653, "Fish Farm", "fish_farm"),
    
    # Breed/raise specific fish types
    "Fish Farm: First Breed - Tuna": LocationData(BASE_ID + 660, "Fish Farm", "fish_farm"),
    "Fish Farm: First Breed - Salmon": LocationData(BASE_ID + 661, "Fish Farm", "fish_farm"),
    "Fish Farm: First Breed - Squid": LocationData(BASE_ID + 662, "Fish Farm", "fish_farm"),
    "Fish Farm: First Breed - Octopus": LocationData(BASE_ID + 663, "Fish Farm", "fish_farm"),
    "Fish Farm: First Breed - Rare Species": LocationData(BASE_ID + 664, "Fish Farm", "fish_farm"),
    
    # Fish farm milestones
    "Fish Farm: Raise 10 Fish to Adulthood": LocationData(BASE_ID + 670, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 25 Fish to Adulthood": LocationData(BASE_ID + 671, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 50 Fish to Adulthood": LocationData(BASE_ID + 672, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 5 Different Species": LocationData(BASE_ID + 673, "Fish Farm", "fish_farm"),
    "Fish Farm: Raise 10 Different Species": LocationData(BASE_ID + 674, "Fish Farm", "fish_farm"),
    "Fish Farm: Max Out Fish Quality": LocationData(BASE_ID + 675, "Fish Farm", "fish_farm"),
    
    # TODO: Add all farmable fish species
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
    "First Find: Buckwheat":          LocationData(BASE_ID + 1314, "Vegetable Farm", "farming"),
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
    # Mission-acquired charms (base game)
    "Charm: Dolphin Necklace (Complete Defeat Pirates)":                LocationData(BASE_ID + 1480, "Blue Hole - Shallow", ""),
    "Charm: Octopus Bracelet (Complete Investigate the Strange Coral)": LocationData(BASE_ID + 1481, "Blue Hole - Mid", ""),
    "Charm: Sea People Bracelet (Complete Beyond the Rock Pile)":       LocationData(BASE_ID + 1482, "Blue Hole - Deep", ""),
    "Charm: Octopus Weapon Charm (Complete Octopus Returns)":           LocationData(BASE_ID + 1483, "Blue Hole - Mid", ""),
    "Charm: Sea People Necklace (Complete Deliver Key to Tenzhin)":     LocationData(BASE_ID + 1484, "Sea People Village", ""),
    "Charm: Shark Teeth Necklace (Complete Revenge Time!)":             LocationData(BASE_ID + 1485, "Blue Hole - Shallow", ""),
    # DLC charms
    "Charm: Leo Keychain (Complete EVIL FACTORY Demo)":                 LocationData(BASE_ID + 1486, "Bancho Sushi", "dlc_dredge"),
    "Charm: Jimbo Coin (Complete Jimbo's Game Craze!)":                 LocationData(BASE_ID + 1487, "Bancho Sushi", ""),
    # Ecowatcher level-up charms
    "Charm: Eco Poison Resist Bracelet (Ecowatcher Level 2)":           LocationData(BASE_ID + 1488, "Blue Hole - Shallow", "ecowatcher"),
    "Charm: Eco Health Bracelet (Ecowatcher Level 3)":                  LocationData(BASE_ID + 1489, "Blue Hole - Shallow", "ecowatcher"),
    "Charm: Eco Gemstone Bracelet (Ecowatcher Level 4)":                LocationData(BASE_ID + 1490, "Blue Hole - Shallow", "ecowatcher"),
    "Charm: Eco Waterproof Bag (Ecowatcher Level 5)":                   LocationData(BASE_ID + 1491, "Blue Hole - Shallow", "ecowatcher"),
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
    **challenge_locations,
    **farming_locations,
    **chicken_farm_locations,
    **fish_farm_locations,
    **collectible_locations,
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
