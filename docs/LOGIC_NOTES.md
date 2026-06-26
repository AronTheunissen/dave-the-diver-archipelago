# Logic Notes — Dave the Diver Archipelago

> Last updated: June 26, 2026
> This document tracks what logic rules are implemented and what still needs work.

---

## ✅ Implemented Logic Rules

### Region Access
| Region | Gate |
|---|---|
| Blue Hole Mid (50-130m) | Suit Lv2+ OR 2 oxygen tanks |
| Blue Hole Deep (130-250m) | Suit Lv3+ OR 3 O2 tanks + Harpoon Lv1 |
| Glacial Passage | Key to Tenzhin + Suit Lv7 (Cold-Resistant) OR teleport |
| Glacier Zone | Suit Lv8 + Tech Suit Parts ×3 OR teleport |
| Hydrothermal Vents | Glacier Zone access + Heat-Resistant Gloves |
| Sea People Village | (Sea People Gloves OR Teleport) AND Translator |
| Fish Farm | Unlock Fish Farm item |
| Vegetable Farm | Unlock Vegetable Farm item |
| Chicken Farm | Unlock Chicken Farm item |
| Vortex regions (DREDGE) | Vortex Entry + Unlock Chicken Farm (red fog) |
| Jungle regions | Jungle DLC enabled |

### Boss Gates
| Boss | Gate |
|---|---|
| Giant Squid | Gas Cutter + Mid depth |
| Clione Queen | Bug Net + Deep depth |
| Truck Hermit Crab | Sea People Gloves (Stormy Night) |
| Giant Wolf Eel | Sea People Gloves + Headlamp + Deep |
| Goblin Shark | Salvage Drone + Underwater Camera + Deep |
| Phantom Jellyfish | Cold Suit (Lv7) + Beluga Whale Ride Whistle |
| Giant Gadon | Glacial Passage + Cobra's Lost Crowbar |
| Helicoprion | Cold Suit (Lv7) |
| Kronosaurus | Heat-Resistant Gloves |
| John Watson | Translator + Cold Suit (Lv7) |
| Mantis Shrimp | Truck Hermit Crab defeated |
| Great White Shark Klaus | Mantis Shrimp defeated + Clara's Omani |
| Lusca | Marinca Completion Trophy + Sea People Gloves |
| Yawie | 3 Control Room Buttons + Laser Device + Suit Lv7 |
| Ebirah | Chapter 5 Complete (Godzilla DLC) |
| Torben | Chapter 5 + Cocktails Unlocked (Ichiban DLC) |
| Caiman | Jungle DLC + Chapter 1-2 |
| Snapping Turtle | Jungle DLC + Utara Lake Upper access + appropriate tools |
| Sulong | Jungle DLC + Lakebed Sea access + Bug Net |
| Stethacanthus | Jungle DLC + Lakebed Sea deep + appropriate fishing |
| Xiphactinus | Jungle DLC + Lakebed Sea deep + appropriate fishing |
| Basilosaurus | Jungle DLC + Final jungle chapter gates |

### Item Chains
| Item | Source |
|---|---|
| Gas Cutter | Story: The Leahs-chan Rescue (Ch1) |
| Headlamp | Defeat Giant Squid (completing Leahs-chan) |
| Underwater Camera | Dr. Bacon after Beyond the Rock Pile |
| Sea People Necklace | Story: Deliver Key to Tenzhin (Ch4) |
| Beluga Whale Ride Whistle | Sub-Mission: Daphne's Whistle |
| Cobra's Lost Crowbar | Story: Cobra's Lost Crowbar (Ch5) |
| Cocktails Unlocked | Competition: Defeat Alex Cooper |
| Marinca Completion Trophy | Marinca: Complete All Entries |
| Night Dive Unlock | Sub-Mission: Giant Stingray at Night |

### Quest Chains & Prerequisites
| Location | Requires |
|---|---|
| What Happened to the Dolphins? | A Dolphin's Request done |
| Defeat Pirates | What Happened to the Dolphins? done |
| Defeat Clione Queen | Catch Clione done |
| Finding the Baby Whale | Whale Cry done |
| Repair Kinglong's Statue | Offer Flowers done |
| Catch Runaway Seahorses | Bug Net |
| Talk to Yami | Bug Net + Catch Runaway Seahorses done |
| Pet Squid Selgio | Bug Net |
| Curious Child | Sea People Necklace |
| Stormy Night | Sea People Gloves |
| Weaponsmith Duff | Always accessible (unlocks weapon shop) |
| All Craft: locations | Weaponsmith Duff done (when sub-missions on) |
| All Cooksta: locations | A Scolding from Yoshie done (when sub-missions on) |
| All Photo: locations | Underwater Camera |
| Photo: Pink Dolphin | Camera + What Happened to Dolphins? |
| Photo: Manta Ray | Camera + Night Dive + Manta Ray sub-mission |
| Photo: Baby Humpback Whale | Camera + Finding the Baby Whale |
| Photo: Loggerhead Turtle | Camera + Stormy Night |
| Photo: Underwater Lake | Camera + Curious Child |
| Otto's Gift? | A Noisy Customer done |
| Jango's Secret Recipe | Mid depth + Vegetable Farm |
| Mxmtoon | Sea People Gloves + Mid depth |
| Michael Bang's Inspiration | Vegetable Farm |
| Competition: Vincent | Mid depth |
| Competition: Wang Pang | Beat Vincent + Deep + Chicken Farm |
| Competition: Alex Cooper | Beat Wang Pang + Deep |
| Competition: Pastro | Beat Alex + Mid + Night Dive + Veg Farm |
| Ichiban DLC all | Chapter 5 + Cocktails Unlocked |
| Godzilla figurines + recipes | Defeat Ebirah |
| Godzilla dish upgrades | Defeat Ebirah |
| All dish upgrades | Unlock Recipe done for that dish |
| Staff training all levels | Having the named staff member |
| Ecowatcher glacial (13) | Glacial Passage access |
| Ecowatcher vents (2) | Hydrothermal Vents access |
| Ecowatcher deep (8) | Blue Hole Deep access |
| Night-only fish (15) | Night Dive Unlock |

---

## ❓ Logic Still Needed / Uncertain

### Items Without Confirmed Source Quests
| Item | Status |
|---|---|
| Salvage Drone | Given by Cobra — exact quest/trigger unknown. Currently ungated. |
| Sea People's Trust | Otto's quest chain — exact trigger unclear |
| Vortex Entry (×5) | Gated by Chicken Farm + DREDGE DLC ✅ but exact per-vortex gate unknown |

### Locations Without Full Gates
| Location | Issue |
|---|---|
| `Sub-Mission: Assisting Ellie` | What triggers Ellie's mission? |
| `Sub-Mission: Reticent Girl` | What triggers this? Who is the reticent girl? |
| `Sub-Mission: Sea Person at the Workshop` | Any prerequisites in the village? |
| `Sub-Mission: Wedding Song Record` | Any prerequisites? |
| `Sub-Mission: Find the Children's Ball` | Any prerequisites? |
| `Sub-Mission: Stormy Night` | Gated by Sea People Gloves ✅ but is it the right trigger? |
| Ecowatcher Marine Plants 3-5 | Need Sea Grape (Limestone Cave = Mid) — region covers it ✅ |

### Seasonal Events
| Event | Notes |
|---|---|
| Tuna Party, Cucumber Party, etc. | In-game limited events for bonus money — no AP relevance ✅ |

### Recipe Logic
| Item | Status |
|---|---|
| Fish sushi recipes | Auto-unlock on first catch — ungated (depth handles it) ✅ |
| Boss recipes | Gated on boss defeat ✅ |
| VIP/Cooksta recipes | Gated on quest/rank ✅ |
| Staff training recipes | Gated on having staff + training level (via dish upgrade rules) ✅ |

### Jungle DLC
| Content | Status |
|---|---|
| 57 lake fish + 6 boss fish | ✅ Fully implemented |
| 36 insects (net + battle beetles) | ✅ Fully implemented with TIDs 40001-40038 |
| 32 skewer recipes | ✅ Fully implemented with TIDs 48150001-48150109 |
| 71 Bancho Grill complex + boss recipes | ✅ Fully implemented with confirmed TIDs |
| 24 Jungle Gun upgrades | ✅ Fully implemented (4 modes × 6 levels) |
| 9 jungle staff unlocks | ✅ Structure complete, gate rules implemented |
| Villager friendship (14 villagers × 2 tiers) | ✅ Structure complete, details pending wiki data |
| Boss logic (6 bosses) | ✅ Structure complete, gate rules implemented |
| Jungle ingredient details | 🟡 TIDs confirmed, wiki unlock data needed |
| Dr. Bacon jungle research | ❓ Structure needed, triggers unknown |

---

## 🔴 Known Logic Gaps (Could Cause Issues)

1. **Salvage Drone** has no source quest gate — could be received before it's normally available
2. **Sea People Bracelet** (charm) — skill-based secret, currently an AP item with no location gate
3. **iDiver App** — removed as a gate (all upgrades are progressive items), 3 iDiver checks added
4. **TID Mapper Dictionaries** — still empty for weapons/recipes/quests (not logic, but needed for client)

---

## 📋 Logic Design Principles

1. **Region access is the primary gate** — depth/area access handles most location availability
2. **Lenient depth gating** — OR logic (suit OR oxygen) prevents players being hard-blocked
3. **DLC content isolated** — regions only connected when DLC option enabled
4. **Quest chains use can_reach()** — the correct AP primitive for checking location completion
5. **Items use state.has()** — for key items received from the AP multiworld
6. **No calendar/RNG gates** — seasonal events and random encounters are not gated
7. **Progression first** — DLC items are filtered before the "always keep progression" rule
