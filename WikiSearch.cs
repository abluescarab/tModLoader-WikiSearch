using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace WikiSearch {
    public class WikiSearch : Mod {
        private const string WikiSearchName = "Search Wiki";
        private const string WikiSearchKey = "Q";

        private static ModHotKey _wikiSearchKey;

        public static Dictionary<Mod, string> RegisteredMods;
        public static bool UseSteamOverlay = true;

        public override void Load() {
            Properties = new ModProperties {
                Autoload = true,
                AutoloadBackgrounds = true,
                AutoloadSounds = true
            };

            RegisteredMods = new Dictionary<Mod, string>();
            _wikiSearchKey = RegisterHotKey(WikiSearchName, WikiSearchKey);
        }

        public override void Unload() {
            _wikiSearchKey = null;
            RegisteredMods = null;
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
                if(_wikiSearchKey != null && _wikiSearchKey.JustPressed) {
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
                RegisteredMods.Add(mod, searchUrl);
                Logger.InfoFormat("[{0}] Successfully registered {1} with WikiSearch.", DateTime.Now, mod.DisplayName);
            }
        }

        private void UnregisterMod(string modName) {
            if(!string.IsNullOrWhiteSpace(modName)) {
                Mod mod = RegisteredMods.Keys.FirstOrDefault(m => m.Name.Equals(modName));

                if(mod != null) {
                    RegisteredMods.Remove(mod);

                    SearchUtils.ModTileItems.Remove(mod.Name);
                    SearchUtils.ModWallItems.Remove(mod.Name);

                    Logger.InfoFormat("[{0}] Successfully unregistered {1} with WikiSearch.", DateTime.Now, mod.DisplayName);
                }
            }
        }
    }
}
