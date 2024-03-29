﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace WikiSearch {
    internal class WikiSearchSystem : ModSystem {
        internal class ModInfo {
            public string Link;
            public HashSet<Item> TileItems;

            public ModInfo(string link) {
                Link = link;
                TileItems = new HashSet<Item>();
            }
        }

        private const string WikiSearchName = "Search Wiki";

        public static ModKeybind WikiSearchKey;
        public static HashSet<Item> DefaultTileItems = new HashSet<Item>();
        public static HashSet<Item> DefaultWallItems = new HashSet<Item>();
        public static Dictionary<Mod, ModInfo> RegisteredMods;

        public static bool UseSteamOverlay = true;

        public override void Load() {
            RegisteredMods = new Dictionary<Mod, ModInfo>();
            WikiSearchKey = KeybindLoader.RegisterKeybind(Mod, WikiSearchName, Keys.Q);
        }

        public override void Unload() {
            WikiSearchKey = null;
            RegisteredMods = null;
        }

        public override void PostSetupContent() {
            if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
                RegisterMod(thorium, "http://thoriummod.fandom.com/index.php?search=%s");
            else
                UnregisterMod("ThoriumMod");

            if(ModLoader.TryGetMod("CalamityMod", out Mod calamity))
                RegisterMod(calamity, "http://calamitymod.fandom.com/index.php?search=%s");
            else
                UnregisterMod("CalamityMod");

            if(ModLoader.TryGetMod("SpiritMod", out Mod spirit))
                RegisterMod(spirit, "http://spiritmod.fandom.com/index.php?search=%s");
            else
                UnregisterMod("SpiritMod");

            if(ModLoader.TryGetMod("Fargowiltas", out Mod fargo))
                RegisterMod(fargo, "https://terrariamods.wiki.gg/index.php?search=Fargo%27s+Mod/%s");
            else
                UnregisterMod("Fargowiltas");

            if(ModLoader.TryGetMod("Tremor", out Mod tremor))
                RegisterMod(tremor, "http://tremormod.fandom.com/index.php?search=%s");
            else
                UnregisterMod("Tremor");

            if(ModLoader.TryGetMod("MagicStorage", out Mod magicStorage))
                RegisterMod(magicStorage, "https://terrariamods.fandom.com/index.php?search=Magic+Storage/%s");
            else
                UnregisterMod("MagicStorage");

            if(ModLoader.TryGetMod("StarsAbove", out Mod starsAbove))
                RegisterMod(starsAbove, "https://terrariamods.wiki.gg/index.php?search=The+Stars+Above/%s");
            else
                UnregisterMod("StarsAbove");
        }

        public static void RegisterMod(Mod mod, string searchUrl) {
            if(mod != null && !string.IsNullOrWhiteSpace(searchUrl)) {
                RegisteredMods.Add(mod, new ModInfo(searchUrl));
                WikiSearch.Log("[{0}] Successfully registered {1} with WikiSearch.", DateTime.Now, mod.DisplayName);
            }
        }

        private void UnregisterMod(string modName) {
            if(!string.IsNullOrWhiteSpace(modName)) {
                Mod mod = RegisteredMods.Keys.FirstOrDefault(m => m.Name.Equals(modName));

                if(mod != null) {
                    RegisteredMods.Remove(mod);
                    WikiSearch.Log("[{0}] Successfully unregistered {1} with WikiSearch.", DateTime.Now, mod.DisplayName);
                }
            }
        }
    }
}
