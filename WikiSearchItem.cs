using Terraria;
using Terraria.ModLoader;

namespace WikiSearch {
    class WikiSearchItem : GlobalItem {
        public override void SetDefaults(Item item) {
            base.SetDefaults(item);

            if(item.createTile > -1) {
                if(item.ModItem != null && WikiSearchSystem.RegisteredMods.ContainsKey(item.ModItem.Mod)) {
                    WikiSearchSystem.RegisteredMods[item.ModItem.Mod].TileItems.Add(item);
                }
                else {
                    WikiSearchSystem.DefaultTileItems.Add(item);
                }
            }

            if(item.createWall > 0 && item.ModItem == null) {
                WikiSearchSystem.DefaultWallItems.Add(item);
            }
        }
    }
}
