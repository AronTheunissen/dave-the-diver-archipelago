using System;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Handles the Archipelago Death Link feature.
    ///
    /// When Death Link is enabled:
    /// - If Dave dies, all other Death Link players die too
    /// - If any other Death Link player dies, Dave dies too
    ///
    /// Detecting player death:
    /// - Hook the player death/respawn method via Harmony (see Patches/PlayerDeathPatch.cs)
    /// - Call DeathLinkHandler.OnPlayerDied() when Dave runs out of HP or oxygen
    ///
    /// Applying death to Dave:
    /// - Call the game's kill/respawn method via ItemHandler
    /// - Show a notification explaining who killed Dave
    /// </summary>
    public static class DeathLinkHandler
    {
        private static DeathLinkService? _service;
        private static ManualLogSource Log => Plugin.Log;

        // Prevent death loops: ignore incoming death while we're processing one
        private static bool _receivingDeath = false;

        // Cooldown to prevent sending multiple deaths from one event
        private static DateTime _lastDeathSent = DateTime.MinValue;
        private static readonly TimeSpan DeathCooldown = TimeSpan.FromSeconds(3);

        public static void Initialize(ArchipelagoSession session)
        {
            if (!ArchipelagoClient.SlotData?.DeathLink ?? true)
            {
                Log.LogInfo("Death Link is disabled for this slot.");
                return;
            }

            _service = session.CreateDeathLinkService();
            _service.OnDeathLinkReceived += OnDeathLinkReceived;
            _service.EnableDeathLink();

            Log.LogInfo("Death Link enabled!");
        }

        public static void Dispose()
        {
            if (_service != null)
            {
                _service.DisableDeathLink();
                _service = null;
            }
        }

        /// <summary>
        /// Call this when Dave dies in-game (from Harmony patch).
        /// Sends a death to all other Death Link players.
        /// </summary>
        public static void OnPlayerDied()
        {
            if (_service == null) return;
            if (_receivingDeath) return; // Don't echo back a received death

            // Cooldown to prevent multiple rapid deaths
            if (DateTime.Now - _lastDeathSent < DeathCooldown) return;
            _lastDeathSent = DateTime.Now;

            var cause = $"{ArchipelagoClient.SlotName} ran out of oxygen in the Blue Hole";
            Log.LogInfo($"Sending death link: {cause}");

            _service.SendDeathLink(new DeathLink(ArchipelagoClient.SlotName, cause));
        }

        /// <summary>
        /// Called when another player's death arrives.
        /// Kills Dave in-game.
        /// </summary>
        private static void OnDeathLinkReceived(DeathLink deathLink)
        {
            if (_receivingDeath) return;

            var source = deathLink.Source ?? "Someone";
            var cause  = deathLink.Cause ?? $"{source} died";

            Log.LogInfo($"Death Link received from {source}: {cause}");

            _receivingDeath = true;
            try
            {
                // Show notification
                NotificationManager.ShowNotification(
                    $"💀 Death Link!",
                    $"{source} died: {cause}",
                    NotificationManager.NotificationType.DeathLink
                );

                // Kill Dave
                KillPlayer();
            }
            finally
            {
                _receivingDeath = false;
            }
        }

        private static void KillPlayer()
        {
            // TODO: Call the game's kill/instant death method
            // Find via Il2CppDumper: look for PlayerCharacter.Die() or similar
            // Example: PlayerCharacter.Instance.TakeDamage(999999);
            //      or: PlayerCharacter.Instance.Die();
            Log.LogInfo("Death Link: Killing player (TODO: implement game API call)");
        }
    }
}
