using HarmonyLib;
using BepInEx.Logging;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the game's fish catch system to detect first catches.
    ///
    /// ## How to find the real class names:
    /// 1. Run Il2CppDumper on GameAssembly.dll + global-metadata.dat
    ///    (https://github.com/Perfare/Il2CppDumper)
    /// 2. Load the generated DummyDll/ in ILSpy or dnSpy
    /// 3. Search for "fish", "catch", "marinca", "encyclopedia"
    ///
    /// ## Known patterns from existing mods:
    /// - All fish/item interactions use CheckAvailableInteraction() + SuccessInteract() pattern
    ///   (confirmed by WhiteMinds/dave-diver-expansion)
    /// - Hook SuccessInteract() on the fish interaction class to detect catches
    /// - Use SaveSystem API (not property getter patches) for reading game state
    ///
    /// ## Known classes (from cheat engine + mod analysis):
    /// - SaveSystem → singleton with PlayerInfoSave accessor
    /// - PlayerInfoSave → ObscuredInt gold/bei/ChefFlame, inventory state
    /// - InGameManager → has FishAllocators for fish spawning
    /// - FishInteraction → inferred name, implements CheckAvailableInteraction/SuccessInteract
    ///
    /// ## Next step (on game machine):
    /// Run Il2CppDumper, search for classes containing "SuccessInteract" or "FirstCatch"
    /// and replace the PLACEHOLDER below with the real class name.
    /// </summary>
    [HarmonyPatch]
    public static class FishCatchPatch
    {
        // ✅ CONFIRMED via dump.cs: FishInteractionBody is the real class
        // ✅ CONFIRMED via dump.cs: SuccessInteraction() is the real method name (not SuccessInteract)
        // Fields confirmed in dump.cs: SuccessPickupFish (UnityEvent), SuccessCarving (UnityEvent)
        // Fish identity must be read from the parent fish AI object via __instance
        [HarmonyPatch(typeof(FishInteractionBody), "SuccessInteraction")]
        [HarmonyPostfix]
        public static void SuccessInteraction_Postfix(FishInteractionBody __instance)
        {
            if (!ArchipelagoClient.IsConnected) return;

            // The fish name can be retrieved from the GameObject name or a parent AI component.
            // FishInteractionBody is attached to the fish GameObject — use its name to identify the species.
            string goName = __instance.gameObject.name;
            var fishName = FishNameMapper.GetDisplayNameFromGameObject(goName);
            if (fishName != null)
                LocationTracker.OnFirstFishCatch(fishName);
        }
    }

    /// <summary>
    /// Maps internal game fish IDs to the display names used in AP location names.
    /// Fill these in by cross-referencing the game's fish data files.
    /// </summary>
    public static class FishNameMapper
    {
        // TODO: Build this mapping by decompiling the game and finding
        // the fish ID → display name lookup table.
        // The keys are internal game IDs, values are AP location name suffixes.
        // Maps GameObject name prefixes/substrings to AP location display names.
        // The GameObject name in IL2CPP typically matches the prefab name from Unity.
        // These were cross-referenced from the fish list and dump.cs prefab naming patterns.
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new(System.StringComparer.OrdinalIgnoreCase)
        {
            // Blue Hole Shallows
            { "AmericanLobster",        "American Lobster" },
            { "BarrelJellyfish",        "Barrel Jellyfish" },
            { "BigBellySeahorse",       "Big-Belly Seahorse" },
            { "BlackAndWhiteSnapper",   "Black and White Snapper" },
            { "BlacktipReefshark",      "Blacktip Reefshark" },
            { "BlueLobster",            "Blue Lobster" },
            { "BlueTang",               "Blue Tang" },
            { "BluefinTuna",            "Bluefin Tuna" },
            { "BoxJellyfish",           "Box Jellyfish" },
            { "CardinalFish",           "Cardinal Fish" },
            { "ClearfinLionfish",       "Clearfin Lionfish" },
            { "Clownfish",              "Clownfish" },
            { "Comber",                 "Comber" },
            { "CopperShark",            "Copper Shark" },
            { "EmperorAngelfish",       "Emperor Angelfish" },
            { "EuropeanLobster",        "European Lobster" },
            { "FlameAngelfish",         "Flame Angelfish" },
            { "FriedEggJellyfish",      "Fried Egg Jellyfish" },
            { "GreatWhiteShark",        "Great White Shark Klaus" },
            { "GreenHumpeadParrotfish", "Green Humphead Parrotfish" },
            { "GreenSeaUrchin",         "Green Sea Urchin" },
            { "JayakarsSeahorse",       "Jayakar's Seahorse" },
            { "LagoonTriggerfish",      "Lagoon Triggerfish" },
            { "LongSnoutedSeahorse",    "Long-Snouted Seahorse" },
            { "LongfinBatfish",         "Longfin Batfish" },
            { "LongspinePorcupinefish", "Longspine Porcupinefish" },
            { "LongspineSquirrelfish",  "Longspine Squirrelfish" },
            { "MantisShrimp",           "Mantis Shrimp" },
            { "MarbeldElectricRay",     "Marbled Electric Ray" },
            { "Marlin",                 "Marlin" },
            { "MediterraneanParrotfish","Mediterranean Parrotfish" },
            { "MorayEel",               "Moray Eel" },
            { "OrbicularBatfish",       "Orbicular Batfish" },
            { "OrnateWrasse",           "Ornate Wrasse" },
            { "PacificSeahorse",        "Pacific Seahorse" },
            { "PurpleSeaUrchin",        "Purple Sea Urchin" },
            { "PyramidButterflyfish",   "Pyramid Butterflyfish" },
            { "RainbowWrasse",          "Rainbow Wrasse" },
            { "RedLionfish",            "Red Lionfish" },
            { "RedBandedLobster",       "Red-banded Lobster" },
            { "RedtoothedTriggerfish",  "Redtoothed Triggerfish" },
            { "SalemaPorgy",            "Salema Porgy" },
            { "SeaGoldie",              "Sea Goldie" },
            { "Sheepshead",             "Sheepshead" },
            { "ShortfinMako",           "Shortfin Mako" },
            { "SmallSpottedDart",       "Small Spotted Dart" },
            { "StarryPuffer",           "Starry Puffer" },
            { "Stingray",               "Stingray" },
            { "StripedCatfish",         "Striped Catfish" },
            { "ThresherShark",          "Thresher Shark" },
            { "TitanTriggerfish",       "Titan Triggerfish" },
            { "TruckHermitCrab",        "Truck Hermit Crab" },
            { "WhiteShrimp",            "White Shrimp" },
            { "WhitelegShrimp",         "Whiteleg Shrimp" },
            { "WhitetipReefshark",      "Whitetip Reefshark" },
            { "YellowTang",             "Yellow Tang" },
            { "YellowbackFusilier",     "Yellowback Fusilier" },
            { "YellowfinTuna",          "Yellowfin Tuna" },
            { "ZebraShark",             "Zebra Shark" },
            // Blue Hole Mid
            { "AtlanticAnglerfish",     "Atlantic Anglerfish" },
            { "AtlanticBonito",         "Atlantic Bonito" },
            { "AtlanticMackerel",       "Atlantic Mackerel" },
            { "BigeyeScad",             "Bigeye Scad" },
            { "BigeyeTrevally",         "Bigeye Trevally" },
            { "BlackTigerShrimp",       "Black Tiger Shrimp" },
            { "BlackfinBarracuda",       "Blackfin Barracuda" },
            { "BlueheadTilefish",       "Bluehead Tilefish" },
            { "CaliforniaSpinyLobster", "California Spiny Lobster" },
            { "ClownFrogfish",          "Clown Frogfish" },
            { "CoralTrout",             "Coral Trout" },
            { "CrystalLobster",         "Crystal Lobster" },
            { "Cuttlefish",             "Cuttlefish" },
            { "DevilScorpionfish",      "Devil Scorpionfish" },
            { "DuskyGrouper",           "Dusky Grouper" },
            { "DwarfSeahorse",          "Dwarf Seahorse" },
            { "FanLobster",             "Fan Lobster" },
            { "GiantSquid",             "Giant Squid" },
            { "GiantTrevally",          "Giant Trevally" },
            { "GiraffeSeahorse",        "Giraffe Seahorse" },
            { "GreatBarracuda",         "Great Barracuda" },
            { "GreyTriggerfish",        "Grey Triggerfish" },
            { "HarlequinHind",          "Harlequin Hind" },
            { "HedgehogSeahorse",       "Hedgehog Seahorse" },
            { "HumboldtSquid",          "Humboldt Squid" },
            { "LongnoseSawshark",       "Longnose Sawshark" },
            { "Lusca",                  "Lusca" },
            { "MackerelScad",           "Mackerel Scad" },
            { "NarrowBarredSpanishMackerel", "Narrow-Barred Spanish Mackerel" },
            { "PaintedComber",          "Painted Comber" },
            { "Sailfish",               "Sailfish" },
            { "SallyLightfootCrab",     "Sally Lightfoot Crab" },
            { "SmoothHammerhead",       "Smooth Hammerhead" },
            { "SpearSquid",             "Spear Squid" },
            { "SpinySeahorse",          "Spiny Seahorse" },
            { "StripedRedMullet",       "Striped Red Mullet" },
            { "TigerShark",             "Tiger Shark" },
            { "TigerTailSeahorse",      "Tiger-Tail Seahorse" },
            { "TropicalRockLobster",    "Tropical Rock Lobster" },
            { "WhiteSpottedJellyfish",  "White Spotted Jellyfish" },
            { "WhiteTrevally",          "White Trevally" },
            { "ZebraSeahorse",          "Zebra Seahorse" },
            // Blue Hole Deep
            { "BloodbelyCombJelly",     "Blood-belly Comb Jellyfish" },
            { "BluespottedStargazer",   "Bluespotted Stargazer" },
            { "ChamberedNautilus",      "Chambered Nautilus" },
            { "Clione",                 "Clione" },
            { "ClioneQueen",            "Clione Queen" },
            { "CombJelly",              "Comb Jelly" },
            { "CookiecutterShark",      "Cookiecutter Shark" },
            { "CrownedSeahorse",        "Crowned Seahorse" },
            { "EasternRockLobster",     "Eastern Rock Lobster" },
            { "Fangtooth",              "Fangtooth" },
            { "FrilledShark",           "Frilled Shark" },
            { "GiantWolfEel",           "Giant Wolf Eel" },
            { "GoblinShark",            "Goblin Shark" },
            { "LinedSeahorse",          "Lined Seahorse" },
            { "MegamouthShark",         "Megamouth Shark" },
            { "NorwayLobster",          "Norway Lobster" },
            { "PacificFanfish",         "Pacific Fanfish" },
            { "RedBream",               "Red Bream" },
            { "Rhinochimaeridae",       "Rhinochimaeridae" },
            { "SalmonSnailfish",        "Salmon Snailfish" },
            { "SeaToad",                "Sea Toad" },
            { "SpiderCrab",             "Spider Crab" },
            { "SpottedSeahorse",        "Spotted Seahorse" },
            { "ThretoothPuffer",        "Threetooth Puffer" },
            { "WhiteSeahorse",          "White Seahorse" },
            // Glacial Passage
            { "Barreleye",              "Barreleye" },
            { "Blobfish",               "Blobfish" },
            { "DumboOctopus",           "Dumbo Octopus" },
            { "PeacockSquid",           "Peacock Squid" },
            { "PelicanEel",             "Pelican Eel" },
            { "VampireSquid",           "Vampire Squid" },
            // Glacier Zone
            { "AlaskaPollock",          "Alaska Pollock" },
            { "AntarcticOctopus",       "Antarctic Octopus" },
            { "ArcticCod",              "Arctic Cod" },
            { "ArcticTelescopeFish",    "Arctic Telescope Fish" },
            { "Capelin",                "Capelin" },
            { "GelatinousSnailfish",    "Gelatinous Snailfish" },
            { "GoldenKingCrab",         "Golden King Crab" },
            { "GreenlandShark",         "Greenland Shark" },
            { "Haddock",                "Haddock" },
            { "HorsehairCrab",          "Horsehair Crab" },
            { "IceFish",                "Ice Fish" },
            { "LeafySeadragon",         "Leafy Seadragon" },
            { "Lumpfish",               "Lumpfish" },
            { "Narwhal",                "Narwhal" },
            { "PhantomJellyfish",       "Phantom Jellyfish" },
            { "PolarEelpout",           "Polar Eelpout" },
            { "PorbeagleShark",         "Porbeagle Shark" },
            { "SnowCrab",               "Snow Crab" },
            { "SnubNosedSpinyEel",      "Snub-nosed Spiny Eel" },
            { "StarrySkate",            "Starry Skate" },
            { "WeedySeadragon",         "Weedy Seadragon" },
            // Hydrothermal Vents
            { "Allenypterus",           "Allenypterus" },
            { "Anomalocaris",           "Anomalocaris" },
            { "DollocarisIngens",       "Dollocaris Ingens" },
            { "Drepanaspis",            "Drepanaspis" },
            { "Dunkleosteus",           "Dunkleosteus" },
            { "Falcatus",               "Falcatus" },
            { "Helicoprion",            "Helicoprion" },
            { "Kronosaurus",            "Kronosaurus" },
            { "Megalograptus",          "Megalograptus" },
            { "Pikaia",                 "Pikaia" },
            { "Qingmendous",            "Qingmendous" },
            { "RubySeadragon",          "Ruby Seadragon" },
            { "TokummiaKatalepsis",     "Tokummia Katalepsis" },
            { "WaptiaFieldensis",       "Waptia Fieldensis" },
            { "Xenacanthus",            "Xenacanthus" },
            { "Yawie",                  "Yawie" },
            // Aberrations — Jellyfish Basin
            { "AuroraJellyfish",        "Aurora Jellyfish" },
            { "BurstingAnglerfish",     "Bursting Anglerfish" },
            { "ConcertinaBarracuda",    "Concertina Barracuda" },
            { "EntangledCrab",          "Entangled Crab" },
            { "GazingShark",            "Gazing Shark" },
            { "ImperiousLobster",       "Imperious Lobster" },
            { "ParhelionJellyfish",     "Parhelion Jellyfish" },
            { "PerishedLoosejaw",       "Perished Loosejaw" },
            { "RadiantSquid",           "Radiant Squid" },
            { "SavageBarracuda",        "Savage Barracuda" },
            { "SeizingSnailfish",       "Seizing Snailfish" },
            // Aberrations — Fog Coast
            { "BarbedEel",              "Barbed Eel" },
            { "BloodskinShark",         "Bloodskin Shark" },
            { "CerebralCrab",           "Cerebral Crab" },
            { "FangedCod",              "Fanged Cod" },
            { "GrotesqueMackerel",      "Grotesque Mackerel" },
            { "HostEel",                "Host Eel" },
            { "MalignantPincer",        "Malignant Pincer" },
            { "ManyEyedMackerel",       "Many Eyed Mackerel" },
            { "SallowSailfish",         "Sallow Sailfish" },
            { "ThreeHeadedCod",         "Three-Headed Cod" },
            { "TuskedGrouper",          "Tusked Grouper" },
            { "VoltaicGrouper",         "Voltaic Grouper" },
            // Aberrations — Black Cliff
            { "BonyWreckfish",          "Bony Wreckfish" },
            { "CortexDecorator",        "Cortex Decorator" },
            { "EnthrallledStonefish",   "Enthralled Stonefish" },
            { "GelatinousStonefish",    "Gelatinous Stonefish" },
            { "GnashingPerch",          "Gnashing Perch" },
            { "ScouringBass",           "Scouring Bass" },
            { "ShatteredWreckfish",     "Shattered Wreckfish" },
            { "SplineredCrab",          "Splintered Crab" },
            { "SproutingEel",           "Sprouting Eel" },
            { "TranslucentSturgeon",    "Translucent Sturgeon" },
            { "WitheredRay",            "Withered Ray" },
        };

        public static string? GetDisplayNameFromGameObject(string goName)
        {
            // Try direct match first, then substring match
            foreach (var kvp in _map)
                if (goName.Contains(kvp.Key, System.StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            return null;
        }

        // Keep old method for any callers
        public static string? GetDisplayName(string fishId) => GetDisplayNameFromGameObject(fishId);
    }
}
