# Dave the Diver - Archipelago Integration

This project adds Archipelago multiworld randomizer support to Dave the Diver.

## Project Structure

```
dave-the-diver-archipelago/
├── apworld/                    # Archipelago world implementation (Python)
│   ├── davethediver/          # APWorld module
│   │   ├── __init__.py        # Main world class
│   │   ├── items.py           # Item definitions
│   │   ├── locations.py       # Location definitions
│   │   ├── regions.py         # Region/area definitions
│   │   ├── rules.py           # Logic rules
│   │   └── options.py         # YAML options
│   └── tests/                 # Unit tests
├── client/                    # BepInEx client mod (C#)
│   ├── DaveDiverAP/          # Main mod project
│   │   ├── Plugin.cs         # BepInEx plugin entry point
│   │   ├── ArchipelagoClient.cs  # AP connection logic
│   │   └── Patches/          # Harmony patches
│   └── lib/                  # Reference DLLs from game
├── docs/                     # Documentation
│   ├── DESIGN.md            # Design decisions
│   ├── LOCATIONS.md         # All check locations
│   └── ITEMS.md             # All randomizable items
└── tools/                   # Development tools
    └── setup/               # Setup scripts
```

## Development Environment Setup

### Prerequisites

1. **Python 3.10+**
2. **Git**
3. **.NET SDK 8.0+**
4. **Visual Studio Code** (Recommended) or Visual Studio
5. **Dave the Diver** (Steam version)

### Quick Setup (New Machine)

**See SETUP_ON_NEW_MACHINE.md for complete instructions!**

Quick version:
```powershell
# Clone the repository
git clone https://github.com/AronTheunissen/dave-the-diver-archipelago.git
cd dave-the-diver-archipelago

# Run the setup script
.\tools\setup\setup-dev-environment.ps1
```

Or follow manual steps below.

## Manual Setup Steps

### 1. Install .NET SDK

Download and install .NET SDK 8.0 from:
https://dotnet.microsoft.com/download/dotnet/8.0

### 2. Clone Archipelago Repository

```powershell
git clone https://github.com/ArchipelagoMW/Archipelago.git tools/Archipelago
cd tools/Archipelago
python -m pip install -e .
```

### 3. Install BepInEx for Dave the Diver

1. Download BepInEx 6 IL2CPP from: https://github.com/BepInEx/BepInEx/releases
2. Extract to your Dave the Diver game directory
3. Run the game once to generate interop assemblies
4. Copy required DLLs to `client/lib/`

### 4. Set Up Python Environment

```powershell
cd apworld
python -m venv venv
.\venv\Scripts\Activate.ps1
pip install -r requirements.txt
```

### 5. Configure Game Path

Create `client/GamePath.props`:
```xml
<Project>
  <PropertyGroup>
    <GamePath>C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver</GamePath>
  </PropertyGroup>
</Project>
```

## Development Workflow

### Working on APWorld (Python)

```powershell
cd apworld
.\venv\Scripts\Activate.ps1
# Edit files in apworld/davethediver/
python -m pytest tests/  # Run tests
```

### Working on Client Mod (C#)

```powershell
cd client
dotnet build
# DLL will be copied to game directory automatically
```

### Testing

1. Generate a seed with your APWorld
2. Host the seed locally
3. Launch Dave the Diver with the client mod
4. Connect to your local server

## Resources

- **Archipelago**: https://archipelago.gg/
- **Archipelago GitHub**: https://github.com/ArchipelagoMW/Archipelago
- **Archipelago Discord**: https://discord.gg/archipelago
- **BepInEx Documentation**: https://docs.bepinex.dev/
- **Dave the Diver Modding**: https://www.nexusmods.com/davethediver

## Contributing

See [DESIGN.md](docs/DESIGN.md) for design decisions and implementation details.

## License

This project is licensed under the MIT License - see LICENSE file for details.
