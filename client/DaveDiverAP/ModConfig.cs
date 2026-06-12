using BepInEx.Configuration;

namespace DaveDiverAP
{
    /// <summary>
    /// BepInEx configuration file support.
    /// Settings are stored in BepInEx/config/DaveDiverAP.cfg
    /// and can be edited manually or via a config manager mod.
    ///
    /// These settings are for mod behaviour that isn't controlled by
    /// the Archipelago YAML (which comes from the server as SlotData).
    /// </summary>
    public static class ModConfig
    {
        // ── Connection (pre-filled from last session or config file) ──────────
        public static ConfigEntry<string> DefaultServer   { get; private set; } = null!;
        public static ConfigEntry<int>    DefaultPort     { get; private set; } = null!;
        public static ConfigEntry<string> DefaultSlotName { get; private set; } = null!;

        // ── UI Settings ───────────────────────────────────────────────────────
        public static ConfigEntry<string> ToggleUIKey         { get; private set; } = null!;
        public static ConfigEntry<float>  NotificationDuration { get; private set; } = null!;
        public static ConfigEntry<bool>   ShowItemNotifications { get; private set; } = null!;

        // ── Gameplay ──────────────────────────────────────────────────────────
        public static ConfigEntry<bool> AutoConnectOnLaunch { get; private set; } = null!;

        public static void Initialize(ConfigFile config)
        {
            // ── Connection section ────────────────────────────────────────────
            DefaultServer = config.Bind(
                "Connection",
                "DefaultServer",
                "localhost",
                "Default Archipelago server address.");

            DefaultPort = config.Bind(
                "Connection",
                "DefaultPort",
                38281,
                "Default Archipelago server port.");

            DefaultSlotName = config.Bind(
                "Connection",
                "DefaultSlotName",
                "Player",
                "Default slot/player name for Archipelago.");

            AutoConnectOnLaunch = config.Bind(
                "Connection",
                "AutoConnectOnLaunch",
                false,
                "If true, automatically connect using saved connection info when the game starts.");

            // ── UI section ────────────────────────────────────────────────────
            ToggleUIKey = config.Bind(
                "UI",
                "ToggleKey",
                "F9",
                "Key to toggle the Archipelago connection window.");

            NotificationDuration = config.Bind(
                "UI",
                "NotificationDuration",
                5f,
                new ConfigDescription(
                    "How long (in seconds) item notifications stay on screen.",
                    new AcceptableValueRange<float>(1f, 30f)));

            ShowItemNotifications = config.Bind(
                "UI",
                "ShowItemNotifications",
                true,
                "Show a notification popup when an item is received from Archipelago.");

            Plugin.Log.LogInfo("ModConfig initialized. Config file: BepInEx/config/DaveDiverAP.cfg");
        }
    }
}
