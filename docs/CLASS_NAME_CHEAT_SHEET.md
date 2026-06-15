# Class Name Cheat Sheet

> This document tells you exactly what to search for when you have access to
> `BepInEx/interop/Assembly-CSharp.dll` (open in dnSpy/ILSpy) or `dump.cs`
> (from Il2CppDumper). For each patch, it lists what we currently use as a
> placeholder and what real thing to search for.

---

## How to Search

### In dnSpy / ILSpy
- Open `BepInEx/interop/Assembly-CSharp.dll`
- Use **Edit → Search** (Ctrl+F or Ctrl+Shift+F)
- Search for the keywords listed below

### In dump.cs (text file)
- Open in any text editor with search (VS Code recommended)
- Use Ctrl+F to search for the keywords listed below

### Using ilspycmd (command line on game machine)
```bash
# List all types containing a keyword
ilspycmd -l type "BepInEx/interop/Assembly-CSharp.dll" | grep -i "fish"

# Show all methods of a specific class
ilspycmd -t FishInteraction "BepInEx/interop/Assembly-CSharp.dll"
```

---

## Priority 1 — Core Gameplay Patches 🔴

### FishCatchPatch.cs
**What it does:** Detects when a fish is caught for the first time.

**Search for:** `SuccessInteract`, `OnFishCaught`, `FishInteract`, `CatchFish`
- Look for a class that has a `SuccessInteract()` method AND references a fish ID or fish name
- The WhiteMinds mod uses `SuccessInteract` for auto-pickup — search their source for which class they patch

**What we need:**
```
Class name:  ??? (currently: FishInteraction)
Method name: ??? (currently: SuccessInteract)
Fish ID field: ??? (currently: fishId)
Fish name field: ??? (currently: fishName)
```

**Patch file:** `client/DaveDiverAP/Patches/FishCatchPatch.cs`

---

### BossDefeatedPatch.cs
**What it does:** Detects when a boss is defeated.

**Search for:** `BossDefeat`, `OnBossKill`, `BossDead`, `BossResult`, `BossManager`, `OnBossEnd`
- Look for a class/method called after a boss fight ends in victory
- Boss names to search for: `Yawie`, `Helicoprion`, `Kronosaurus`, `GoblinShark`, `Klaus`

**What we need:**
```
Class name:  ??? (currently: BossManager)
Method name: ??? (currently: OnBossDefeated)
Boss ID field: ??? (currently: bossId)
```

**Patch file:** `client/DaveDiverAP/Patches/BossDefeatedPatch.cs`

---

### StoryProgressPatch.cs
**What it does:** Detects chapter completions and quest/mission completions.

**Search for:** `MissionComplete`, `ChapterClear`, `QuestComplete`, `OnMissionEnd`, `MissionManager`, `QuestManager`
- Look for something that fires when a chapter ends or a side quest finishes
- Also search for: `Chapter1`, `Chapter2`, story flag names

**What we need:**
```
Class name:  ??? (currently: MissionManager)
Method: chapter complete ??? (currently: OnChapterComplete)
Method: mission complete ??? (currently: OnMissionComplete)
Chapter ID field: ??? (currently: chapterId)
Mission ID field: ??? (currently: missionId)
```

**Patch file:** `client/DaveDiverAP/Patches/StoryProgressPatch.cs`

---

### RecipeUnlockPatch.cs
**What it does:** Detects when a recipe is unlocked OR a dish is researched to a new level.

**Search for:** `RecipeUnlock`, `UnlockRecipe`, `AddRecipe`, `ResearchDish`, `DishUpgrade`, `RecipeManager`, `CookingManager`
- Also search for: `ArtisanFlame` (the currency used for research)

**What we need:**
```
Class name:  ??? (currently: RecipeManager)
Method: unlock ??? (currently: UnlockRecipe)
Method: upgrade ??? (currently: UpgradeDish)
Recipe ID field: ??? (currently: recipeId)
Dish level field: ??? (currently: dishLevel)
```

**Patch file:** `client/DaveDiverAP/Patches/RecipeUnlockPatch.cs`

---

### WeaponCraftPatch.cs
**What it does:** Detects when a weapon is crafted at Duff's shop.

**Search for:** `WeaponCraft`, `CraftWeapon`, `DuffShop`, `WeaponShop`, `OnWeaponCreate`, `GunCraft`
- Also search for: `Duff` (the NPC name), `Blueprint`

**What we need:**
```
Class name:  ??? (currently: WeaponShopManager)
Method name: ??? (currently: OnWeaponCrafted)
Weapon ID field: ??? (currently: weaponId)
```

**Patch file:** `client/DaveDiverAP/Patches/WeaponCraftPatch.cs`

---

### GameStatePatch.cs
**What it does:** Detects scene transitions (boat, diving, restaurant, farm) to enable/disable item delivery.

**Search for:** `SceneManager`, `GameState`, `OnSceneLoad`, `BoatScene`, `DiveScene`, `RestaurantScene`
- Also search for: `DayStart`, `NightStart`, `DiveStart`, `DiveEnd`
- The WhiteMinds mod does scene switching — check their source for scene class names

**What we need:**
```
Boat scene class:       ??? (currently: BoatSceneManager / OnBoatEnter)
Dive scene class:       ??? (currently: DiveSceneManager / OnDiveStart)
Restaurant scene class: ??? (currently: RestaurantSceneManager / OnRestaurantStart)
Farm scene class:       ??? (currently: FarmSceneManager / OnFarmEnter)
Loading class:          ??? (currently: LoadingManager / OnLoadingStart)
```

**Patch file:** `client/DaveDiverAP/Patches/GameStatePatch.cs`

---

### PlayerDeathPatch.cs
**What it does:** Detects when Dave dies (for Death Link).

**Search for:** `PlayerDeath`, `OnDeath`, `OxygenDepleted`, `Die()`, `PlayerCharacter`, `DaveCharacter`
- Also search for: `GameOver`, `DiveEnd` with failure condition

**What we need:**
```
Oxygen class:      ??? (currently: OxygenSystem / OnOxygenDepleted)
Player class:      ??? (currently: PlayerCharacter / Die)
```

**Patch file:** `client/DaveDiverAP/Patches/PlayerDeathPatch.cs`

---

## Priority 2 — Secondary Systems 🟡

### CookstaPatch.cs
**What it does:** Tracks Cooksta follower count, Best Taste score, and researched recipe count.

**Search for:** `Cooksta`, `Follower`, `FollowerCount`, `BestTaste`, `ResearchedRecipe`, `SocialMedia`

**What we need:**
```
Class name:               ??? (currently: CookstaManager)
Follower count setter:    ??? (currently: set_FollowerCount)
Best taste setter:        ??? (currently: set_BestTaste)
Researched recipe setter: ??? (currently: set_ResearchedRecipeCount)
```

**Patch file:** `client/DaveDiverAP/Patches/CookstaPatch.cs`

---

### EcowatcherPatch.cs
**What it does:** Detects Ecowatcher mission completions and level-ups.

**Search for:** `Ecowatcher`, `EcoWatcher`, `ResearchPoint`, `EcoMission`, `EcoLevel`

**What we need:**
```
Class name:      ??? (currently: EcowatcherManager)
Mission method:  ??? (currently: OnMissionComplete)
Level setter:    ??? (currently: set_Level)
Mission ID field: ??? (currently: missionId)
```

**Patch file:** `client/DaveDiverAP/Patches/EcowatcherPatch.cs`

---

### RestaurantPatch.cs
**What it does:** Detects customer milestones and VIP quest completions.

**Search for:** `SushiBar`, `Restaurant`, `Customer`, `OnCustomerServed`, `VIPMission`, `BanchoSushi`
- Also search for: `CustomerCount`, `TotalCustomers`

**What we need:**
```
Class name:       ??? (currently: SushiBarManager)
Customer method:  ??? (currently: OnCustomerServed)
VIP method:       ??? (currently: OnVIPMissionComplete)
Customer ID field: ??? (currently: customerId)
VIP ID field:     ??? (currently: vipId)
```

**Patch file:** `client/DaveDiverAP/Patches/RestaurantPatch.cs`

---

### FarmPatch.cs
**What it does:** Detects farm harvests, upgrades, and fish farm milestones.

**Search for:** `VegetableFarm`, `ChickenFarm`, `FishFarm`, `Farm`, `Harvest`, `Egg`, `FishBreed`
- Also search for: `Otto` (the NPC who manages the farm)

**What we need:**
```
Veg farm class:     ??? (currently: VegetableFarmManager)
  Methods: OnFirstHarvest, OnTierUpgrade, OnHarvest
Chicken farm class: ??? (currently: ChickenFarmManager)
  Methods: OnTierUpgrade, OnEggCollected
Fish farm class:    ??? (currently: FishFarmManager)
  Methods: OnTankUpgrade, OnFirstBreed, OnFishReachedAdulthood
```

**Patch file:** `client/DaveDiverAP/Patches/FarmPatch.cs`

---

### ChallengePatch.cs
**What it does:** Detects challenge completions.

**Search for:** `Challenge`, `ChallengeComplete`, `ChallengeManager`, `OnChallengeEnd`

**What we need:**
```
Class name:       ??? (currently: ChallengeManager)
Method name:      ??? (currently: OnChallengeComplete)
Challenge ID field: ??? (currently: challengeId)
```

**Patch file:** `client/DaveDiverAP/Patches/ChallengePatch.cs`

---

### PhotographyPatch.cs
**What it does:** Detects photography mission completions and special photos.

**Search for:** `Photo`, `Camera`, `Photography`, `TakoPhoto`, `PhotographyMission`
- Also search for: `Tako` (the NPC who gives photography missions)

**What we need:**
```
Class name:        ??? (currently: PhotographyManager)
Mission method:    ??? (currently: OnMissionComplete)
Special photo:     ??? (currently: OnSpecialPhotoTaken)
Photo taken:       ??? (currently: OnPhotoTaken)
Perfect score:     ??? (currently: OnPerfectScoreAchieved)
```

**Patch file:** `client/DaveDiverAP/Patches/PhotographyPatch.cs`

---

### CollectiblePatch.cs
**What it does:** Detects treasure chest opens, teleport point discoveries, and Duff shop upgrades.

**Search for:** `TreasureChest`, `Chest`, `TeleportPoint`, `TeleportUnlock`, `DuffShop`
- Also search for: `SuccessInteract` (chests use same pattern as fish)

**What we need:**
```
Chest class:         ??? (currently: TreasureChest / SuccessInteract)
Teleport class:      ??? (currently: TeleportPoint / SuccessInteract)
Duff shop class:     ??? (currently: DuffShopManager / OnUpgradePurchased)
Chest ID field:      ??? (currently: chestId)
Teleport ID field:   ??? (currently: teleportId)
```

**Patch file:** `client/DaveDiverAP/Patches/CollectiblePatch.cs`

---

## Priority 3 — Minor Systems 🟢

### IngredientPatch.cs
**What it does:** Detects first-time collection of sea plant ingredients.

**Search for:** `Ingredient`, `SeaPlant`, `PickupIngredient`, `OnIngredientCollect`, `Vendor`, `Shop`
- Also search for: `Agar`, `Kelp`, `Kajime` (specific ingredient names)

**What we need:**
```
Ingredient class:  ??? (currently: IngredientObject / SuccessInteract)
Vendor class:      ??? (currently: VendorManager / OnItemPurchased)
Ingredient name field: ??? (currently: ingredientName)
```

**Patch file:** `client/DaveDiverAP/Patches/IngredientPatch.cs`

---

### MinigamePatch.cs
**What it does:** Detects minigame wins (seahorse race, card game).

**Search for:** `SeahorseRace`, `Race`, `CardGame`, `Minigame`, `MiniGame`

**What we need:**
```
Race class:     ??? (currently: SeahorseRaceManager / OnRaceWon)
Card class:     ??? (currently: CardGameManager / OnAllGamesComplete)
```

**Patch file:** `client/DaveDiverAP/Patches/MinigamePatch.cs`

---

### CharmPatch.cs
**What it does:** Detects charm acquisitions.

**Search for:** `Charm`, `OnCharmGet`, `CharmManager`, `CharmUnlock`, `Bracelet`, `Necklace`

**What we need:**
```
Class name:    ??? (currently: CharmManager / OnCharmAcquired)
Charm ID field: ??? (currently: charmId)
```

**Patch file:** `client/DaveDiverAP/Patches/CharmPatch.cs`

---

## SaveSystem API (for ItemHandler.cs)

When giving items to the player, we need to call the game's SaveSystem or inventory API.

**Search for:** `SaveSystem`, `PlayerInfoSave`, `Inventory`, `AddItem`, `SetGold`
- The save editor (DaveSaveEd) modifies: Gold, Bei, ArtisanFlame, FollowerCount, ingredients
- All these fields exist in the save data — search for their names

**Known save file location:**
```
C:\Users\[Username]\AppData\LocalLow\nexon\DAVE THE DIVER\SteamSData\
```

**Key warning:** IL2CPP uses **ObscuredInt** for encrypted values — always use the
SaveSystem setter methods, never write to raw fields directly!

**What we need:**
```
Save system class:    ??? (currently: SaveSystem)
Player save class:    ??? (currently: PlayerInfoSave)
Add weapon method:    ???
Unlock recipe method: ???
Add ingredient method: ???
iDiver upgrade method: ???
```

---

## ✅ Confirmed Real Class Names (from WhiteMinds dave-diver-expansion)

These are **confirmed real game class names** extracted from the WhiteMinds mod source code:

### Fish / Item Pickup
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `FishInteraction` | `FishInteractionBody` | Fish catching class |
| `SuccessInteract` | `SuccessInteract(BaseCharacter)` | ✅ Confirmed method signature |
| `PickupInstanceItem` | `PickupInstanceItem` | ✅ Confirmed — item pickup |
| `PickupInstanceItem_SeaUrchin` | `PickupInstanceItem_SeaUrchin` | Sea urchin special case |
| `InstanceItemChest` | `InstanceItemChest` | ✅ Confirmed — treasure chests |

### Player / Core
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `PlayerCharacter` | `PlayerCharacter` | ✅ Confirmed — main player class |
| `InGameManager` | `InGameManager` | ✅ Confirmed — has FishAllocators |
| `FishAllocator` | `FishAllocator` | Fish spawn manager |
| `PlayerBreathHandler` | `PlayerBreathHandler` | ✅ Oxygen/breath system! |
| `BuffHandler` | `BuffHandler` | Status effects |
| `CharacterController2D` | `CharacterController2D` | 2D movement |

### Scene Management
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `BoatSceneManager` | `LobbyPlayer` | Boat/lobby area player class |
| `MoveScenePanel` | `MoveScenePanel` | Scene transition panel |

### Restaurant / Sushi Bar
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `SushiBarManager` | `SushiBarManager` | ✅ Confirmed (devopsdinosaur) |
| `SushiBarCustomer` | `SushiBarCustomer` | ✅ Confirmed |
| `SushiBarStaffBase` | `SushiBarStaffBase` | ✅ Confirmed |

### Farm
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `VegetableFarmManager` | `Farm.FarmPlayerView` | Veg farm area |
| `FarmCore` | `Farm.FarmCore` | Farm core mechanics |
| `FishFarmManager` | `FishFarm.FishFarmPlayerView` | Fish farm area |

### Equipment / Weapons
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `SubEquipmentManager` | `SubEquipmentManager` | Equipment system |
| `HarpoonHandler` | `HarpoonProjectile` | Harpoon projectile |
| `InventoryManager` | `InstanceItemInventory` | Equipment inventory |

### Mining / Environment
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `BreakableLootObject` | `BreakableLootObject` | Breakable objects (type enum available) |
| `InteractionGimmick_Mining` | `InteractionGimmick_Mining` | Mining interaction |
| `CrabTrapZone` | `CrabTrapZone` | Crab trap zones |
| `CrabTrapObject` | `CrabTrapObject` | Individual crab traps |

### Minigames
| Placeholder | Real Class Name | Notes |
|---|---|---|
| `SeahorseRaceManager` | `SeahorseRacer` | Seahorse racer class |
| `SeahorseRaceSession` | `SeahorseRaceSessionPlay` | Race session |

### Key Enums Found
```csharp
// FishInteractionType — use Pickup = 2 to detect catches
enum FishInteractionType { None, Attack, Pickup, Special }

// BreakableLootObjectType — 8 values for different breakable types
enum BreakableLootObjectType { ... }

// SubHelperType — 13 values for equipment types
enum SubHelperType { ... }
```

### Key Patterns Confirmed
```csharp
// Singleton access pattern used in this game:
Singleton<T>._instance
SingletonNoMono<T>.s_Instance

// Universal interaction pattern:
CheckAvailableInteraction() → SuccessInteract(BaseCharacter player)

// Chest opening — patch THIS:
[HarmonyPatch(typeof(InstanceItemChest), "SuccessInteract")]
```

### Save System (from DaveSaveEd)
- Save files: `GameSave_00_GD.sav` (XOR encrypted, decrypts to JSON)
- Editable fields confirmed: `Gold`, `Bei`, `ArtisanFlame`, `FollowerCount`
- Ingredients stored as JSON with byproducts, plants, seasonings sections
- Uses `nlohmann_json` + `sqlite3` for ID lookups internally

---

## Known Good Info (from existing mods)

These are confirmed from the WhiteMinds mod and other sources:

- ✅ Fish/item/chest pickup uses `SuccessInteract()` pattern
- ✅ BepInEx 6 IL2CPP works (use BepInEx 6.0.0-be.752 or newer)
- ✅ Patches target `BepInEx/interop/Assembly-CSharp.dll`, NOT the game's raw DLL
- ✅ Values use ObscuredInt (encrypted) — use setters, not raw field access
- ✅ ilspycmd works on the interop DLL to list/decompile classes

## Existing Mods to Check for Class Names

These open-source mods may already contain some real class names:

1. **https://github.com/WhiteMinds/dave-diver-expansion** — best reference, auto-pickup patches
2. **https://github.com/devopsdinosaur/dave-the-diver-mods** — Super Dave, farm/restaurant patches
3. **https://github.com/Arutsuyo/SuperDave2.0** — enhanced Super Dave, updated Sept 2025
4. **https://github.com/FNGarvin/DaveSaveEd** — save editor, reveals save data field names
