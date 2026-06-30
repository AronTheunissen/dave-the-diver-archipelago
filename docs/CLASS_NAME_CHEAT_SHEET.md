# Dave the Diver — Harmony Patch Class Name Reference

All class and method names **confirmed via dump.cs** generated with Il2CppDumper on game version
with Jungle DLC installed (dump dated 2026-06-19).

---

## ✅ Confirmed Patches & Hook Points

| Patch File | Hook Class | Hook Method | Notes |
|---|---|---|---|
| `FishCatchPatch` | `FishInteractionBody` | `SuccessInteract(BaseCharacter)` | Fires on any fish interaction (carve/pickup/balloon) |
| `BossDefeatedPatch` | `CommonBossDead` | `DoJob()` | Fires for ALL bosses via BossSceneSO job system |
| `RecipeUnlockPatch` | `SaveData` | `AddUnlockRecipeSaveData(int, DateTime)` | Fires when a recipe is unlocked and saved |
| `RecipeUnlockPatch` | `SaveData` | `UpdateUnlockRecipeSave()` | Fires when dish research level increases |
| `WeaponCraftPatch` | `DREventTriggerManager` | `WeaponCraftTreeEventTrigger(int, int, int)` | Static method; craftID, row, col |
| `StoryProgressPatch` | `ChapterManager` | `set_currentChapterInfo` | Fires when chapter changes |
| `StoryProgressPatch` | `MissionManager` | `UpdateMission(MissionClearType, int, int, ...)` | Central hub for ALL mission updates |
| `ChallengePatch` | `MissionManager` | `UpdateMission(MissionClearType, int, int, ...)` | Same hook — filter by challenge TID |
| `EcowatcherPatch` | `EcoWatcherDeliverPopup` | `OnDREvent` | Fires on Ecowatcher delivery confirm |
| `EcowatcherPatch` | `EcoWatcherResearchRankUpPopup` | `OnDREvent` | Fires on research rank up |
| `CookstaPatch` | `SNSInfoSave` | `set_followerCount(ObscuredInt)` | Fires when follower count changes |
| `CookstaPatch` | `SNSInfoSave` | `set_grade` | Fires when Cooksta grade changes |
| `CookstaPatch` | `SNSInfoManager` | `CheckGradeConditionMessage()` | Re-evaluate rank-up conditions |
| `PhotographyPatch` | `LobbyPostRoutine` | `PhotoRewardSequence` | Fires after photo is scored |
| `FarmPatch` | `MVFarmFieldController` | `DoHarvest(int)` | Fires on any farm lane harvest |
| `FarmPatch` | `SaveData.FarmSave` | `SetDailyHarvestItems()` | Fires at day start; populates daily harvest incl. eggs |
| `FarmPatch` | `SaveData.FishFarmAreaSave` | `set_IsOpen(ObscuredBool)` | Fires when fish farm area is unlocked |
| `MinigamePatch` | `SeahorseRaceSessionPlay` | `OnGoal(int lane)` | Fires when any racer finishes; filter lane==4 |
| `CollectiblePatch` | `InstanceItemChest` | `SuccessInteract(BaseCharacter)` | Fires on chest open |
| `PlayerDeathPatch` | `PlayerCharacter` | `OnDie()` (no args) | Public entry point for player death |
| `RestaurantPatch` | `SushiBarAnalyticsReportSequenceCookStar` | `DoSequence` | Fires after each service night |
| `RestaurantPatch` | `SushiBarManager` | `CanProcessVIPShowdownResult(out MissionData, out MissionConditionData, out VIPCustomer)` | Returns true when VIP result pending |
| `GameStatePatch` | `LobbyPlayer` | `ChangeLobbyPlayerState(LobbyPlayer.LobbyPlayerState)` | InBoat=12, Diving=5 |
| `GameStatePatch` | `SushiBarManager` | `OnEventSushiBarOpened()` | Fires when restaurant opens for service |
| `IngredientPatch` | `SaveData` | `AddIngredientsSaveData(IngredientsData)` | Fires when ingredient added to save |
| `CharmPatch` | `LobbyCharmSwapPanel` | `AutoEquipCharmItem(int tid)` | Fires when charm is auto-equipped on acquire |

---

## 🔑 Key Classes & Fields

### Boss System
```
BossScene : DRMonoBehaviour                    // Base class for all boss encounters
  static BossScene Current                     // Static ref to active boss scene
  BossSceneSO bossSceneSO                     // ScriptableObject with boss data
  EnumBossFishType bossType                    // Enum identifying boss type (see below)
  
CommonBossDead : BossSceneSO.JobStuff         // Job fired when boss dies — hook DoJob()
EbirahBattleScene : BossScene                 // Ebirah-specific scene (has FinishBossScene())
BossGardonScene : BossScene                   // Gardon-specific scene
BossLuscaScene : BossScene                    // Lusca-specific scene

EnumBossFishType (int enum):
  K99_Unknown=0, k00_Boss_GiantSquid=1, k01_Boss_HermitCrab=2,
  k02_Boss_WolfFish=3, k03_Boss_Clione=4, k04_Boss_JW2=5,
  k05_Boss_Gardon=6, k06_Boss_MantisShrimp=7, k07_Boss_GoblinShark=8,
  k_Boss_Helicoprion=9, k_Boss_GreatWhiteShark=10, k_Boss_Anomalocaris=11,
  k_Boss_Lusca=12, k_Boss_Ebirah=100,
  Jungle DLC: k_BossStethacanthus=201, k_BossXiphactinus=202,
              k_BossSulong=203, k_BossSnappingTurtle=204
```

### Fish Catching
```
FishInteractionBody : MonoBehaviour            // Attached to each fish in the world
  void SuccessInteract(BaseCharacter player)  // Hook this — fires on catch/carve/balloon
  UnityEvent SuccessPickupFish                // UnityEvent (don't hook directly)
  UnityEvent SuccessCarving
```

### Recipe / Dish System
```
SaveData                                       // Game's main save class (not our mod's)
  void AddUnlockRecipeSaveData(int id, DateTime unlockTime)  // Hook: recipe unlocked
  void UpdateUnlockRecipeSave()               // Hook: dish research level updated
  bool IsUnlockRecipe(int id)                 // Query only — do NOT hook
  Dictionary<int, UnlockRecipeSave> unlockRecipeData
  
UnlockRecipeSave                              // Per-recipe save data
```

### Weapon Crafting
```
DREventTriggerManager (static class)          // Central event trigger hub
  static void WeaponCraftTreeEventTrigger(int craftID, int row, int col)  // Hook this

WeaponCraftTreeViewPanel : DRMonoBehaviour    // UI panel — DO NOT hook here
WeaponCraftTreePanel : DRMonoBehaviour        // Parent panel
```

### Mission / Quest / Challenge System
```
MissionManager : Singleton<MissionManager>
  static void UpdateMission(MissionClearType type, int target, int count,
                            bool isSkipEnqueueDialogData, Predicate<MissionData> extraChecker,
                            bool doNotUpdateCanvas)  // Hook this for quests + challenges
  MissionClearCondition GetMissionClearCondition(int tid)

ChapterManager : Singleton<ChapterManager>
  ChapterInfo currentChapterInfo { get; set; }  // Hook setter
  
MissionData
  int MissionClearTID  // at offset 0x2C
```

### Seahorse Racing
```
SeahorseRaceSessionPlay : MonoBehaviour, ISessionPlay   // sealed class
  void OnGoal(int lane)                       // Hook this — fires when any racer finishes
  private IEnumerator OnGoalPlayer()          // Internal coroutine — do NOT hook
  private SeahorseRaceSession _session        // at offset 0x70 — use Traverse to read

SeahorseRaceSession                           // sealed class
  SeahorseRaceTrackData trackData { get; }

SeahorseRaceTrackData                         // sealed class
  const int playerLane = 4                    // Player is always lane 4
  SeahorseRaceTrackKey trackKey { get; }

SeahorseRaceTrackKey
  private SeahorseRaceTrackKey.Division _division  // Use Traverse to read
  
SeahorseRaceTrackKey.Division (enum):
  C=0 (Easy), B=1 (Medium), A=2 (Hard), S=3 (Expert)
```

### Fish Farm
```
FishFarmManager : Singleton<FishFarmManager>
SaveData.FishFarmSave                         // Fish farm save data
SaveData.FishFarmAreaSave                     // Per-area save data
  ObscuredInt AreaID                          // Matches FishFarmAreaType enum
  ObscuredBool IsOpen                         // Hook set_IsOpen
  
FishFarmAreaType (enum):
  None=0, A=1, B=2, C=3, D=4, E=5, F=6, G=7, H=8
  
FishFarmDynamicEnvironmentController : Singleton<FishFarmDynamicEnvironmentController>
```

### Vegetable / Chicken Farm
```
MVFarmFieldController                         // Manages farm field lanes
  void DoHarvest(int laneNum)                 // Hook: vegetable harvest AND egg collection
  void RequestSowSeed(int laneNum, int seedTID)

SaveData.FarmSave
  void SetDailyHarvestItems()                 // Hook: populates daily harvest (incl. eggs)
  bool HasHarvestItemToClaim { get; set; }
```

### Restaurant / VIP
```
SushiBarManager : Singleton<SushiBarManager>
  void OnEventSushiBarOpened()                // Hook: restaurant opens for service
  bool CanProcessVIPShowdownResult(out MissionData missionData,
       out MissionConditionData resultCondition, out VIPCustomer customer)
  private IEnumerator ProcessVIPShowdownResult()  // Internal — don't hook

VIPCustomer : SushiBarCustomer
  string AssetKey                             // Identifies VIP character
  MissionData m_LinkMissionData               // Linked mission

VIPCookingScenarioDataList.VIP_TID (enum):
  WangPang=9100017, Alex=9100018, Pastro=9100019

SushiBarAnalyticsReportSequenceCookStar
  IEnumerator DoSequence()                    // Hook: fires after each service night
```

### Cooksta / SNS
```
SNSInfoSave
  ObscuredInt followerCount { get; set; }     // Hook set_followerCount
  
SNSInfoManager : Singleton<SNSInfoManager>
  void CheckGradeConditionMessage()           // Hook for recipe/grade checks
```

### Ecowatcher
```
EcoWatcherDeliverPopup : MonoBehaviour, IOnDirectHandler_UI
  // Hook OnDREvent — fires on delivery confirm

EcoWatcherResearchRankUpPopup : MonoBehaviour, IOnDirectHandler_UI  
  // Hook OnDREvent — fires on research rank up
```

### Photography
```
LobbyPostRoutine
  IEnumerator PhotoRewardSequence             // Hook — fires after photo is scored
  
PhotoZone : MiniGameBase<PhotoZone.Data, PhotoZone.Result>
PhotoZoneEntity : PhotoZone                   // World instance
```

### Collectibles / Chests
```
InstanceItemChest : MonoBehaviour, IInteractionObject
  void SuccessInteract(BaseCharacter player)  // Hook: chest opened
```

### Player Death
```
PlayerCharacter
  void OnDie()                                // Hook: no-arg overload = public entry point
  void OnDie(PlayerCharacter.DieAnimType)     // Also exists — don't hook this one
  
PlayerBreathHandler                           // Manages oxygen HP (m_HP at 0x9C)
```

### Game State
```
LobbyPlayer : MonoBehaviour
  void ChangeLobbyPlayerState(LobbyPlayer.LobbyPlayerState state)
  
LobbyPlayer.LobbyPlayerState (enum):
  InBoat=12, Diving=5, MorningStart=7, AfternoonStart=8
```

### Ingredients
```
IngredientsStorage : SingletonNoMono<IngredientsStorage>
  void AddIngredients(int ingredientsID, int count = 1, Place place = 0)  // Give ingredient

IngredientsData                               // Game's ingredient data class
  int ingredientsID                           // lowercase — NOT ObscuredInt
  int level, parentID, rank
  IngredientsType type
  int[] counts
```

### Charms
```
LobbyCharmSwapPanel
  void AutoEquipCharmItem(int tid)            // Hook: fires when charm acquired+auto-equipped
  
CharmSpecData                                 // Design data for each charm
```

---

## 🌿 Jungle DLC Class Names (confirmed via dump.cs 2026-06-26)

| Purpose | Class Name | Notes |
|---|---|---|
| Rod fishing catch | `FishingRodHandler` | `MonoBehaviour` — hook catch completion method |
| Jungle NPC manager | `VillageManager` | `Singleton<VillageManager>` — manages all villager relationships |
| Villager NPC state | `VillageNPCState` | Per-NPC state machine |
| Bancho Grill restaurant | `JungleSushiBarManagerSystem` | `static class` — Bancho Grill manager |
| Grill recipes | `GrillRecipeEntity` | Design data for Bancho Grill recipes |
| Grill recipe save | `SaveData.GrillRecipeDataDic` | `Dictionary<int, GrillRecipeEntity>` |
| Jungle equipment | `JungleEquipmentLevelEntity` | Purification Filter, Machete etc. levels |
| Jungle equipment UI | `JungleEquipmentLevelUpCell` | Level-up UI — hook here for equipment upgrades |
| Jungle ingredients | `JungleIngredientGroup` | Design data for jungle ingredients |
| Jungle missions | `JungleMissionPhoneAlarmController` | Mission phone alerts for jungle |
| Jungle special cond. | `JungleMissionSpecialConditionCheck` | Special mission conditions (NPC agree etc.) |
| Jungle lake | `JungleLakeEnvironmentController` | Lake environment controller |
| Bancho Grill rank | `JungleSushiBarRankEntity` | Design data for Bancho Grill rank |
| Jungle equipment const | `ItemType.JungleFishingRod = 60` | Fishing rod item type enum value |
| Jungle commit mission | `JDLC_JUNGLE_COMMIT_MISSION_TID = 410010001` | Jungle main commit mission TID |

---

## 🚫 Classes That Do NOT Exist (old placeholder names)

These names appeared in old docs but are **NOT in the game**:

| Fake Name | What it actually is |
|---|---|
| `BossContainer` | `BossScene` / `CommonBossDead` |
| `RecipeResearch` | `SaveData.AddUnlockRecipeSaveData` |
| `DuffShop` | `PhoneAppList` constant (value 14060002) — just a UI app |
| `DuffShopManager` | Does not exist |
| `WeaponCraft` | `DREventTriggerManager.WeaponCraftTreeEventTrigger` |
| `ChallengeManager` | `MissionManager` handles challenges too |
| `CardGameManager` | Not confirmed — needs investigation |
| `EcowatcherDatabase` | `EcoWatcherDeliverPopup` / `EcoWatcherResearchRankUpPopup` |
| `SpeciesEntry` / `SpeciesUnlock` | Not found in dump |
| `PhotoCamera` / `PhotoCapture` | `PhotoZone` / `LobbyPostRoutine.PhotoRewardSequence` |
| `SeahorseRaceManager` | `SeahorseRaceSessionPlay` |
| `RaceResult` | `SeahorseRaceSession.Destination` |
| `VegetableGarden` | `MVFarmFieldController` |
| `FishFarm` (class) | `FishFarmManager` / `FishFarmDynamicEnvironmentController` |
| `ChickenFarm` (class) | `SaveData.FarmSave.SetDailyHarvestItems` |
| `SNSPost` / `SNSLikes` | `SNSInfoSave.set_followerCount` |

---

## 🛡️ Load Guard Pattern

When hooking gameplay methods that may also fire during save/load, use `ItemQueue.IsGameReady` to guard against re-applying items during load replay.

### Why This Is Needed

Methods like `IngredientsStorage.AddIngredients()` fire in two contexts:
1. **During gameplay** (player action) — when we want to detect completion
2. **During save load** (deserialization replay) — when we DON'T want to re-trigger

Without a guard, items get incorrectly applied twice, or crashes occur from invalid game state during load.

### Pattern: ItemQueue.IsGameReady Guard

```csharp
[HarmonyPatch(typeof(IngredientsStorage), "AddIngredients")]
[HarmonyPostfix]
public static void OnIngredientsAdded(int ingredientId, int count, Place place)
{
    try
    {
        // Skip during save/load — ItemQueue tracks when game is fully ready
        if (!ItemQueue.Instance.IsGameReady)
            return;
        
        // Now safe to mark location as checked
        if (LocationTracker.TryGetLocationId(ingredientId, place, out var locId))
        {
            Plugin.Log.LogDebug($"[Ingredient] {ingredientId} at {place} → {locId}");
            ConnectionManager.Instance?.SendLocationCheck(locId);
        }
    }
    catch (Exception ex)
    {
        Plugin.Log.LogError($"[Ingredient] Exception: {ex.Message}");
    }
}
```

### How IsGameReady Works

`ItemQueue.IsGameReady` is set to `true` after:
- Game fully loads (scene is active)
- Player can interact with the game
- All save/load replay is complete

The flag is used to distinguish:
- ✅ **Real player actions** (IsGameReady = true)
- ❌ **Deserialization replay** (IsGameReady = false during load)

This pattern replaces dangerous hooks on `SaveData.*` methods, which fire during replay and cause crashes.
