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

        // ⚠️ COROUTINE HOOK DISABLED:
        // PhotoRewardSequence is an IEnumerator coroutine — Harmony cannot directly patch coroutines
        // in IL2CPP because they compile into state machine classes. Patching them causes a startup crash.
        //
        // TODO: Find a non-coroutine method that fires after a photo is scored. Candidates:
        //   - LobbyPostRoutine: look for a non-coroutine method called by PhotoRewardSequence's MoveNext()
        //   - PhotoZone: look for an OnPhotoSuccess() or similar callback
        //   - InteractionGimmick_PhotoZone.SuccessInteraction() — fires when photo zone is activated
        //     (may fire before scoring, but guaranteed non-coroutine)
        //
        // [HarmonyPatch(typeof(LobbyPostRoutine), "PhotoRewardSequence")]
        // [HarmonyPostfix]
        // public static void OnPhotoCompleted_Postfix(LobbyPostRoutine __instance) { ... }

        private static int _totalPhotos = 0;
    }
}
