# Quick Start Guide

> **Current status:** APWorld complete, C# client complete with all game API calls implemented.
> Main remaining tasks: (1) fill in quest/challenge/weapon TID mappings via UnityExplorer, (2) in-game testing.

---

## For Players (Want to play the randomizer?)

See **[docs/SETUP_GUIDE.md](docs/SETUP_GUIDE.md)** — it covers everything from
BepInEx installation to connecting to your multiworld server.

**Minimum requirements:**
- Dave the Diver (Steam)
- BepInEx 6 IL2CPP (free)
- An Archipelago server to connect to

---

## For Developers (Want to contribute?)

### What's already done
- ✅ Full APWorld: 1,134 locations, 276 items, logic rules, 5 goals, DLC support
- ✅ C# client mod with 17 Harmony patches, all game API calls implemented
- ✅ In-game UI (F9): Connection · Hints · Progress (with item tracker, category breakdown)
- ✅ Death Link, hint system, boat-only item delivery, save/restore state
- ✅ Player setup guide

### What still needs doing
See **[TODO.md](TODO.md)** for the full list. The most impactful tasks are:

1. **Fill in TID mapper dictionaries** — run game with UnityExplorer to capture
   weapon craft TIDs, recipe TIDs, quest/mission TIDs
   (see `docs/TID_RECORDING_SHEET.md` for the process)
2. **Jungle DLC content** — new fish, bosses, and locations (wiki not yet updated)
3. **In-game testing** — verify all 17 patches fire correctly end-to-end

### Quick setup

**Prerequisites:** Python 3.10+, .NET SDK 6+, Git

```powershell
git clone https://github.com/AronTheunissen/dave-the-diver-archipelago.git
cd dave-the-diver-archipelago

# APWorld (Python)
cd apworld
python -m venv venv
.\venv\Scripts\Activate.ps1
pip install -r requirements.txt

# C# Client
cd ..\client
dotnet build
```

### Key files to know

| File | Purpose |
|---|---|
| `apworld/davethediver/__init__.py` | World class — start here for APWorld logic |
| `apworld/davethediver/items.py` | All 276 items with IDs and categories |
| `apworld/davethediver/locations.py` | All 1,134 locations with regions |
| `apworld/davethediver/rules.py` | Region access logic |
| `client/DaveDiverAP/ItemHandler.cs` | All item delivery — real game API calls ✅ |
| `client/DaveDiverAP/Patches/` | 17 Harmony patches — all class names confirmed ✅ |
| `docs/CLASS_NAME_CHEAT_SHEET.md` | Confirmed class/method names from dump.cs |
| `TODO.md` | Full task list with priorities |

### Useful commands

```powershell
# Validate the APWorld loads correctly
cd apworld
python -c "from davethediver import DaveDiverWorld; print('OK')"

# Run unit tests
python -m pytest tests/ -v

# Build the C# client
cd client
dotnet build DaveDiverAP/DaveDiverAP.csproj

# Check git status
git status
git log --oneline -10
```
