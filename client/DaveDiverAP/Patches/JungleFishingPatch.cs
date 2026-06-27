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
            { 42013501, "Moonlight Gourami" },   // confirmed 2026-06-27
            { 42013502, "Three Spot Gourami" },  // confirmed 2026-06-27
            { 42013503, "Malayan Leaf Fish" },    // confirmed 2026-06-27
            { 42013504, "Snakeskin Gourami" },   // confirmed 2026-06-27
            { 42013505, "Giant Gourami" },       // confirmed 2026-06-27
            { 42013506, "Emperor Snakehead" },   // confirmed 2026-06-27
            { 42013507, "Striped Snakehead" },   // confirmed 2026-06-27
            // 42013508 — not yet caught
            { 42013509, "Peacock Bass" },        // confirmed 2026-06-27
            { 42013510, "Tambaqui" },            // confirmed 2026-06-27
            { 42013511, "Malayan Mahseer" },     // confirmed 2026-06-27
            { 42013512, "Redtail Catfish" },     // confirmed 2026-06-27
            { 42013513, "Tapah" },               // confirmed 2026-06-27
            // All 12 rod fish confirmed (2026-06-27): 42013508 is unused/skipped
            // Complete list: Moonlight Gourami, Three Spot Gourami, Malayan Leaf Fish,
            // Snakeskin Gourami, Giant Gourami, Emperor Snakehead, Striped Snakehead,
            // Peacock Bass, Tambaqui, Malayan Mahseer, Redtail Catfish, Tapah
        };

        public static string? GetDisplayName(int tid) =>
            _map.TryGetValue(tid, out var name) ? name : null;
    }
}
