using System;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;

namespace DaveDiverAP
{
    /// <summary>
    /// Applies received Archipelago items to the game state.
    /// Each item name maps to a specific game effect.
    /// </summary>
    public static class ItemHandler
    {
        private static ManualLogSource Log => Plugin.Log;

        /// <summary>
        /// Route a received item to the appropriate handler based on its name.
        /// </summary>
        public static void ApplyItem(NetworkItem item)
        {
            var name = item.ItemName;
            Log.LogInfo($"Applying item: {name}");

            // Save item index so we don't replay it next session
            SaveData.SetLastItemIndex(item.ItemIndex);

            // ── Progressive equipment ────────────────────────────────────────
            if (name == "Progressive Oxygen Tank")    { UpgradeOxygenTank();    return; }
            if (name == "Progressive Harpoon")        { UpgradeHarpoon();       return; }
            if (name == "Progressive Diving Suit")    { UpgradeDivingSuit();    return; }

            // ── Area unlock items ────────────────────────────────────────────
            if (name == "Sea People Gloves")          { UnlockSeaPeopleGloves();     return; }
            if (name == "Sea People Translator")      { UnlockTranslator();          return; }
            if (name == "Key to Tenzhin")             { UnlockKeyToTenzhin();        return; }
            if (name == "Tech Suit Parts")            { AddTechSuitPart();           return; }
            if (name == "Laser Device")               { UnlockLaserDevice();         return; }
            if (name == "Control Room Button")        { AddControlRoomButton();      return; }
            if (name == "Sea People's Trust")         { UnlockSeaPeopleTrust();      return; }
            if (name == "Vortex Entry")               { AddVortexEntry();            return; }
            if (name == "Teleport Mirror")            { UnlockTeleportMirror();      return; }
            if (name == "Teleport to Sea People Village") { UnlockTeleportSPV();     return; }
            if (name == "Teleport to Glacier")        { UnlockTeleportGlacier();     return; }
            if (name == "Teleport to Deep Blue Hole") { UnlockTeleportDeep();        return; }

            // ── Farm unlocks ─────────────────────────────────────────────────
            if (name == "Unlock Fish Farm")           { UnlockFarm("fish");     return; }
            if (name == "Unlock Vegetable Farm")      { UnlockFarm("vegetable"); return; }
            if (name == "Unlock Chicken Farm")        { UnlockFarm("chicken");  return; }

            // ── Cooksta rank ──────────────────────────────────────────────────────
            if (name == "Progressive Cooksta Rank") { UpgradeCookstaRank(); return; }

            // ── Chapter completion ───────────────────────────────────────────
            if (name.StartsWith("Chapter ") && name.EndsWith(" Complete"))
            {
                var chapterNum = int.Parse(name.Split(' ')[1]);
                CompleteChapter(chapterNum);
                return;
            }

            // ── Charms ───────────────────────────────────────────────────────
            if (IsCharm(name)) { UnlockCharm(name); return; }

            // ── Weapons ──────────────────────────────────────────────────────
            if (IsWeapon(name)) { UnlockWeapon(name); return; }

            // ── Progressive dish upgrades ─────────────────────────────────────
            if (name.StartsWith("Progressive ")) { UpgradeDish(name[12..]); return; }

            // ── Recipes ──────────────────────────────────────────────────────
            if (name.StartsWith("Recipe: ")) { UnlockRecipe(name[8..]); return; }

            // ── Ingredients (filler) ─────────────────────────────────────────
            if (IsIngredient(name)) { GiveIngredient(name); return; }

            // ── Story key items ───────────────────────────────────────────────
            if (name == "Sea People Bracelet")  { UnlockAbility("oxygen_grace"); return; }
            if (name == "Bug Net")              { UnlockAbility("bug_net");      return; }
            if (name == "Night Dive Unlock")    { UnlockAbility("night_dive");   return; }
            if (name == "iDiver App")           { UnlockAbility("idiver_app");   return; }
            if (name == "Cargo Box Upgrade")    { UpgradeCargoBox();             return; }

            // ── Currency (filler) ─────────────────────────────────────────────
            if (name.StartsWith("Gold "))  { GiveCurrency("gold",  name); return; }
            if (name.StartsWith("Bei "))   { GiveCurrency("bei",   name); return; }

            Log.LogWarning($"Unhandled item: {name}");
        }

        // ── Implementation stubs ─────────────────────────────────────────────
        // Each method below calls into the game's internal APIs via the
        // interop assemblies. The exact method names will be confirmed
        // by decompiling Assembly-CSharp.dll with dnSpy or ILSpy.

        private static void UpgradeOxygenTank()
        {
            // TODO: Call game API to increment oxygen tank upgrade level
            // Example: PlayerManager.Instance.UpgradeOxygenTank();
            Log.LogInfo("Upgrading oxygen tank");
        }

        private static void UpgradeHarpoon()
        {
            Log.LogInfo("Upgrading harpoon");
            // TODO: Call game API
        }

        private static void UpgradeDivingSuit()
        {
            Log.LogInfo("Upgrading diving suit level");
            // TODO: Call game API
        }

        private static void UnlockSeaPeopleGloves()
        {
            Log.LogInfo("Unlocking Sea People Gloves");
            // TODO: Set flag in game save data
        }

        private static void UnlockTranslator()
        {
            Log.LogInfo("Unlocking Sea People Translator");
        }

        private static void UnlockKeyToTenzhin()
        {
            Log.LogInfo("Unlocking Key to Tenzhin");
        }

        private static void AddTechSuitPart()
        {
            Log.LogInfo("Adding Tech Suit Part");
        }

        private static void UnlockLaserDevice()
        {
            Log.LogInfo("Unlocking Laser Device");
        }

        private static void AddControlRoomButton()
        {
            Log.LogInfo("Adding Control Room Button");
        }

        private static void UnlockSeaPeopleTrust()
        {
            Log.LogInfo("Unlocking Sea People's Trust");
        }

        private static void AddVortexEntry()
        {
            Log.LogInfo("Adding Vortex Entry");
        }

        private static void UnlockTeleportMirror()
        {
            Log.LogInfo("Unlocking Teleport Mirror");
        }

        private static void UnlockTeleportSPV()
        {
            Log.LogInfo("Unlocking teleport: Sea People Village");
        }

        private static void UnlockTeleportGlacier()
        {
            Log.LogInfo("Unlocking teleport: Glacier");
        }

        private static void UnlockTeleportDeep()
        {
            Log.LogInfo("Unlocking teleport: Deep Blue Hole");
        }

        private static void UnlockFarm(string farmType)
        {
            Log.LogInfo($"Unlocking {farmType} farm");
        }

        private static void CompleteChapter(int chapter)
        {
            Log.LogInfo($"Setting chapter {chapter} complete flag");
        }

        private static void UpgradeCookstaRank()
        {
            // TODO: Call game API to increment Cooksta rank
            // Coal(0) -> Bronze(1) -> Silver(2) -> Gold(3) -> Platinum(4) -> Diamond(5)
            // Find via Il2CppDumper: CookstaManager.set_Rank() or similar
            Log.LogInfo("Upgrading Cooksta rank");
        }

        private static void UnlockCharm(string charmName)
        {
            Log.LogInfo($"Unlocking charm: {charmName}");
        }

        private static void UnlockWeapon(string weaponName)
        {
            Log.LogInfo($"Unlocking weapon: {weaponName}");
        }

        private static void UpgradeDish(string dishName)
        {
            Log.LogInfo($"Upgrading dish: {dishName}");
        }

        private static void UnlockRecipe(string recipeName)
        {
            Log.LogInfo($"Unlocking recipe: {recipeName}");
        }

        private static void GiveIngredient(string ingredientName)
        {
            Log.LogInfo($"Giving ingredient: {ingredientName}");
        }

        private static void UnlockAbility(string abilityKey)
        {
            Log.LogInfo($"Unlocking ability: {abilityKey}");
        }

        private static void UpgradeCargoBox()
        {
            Log.LogInfo("Upgrading cargo box");
        }

        private static void GiveCurrency(string type, string itemName)
        {
            // Parse amount from item name e.g. "Gold (Small)" -> 500g
            int amount = itemName.Contains("Large") ? 5000
                       : itemName.Contains("Medium") ? 2000
                       : 500;
            Log.LogInfo($"Giving {amount} {type}");
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static readonly string[] _charms = {
            "Dolphin Necklace", "Octopus Bracelet", "Sea People Bracelet",
            "Octopus Weapon Charm", "Sea People Necklace", "Shark Teeth Necklace",
            "Eco Poison Resist Bracelet", "Eco Health Bracelet", "Eco Gemstone Bracelet",
            "Eco Waterproof Bag", "Leo Keychain", "Jimbo Coin"
        };

        private static bool IsCharm(string name) =>
            Array.IndexOf(_charms, name) >= 0;

        private static bool IsWeapon(string name) =>
            name.Contains("Rifle") || name.Contains("Gun") || name.Contains("Launcher") ||
            name.Contains("Dart") || name.Contains("Axel") || name.Contains("Knife") ||
            name.Contains("Bomb");

        private static bool IsIngredient(string name) =>
            name.EndsWith(" x1") || name.EndsWith(" x2") || name.EndsWith(" x5") || name.EndsWith(" x10");
    }
}
