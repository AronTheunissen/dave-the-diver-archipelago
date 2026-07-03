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
        // ── Fallback: hook MissionManager.GetClearMissionDialogData ──────────
        // Fish first catches are tracked as missions in Dave the Diver.
        // This hook fires when ANY mission clears — we log the TID so we can
        // build the fish TID → AP location mapping.
        // UpdateMission has multiple overloads — removed to avoid ambiguous match error.

        // ── Secondary: hook FishInteractionBody.SuccessSubInteract ──────────
        // ✅ CONFIRMED via Unity Explorer: SuccessSubInteract(BaseCharacter) exists
        // This fires for small fish caught with harpoon/net (sub-interaction path)
        // as opposed to SuccessInteract which fires for large fish (carve path).
        [HarmonyPatch(typeof(FishInteractionBody), "SuccessSubInteract")]
        [HarmonyPostfix]
        public static void SuccessSubInteract_Postfix(FishInteractionBody __instance)
        {
            try
            {
                Plugin.Log.LogInfo($"[FishCatchPatch] SuccessSubInteract fired! GO={__instance?.gameObject?.name ?? "null"} Connected={ArchipelagoClient.IsConnected}");
                if (!ArchipelagoClient.IsConnected) return;
                if (!ItemQueue.IsGameLoaded) return;

                var goName = __instance?.gameObject?.name ?? "";
                if (string.IsNullOrEmpty(goName)) return;

                var locationName = FishNameMapper.GetLocationFromGameObject(goName);
                if (locationName != null)
                {
                    Plugin.Log.LogInfo($"[FishCaught via SubInteract] GO={goName} → Location=\"{locationName}\"");
                    ArchipelagoClient.CheckLocation(locationName);
                }
                else
                {
                    Plugin.Log.LogWarning($"[FishCaught via SubInteract] GO={goName} — no location mapping found");
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[FishCatchPatch] SuccessSubInteract_Postfix threw: {ex}");
            }
        }

        // ── Primary: hook FishInteractionBody.SuccessInteract ────────────────
        // ✅ CONFIRMED: fires for LARGE fish (kill + carve path) only.
        //    Small fish caught with harpoon/net use a different pickup path.
        // ✅ CONFIRMED via dump.cs: SuccessInteract(BaseCharacter player) is the real method name
        [HarmonyPatch(typeof(FishInteractionBody), "SuccessInteract")]
        [HarmonyPostfix]
        public static void SuccessInteract_Postfix(FishInteractionBody __instance)
        {
            try
            {
                Plugin.Log.LogInfo($"[FishCatchPatch] SuccessInteract fired! GO={__instance?.gameObject?.name ?? "null"} Connected={ArchipelagoClient.IsConnected}");
                if (!ArchipelagoClient.IsConnected) return;

                // GameObject name format confirmed via UnityExplorer (2026-06-27):
                // "SA_2010132_Thresher_Shark01(Clone)" — TID is the number after "SA_"
                // This is much more reliable than string matching on species names.
                string goName = __instance.gameObject.name;

                // Try TID-based lookup first (preferred)
                var fishName = FishNameMapper.GetDisplayNameFromTID(goName);

                // Fall back to name-based lookup for any fish not yet in the TID map
                if (fishName == null)
                    fishName = FishNameMapper.GetDisplayNameFromGameObject(goName);

                if (fishName != null)
                {
                    Plugin.Log.LogInfo($"[FishCaught] GO={goName} → Location=\"First Catch: {fishName}\"");
                    LocationTracker.OnFirstFishCatch(fishName);
                }
                else
                {
                    Plugin.Log.LogInfo($"[FishCaught] GO={goName} → UNMAPPED (add to FishNameMapper)");
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[FishCatchPatch] SuccessInteract_Postfix threw: {ex}");
            }
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

        // TID-based lookup — parses the TID from GameObject name "SA_2010132_Thresher_Shark01(Clone)"
        // TIDs confirmed via CaughtFishData dump (2026-06-27)
        private static readonly System.Collections.Generic.Dictionary<int, string> _tidMap = new()
        {
            // ── Blue Hole Shallow ─────────────────────────────────────────────
            { 2010002, "Clownfish" },
            { 2010003, "Comber" },
            { 2010004, "Cardinal Fish" },
            { 2010005, "Sea Goldie" },
            { 2010006, "Pyramid Butterflyfish" },
            { 2010007, "Yellow Tang" },
            { 2010008, "Salema Porgy" },
            { 2010009, "Orbicular Batfish" },
            { 2010010, "Blue Tang" },
            { 2010011, "Long-Snouted Seahorse" },
            { 2010012, "Rainbow Wrasse" },
            { 2010013, "Lagoon Triggerfish" },
            { 2010014, "Small Spotted Dart" },
            { 2010015, "Yellowback Fusilier" },
            { 2010016, "Ornate Wrasse" },
            { 2010017, "Longfin Batfish" },
            { 2010018, "Mediterranean Parrotfish" },
            { 2010019, "Redtoothed Triggerfish" },
            { 2010020, "Black and White Snapper" },
            { 2010021, "Green Humphead Parrotfish" },
            { 2010022, "Fried Egg Jellyfish" },
            { 2010023, "Barrel Jellyfish" },
            { 2010025, "Red Lionfish" },
            { 2010027, "Starry Puffer" },
            { 2010028, "Moray Eel" },
            { 2010029, "Titan Triggerfish" },
            { 2010030, "Sheepshead" },
            { 2010031, "Red-banded Lobster" },
            { 2010033, "Flame Angelfish" },
            { 2010034, "Emperor Angelfish" },
            { 2010036, "Striped Catfish" },
            { 2010037, "Longspine Porcupinefish" },
            { 2010038, "Longspine Squirrelfish" },
            { 2010039, "Clearfin Lionfish" },
            { 2010040, "Purple Sea Urchin" },
            { 2010041, "Red-banded Lobster" },
            { 2010042, "American Lobster" },
            { 2010043, "Blue Lobster" },
            { 2010044, "European Lobster" },
            { 2010045, "Green Sea Urchin" },
            { 2010058, "Stingray" },
            { 2010059, "Whiteleg Shrimp" },
            { 2010060, "Box Jellyfish" },
            { 2010061, "White Shrimp" },
            { 2010062, "Shortfin Mako" },
            { 2010064, "Marlin" },
            { 2010065, "Thresher Shark" },
            { 2010066, "Blacktip Reefshark" },
            { 2010067, "Whitetip Reefshark" },
            { 2010069, "Copper Shark" },
            { 2010070, "Zebra Shark" },
            { 2010071, "Bluefin Tuna" },
            { 2010072, "Yellowfin Tuna" },
            { 2010073, "Marbled Electric Ray" },
            { 2010074, "Sailfish" },
            { 2010075, "Smooth Hammerhead" },
            { 2010077, "White Shrimp" },
            { 2010078, "Sally Lightfoot Crab" },
            { 2010079, "Black Tiger Shrimp" },
            { 2010080, "Mantis Shrimp" },
            { 2010081, "California Spiny Lobster" },
            { 2010082, "Tropical Rock Lobster" },
            { 2010083, "Fan Lobster" },
            { 2010084, "Crystal Lobster" },
            { 2010085, "Humboldt Squid" },
            // Seahorses
            { 2010114, "Big-Belly Seahorse" },
            { 2010115, "Jayakar's Seahorse" },
            { 2010116, "Pacific Seahorse" },
            { 2010117, "Dwarf Seahorse" },
            { 2010118, "Giraffe Seahorse" },
            { 2010119, "Hedgehog Seahorse" },
            // ── Blue Hole Mid ─────────────────────────────────────────────────
            { 2010101, "Bluehead Tilefish" },
            { 2010102, "Clown Frogfish" },
            { 2010103, "Painted Comber" },
            { 2010105, "Bigeye Scad" },
            { 2010106, "Striped Red Mullet" },
            { 2010107, "Mackerel Scad" },
            { 2010108, "Harlequin Hind" },
            { 2010109, "Bigeye Trevally" },
            { 2010110, "Coral Trout" },
            { 2010111, "Grey Triggerfish" },
            { 2010112, "Atlantic Bonito" },
            { 2010113, "White Trevally" },
            { 2010121, "Great Barracuda" },
            { 2010123, "Narrow-Barred Spanish Mackerel" },
            { 2010124, "Cuttlefish" },
            { 2010125, "Dusky Grouper" },
            { 2010129, "Atlantic Mackerel" },
            { 2010130, "Spear Squid" },
            { 2010131, "Blackfin Barracuda" },
            { 2010132, "Thresher Shark" },  // confirmed from GO name
            { 2010133, "Tiger Shark" },
            { 2010134, "Sailfish" },
            { 2010135, "Longnose Sawshark" },
            { 2010136, "Giant Trevally" },
            { 2010137, "Spear Squid" },
            { 2010138, "Devil Scorpionfish" },
            { 2010139, "California Spiny Lobster" },
            { 2010140, "Fan Lobster" },
            { 2010141, "Crystal Lobster" },
            { 2010142, "White Spotted Jellyfish" },
            // Seahorses mid
            { 2010120, "Spiny Seahorse" },
            { 2010122, "Tiger-Tail Seahorse" },
            { 2010126, "Zebra Seahorse" },
            // ── Blue Hole Deep ────────────────────────────────────────────────
            { 2010201, "Chambered Nautilus" },
            { 2010202, "Fangtooth" },
            { 2010204, "Clione" },
            { 2010205, "Sea Toad" },
            { 2010207, "Pacific Fanfish" },
            { 2010208, "Cookiecutter Shark" },
            { 2010211, "Salmon Snailfish" },
            { 2010212, "Spider Crab" },
            { 2010214, "Blood-belly Comb Jellyfish" },
            { 2010215, "Comb Jelly" },
            { 2010216, "Crowned Seahorse" },
            { 2010217, "Threetooth Puffer" },
            { 2010218, "Bluespotted Stargazer" },
            { 2010219, "Red Bream" },
            { 2010220, "Rhinochimaeridae" },
            { 2010221, "Megamouth Shark" },
            { 2010222, "Frilled Shark" },
            { 2010223, "Norway Lobster" },
            { 2010224, "Eastern Rock Lobster" },
            { 2010225, "Lined Seahorse" },
            { 2010226, "Spotted Seahorse" },
            { 2010227, "White Seahorse" },
            { 2010228, "Atlantic Anglerfish" },
            { 2010229, "Box Jellyfish" },
            { 2010230, "Megamouth Shark" },
            { 2010231, "Spider Crab" },
            { 2010232, "Frilled Shark" },
            { 2010233, "Norway Lobster" },
            { 2010234, "Eastern Rock Lobster" },
            { 2010236, "Megamouth Shark" },
            { 2010237, "Frilled Shark" },
            { 2010238, "Norway Lobster" },
            { 2010240, "Eastern Rock Lobster" },
            { 2010241, "Spider Crab" },
            // ── Glacial Passage ───────────────────────────────────────────────
            { 2010301, "Peacock Squid" },
            { 2010302, "Dumbo Octopus" },
            { 2010303, "Barreleye" },
            { 2010304, "Vampire Squid" },
            { 2010305, "Blobfish" },
            { 2010306, "Pelican Eel" },
            // ── Glacier Zone ──────────────────────────────────────────────────
            { 2010401, "Arctic Cod" },
            { 2010402, "Gelatinous Snailfish" },
            { 2010403, "Antarctic Octopus" },
            { 2010404, "Polar Eelpout" },
            { 2010405, "Ice Fish" },
            { 2010406, "Arctic Telescope Fish" },
            { 2010407, "Lumpfish" },
            { 2010408, "Capelin" },
            { 2010409, "Snow Crab" },
            { 2010410, "Haddock" },
            { 2010411, "Golden King Crab" },
            { 2010412, "Horsehair Crab" },
            { 2010413, "Narwhal" },
            { 2010414, "Starry Skate" },
            { 2010415, "Snub-nosed Spiny Eel" },
            { 2010416, "Greenland Shark" },
            { 2010417, "Porbeagle Shark" },
            { 2010418, "Alaska Pollock" },
            // Glacier seahorses (TIDs TBD — these overlap with existing entries, need confirmation)
            // { 2010xxx, "Leafy Seadragon" },
            // { 2010xxx, "Weedy Seadragon" },
            // ── Hydrothermal Vents ────────────────────────────────────────────
            { 2010501, "Waptia Fieldensis" },
            { 2010502, "Pikaia" },
            { 2010503, "Allenypterus" },
            { 2010504, "Dollocaris Ingens" },
            { 2010505, "Falcatus" },
            { 2010506, "Anomalocaris" },
            { 2010507, "Megalograptus" },
            { 2010508, "Qingmendous" },
            { 2010509, "Xenacanthus" },
            { 2010510, "Tokummia Katalepsis" },
            { 2010511, "Dunkleosteus" },
            { 2010512, "Drepanaspis" },
            // Vents seahorse
            { 2010550, "Ruby Seadragon" },
            // ── Boss fish ─────────────────────────────────────────────────────
            { 2010801, "Great White Shark Klaus" },  // Vortex boss
            { 2010901, "Mantis Shrimp" },            // Boss
            // ── Aberrations — Jellyfish Basin ─────────────────────────────────
            { 2011201, "Aurora Jellyfish" },
            { 2011202, "Bursting Anglerfish" },
            { 2011203, "Parhelion Jellyfish" },
            { 2011204, "Radiant Squid" },
            { 2011205, "Seizing Snailfish" },
            { 2011206, "Perished Loosejaw" },
            { 2011207, "Savage Barracuda" },
            { 2011208, "Concertina Barracuda" },
            { 2011209, "Entangled Crab" },
            { 2011210, "Imperious Lobster" },
            { 2011211, "Gazing Shark" },
            // ── Aberrations — Fog Coast ───────────────────────────────────────
            { 2011213, "Fanged Cod" },
            { 2011214, "Grotesque Mackerel" },
            { 2011215, "Many Eyed Mackerel" },
            { 2011216, "Three-Headed Cod" },
            { 2011217, "Cerebral Crab" },
            { 2011218, "Barbed Eel" },
            { 2011219, "Voltaic Grouper" },
            { 2011220, "Tusked Grouper" },
            { 2011221, "Host Eel" },
            { 2011222, "Malignant Pincer" },
            { 2011223, "Sallow Sailfish" },
            { 2011224, "Bloodskin Shark" },
            // ── Aberrations — Black Cliff ─────────────────────────────────────
            { 2011225, "Gelatinous Stonefish" },
            { 2011226, "Enthralled Stonefish" },
            { 2011227, "Gnashing Perch" },
            { 2011228, "Scouring Bass" },
            { 2011229, "Cortex Decorator" },
            { 2011230, "Shattered Wreckfish" },
            { 2011231, "Bony Wreckfish" },
            { 2011232, "Sprouting Eel" },
            { 2011233, "Splintered Crab" },
            { 2011234, "Withered Ray" },
            { 2011212, "Translucent Sturgeon" },
            // ── Godzilla DLC fish ─────────────────────────────────────────────
            { 2012006, "Aurora Jellyfish" },
            { 2012007, "Barbed Eel" },
            { 2012008, "Bloodskin Shark" },
            { 2012009, "Bony Wreckfish" },
            { 2012010, "Bursting Anglerfish" },
            { 2012011, "Cerebral Crab" },
            { 2012012, "Concertina Barracuda" },
            { 2012013, "Cortex Decorator" },
            { 2012014, "Entangled Crab" },
            { 2012015, "Enthralled Stonefish" },
            { 2012017, "Fanged Cod" },
            { 2012018, "Gazing Shark" },
            { 2012019, "Gelatinous Stonefish" },
            { 2012020, "Gnashing Perch" },
            { 2012021, "Grotesque Mackerel" },
            { 2012022, "Host Eel" },
            { 2012023, "Imperious Lobster" },
            // ── Jungle DLC fish (TIDs confirmed via GO name dump 2026-06-27) ──
            // Base game fish appearing in jungle lake (201060x range)
            { 2010601, "Kissing Gourami" },
            { 2010602, "Walking Catfish" },
            { 2010604, "Chocolate Gourami" },
            { 2010605, "Red-Bellied Piranha" },
            { 2010608, "Tilapia" },
            { 2010609, "Red Discus" },
            { 2010610, "Lemon Yellow Lab" },
            { 2010611, "Bluegray Mbuna" },
            { 2010612, "Pearl Gourami" },
            // Jungle DLC exclusive fish (4201xxxx range — prefix 4 = Jungle DLC)
            { 42010102, "Walking Catfish" },
            { 42010104, "Largemouth Bass" },
            { 42010105, "Tilapia" },
            { 42010109, "Archerfish" },
            { 42010110, "Black Caiman" },
            { 42010111, "Brown Discus" },
            { 42010112, "Green Discus" },
            { 42010113, "Blue Discus" },
            { 42010114, "Heckel Discus" },
            { 42010203, "Mud Carp" },
            { 42010204, "Piraiba Catfish" },
            { 42010205, "Bluegill" },
            { 42010206, "Indonesian Tiger Perch" },
            { 42010207, "Grass Carp" },
            { 42010301, "Giant Freshwater Stingray" },
            { 42010302, "Electric Eel" },
            { 42010303, "Nile Perch" },
            { 42010304, "Horse Face Loach" },
            { 42010305, "Alligator Gar" },
            { 42010306, "Great Sturgeon" },
            { 42010308, "King Salmon" },
            { 42010401, "Giant Snakehead" },
            { 42010402, "Armored Catfish" },
            { 42010503, "Asian Arowana" },
            { 42010601, "Pirarucu" },
            { 42010701, "Redeye Piranha" },
            { 42010801, "Fire Eel" },
            { 42010901, "Goliath Tigerfish" },
            { 42011001, "Clown Featherback" },
            // NOTE: TIDs 42011103/106/107 appear in BOTH lake (Clown Loach/Largemouth Bass/Mud Carp)
            // AND lakebed (Ophthalmosaurus/Stylonurus/Ammonite). The game reuses TID slots per zone.
            // Mapped to the lakebed (ancient) fish here since they share the same TID.
            // The GO name substring will distinguish them when the name-based fallback runs.
            { 42011103, "Ophthalmosaurus" },   // Also: Clown Loach in lake zone
            { 42011106, "Stylonurus" },         // Also: Largemouth Bass in lake zone
            { 42011107, "Ammonite" },           // Also: Mud Carp in lake zone
            // ── Jungle lakebed ancient fish (4201111x range, confirmed 2026-06-27) ──
            { 42011101, "Eagle Shark" },
            { 42011104, "Parameteroraspis" },
            { 42011105, "Paradoxides" },
            { 42011108, "Tullimonstrum" },
            { 42011109, "Promissum" },
            { 42011110, "Hensodon" },
            { 42011111, "Red Feather Starfish" },
            { 42011112, "Eomesodon" },
            { 42011113, "Exellia" },
            { 42011114, "Foreyia" },
            { 42011115, "Orthoceras" },
            { 42011116, "Burgessomedusa" },
            { 42011119, "Gyrodus" },
            { 42011120, "Sacabambaspis" },
            // ── Jungle rod-caught fish (42013xxx range, confirmed 2026-06-27) ────
            { 42013501, "Moonlight Gourami" },   // confirmed via FishInfo.TID
            // TODO: catch more rod fish to fill in remaining 42013xxx TIDs
            // ── Special / Seahorse farm ───────────────────────────────────────
            { 2013001, "Long-Snouted Seahorse" },
            { 2013002, "Big-Belly Seahorse" },
            { 2013003, "Jayakar's Seahorse" },
            { 2013004, "Pacific Seahorse" },
            { 2013005, "Dwarf Seahorse" },
            { 2013006, "Giraffe Seahorse" },
            { 2013007, "Hedgehog Seahorse" },
            { 2013009, "Spiny Seahorse" },
            { 2013010, "Tiger-Tail Seahorse" },
            { 2013011, "Zebra Seahorse" },
            { 2013012, "Crowned Seahorse" },
            { 2013015, "Leafy Seadragon" },
        };

        public static string? GetDisplayNameFromTID(string goName)
        {
            // Parse TID from "SA_2010132_Thresher_Shark01(Clone)" format
            var parts = goName.Split('_');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int tid))
            {
                if (_tidMap.TryGetValue(tid, out var name))
                    return name;
            }
            return null;
        }

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

        // Mission TID → AP location name for fish first-catch missions.
        // TIDs are logged via [MissionCleared] debug output — populate as we discover them.
        // Format: game fires GetClearMissionDialogData with a MissionData whose TID
        // corresponds to the "First Catch: X" mission for each fish species.
        private static readonly System.Collections.Generic.Dictionary<int, string> _missionTidMap = new()
        {
            // TODO: populate from [MissionCleared] log output while playing
            // Example: { 12345678, "First Catch: Clownfish" }
        };

        public static string? GetLocationFromMissionTID(int missionTID) =>
            _missionTidMap.TryGetValue(missionTID, out var name) ? name : null;

        // Fish ID → AP location name.
        // fishId is the game's internal fish data TID (from DR_GameData_Fish.json).
        // These are logged via [FishGet] fishId=XXXXX when a fish is caught.
        // Populate from log output while playing.
        private static readonly System.Collections.Generic.Dictionary<int, string> _fishIdMap = new()
        {
            // TODO: populate from [FishGet] log output while playing
            // Example: { 14011001, "First Catch: Clownfish" }
        };

        public static string? GetLocationFromFishId(int fishId) =>
            _fishIdMap.TryGetValue(fishId, out var name) ? name : null;
    }
}
