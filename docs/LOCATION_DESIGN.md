# Location Design - Dave the Diver

## Philosophy

Locations should feel **meaningful** and track **actual progression** in the game.

## Location Categories

### 1. Fish - First Catch ✅ **IMPLEMENTED**

**Concept:** Checking a location when you catch a fish species for the first time.

**Why this works:**
- Encourages exploring different depths and areas
- Natural progression as you unlock better equipment
- Fits the core gameplay loop
- Can scale from 50-100+ locations depending on how many fish we include

**Implementation in client mod:**
```csharp
// In FishCatchPatch.cs
[HarmonyPatch(typeof(FishManager), "OnFishCaught")]
static void Postfix(FishData fish)
{
    // Check if this is first time catching this species
    if (!GameStateManager.HasCaughtFish(fish.fishId))
    {
        GameStateManager.MarkFishCaught(fish.fishId);
        
        long locationId = GetLocationIdForFirstCatch(fish.fishId);
        APClient.CheckLocation(locationId);
        
        Debug.Log($"First catch: {fish.fishName}!");
    }
}
```

**Categories:**
- Common fish (20-30 species) - Easy to find
- Rare fish (30-50 species) - Require specific depths/locations
- Boss fish (5-10 species) - Major encounters

**Total: ~60-90 locations**

---

### 2. Dish Upgrades ✅ **IMPLEMENTED**

**Concept:** Checking a location when you level up a sushi dish.

**Why this works:**
- Core restaurant gameplay mechanic
- Requires collecting ingredients and serving customers
- Each dish has 4-5 upgrade levels
- Encourages using diverse recipes

**Implementation in client mod:**
```csharp
// In RecipeUpgradePatch.cs
[HarmonyPatch(typeof(RecipeManager), "UpgradeRecipe")]
static void Postfix(RecipeData recipe, int newLevel)
{
    long locationId = GetLocationIdForDishUpgrade(recipe.recipeId, newLevel);
    APClient.CheckLocation(locationId);
    
    Debug.Log($"Upgraded {recipe.recipeName} to level {newLevel}!");
}
```

**Scaling options:**
- **Conservative:** Only track upgrades for key dishes (20-30 dishes × 4 levels = 80-120 locations)
- **Moderate:** Track ~50 popular dishes (50 × 4 = 200 locations)
- **Comprehensive:** Track all 100+ dishes (100 × 4 = 400+ locations)

**Recommendation:** Start conservative, make comprehensive optional via YAML

---

### 3. Recipe Unlocks

**Concept:** Checking a location when you unlock a new recipe.

**Why this works:**
- Direct progression mechanic
- Usually unlocked by catching specific fish or story events
- Clear milestone

**Total: ~100+ locations** (one per recipe in the game)

---

### 4. Other Location Types

#### Story Progression (~15 locations)
- Chapter completions
- Key story events
- Character meetings

#### Boss Battles (~10 locations)
- Major boss defeats
- Mini-boss encounters

#### Side Quests (~30-40 locations)
- NPC quest completions
- Special event completions

#### Collectibles (~20-30 locations)
- Treasure chests
- Hidden items
- Photo spots

#### Milestones (~30-40 locations)
- Serve X customers
- Earn X gold
- Catch X fish species
- Achieve X star rating

---

## Design Decisions

### Should Every Fish Be a Location?

**Pros:**
- Lots of content (100+ fish species)
- Encourages completionism
- Easy to track

**Cons:**
- Many fish are functionally identical
- Could make item pool too diluted
- Common fish are not meaningful checks

**Decision:** ✅ **YES - First catch only**

Make it optional via YAML:
- `fish_checks: "none"` - No fish checks
- `fish_checks: "rare_only"` - Only rare/boss fish (~40 locations)
- `fish_checks: "all"` - All fish species (~100+ locations)

**Default:** `"all"` - Embrace the fishing aspect of the game!

---

### Should Every Dish Upgrade Be a Location?

**Pros:**
- Core restaurant mechanic
- Natural progression
- Can create 400+ locations
- Incentivizes upgrading diverse dishes

**Cons:**
- VERY large location pool
- Some dishes are never used
- Might feel grindy

**Decision:** ✅ **YES - But make it configurable**

Make it optional via YAML:
- `dish_upgrades: "none"` - No dish upgrade checks
- `dish_upgrades: "key_dishes"` - Only ~20 important dishes (~80 locations)
- `dish_upgrades: "popular"` - ~50 commonly used dishes (~200 locations)
- `dish_upgrades: "all"` - All 100+ dishes (~400+ locations)

**Default:** `"key_dishes"` - Balance between content and grind

---

### Recipe Unlocks vs Dish Upgrades

**Question:** Should these be separate or combined?

**Decision:** ✅ **SEPARATE**

- **Recipe unlock** = You discover the recipe (can now make it)
- **Dish upgrade** = You improve an existing recipe (better quality/price)

Both are distinct progression systems in Dave the Diver.

---

## Total Location Count Estimates

### Conservative Build (Beginner-friendly)
- Story: 15
- Boss battles: 10
- Rare fish first catch: 40
- Key dish upgrades: 80
- Recipe unlocks: 50 (key recipes only)
- Quests: 30
- Milestones: 25
- Collectibles: 20
**Total: ~270 locations**

### Moderate Build (Recommended)
- Story: 15
- Boss battles: 10
- All fish first catch: 100
- Popular dish upgrades: 200
- All recipe unlocks: 100
- Quests: 40
- Milestones: 35
- Collectibles: 30
**Total: ~530 locations**

### Comprehensive Build (Completionist)
- Story: 15
- Boss battles: 10
- All fish first catch: 100
- All dish upgrades: 400
- All recipe unlocks: 100
- All quests: 50
- All milestones: 40
- All collectibles: 35
**Total: ~750 locations**

---

## YAML Options to Implement

```yaml
# Example player YAML configuration

dave_the_diver:
  # Fish catching checks
  fish_checks:
    none: 0
    rare_only: 1
    all: 2  # Default
    
  # Dish upgrade checks
  dish_upgrades:
    none: 0
    key_dishes: 1  # Default - ~20 important dishes
    popular: 2     # ~50 dishes
    all: 3         # All 100+ dishes
    
  # Recipe unlocks
  recipe_checks:
    key_only: 0    # Only story/progression recipes
    all: 1         # Default - all recipes
    
  # Other options
  include_minigames: true
  include_collectibles: true
  require_all_recipes: false  # For victory condition
  require_all_fish: false     # For victory condition
```

---

## Implementation Priority

### Phase 1 (MVP)
1. ✅ First catch for rare/boss fish (~40 locations)
2. ✅ Key dish upgrades (~80 locations)
3. ✅ Story progression (~15 locations)
4. ✅ Boss battles (~10 locations)
**Total: ~145 locations** - Enough for playable seed

### Phase 2 (Full Release)
1. All fish first catch (~100 locations)
2. All recipe unlocks (~100 locations)
3. All quests (~40 locations)
4. All milestones (~35 locations)
**Total: ~420 locations** - Full experience

### Phase 3 (Completionist)
1. All dish upgrades (~400 locations)
2. All collectibles (~35 locations)
**Total: ~850+ locations** - For hardcore players

---

## Client Mod Implementation Notes

### Tracking First Catches

The mod needs to persist which fish have been caught:

```csharp
// In GameStateManager.cs
public class FishTracker
{
    private HashSet<int> caughtFishIds = new HashSet<int>();
    
    public bool HasCaughtFish(int fishId) => caughtFishIds.Contains(fishId);
    
    public void MarkFishCaught(int fishId)
    {
        caughtFishIds.Add(fishId);
        SaveToFile();
    }
}
```

### Tracking Dish Upgrades

Track highest level achieved for each dish:

```csharp
public class DishTracker
{
    private Dictionary<int, int> dishLevels = new Dictionary<int, int>();
    
    public void OnDishUpgraded(int recipeId, int newLevel)
    {
        if (!dishLevels.ContainsKey(recipeId) || dishLevels[recipeId] < newLevel)
        {
            dishLevels[recipeId] = newLevel;
            CheckLocationForLevel(recipeId, newLevel);
        }
    }
}
```

---

## Final Recommendation

**Start with ~150-200 locations for MVP**, focusing on:
- First catch for rare/interesting fish
- Key dish upgrades
- Story progression
- Major milestones

**Then expand to 400-500 locations** for full release:
- All fish first catch
- All recipe unlocks
- All quests

**Make 800+ location builds optional** via YAML for completionists.

This gives a good progression curve and allows players to customize their experience!
