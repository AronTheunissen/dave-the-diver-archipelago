using System.Collections.Generic;

namespace DaveDiverAP
{
    /// <summary>
    /// Parses and holds the slot data received from the Archipelago server.
    /// This mirrors the options set in the player's YAML file.
    /// </summary>
    public class SlotData
    {
        // Victory condition
        public int Goal { get; }

        // Fish checks
        public int FishChecks { get; }         // 0=none, 1=rare_only, 2=all

        // Dish/recipe options
        public int DishUpgrades { get; }       // 0=none, 1=key, 2=popular, 3=all
        public int RecipeChecks { get; }       // 0=key_only, 1=all

        // Optional systems
        public bool IncludeCooksta { get; }
        public bool IncludeEcowatcher { get; }
        public bool IncludePhotography { get; }
        public bool IncludeChallenges { get; }
        public bool IncludeFarming { get; }
        public bool IncludeChickenFarm { get; }
        public bool IncludeFishFarm { get; }
        public bool IncludeMinigames { get; }
        public bool IncludeWeaponShop { get; }

        // DLC ownership
        public bool HasDredgeDLC { get; }
        public bool HasGodzillaDLC { get; }
        public bool HasIchibanDLC { get; }
        public bool HasJungleDLC { get; }

        // Starting equipment levels
        public int StartingOxygenLevel { get; }
        public int StartingHarpoonLevel { get; }
        public int StartingSuitLevel { get; }

        // Misc
        public bool DeathLink { get; }
        public int TrapFrequency { get; }

        public SlotData(Dictionary<string, object> data)
        {
            Goal                = GetInt(data, "goal", 0);
            FishChecks          = GetInt(data, "fish_checks", 2);
            DishUpgrades        = GetInt(data, "dish_upgrades", 1);
            RecipeChecks        = GetInt(data, "recipe_checks", 1);
            IncludeCooksta      = GetBool(data, "include_cooksta", true);
            IncludeEcowatcher   = GetBool(data, "include_ecowatcher", true);
            IncludePhotography  = GetBool(data, "include_photography", true);
            IncludeChallenges   = GetBool(data, "include_challenges", true);
            IncludeFarming      = GetBool(data, "include_farming", true);
            IncludeChickenFarm  = GetBool(data, "include_chicken_farm", true);
            IncludeFishFarm     = GetBool(data, "include_fish_farm", true);
            IncludeMinigames    = GetBool(data, "include_minigames", true);
            IncludeWeaponShop   = GetBool(data, "include_weapon_shop", true);
            HasDredgeDLC        = GetBool(data, "has_dredge_dlc", false);
            HasGodzillaDLC      = GetBool(data, "has_godzilla_dlc", false);
            HasIchibanDLC       = GetBool(data, "has_ichiban_dlc", false);
            HasJungleDLC        = GetBool(data, "has_jungle_dlc", false);
            StartingOxygenLevel = GetInt(data, "starting_oxygen_level", 1);
            StartingHarpoonLevel= GetInt(data, "starting_harpoon_level", 1);
            StartingSuitLevel   = GetInt(data, "starting_suit_level", 1);
            DeathLink           = GetBool(data, "death_link", false);
            TrapFrequency       = GetInt(data, "trap_frequency", 0);
        }

        private static int GetInt(Dictionary<string, object> data, string key, int defaultValue)
        {
            if (data.TryGetValue(key, out var val) && val is long l)
                return (int)l;
            return defaultValue;
        }

        private static bool GetBool(Dictionary<string, object> data, string key, bool defaultValue)
        {
            if (data.TryGetValue(key, out var val) && val is long l)
                return l != 0;
            return defaultValue;
        }
    }
}
