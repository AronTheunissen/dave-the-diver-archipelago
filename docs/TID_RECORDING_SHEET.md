# Dave the Diver — TID Recording Sheet

Use this sheet during your UnityExplorer session to record the TID numbers needed
to fill in the patch file dictionaries in the C# client mod.

**How to open UnityExplorer:** Launch the game → press **F7**

---

## ⚡ Fastest Approach: C# Console Dumps

Before browsing manually, try these commands in the **C# Console** tab.
Each one dumps a whole category at once — screenshot the output!

```csharp
// All unlocked recipe TIDs
foreach (var kv in SaveData.Instance.GetAllUnlockRecipes())
    UnityExplorer.ExplorerCore.Log($"{kv.Key} = {kv.Value}");

// All mission TIDs (covers quests, challenges, ecowatcher)
foreach (var m in MissionManager.Instance.GetAllMissions())
    UnityExplorer.ExplorerCore.Log($"{m.TID} = {m.name}");

// All charm TIDs
foreach (var c in CharmSpecData.GetAll())
    UnityExplorer.ExplorerCore.Log($"{c.TID} = {c.name}");

// All ingredient TIDs
foreach (var i in IngredientsData.GetAll())
    UnityExplorer.ExplorerCore.Log($"{i.TID} = {i.name}");
```

For **weapons**, go to Duff's shop and craft/browse — each craft fires
`WeaponCraftTreeEventTrigger(craftID, row, col)` which will print to the
BepInEx console log if you add a temporary log line to `WeaponCraftPatch.cs`.
Alternatively, search for `GunSpecData` in Object Search.

---

## 🔫 Weapons
*Screen: Duff's weapon shop — search `GunSpecData` in Object Search*

| Weapon Name | TID |
|---|---|
| Basic Underwater Rifle | |
| Underwater Rifle II | |
| Underwater Rifle III | |
| Death Rifle | |
| Flame Rifle I | |
| Flame Rifle II | |
| Explosive Rifle | |
| Tranquilizer Rifle | |
| Poison Rifle I | |
| Poison Rifle II | |
| Hell Poison Rifle | |
| Lightning Rifle I | |
| Lightning Rifle II | |
| Shock Rifle I | |
| Shock Rifle II | |
| Thunderbolt Rifle | |
| Small Net Gun | |
| Medium Net Gun | |
| Large Net Gun | |
| Steel Net Gun | |
| Hush Dart | |
| Enhanced Hush Dart | |
| Triple Axel | |
| Quattro Axel | |
| Quattro Axel II | |
| Penta Axel | |
| Flame Triple Axel | |
| Flame Triple Axel II | |
| Explosive Triple Axel | |
| Tranquilizer Triple Axel | |
| Poison Triple Axel | |
| Poison Triple Axel II | |
| Hell Poison Triple Axel | |
| Lightning Triple Axel | |
| Shock Triple Axel | |
| Shock Triple Axel II | |
| Thunderbolt Triple Axel | |
| Red Sniper Rifle | |
| Red Sniper Rifle II | |
| RSR III | |
| Death Sniper Rifle | |
| Flame Sniper Rifle I | |
| Flame Sniper Rifle II | |
| Explosive Sniper Rifle | |
| Tranquilizer Mosin-Nagant | |
| Poison Sniper Rifle I | |
| Poison Sniper Rifle II | |
| Hell Poison Sniper Rifle | |
| Lightning Sniper Rifle I | |
| Lightning Sniper Rifle II | |
| Shock Sniper Rifle I | |
| Shock Sniper Rifle II | |
| Thunderbolt Sniper Rifle | |
| Sticky Bomb Gun | |
| Sticky Bomb Gun II | |
| Sticky Bomb Gun III | |
| Sticky Mine Launcher I | |
| Sticky Mine Launcher II | |
| Sticky Tranquilizing Bomb Gun | |
| Poison Mine Launcher | |
| Poison Mine Launcher II | |
| Lightning Mine Launcher I | |
| Lightning Mine Launcher II | |
| Shock Mine Launcher I | |
| Shock Mine Launcher II | |
| Grenade Launcher | |
| Grenade Launcher II | |
| Grenade Launcher III | |
| Tranquilizer Gas Bomb Launcher | |
| Poison Launcher | |
| Gravity Launcher | |
| Blackhole Launcher | |
| Flash Grenade Launcher | |
| Ice Gun | |
| Enhanced Ice Gun | |
| Ultra Ice Gun | |
| Drain Gun | |
| Enhanced Drain Gun | |
| Power Drain Gun | |

---

## 🍣 Recipes
*Run the C# Console dump above while in the restaurant — it lists all at once.*
*There are ~150 recipes so a dump is much faster than finding them one by one.*

Paste the dump output here, or record individual ones below as needed:

```
(paste dump output here)
```

---

## 🎪 Challenges
*Run the MissionManager dump above at the challenge board — filter results by name*

| Challenge Name | TID |
|---|---|
| Challenge: Catch 5 Fish in 60 Seconds | |
| Challenge: Catch 10 Fish in 90 Seconds | |
| Challenge: Defeat 3 Bosses in One Dive | |
| Challenge: Complete a Dive Without Taking Damage | |
| Challenge: Catch a Fish of Every Type in One Dive | |
| Challenge: Serve 10 Customers in One Night | |
| Challenge: Earn 5,000 Bei in One Night | |
| Challenge: Complete 3 VIP Orders | |
| Challenge: Catch a Rank 9 Fish | |

---

## 📖 Story Quests
*Same MissionManager dump — look for quest/story entries*

| Quest Name | TID |
|---|---|
| Quest: Complete Duff's First Request | |
| Quest: Help Dr. Bacon | |
| Quest: Find the Sea People Village | |
| Quest: Defeat the Cobra Gang | |
| Quest: Deliver the Cargo Box | |
| Quest: Win Sea People's Trust | |
| Quest: Deliver Key to Tenzhin | |
| Quest: Duff's Dream Concert | |
| Quest: Serve All VIP Guests | |
| Quest: Complete Otto's Gift | |

---

## 💎 Charms
*Run the CharmSpecData dump above, or search `CharmSpecData` in Object Search*

| Charm Name | TID |
|---|---|
| Dolphin Necklace | |
| Octopus Bracelet | |
| Sea People Bracelet | |
| Octopus Weapon Charm | |
| Sea People Necklace | |
| Shark Teeth Necklace | |
| Leo Keychain | |
| Jimbo Coin | |
| Eco Poison Resist Bracelet | |
| Eco Health Bracelet | |
| Eco Gemstone Bracelet | |
| Eco Waterproof Bag | |

---

## 🌿 Ingredients
*Run the IngredientsData dump above, or pick up a sea plant and search `IngredientsData`*

| Ingredient Name | TID |
|---|---|
| Agar | |
| Kajime | |
| Seaweed | |
| Kelp | |
| Sea Grape | |
| Bladderwrack | |
| Hyalonema | |
| Southern Bull Kelp | |
| Black Coral | |
| Buckbean | |
| Truffle | |
| Rainbow Cap | |

---

## 🍽️ VIP Guests
*Serve a VIP in the restaurant and watch the MissionManager or SushiBarManager fire*

| VIP Name | String ID / TID |
|---|---|
| Vincent Yamaoka | |
| Gourmet Pastro | |
| *(note down any others that appear)* | |

---

## 📝 Notes
*(Use this space for anything unexpected you find)*

