# Victory Conditions - Dave the Diver

## 🎯 Multiple Ways to Win!

Dave the Diver has SO much content that a one-size-fits-all goal doesn't work. Choose your victory condition based on how much time you want to invest!

---

## 🏆 Victory Condition Options

### 1. **Complete Final Chapter Only** ⚡ (Speedrun)

**Goal:** Beat Chapter 6

**Requirements:**
- Complete Chapter 6 (final chapter)

**Estimated Time:** 5-10 hours

**Best for:**
- New Archipelago players
- Quick multiworld sessions
- Racing/speedrunning
- Testing seeds

**What you'll do:**
- Rush to unlock deep diving
- Get minimum equipment needed
- Beat final chapter ASAP
- Skip most side content

**Challenge:** Can you find the optimal route?

---

### 2. **Complete All Chapters** ⭐ (Recommended - Default)

**Goal:** Beat all 6 chapters in any order

**Requirements:**
- Complete Chapter 1
- Complete Chapter 2
- Complete Chapter 3
- Complete Chapter 4
- Complete Chapter 5
- Complete Chapter 6

**Estimated Time:** 15-25 hours

**Best for:**
- Standard Archipelago experience
- Balanced gameplay
- First-time Dave the Diver players
- Multiworld with friends

**What you'll do:**
- Explore all major areas
- Experience full story
- Unlock all regions
- Mix diving and restaurant gameplay

**Challenge:** Chapters can be done in any order based on what items you find!

---

### 3. **Complete Main Story + Cooksta** 📱 (Social Media Mogul)

**Goal:** All chapters + 10,000 Cooksta followers

**Requirements:**
- Complete all 6 chapters
- Reach 10,000 Cooksta followers

**Estimated Time:** 30-40 hours

**Best for:**
- Restaurant management fans
- Completionists (lite)
- Players who enjoy the social media mechanic

**What you'll do:**
- Complete full story
- Master the restaurant
- Post dishes to Cooksta
- Unlock and upgrade many recipes
- Build follower base

**Challenge:** Balancing diving for ingredients with running a successful restaurant!

---

### 4. **Restaurant Tycoon** 🍣 (Master Chef)

**Goal:** All chapters + 5-star restaurant + all key recipes

**Requirements:**
- Complete all 6 chapters
- Achieve 5-star restaurant rating
- Unlock all key recipes (configurable)
- (Optional) Upgrade key dishes to max level

**Estimated Time:** 30-40 hours

**Best for:**
- Restaurant simulation fans
- Players who love cooking mechanics
- Recipe collectors

**What you'll do:**
- Complete story
- Perfect restaurant operations
- Unlock every important recipe
- Serve high-quality dishes
- Manage staff effectively
- Keep customers happy

**Challenge:** 5-star rating requires consistent excellent service!

---

### 5. **Master Diver** 🐟 (Ocean Explorer)

**Goal:** All chapters + complete Ecowatcher + catch all fish

**Requirements:**
- Complete all 6 chapters
- Complete all Ecowatcher entries
- Catch all fish species (first time)
- (Optional) Catch all Marinca (marine life)

**Estimated Time:** 50-70 hours

**Best for:**
- Completionists
- Pokemon-style "catch 'em all" fans
- Exploration lovers
- Players who enjoy the diving more than restaurant

**What you'll do:**
- Complete story
- Explore every depth
- Visit all regions
- Catch every fish species
- Document all marine life
- Find rare spawns

**Challenge:** Some fish are very rare or depth-specific!

---

### 6. **100% Completion** 💯 (Ultimate Challenge)

**Goal:** EVERYTHING

**Requirements:**
- Complete all 6 chapters
- Catch all fish species (100+)
- Unlock all recipes (100+)
- Complete all Ecowatcher entries
- Max Cooksta followers (10,000+)
- 5-star restaurant rating
- Complete all photography missions
- Harvest all crop types
- Breed all fish farm species
- Complete all side quests
- Beat all minigames
- Complete all challenges

**Estimated Time:** 100+ hours

**Best for:**
- Hardcore completionists
- Long-term multiworld campaigns
- "I want to see everything" players
- Streamers/content creators

**What you'll do:**
- EVERYTHING in Dave the Diver
- Every system maxed out
- Every location checked
- True 100% completion

**Challenge:** This is a MASSIVE undertaking!

---

## 📊 Victory Condition Comparison

| Goal | Time | Difficulty | Locations | Items Needed | Best For |
|------|------|------------|-----------|--------------|----------|
| **Final Chapter Only** | 5-10h | Easy | ~30-50 | Core progression only | Speedrun |
| **All Chapters** ⭐ | 15-25h | Medium | ~100-150 | Most progression items | Standard play |
| **Chapters + Cooksta** | 30-40h | Medium-Hard | ~150-200 | Progression + many recipes | Restaurant fans |
| **Restaurant Tycoon** | 30-40h | Medium-Hard | ~150-250 | Progression + all recipes | Chef roleplay |
| **Master Diver** | 50-70h | Hard | ~200-300 | Everything except recipes | Collection fans |
| **100% Completion** | 100+ | Very Hard | ~400-750 | EVERYTHING | Completionists |

---

## ⚙️ YAML Configuration

### Example configurations:

**Speedrun setup:**
```yaml
dave_the_diver:
  goal: final_chapter_only
  fish_checks: rare_only
  dish_upgrades: none
  include_challenges: false
  starting_oxygen_level: 2
  starting_harpoon_level: 2
```

**Standard play (default):**
```yaml
dave_the_diver:
  goal: all_chapters
  fish_checks: all
  dish_upgrades: key_dishes
  recipe_checks: all
  include_cooksta: true
  include_photography: true
```

**Completionist:**
```yaml
dave_the_diver:
  goal: hundred_percent
  fish_checks: all
  dish_upgrades: all
  recipe_checks: all
  require_all_fish: true
  require_all_recipes: true
  restaurant_rating_required: 5
  cooksta_followers_required: 10000
  include_ecowatcher: true
  include_photography: true
  include_challenges: true
  include_farming: true
  include_fish_farm: true
```

**Restaurant focus:**
```yaml
dave_the_diver:
  goal: restaurant_tycoon
  fish_checks: rare_only  # Only what you need for recipes
  dish_upgrades: popular  # Many dishes to upgrade
  recipe_checks: all
  require_all_recipes: true
  restaurant_rating_required: 5
  include_cooksta: true
```

---

## 🎮 How Goals Affect Item Pool

### Final Chapter Only:
- Fewer checks = faster completion
- Focus on depth progression items
- Skip most side content items

### All Chapters:
- Balanced item pool
- All core progression items
- Some side content items

### Extended Goals:
- More checks = more items needed
- All progression items
- Many useful/filler items
- Side content becomes important

---

## 💡 Custom Goal (Advanced)

You can also create custom requirements:

```yaml
dave_the_diver:
  goal: all_chapters  # Base goal
  chapters_required: 6
  
  # Add your own requirements
  require_all_fish: false
  require_all_recipes: true
  restaurant_rating_required: 4
  cooksta_followers_required: 5000
  
  # Include what you want
  include_challenges: true
  include_farming: true
```

---

## 🏁 Victory Condition Implementation

### In rules.py:

```python
def set_completion_condition(world):
    """Set victory condition based on player options"""
    player = world.player
    goal = world.options.goal.value
    
    if goal == 0:  # Final chapter only
        world.multiworld.completion_condition[player] = lambda state: \
            state.has("Chapter 6 Complete", player)
    
    elif goal == 1:  # All chapters
        world.multiworld.completion_condition[player] = lambda state: (
            state.has("Chapter 1 Complete", player) and
            state.has("Chapter 2 Complete", player) and
            state.has("Chapter 3 Complete", player) and
            state.has("Chapter 4 Complete", player) and
            state.has("Chapter 5 Complete", player) and
            state.has("Chapter 6 Complete", player)
        )
    
    elif goal == 2:  # Chapters + Cooksta
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            state.has("Cooksta: 10000 Followers", player)
        )
    
    elif goal == 3:  # Restaurant Tycoon
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            state.has("Restaurant Rating: 5 Stars", player) and
            has_all_key_recipes(state, player)
        )
    
    elif goal == 4:  # Master Diver
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            state.has("Ecowatcher: Complete All Fish", player) and
            has_all_fish_caught(state, player)
        )
    
    elif goal == 5:  # 100% Completion
        world.multiworld.completion_condition[player] = lambda state: (
            has_all_chapters(state, player) and
            has_all_fish_caught(state, player) and
            has_all_recipes(state, player) and
            state.has("Ecowatcher: Complete All Marinca", player) and
            state.has("Cooksta: 10000 Followers", player) and
            state.has("Restaurant Rating: 5 Stars", player) and
            has_all_photography_missions(state, player) and
            has_all_farming_crops(state, player) and
            has_all_fish_farm_species(state, player)
        )
```

---

## 🎯 Recommended for Different Players

**New to Archipelago?** → All Chapters  
**Want quick game?** → Final Chapter Only  
**Love restaurants?** → Restaurant Tycoon  
**Love exploration?** → Master Diver  
**Want everything?** → 100% Completion  
**Racing with friends?** → Final Chapter Only (same seed!)  
**Long campaign?** → 100% Completion  

---

**The beauty of multiple victory conditions: Play Dave the Diver YOUR way!** 🎮🌊🍣
