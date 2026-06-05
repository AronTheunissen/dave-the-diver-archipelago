# Setting Up on a New Machine

## 📋 What's Included in the Repository

✅ **Everything you need is in the repo:**
- Complete project structure
- APWorld starter code with examples
- Development documentation and guides
- Setup scripts
- Python requirements file
- .gitignore configuration

## ⚠️ What's NOT Included (You'll Need to Install)

These are excluded by `.gitignore` and must be installed on each machine:

1. **Python virtual environment** (`apworld/venv/`) - Will be created
2. **Archipelago repository** (`tools/Archipelago/`) - Will be cloned
3. **Game installation** - You need Dave the Diver installed
4. **BepInEx setup** - Must be installed to game directory
5. **Your personal config** (`client/GamePath.props`) - Machine-specific

## 🚀 Quick Setup on New Machine

### Prerequisites to Install:

1. **Python 3.10+**
   - Download: https://www.python.org/downloads/
   - Make sure to check "Add Python to PATH"

2. **Git**
   - Download: https://git-scm.com/downloads

3. **.NET SDK 8.0** (for C# client mod development)
   - Download: https://dotnet.microsoft.com/download/dotnet/8.0

4. **Visual Studio Code** (recommended)
   - Download: https://code.visualstudio.com/

5. **Dave the Diver** (Steam)
   - Have it installed

### Step-by-Step Setup:

#### 1. Clone the Repository

```powershell
# Navigate to your projects folder
cd C:\Users\YourName\Documents

# Clone the repository
git clone https://github.com/AronTheunissen/dave-the-diver-archipelago.git
cd dave-the-diver-archipelago
```

#### 2. Run the Setup Script

The repository includes a setup script that automates everything:

```powershell
# Option 1: Run the automated setup script
.\tools\setup\setup-dev-environment.ps1

# If you get execution policy errors, use:
powershell -ExecutionPolicy Bypass -File .\tools\setup\setup-dev-environment.ps1
```

The script will:
- Check for Python, Git, and .NET SDK
- Clone the Archipelago repository
- Create Python virtual environment
- Install Python dependencies
- Prompt for your game path

#### 3. Manual Setup (If Script Fails)

If the script doesn't work, do these steps manually:

**A. Set up Python environment:**
```powershell
cd apworld
python -m venv venv
.\venv\Scripts\Activate.ps1
pip install -r requirements.txt
```

**B. Clone Archipelago repository:**
```powershell
cd ..
git clone https://github.com/ArchipelagoMW/Archipelago.git tools/Archipelago
```

**C. Configure game path:**

Create `client/GamePath.props`:
```xml
<Project>
  <PropertyGroup>
    <GamePath>C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver</GamePath>
  </PropertyGroup>
</Project>
```

Replace with your actual game installation path.

#### 4. Install BepInEx to Game

1. Download BepInEx 6 IL2CPP (version 6.0.0-be.674)
   - From: https://github.com/BepInEx/BepInEx/releases
2. Extract to your Dave the Diver folder
3. Run the game once (takes ~30 seconds first time)
4. Close the game

#### 5. Verify Setup

**Test Python environment:**
```powershell
cd apworld
.\venv\Scripts\python.exe --version
.\venv\Scripts\python.exe -c "import pytest; print('Success!')"
```

**Test .NET SDK:**
```powershell
dotnet --version
```

**Check Git:**
```powershell
git --version
```

## 📁 What Each Folder Contains

```
dave-the-diver-archipelago/
│
├── apworld/                     # Python APWorld development
│   ├── davethediver/           # Your world implementation (in repo ✅)
│   │   ├── __init__.py         # Main world class
│   │   ├── items.py            # ~50 example items
│   │   └── locations.py        # ~40 example locations
│   ├── requirements.txt        # Python dependencies (in repo ✅)
│   └── venv/                   # Virtual environment (NOT in repo ⚠️)
│
├── client/                     # C# client mod development
│   ├── GamePath.props          # Your game path (NOT in repo ⚠️)
│   └── lib/                    # Reference DLLs (NOT in repo ⚠️)
│
├── docs/                       # Documentation (in repo ✅)
│   └── DESIGN.md              # Design decisions
│
├── tools/                      
│   ├── setup/                  # Setup scripts (in repo ✅)
│   └── Archipelago/           # AP reference (NOT in repo ⚠️)
│
└── Documentation files (in repo ✅)
    ├── START_HERE.md
    ├── QUICK_START.md
    ├── DEVELOPMENT_GUIDE.md
    ├── README.md
    └── etc.
```

## 🔄 Syncing Between Machines

### Pushing Changes from Machine 1:

```powershell
git add .
git commit -m "Add more items and locations"
git push
```

### Pulling Changes on Machine 2:

```powershell
git pull
```

**Important:** The Python virtual environment and Archipelago repository are local to each machine. Only your code and documentation sync via Git.

## ✅ Quick Verification Checklist

After setup on new machine, verify:

- [ ] Python virtual environment works: `.\apworld\venv\Scripts\python.exe --version`
- [ ] Python dependencies installed: `pip list` shows pytest, black, etc.
- [ ] Archipelago repo cloned: `tools/Archipelago/` folder exists
- [ ] .NET SDK installed: `dotnet --version` works
- [ ] Git configured: `git config --global user.name` shows your name
- [ ] Game path configured: `client/GamePath.props` exists
- [ ] BepInEx installed: `[GamePath]/BepInEx/` folder exists

## 🎯 Ready to Develop!

Once setup is complete, you can:

**Work on APWorld (Python):**
```powershell
cd apworld
.\venv\Scripts\Activate.ps1
# Edit files in davethediver/
code .  # Opens VS Code
```

**Work on Client (C#):**
```powershell
cd client
dotnet build
# Edit .cs files
```

**Test APWorld:**
```powershell
cd tools/Archipelago
# Copy your APWorld
cp -r ../../apworld/davethediver ./worlds/
python Generate.py
```

## 🔧 Troubleshooting

### "Python not found"
- Install Python from python.org
- Make sure "Add to PATH" was checked during installation
- Restart PowerShell after installing

### "Execution Policy" errors
```powershell
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
```

### "Git not found"
- Install Git from git-scm.com
- Restart PowerShell after installing

### Virtual environment issues
Delete and recreate:
```powershell
cd apworld
Remove-Item -Recurse -Force venv
python -m venv venv
.\venv\Scripts\Activate.ps1
pip install -r requirements.txt
```

## 💡 Pro Tips

1. **Use VS Code:** Open the entire `dave-the-diver-archipelago` folder in VS Code for the best experience

2. **Git branches:** Create feature branches for major changes:
   ```powershell
   git checkout -b feature/add-more-items
   # Make changes
   git commit -m "Add 50 more items"
   git push -u origin feature/add-more-items
   ```

3. **Commit often:** Don't wait until everything is perfect

4. **Test before pushing:** Make sure your code at least runs before pushing

5. **Pull before starting work:** Always `git pull` before starting to avoid conflicts

## 📞 Need Help?

Check these files in the repository:
- `START_HERE.md` - Quick orientation
- `DEVELOPMENT_GUIDE.md` - Complete development workflow with examples
- `QUICK_START.md` - Setup instructions
- `docs/DESIGN.md` - Design decisions

Join the Archipelago Discord for community support!

---

## Summary: Yes, You're Ready! ✅

**The repository contains everything you need** to start developing on any machine:
- ✅ All source code
- ✅ Setup scripts
- ✅ Documentation and guides
- ✅ Dependencies list (requirements.txt)
- ✅ Project structure

**You just need to:**
1. Clone the repository
2. Run the setup script (or follow manual steps)
3. Configure your local game path
4. Start developing!

The setup takes about **10-15 minutes** on a new machine. 🚀
