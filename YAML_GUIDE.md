# YAML Configuration Guide - Dave the Diver

## 📋 Quick Start

1. Copy one of the example YAML files
2. Customize the options to your preference
3. Save it in your Archipelago player folder
4. Generate your multiworld!

---

## 📁 Example YAML Files Included

### **player-options-example.yaml**
- Complete template with ALL options documented
- Use this as a reference
- Default balanced settings

### **player-options-speedrun.yaml** ⚡
- Defeat Yawie ASAP (minimal checks)
- Minimal checks (~50)
- Good starting equipment
- 5-10 hour completion

### **player-options-completionist.yaml** 💯
- 100% completion goal
- ALL checks (~750+)
- Everything enabled
- 100+ hour completion

### **player-options-restaurant-focus.yaml** 🍣
- Diamond Rank goal (Cooksta + restaurant focus)
- Focus on recipes and dishes
- Cooksta and farming included
- 30-40 hour completion

### **player-options-exploration-focus.yaml** 🐟
- Master Diver goal
- Catch all fish!
- Ecowatcher and photography focus
- 50-70 hour completion

---

## ⚙️ Option Categories

### 🎯 **Goal Options**

> **Available goals:** `defeat_yawie` (default) · `defeat_all_bosses` · `diamond_rank` · `master_diver` · `hundred_percent`

**goal:**
- `defeat_yawie` - Defeat the final boss (default, 15-25h)
- `defeat_all_bosses` - Defeat Yawie + all 15 story/vortex bosses (20-35h)
- `diamond_rank` - Defeat Yawie + 720 Cooksta followers + 375 Best Taste + 32 recipes (30-45h)
- `master_diver` - Defeat Yawie + catch every fish species (40-60h)
- `hundred_percent` - All of the above (80-120h)

---

### 🐟 **Fish Options**

**fish_checks:**
- `none` - No fish catching checks
- `rare_only` - Only rare/boss fish (~40 checks)
- `all` - Every fish species ⭐ DEFAULT (~100+ checks)

**require_all_fish:**
- `true` - Must catch all fish to win (only with certain goals)
- `false` - Catching all fish is optional ⭐ DEFAULT

---

### 🍣 **Restaurant Options**

**dish_upgrades:**
- `none` - No dish upgrade checks (0)
- `key_dishes` - Important dishes only ⭐ DEFAULT (~80 checks)
- `popular` - Commonly used dishes (~200 checks)
- `all` - Every single dish (~400+ checks)

**recipe_checks:**
- `key_only` - Only progression recipes (~30 checks)
- `all` - Every recipe ⭐ DEFAULT (~100+ checks)

**require_all_recipes:**
- `true` - Must unlock all recipes to win
- `false` - Recipes are optional ⭐ DEFAULT

**restaurant_rating_required:** (0-5)
- Minimum star rating for victory (if goal includes it)
- Default: `5`

---

### 📱 **Side Content Options**

**include_cooksta:** (true/false)
- Include Cooksta social media checks
- Default: `true`

**cooksta_followers_required:** (0-10000)
- Followers needed for victory (if goal includes it)
- Default: `10000`

**include_ecowatcher:** (true/false)
- Include marine database completion
- Default: `true`

**include_photography:** (true/false)
- Include Tako's photo missions
- Default: `true`

**include_challenges:** (true/false)
- Include skill-based challenges (can be hard!)
- Default: `false`

**include_farming:** (true/false)
- Include vegetable garden
- Default: `true`

**include_fish_farm:** (true/false)
- Include fish breeding
- Default: `true`

**include_minigames:** (true/false)
- Include seahorse racing, card games, etc.
- Default: `true`

---

### ⚡ **Starting Equipment**

**starting_oxygen_level:** (0-5)
- How many oxygen upgrades to start with
- Default: `1`

**starting_harpoon_level:** (0-3)
- 0 = none, 1 = basic, 2 = enhanced, 3 = advanced
- Default: `1`

**starting_diving_suit_level:** (0-3)
- 0 = none, 1 = basic, 2 = enhanced, 3 = deep sea
- Default: `1`

---

### 💪 **Difficulty Options**

**oxygen_requirement:**
- `lenient` - Only 3 oxygen upgrades for deep areas
- `normal` - 4 oxygen upgrades needed ⭐ DEFAULT
- `strict` - 5 oxygen upgrades needed

---

### 🎲 **Special Options**

**death_link:** (true/false)
- When you die, everyone dies!
- Default: `false`

**teleport_behavior:**
- `required` - Must find teleport destinations ⭐ DEFAULT
- `fast_travel_only` - Teleports just for convenience

**trap_frequency:**
- `none` - No trap items ⭐ DEFAULT
- `low` - ~5% traps
- `medium` - ~10% traps
- `high` - ~15% traps
- (Not implemented yet)

---

## 🎯 Recommended Setups by Player Type

### **New to Archipelago?**
Use: `player-options-example.yaml`
- Goal: `all_chapters`
- Standard difficulty
- All content enabled
- 15-25 hours

### **Speedrunner?**
Use: `player-options-speedrun.yaml`
- Goal: `final_chapter_only`
- Minimal checks
- Better starting gear
- 5-10 hours

### **Love Cooking Games?**
Use: `player-options-restaurant-focus.yaml`
- Goal: `restaurant_tycoon`
- All recipes and dishes
- Cooksta included
- 30-40 hours

### **Love Exploration?**
Use: `player-options-exploration-focus.yaml`
- Goal: `master_diver`
- Catch all fish!
- Strict difficulty
- 50-70 hours

### **Completionist?**
Use: `player-options-completionist.yaml`
- Goal: `hundred_percent`
- Everything enabled
- Maximum checks
- 100+ hours

---

## 📝 Creating Your Own YAML

1. **Start with an example:**
   ```bash
   cp player-options-example.yaml my-options.yaml
   ```

2. **Edit the file:**
   - Change `name: YourName` to your player name
   - Adjust goal and options to your preference
   - Save the file

3. **Generate your world:**
   - Place YAML in Archipelago's `Players` folder
   - Run the generator
   - Join the multiworld!

---

## 💡 Tips for Configuration

### **First Time Playing Dave the Diver?**
- Use `goal: all_chapters`
- Keep most options at default
- Start with `fish_checks: all` to experience the fishing
- Use `dish_upgrades: key_dishes` to avoid overwhelming recipe counts

### **Playing with Friends?**
- Coordinate goals! Mix speedrunners with completionists
- Enable `death_link: true` for extra challenge
- Use similar difficulty settings for balanced progression

### **Short on Time?**
- Use `final_chapter_only` goal
- Set `fish_checks: rare_only`
- Set `dish_upgrades: none`
- Increase starting equipment levels

### **Want Maximum Content?**
- Use `hundred_percent` goal
- Set everything to `all`
- Enable ALL side content
- Prepare for 100+ hours!

---

## 🔧 Troubleshooting

**"Too many/few checks"**
- Adjust `fish_checks` and `dish_upgrades` to control check density
- Use `none`/`key_only` for fewer checks
- Use `all` for more checks

**"Too hard/easy"**
- Adjust `oxygen_requirement`
- Change `starting_*_level` values
- Modify `goal` for different completion times

**"Missing specific content"**
- Check `include_*` toggles
- Some content only matters for certain goals

**"YAML syntax error"**
- Make sure indentation is correct (use spaces, not tabs)
- Check that options match exactly (case-sensitive)
- Use the example files as templates

---

## 🚀 Quick Reference

```yaml
name: YourName
game: Dave the Diver

Dave the Diver:
  goal: all_chapters             # Your victory condition
  fish_checks: all               # none/rare_only/all
  dish_upgrades: key_dishes      # none/key_dishes/popular/all
  include_cooksta: true          # Include social media?
  starting_oxygen_level: 1       # 0-5
  oxygen_requirement: normal     # lenient/normal/strict
  death_link: false              # When you die, everyone dies
```

---

**Happy diving! 🌊🍣🎮**
