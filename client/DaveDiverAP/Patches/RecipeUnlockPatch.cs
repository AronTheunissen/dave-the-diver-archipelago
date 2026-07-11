using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the recipe unlock and dish upgrade system.
    ///
    /// IMPORTANT: Class/method names are PLACEHOLDERS.
    /// Look for: RecipeManager, CookingManager, MenuManager, DishUpgradeManager.
    /// The recipe unlock likely fires when a new recipe becomes available in the menu.
    /// The dish upgrade fires when research is completed using Artisan's Flame.
    /// </summary>
    [HarmonyPatch]
    public static class RecipeUnlockPatch
    {
        // ── Boss/story recipe TIDs — always pass through unblocked ──────────────────────────
        // These are tied to boss defeats and story progression. They must not be blocked
        // as they trigger critical story events (staff unlocks, chapter progression, etc.).
        private static readonly System.Collections.Generic.HashSet<int> _bossRecipeTIDs = new()
        {
            8051009,  // Whole-Roasted Shark Head (unlocks staff hiring!)
            8051101,  // Steamed Wolf Eel
            8051102,  // Clione Queen Soup
            8051103,  // Goblin Shark Belly Roast
            8051104,  // Stir-Fried Hermit Crab and Seaweed
            8051105,  // Boiled Mantis Shrimp with Soy Paste
            8051106,  // White Shark Omelet
            8051107,  // Phantom Jellyfish Jelly
            8051108,  // Roasted Helicoprion Tail
            8051109,  // Steamed Kronosaurus Tongue
            8051110,  // Yawie Steamed Meat
            8051111,  // Blanched Lusca Tentacle (jungle DLC boss)
            8051112,  // Lusca Neck Tadaki (jungle DLC boss)
        };

        // ── Recipe becomes researchable — BLOCK and send AP check ───────────────────────────
        // ✅ CONFIRMED: AddUnlockRecipeSaveData(int id, DateTime unlockTime) fires when a
        // recipe becomes RESEARCHABLE (e.g. Cooksta rank up, staff level up, story event).
        //
        // Design: we BLOCK this call so the recipe does NOT become researchable in vanilla.
        // We send the AP check instead. The AP item "Recipe: X" calls ApplyRecipeUnlock
        // which makes it researchable AND auto-researches it to level 1.
        //
        // Boss/story recipes pass through unblocked (story-critical).
        // AP-driven calls (_allowDishSave=true) pass through (used by ApplyRecipeUnlock).
        //
        // Note: the research menu UI may lock if unlockRecipeData is empty.
        // TODO: Patch the research menu UI check to always show the menu open,
        //       regardless of whether unlockRecipeData has entries.
        [HarmonyPatch(typeof(global::SaveData), "AddUnlockRecipeSaveData")]
        [HarmonyPrefix]
        public static bool UnlockRecipe_Prefix(int id)
        {
            if (!ArchipelagoClient.IsConnected) return true;
            if (_allowDishSave) return true; // AP-driven — allow through
            if (_bossRecipeTIDs.Contains(id)) return true; // boss/story recipe — allow through

            // Block vanilla unlock and send AP check instead
            var recipeName = RecipeNameMapper.GetDisplayName(id);
            Plugin.Log.LogInfo($"[Recipe] Blocked vanilla researchable: {recipeName ?? $"TID={id}"} — sending AP check");
            if (recipeName != null)
                LocationTracker.OnRecipeUnlocked(recipeName);
            return false; // block the save
        }

        [HarmonyPatch(typeof(global::SaveData), "AddUnlockRecipeSaveData")]
        [HarmonyPostfix]
        public static void UnlockRecipe_Postfix(int id)
        {
            // Only fires when _allowDishSave=true (AP-driven) or boss recipe — nothing to do.
        }

        // ── Block auto-leveling by game (prefix) ─────────────────────────────
        // When connected to AP, dish research levels are controlled by AP items,
        // NOT by the game's normal progression (catching fish auto-levels sushi).
        // We cancel AddCookingStudySaveData unless AP explicitly triggered it.
        internal static bool _allowDishSave = false;

        // ── Block AddCookingStudySaveData level=0 (recipe becoming researchable) ─────────────
        // Level 0 = game creating a new cookingStudySave entry at "researchable" state.
        // This fires BEFORE UpdateCookingStudySaveData(level=0) for the same event.
        // We block it to prevent recipes appearing as researchable without AP permission.
        // Boss/story recipes and AP-driven saves pass through unblocked.
        [HarmonyPatch(typeof(global::SaveData), "AddCookingStudySaveData")]
        [HarmonyPrefix]
        public static bool AddCookingStudySaveData_Prefix(CookingStudyData data)
        {
            if (!ArchipelagoClient.IsConnected) return true;
            if (_allowDishSave) return true;
            if (data == null) return true;

            // Only block level 0 (researchable state) — level 1+ passes through
            if (data.studyLevel != 0) return true;

            // Allow boss/story recipes through
            if (_bossRecipeTIDs.Contains(data.recipeID)) return true;

            var dishName = RecipeNameMapper.GetDisplayName(data.recipeID);
            Plugin.Log.LogInfo($"[Recipe] Blocked AddCookingStudySaveData level-0: {dishName ?? $"TID={data.recipeID}"} (AP controls this)");
            return false; // block
        }

        // ── Dish unlock/upgrade (postfix for AP check sending) ───────────────
        // ✅ CONFIRMED via Unity Explorer: AddCookingStudySaveData(CookingStudyData data)
        // fires when a dish research is completed. CookingStudyData has id and level fields.
        [HarmonyPatch(typeof(global::SaveData), "AddCookingStudySaveData")]
        [HarmonyPostfix]
        public static void AddCookingStudySaveData_Postfix(CookingStudyData data)
        {
            try
            {
                if (!ArchipelagoClient.IsConnected) return;
                if (_allowDishSave) return; // AP-driven save — don't send duplicate check
                if (data == null) return;

                int tid = data.recipeID;
                int level = data.studyLevel;

                // Level 0 = researchable state — blocked by prefix, postfix still fires
                // but we should NOT send a check here.
                if (level <= 0) return;

                Plugin.Log.LogInfo($"[DishUpgrade] AddCookingStudySaveData TID={tid} Level={level}");

                var dishName = RecipeNameMapper.GetDisplayName(tid);
                if (dishName != null)
                {
                    Plugin.Log.LogInfo($"[DishUpgrade] {dishName} → Level {level}");
                    // Level 1 = first sushi unlock (fish catch) → send recipe unlock check
                    // Level 2+ = research upgrade → send dish upgrade check
                    if (level == 1)
                        LocationTracker.OnRecipeUnlocked(dishName);
                    else
                        LocationTracker.OnDishUpgraded(dishName, level);
                }
                else
                {
                    Plugin.Log.LogInfo($"[DishUpgrade] Unknown dish TID={tid} level={level}");
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[RecipeUnlockPatch] AddCookingStudySaveData_Postfix threw: {ex}");
            }
        }

        // ── DESIGN: Dish research (Option 1 — game tracks levels naturally) ─────────────────
        // Both sushi (8050xxx/8052xxx) AND cooked dishes (8051xxx) use cookingStudySave:
        //   - AddCookingStudySaveData  → first time a dish gets a research level (unlock / first research)
        //   - UpdateCookingStudySaveData → subsequent level increases (restaurant research panel)
        // We do NOT block either — the game handles levels normally.
        // We only observe via postfixes to send AP checks.
        // No AP items are sent back for dish upgrades (one-way checks).

        // ── Block UpdateCookingStudySaveData level=0 (recipe becoming researchable) ──────────
        // Level 0 = game marking a dish as "researchable" in cookingStudySave.
        // We BLOCK this so dishes don't appear as researchable without AP sending the item.
        // Boss/story recipes and AP-driven saves pass through unblocked.
        [HarmonyPatch(typeof(global::SaveData), "UpdateCookingStudySaveData")]
        [HarmonyPrefix]
        public static bool UpdateCookingStudySaveData_Prefix(CookingStudyData data)
        {
            if (!ArchipelagoClient.IsConnected) return true;
            if (_allowDishSave) return true;
            if (data == null) return true;

            // Only block level 0 (researchable state) — all other levels pass through
            if (data.studyLevel != 0) return true;

            // Allow boss/story recipes through (they pass through AddUnlockRecipeSaveData too)
            if (_bossRecipeTIDs.Contains(data.recipeID)) return true;

            var dishName = RecipeNameMapper.GetDisplayName(data.recipeID);
            Plugin.Log.LogInfo($"[Recipe] Blocked level-0 researchable state: {dishName ?? $"TID={data.recipeID}"} (AP controls this)");
            return false; // block
        }

        // ── Hook UpdateCookingStudySaveData to send AP check when dish is upgraded ──────────
        [HarmonyPatch(typeof(global::SaveData), "UpdateCookingStudySaveData")]
        [HarmonyPostfix]
        public static void UpdateCookingStudySaveData_Postfix(CookingStudyData data)
        {
            try
            {
                if (!ArchipelagoClient.IsConnected) return;
                if (_allowDishSave) return; // AP-driven save — don't send duplicate check
                if (data == null) return;

                int tid = data.recipeID;
                int level = data.studyLevel;

                // Level 0 = dish becoming researchable (not yet researched) — ignore
                if (level <= 0) return;

                var dishName = RecipeNameMapper.GetDisplayName(tid);
                if (dishName != null)
                {
                    Plugin.Log.LogInfo($"[DishUpgrade] UpdateCookingStudySaveData: {dishName} → Level {level}");
                    if (level == 1)
                    {
                        // Level 1 via UpdateCookingStudySaveData = game setting dish as researchable
                        // or AP auto-researching it. The AP check was already sent at
                        // AddUnlockRecipeSaveData time (UnlockRecipe_Prefix). Do NOT send again.
                        Plugin.Log.LogInfo($"[DishUpgrade] Level 1 via Update — AP check already sent at researchable time, skipping.");
                    }
                    else
                    {
                        // Level 2+ = upgrade in restaurant research panel
                        LocationTracker.OnDishUpgraded(dishName, level);
                    }
                }
                else
                {
                    Plugin.Log.LogInfo($"[DishUpgrade] Unknown dish TID={tid} level={level}");
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[RecipeUnlockPatch] UpdateCookingStudySaveData_Postfix threw: {ex}");
            }
        }

        // ── Stubs for GameStatePatch compatibility (no longer needed) ────────────────────
        internal static void SnapshotRecipeLevels(global::SaveData saveData) { /* no-op */ }
        internal static void UpdateSnapshotLevel(int recipeTID, int level) { /* no-op */ }
    }

    // ── Lock the Research button in the restaurant UI ─────────────────────────────────────
    // SushiBarButton.Refresh() is called whenever the button state is updated.
    // We hook it to force Lock=true on the Research button when AP is connected.
    // This prevents players from manually researching recipes (AP controls recipe unlocks).
    [HarmonyPatch(typeof(global::SushiBarButton), "Refresh")]
    public static class SushiBarResearchButtonPatch
    {
        [HarmonyPostfix]
        public static void Postfix(global::SushiBarButton __instance)
        {
            try
            {
                if (!ArchipelagoClient.IsConnected) return;

                // Check if this is the Research button by its MenuType
                // MenuType is an enum — Research type needs to be confirmed
                // For now use the button name as fallback
                var go = __instance.gameObject;
                if (go == null) return;
                if (!go.name.Contains("Research", System.StringComparison.OrdinalIgnoreCase)) return;

                // Force the research button to always be locked when AP is connected
                if (!__instance.Lock)
                {
                    __instance.Lock = true;
                    Plugin.Log.LogInfo("[SushiBar] Research button locked (AP controls recipe unlocks)");
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[SushiBarResearchButtonPatch] Postfix threw: {ex.Message}");
            }
        }
    }

    public static class RecipeNameMapper
    {
        // TODO: Fill in with real internal recipe/dish IDs from decompiled game
        // Maps recipe TID (design sheet integer ID) to AP location display name.
        // TODO: Fill in by cross-referencing the game's recipe design sheet data
        // (search dump.cs for "RecipeTID" or open design tables in UnityExplorer)
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // ── Basic fish sushi (catch-unlocked) ────────────────────────────────
            { 8050001, "Clownfish Sushi" },
            { 8050002, "Comber Sushi" },
            { 8050003, "Cardinalfish Sushi" },
            { 8050004, "Sea Goldie Sushi" },
            { 8050005, "Pyramid Butterflyfish Sushi" },
            { 8050006, "Yellow Tang Sushi" },
            { 8050007, "Salema Porgy Sushi" },
            { 8050008, "Orbicular Batfish Fry" },
            { 8050009, "Blue Tang Sushi" },
            { 8050011, "Rainbow Wrasse Sushi" },
            { 8050012, "Lagoon Triggerfish Sushi" },
            { 8050013, "Smallspotted Dart Sushi" },
            { 8050014, "Yellowback Fusilier Sushi" },
            { 8050015, "Ornate Wrasse Sushi" },
            { 8050016, "Longfin Batfish Sushi" },
            { 8050017, "Mediterranean Parrotfish Sushi" },
            { 8050018, "Redtoothed Triggerfish Sushi" },
            { 8050019, "B&W Snapper Sushi" },
            { 8050020, "Green Humphead Parrotfish Sushi" },
            { 8050021, "Red Lionfish Sushi" },
            { 8050022, "Bluehead Tilefish Sushi" },
            { 8050023, "Clown Frogfish Sushi" },
            { 8050024, "Painted Comber Sushi" },
            { 8050026, "Bigeye Scad Sushi" },
            { 8050027, "Striped Red Mullet Sushi" },
            { 8050029, "Harlequin Hind Sushi" },
            { 8050030, "Bigeye Trevally Sushi" },
            { 8050031, "Coral Trout Sushi" },
            { 8050032, "Grey Triggerfish Sushi" },
            { 8050033, "Atlantic Bonito Sushi" },
            { 8050034, "Atlantic Mackerel Sushi" },
            { 8050035, "White Trevally Sushi" },
            { 8050036, "Cuttlefish Sushi" },
            { 8050037, "Dusky Grouper Sushi" },
            { 8050038, "Narrow-barred Spanish mackerel Sushi" },
            { 8050042, "Giant Trevally Sushi" },
            { 8050043, "Blackfin Barracuda Sushi" },
            { 8050044, "Whitetip Reefshark Sushi" },
            { 8050045, "Tiger shark Sushi" },
            { 8050046, "Barrel Jellyfish Sushi" },
            { 8050047, "Fried Egg Jellyfish Sushi" },
            { 8050048, "White Spotted Jellyfish Sushi" },
            { 8050049, "Great Barracuda Sushi" },
            { 8050050, "Mackerel Scad Sushi" },
            { 8050051, "Titan Triggerfish Sushi" },
            { 8050052, "Norimaki" },
            { 8050053, "Longnose Sawshark Sushi" },
            { 8050054, "Chambered Nautilus Sushi" },
            { 8050055, "Fangtooth Sushi" },
            { 8050056, "Frilled Shark Sushi" },
            { 8050057, "Bluespotted Stargazer Sushi" },
            { 8050059, "Rhinochimaeridae Sushi" },
            { 8050060, "Spider Crab Sushi" },
            { 8050061, "Megamouth Shark Sushi" },
            { 8050062, "Cookiecutter Shark Sushi" },
            { 8050063, "Sea toad Sushi" },
            { 8050064, "Salmon Snailfish Sushi" },
            { 8050065, "Pacific Fanfish Sushi" },
            { 8050066, "Threetooth Puffer Sushi" },
            { 8050067, "Red bream Sushi" },
            { 8050068, "Atlantic Anglerfish Sushi" },
            { 8050069, "Comb Jelly Sushi" },
            { 8050070, "Blood-belly Comb Jelly Sushi" },
            { 8050071, "Blacktip Reefshark Sushi" },
            { 8050072, "Copper shark Sushi" },
            { 8050073, "Box Jellyfish Sushi" },
            { 8050074, "Moray Eel Sushi" },
            { 8050075, "Sally Lightfoot Crab Sushi" },
            { 8050076, "Peacock Squid Sushi" },
            { 8050077, "Dumbo Octopus Sushi" },
            { 8050078, "Barreleye Sushi" },
            { 8050079, "Blobfish Sushi" },
            { 8050080, "Vampire Squid Sushi" },
            { 8050081, "Arctic Cod Sushi" },
            { 8050082, "Gelatinous Snailfish Sushi" },
            { 8050083, "Antarctic Octopus Sushi" },
            { 8050084, "Greenland Shark Sushi" },
            { 8050085, "Polar Eelpout Sushi" },
            { 8050086, "Porbeagle Shark Sushi" },
            { 8050087, "Ice Fish Sushi" },
            { 8050088, "Capelin Sushi" },
            { 8050089, "Narwhal Sushi" },
            { 8050090, "Haddock Sushi" },
            { 8050091, "Starry Skate Sushi" },
            { 8050092, "Shortfin Mako Sushi" },
            { 8050093, "Thresher Shark Sushi" },
            { 8050094, "Smooth Hammerhead Sushi" },
            { 8050095, "Zebra Shark Sushi" },
            { 8050096, "Pelican Eel Sushi" },
            { 8050097, "White Shrimp Sushi" },
            { 8050098, "Humboldt Squid Sushi" },
            { 8050099, "Devil Scorpionfish Sushi" },
            { 8050100, "Marlin Sushi" },
            { 8050101, "Swordfish Sushi" },
            { 8050102, "Sailfish Sushi" },
            { 8050103, "Waptia Sushi" },
            { 8050104, "Pikaia Sushi" },
            { 8050105, "Allenypterus Sushi" },
            { 8050106, "Qingmenodus Sushi" },
            { 8050107, "Falcatus Sushi" },
            { 8050108, "Drepanaspis Sushi" },
            { 8050109, "Dunkleosteus Sushi" },
            { 8050110, "Megalograptus Sushi" },
            { 8050111, "Young Anomalocaris Sushi" },
            { 8050112, "Seadragon Onigiri" },
            { 8050113, "Arctic Telescope Fish Sushi" },
            { 8050114, "Alaska Pollock Sushi" },
            { 8050115, "Lumpfish sushi" },
            { 8050116, "Snub-nosed Spiny Eel Sushi" },
            { 8050117, "Xenacanthus Sushi" },
            { 8050119, "Longspine Squirrelfish Sushi" },
            { 8050120, "Clearfin Lionfish Sushi" },
            { 8050121, "Blackfin Barracuda Sushi" },
            { 8050122, "Spear Squid Sushi" },
            { 8050123, "Red-banded Lobster Sushi" },
            { 8050124, "American Lobster Sushi" },
            { 8050125, "Blue Lobster Sushi" },
            { 8050126, "California Spiny Lobster Sushi" },
            { 8050127, "Fan Lobster Sushi" },
            { 8050128, "Norway Lobster Sushi" },
            { 8050129, "Golden King Crab Sushi" },
            { 8050130, "Snow Crab Sushi" },
            { 8050131, "Horsehair Crab Sushi" },
            { 8050132, "European Lobster Sushi" },
            { 8050133, "Tropical Rock Lobster Sushi" },
            { 8050134, "Crystal Lobster Sushi" },
            { 8050135, "Eastern Rock Lobster Sushi" },
            { 8050136, "Dollocaris Ingens Sushi" },
            { 8050137, "Tokummia Katalepsis Sushi" },
            { 8050138, "Fanged Cod Sushi" },
            { 8050139, "Three-Headed Cod Sushi" },
            { 8050140, "Grotesque Mackerel Sushi" },
            { 8050141, "Many Eyed Mackerel Sushi" },
            { 8050142, "Tusked Grouper Sushi" },
            { 8050143, "Voltaic Grouper Sushi" },
            { 8050144, "Barbed Eel Sushi" },
            { 8050145, "Host Eel Sushi" },
            { 8050146, "Bloodskin Shark Sushi" },
            { 8050147, "Sallow Sailfish Sushi" },
            { 8050148, "Cerebral Crab Sushi" },
            { 8050149, "Malignant Pincer Sushi" },
            { 8050150, "Scouring Bass Sushi" },
            { 8050151, "Gnashing Perch Sushi" },
            { 8050152, "Shattered Wreckfish Sushi" },
            { 8050153, "Bony Wreckfish Sushi" },
            { 8050154, "Gelatinous Stonefish Sushi" },
            { 8050155, "Enthralled Stonefish Sushi" },
            { 8050156, "Sprouting Eel Sushi" },
            { 8050157, "Translucent Sturgeon Sushi" },
            { 8050158, "Withered Ray Sushi" },
            { 8050159, "Splintered Crab Sushi" },
            { 8050160, "Cortex Decorator Sushi" },
            { 8050161, "Aurora Jellyfish Sushi" },
            { 8050162, "Parhelion Jellyfish Sushi" },
            { 8050163, "Radiant Squid Sushi" },
            { 8050164, "Seizing Snailfish Sushi" },
            { 8050165, "Perished Loosejaw Sushi" },
            { 8050166, "Bursting Anglerfish Sushi" },
            { 8050167, "Savage Barracuda Sushi" },
            { 8050168, "Concertina Barracuda Sushi" },
            { 8050169, "Gazing Shark Sushi" },
            { 8050170, "Imperious Lobster Sushi" },
            { 8050171, "Entangled Crab Sushi" },

            // ── DLC sushi ────────────────────────────────────────────────────────
            { 8058122, "Purple Sea Urchin Sushi" },
            { 8058123, "Flame Angelfish Sushi" },
            { 8058124, "Emperor Angelfish Sushi" },
            { 8058125, "Gunnel Sushi" },
            { 8058126, "Sheepshead Sushi" },
            { 8058127, "Stingray Sushi" },
            { 8058128, "Marbled Electric Ray Sushi" },
            { 8058129, "Striped Catfish Sushi" },
            { 8058354, "Clione Sushi" },

            // ── Tuna bar sushi ───────────────────────────────────────────────────
            { 8052001, "Bluefin Tuna Akami Sushi" },
            { 8052002, "Bluefin Tuna Chutoro Sushi" },
            { 8052003, "Bluefin Tuna Ootoro Sushi" },
            { 8052004, "Yellowfin Tuna Akami Sushi" },
            { 8052005, "Yellowfin Tuna Chutoro Sushi" },
            { 8052006, "Yellowfin Tuna Ootoro Sushi" },
            { 8052007, "Bluefin Tuna Rice Bowl" },
            { 8052008, "Hawaiian Poke" },
            { 8052009, "Yellowfin Tuna Steak" },
            { 8052011, "Raw Black Tiger Shrimp Sushi" },
            { 8052012, "Cooked Whiteleg Shrimp Sushi" },

            // ── Cooked dishes ────────────────────────────────────────────────────
            { 8051001, "Boiled Yellowback Fusilier" },
            { 8051002, "Seahorse Skewers" },
            { 8051003, "Salt-grilled Redtoothed Triggerfish" },
            { 8051004, "Agar Tokoroten" },
            { 8051005, "Seasoned Kajime" },
            { 8051006, "Smallspotted Dart Kajime Soup" },
            { 8051007, "Stellate Puffer Nicogori" },
            { 8051008, "Moray Eel Curry" },
            { 8051009, "Whole-Roasted Shark Head" },
            { 8051010, "Striped Red Mullet Tangle Roll" },
            { 8051011, "White Trevally Kombu Ochazuke" },
            { 8051012, "Seagrapes Jellyfish Sushi" },
            { 8051013, "Stellate Puffer Special Sushi" },
            { 8051014, "Seagrapes Special Sushi" },
            { 8051015, "Batfish Ricebowl" },
            { 8051016, "Trout Sea Grapes Ricebowl" },
            { 8051017, "Seahorse Udon" },
            { 8051018, "Mackerel Scad Hotdog" },
            { 8051019, "Shark Karaage" },
            { 8051020, "Black Vinegar Braised Parrotfish" },
            { 8051021, "Atlantic Bonito Curry" },
            { 8051022, "Narwhal Miso Soup" },
            { 8051023, "Humphead Parrotfish Curry" },
            { 8051024, "Trevally Nanbanzuke" },
            { 8051025, "Fried Onion Cuttlefish" },
            { 8051026, "Dusky Grouper Steak" },
            { 8051027, "Seahorse Salad" },
            { 8051028, "Great Barracuda Canape" },
            { 8051029, "Tropical Fish Sushi Set" },
            { 8051030, "Vegetable Sushi" },
            { 8051031, "Sweet and Sour Stargazer" },
            { 8051032, "Sea Toad and Cucumber Gunkan Sushi" },
            { 8051033, "Great Spider Crab and Cucumber Sushi" },
            { 8051034, "Nasu Dengaku" },
            { 8051035, "Deep-Fried Eggplant Shrimp Meatballs" },
            { 8051036, "Deep Fish Tempura" },
            { 8051037, "Humboldt Ink Pasta" },
            { 8051038, "Pickled Vegetables" },
            { 8051039, "Deep-Fried Vegetables" },
            { 8051040, "Skewered Cucumber" },
            { 8051041, "Special Fried Shrimp Sushi" },
            { 8051043, "Rice with White Shrimp Meat" },
            { 8051044, "Boiled and Deep-Fried White Shrimp" },
            { 8051045, "Marlin and Soybean Paste Roast" },
            { 8051046, "Boiled Sailfish and Seaweed" },
            { 8051047, "Wrasse Curry" },
            { 8051048, "Great Spider Crab Curry" },
            { 8051049, "Fried Rice with Sally Lightfoot Crab" },
            { 8051050, "Blobfish Spring Roll" },
            { 8051051, "Tomato Egg Soup" },
            { 8051052, "Mianbao Xia" },
            { 8051053, "Fried Tomato and Snailfish" },
            { 8051054, "Plotosid Pie" },
            { 8051055, "Pelican Eel Jelly" },
            { 8051056, "Smoked Atlantic Mackerel Scramble" },
            { 8051057, "Comber Sandwich" },
            { 8051058, "Narrow-barred Spanish Mackerel Arancini" },
            { 8051059, "Antarctic Octopus Carpaccio" },
            { 8051060, "Arctic Cod Risotto" },
            { 8051061, "Peacock Squid Ripieni" },
            { 8051062, "Haddock Acqua Pazza" },
            { 8051063, "Seaweed Rolled Omelet" },
            { 8051064, "Latok Omelet" },
            { 8051065, "Three-Colored Squid Roast" },
            { 8051066, "Dried Stingray" },
            { 8051067, "Dumbo Takoyaki" },
            { 8051068, "Roasted Capelin" },
            { 8051069, "Boiled Porbeagle Shark" },
            { 8051070, "Rice with Purple Sea Urchin Sushi" },
            { 8051071, "Deep-Fried Red Lionfish" },
            { 8051072, "Roasted Tropical Fish and Garlic" },
            { 8051073, "Crimson Fish Roll" },
            { 8051074, "Big-Eyed Scad and Soybean Paste Roast" },
            { 8051075, "Fried Seahorses" },
            { 8051076, "Trevally Sandwich" },
            { 8051077, "Ice Fish Curry" },
            { 8051078, "Rice with Great Spider Crab Meat" },
            { 8051079, "Seasoned Jellyfish" },
            { 8051080, "Falcatus Soybean Paste Soup" },
            { 8051081, "Seasoned Waptia Fieldensis" },
            { 8051082, "Pikaia Ramen" },
            { 8051083, "Pufferfish Dumpling Soup" },
            { 8051084, "Moonlight Bladderwrack Roll" },
            { 8051085, "Stir-fried Habanero Lobster" },
            { 8051086, "Grilled Eel with Habanero" },
            { 8051087, "Fried Habanero Fangtooth" },
            { 8051088, "Hot Pepper Tuna" },
            { 8051089, "Soy Sauce Marinated Crab" },
            { 8051090, "Steamed Eastern Rock Lobster & Egg" },
            { 8051091, "Lobster Platter" },
            { 8051092, "Crystal Lobster Roll" },
            { 8051093, "Seasoned Long-spine Porcupinefish Skin" },
            { 8051094, "Ebirah Chasing Sashimi" },
            { 8051095, "Godzilla vs. Ebirah Curry" },
            { 8051096, "Deep Sea Kaiju Ramen" },
            { 8051097, "Stingray Sashimi Cold Noodles" },
            { 8051098, "Eggplant Soba Oyaki" },
            { 8051099, "Spear Squid Soba Futomaki" },
            { 8051100, "Warm Atlantic Mackerel Soba" },
            { 8051101, "Steamed Wolf Eel" },
            { 8051102, "Clione Queen Soup" },
            { 8051103, "Goblin Shark Belly Roast" },
            { 8051104, "Stir-Fried Hermit Crab and Seaweed" },
            { 8051105, "Boiled Mantis Shrimp with Soy Paste" },
            { 8051106, "White Shark Omelet" },
            { 8051107, "Phantom Jellyfish Jelly" },
            { 8051108, "Roasted Helicoprion Tail" },
            { 8051109, "Steamed Kronosaurus Tongue" },
            { 8051110, "Yawie Steamed Meat" },
            { 8051111, "Blanched Lusca Tentacle" },
            { 8051112, "Lusca Neck Tadaki" },

            // ── Ingredient/special dishes ────────────────────────────────────────
            { 8051201, "Hyalonema Tuna Sashimi" },
            { 8051202, "Steamed Hyalonema Angler Fish" },
            { 8051203, "Boiled Asian Sheepshead Wrasse & Truffle" },
            { 8051204, "Truffle Sailfish Tartare" },
            { 8051205, "Grilled Antarctic Octopus & Truffle" },
            { 8051206, "Truffle Blue Lobster Tail Sushi" },
            { 8051207, "Truffle Shark Sandwich" },
            { 8051208, "Rainbow Cap Triggerfish Fishcake" },
            { 8051209, "Rainbow Cap Pacific Fanfish Ochazuke" },
            { 8051210, "Rainbow Cap Eel Skewers" },
            { 8051211, "Chirashi Sushi" },
            { 8051212, "Cold Jellyfish & Green Sea Urchin Salad" },
            { 8051213, "Deep-Fried Sea Urchin" },
            { 8051214, "Green Sea Urchin & Cucumber Salad" },

            // ── DLC / event dishes ───────────────────────────────────────────────
            { 8059031, "Fish Dim Sum" },
            { 8059036, "Fish and Chips" },
            { 8059037, "Carbonara" },
            { 8050025, "Humphead Parrotfish Sushi" },
        };

        public static string? GetDisplayName(int recipeId) =>
            _map.TryGetValue(recipeId, out var name) ? name : null;

        // Reverse lookup: name → TID (built lazily from _map)
        private static System.Collections.Generic.Dictionary<string, int>? _reverseMap;
        private static System.Collections.Generic.Dictionary<string, int> ReverseMap
        {
            get
            {
                if (_reverseMap == null)
                {
                    _reverseMap = new System.Collections.Generic.Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);
                    foreach (var kvp in _map)
                    {
                        // Use lowercase name as key; skip duplicates (keep first)
                        if (!_reverseMap.ContainsKey(kvp.Value))
                            _reverseMap[kvp.Value] = kvp.Key;
                    }
                }
                return _reverseMap;
            }
        }

        /// <summary>Returns the recipe TID for the given display name, or -1 if not found.</summary>
        public static int GetTIDFromName(string recipeName) =>
            ReverseMap.TryGetValue(recipeName, out var tid) ? tid : -1;
    }
}
