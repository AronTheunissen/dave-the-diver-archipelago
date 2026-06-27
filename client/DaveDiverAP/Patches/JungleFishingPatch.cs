using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the Jungle DLC fishing rod system to detect first catches.
    ///
    /// The fishing rod system is completely separate from the underwater FishInteractionBody system.
    /// Rod fish are not present as scene objects — they're data-only until caught.
    ///
    /// Hook confirmed via UnityExplorer (2026-06-27):
    /// - JDLC.Fishing.FishingGameManager.Instance.FishingContext.FishInfo.TID = fish TID
    /// - JDLC.Fishing.FishingGameManager.Instance.FishingContext.FishInfo.IsCaught = true on catch
    /// - TID range for rod fish: 42013xxx (e.g. 42013501 = Moonlight Gourami)
    ///
    /// Best hook: JDLC.Fishing.FishingResultPanel — shown after every catch.
    /// Alternative: Hook FishingStateManager state transition to "Caught" state.
    /// </summary>
    [HarmonyPatch]
    public static class JungleFishingPatch
    {
        // ✅ Hook FishingResultPanel.Show() — fired when the catch result UI appears
        // This is the most reliable hook since it fires exactly once per catch
        [HarmonyPatch(typeof(JDLC.Fishing.FishingResultPanel), "Show")]
        [HarmonyPostfix]
        public static void OnFishingResultShown_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;

            // Get the caught fish TID from FishingGameManager
            var fm = JDLC.Fishing.FishingGameManager.Instance;
            if (fm == null) return;

            var ctx = fm.FishingContext;
            if (ctx == null) return;

            var fishInfo = ctx.FishInfo;
            if (fishInfo == null) return;

            // Only process if the fish was actually caught (not escaped)
            if (!fishInfo.IsCaught) return;

            int tid = fishInfo.TID;

            // Look up the fish name from TID
            var fishName = RodFishNameMapper.GetDisplayName(tid);

            if (fishName != null)
            {
                Log.LogInfo($"[RodFishCaught] TID={tid} → Location=\"First Catch: {fishName}\"");
                LocationTracker.OnFirstFishCatch(fishName);
            }
            else
            {
                Log.LogInfo($"[RodFishCaught] TID={tid} → UNMAPPED (add to RodFishNameMapper)");
            }
        }
    }

    /// <summary>
    /// Maps rod fish TIDs to AP location display names.
    /// TID range: 42013xxx — confirmed via FishingGameManager.FishingContext.FishInfo.TID
    /// </summary>
    public static class RodFishNameMapper
    {
        // TIDs confirmed via UnityExplorer FishInfo.TID inspection (2026-06-27)
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            // ── Jungle rod fish (42013xxx range) ─────────────────────────────
            { 42013501, "Moonlight Gourami" },  // confirmed 2026-06-27
            // TODO: catch remaining rod fish species to fill in TIDs
            // Expected species (from Jungle DLC fish lists):
            // { 42013xxx, "Siamese Fighting Fish" },
            // { 42013xxx, "Banded Archer Fish" },
            // { 42013xxx, "Giant Gourami" },
            // { 42013xxx, "Spotted Snakehead" },
            // Add more as you catch them — check BepInEx log for [RodFishCaught] TID=xxxxx UNMAPPED
        };

        public static string? GetDisplayName(int tid) =>
            _map.TryGetValue(tid, out var name) ? name : null;
    }
}
