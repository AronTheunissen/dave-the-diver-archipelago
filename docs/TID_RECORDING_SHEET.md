# Dave the Diver — TID Recording Sheet

Use this sheet during your UnityExplorer session to record TID numbers.

**How to open UnityExplorer:** Launch the game → press **F7**
**How to use C# console:** UnityExplorer → C# Console tab → paste script → Run

---

## 📋 TID Mapping Status (as of 2026-06-27)

| Mapper | File | Status | Notes |
|---|---|---|------|
| `BossNameMapper` | `BossDefeatedPatch.cs` | ✅ Complete | Uses `EnumBossFishType` enum — no TIDs needed |
| `FishNameMapper` | `FishCatchPatch.cs` | ✅ Complete | 200+ TIDs from `CaughtFishData` dump + GO name format |
| `RodFishNameMapper` | `JungleFishingPatch.cs` | ✅ Complete | All 12 rod fish TIDs confirmed (42013501-42013513) |
| `WeaponNameMapper` | `WeaponCraftPatch.cs` | ✅ Complete | 48 weapon TIDs from `SubEquipmentData` dump (3060xxx range) |
| `RecipeNameMapper` | `RecipeUnlockPatch.cs` | ✅ Complete | 248 recipe TIDs from `unlockRecipeData` dump (8050xxx-8059xxx) |
| `IngredientNameMapper` | `IngredientPatch.cs` | ✅ Complete | All 12 sea plants + Truffle + Rainbow Cap confirmed |
| `CharmMapper` | `CharmPatch.cs` | ✅ Complete | All 18 charms (base + Jungle DLC) confirmed from `CharmSpecData` dump |
| `QuestNameMapper` | `StoryProgressPatch.cs` | ✅ Complete | 130+ mission TIDs from `MissionDictionary` dump |
| `VIPNameMapper` | `RestaurantPatch.cs` | ✅ Complete | WangPang=9100017, Alex=9100018, Pastro=9100019 |
| `ChallengeNameMapper` | `ChallengePatch.cs` | 🟡 Low priority | Challenges removed as placeholder content |

---

## ✅ All Confirmed TIDs (2026-06-27 session)

### Fish TIDs (`CaughtFishData` dump)
Format: TID → Species, from `SA_TIDNUMBER_SpeciesName` GameObject names
- **Blue Hole Shallow:** `2010002`-`2010085` range
- **Blue Hole Mid:** `2010101`-`2010142` range
- **Blue Hole Deep:** `2010201`-`2010241` range
- **Glacial Passage:** `2010301`-`2010306`
- **Glacier Zone:** `2010401`-`2010418`
- **Hydrothermal Vents:** `2010501`-`2010512`
- **Aberrations (Jellyfish Basin):** `2011201`-`2011211`
- **Aberrations (Fog Coast):** `2011213`-`2011224`
- **Aberrations (Black Cliff):** `2011225`-`2011234`
- **Godzilla DLC:** `2012006`-`2012023`
- **Jungle Lake (upper):** `2010601`-`2010612` (base game fish in jungle)
- **Jungle Lake (DLC exclusive):** `42010102`-`42011234` range
- **Jungle Lakebed (ancient):** `42011101`-`42011120` range
- **Jungle Rod Fish:** `42013501`-`42013513` (all 12 confirmed)

### Weapon TIDs (`SubEquipmentData` dump)
Format: `3060xxx` where hundreds digit = weapon tree
```
Basic Rifle: 3060001-3060012
Net Gun: 3060101-3060106
Triple Axel: 3060201-3060210
Red Sniper Rifle: 3060301-3060306
Sticky Bomb Gun: 3060401-3060403
Grenade Launcher: 3060501-3060502
Ice Gun: 3060601-3060603
Hush Dart: 3060701-3060703
Drain Gun: 3060801-3060803
Unknown: 3060901, 3060903 (TODO: identify)
```

### Recipe TIDs (`unlockRecipeData` dump)
Format: `8050xxx` = sushi, `8051xxx` = menu dishes, `8052xxx` = grill, `8058xxx` = seasonal
- 248 total recipes confirmed

### Charm TIDs (`CharmSpecData` dump)
```
Dolphin Necklace=3017001, Sea People Bracelet=3017011,
Octopus Bracelet=3017021, Eco Health Bracelet=3017031,
Eco Poison Resist=3017041, Octopus Weapon Charm=3017042,
Sea People Necklace=3017043, Shark Teeth Necklace=3017044,
Eco Gemstone Bracelet=3017045, Eco Waterproof Bag=3017046,
Jimbo Coin=3017049, Leo Keychain=3017101,
Crocodile Tooth Necklace=43017101, Charm of Abundance=43017102,
Anti-Gravity Device=43017103, Gold Necklace of Sloth=43017104,
Bracelet of Strength=43017105, Air Resonance Necklace=43017106
```
Note: `3017047` (InteractionTimeReduce) = confirmed cut content, ignored.

### Ingredient TIDs (`IngredientsStorage` dump)
```
Sea Grape=1027101, Agar=1027102, Kajime=1027103, Kelp=1027104,
Seaweed=1027106, Black Coral=1027107, Southern Bull Kelp=1027108,
Buckbean=1027109, Bladderwrack=1027110, Hyalonema=1027111,
Truffle=1026011, Rainbow Cap=1026012
Farm: Rice=1027002, Carrot=1027001, Wheat=1027004, Bean=1027003,
Buckwheat=1027019, Garlic=1027018, Habanero=1027013,
Onion=1027011, Cucumber=1027015, Eggplant=1027016,
Egg=1027014, Grade A Egg=1027017, Cherry Tomato=1027008
```

### Mission TIDs (`MissionDictionary` dump)
- 513 total missions cleared (all confirmed)
- Base game: `100xxxxx` range
- Jungle DLC: `410xxxxx` range
- See `StoryProgressPatch.cs` `QuestNameMapper` for full mapping
- Internal types filtered: `JungleRankReward`, `JungleRelationEvent` (state machines, not player-facing)

### Boss Types (`EnumBossFishType`)
```
GiantSquid=1, HermitCrab=2, WolfFish=3, Clione=4, JW2=5,
Gardon=6, MantisShrimp=7, GoblinShark=8, Helicoprion=9,
GreatWhiteShark=10, Anomalocaris=11, Lusca=12, Ebirah=100,
Stethacanthus=201, Xiphactinus=202, Sulong=203, SnappingTurtle=204
```

### VIP Cooking TIDs
```
WangPang=9100017, Alex=9100018, Pastro=9100019
```

---

## 🔴 Still Unknown

| Item | Notes |
|---|---|
| Weapon TIDs 3060901, 3060903 | Seen in dump but unknown — possibly Jungle DLC weapons |
| Some weapon upgrade branches | Lightning/Shock/Thunderbolt variants not yet crafted |
| Fish farm area progression TIDs | Fish farm area unlocks (A-H) may need TID verification |

---

## 📋 Useful C# Console Scripts

### Dump fish in current scene
```csharp
var list = GameObject.FindObjectsOfTypeAll(Il2CppType.Of<FishInteractionBody>());
UnityExplorer.ExplorerCore.Log("Fish in scene: " + list.Length);
for (int i = 0; i < list.Length; i++)
{
    var f = list[i].Cast<FishInteractionBody>();
    UnityExplorer.ExplorerCore.Log("FISH: " + f.gameObject.name);
}
```

### Get current rod fish TID (while fishing)
```csharp
var fm = JDLC.Fishing.FishingGameManager.Instance;
var fishInfo = fm.FishingContext.FishInfo;
UnityExplorer.ExplorerCore.Log("TID: " + fishInfo.TID + " | Caught: " + fishInfo.IsCaught);
```

### Dump all cleared missions
```csharp
var mm = MissionManager.Instance;
var dict = mm.MissionDictionary;
var keys = dict.Keys.GetEnumerator();
while (keys.MoveNext())
{
    int tid = keys.Current;
    var m = dict[tid];
    UnityExplorer.ExplorerCore.Log("MISSION | TID: " + tid + " | Type: " + m.Type + " | Title: " + m.Title);
}
```

### Dump all ingredients
```csharp
var storage = IngredientsStorage.Instance;
var list = storage.GetIngredients(IngredientsType.All);
foreach (var i in list)
    UnityExplorer.ExplorerCore.Log("INGREDIENT | ID: " + i.ingredientsID + " | Name: " + i.Entity.Name);
```

---

## 📝 Session Log

| Date | What was found | Key TIDs |
|---|---|---|
| 2026-06-19 | VIP cooking TIDs from dump.cs | WangPang=9100017, Alex=9100018, Pastro=9100019 |
| 2026-06-19 | Boss types from EnumBossFishType enum | See above |
| 2026-06-26 | Rainbow Cap + farm vegetable TIDs | Rainbow Cap=1026012, Bean=1027003, etc. |
| 2026-06-26 | Full ingredient dump (235 entries) | All confirmed |
| 2026-06-26 | Jungle Insect/Skewer/Grill Recipe TIDs | 40001-40038, 48150001-48150109 |
| 2026-06-27 | All charm TIDs from CharmSpecData dump | 18 charms confirmed, cut content identified |
| 2026-06-27 | All 513 mission TIDs from MissionDictionary | Full QuestNameMapper filled |
| 2026-06-27 | All fish TIDs from CaughtFishData dump | 200+ base game + 50+ Jungle fish |
| 2026-06-27 | All weapon TIDs from SubEquipmentData | 48 weapons (3060xxx range) |
| 2026-06-27 | All 12 rod fish TIDs via FishInfo.TID | 42013501-42013513 |
| 2026-06-27 | Jungle lakebed ancient fish (17 species) | 42011101-42011120 range |
