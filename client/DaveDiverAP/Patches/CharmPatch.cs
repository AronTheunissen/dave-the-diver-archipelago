using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches charm acquisition from mission completions.
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "Charm", "Accessory", "Equipment", "CharmManager"
    ///
    /// Charms are rewarded by completing specific missions:
    /// - Dolphin Necklace:    Complete Defeat Pirates
    /// - Octopus Bracelet:    Complete Investigate the Strange Coral
    /// - Sea People Bracelet: Complete Beyond the Rock Pile
    /// - Octopus Weapon Charm:Complete Octopus Returns
    /// - Sea People Necklace: Complete Deliver Key to Tenzhin (already in StoryProgressPatch)
    /// - Shark Teeth Necklace:Complete Revenge Time!
    /// - Leo Keychain:        DREDGE DLC
    /// - Jimbo Coin:          Jimbo's Game Craze! mission
    /// Ecowatcher charms are handled by EcowatcherPatch.
    /// </summary>
    [HarmonyPatch]
    public static class CharmPatch
    {
        // Fires when a charm is added to the player's inventory
        [HarmonyPatch(typeof(CharmManager), "OnCharmAcquired")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnCharmAcquired_Postfix(string charmId)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var (charmName, sourceMission) = CharmMapper.GetCharmInfo(charmId);
            if (charmName != null && sourceMission != null)
                ArchipelagoClient.CheckLocation($"Charm: {charmName} ({sourceMission})");
        }
    }

    public static class CharmMapper
    {
        private static readonly System.Collections.Generic.Dictionary<string, (string charm, string mission)> _map = new()
        {
            // TODO: Fill in real charm IDs from Il2CppDumper
            // { "CHARM_DOLPHIN",        ("Dolphin Necklace",      "Complete Defeat Pirates") },
            // { "CHARM_OCTOPUS",        ("Octopus Bracelet",      "Complete Investigate the Strange Coral") },
            // { "CHARM_SEA_BRACELET",   ("Sea People Bracelet",   "Complete Beyond the Rock Pile") },
            // { "CHARM_WEAPON",         ("Octopus Weapon Charm",  "Complete Octopus Returns") },
            // { "CHARM_SEA_NECKLACE",   ("Sea People Necklace",   "Complete Deliver Key to Tenzhin") },
            // { "CHARM_SHARK",          ("Shark Teeth Necklace",  "Complete Revenge Time!") },
            // { "CHARM_LEO",            ("Leo Keychain",          "Complete EVIL FACTORY Demo") },
            // { "CHARM_JIMBO",          ("Jimbo Coin",            "Complete Jimbo's Game Craze!") },
        };

        public static (string? charm, string? mission) GetCharmInfo(string charmId)
        {
            if (_map.TryGetValue(charmId, out var info))
                return info;
            return (null, null);
        }
    }
}
