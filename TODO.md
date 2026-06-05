# Dave the Diver Archipelago - TODO List

## 🎯 What You Can Do RIGHT NOW (No Prerequisites Needed)

### 1. Game Analysis ⭐ **START HERE**

**Goal:** Document all items and locations in Dave the Diver

**How:**
1. Open `GAME_ANALYSIS_TEMPLATE.md`
2. Watch Dave the Diver playthroughs on YouTube
   - Search: "Dave the Diver 100% playthrough"
   - Or "Dave the Diver all recipes"
   - Or "Dave the Diver complete guide"
3. Check the Dave the Diver Wiki: https://dave-the-diver.fandom.com/
4. Fill in the template sections:
   - Weapons and equipment
   - All recipes (there are 100+!)
   - Story chapters
   - Boss fish
   - Side quests
   - Minigames

**Output:** Completed analysis with 150-300 items and locations identified

**Time Estimate:** 4-6 hours of research

---

### 2. Study Existing APWorlds

**Goal:** Learn how other games implement Archipelago

**How:**
1. Open `tools/Archipelago/worlds/`
2. Study these similar games:

**Stardew Valley** (Best reference - similar gameplay):
```
tools/Archipelago/worlds/stardew_valley/
├── __init__.py          # See how they structure the world
├── items.py             # ~400 items defined
├── locations.py         # ~400 locations
├── regions.py           # Farm, Town, Mines, etc.
├── rules.py             # Complex logic rules
└── options.py           # Many YAML options
```

**Subnautica** (Underwater exploration):
```
tools/Archipelago/worlds/subnautica/
├── __init__.py
├── items.py             # Equipment and tech
├── locations.py         # Databoxes, wrecks
└── rules.py             # Depth-based progression
```

**What to look for:**
- How they categorize items (progression vs useful vs filler)
- How they define regions and connections
- How they write logic rules
- How they handle crafting/recipes

**Time Estimate:** 2-3 hours

---

### 3. Expand Items List

**Goal:** Go from ~50 items to 150-300 items

**How:**
1. Open `apworld/davethediver/items.py`
2. Based on your game analysis, add:
   - All weapons (harpoons, tips, melee)
   - All diving equipment (oxygen, cargo, suits)
   - All recipes (100+ in the game!)
   - All restaurant upgrades
   - All staff unlocks
   - Key story items
   - Filler items (gold, materials)

**Example additions:**
```python
# Add to weapon_items
"Poison Harpoon Tip": ItemData(BASE_ID + 15, ItemClassification.useful),
"Triple Axe Harpoon Tip": ItemData(BASE_ID + 16, ItemClassification.useful),

# Add all recipes
recipe_items: Dict[str, ItemData] = {
    "Recipe: Tuna Nigiri": ItemData(BASE_ID + 250, ItemClassification.useful),
    "Recipe: Salmon Roll": ItemData(BASE_ID + 251, ItemClassification.useful),
    "Recipe: Premium Sushi Set": ItemData(BASE_ID + 252, ItemClassification.useful),
    # ... 100+ more recipes
}
```

**Time Estimate:** 3-4 hours

---

### 4. Expand Locations List

**Goal:** Go from ~40 locations to 150-300 locations

**How:**
1. Open `apworld/davethediver/locations.py`
2. Based on your game analysis, add:
   - All story chapter completions
   - All boss defeats
   - Notable fish catches
   - Recipe unlocks
   - Restaurant milestones
   - Side quest completions
   - Minigame victories
   - Treasure chests
   - Achievement milestones

**Example additions:**
```python
# More fish locations
"Catch Blue Marlin": LocationData(BASE_ID + 104, "Blue Hole - Mid"),
"Catch Giant Tuna": LocationData(BASE_ID + 105, "Blue Hole - Deep"),
"Catch Manta Ray": LocationData(BASE_ID + 106, "Blue Hole - Mid"),

# Recipe locations
"Unlock Recipe: Tuna Nigiri": LocationData(BASE_ID + 250, "Bancho Sushi"),
"Unlock Recipe: Salmon Roll": LocationData(BASE_ID + 251, "Bancho Sushi"),

# Quest locations
"Complete Duff's Quest 1": LocationData(BASE_ID + 401, "Blue Hole"),
"Complete Dr. Bacon's Quest 1": LocationData(BASE_ID + 402, "Blue Hole"),
```

**Time Estimate:** 3-4 hours

---

### 5. Read Archipelago Documentation

**Goal:** Understand how Archipelago works

**Resources:**
1. **Archipelago Website:** https://archipelago.gg/
   - Read "How it Works"
   - Check supported games
   
2. **GitHub Wiki:** https://github.com/ArchipelagoMW/Archipelago/wiki
   - World Development Guide
   - Logic Documentation
   
3. **Join Discord:** https://discord.gg/archipelago
   - Browse #apworld-development
   - See how others ask questions
   - Look for similar games

**Time Estimate:** 1-2 hours

---

### 6. Plan Regions and Logic

**Goal:** Design how your game areas connect

**How:**
1. Create a document mapping regions:
```
Menu
  └─> Bancho Sushi
       ├─> Blue Hole - Shallow (no requirements)
       │    └─> Blue Hole - Mid (requires: Oxygen +2 OR Enhanced Suit)
       │         └─> Blue Hole - Deep (requires: Advanced Harpoon + Deep Suit + Oxygen +4)
       ├─> Glacier (requires: Cold Protection Suit + Chapter 4 Complete)
       └─> Sea People Village (requires: VIP Card + Chapter 3 Complete)
```

2. Document what items are needed to access each region
3. Document what items are needed to check each location

**Time Estimate:** 2-3 hours

---

### 7. Write Design Documents

**Goal:** Document your design decisions

**How:**
1. Update `docs/DESIGN.md` with:
   - Final item/location counts
   - Region structure
   - Logic rules
   - YAML options you'll implement
   
2. Create a roadmap document:
   - What features to implement first
   - What can wait for v1.1
   - Known challenges

**Time Estimate:** 1-2 hours

---

### 8. Set Up Development Tools (Optional)

**If you want to start coding, even without .NET:**

**Install VS Code:**
1. Download: https://code.visualstudio.com/
2. Install Python extension
3. Open the `dave-the-diver-archipelago` folder

**Configure Python in VS Code:**
1. Press `Ctrl+Shift+P`
2. Type "Python: Select Interpreter"
3. Choose `.\apworld\venv\Scripts\python.exe`

**Now you can:**
- Edit Python files with IntelliSense
- See syntax errors immediately
- Use integrated terminal

**Time Estimate:** 30 minutes

---

## 📊 Recommended Work Order

### Phase 1: Research (Can do now!)
1. ✅ Game analysis (4-6 hours) ⭐ **PRIORITY**
2. ✅ Study existing APWorlds (2-3 hours)
3. ✅ Read Archipelago docs (1-2 hours)
4. ✅ Plan regions and logic (2-3 hours)

**Total: ~10-14 hours** - Can complete this weekend!

### Phase 2: Python Implementation (Need Python setup)
5. Expand items list (3-4 hours)
6. Expand locations list (3-4 hours)
7. Create regions.py (2-3 hours)
8. Create rules.py (3-4 hours)
9. Create options.py (1-2 hours)
10. Update __init__.py (2-3 hours)

**Total: ~14-20 hours** - Next week

### Phase 3: C# Client (Need .NET SDK + BepInEx)
11. Set up C# project (1 hour)
12. Implement Archipelago client (4-6 hours)
13. Create Harmony patches (10-15 hours)
14. Test integration (3-5 hours)

**Total: ~18-27 hours** - Week after

### Phase 4: Testing & Polish
15. Generate test seeds (2 hours)
16. Solo playthrough (6-8 hours)
17. Fix bugs (5-10 hours)
18. Multiworld testing (3-5 hours)

**Total: ~16-25 hours** - Final week

---

## 🎯 This Week's Goal

**Complete Phase 1: Research**

By end of this week, you should have:
- [ ] Complete game analysis template filled out
- [ ] 150-300 items identified and categorized
- [ ] 150-300 locations identified and categorized
- [ ] Region structure planned
- [ ] Logic requirements documented
- [ ] Understanding of how Archipelago works

**This gives you everything needed to implement Phase 2!**

---

## 💡 Pro Tips

1. **Don't try to be perfect** - Start with 80% coverage, you can add more later
2. **Focus on progression first** - What's required to beat the game?
3. **Use the wiki liberally** - Don't spend hours in-game when info is online
4. **Take notes while researching** - You'll forget details
5. **Commit often** - After each section of analysis, commit to git

---

## 🤔 Questions to Answer During Research

- How many main story chapters are there?
- What's the final boss/goal?
- Which equipment unlocks new areas?
- Are there multiple endings?
- What recipes are required vs optional?
- How many total fish species exist?
- What's the deepest dive depth?
- Are there any sequence breaks or skips?
- What's the minimum equipment to beat the game?

---

## ✅ Success Criteria

You're ready for Phase 2 when you can answer:
- ✓ "How many items will be in the randomizer?" (150-300)
- ✓ "What are the main progression items?" (List them)
- ✓ "How do players access each region?" (Requirements)
- ✓ "What's the victory condition?" (Beat chapter 6? Catch all fish?)
- ✓ "How will we handle recipes?" (All required? Some optional?)

---

**Start with GAME_ANALYSIS_TEMPLATE.md and you're on your way!** 🎮
