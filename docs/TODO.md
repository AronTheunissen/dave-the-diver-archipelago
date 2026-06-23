# Dave the Diver Archipelago - TODO List

> Last updated: June 23, 2026 (evening)
> Current status: APWorld complete with full logic rules, 1,400+ locations, 320+ items. C# client complete. Awaiting TID mapping and in-game testing.

---

## ✅ COMPLETED

### APWorld (Python)
- [x] Core world class (`__init__.py`) with item pool, location placement, slot data
- [x] 1,400+ locations across 23 regions (incl. all DLC)
- [x] 320+ items (fish, weapons, recipes, charms, ingredients, progressive equipment, DLC items)
- [x] 5 victory conditions (Defeat Yawie, All Bosses, Diamond Rank, Master Diver, 100%)
- [x] Comprehensive logic rules — boss gates, item chains, quest prerequisites (see `docs/LOGIC_NOTES.md`)
- [x] All base game fish species (203) placed in correct depth zones with night dive rules
- [x] All weapon trees (79 variants across 9 weapons)
- [x] All dish upgrades (549 checks) with correct max levels + recipe unlock prerequisite rules
- [x] Cooksta rank system (5 ranks, 12 requirement checks, gated by A Scolding from Yoshie)
- [x] Ecowatcher missions (44+ real checks with depth/region gates)
- [x] Ingredient first-find checks + filler items
- [x] Charm system (12 charms from missions + Ecowatcher)
- [x] Staff system (24 named staff, hire + training, configurable depth)
- [x] Sub-missions (29 checks, toggleable, with quest chains)
- [x] Cooking competition chain (4 fights with ingredient gates)
- [x] VIP quest locations with ingredient access gates
- [x] Photography system (20 real photo spots + 8 murals, all gated on Underwater Camera)
- [x] `should_include_item()` and `should_include_location()` filtering
- [x] `fill_slot_data()` passing all 28 options to client
- [x] 7 chapters with correct story structure
- [x] Unit tests (54/54 passing)

### DLC Content
- [x] **DREDGE DLC** — Aberration vortex fish (34), Drain Gun tree, Leo Keychain, tagged `dlc_dredge`
- [x] **Godzilla DLC** — 2 recipes + 20 Kaiju figurine checks (all regions, gated by Ebirah), tagged `dlc_godzilla`
- [x] **Ichiban DLC** — 4 recipe unlocks + dish upgrades, Buckwheat crop, Beat 'Em Up, Karaoke, 3 staff (Hamako/Etsuko/Chitose), Torben boss, 2 missions (Operation Sea Blue Eradication + Cold Noodles), gated by Chapter 5 + Cocktails Unlocked, tagged `dlc_ichiban`
- [x] **Jungle DLC structure** — 8 regions, 30+ items, 100+ location checks (see below for TODOs), tagged `dlc_jungle`

### Jungle DLC (Structure Complete — Data TODOs Remain)
- [x] 8 new regions: Utara Village, Bancho Grill, Utara Lake Upper/Lower, Lakebed Sea, Setah Forest, Murau Temple, Surga Falls
- [x] 7 chapter + epilogue story checks
- [x] 6 boss defeat checks (Caiman, Snapping Turtle, Sulong, Stethacanthus, Xiphactinus, Basilosaurus)
- [x] 9 staff unlock checks (Yasuto, Martin Tweed, Rover, Om Nom, Charlie Bonnet III, William Longbottom, Mita, Udo, Sato)
- [x] 28 villager friendship reward checks (14 confirmed villagers × 2 tiers)
- [x] 8 minigame checks (beetle battles, hide & seek, shooting range, duck hunting, rope puzzle, land fishing)
- [x] 5 Insectagram checks
- [x] 20 fish first-catch placeholders (known species)
- [x] 10 jungle ingredient first-find checks
- [x] 5 Bancho Grill restaurant milestone checks
- [x] 9 exploration milestone checks
- [x] Progressive Purification Filter (3 tiers), Machete, Bug Net, Fishing Rod, Villager Trust, Jungle Gun forms
- [x] Full logic rules (region gating, tool requirements, boss sequence)

### C# Client Mod
- [x] Plugin entry point (BepInEx 6 IL2CPP)
- [x] Archipelago connection with auto-reconnect
- [x] Item queue (thread-safe, boat-only delivery)
- [x] Death Link support
- [x] 3-tab in-game UI (F9): Connection · Hints · Progress
- [x] Goal tracker for all 5 victory conditions
- [x] Hint system (request by item or location name)
- [x] Progress tracker with item tracker (Equipment/Key Items/Charms/Weapons tabs) + live category breakdown
- [x] Toast notifications (item received, death, connection, goal)
- [x] BepInEx config file support
- [x] Save/restore session state (full persistent state for all item types)
- [x] SlotData parsing (all 25 options)
- [x] 17 Harmony patches — **all real class names confirmed via dump.cs** ✅
- [x] **ItemHandler — all game API calls implemented** (PhoneAppUpgradeManager, MissionManager, SaveData, ChapterManager, etc.)
- [x] SaveLoadPatch — reapplies all items on save load via first boat entry
- [x] LocationTracker for all location categories

### Reverse Engineering
- [x] Generated dump.cs via Il2CppDumper on game machine
- [x] Confirmed all 17 patch class/method names from dump.cs
- [x] Updated `docs/CLASS_NAME_CHEAT_SHEET.md` with all confirmed names
- [x] Identified key save data classes: `SaveData`, `SNSInfoSave`, `ChapterManager`, `MissionManager`

### Documentation
- [x] `docs/SETUP_GUIDE.md` — player setup guide
- [x] `docs/MODDING_NOTES.md` — reverse engineering guide
- [x] `docs/DESIGN.md` — design decisions
- [x] `docs/CLASS_NAME_CHEAT_SHEET.md` — all confirmed class names from dump.cs

---

## 🔴 CRITICAL — Blocking Actual Play

### Fill in TID Mapper Dictionaries
All patches have the correct classes and methods, but the `*NameMapper` dictionaries
that map game design-sheet TID integers → AP location names are still empty.

**How to get TIDs:** Install **UnityExplorer** as a BepInEx plugin, run the game,
and use the inspector to find TID values on live objects. Or search `dump.cs` for
static const values near the relevant classes.

| Mapper | Entries needed | File |
|---|---|---|
| `FishNameMapper._map` | ~200 entries (GameObject name → fish name) | `FishCatchPatch.cs` ✅ Already populated! |
| `BossNameMapper._map` | 16 entries (scene name substring → boss name) | `BossDefeatedPatch.cs` ✅ Already populated! |
| `WeaponNameMapper._idMap` | ~79 entries (craft TID → weapon name) | `WeaponCraftPatch.cs` |
| `RecipeNameMapper._map` | ~54 entries (recipe TID → recipe name) | `RecipeUnlockPatch.cs` |
| `QuestNameMapper._map` | ~20 entries (mission TID → quest name) | `StoryProgressPatch.cs` |
| `ChallengeNameMapper._map` | 9 entries (mission TID → challenge name) | `ChallengePatch.cs` |
| `IngredientNameMapper._map` | ~12 entries (ingredient TID → name) | `IngredientPatch.cs` |
| `CharmMapper._map` | 8 entries (charm TID → name + source) | `CharmPatch.cs` |

### ~~Implement ItemHandler Game API Calls~~ ✅ DONE
All `ItemHandler.cs` methods now use real game API calls:
- `PhoneAppUpgradeManager.ApplyUpgrade()` — progressive equipment (oxygen/harpoon/suit/cargo)
- `MissionManager.UpdateMission()` — key items (gloves, translator, teleports, farms, etc.)
- `DREventTriggerManager.WeaponCraftTreeEventTrigger()` — weapon unlocks
- `SaveData.AddUnlockRecipeSaveData()` / `UpdateUnlockRecipeSave()` — recipes/dish upgrades
- `SNSInfoSave.set_grade()` — Cooksta rank
- `ChapterManager.SetChapterComplete()` — chapter completion
- `LobbyCharmSwapPanel.AutoEquipCharmItem()` — charms
- `IngredientsStorage.AddIngredients()` — ingredients + counter key items
- `PlayerInfoSave.set_Gold()` / `set_bei()` — currency
- `ReapplyAllItems()` fires on first boat entry after every save load

### In-Game Testing
- [ ] Build mod on game machine (`dotnet build` in `client/DaveDiverAP/`)
- [ ] Install and connect to a test Archipelago server
- [ ] Verify fish catches trigger correctly
- [ ] Verify boss defeats trigger correctly
- [ ] Verify boat-only item delivery works
- [ ] Verify goal completion fires correctly

---

## 🟡 IMPORTANT — Data To Fill In (Requires Playing)

### Godzilla DLC
- [ ] Confirm exact Kaiju figurine locations per region (currently estimated) — update `locations.py`

### Jungle DLC — Fish & Recipes (~60 fish + all recipes still needed)
Fill these in by pasting wiki data (same process as base game fish list):
- [ ] Full fish list for Utara Lake Upper (IDs `_J+210` to `_J+249` reserved)
- [ ] Full fish list for Utara Lake Lower (IDs `_J+258` to `_J+299` reserved)
- [ ] Full fish list for Lakebed Sea (IDs `_J+302` to `_J+349` reserved)
- [ ] Rod-fished species at Surga Falls / Setah Forest (IDs `_J+350` to `_J+399` reserved)
- [ ] All Bancho Grill recipes (fish-catch unlocks + Artisan Flame research + VIP + rank)
- [ ] Remaining ~18 villagers (IDs `_J+88` to `_J+123` reserved)
- [ ] Dish upgrade checks for all Jungle recipes (same pattern as base game)

### Base Game
- [x] Ichiban DLC recipe unlock conditions confirmed — staff training recipes ✅

---

## 🟡 IMPORTANT — Quality & Completeness

### Unit Tests (55/55 passing ✅)
- [x] ID uniqueness, no duplicate IDs, no item/location collisions
- [x] `should_include_item()` filtering — all categories, DLC flags, traps
- [x] `should_include_location()` filtering — fish 3-way, all toggles, DLC
- [x] All location regions valid (in REGION_NAMES)
- [x] `fill_slot_data()` key coverage and value types
- [ ] Test region access rules (needs Archipelago State mock)
- [ ] Test victory conditions end-to-end (needs full world generation)

---

## 🟢 NICE TO HAVE — Polish

### Connection UI Improvements
- [ ] Auto-connect on game launch (config option exists, needs UI toggle)
- [ ] Better error messages for common connection failures
- [ ] Server browser / recent servers list

### Spoiler Log Viewer
- [ ] Add 4th tab to UI showing where your items are in the multiworld

### Archipelago Submission
- [ ] Review Archipelago submission guidelines
- [ ] Add `apworld/davethediver/data/` folder with any required data files
- [ ] Register on Archipelago website
