using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace WikiSearch {
    public class WikiSearchConfig : ModConfig {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("Open results in Steam overlay")]
        public bool UseSteamOverlay;

        public override void OnChanged() {
            WikiSearchSystem.UseSteamOverlay = UseSteamOverlay;
        }
    }
}
