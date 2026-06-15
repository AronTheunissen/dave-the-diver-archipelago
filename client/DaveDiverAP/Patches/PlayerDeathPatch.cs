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
        // ✅ CONFIRMED: PlayerBreathHandler is the real oxygen system class (WhiteMinds mod)
        // Still needed via Il2CppDumper: exact method name that fires on depletion
        // Search for: "OnBreathDepleted", "OnOxygenEmpty", "OnSuffocate", "Die" in PlayerBreathHandler
        [HarmonyPatch(typeof(PlayerBreathHandler), "OnOxygenDepleted")]  // class confirmed, method name still PLACEHOLDER
        [HarmonyPostfix]
        public static void OnOxygenDepleted_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (!ArchipelagoClient.SlotData?.DeathLink ?? true) return;

            DeathLinkHandler.OnPlayerDied();
        }

        // ── Damage-based death ────────────────────────────────────────────────
        // ✅ CONFIRMED: PlayerCharacter is the real class name (confirmed by multiple mods)
        // Still needed via Il2CppDumper: exact death method name in PlayerCharacter
        // Search for: "Die", "Death", "OnDeath", "Kill", "OnDamageKill" in PlayerCharacter
        [HarmonyPatch(typeof(PlayerCharacter), "Die")]  // class confirmed, method name still PLACEHOLDER
        [HarmonyPostfix]
        public static void PlayerDie_Postfix()
        {
            if (!ArchipelagoClient.IsConnected) return;
            if (!ArchipelagoClient.SlotData?.DeathLink ?? true) return;

            DeathLinkHandler.OnPlayerDied();
        }
    }
}
