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
        // ✅ CONFIRMED via dump.cs: PhotoZone is the real class (MiniGameBase<PhotoZone.Data, PhotoZone.Result>)
        //    Fields: photozoneTID (int), OnEnterPhotoMode (UnityEvent), OnExitPhotoMode (UnityEvent)
        //    PhotoZoneEntity : PhotoZone, IMappableObject<PhotoZone> — the placed instance in world
        //    LobbyPostRoutine has PhotoRewardSequence coroutine — fires after photo is scored
        //    InteractionGimmick_PhotoZone fires when player activates a photo zone

        // Fires when a photo zone is successfully completed (reward sequence runs)
        [HarmonyPatch(typeof(LobbyPostRoutine), "PhotoRewardSequence")]
        [HarmonyPostfix]
        public static void OnPhotoCompleted_Postfix(LobbyPostRoutine __instance)
        {
            try
            {
                if (!ArchipelagoClient.IsConnected) return;
                _totalPhotos++;
                // TODO: LocationTracker.OnPhotoTaken not yet implemented
                // LocationTracker.OnPhotoTaken(_totalPhotos, 0);
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[PhotographyPatch] OnPhotoCompleted_Postfix threw: {ex}");
            }
        }

        private static int _totalPhotos = 0;
    }
}
