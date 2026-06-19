# Dave the Diver — Archipelago Randomizer

An [Archipelago](https://archipelago.gg/) multiworld randomizer integration for **Dave the Diver** (Steam, by MINTROCKET).

## Project Status

| Component | Status |
|---|---|
| APWorld (Python) | ✅ Complete — 1,134 locations, 276 items, 55 tests passing |
| C# Client Mod | ✅ Complete — all 17 patches wired with real class names |
| Reverse Engineering | ✅ dump.cs analysed — all class/method names confirmed |
| Player Setup Guide | ✅ See `docs/SETUP_GUIDE.md` |
| Unit Tests | ✅ 55/55 passing |
| In the Jungle DLC | ⏳ Released June 18, 2026 — content integration in progress |

## What's Randomized

### 1,134 Locations across 15 regions
- 🐟 **203 fish first-catches** — every species in their correct depth zone
- 🍣 **549 dish upgrade checks** — research tiers for every Menu dish
- 🔫 **79 weapon crafts** — all variants across 9 weapon trees (Duff's shop)
- 📱 **44 Ecowatcher missions** — real mission data from the wiki
- 🦀 **34 aberration fish** — across 3 vortex regions (Jellyfish Basin, Fog Coast, Black Cliff)
- 🌿 **25 ingredient first-finds** — sea plants, rare forageables, farm crops
- 🍽️ **54 recipe unlocks** — fish sushi, VIP, boss, Cooksta rank recipes
- 🏆 **12 Cooksta rank requirements** — followers, Best Taste, researched recipes
- 💎 **12 charm acquisitions** — from missions and Ecowatcher level-ups
- 📷 **12 photography missions**, 🎯 **9 challenges**, ⛵ **4 minigames**
- 🌱 **Farm, fish farm, chicken farm** milestones
- 🗺️ **Story chapters, quests, boss defeats, collectibles, teleport points**

### 276 Items
- Progressive diving suit (8 levels, 40m → 800m depth)
- Progressive oxygen tanks (6 levels) and harpoon (4 levels)
- 79 named weapon variants (Basic Rifle → Thunderbolt Rifle)
- 83 progressive dish items (received when you research dishes)
- Area unlocks (Fish Farm, Vegetable Farm, Chicken Farm, Vortex Entry)
- Key items (Sea People Translator, Key to Tenzhin, Tech Suit Parts, etc.)
- 12 charms, 25 ingredients as filler, recipe items, and more

### 5 Victory Conditions
| # | Goal | Condition |
|---|---|---|
| 0 | **Defeat Yawie** *(default)* | Defeat the final boss |
| 1 | **Defeat All Bosses** | Yawie + all 15 story/vortex bosses |
| 2 | **Diamond Rank** | Yawie + 720 Cooksta followers + 375 Best Taste + 32 researched recipes |
| 3 | **Master Diver** | Yawie + catch every fish species |
| 4 | **100% Completion** | All of the above |

## Project Structure

```
dave-the-diver-archipelago/
├── apworld/
│   └── davethediver/
│       ├── __init__.py        # World class, item pool, fill_slot_data
│       ├── items.py           # 276 items with IDs, categories, counts
│       ├── locations.py       # 1,134 locations with IDs, regions, categories
│       ├── regions.py         # 15 regions with connection rules
│       ├── rules.py           # Logic rules (depth access, region gating)
│       └── options.py         # 25 YAML options for player customization
├── client/
│   └── DaveDiverAP/
│       ├── Plugin.cs              # BepInEx 6 IL2CPP entry point
│       ├── ArchipelagoClient.cs   # AP server connection
│       ├── ItemHandler.cs         # Routes 276 items to game effects
│       ├── LocationTracker.cs     # Maps 1,134 game events to AP checks
│       ├── GoalTracker.cs         # Tracks all 5 victory conditions
│       ├── DeathLinkHandler.cs    # Death Link support
│       ├── SlotData.cs            # Parses all 25 YAML options
│       ├── SaveData.cs            # Persists connection state
│       ├── ItemQueue.cs           # Thread-safe item delivery (boat only)
│       ├── ModConfig.cs           # BepInEx config file
│       ├── Patches/               # 17 Harmony patch files
│       └── UI/                    # In-game UI (F9): Connection, Hints, Progress
├── docs/
│   ├── SETUP_GUIDE.md             # Player installation guide
│   ├── CLASS_NAME_CHEAT_SHEET.md  # Reverse engineering reference
│   ├── MODDING_NOTES.md           # IL2CPP / BepInEx technical notes
│   ├── DESIGN.md                  # Design decisions
│   ├── PROGRESSION_DESIGN.md      # Region gating & progression logic
│   └── ...                        # Other design docs
├── tools/
│   └── setup/
│       └── setup-dev-environment.ps1
└── player-options-*.yaml          # Example YAML configurations
```

## Getting Started

### Playing the randomizer
See **[docs/SETUP_GUIDE.md](docs/SETUP_GUIDE.md)** for the complete player guide.

**Quick version:**
1. Install BepInEx 6 IL2CPP in your Dave the Diver folder
2. Drop `DaveDiverAP.dll` into `BepInEx/plugins/`
3. Generate a multiworld at [archipelago.gg](https://archipelago.gg) using your YAML
4. Launch the game and press **F9** to connect

### Developing

**Prerequisites:** Python 3.10+, .NET SDK 6+

```powershell
# Clone and set up
git clone https://github.com/AronTheunissen/dave-the-diver-archipelago.git
cd dave-the-diver-archipelago

# APWorld development
cd apworld
python -m venv venv
.\venv\Scripts\Activate.ps1
pip install -r requirements.txt

# C# client development
cd client
dotnet build
```

See **[DEVELOPMENT_GUIDE.md](DEVELOPMENT_GUIDE.md)** for detailed workflow and **[TODO.md](TODO.md)** for current tasks.

## DLC Support

| DLC | Toggle | Status |
|---|---|---|
| DREDGE Content Pack (free) | `has_dredge_dlc` | ✅ Implemented |
| Godzilla Content Pack (free, time-limited) | `has_godzilla_dlc` | ✅ Implemented |
| Ichiban's Holiday (paid) | `has_ichiban_dlc` | ⏳ Structure ready |
| In the Jungle (paid, released June 18 2026) | `has_jungle_dlc` | ⏳ Content integration in progress |

## Resources

- **Archipelago:** https://archipelago.gg/
- **Archipelago GitHub:** https://github.com/ArchipelagoMW/Archipelago
- **BepInEx:** https://docs.bepinex.dev/
- **Dave the Diver Wiki:** https://dave-the-diver.fandom.com/

## License

MIT — see [LICENSE](LICENSE)
