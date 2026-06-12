# Dave the Diver — Archipelago Setup Guide

This guide walks you through everything you need to play Dave the Diver
as part of an Archipelago multiworld randomizer.

---

## Requirements

| Requirement | Version | Download |
|---|---|---|
| Dave the Diver (Steam) | Latest | [Steam](https://store.steampowered.com/app/1868140) |
| BepInEx 6 IL2CPP | 6.0.0-be.752+ | [GitHub](https://github.com/BepInEx/BepInEx/releases) |
| .NET Runtime | 6.0+ | [Microsoft](https://dotnet.microsoft.com/download/dotnet/6.0) |
| Archipelago | 0.4.4+ | [archipelago.gg](https://archipelago.gg) |

---

## Step 1 — Install BepInEx

1. Download **BepInEx 6 IL2CPP** for Windows x64 from the link above.
2. Extract the contents into your Dave the Diver game directory:
   ```
   C:\Program Files (x86)\Steam\steamapps\common\Dave the Diver\
   ```
   After extraction, you should see a `BepInEx\` folder alongside `DAVE THE DIVER.exe`.

3. **Launch the game once** to let BepInEx generate interop assemblies.
   - You'll see a BepInEx console window appear briefly — that's normal.
   - Close the game after the main menu loads.

4. Verify the installation by checking that these folders exist:
   ```
   Dave the Diver\BepInEx\interop\
   Dave the Diver\BepInEx\plugins\
   Dave the Diver\BepInEx\config\
   ```

---

## Step 2 — Install the Archipelago Client Mod

1. Download `DaveDiverAP.dll` from the [latest release](https://github.com/AronTheunissen/dave-the-diver-archipelago/releases).

2. Copy it into your BepInEx plugins folder:
   ```
   Dave the Diver\BepInEx\plugins\DaveDiverAP.dll
   ```

3. Launch the game. You should see a BepInEx log entry:
   ```
   [Info   : DaveDiverAP] Dave the Diver Archipelago v0.1.0 loaded successfully!
   ```

---

## Step 3 — Configure Your YAML

1. Download the template YAML file: [`player-options-example.yaml`](../player-options-example.yaml)

2. Open it in any text editor and set your options:

```yaml
name: YourName        # Your Archipelago slot name

dave-the-diver:
  goal: defeat_yawie  # See goals section below

  # How many fish species to include as checks
  fish_checks: all    # none | rare_only | all

  # Weapon shop crafting
  include_weapon_shop: true

  # Optional systems
  include_cooksta: true
  include_ecowatcher: true
  include_photography: true
  include_challenges: true
  include_farming: true
  include_chicken_farm: true
  include_fish_farm: true
  include_minigames: true

  # DLC — only enable if you own it!
  has_dredge_dlc: false
  has_godzilla_dlc: false
  has_ichiban_dlc: false
  has_jungle_dlc: false

  # Starting equipment (1 = base level)
  starting_oxygen_level: 1
  starting_harpoon_level: 1
  starting_suit_level: 1

  # Death Link — dying sends deaths to other linked players
  death_link: false
```

### Available Goals

| Goal | Description | Estimated Time |
|---|---|---|
| `defeat_yawie` | Defeat the final boss | ~15-25 hrs |
| `defeat_all_bosses` | Defeat Yawie + all 15 optional bosses | ~25-35 hrs |
| `diamond_rank` | Yawie + 720 Cooksta followers + 375 Best Taste + 32 researched recipes | ~30-40 hrs |
| `master_diver` | Yawie + catch every fish species | ~50-70 hrs |
| `hundred_percent` | Everything above combined | 100+ hrs |

---

## Step 4 — Generate & Host a Multiworld

### Option A — archipelago.gg (recommended)
1. Go to [archipelago.gg](https://archipelago.gg) and create an account.
2. Upload your YAML file to the website.
3. Generate a multiworld with your group.
4. The website will host the session automatically.

### Option B — Local hosting
1. Download the Archipelago server from [archipelago.gg/downloads](https://archipelago.gg/downloads).
2. Place your YAML file in the `Players/` folder.
3. Run `ArchipelagoServer.exe` and generate a multiworld.

---

## Step 5 — Connect In-Game

1. Launch Dave the Diver.
2. **Start or load a save file** — the mod requires an active game session.
3. Press **F9** to open the Archipelago connection window.
4. Enter your connection details:
   - **Server**: The address from the host (e.g. `archipelago.gg` or `localhost`)
   - **Port**: Typically `38281` (shown in the server output)
   - **Slot Name**: Must match your YAML `name` field exactly
   - **Password**: Leave blank unless the host set one
5. Click **Connect**.

You should see:
```
🔗 Connected!
Archipelago: YourName @ archipelago.gg:38281
```

---

## Using the In-Game UI (F9)

The Archipelago window has three tabs:

### Connection Tab
- Shows connection status and server info
- **⏳ items waiting** — items are only delivered when Dave is on the boat
- Recent item receive log with timestamps

### Hints Tab
- **Request a hint** to find out where an item is in the multiworld
- Costs hint points (earned by completing checks)
- View all received hints with found/unfound status

### Progress Tab
- Overall check completion bar (X / Y checks)
- Goal-specific progress bars (boss count, Cooksta requirements, fish caught)
- Category breakdown (fish, weapons, recipes, etc.)

---

## Important Notes

### Items are delivered on the boat 🚤
Items received from other players are **held** until Dave is standing on the boat at the start of the day. They are **not** delivered while:
- Diving in the Blue Hole
- Managing Bancho Sushi at night
- Working on the farms

### Starting equipment
The mod sends you your starting equipment (oxygen tank, harpoon, diving suit) immediately on connection. Check the item log — they should appear as the first items received.

### Progression items to watch for
These items are required to reach new areas:
- 🧤 **Sea People Gloves** — required to swim to Sea People Village
- 📖 **Sea People Translator** — required to interact in the village
- 🔑 **Key to Tenzhin** — opens the Glacial Passage gate
- 🧊 **Progressive Diving Suit** (levels 7+) — required for glacier areas
- ⚙️ **Tech Suit Parts** (×3) — required for the full Glacier Zone
- 🔴 **Control Room Button** (×3) + **Laser Device** — required to fight Yawie

### Death Link
If Death Link is enabled in your YAML, dying in the Blue Hole (oxygen depletion or enemy damage) will cause all other Death Link players to die simultaneously — and vice versa.

---

## Troubleshooting

**The F9 menu doesn't open**
- Make sure `DaveDiverAP.dll` is in `BepInEx\plugins\`
- Check the BepInEx log at `BepInEx\LogOutput.log` for errors

**"Authentication failed" on connection**
- Double-check your slot name matches the YAML exactly (case-sensitive)
- Make sure you're using the right server address and port

**Items aren't being delivered**
- Items are only delivered on the boat — complete your current dive/restaurant shift first
- The Connection tab shows "⏳ N items waiting" when items are queued

**Checks aren't being sent**
- The Harmony patches need the game's real class names (see `docs/MODDING_NOTES.md`)
- This is expected in early versions — the mod framework is complete but class names need filling in via Il2CppDumper

---

## DLC Notes

| DLC | Status | Set in YAML |
|---|---|---|
| DREDGE Content Pack | Free — adds aberration vortex fish + Drain Gun | `has_dredge_dlc: true` |
| Godzilla Content Pack | Free, time-limited — adds Godzilla recipes | `has_godzilla_dlc: true` |
| Ichiban's Holiday Pack | Paid — adds minigames and staff | `has_ichiban_dlc: true` |
| In the Jungle Pack | Paid (June 2026) — adds new region | `has_jungle_dlc: true` |

> ⚠️ Only enable DLC options if you actually own the DLC. Enabling DLC you don't own will add checks you can never complete, making the seed unfinishable.

---

## Getting Help

- **Discord**: Join the Archipelago Discord at [discord.gg/archipelago](https://discord.gg/archipelago) and find the Dave the Diver thread
- **GitHub Issues**: [github.com/AronTheunissen/dave-the-diver-archipelago/issues](https://github.com/AronTheunissen/dave-the-diver-archipelago/issues)
- **Modding Notes**: See [`docs/MODDING_NOTES.md`](MODDING_NOTES.md) for technical details

---

*This mod is a fan project and is not affiliated with MINTROCKET or Archipelago.*
