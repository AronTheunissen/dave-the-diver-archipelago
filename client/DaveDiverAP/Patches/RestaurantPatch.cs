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

        // ✅ CONFIRMED: SushiBarManager is the real class name (devopsdinosaur mod)
        // ✅ CONFIRMED: SushiBarCustomer and SushiBarStaffBase also confirmed
        // Method names still need confirming via Il2CppDumper
        // Search for: "OnServe", "OnCustomerLeave", "CustomerComplete" in SushiBarManager
        // Fires when a customer is successfully served and leaves happy
        [HarmonyPatch(typeof(SushiBarManager), "OnCustomerServed")]  // class confirmed, method still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnCustomerServed_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalCustomers++;
            LocationTracker.OnCustomersServed(_totalCustomers);
        }

        // Fires when a VIP mission is completed (e.g. serving Vincent, Michael Bang etc.)
        // VIP missions have unique quest IDs mapped to their AP location names
        [HarmonyPatch(typeof(SushiBarManager), "OnVIPMissionComplete")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnVIPMissionComplete_Postfix(string vipId)
        {
            if (!ArchipelagoClient.IsConnected) return;

            var locationName = VIPNameMapper.GetLocationName(vipId);
            if (locationName != null)
                ArchipelagoClient.CheckLocation(locationName);
        }
    }

    public static class VIPNameMapper
    {
        private static readonly System.Collections.Generic.Dictionary<string, string> _map = new()
        {
            // TODO: Fill in with real VIP IDs from Il2CppDumper
            // { "VIP_VINCENT",  "Quest: Serve Vincent Yamaoka (The Gourmet)" },
            // { "VIP_MICHAEL",  "Quest: Serve Michael Bang (Movie Director)" },
            // { "VIP_SAMMY",    "Quest: Serve Sammy (Rapper)" },
            // { "VIP_WANG",     "Quest: Serve Wang Pang (Chef Competitor)" },
            // { "VIP_ALEX",     "Quest: Serve Alex Cooper (Chef Competitor)" },
            // { "VIP_PASTRO",   "Quest: Serve Pastro Antogiovani (Chef Competitor)" },
        };

        public static string? GetLocationName(string vipId) =>
            _map.TryGetValue(vipId, out var name) ? name : null;
    }
}
