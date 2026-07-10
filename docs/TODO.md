# Dave the Diver Archipelago — Master TODO

> Last updated: 2026-07-07
> Goal: A fully working, submittable Archipelago randomizer for Dave the Diver (all DLC).

This file lists **only what remains to be done**. Completed work is not tracked here.
See session notes and git log for history.

---

## 🔴 CRITICAL — Blocking Actual Play

These must be fixed before anyone can run a real game.

### C# Client Patches

- [ ] **Save-load crash on "Continue"** — The mod may crash when loading a save from the main menu. Believed to be fixed by `IsGameReady` guards added in Session 3, but needs in-game verification. If still crashing, bisect by disabling patches one by one (see `SESSION_NOTES_2026-06-29.md`).

- [ ] **CommonBossDead.DoJob NullRef loop** — Boss defeat fires a NullRef repeatedly after the cutscene. Current void-prefix guard stops the loop but is a workaround. Real fix: patch `BossSceneSO.JobStuff` base class, or find a cleaner hook point. Not game-breaking but noisy.

- [ ] **RecipeUnlockPatch.UpgradeDish_Postfix** — Currently commented out. `LocationTracker.OnDishResearchUpdated` is not yet implemented. This means dish upgrade checks are never sent to the AP server during play — a major gap.

- [ ] **SaveLoadPatch** — Disabled because `SaveSystem` is not in the interop DLL. Need to either find it in another DLL or re-implement the hook via a different class. Required for reapplying items on game load.

- [ ] **MinigamePatch** — Disabled (seahorse racing, card games — interop class missing). Minigame location checks will silently never fire.

- [ ] **EcowatcherPatch** — Disabled (`EcoWatcherDeliverPopup` not found in interop). Ecowatcher location checks will silently never fire.

- [ ] **CharmPatch TID verification** — Charm TIDs need to be verified in-game to confirm they are correct.

- [ ] **staff_training_locations not in location_table** — `staff_training_locations` dict is defined but never added to `location_table` in `locations.py`. Either add it or confirm it's intentionally excluded (superseded by `staff_all_levels_locations`).

---

## 🟠 HIGH PRIORITY — Required for Completeness

These are needed for the randomizer to cover all game content correctly.

### Dish Upgrade Checks (Major Gap)

- [x] **Add sushi dishes to dish_upgrade_locations** — Done (2026-07-10). All base game sushi (8050xxx, ~110 dishes) and Tuna Bar sushi (8052xxx, 8 dishes) added to `dish_upgrade_locations` in `_D2` block (BASE_ID+12000). Corresponding `Progressive [Name]` items added to `dish_upgrade_items` in `items.py`.

- [x] **Update existing dish_upgrade_locations max levels** — Done (2026-07-10). All non-boss cooked dishes updated to max level 10 per spreadsheet. Also fixed `items.py` `dish_upgrade_items` counts to match.

- [x] **Special Fried Shrimp Sushi and Vegetable Sushi** — Updated to max level 10 per spreadsheet (2026-07-10). Both in `dish_upgrade_locations` and `dish_upgrade_items`.

### Jungle Grill Recipes (Major Gap)

- [ ] **TID dump for all grill recipes** — Run `tools/unity_explorer_dump_grill.cs` in UnityExplorer while in-game. It uses `GrillRecipeDataDic` and saves to `grill_dump.txt` + clipboard. Paste output into chat. We need TIDs for ~75 simple "Grilled X" recipes not yet in `jungle_restaurant_locations`. These are upgradeable and need to be added as locations.

- [ ] **Verify recipe name mismatches** — Several complex grill recipes in the code may have wrong names. Check these pairs in the in-game recipe research screen:
  - "Gourami Fried" vs "Sweet and Sour Gourami"
  - "Largemouth Bass Boiled" vs "Spicy Largemouth Bass Stew"
  - "Banana Halo-Halo" vs "Fruit Halo-Halo"
  - "Banana Blossom Salad" vs "Banana Flower Salad"
  - "Mud Carp Grilled in Banana Leaf" vs "Grilled Mud Carp with Herbs"
  - "Piraiba Catfish Tamarind Soup" vs "Piraiba Catfish Soup"
  - "Electric Eel Sliced" vs "Smoked Eel Slices"
  - "Crayfish Sambal Stir-fried" vs "Red Swamp Crayfish Sambal Stir-Fry"
  - "Crayfish Lemongrass Steamed" vs "Steamed Freshwater Crayfish with Lemongrass"
  - "Ciurcopterus Stir-fried" vs "Braised Ciurcopterus"
  - "Ammonite Salad" vs "Ammonite Water Spinach Salad"
  - "Clown Featherback Taro Fried" vs "Clown Featherback Taro Croquette"
  - "Tambaqui Grilled" vs "Tambaqui Steak"
  - "Tangsuyuk" vs "Sweet and Sour Pork"
  - "King Trumpet Mushroom Stir-fried" vs "Stir-Fried Spicy King Oyster Mushrooms"
  - "Xiphactinus Spicy Soup" vs "Xiphactinus Tamarind Soup" (boss recipe)
  - "Stethacanthus Coconut Stew" vs "Stethacanthus Fin Soup" (boss recipe)
  - "Ophtalmosaurus Grilled" vs "Ophthalmosaurus Whole Roasted Head" (boss recipe)

- [ ] **Add missing complex grill recipes** — After TID dump, add: Triple Fried Bananas, Steamed Tricolor Discus, Herb-Stuffed Sarcastic Fringehead Roast, Aquilolamna Stew, and all confirmed "Grilled X" simple recipes.

- [ ] **Jungle grill recipe max upgrade levels** — Check the in-game research screen for a sample of jungle grill recipes to confirm whether they all cap at 10, or vary. Fill in the Max Level column in `docs/SUSHI_UPGRADE_LEVELS.md` → Jungle section.

### Quest / Mission TIDs (Client-Side)

- [ ] **QuestNameMapper — fill in mission TIDs** — `QuestNameMapper` in the client has all quest entries as commented placeholders. Mission TIDs need to be recorded in-game via the `StoryProgressPatch` auto-logging (check BepInEx log after triggering each quest/story beat). A few are confirmed (see `SESSION_NOTES_2026-07-03.md`); most are not.
  - Particularly needed: all sub-mission TIDs, cooking competition TIDs, chapter completion TIDs

- [ ] **Verify mission TID 10010002** — Listed in session notes as "NOT cleared — prologue skip TID?" — check in-game what this refers to.

### Logic Gaps

- [ ] **Salvage Drone source quest** — Currently ungated in logic. Find the exact quest/trigger that gives the player the Salvage Drone and add as a gate in `rules.py`.

- [ ] **Sub-missions with unknown triggers** — These locations exist in code but have no prerequisite rules confirmed:
  - `Sub-Mission: Assisting Ellie` — what triggers Ellie's mission?
  - `Sub-Mission: Reticent Girl` — unknown trigger
  - `Sub-Mission: Sea Person at the Workshop` — any village prerequisites?
  - `Sub-Mission: Wedding Song Record` — any prerequisites?
  - `Sub-Mission: Find the Children's Ball` — any prerequisites?

- [ ] **Dr. Bacon jungle research checks** — Structure not yet implemented. What are the research milestones and their triggers in the jungle DLC?

- [ ] **Villager friendship details** — Structure is in place (14 villagers × 2 tiers) but exact friendship point thresholds and reward conditions need verification from gameplay.

---

## 🟡 IMPORTANT — In-Game Testing

Nothing below is blocked on code changes — it just requires playing the game with the mod active.

### Build & Connect
- [ ] Build mod on game machine: `dotnet build` in `client/DaveDiverAP/`
- [ ] Connect to a test Archipelago server and start a new game

### Core Functionality
- [ ] Fish catches trigger location checks correctly
- [ ] Boss defeats trigger location checks correctly
- [ ] Recipe unlocks trigger location checks correctly
- [ ] Dish upgrade checks fire correctly (requires `UpgradeDish_Postfix` fix above first)
- [ ] Items received from AP are applied correctly (ingredients, recipes, weapons, equipment)
- [ ] Boat-only item delivery works (items queue and deliver on first boat arrival)
- [ ] Save/load "Continue" does not crash (critical — see above)
- [ ] Items are correctly reapplied after loading a save
- [ ] Goal completion fires for all 5 victory conditions

### UI & Polish
- [ ] Connection UI works correctly (connect/disconnect, status display)
- [ ] Progress UI shows correct counts for all categories
- [ ] Hint system returns correct item/location information
- [ ] Toast notifications appear for item received, death link, goal complete
- [ ] Death Link sends and receives correctly

---

## 🟢 NICE TO HAVE — Polish & Submission

These are not blocking play but are needed before submitting to Archipelago.

### APWorld Polish
- [ ] **Archipelago submission review** — Review the official Archipelago game submission guidelines and checklist
- [ ] **`apworld/davethediver/data/` folder** — Check if any required data files (item/location tables, etc.) are needed by the AP framework
- [ ] **Logic unit tests** — Add tests for region access rules and victory conditions (requires Archipelago State mock)
- [ ] **End-to-end world generation test** — Generate a full world and verify all locations are reachable

### Client Polish
- [ ] **Auto-connect on game launch** — Config option exists, needs UI toggle to expose it
- [ ] **Better connection error messages** — More descriptive errors for common failures (wrong port, server down, etc.)
- [ ] **Spoiler log tab** — 4th tab in the F9 UI showing where your items are in the multiworld
- [ ] **Recent servers list** — Remember last-used server/port/slot in connection UI

### Documentation
- [ ] **YAML_GUIDE.md** — Verify all options and their effects are documented accurately
- [ ] **SETUP_GUIDE.md** — Update with final mod installation steps once build process is finalized
- [ ] **Register on Archipelago website** — Once submission is accepted

---

## 📋 Reference: Known Correct Data

Quick reference for things already confirmed and implemented:

| Thing | Status |
|-------|--------|
| Base game sushi max level | All 10 (confirmed via spreadsheet) |
| Tuna Bar sushi max level | All 10 (confirmed via spreadsheet) |
| Truffle VIP dish max level | All 5 (confirmed via spreadsheet) |
| Boss recipe max level | All 1 (no upgrades) |
| Cooked dish max levels in code | Mostly wrong (should be 10) — see SUSHI_UPGRADE_LEVELS.md |
| Jungle grill recipe max levels | Unknown — needs in-game research |
| All fish TIDs | Confirmed via FishCatchPatch._fishIdMap |
| All weapon TIDs | Confirmed via weapon_locations |
| Quest TIDs | Partially confirmed — see SESSION_NOTES_2026-07-03.md |
| Jungle insect TIDs | Confirmed: 40001–40038 |
| Jungle skewer recipe TIDs | Confirmed: 48150001–48150109 |
| Jungle complex grill recipe TIDs | Confirmed: 8054101–8054308 |
| Simple "Grilled X" recipe TIDs | ❌ Not yet recorded — needs UnityExplorer dump |
