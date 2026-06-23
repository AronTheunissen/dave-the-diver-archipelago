# Dave the Diver — Archipelago Randomizer

An [Archipelago](https://archipelago.gg/) multiworld randomizer integration for **Dave the Diver** (Steam, by MINTROCKET).

## Project Status

| Component | Status |
|---|---|
| APWorld (Python) | ✅ Complete — 1,300+ locations, 300+ items, 55 tests passing |
| C# Client Mod | ✅ Complete — 17 patches, all game API calls implemented |
| Reverse Engineering | ✅ dump.cs analysed — all class/method names confirmed |
| Player Setup Guide | ✅ See `docs/SETUP_GUIDE.md` |
| Unit Tests | ✅ 55/55 passing |
| In the Jungle DLC | ⏳ Released June 18, 2026 — awaiting wiki data for fish/recipes |

## What's Randomized

### 1,300+ Locations across 23 regions
- 🐟 **203 fish first-catches** — every species in their correct depth zone
- 🍣 **549 dish upgrade checks** — research tiers for every Menu dish
- 🔫 **79 weapon crafts** — all variants across 9 weapon trees (Duff's shop)
- 👨‍🍳 **Staff hire + training** — 24 named staff, configurable depth (hire / milestones / all levels)
- 📱 **44 Ecowatcher missions** — real mission data from the wiki
- 🦀 **34 aberration fish** — across 3 vortex regions (DREDGE DLC)
- 🌿 **25 ingredient first-finds** — sea plants, rare forageables, farm crops
- 🍽️ **54+ recipe unlocks** — fish sushi, VIP, boss, Cooksta rank recipes
- 🏆 **12 Cooksta rank requirements** — followers, Best Taste, researched recipes
- 💎 **12 charm acquisitions** — from missions and Ecowatcher level-ups
- 📷 **12 photography missions**, 🎯 **9 challenges**, ⛵ **4 minigames**
- 🌱 **Farm, fish farm, chicken farm** milestones
- 🗺️ **Story chapters, quests, boss defeats, collectibles, teleport points**

### 300+ Items
- Progressive diving suit (8 levels, 40m → 800m depth)
- Progressive oxygen tanks (6 levels) and harpoon (4 levels)
- 79 named weapon variants (Basic Rifle → Thunderbolt Rifle)
- 83 progressive dish items (received when you research dishes)
- 24 named staff members (progression items that gate recipes)
- Area unlocks (Fish Farm, Vegetable Farm, Chicken Farm, Vortex Entry)
- Key items (Sea People Translator, Key to Tenzhin, Tech Suit Parts, Cocktails Unlocked, etc.)
- 12 charms, 25 ingredients as filler, recipe items, and more

### 5 Victory Conditions
| # | Goal | Condition |
|---|---|---|
| 0 | **Defeat Yawie** *(default)* | Defeat the final boss |
| 1 | **Defeat All Bosses** | Yawie + all story/vortex bosses (+ DLC bosses if enabled) |
| 2 | **Diamond Rank** | Yawie + 720 Cooksta followers + 375 Best Taste + 32 researched recipes |
| 3 | **Master Diver** | Yawie + catch every fish species |
| 4 | **100% Completion** | All of the above |

## DLC Support

| DLC | Toggle | Status |
|---|---|---|
| DREDGE Content Pack (free) | `has_dredge_dlc` | ✅ Complete |
| Godzilla Content Pack (free, time-limited) | `has_godzilla_dlc` | ✅ Complete |
| Ichiban's Holiday (paid) | `has_ichiban_dlc` | ✅ Complete |
| In the Jungle (paid, released June 18 2026) | `has_jungle_dlc` | ⏳ Structure complete, fish/recipes pending |

## Getting Started

### Playing the randomizer
See **[docs/SETUP_GUIDE.md](docs/SETUP_GUIDE.md)** for the complete player guide.

**Quick version:**
1. Install BepInEx 6 IL2CPP in your Dave the Diver folder
2. Drop `DaveDiverAP.dll` into `BepInEx/plugins/`
3. Generate a multiworld at [archipelago.gg](https://archipelago.gg) using your YAML
4. Launch the game and press **F9** to connect

### Developing / Contributing

**Prerequisites:** Python 3.10+, .NET SDK 6+, Git

```powershell
git clone https://github.com/AronTheunissen/dave-the-diver-archipelago.git
cd dave-the-diver-archipelago

# APWorld development
cd apworld
python -m venv venv
.\venv\Scripts\Activate.ps1
pip install -r requirements.txt
python -m pytest tests/ -v   # 55/55 should pass

# C# client development
cd ..\client
dotnet build DaveDiverAP/DaveDiverAP.csproj
```

See **[DEVELOPMENT_GUIDE.md](DEVELOPMENT_GUIDE.md)** for architecture, key files, and contribution workflow.
See **[TODO.md](TODO.md)** for current tasks and priorities.

## Project Structure

```
dave-the-diver-archipelago/
├── apworld/davethediver/
│   ├── __init__.py        # World class, item pool, fill_slot_data
│   ├── items.py           # 300+ items with IDs, categories, counts
│   ├── locations.py       # 1,300+ locations with IDs, regions, categories
│   ├── regions.py         # 23 regions with connection rules (DLC-gated)
│   ├── rules.py           # Logic rules (depth access, key items, DLC gates)
│   └── options.py         # 27 YAML options for player customization
├── client/DaveDiverAP/
│   ├── Plugin.cs              # BepInEx 6 IL2CPP entry point
│   ├── ItemHandler.cs         # Routes 300+ items to real game API calls
│   ├── LocationTracker.cs     # Maps 1,300+ game events to AP checks
│   ├── GoalTracker.cs         # Tracks all 5 victory conditions
│   ├── ArchipelagoClient.cs   # AP server connection
│   ├── DeathLinkHandler.cs    # Death Link support
│   ├── SaveData.cs            # Persists all item state between sessions
│   ├── Patches/               # 17 Harmony patch files (all class names confirmed)
│   └── UI/                    # In-game UI (F9): Connection · Hints · Progress
├── docs/
│   ├── SETUP_GUIDE.md             # Player installation guide
│   ├── DEVELOPMENT_GUIDE.md       # Architecture, workflows, design decisions
│   ├── CLASS_NAME_CHEAT_SHEET.md  # Confirmed class/method names from dump.cs
│   ├── MODDING_NOTES.md           # IL2CPP / BepInEx technical notes
│   └── TID_RECORDING_SHEET.md     # Guide for recording TID values in-game
├── player-options-*.yaml          # Example YAML configurations
└── TODO.md                        # Current task list
```

## Resources

- **Archipelago:** https://archipelago.gg/
- **BepInEx:** https://docs.bepinex.dev/
- **Dave the Diver Wiki:** https://dave-the-diver.fandom.com/

## License

MIT — see [LICENSE](LICENSE)
