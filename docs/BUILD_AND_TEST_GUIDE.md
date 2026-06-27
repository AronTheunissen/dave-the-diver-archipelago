# Dave the Diver AP — Build & First Test Guide

This guide walks you through building the C# client mod and doing your first live test
against a real Archipelago server.

---

## Prerequisites

Make sure you have these on your **gaming machine** (where Dave the Diver is installed):

| Tool | Where to get | Notes |
|---|---|---|
| Dave the Diver (Steam) | Steam | Any version with BepInEx already set up |
| BepInEx 6 IL2CPP | [GitHub](https://github.com/BepInEx/BepInEx/releases) | Must be IL2CPP build, NOT Mono |
| .NET SDK 8+ | [dotnet.microsoft.com](https://dotnet.microsoft.com/download) | For building the mod |
| Git | [git-scm.com](https://git-scm.com/) | To clone the repo |
| UnityExplorer | [GitHub](https://github.com/sinai-dev/UnityExplorer/releases) | Optional, for debugging |

---

## Step 1 — Set Up BepInEx (if not already done)

1. Download **BepInEx 6.0.0-be.752 IL2CPP x64** (this specific version is confirmed working)
2. Extract into your Dave the Diver folder (next to `Dave the Diver.exe`)
3. Launch the game **once** and close it — BepInEx will generate its folder structure
4. Confirm `BepInEx/plugins/` exists

---

## Step 2 — Clone the Repository

```powershell
git clone https://github.com/AronTheunissen/dave-the-diver-archipelago.git
cd dave-the-diver-archipelago
```

---

## Step 3 — Configure the Build

1. Copy `client/GamePath.props.example` to `client/GamePath.props`
2. Edit `client/GamePath.props` and set your game path:

```xml
<Project>
  <PropertyGroup>
    <GameDir>C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver</GameDir>
  </PropertyGroup>
</Project>
```

3. Copy the required DLLs to `client/lib/`:
   - From `Dave the Diver/BepInEx/interop/` copy:
     - `Assembly-CSharp.dll`
     - `UnityEngine.dll`
     - `UnityEngine.CoreModule.dll`
   - From `Dave the Diver/BepInEx/core/` copy:
     - `BepInEx.dll`
     - `HarmonyX.dll`
   - From `Dave the Diver/Dave the Diver_Data/Managed/` copy:
     - `Archipelago.MultiClient.Net.dll` (if not using NuGet)

   > **Tip:** See `client/lib/README.md` for the complete list.

---

## Step 4 — Build the Mod

```powershell
cd client/DaveDiverAP
dotnet build -c Release
```

If successful, `DaveDiverAP.dll` will appear in `bin/Release/net6.0/`.

**Common build errors:**
- `Could not find Assembly-CSharp.dll` → check `client/lib/` has the DLLs from Step 3
- `GamePath.props not found` → did you copy and edit `GamePath.props.example`?
- `namespace JDLC not found` → you need the Jungle DLC interop DLLs in `client/lib/`

---

## Step 5 — Install the Mod

Copy `DaveDiverAP.dll` to `Dave the Diver/BepInEx/plugins/DaveDiverAP.dll`

```powershell
# From the repo root:
copy client\DaveDiverAP\bin\Release\net6.0\DaveDiverAP.dll "C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\BepInEx\plugins\"
```

---

## Step 6 — Set Up a Test Archipelago Server

### Option A — Quick test with Archipelago website
1. Go to [archipelago.gg](https://archipelago.gg)
2. Upload one of the example YAML files from the repo root (e.g. `player-options-speedrun.yaml`)
3. Generate a room — you'll get a server address like `archipelago.gg:12345`

### Option B — Local server (faster iteration)
1. Download the Archipelago server from [archipelago.gg/downloads](https://archipelago.gg/downloads)
2. Put your YAML in the `Players/` folder
3. Run `ArchipelagoServer.exe` — note the port it starts on (default: 38281)
4. Use `localhost:38281` as your server address

---

## Step 7 — First Launch & Connect

1. Launch Dave the Diver via Steam
2. Watch `BepInEx/LogOutput.log` — you should see:
   ```
   [Info   :   BepInEx] Loading [DaveDiverAP 1.0.0]
   [Info   :DaveDiverAP] DaveDiverAP initialized. Press F9 to open connection UI.
   ```
3. Press **F9** in-game to open the connection UI
4. Enter your server details:
   - **Server:** `archipelago.gg` (or `localhost`)
   - **Port:** `12345` (or `38281` for local)
   - **Slot Name:** your player name from the YAML
   - **Password:** leave blank unless your room has one
5. Click **Connect**
6. You should see: `✅ Connected as [YourName] — Dave the Diver`

---

## Step 8 — Verify Everything Works

### Check the BepInEx log for these messages:

**On connection:**
```
[Info   :DaveDiverAP] Connected to Archipelago server as YourName
[Info   :DaveDiverAP] Slot data received: goal=0, fish_checks=all, ...
```

**When you catch a fish:**
```
[Info   :DaveDiverAP] [FishCaught] GO=SA_2010002_Clownfish → Location="First Catch: Clownfish"
[Info   :DaveDiverAP] Location checked: First Catch: Clownfish (ID=4456448)
```

**When you receive an item:**
```
[Info   :DaveDiverAP] [ItemReceived] Progressive Diving Suit (from YourName's world)
```

**When you complete a mission:**
```
[Info   :DaveDiverAP] [MissionCleared] TID=10010003 Type=Side Title="Weaponsmith Duff"
[Info   :DaveDiverAP] Location checked: Quest: Complete Duff's First Request (ID=...)
```

---

## Step 9 — What to Test First

Work through these in order — they cover the most important code paths:

### 🐟 Fish catches
- Dive into Blue Hole Shallows and catch any fish
- Check log for `[FishCaught]` → `Location checked`
- If you see `UNMAPPED`, note the GO name and report it

### 🍣 Recipe unlocks
- Go to restaurant management and unlock a recipe
- Check log for `[RecipeUnlocked]` → `Location checked`

### 🚤 Boat-only item delivery
- Receive an item from another player (or use the Archipelago website to send yourself one)
- Go diving — item should NOT arrive yet
- Surface and return to boat — item should arrive with a notification

### 💀 Death Link (if enabled in YAML)
- Enable `death_link: true` in your YAML
- Let Dave die underwater
- Check that the death is sent to other players
- Have another player die and verify Dave dies too

### 🏆 Goal completion
- With goal `defeat_yawie`, defeat Yawie
- Check log for `[GoalComplete] Defeat Yawie`
- Verify the Archipelago server shows you as completed

---

## Troubleshooting

### F9 doesn't open the UI
- Check `LogOutput.log` for BepInEx errors
- Make sure you're using **IL2CPP** BepInEx, not Mono
- Verify the DLL is in `BepInEx/plugins/` (not a subfolder)

### "Could not connect to server"
- Check the server address and port
- If using local server, make sure `ArchipelagoServer.exe` is running
- Check Windows Firewall isn't blocking the connection

### Fish catches not registering
- Check the log for `[FishCaught] GO=SA_XXXXXXX_FishName → UNMAPPED`
- If unmapped, the TID isn't in `FishNameMapper` yet — report it!
- Make sure you're actually picking up the fish (press E/interact), not just killing it

### Items not being delivered
- Items only deliver on the **boat** — make sure you're back at the boat
- Check `[ItemReceived]` in the log to confirm the item arrived
- Check `[ItemQueue]` for pending items count

### Mod crashes on startup
- Check `LogOutput.log` for the exact error
- Most common: missing DLL in `client/lib/` — rebuild with correct DLLs
- Try deleting `BepInEx/cache/` and relaunching

---

## Watching the Log Live

To watch the BepInEx log in real-time (PowerShell):

```powershell
Get-Content "C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\BepInEx\LogOutput.log" -Wait -Tail 50
```

This is extremely useful for debugging — run it in a separate window while playing.

---

## Known Issues / First Test Checklist

- [ ] Mod loads without errors (check LogOutput.log on startup)
- [ ] F9 opens connection UI
- [ ] Connection succeeds
- [ ] Fish catch registers in log and sends check
- [ ] Item receipt shows notification toast
- [ ] Items only deliver on boat (not while diving)
- [ ] Mission completion registers in log
- [ ] Death Link works (if enabled)
- [ ] Goal completion fires correctly

Report any issues at: https://github.com/AronTheunissen/dave-the-diver-archipelago/issues
