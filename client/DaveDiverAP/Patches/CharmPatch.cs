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
        //    We hook AutoEquipCharmItem — but guard with ItemQueue.IsGameReady so that
        //    the load-time auto-equip (restore from save) is ignored.
        //
        // 🛡️ LOAD GUARD: IsGameReady is false during save deserialization, so the crash
        //    that previously occurred on "Continue" is prevented.
        [HarmonyPatch(typeof(LobbyCharmSwapPanel), "AutoEquipCharmItem")]
        [HarmonyPostfix]
        public static void OnCharmAcquired_Postfix(int tid)
        {
            try
            {
                // Guard: skip load-time auto-equip (IsGameReady is false during save loading)
                if (!ItemQueue.IsGameReady) return;
                if (!ArchipelagoClient.IsConnected) return;

                var charmInfo = CharmMapper.GetCharmInfo(tid);
                var charmName = charmInfo.charm;
                var sourceMission = charmInfo.mission;
                if (charmName != null && sourceMission != null)
                {
                    Plugin.Log.LogInfo($"[Charm] Acquired: {charmName} via {sourceMission} (TID={tid})");
                    ArchipelagoClient.CheckLocation($"Charm: {charmName} ({sourceMission})");
                }
                else
                {
                    Plugin.Log.LogInfo($"[Charm] Unknown charm TID={tid} (add to CharmMapper if needed)");
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[CharmPatch] OnCharmAcquired_Postfix threw: {ex}");
            }
        }
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
