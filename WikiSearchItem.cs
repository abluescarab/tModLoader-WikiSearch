using Terraria;
using Terraria.ModLoader;

namespace WikiSearch {
    class WikiSearchItem : GlobalItem {
        public override bool Autoload(ref string name) {
            return true;
        }

        public override void SetDefaults(Item item) {
            base.SetDefaults(item);
            if(item.createTile > -1) {
                SearchUtils.createTileItems.Add(item);
            }

            if(item.createWall > 0) {
                SearchUtils.createWallItems.Add(item);
            }
        }
    }
}
