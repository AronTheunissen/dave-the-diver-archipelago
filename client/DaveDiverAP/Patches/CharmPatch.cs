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
        // ✅ CONFIRMED via dump.cs: Charm inventory uses AutoEquipCharmItem(int tid)
        //    and AddCharm property in save data (JsonProperty "AddCharm", int).
        //    The charm TID identifies which charm was acquired.
        //    We hook the inventory method that adds the charm item (AutoEquipCharmItem
        //    is called when a charm reward is granted and auto-equipped).
        //    CharmSpecData has the ability data; identified by TID from design sheet.
        [HarmonyPatch(typeof(LobbyCharmSwapPanel), "AutoEquipCharmItem")]
        [HarmonyPostfix]
        public static void OnCharmAcquired_Postfix(int tid)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var (charmName, sourceMission) = CharmMapper.GetCharmInfo(tid);
            if (charmName != null && sourceMission != null)
                ArchipelagoClient.CheckLocation($"Charm: {charmName} ({sourceMission})");
        }
    }

    public static class CharmMapper
    {
        // Maps charm TID (integer) to (charm name, source mission) for AP location checking.
        // TIDs confirmed via UnityExplorer CharmSpecData dump.
        private static readonly System.Collections.Generic.Dictionary<int, (string charm, string mission)> _map = new()
        {
            // ── Mission-acquired charms (base game) ──────────────────────────────
            { 3017001, ("Dolphin Necklace",      "Complete Defeat Pirates") },
            { 3017021, ("Octopus Bracelet",      "Complete Investigate the Strange Coral") },
            { 3017031, ("Sea People Bracelet",   "Complete Beyond the Rock Pile") },
            { 3017042, ("Octopus Weapon Charm",  "Complete Octopus Returns") },
            { 3017011, ("Sea People Necklace",   "Complete Deliver Key to Tenzhin") },
            { 3017044, ("Shark Teeth Necklace",  "Complete Revenge Time!") },
            // ── DLC charms ───────────────────────────────────────────────────────
            { 3017101, ("Leo Keychain",          "Complete EVIL FACTORY Demo") },
            { 3017049, ("Jimbo Coin",            "Complete Jimbo's Game Craze!") },
        };

        public static (string? charm, string? mission) GetCharmInfo(int tid)
        {
            if (_map.TryGetValue(tid, out var info))
                return info;
            return (null, null);
        }
    }
}
