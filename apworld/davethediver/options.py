"""
Dave the Diver - YAML Options

This file defines player configuration options for generating seeds.
"""

from dataclasses import dataclass
from Options import (
    PerGameCommonOptions,
    Toggle,
    DefaultOnToggle,
    Range,
    Choice,
    OptionSet,
)


# === GOAL OPTIONS ===

class Goal(Choice):
    """What is required to complete the game?
    
    - Defeat Yawie: Defeat the final boss (fastest, ~15-25 hours)
    - Defeat All Bosses: Defeat Yawie + all optional bosses including vortex fights (~25-35 hours)
    - Diamond Rank: Defeat Yawie + reach Cooksta Diamond rank (720 followers, 375 best taste, 32 researched recipes) (long, ~30-40 hours)
    - Master Diver: Defeat Yawie + catch every fish species (complete MarinCa collection) (very long, ~50-70 hours)
    - 100% Completion: Everything - all bosses, Diamond rank, all fish caught (extreme, 100+ hours)
    """
    display_name = "Victory Condition"
    option_defeat_yawie = 0
    option_defeat_all_bosses = 1
    option_diamond_rank = 2
    option_master_diver = 3
    option_hundred_percent = 4
    default = 0  # Defeat Yawie


class ChaptersRequired(Range):
    """How many chapters must be completed (if using specific count goal)"""
    display_name = "Chapters Required"
    range_start = 1
    range_end = 7
    default = 7


# === FISH CATCHING OPTIONS ===

class FishChecks(Choice):
    """Which fish catches count as location checks?
    
    - None: No fish catching checks
    - Rare Only: Only rare and boss fish (~40 checks)
    - All Fish: Every fish species first catch (~100+ checks)
    """
    display_name = "Fish Catching Checks"
    option_none = 0
    option_rare_only = 1
    option_all = 2
    default = 2  # All fish


class RequireAllFish(Toggle):
    """Require catching all fish species for victory (only if goal includes it)"""
    display_name = "Require All Fish Caught"
    default = False


# === RESTAURANT OPTIONS ===

class DishUpgrades(Choice):
    """Which dish upgrades count as location checks?
    
    - None: No dish upgrade checks
    - Key Dishes: ~20 important dishes, 4 levels each (~80 checks)
    - Popular Dishes: ~50 commonly used dishes (~200 checks)
    - All Dishes: Every recipe in the game (~400+ checks)
    """
    display_name = "Dish Upgrade Checks"
    option_none = 0
    option_key_dishes = 1
    option_popular = 2
    option_all = 3
    default = 1  # Key dishes


class RecipeChecks(Choice):
    """Which recipe unlocks count as checks?
    
    - Key Only: Only progression/story recipes (~30 checks)
    - All Recipes: Every recipe in the game (~100+ checks)
    """
    display_name = "Recipe Unlock Checks"
    option_key_only = 0
    option_all = 1
    default = 1  # All recipes


class RequireAllRecipes(Toggle):
    """Require unlocking all recipes for victory (only if goal includes it)"""
    display_name = "Require All Recipes"
    default = False


class RestaurantRatingRequired(Range):
    """Minimum restaurant star rating required for victory (if goal includes it)"""
    display_name = "Required Restaurant Rating"
    range_start = 0
    range_end = 5
    default = 5


# === SIDE CONTENT OPTIONS ===

class IncludeCooksta(DefaultOnToggle):
    """Include Cooksta (social media) milestones as checks"""
    display_name = "Include Cooksta Checks"


class CookstaFollowersRequired(Range):
    """Cooksta followers needed for victory (if goal includes it)"""
    display_name = "Cooksta Followers Required"
    range_start = 0
    range_end = 10000
    default = 10000


class IncludeEcowatcher(DefaultOnToggle):
    """Include Ecowatcher (marine database) completion as checks"""
    display_name = "Include Ecowatcher Checks"


class IncludePhotography(DefaultOnToggle):
    """Include Tako's photography missions as checks"""
    display_name = "Include Photography Checks"


class IncludeChallenges(Toggle):
    """Include in-game challenges as checks (can be difficult!)"""
    display_name = "Include Challenge Checks"
    default = False


class IncludeFarming(DefaultOnToggle):
    """Include vegetable garden farming milestones as check locations"""
    display_name = "Include Vegetable Farming"


class IncludeChickenFarm(DefaultOnToggle):
    """Include chicken farm milestones as check locations.
    Note: The chicken farm is at the same physical location as the vegetable farm
    but is a separate unlockable system."""
    display_name = "Include Chicken Farm"


class IncludeFishFarm(DefaultOnToggle):
    """Include fish farm breeding/raising as checks"""
    display_name = "Include Fish Farm Checks"


class IncludeMinigames(DefaultOnToggle):
    """Include minigame completions as checks"""
    display_name = "Include Minigame Checks"


class IncludeWeaponShop(DefaultOnToggle):
    """Include Duff's Weapon Shop crafting as check locations.
    Each named weapon variant (e.g. Flame Rifle I, Thunderbolt Sniper Rifle) is a
    separate check — crafting it gives an AP reward."""
    display_name = "Include Weapon Shop"


class StaffTrainingDepth(Choice):
    """How deep does staff training go as checks?

    - None: No staff checks at all
    - Hire Only: One check per staff member when recruited (21 checks)
    - Milestones: Hire + Level 5/10/15/20 training checks (105 checks).
      Items are named staff members (e.g. 'Maki').
    - All Levels: Hire + every level 1-20 (420 checks).
      Items become 'Progressive [Name]' (×20) — finding Maki a 2nd time
      trains her to level 2, 3rd time to level 3, etc.
    """
    display_name = "Staff Training Depth"
    option_none = 0
    option_hire_only = 1
    option_milestones = 2
    option_all_levels = 3
    default = 2  # Milestones


class IncludeIngredientChecks(DefaultOnToggle):
    """Include first-find ingredient checks (sea plants, rare forageables, farm crops).
    Each ingredient gives one check the first time it's collected (~25 checks)."""
    display_name = "Include Ingredient Checks"


class IncludeSubMissions(DefaultOnToggle):
    """Include side quest / sub-mission completion checks (~25 checks).
    These are optional story missions like A Dolphin's Request, Whale Cry,
    Trapped in the Glacial Cave, etc. Toggle off for a more streamlined experience."""
    display_name = "Include Sub-Missions"


# === DLC OPTIONS ===
# Each DLC adds new content — only include it if the player actually owns it.
# DREDGE DLC is free; Godzilla was free but time-limited; others are paid.

class HasDredgeDLC(Toggle):
    """Enable content from the DREDGE Content Pack (free DLC).
    Adds: aberrant fish vortex regions (Jellyfish Basin, Fog Coast, Black Cliff),
    the Drain Gun weapon tree, and aberration sushi recipes."""
    display_name = "Has DREDGE DLC"


class HasGodzillaDLC(Toggle):
    """Enable content from the Godzilla Content Pack (free, time-limited DLC).
    Adds: Godzilla boss fight, monster figures collectibles, and Godzilla-themed recipes."""
    display_name = "Has Godzilla DLC"


class HasIchibanDLC(Toggle):
    """Enable content from the Ichiban's Holiday Content Pack (paid, time-limited DLC).
    Adds: Ichiban minigames and new sushi bar staff."""
    display_name = "Has Ichiban's Holiday DLC"


class HasJungleDLC(Toggle):
    """Enable content from the In the Jungle Content Pack (paid expansion, June 2026).
    Adds: jungle lake region, Bancho Grill restaurant, Utara Village, and new story content.
    Note: This DLC released June 18, 2026 — enable only if you own it."""
    display_name = "Has In the Jungle DLC"


# === PROGRESSION DIFFICULTY ===

class StartingOxygenLevel(Range):
    """How many progressive oxygen tank upgrades to start with (0-5)"""
    display_name = "Starting Oxygen Level"
    range_start = 0
    range_end = 5
    default = 1


class StartingHarpoonLevel(Range):
    """Which progressive harpoon to start with (0 = none, 1 = basic, 2 = enhanced, 3 = advanced)"""
    display_name = "Starting Harpoon Level"
    range_start = 0
    range_end = 3
    default = 1


class StartingSuitLevel(Range):
    """Which progressive diving suit to start with (0 = none, 1 = basic, 2 = enhanced, 3 = deep sea)"""
    display_name = "Starting Diving Suit Level"
    range_start = 0
    range_end = 3
    default = 1


class OxygenRequirement(Choice):
    """How many oxygen upgrades are needed to reach deep areas?
    
    - Lenient: Only 3 upgrades needed
    - Normal: 4 upgrades needed
    - Strict: 5 upgrades needed
    """
    display_name = "Oxygen Requirement Difficulty"
    option_lenient = 0
    option_normal = 1
    option_strict = 2
    default = 1


# === ADDITIONAL OPTIONS ===

class DeathLink(Toggle):
    """When you die, everyone dies. When someone else dies, you die."""
    display_name = "Death Link"
    default = False


class TeleportBehavior(Choice):
    """How teleport mirrors work
    
    - Required for Progression: Must find teleport destinations to access areas
    - Fast Travel Only: Areas accessible without teleports, mirrors just convenience
    """
    display_name = "Teleport Mirror Behavior"
    option_required = 0
    option_fast_travel_only = 1
    default = 0  # Required


class TrapItemFrequency(Choice):
    """How many trap items to include in the item pool
    
    - None: No traps
    - Low: ~5% traps
    - Medium: ~10% traps
    - High: ~15% traps
    """
    display_name = "Trap Item Frequency"
    option_none = 0
    option_low = 1
    option_medium = 2
    option_high = 3
    default = 0  # None (not implemented yet)


# === COMBINE ALL OPTIONS ===

@dataclass
class DaveDiverOptions(PerGameCommonOptions):
    """All options for Dave the Diver"""
    
    # Goal
    goal: Goal
    chapters_required: ChaptersRequired
    
    # Fish
    fish_checks: FishChecks
    require_all_fish: RequireAllFish
    
    # Restaurant
    dish_upgrades: DishUpgrades
    recipe_checks: RecipeChecks
    require_all_recipes: RequireAllRecipes
    restaurant_rating_required: RestaurantRatingRequired
    
    # Side content
    include_cooksta: IncludeCooksta
    cooksta_followers_required: CookstaFollowersRequired
    include_ecowatcher: IncludeEcowatcher
    include_photography: IncludePhotography
    include_challenges: IncludeChallenges
    include_farming: IncludeFarming
    include_chicken_farm: IncludeChickenFarm
    include_fish_farm: IncludeFishFarm
    include_minigames: IncludeMinigames
    include_weapon_shop: IncludeWeaponShop
    staff_training_depth: StaffTrainingDepth
    include_ingredient_checks: IncludeIngredientChecks
    include_sub_missions: IncludeSubMissions

    # DLC ownership
    has_dredge_dlc: HasDredgeDLC
    has_godzilla_dlc: HasGodzillaDLC
    has_ichiban_dlc: HasIchibanDLC
    has_jungle_dlc: HasJungleDLC

    # Progression difficulty
    starting_oxygen_level: StartingOxygenLevel
    starting_harpoon_level: StartingHarpoonLevel
    starting_diving_suit_level: StartingSuitLevel
    oxygen_requirement: OxygenRequirement
    
    # Additional
    death_link: DeathLink
    teleport_behavior: TeleportBehavior
    trap_frequency: TrapItemFrequency
