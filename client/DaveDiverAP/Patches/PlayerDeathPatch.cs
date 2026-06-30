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
        // ✅ CONFIRMED via dump.cs: PlayerBreathHandler is the class (field: m_HP at 0x9C)
        // PlayerBreathHandler implements IHasHP — when HP hits 0 oxygen is gone.
        // The player dies via PlayerCharacter.OnDie() which PlayerBreathHandler calls.
        // We hook PlayerCharacter.OnDie() for both death causes below.

        // ── Damage-based AND oxygen death ─────────────────────────────────────
        // ✅ CONFIRMED via dump.cs: PlayerCharacter has public void OnDie() 
        // and public void OnDie(PlayerCharacter.DieAnimType dieType = 0)
        // We patch the no-arg overload which is the public entry point.
        [HarmonyPatch(typeof(PlayerCharacter), "OnDie", new System.Type[0])]
        [HarmonyPostfix]
        public static void PlayerDie_Postfix()
        {
            try
            {
                if (!ArchipelagoClient.IsConnected) return;
                if (!ArchipelagoClient.SlotData?.DeathLink ?? true) return;

                DeathLinkHandler.OnPlayerDied();
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogError($"[PlayerDeathPatch] PlayerDie_Postfix threw: {ex}");
            }
        }
    }
}
