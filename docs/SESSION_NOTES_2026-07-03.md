# Session Notes 2026-07-03

## ContentsList IDs (from Unity Explorer: DataManager.ContentsUnlockDataDic)

These are the `ContentsList` enum values (ContentsID column) used with
`ContentsUnlockManager.Instance.UnlockContents((ContentsList)id)`.

### Should be unlocked at AP game start (always available)
| ContentsID | NameTextID | Notes |
|-----------|-----------|-------|
| 10031 | ContentsName_FishCard | Marinca (Fish Encyclopedia) — unlocked by Sato after first dive |
| 10014 | ContentsName_Idiver | iDiver app — unlocked by Dr. Bacon at chapter 1 |
| 10005 | ContentsName_SushiBar_Recipe | Recipe management tab |
| 10006 | ContentsName_SushiBar_Research | Research (dish upgrade) tab |
| 10007 | ContentsName_SushiBar_Staff | Staff management |
| 10008 | ContentsName_SushiBar_Ingredients | Ingredients management |
| 10009 | ContentsName_SushiBar_Storage | Storage management |
| 10004 | ContentsName_Craft | Weapon crafting |
| 10040 | ContentsName_SnsOpen | Cooksta SNS app |
| 10028 | ContentsName_EcoWatcher | Ecowatcher app |
| 10029 | ContentsName_Charm | Charm equipment |
| 10011 | ContentsName_SubMission | Sub-missions |
| 10027 | ContentsName_SushiDash | Sushi dash minigame |
| 10048 | ContentsName_GreenTea | Green tea minigame |

### Items that should be AP items (received from Archipelago)
| ContentsID | NameTextID | Notes |
|-----------|-----------|-------|
| 10001 | ContentsName_FarmOpen | Vegetable Farm unlock |
| 10010 | ContentsName_SushiBar_FishFarm | Fish Farm unlock |
| 10016 | ContentsName_Farm_Animal | Chicken Farm unlock |
| 10024 | ContentsName_NightDive | Night Dive unlock |
| 10030 | ContentsName_Catcher | Net Catcher (for small fish) |
| 10044 | ContentsName_Camera | Underwater Camera |
| 10066 | ContentsName_ManagementApp | Management App |
| 10067 | ContentsName_CrabTrap | Crab Trap |
| 10013 | ContentsName_CobraShop | Cobra's Shop |
| 11008 | ContentsName_iDiver_Suit_Lv2 | iDiver Cold Resistant Suit Lv2 |
| 11016 | ContentsName_KeyRing | Key Ring (charm slots) |
| 11016 | ContentsName_iDiver_Basic_Knife | Dive Knife |
| 11015 | ContentsName_IcePicking | Ice Picking (glacier tool) |
| 10033 | ContentsName_Mirror | Sea People Mirror (teleport) |
| 10034 | ContentsName_Phone | Dave's Phone |
| 10047 | ContentsName_EscapePod | Escape Pod |
| 10055 | ContentsName_BelugaTaxi | Beluga Whale Taxi |
| 10045 | ContentsName_Drone | Salvage Drone |
| 10025 | ContentsName_Grab | Grab ability |
| 11007 | ContentsName_SushiBar_Branch | Sushi Bar Branch |
| 10002 | ContentsName_SpecialCustomer | Special Customer events |
| 10039 | ContentsName_WeeklyFish | Weekly Fishing |
| 10075 | ContentsName_MeleeWeapon | Melee Weapon tree |

### Jungle DLC contents
| ContentsID | NameTextID | Notes |
|-----------|-----------|-------|
| 410041 | ContentsName_DLC_Jungle_Fishing | Jungle fishing |
| 410042 | ContentsName_DLC_Jungle_BugNet | Bug Net |
| 410003 | ContentsName_DLC_Jungle_HerbalistShop | Herbalist Shop |
| 410009 | ContentsName_DLC_Jungle_BanchoSushi_Grill | Bancho Grill |
| 410021 | ContentsName_DLC_Jungle_MetaGun | Meta Gun |
| 410031 | ContentsName_DLC_Jungle_Machete | Machete |

## Fish TIDs (from Unity Explorer: DataManager.FishInfoDataDic)

See FishCatchPatch.cs _fishIdMap for complete mapping.
Key finding: seahorse ocean TIDs are NOT in FishInfoDataDic — find via [FishCaught] log.

## Confirmed Mission TIDs
| TID | Title | Notes |
|-----|-------|-------|
| 10010001 | Prepare Sushi Ingredients | Cleared in prologue |
| 10010003 | Weaponsmith Duff | Sub-mission, cleared in prologue |
| 10010004 | Tracking the Sea People | Cleared in prologue |
| 10012903 | Not Enough Workers | Sub-mission |
| 10012004 | Red Ecological Data | Ecowatcher mission |
| 10015001 | A Dolphin's Request | Sub-mission (charm reward) |
| 10010002 | ??? | NOT cleared — prologue skip TID? Check in-game |

## Recipe TIDs
- `8050xxx` = Sushi/appetizer unlock TIDs (e.g. 8050001 = Clownfish Sushi)
- `8051xxx` = Main dish unlock TIDs (e.g. 8051009 = Whole-Roasted Shark Head)
- `CookingStudyData.recipeID` = TID, `.studyLevel` = research level

## ContentsList IDs for iDiver Upgrades (from dump.cs)
- TID 3001001 = Oxygen Tank Level 1
- TID 3002001 = Harpoon Level 1  
- TID 3003001 = Diving Suit Level 1
(confirmed working via CompleteMission)
