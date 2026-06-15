# Dave the Diver Archipelago - TODO List

> Last updated: June 15, 2026
> Current status: APWorld complete, C# client skeleton complete, awaiting game internals to wire up patches

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
- [x] 17 Harmony patch skeletons (all categories covered)
- [x] ItemHandler stubs for all 276 items
- [x] LocationTracker stubs for all 1,134 locations

### Documentation
- [x] `docs/SETUP_GUIDE.md` — player setup guide
- [x] `docs/MODDING_NOTES.md` — reverse engineering guide
- [x] `docs/DESIGN.md` — design decisions

---

## 🔴 CRITICAL — Blocking Actual Play

### Fill in Harmony Patch Class Names
The single biggest blocker. All 17 patches use PLACEHOLDER class names.
**Requires:** `Assembly-CSharp.dll` from BepInEx/interop/ OR Il2CppDumper output.
**See:** `docs/CLASS_NAME_CHEAT_SHEET.md` for exactly what to search for.

| Patch | Placeholder Class | Priority |
|---|---|---|
| `FishCatchPatch.cs` | `FishInteraction` | 🔴 High |
| `BossDefeatedPatch.cs` | `BossManager` | 🔴 High |
| `StoryProgressPatch.cs` | `MissionManager` | 🔴 High |
| `RecipeUnlockPatch.cs` | `RecipeManager` | 🔴 High |
| `WeaponCraftPatch.cs` | `WeaponShopManager` | 🔴 High |
| `GameStatePatch.cs` | `BoatSceneManager` etc. | 🔴 High |
| `PlayerDeathPatch.cs` | `OxygenSystem`, `PlayerCharacter` | 🔴 High |
| `CookstaPatch.cs` | `CookstaManager` | 🟡 Medium |
| `EcowatcherPatch.cs` | `EcowatcherManager` | 🟡 Medium |
| `RestaurantPatch.cs` | `SushiBarManager` | 🟡 Medium |
| `FarmPatch.cs` | `VegetableFarmManager` etc. | 🟡 Medium |
| `ChallengePatch.cs` | `ChallengeManager` | 🟡 Medium |
| `PhotographyPatch.cs` | `PhotographyManager` | 🟡 Medium |
| `CollectiblePatch.cs` | `TreasureChest`, `TeleportPoint` etc. | 🟡 Medium |
| `IngredientPatch.cs` | `IngredientObject`, `VendorManager` | 🟢 Low |
| `MinigamePatch.cs` | `SeahorseRaceManager` etc. | 🟢 Low |
| `CharmPatch.cs` | `CharmManager` | 🟢 Low |

### Fill in ID Mapper Dictionaries
Each patch has a `*NameMapper` dictionary that maps game internal IDs → AP location names.
All are empty (commented-out examples only). Needs actual game IDs from Il2CppDumper.

- [ ] FishNameMapper (~200 entries)
- [ ] BossNameMapper (16 entries)
- [ ] WeaponNameMapper (~80 entries)
- [ ] RecipeNameMapper (~100 entries)
- [ ] QuestNameMapper (~20 entries)
- [ ] CharmMapper (8 entries)
- [ ] ChallengeNameMapper (9 entries)
- [ ] VIPNameMapper (6 entries)

### Implement ItemHandler Game API Calls
All `ItemHandler.cs` methods are stubs. Need real SaveSystem API calls to actually give items to the player.

- [ ] `GiveWeapon()` — add weapon to Duff's shop / inventory
- [ ] `UnlockRecipe()` — unlock recipe in restaurant
- [ ] `UpgradeDish()` — apply dish research level
- [ ] `GiveIngredient()` — add ingredient to inventory
- [ ] `UpgradeDivingSuit()` / `UpgradeOxygenTank()` / `UpgradeHarpoon()` — iDiver upgrades
- [ ] `UnlockRegion()` — unlock area (teleport/access)
- [ ] `UpgradeCookstaRank()` — apply Cooksta rank
- [ ] `GiveCharm()` — equip/unlock charm

### Fix GoalTracker Bug
- [ ] `GoalTracker.cs` line ~114: `_allMarincaComplete` variable declared but never initialized (should be `= false`)

---

## 🟡 IMPORTANT — Quality & Completeness

### Unit Tests
- [ ] Create `apworld/tests/` directory
- [ ] Test item pool generation for each option combination
- [ ] Test `should_include_item()` filtering
- [ ] Test `should_include_location()` filtering
- [ ] Test region access rules (can reach each region with correct items)
- [ ] Test victory conditions
- [ ] Test ID uniqueness (no duplicate item/location IDs)
- [ ] Test all location regions are valid

### In the Jungle DLC Content (Available June 18, 2026)
- [ ] New fish species (freshwater lake ecosystem)
- [ ] New locations (Bancho Grill, Utara Village, jungle lake)
- [ ] New items (jungle ingredients, new recipes)
- [ ] New regions (Jungle Lake, Bancho Grill, Utara Village)
- [ ] New goals or goal extensions
- [ ] Tag all new content with `dlc_jungle` category

### Connection UI Improvements
- [ ] Auto-connect on game launch (config option already exists, UI toggle needed)
- [ ] Better error messages for common connection failures
- [ ] Server browser / recent servers list

---

## 🟢 NICE TO HAVE — Polish

### Spoiler Log Viewer
- [ ] Add 4th tab to UI showing where your items are in the multiworld
- [ ] Filter by item category

### README Update
- [ ] Update README.md to reflect current project state (it still shows the old skeleton structure)
- [ ] Add screenshots of the in-game UI

### Archipelago Submission
- [ ] Review Archipelago submission guidelines
- [ ] Add `apworld/davethediver/data/` folder with any required data files
- [ ] Register on Archipelago website

---

## 📋 How to Get the Class Names (Tonight's Task)

**Option A — BepInEx interop DLL (easiest):**
1. Install BepInEx 6 on game machine, run game once
2. Find `BepInEx/interop/Assembly-CSharp.dll`
3. Open in dnSpy or ILSpy and search for class names
4. See `docs/CLASS_NAME_CHEAT_SHEET.md` for what to search

**Option B — Il2CppDumper:**
1. Download Il2CppDumper from GitHub
2. Run against `GameAssembly.dll` + `il2cpp_data/Metadata/global-metadata.dat`
3. Share `dump.cs` — I can find all names from that file

**Option C — Existing mods:**
- Check https://github.com/WhiteMinds/dave-diver-expansion source code
- Check https://github.com/devopsdinosaur/dave-the-diver-mods source code
- Check https://github.com/Arutsuyo/SuperDave2.0 source code
- These may already reveal some real class names!
