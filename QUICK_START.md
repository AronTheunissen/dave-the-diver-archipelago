# Quick Start Guide

## 🎉 Your Development Environment is Ready!

I've set up a complete project structure for creating Dave the Diver Archipelago support.

## What's Been Done

✅ **Project Structure Created**
- APWorld Python package with starter code
- Client mod C# project structure  
- Documentation framework
- Archipelago repository cloned for reference

✅ **Python Environment Set Up**
- Virtual environment created in `apworld/venv/`
- Dependencies installed (pytest, black, pylint, etc.)

✅ **Starter Code Written**
- ~50 example items defined
- ~40 example locations defined
- Basic APWorld class structure
- Design document with implementation plan

## What You Need to Do Next

### 1. Install .NET SDK (Required for C# client mod)

Since `dotnet` is not installed, you need to:

**Download and install .NET SDK 8.0:**
👉 https://dotnet.microsoft.com/download/dotnet/8.0

Choose "Windows x64" installer and run it.

### 2. Install BepInEx to Dave the Diver

**Download BepInEx 6 IL2CPP (version 6.0.0-be.674):**
👉 https://github.com/BepInEx/BepInEx/releases

1. Extract the ZIP to your Dave the Diver game folder
2. Run Dave the Diver once (it will take ~30 seconds to start)
3. Close the game - BepInEx has generated interop assemblies

**Find your game folder:**
- Open Steam, right-click Dave the Diver
- Manage → Browse local files

### 3. Configure Your Game Path

Create this file: `client/GamePath.props`

```xml
<Project>
  <PropertyGroup>
    <GamePath>YOUR_GAME_PATH_HERE</GamePath>
  </PropertyGroup>
</Project>
```

Replace `YOUR_GAME_PATH_HERE` with your actual path, like:
`C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver`

## Development Workflow

### Working on APWorld (Python)

```powershell
cd dave-the-diver-archipelago/apworld

# Activate virtual environment (use python.exe directly to avoid execution policy issues)
# Or just use: .\venv\Scripts\python.exe

# Edit files in davethediver/ folder
# - items.py: Add more items
# - locations.py: Add more locations
# - __init__.py: Implement world logic
```

### Working on Client Mod (C#) - After .NET is installed

```powershell
cd dave-the-diver-archipelago/client
dotnet new classlib -n DaveDiverAP -f net48
dotnet add DaveDiverAP package BepInEx.Core
dotnet add DaveDiverAP package BepInEx.IL2CPP
```

## Project Files to Know

📄 **SETUP_COMPLETE.md** - Detailed next steps and status
📄 **README.md** - Full project documentation  
📄 **docs/DESIGN.md** - Implementation design and decisions
📄 **apworld/davethediver/items.py** - Item definitions (expand this!)
📄 **apworld/davethediver/locations.py** - Location definitions (expand this!)

## Your First Task: Game Analysis

To create a good randomizer, you need to document everything in Dave the Diver:

1. **Play through the game** (or watch a playthrough)
2. **Document in a spreadsheet:**
   - All weapons and equipment
   - All recipes
   - All story checkpoints  
   - All side quests
   - All bosses
   - All minigames
   - All collectibles

3. **Categorize each as:**
   - ✅ Location (gives you an item)
   - 📦 Item (something you receive)
   - 🔓 Progression (required to advance)
   - 💡 Useful (helpful but not required)
   - 🎁 Filler (common rewards)

This will help you expand the ~50 items to 150-300 items needed for a good randomizer!

## Need Help?

- Read the **design document**: `docs/DESIGN.md`
- Check **Archipelago Discord** for community help
- Look at **similar games** already in Archipelago (Stardew Valley, Subnautica)
- Reference the **example mod**: https://github.com/WhiteMinds/dave-diver-expansion

## Next Steps Summary

1. ✅ Project created
2. ⏳ Install .NET SDK 8.0
3. ⏳ Install BepInEx to game
4. ⏳ Configure game path  
5. ⏳ Do comprehensive game analysis
6. ⏳ Expand items/locations to 150-300 each
7. ⏳ Implement APWorld logic
8. ⏳ Create C# client mod
9. ⏳ Test and refine

**You're on your way to creating an awesome Archipelago world! 🎮🌊🍣**
