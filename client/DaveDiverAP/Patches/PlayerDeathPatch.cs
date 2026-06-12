using HarmonyLib;

namespace DaveDiverAP.Patches
{
    /// <summary>
    /// Patches the player death/oxygen depletion system to detect when Dave dies.
    /// Used for Death Link — sends a death event to all other Death Link players.
    ///
    /// IMPORTANT: Class/method names are PLACEHOLDERS.
    /// Decompile GameAssembly.dll with Il2CppDumper to find real names.
    ///
    /// ## What to search for in Il2CppDumper output:
    /// - "PlayerCharacter" class — look for Die(), Death(), OnDeath(), Kill()
    /// - "OxygenSystem" or "AirSystem" — look for OnOxygenDepleted(), RunOutOfAir()
    /// - Methods that trigger the death animation/respawn sequence
    /// - Methods referenced in the SuperDave mod (infinite air feature)
    ///
    /// ## From existing mod research:
    /// - PlayerCharacter is the main player class (confirmed by SuperDave mod)
    /// - OxygenSystem is likely separate (SuperDave patches it for infinite air)
    ///
    /// Dave can die from:
    /// 1. Running out of oxygen (most common)
    /// 2. Taking too much damage from enemies
    /// </summary>
    [HarmonyPatch]
    public static class PlayerDeathPatch
    {
        // ── Oxygen depletion death ────────────────────────────────────────────
        // PLACEHOLDER: Replace OxygenSystem with real class name
        // Look for a method called when oxygen hits 0 and triggers death sequence
        [HarmonyPatch(typeof(OxygenSystem), "OnOxygenDepleted")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void OnOxygenDepleted_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (!ArchipelagoClient.SlotData?.DeathLink ?? true) return;

            DeathLinkHandler.OnPlayerDied();
        }

        // ── Damage-based death ────────────────────────────────────────────────
        // PLACEHOLDER: Replace PlayerCharacter.Die with real class/method name
        // This fires when HP reaches 0 from enemy damage
        [HarmonyPatch(typeof(PlayerCharacter), "Die")]  // PLACEHOLDER
        [HarmonyPostfix]
        public static void PlayerDie_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (!ArchipelagoClient.SlotData?.DeathLink ?? true) return;

            DeathLinkHandler.OnPlayerDied();
        }
    }
}
