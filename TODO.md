# Dave the Diver Archipelago - TODO List

> Last updated: June 15, 2026
> Current status: APWorld complete, C# client complete with real class names, awaiting TID mapping and in-game testing

---

## ✅ COMPLETED

### APWorld (Python)
- [x] Core world class (`__init__.py`) with item pool, location placement, slot data
- [x] 1,134 locations across 15 regions
- [x] 276 items (fish, weapons, recipes, charms, ingredients, progressive equipment, etc.)
- [x] 5 victory conditions (Defeat Yawie, All Bosses, Diamond Rank, Master Diver, 100%)
- [x] Full logic rules with game-accurate region gating
- [x] All fish species (203) placed in correct depth zones
- [x] All weapon trees (79 variants across 9 weapons)
- [x] All dish upgrades (549 checks) with correct max levels
- [x] Cooksta rank system (5 ranks, 12 requirement checks)
- [x] Ecowatcher missions (44 real checks from wiki data)
- [x] Ingredient first-find checks + filler items
- [x] Charm system (12 charms from missions + Ecowatcher)
- [x] DLC support: DREDGE, Godzilla, Ichiban, Jungle toggles
- [x] `should_include_item()` and `should_include_location()` filtering
- [x] `fill_slot_data()` passing all 25 options to client
- [x] 7 chapters with correct story structure
- [x] Unit tests (55/55 passing)

### C# Client Mod
- [x] Plugin entry point (BepInEx 6 IL2CPP)
- [x] Archipelago connection with auto-reconnect
- [x] Item queue (thread-safe, boat-only delivery)
- [x] Death Link support
- [x] 3-tab in-game UI (F9): Connection · Hints · Progress
- [x] Goal tracker for all 5 victory conditions
- [x] Hint system (request by item or location name)
- [x] Progress tracker with category breakdown
- [x] Toast notifications (item received, death, connection, goal)
- [x] BepInEx config file support
- [x] Save/restore session state
- [x] SlotData parsing (all 25 options)
- [x] 17 Harmony patches — **all real class names confirmed via dump.cs** ✅
- [x] ItemHandler stubs for all 276 items
- [x] LocationTracker stubs for all 1,134 locations

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

### Implement ItemHandler Game API Calls
All `ItemHandler.cs` methods are stubs. Need real SaveSystem API calls to actually give items to the player.
**Class names are now confirmed from dump.cs — just need to call the right methods.**

- [ ] `GiveWeapon()` — add weapon to Duff's shop / inventory via `SaveData`
- [ ] `UnlockRecipe()` — unlock recipe via `SaveData.AddUnlockRecipeSaveData()`
- [ ] `UpgradeDish()` — apply dish research level via `SaveData.UpdateUnlockRecipeSave()`
- [ ] `GiveIngredient()` — add ingredient via `SaveData.AddIngredientsSaveData()`
- [ ] `UpgradeDivingSuit()` / `UpgradeOxygenTank()` / `UpgradeHarpoon()` — iDiver upgrades
- [ ] `UnlockRegion()` — unlock area (teleport/access) via `SaveData`
- [ ] `UpgradeCookstaRank()` — apply Cooksta rank via `SNSInfoSave`
- [ ] `GiveCharm()` — equip/unlock charm via `LobbyCharmSwapPanel.AutoEquipCharmItem()`

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

### In the Jungle DLC Content (Available June 18, 2026 — Thursday!)
- [ ] New fish species (freshwater lake ecosystem)
- [ ] New locations (Bancho Grill, Utara Village, jungle lake, ancient temples)
- [ ] New items (jungle ingredients, new recipes)
- [ ] New regions (Jungle Lake, Bancho Grill, Utara Village, Ancient Temple)
- [ ] New goals or goal extensions
- [ ] Tag all new content with `dlc_jungle` category

### In-Game Testing
- [ ] Build mod on game machine (`dotnet build` in `client/DaveDiverAP/`)
- [ ] Install and connect to a test Archipelago server
- [ ] Verify fish catches trigger correctly
- [ ] Verify boss defeats trigger correctly
- [ ] Verify boat-only item delivery works
- [ ] Verify goal completion fires correctly

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
