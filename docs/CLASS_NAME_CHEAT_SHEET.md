# Dave the Diver — Harmony Patch Class Name Reference

All class and method names confirmed via **dump.cs** (generated with Il2CppDumper on the
`GameAssembly.dll` + `global-metadata.dat` from Dave the Diver v1.x, June 2026).

## ✅ Fully Confirmed Classes & Methods

| Patch File | Class | Method | Notes |
|---|---|---|---|
| `FishCatchPatch` | `FishInteractionBody` | `SuccessInteraction()` | All fish/item catches — MonoBehaviour with UnityEvent fields |
| `BossDefeatedPatch` | `BossScene` | `FinishBossScene()` | Fires when boss HP reaches 0 and death sequence starts |
| `PlayerDeathPatch` | `PlayerCharacter` | `OnDie()` | No-arg overload; covers both oxygen & damage death |
| `WeaponCraftPatch` | `WeaponCraftTreeViewPanel` | `WeaponCraftTreeEventTrigger(int craftID, int row, int col)` | `WeaponCraftTreeEvent` struct: craftID, rowIndex, colIndex |
| `RecipeUnlockPatch` | `SaveData` | `AddUnlockRecipeSaveData(int id, DateTime)` | Recipe unlock persisted here |
| `RecipeUnlockPatch` | `SaveData` | `UpdateUnlockRecipeSave()` | Research level-up persisted here |
| `StoryProgressPatch` | `ChapterManager` | `set_currentChapterInfo(ChapterInfo)` | Singleton; ChapterInfo has chapterNumber field |
| `StoryProgressPatch` | `MissionManager` | `UpdateMission(MissionClearType, int target, int count, ...)` | Central hub for ALL missions; target = TID |
| `GameStatePatch` | `LobbyPlayer` | `ChangeLobbyPlayerState(LobbyPlayer.LobbyPlayerState)` | `InBoat=12` enables item delivery; `Diving=5` disables |
| `GameStatePatch` | `SushiBarManager` | `OnRestaurantStart()` | Singleton; additional restaurant disable guard |
| `CookstaPatch` | `SNSInfoSave` | `set_followerCount(ObscuredInt)` | ObscuredInt — cast to int |
| `CookstaPatch` | `SNSInfoSave` | `set_grade(ObscuredInt)` | Grade = rank (Bronze/Silver/Gold/Platinum/Diamond) |
| `CookstaPatch` | `SNSInfoManager` | `CheckGradeConditionMessage(...)` | SingletonNoMono; fires on rank-up check |
| `PhotographyPatch` | `LobbyPostRoutine` | `PhotoRewardSequence()` | Coroutine; fires after photo scored |
| `ChallengePatch` | `MissionManager` | `UpdateMission(MissionClearType, int, int)` | Same as StoryProgress — filter by TID |
| `EcowatcherPatch` | `EcoWatcherDeliverPopup` | `OnDREvent(...)` | Fires on deliver confirmation |
| `EcowatcherPatch` | `EcoWatcherResearchRankUpPopup` | `OnDREvent(...)` | Fires on Ecowatcher level-up |
| `FarmPatch` | `MVFarmFieldController` | `DoHarvest(int laneNum)` | Veg farm; also has `RequestSowSeed(int, int)` |
| `FarmPatch` | `MVFarmHarvestPopupCtrler` | `OnEggCollected()` | Chicken farm egg collection popup |
| `FarmPatch` | `FishFarmDynamicEnvironmentController` | `OnFishFarmUpgraded(int)` | Singleton; fish farm upgrades |
| `MinigamePatch` | `SeahorseRaceSessionPlay` | `OnGoalPlayer()` | Sealed class; fires when player crosses finish line |
| `CollectiblePatch` | `InstanceItemChest` | `SuccessInteraction()` | Confirmed — chests use same pattern as fish |
| `IngredientPatch` | `SaveData` | `AddIngredientsSaveData(IngredientsData)` | Fires on any ingredient first pick-up or purchase |
| `CharmPatch` | `LobbyCharmSwapPanel` | `AutoEquipCharmItem(int tid)` | tid = CharmSpecData TID |
| `RestaurantPatch` | `SushiBarAnalyticsReportSequenceCookStar` | `DoSequence()` | Coroutine; fires after each restaurant service night |

---

## 🔧 Still Needs TID Mapping (design sheet data)

These patches have the correct classes and methods but need the design sheet TID integers
to map game events to AP location names. Get TIDs via **UnityExplorer** in-game or by
searching the `dump.cs` for static const values near the relevant classes.

| Patch | What to find | Where to look |
|---|---|---|
| `WeaponCraftPatch` | weapon craft TIDs → `_idMap` | Search dump.cs near `GunSpecData` |
| `RecipeUnlockPatch` | recipe TIDs → `_map` | Search dump.cs near `UnlockRecipeSave` |
| `StoryProgressPatch` | mission TIDs → `_map` | Search dump.cs near `MissionManager` |
| `ChallengePatch` | challenge TIDs → `_map` | Same — filter by `MissionClearType` |
| `IngredientPatch` | ingredient TIDs → `_map` | Search dump.cs near `IngredientsData` |
| `CharmPatch` | charm TIDs → `_map` | Search dump.cs near `CharmSpecData` |

---

## 📋 Key Class Reference (from dump.cs)

### Player & Death
```
PlayerCharacter        : BaseCharacter, IDamageable ...
  - public void OnDie()                            // ← HOOK THIS (no-arg overload)
  - public void OnDie(DieAnimType dieType = 0)

PlayerBreathHandler    : MonoBehaviour, IHasHP
  - private float m_HP  // 0x9C                   // oxygen HP; when 0 calls PlayerCharacter.OnDie()
```

### Fish Catching
```
FishInteractionBody    : MonoBehaviour, IInteractionObject
  - public UnityEvent SuccessPickupFish
  - public UnityEvent SuccessCarving
  - public override void SuccessInteraction()      // ← HOOK THIS
```

### Boss Fights
```
BossScene              : DRMonoBehaviour
  - public static BossScene Current
  - public BossSceneSO bossSceneSO                 // SO name identifies the boss
  - public void FinishBossScene()                  // ← HOOK THIS

Boss controller subclasses of SABossControllerBase:
  BossClioneController, BossWolffishController, BossGoblinSharkController,
  BossGreatWhiteSharkController, BossHelicoprionController, HermitCrabController,
  BossJW2Controller (John Watson ch4), BossJW3Controller (John Watson ch7),
  BossLuscaController, BossMantisShrimpController, BossKronosaurus,
  SABossAnomalocaris (Yawie), SABossEbirah, BossGiantGardonController
```

### Weapon Crafting
```
WeaponCraftTreeEvent (struct)
  - int craftID    // weapon design sheet TID
  - int rowIndex
  - int colIndex

WeaponCraftTreeViewPanel : DRMonoBehaviour, IDREventListener<WeaponCraftTreeEvent>
  - public static void WeaponCraftTreeEventTrigger(int craftID, int row, int col)  // ← HOOK THIS
```

### Recipes & Research
```
SaveData
  - public void AddUnlockRecipeSaveData(int id, DateTime unlockTime)  // ← recipe unlock
  - public void UpdateUnlockRecipeSave()                              // ← research level up
  - public void AddIngredientsSaveData(IngredientsData data)          // ← ingredient first find
  - public void RemoveIngredientsSaveData(int id)
  - public Dictionary<int, UnlockRecipeSave> GetAllUnlockRecipes()

UnlockRecipeSave : ISerializable
  - private ObscuredInt m_UnlockRecipeID  // 0x10
```

### Chapters & Missions
```
ChapterManager         : Singleton<ChapterManager>
  - public ChapterInfo currentChapterInfo { get; set; }  // ← hook setter
  - private List<ChapterInfo> _chapters

MissionManager         : Singleton<MissionManager>, IEditorTime
  - public static void UpdateMission(MissionClearType type, int target, int count,
        bool isSkipEnqueueDialogData = false, Predicate<MissionData> extraChecker = null,
        bool doNotUpdateCanvas = false)             // ← HOOK THIS (target = mission TID)
```

### Cooksta / SNS
```
SNSInfoSave            : ISerializable
  - public ObscuredInt followerCount { get; set; }  // ← hook set_followerCount
  - public ObscuredInt grade { get; set; }           // ← hook set_grade
  - private ObscuredInt m_LikeCount  // 0x24

SNSInfoManager         : SingletonNoMono<SNSInfoManager>
  - public static void CheckGradeConditionMessage(...)  // ← HOOK THIS
  - public static SNSGrade Grade { get; }

LobbyPlayer.LobbyPlayerState enum:
  Idle=0, Call=1, Die=2, Clear=3, MaskOffClear=4, Diving=5,
  ThumbUp=6, MorningStart=7, AfternoonStart=8, EveningStart=9,
  Memo=10, EnterBoat=11, InBoat=12    ← item delivery fires on InBoat=12
```

### Game State / Boat
```
LobbyPlayer            : MonoBehaviour
  - public void ChangeLobbyPlayerState(LobbyPlayer.LobbyPlayerState state)  // ← HOOK THIS
  - public LobbyPlayer.LobbyPlayerState CurrentState { get; }
  - private LobbyPlayer.LobbyPlayerState m_State  // 0x68
```

### Farming
```
MVFarmFieldController  : MonoBehaviour
  - public void DoHarvest(int laneNum)            // ← HOOK THIS (veg farm)
  - public void RequestSowSeed(int laneNum, int seedTID)
  - public MVFarmLaneLocker.LockStatus GetCurrentLaneLockState(int laneNum)

MVFarmHarvestPopupCtrler : MonoBehaviour
  - hook OnEggCollected()                         // ← HOOK THIS (chicken farm)

FishFarmDynamicEnvironmentController : Singleton<FishFarmDynamicEnvironmentController>
  - hook OnFishFarmUpgraded(int)                  // ← HOOK THIS (fish farm)
```

### Photography
```
PhotoZone              : MiniGameBase<PhotoZone.Data, PhotoZone.Result>
  - public int photozoneTID
  - public UnityEvent OnEnterPhotoMode / OnExitPhotoMode

LobbyPostRoutine       : MonoBehaviour
  - private IEnumerator PhotoRewardSequence()     // ← HOOK THIS
  - internal Reward <PhotoRewardSequence>b__24_2(int elem)

InteractionGimmick_PhotoZone : InteractionGimmick
  - fires when player activates a photo zone
```

### Ecowatcher
```
EcoWatcherDeliverPopup : MonoBehaviour, IOnDirectHandler_UI
  - public EcoWatcherDeliverCell cell
  - hook OnDREvent()                              // ← HOOK THIS (deliver confirmation)

EcoWatcherResearchRankUpPopup : MonoBehaviour, IOnDirectHandler_UI
  - hook OnDREvent()                              // ← HOOK THIS (level-up popup)
```

### Charms
```
LobbyCharmSwapPanel    : MonoBehaviour
  - public void AutoEquipCharmItem(int tid)       // ← HOOK THIS
  - public void AutoUnequipCharmItem(int tid)
  - public int GetEquipCharmCount()

CharmSpecData          : SpecDataBase
  - public List<CharacterAbilityData> Abilitys
```

### Restaurant
```
SushiBarManager        : Singleton<SushiBarManager>

SushiBarAnalyticsReportSequenceCookStar : SushiBarAnalyticsReportSequenceElement
  - private SushiBarAnalyticsResult m_GainFollowerResult
  - private UITextCounter m_GainFollowerCount
  - IEnumerator DoSequence()                      // ← HOOK THIS (post-service night)
```

### Collectibles / Chests
```
InstanceItemChest      : MonoBehaviour, IInteractionObject
  - public void SuccessInteraction()              // ← HOOK THIS
  - private bool IsOpen  // 0x63
```

### Seahorse Racing
```
SeahorseRaceSessionPlay : MonoBehaviour, ISessionPlay (sealed class)
  - IEnumerator OnGoalPlayer()                    // ← HOOK THIS (player finishes race)
  - IEnumerator OnReturnToMenu_Impl()
  - IEnumerator Start_Impl()

SeahorseRacerState_Finish : SeahorseRacerState   // win state
SeahorseRacerState_Fail   : SeahorseRacerState_Finish  // lose state
SeahorseRaceSession.Destination                  // finish line data
```
