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
    
    - Complete Final Chapter: Beat Chapter 6 only (fastest, ~5-10 hours)
    - Complete All Chapters: Beat all 6 chapters in any order (medium, ~15-25 hours)
    - Complete Main Story + Cooksta: All chapters + 10,000 Cooksta followers (long, ~30-40 hours)
    - Restaurant Tycoon: All chapters + 5-star restaurant + all key recipes (long, ~30-40 hours)
    - Master Diver: All chapters + all Ecowatcher entries + catch all fish species (very long, ~50-70 hours)
    - 100% Completion: Everything - all chapters, all fish, all recipes, all farms, all photos (extreme, 100+ hours)
    """
    display_name = "Victory Condition"
    option_final_chapter_only = 0
    option_all_chapters = 1
    option_chapters_plus_cooksta = 2
    option_restaurant_tycoon = 3
    option_master_diver = 4
    option_hundred_percent = 5
    default = 1  # All chapters


class ChaptersRequired(Range):
    """How many chapters must be completed (if using specific count goal)"""
    display_name = "Chapters Required"
    range_start = 1
    range_end = 6
    default = 6


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
    """Include vegetable garden farming as checks"""
    display_name = "Include Farming Checks"


class IncludeFishFarm(DefaultOnToggle):
    """Include fish farm breeding/raising as checks"""
    display_name = "Include Fish Farm Checks"


class IncludeMinigames(DefaultOnToggle):
    """Include minigame completions as checks"""
    display_name = "Include Minigame Checks"


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
    include_fish_farm: IncludeFishFarm
    include_minigames: IncludeMinigames
    
    # Progression difficulty
    starting_oxygen_level: StartingOxygenLevel
    starting_harpoon_level: StartingHarpoonLevel
    starting_diving_suit_level: StartingSuitLevel
    oxygen_requirement: OxygenRequirement
    
    # Additional
    death_link: DeathLink
    teleport_behavior: TeleportBehavior
    trap_frequency: TrapItemFrequency
