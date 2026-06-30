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
        // ⚠️ DISABLED: AutoEquipCharmItem fires during load (charms are auto-equipped when
        //    save data is restored). This caused a silent crash on "Continue" from main menu.
        //
        // ✅ BETTER HOOK: Hook MissionManager.UpdateMission() and filter for charm reward missions
        //    by checking if the mission TID matches known charm-granting missions.
        //    MissionManager only fires during active gameplay, not during save load.
        //
        // [HarmonyPatch(typeof(LobbyCharmSwapPanel), "AutoEquipCharmItem")]
        // [HarmonyPostfix]
        // public static void OnCharmAcquired_Postfix(int tid)
        // {
        //     if (!ArchipelagoClient.IsConnected) return;
        //     var (charmName, sourceMission) = CharmMapper.GetCharmInfo(tid);
        //     if (charmName != null && sourceMission != null)
        //         ArchipelagoClient.CheckLocation($"Charm: {charmName} ({sourceMission})");
        // }
    }

    public static class CharmMapper
    {
        // Maps charm TID (integer) to (charm name, source mission) for AP location checking.
        // TIDs confirmed via UnityExplorer CharmSpecData dump (2026-06-27).
        private static readonly System.Collections.Generic.Dictionary<int, (string charm, string mission)> _map = new()
        {
            // ── Mission-acquired charms (base game) ──────────────────────────────
            // TID cross-reference: dump name → internal name → confirmed TID
            { 3017001, ("Dolphin Necklace",      "Complete Defeat Pirates") },           // LongDash
            { 3017021, ("Octopus Bracelet",      "Complete Investigate the Strange Coral") }, // ShortDash
            { 3017011, ("Sea People Bracelet",   "Complete Beyond the Rock Pile") },     // ExtraTime
            { 3017042, ("Octopus Weapon Charm",  "Complete Octopus Returns") },          // WeaponDMG_UP
            { 3017043, ("Sea People Necklace",   "Complete Deliver Key to Tenzhin") },   // UVField
            { 3017044, ("Shark Teeth Necklace",  "Complete Revenge Time!") },            // HarpoonDMG_UP
            // ── Ecowatcher charms ────────────────────────────────────────────────
            { 3017041, ("Eco Poison Resist Bracelet", "Ecowatcher Level 2") },           // Poison_resist
            { 3017031, ("Eco Health Bracelet",        "Ecowatcher Level 3") },           // Defense10
            { 3017045, ("Eco Gemstone Bracelet",      "Ecowatcher Level 4") },           // MiningBonus
            { 3017046, ("Eco Waterproof Bag",         "Ecowatcher Level 5") },           // LootboxWeightBonus
            // ── DLC charms ───────────────────────────────────────────────────────
            { 3017101, ("Leo Keychain",          "Complete EVIL FACTORY Demo") },        // EF_LeoHead (DREDGE DLC)
            { 3017049, ("Jimbo Coin",            "Complete Jimbo's Game Craze!") },      // JimboCombo
            // ── Jungle DLC charms (max villager friendship) ──────────────────────
            { 43017101, ("Crocodile Tooth Necklace", "Complete Operation: Sulong Hunt") },
            { 43017102, ("Charm of Abundance",       "Max Friendship: Panutah") },
            { 43017103, ("Anti-Gravity Device",      "Max Friendship: Muna") },
            { 43017104, ("Gold Necklace of Sloth",   "Max Friendship: Harta") },
            { 43017105, ("Bracelet of Strength",     "Max Friendship: Uzme") },
            { 43017106, ("Air Resonance Necklace",   "Max Friendship: Bonita") },
        };

        public static (string? charm, string? mission) GetCharmInfo(int tid)
        {
            if (_map.TryGetValue(tid, out var info))
                return info;
            return (null, null);
        }
    }
}
