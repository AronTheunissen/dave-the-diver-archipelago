# New Features - Additional Game Systems

## Overview

Dave the Diver has MANY interconnected systems beyond just diving and restaurant management. This document explains how we're integrating them as Archipelago location checks.

---

## 🍕 Cooksta (Social Media App)

**What it is:** In-game social media app where you post dishes and gain followers

**Why it's a great check:**
- Core progression system
- Unlocks special customers and rewards
- Natural milestones (follower counts)
- Ties directly to restaurant success

**Location checks:**
- Follower milestones (100, 500, 1000, 2500, 5000, 10000)
- Post count milestones (10, 25, 50 posts)
- Special achievements (first viral post, max likes)

**Implementation:**
```csharp
[HarmonyPatch(typeof(CookstaManager), "OnFollowerGained")]
static void Postfix(int newFollowerCount)
{
    // Check milestones: 100, 500, 1000, etc.
    if (newFollowerCount >= 1000 && !GameState.HasChecked("Cooksta_1000"))
    {
        APClient.CheckLocation(GetLocationId("Cooksta: 1000 Followers"));
    }
}
```

**Total locations:** ~15

---

## 🐠 Ecowatcher (Marine Life Database)

**What it is:** App that catalogs all fish and marine life (Marinca)

**Why it's a great check:**
- Encourages exploration and documentation
- Natural collection mechanic
- Separate from just catching fish (requires photographing/scanning)
- Has category completion milestones

**Location checks:**
- Complete all fish in a region (Shallow, Mid, Deep, Glacier)
- Marinca logging milestones (25, 50, 100 entries)
- Fish species milestones (50, 100, 150 species)
- 100% completion

**Implementation:**
```csharp
[HarmonyPatch(typeof(EcowatcherManager), "OnMarincaLogged")]
static void Postfix(MarincaData marinca)
{
    int totalLogged = GameState.GetMarincaCount();
    if (totalLogged == 50)
    {
        APClient.CheckLocation(GetLocationId("Ecowatcher: Log 50 Marinca"));
    }
}
```

**Total locations:** ~12

---

## 📸 Photography (Tako's Missions)

**What it is:** Tako gives you photography missions to capture specific scenes/creatures

**Why it's a great check:**
- Unique gameplay mechanic
- Requires skill and exploration
- Each mission is a distinct objective
- Special photo spots are hidden collectibles

**Location checks:**
- Complete each Tako photography mission
- Special photo spot discoveries
- Photography milestones (total photos taken)
- Perfect score achievements

**Implementation:**
```csharp
[HarmonyPatch(typeof(PhotographyManager), "OnMissionComplete")]
static void Postfix(int missionId, int score)
{
    long locationId = GetLocationId($"Photography: Complete Mission {missionId}");
    APClient.CheckLocation(locationId);
}
```

**Total locations:** ~12-20 (depends on number of missions)

---

## 🎯 Challenges

**What it is:** In-game challenges and special objectives

**Why it's a great check:**
- Tests player skill
- Optional but rewarding content
- Clear success criteria
- Adds variety to location types

**Location checks:**
- Time attack challenges (catch X fish in Y seconds)
- Combat challenges (defeat enemies without damage)
- Weapon-specific challenges (harpoon only, melee only)
- Restaurant challenges (perfect timing serves)
- Survival challenges (no oxygen refills, no damage)

**Implementation:**
```csharp
[HarmonyPatch(typeof(ChallengeManager), "OnChallengeComplete")]
static void Postfix(string challengeId)
{
    long locationId = GetLocationId($"Challenge: {challengeId}");
    APClient.CheckLocation(locationId);
}
```

**Total locations:** ~10-20 (expandable based on what challenges exist)

---

## 🌱 Farming (Vegetable Garden)

**What it is:** Grow vegetables and ingredients for restaurant dishes

**Why it's a great check:**
- Core progression system
- Unlocks better recipes
- Multiple crop types to discover
- Upgrade progression

**Location checks:**
- Unlock vegetable garden
- Garden upgrade tiers (1, 2, 3)
- First harvest of each crop type
- Harvest count milestones
- Grow all crop types completion

**Implementation:**
```csharp
[HarmonyPatch(typeof(FarmingManager), "OnCropHarvested")]
static void Postfix(CropData crop)
{
    // Check for first harvest of this crop type
    if (!GameState.HasHarvested(crop.cropId))
    {
        GameState.MarkHarvested(crop.cropId);
        APClient.CheckLocation(GetLocationId($"Farming: First Harvest - {crop.name}"));
    }
}
```

**Total locations:** ~15-20

---

## 🐟 Fish Farm

**What it is:** Raise and breed fish in tanks for the restaurant

**Why it's a great check:**
- Alternative to catching wild fish
- Breeding/raising mechanic
- Multiple species to farm
- Tank upgrade progression

**Location checks:**
- Unlock fish farm
- Tank upgrade tiers
- First successful breed of each species
- Raise X fish to adulthood milestones
- Species diversity milestones
- Quality achievements

**Implementation:**
```csharp
[HarmonyPatch(typeof(FishFarmManager), "OnFishMatured")]
static void Postfix(FishFarmData fish)
{
    // Check first breed of this species
    if (!GameState.HasBred(fish.speciesId))
    {
        GameState.MarkBred(fish.speciesId);
        APClient.CheckLocation(GetLocationId($"Fish Farm: First Breed - {fish.name}"));
    }
    
    // Check total raised milestone
    int totalRaised = GameState.GetTotalFishRaised();
    if (totalRaised == 25)
    {
        APClient.CheckLocation(GetLocationId("Fish Farm: Raise 25 Fish to Adulthood"));
    }
}
```

**Total locations:** ~16-20

---

## 📊 Location Count Impact

### Before adding these systems: ~90 locations

### After adding these systems: ~150 locations defined

### Potential with full expansion:

| Category | Current | Potential |
|----------|---------|-----------|
| Fish (first catch) | 19 | 100+ |
| Dish upgrades | 16 | 400+ |
| Recipe unlocks | 5 | 100+ |
| Story/Bosses/Quests | 15 | 30+ |
| **Cooksta** | **15** | **20** |
| **Ecowatcher** | **12** | **15** |
| **Photography** | **12** | **20** |
| **Challenges** | **10** | **20** |
| **Farming** | **15** | **20** |
| **Fish Farm** | **16** | **25** |
| Minigames | 4 | 15 |
| Collectibles | 4 | 30 |
| Achievements | 8 | 20 |
| **TOTAL** | **~150** | **~815+** |

---

## 🎮 Gameplay Impact

These systems make the randomizer feel more integrated with Dave the Diver because:

1. **Every major game system is represented**
   - Not just "catch fish, serve sushi"
   - Side activities become meaningful

2. **Encourages diverse gameplay**
   - Can't just grind one activity
   - Need to engage with farming, photography, etc.

3. **Natural progression gates**
   - Fish farm requires certain fish species
   - Farming requires garden unlocks
   - Photography requires exploring specific areas

4. **Completionist content**
   - Ecowatcher gives reason to document everything
   - Cooksta gives reason to try all dishes
   - Challenges test your skills

---

## 🔧 YAML Options

Recommend making these configurable:

```yaml
dave_the_diver:
  include_cooksta: true       # Default: true
  include_ecowatcher: true    # Default: true
  include_photography: true   # Default: true
  include_challenges: true    # Default: true (skip for casual players)
  include_farming: true       # Default: true
  include_fish_farm: true     # Default: true
  
  # Difficulty options
  challenges_difficulty:
    easy: 0
    normal: 1     # Default
    hard: 2
    expert: 3     # Includes time attacks and no-damage runs
```

---

## 💡 Implementation Priority

### Phase 1 (MVP)
- ✅ Fish first catch
- ✅ Dish upgrades
- ✅ Story progression
- Skip these for MVP

### Phase 2 (Full Release)
- ✅ Cooksta milestones
- ✅ Farming basics
- ✅ Fish farm basics
- ✅ Photography missions (at least main ones)

### Phase 3 (Completionist)
- ✅ All Ecowatcher entries
- ✅ All challenges
- ✅ All photography spots
- ✅ All farming crops
- ✅ All fish farm species

---

## 🎯 Why This Makes Dave the Diver Perfect for Archipelago

Most games focus on ONE main progression system. Dave the Diver has:

1. **Diving** (exploration, combat, fishing)
2. **Restaurant** (cooking, serving, management)
3. **Social Media** (Cooksta followers, posts)
4. **Research** (Ecowatcher database)
5. **Photography** (Tako's missions)
6. **Farming** (vegetable garden)
7. **Aquaculture** (fish farm)
8. **Minigames** (seahorse racing, card games)
9. **Story** (6 chapters, multiple NPCs)

**= 9 interconnected systems = TONS of meaningful location checks!**

This gives us 750+ potential locations without ANY feeling forced or arbitrary. Every check is a real milestone in the game.

---

**Dave the Diver is basically MADE for Archipelago!** 🎮🌊🍣
