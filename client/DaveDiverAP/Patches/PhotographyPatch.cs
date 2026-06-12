using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches Tako's photography mission system.
    /// PLACEHOLDER class names — find via Il2CppDumper.
    /// Search for: "Photography", "Tako", "Camera", "PhotoMission"
    /// </summary>
    [HarmonyPatch]
    public static class PhotographyPatch
    {
        // Fires when a photography mission is completed
        [HarmonyPatch(typeof(PhotographyManager), "OnMissionComplete")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnMissionComplete_Postfix(int missionNumber)
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation($"Photography: Complete Mission {missionNumber}");
        }

        // Fires when a special photo spot is photographed (Giant Squid, Whale Shark, etc.)
        [HarmonyPatch(typeof(PhotographyManager), "OnSpecialPhotoTaken")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnSpecialPhotoTaken_Postfix(string subjectName)
        {
            if (!ArchipelagoClient.IsConnected) return;
            ArchipelagoClient.CheckLocation($"Photo: {subjectName}");
        }

        // Tracks total photo count milestones (50, 100)
        private static int _totalPhotos = 0;
        [HarmonyPatch(typeof(PhotographyManager), "OnPhotoTaken")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnPhotoTaken_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            _totalPhotos++;
            if (_totalPhotos == 50)  ArchipelagoClient.CheckLocation("Photography: Take 50 Photos");
            if (_totalPhotos == 100) ArchipelagoClient.CheckLocation("Photography: Take 100 Photos");
        }

        // Fires when 10 missions have been completed with perfect score
        [HarmonyPatch(typeof(PhotographyManager), "OnPerfectScoreAchieved")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnPerfectScoreAchieved_Postfix(int perfectCount)
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (perfectCount >= 10)
                ArchipelagoClient.CheckLocation("Photography: Perfect Score on 10 Missions");
        }
    }
}
