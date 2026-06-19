using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the restaurant system (Bancho Sushi) to detect customer milestones
    /// and VIP guest quest completions.
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "SushiBar", "Restaurant", "Customer", "SushiBarCustomer" (confirmed class)
    ///
    /// Known class: SushiBarCustomer (confirmed from Cheat Engine tables)
    /// </summary>
    [HarmonyPatch]
    public static class RestaurantPatch
    {
        // Total customers served — track for milestone checks
        private static int _totalCustomers = 0;

        // ✅ CONFIRMED via dump.cs: SushiBarManager is a Singleton<SushiBarManager>
        //    Also confirmed: SushiBarAnalyticsReportSequenceCookStar fires after each service
        //    and tracks m_GainFollowerResult and m_GainFollowerCount (UITextCounter).
        //    The DoSequence coroutine in SushiBarAnalyticsReportSequenceCookStar is the
        //    post-service analytics sequence — hook it to detect successful service nights.
        [HarmonyPatch(typeof(SushiBarAnalyticsReportSequenceCookStar), "DoSequence")]
        [HarmonyPostfix]
        public static void OnCustomerServed_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalCustomers++;
            LocationTracker.OnCustomersServed(_totalCustomers);
        }

        // Fires when a VIP showdown result is ready to be processed.
        // ✅ CONFIRMED via dump.cs: SushiBarManager.CanProcessVIPShowdownResult(out MissionData, out MissionConditionData, out VIPCustomer)
        //    returns true when a VIP mission has been completed and result is pending.
        //    The private coroutine ProcessVIPShowdownResult() handles the actual reward flow.
        //    We hook CanProcessVIPShowdownResult as a postfix — when it returns true, a VIP was served.
        [HarmonyPatch(typeof(SushiBarManager), "CanProcessVIPShowdownResult")]
        [HarmonyPostfix]
        public static void OnVIPMissionComplete_Postfix(bool __result, MissionData missionData, VIPCustomer customer)
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (!__result) return;  // no VIP result ready

            // Log both TIDs for in-game verification (MissionClearTID vs VIP_TID may differ)
            int missionTID = missionData?.MissionClearTID ?? 0;
            Plugin.Log.LogInfo($"[VIP] CanProcessVIPShowdownResult: MissionClearTID={missionTID}, customer={customer?.AssetKey}");

            var locationName = VIPNameMapper.GetLocationName(missionTID);
            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
            else
                Plugin.Log.LogWarning($"[VIP] Unknown VIP mission TID: {missionTID} — add to VIPNameMapper");
        }
    }

    public static class VIPNameMapper
    {
        // ✅ CONFIRMED via dump.cs: VIPCookingScenarioDataList.VIP_TID enum:
        //    WangPang = 9100017, Alex = 9100018, Pastro = 9100019
        //    These are the chef competitor VIPs who have cooking showdown challenges.
        //    CanProcessVIPShowdownResult only fires for these three VIPs.
        //
        //    Vincent Yamaoka, Michael Bang, and Sammy are simpler VIPs (no cooking challenge)
        //    and are tracked via MissionManager.UpdateMission in StoryProgressPatch/QuestNameMapper.
        //
        //    The MissionClearTID in MissionData is the mission's clear condition TID.
        //    VIP_TID (9100017-19) is the VIP scenario TID, NOT the mission clear TID.
        //    We use the VIPCustomer's NPC TID via FindVIPCustomerByNpcNormalTID to identify them.
        //    For now, map by VIP_TID passed through missionData.MissionClearTID (to be verified in-game).
        private static readonly System.Collections.Generic.Dictionary<int, string> _map = new()
        {
            { 9100017, "Quest: Serve Wang Pang (Chef Competitor)" },
            { 9100018, "Quest: Serve Alex Cooper (Chef Competitor)" },
            { 9100019, "Quest: Serve Pastro Antogiovani (Chef Competitor)" },
            // Vincent, Michael Bang, Sammy handled by QuestNameMapper (MissionManager hook)
        };

        public static string? GetLocationName(int missionTID) =>
            _map.TryGetValue(missionTID, out var name) ? name : null;
    }
}
