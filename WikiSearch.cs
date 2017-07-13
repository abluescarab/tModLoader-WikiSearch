using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace WikiSearch {
    public class WikiSearch : Mod {
        private const string WIKI_SEARCH_NAME = "Search Wiki";
        private const string WIKI_SEARCH_KEY = "Q";

        private static ModHotKey wikiSearchKey;

        public static Dictionary<Mod, string> registeredMods = new Dictionary<Mod, string>();

        public override void Load() {
            wikiSearchKey = RegisterHotKey(WIKI_SEARCH_NAME, WIKI_SEARCH_KEY);
        }

        public override void PostSetupContent() {
            Mod thorium = ModLoader.GetMod("ThoriumMod");
            Mod calamity = ModLoader.GetMod("CalamityMod");
            Mod spirit = ModLoader.GetMod("SpiritMod");
            Mod fargo = ModLoader.GetMod("Fargowiltas");
            Mod tremor = ModLoader.GetMod("Tremor");

            if(thorium != null)
                RegisterMod(thorium, "http://thoriummod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("ThoriumMod");

            if(calamity != null)
                RegisterMod(calamity, "http://calamitymod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("CalamityMod");

            if(spirit != null)
                RegisterMod(spirit, "http://spiritmod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("SpiritMod");

            if(fargo != null)
                RegisterMod(fargo, "http://fargosmod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("Fargowiltas");

            if(tremor != null)
                RegisterMod(tremor, "http://tremormod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("Tremor");
        }

        public override void PostUpdateInput() {
            try {
                if(wikiSearchKey != null && wikiSearchKey.JustPressed) {
                    SearchUtils.SearchWiki();
                }
            }
            catch(KeyNotFoundException) {
                // ignore
            }
        }

        public override object Call(params object[] args) {
            string call = args[0] as string;
            Mod mod = args[1] as Mod;

            if(call.Equals("RegisterMod") && mod != null) {
                RegisterMod(args[1] as Mod, args[2] as string);
            }

            return null;
        }

        private void RegisterMod(Mod mod, string searchUrl) {
            if(mod != null && !string.IsNullOrWhiteSpace(searchUrl)) {
                registeredMods.Add(mod, searchUrl);
                ErrorLogger.Log("[" + DateTime.Now + "] Successfully registered " + mod.DisplayName + " with WikiSearch.");
            }
        }

        private void UnregisterMod(string modName) {
            if(!string.IsNullOrWhiteSpace(modName)) {
                Mod mod = registeredMods.Keys.FirstOrDefault(m => m.Name.Equals(modName));

                if(mod != null) {
                    registeredMods.Remove(mod);

                    SearchUtils.modTileItems.Remove(mod.Name);
                    SearchUtils.modWallItems.Remove(mod.Name);
                    ErrorLogger.Log("[" + DateTime.Now + "] Successfully unregistered " + mod.DisplayName + " with WikiSearch.");
                }
            }
        }
    }
}
