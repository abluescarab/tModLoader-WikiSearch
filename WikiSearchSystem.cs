using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace WikiSearch {
    internal class WikiSearchSystem : ModSystem {
        private const string WikiSearchName = "Search Wiki";
        private const string WikiSearchKey = "Q";

        private static ModKeybind _wikiSearchKey;

        public static Dictionary<Mod, string> RegisteredMods;
        public static bool UseSteamOverlay = true;

        public override void Load() {
            RegisteredMods = new Dictionary<Mod, string>();
            _wikiSearchKey = KeybindLoader.RegisterKeybind(Mod, WikiSearchName, WikiSearchKey);
        }

        public override void Unload() {
            _wikiSearchKey = null;
            RegisteredMods = null;
        }

        public override void PostUpdateInput() {
            try {
                if(_wikiSearchKey != null && _wikiSearchKey.JustPressed) {
                    SearchUtils.SearchWiki();
                }
            }
            catch(KeyNotFoundException) {
                // ignore
            }
        }

        public override void PostSetupContent() {
            if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                RegisterMod(thorium, "http://thoriummod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("ThoriumMod");

            if(ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                RegisterMod(calamity, "http://calamitymod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("CalamityMod");

            if(ModLoader.TryGetMod("SpiritMod", out Mod spirit))
                RegisterMod(spirit, "http://spiritmod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("SpiritMod");

            if(ModLoader.TryGetMod("Fargowiltas", out Mod fargo))
                RegisterMod(fargo, "http://fargosmod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("Fargowiltas");

            if(ModLoader.TryGetMod("Tremor", out Mod tremor))
                RegisterMod(tremor, "http://tremormod.gamepedia.com/index.php?search=%s");
            else
                UnregisterMod("Tremor");
        }

        public static void RegisterMod(Mod mod, string searchUrl) {
            if(mod != null && !string.IsNullOrWhiteSpace(searchUrl)) {
                RegisteredMods.Add(mod, searchUrl);
                WikiSearch.Log("[{0}] Successfully registered {1} with WikiSearch.", DateTime.Now, mod.DisplayName);
            }
        }

        private void UnregisterMod(string modName) {
            if(!string.IsNullOrWhiteSpace(modName)) {
                Mod mod = RegisteredMods.Keys.FirstOrDefault(m => m.Name.Equals(modName));

                if(mod != null) {
                    RegisteredMods.Remove(mod);

                    SearchUtils.ModTileItems.Remove(mod.Name);
                    SearchUtils.ModWallItems.Remove(mod.Name);

                    WikiSearch.Log("[{0}] Successfully unregistered {1} with WikiSearch.", DateTime.Now, mod.DisplayName);
                }
            }
        }
    }
}
