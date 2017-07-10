using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace WikiSearch {
    public class WikiSearch : Mod {
        private HotKey wikiSearchKey = new HotKey("Search Wiki", Keys.Q);

        public static Dictionary<Mod, string> registeredMods = new Dictionary<Mod, string>();

        public override void Load() {
            RegisterHotKey(wikiSearchKey.Name, wikiSearchKey.DefaultKey.ToString());
        }

        public override void HotKeyPressed(string name) {
            if(HotKey.JustPressed(this, name)) {
                if(name.Equals(wikiSearchKey.Name)) {
                    SearchUtils.SearchWiki();
                }
            }
        }

        public override void PostSetupContent() {
            RegisterMod(ModLoader.GetMod("ThoriumMod"), "http://thoriummod.gamepedia.com/index.php?search=%s");
            RegisterMod(ModLoader.GetMod("CalamityMod"), "http://calamitymod.gamepedia.com/index.php?search=%s");
            RegisterMod(ModLoader.GetMod("SpiritMod"), "http://spiritmod.gamepedia.com/index.php?search=%s");
            RegisterMod(ModLoader.GetMod("Fargowiltas"), "http://fargosmod.gamepedia.com/index.php?search=%s");
            RegisterMod(ModLoader.GetMod("Tremor"), "http://tremormod.gamepedia.com/index.php?search=%s");
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
    }
}
