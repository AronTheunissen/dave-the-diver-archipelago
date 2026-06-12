# Dave the Diver — Modding & Reverse Engineering Notes

This document summarizes research into Dave the Diver's internal code structure
to help implement the Harmony patches in the C# client mod.

## Game Technical Details

- **Engine:** Unity 6000.0.52f1
- **Compilation:** IL2CPP (NOT Mono — this matters a lot for patching)
- **Platform:** 64-bit Windows only
- **BepInEx version:** 6.0.0-be.752 (latest tested as of June 2026)

## How IL2CPP Affects Modding

Unlike Mono games, IL2CPP games don't have a readable `Assembly-CSharp.dll`.
Instead:
1. BepInEx runs `cpp2il` on first launch to generate interop DLLs in `BepInEx/interop/`
2. You reference these generated interop DLLs in your mod project
3. Harmony patches target the interop wrapper classes directly

**Critical lesson from existing mods:**
> Use the SaveSystem API to read/write game state — do NOT patch property getters directly,
> as IL2CPP property getters return garbage values when patched with Harmony.
> (Source: WhiteMinds/dave-diver-expansion changelog v1.1.0)

## How to Find Real Class Names

### Step 1: Run Il2CppDumper
```
Il2CppDumper.exe GameAssembly.dll global-metadata.dat
```
Both files are in the game's installation directory.
- Download: https://github.com/Perfare/Il2CppDumper
- Output: `DummyDll/` folder with C# stubs for all classes

### Step 2: Load in ILSpy
Open the generated DLLs in ILSpy (https://github.com/icsharpcode/ILSpy) and search for:
- `SuccessInteract` — universal interaction hook
- `FirstCatch` or `first_catch` — fish first-catch tracking
- `Recipe` or `Unlock` — recipe system
- `Chapter` or `Mission` or `Quest` — story progression
- `Weapon` or `Craft` or `Duff` — weapon shop
- `Boss` or `Defeat` — boss tracking

### Step 3: Reference existing mods
- **devopsdinosaur/dave-the-diver-mods**: https://github.com/devopsdinosaur/dave-the-diver-mods
- **WhiteMinds/dave-diver-expansion**: https://github.com/WhiteMinds/dave-diver-expansion (most recent, BepInEx 6)
- **Arutsuyo/SuperDave2.0**: https://github.com/Arutsuyo/SuperDave2.0
- **FNGarvin/DaveSaveEd**: https://github.com/FNGarvin/DaveSaveEd (save file structure)

## Known Classes (Confirmed)

| Class | Source | Notes |
|---|---|---|
| `SaveSystem` | Cheat Engine tables | Singleton, provides access to PlayerInfoSave |
| `PlayerInfoSave` | Cheat Engine tables | Main save data class, uses ObscuredInt encryption |
| `InventoryItemSlotSave` | Cheat Engine v1.0.5.1749+ | Item slot data: m_Index +18, m_ItemID +2C, m_TotalCount +40 |
| `InGameManager` | Mod source code | Has `FishAllocators` for fish spawning |
| `PlayerCharacter` | Mod source code | Player movement, stats |
| `SushiBarCustomer` | Cheat Engine v1.0.5.1749+ | Customer management |
| `StaffBancho` / `StaffData` | Cheat Engine v1.0.5.1749+ | Restaurant staff |
| `IngredientsDetailPanel` | Cheat Engine v1.0.5.1749+ | Recipe/ingredient UI, likely connects to recipe system |
| `HarpoonHandler` | Cheat engine references | Harpoon weapon system |
| `MeleeHandler` | Cheat engine references | Melee weapon system |

## Known Patterns

### Universal Interaction Pattern
ALL fish catches, item pickups, and chest openings use this two-method pattern:
```csharp
// 1. Check if interaction is available
bool CheckAvailableInteraction();

// 2. Execute the interaction (HOOK THIS for fish catches)
void SuccessInteract();
```

### Save Data Access Pattern
```csharp
// DO THIS (safe):
var saveData = SaveSystem.Instance.PlayerInfoSave;
int gold = saveData.get_Gold(); // Uses proper decryption

// DON'T DO THIS (broken under IL2CPP):
// [HarmonyPatch(typeof(PlayerInfoSave), "get_Gold")]  // Returns garbage!
```

### ObscuredInt Encryption
Currency and key values are XOR-encrypted. The ObscuredInt struct contains:
- The encrypted value
- The encryption key
Always use the game's getter/setter methods to access these — never read the raw field.

## Known Setter Methods (from Cheat Engine)
These can be targeted for Harmony Postfix patches:
- `PlayerInfoSave.set_bei` — Bei currency
- `PlayerInfoSave.set_ChefFlame` — Artisan's Flame (cooking resource)
- `PlayerInfoSave.set_Gold` — Gold currency (inferred)

## Filling in Harmony Patches

For each patch file in `client/DaveDiverAP/Patches/`, the process is:

1. Run Il2CppDumper on the game machine
2. Search for relevant class/method names
3. Replace the `// PLACEHOLDER` comments with real names
4. Add field access code to read data from `__instance`

### Priority order:
1. **FishCatchPatch.cs** — Hook `SuccessInteract()` on fish class (highest impact, 203 checks)
2. **StoryProgressPatch.cs** — Hook chapter/mission completion (10 story checks)
3. **BossDefeatedPatch.cs** — Hook boss defeat callback (16 boss checks)
4. **RecipeUnlockPatch.cs** — Hook recipe unlock + dish upgrade (600+ checks)
5. **WeaponCraftPatch.cs** — Hook Duff's shop craft completion (79 checks)

## Version Stability Warning

> Every game update changes a lot due to IL2CPP obfuscation.
> AOB (Array of Bytes) scanning is more robust than fixed offsets.
> Re-run Il2CppDumper after each major game update.

Tested BepInEx version: **6.0.0-be.752**
Game version at time of research: **v1.0.5.1791**

## Resources

- Il2CppDumper: https://github.com/Perfare/Il2CppDumper
- ILSpy: https://github.com/icsharpcode/ILSpy
- dnSpy: https://github.com/dnSpy/dnSpy
- BepInEx IL2CPP Pack for Dave the Diver: https://www.nexusmods.com/davethediver/mods/3
- Dave the Diver official Discord: https://discord.com/invite/davethediver
- FearLess Cheat Engine tables: https://fearlessrevolution.com/viewtopic.php?t=37355
