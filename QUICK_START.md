# Quick Start Guide

> **Current status:** APWorld complete, C# client complete with all real class/method names confirmed via dump.cs.
> The main remaining tasks are: (1) implement `ItemHandler.cs` game API calls, (2) fill in quest/challenge TID mappings, (3) in-game testing.

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
- ✅ C# client mod skeleton with 17 Harmony patches, in-game UI, Death Link, hints
- ✅ Player setup guide

### What still needs doing
See **[TODO.md](TODO.md)** for the full list. The most impactful tasks are:

1. **Implement `ItemHandler.cs` game API calls** — give items to the player in-game
   (class names confirmed — just needs the actual SaveData setter calls)
2. **Fill in quest/challenge TIDs** — run game and use BepInEx log to capture TIDs
   (see `docs/TID_RECORDING_SHEET.md` for scripts and what's confirmed)
3. **Jungle DLC content** — new fish, bosses, and locations from June 18 2026 release
4. **In-game testing** — verify all 17 patches fire correctly

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
| `client/DaveDiverAP/Patches/` | 17 Harmony patches — all class names confirmed ✅ |
| `client/DaveDiverAP/ItemHandler.cs` | Item delivery stubs — needs game API calls |
| `docs/CLASS_NAME_CHEAT_SHEET.md` | What to search for in the decompiler |
| `TODO.md` | Full task list with priorities |

### Useful commands

```powershell
# Validate the APWorld loads correctly
cd apworld
python -c "from davethediver import DaveDiverWorld; print('OK')"

# Build the C# client
cd client
dotnet build DaveDiverAP/DaveDiverAP.csproj

# Check git status
git status
git log --oneline -10
```
