using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace WikiSearch {
    class WikiSearchItem : GlobalItem {
        public override void SetDefaults(Item item) {
            base.SetDefaults(item);
            if(item.createTile > -1) {
                if(item.ModItem != null) {
                    if(!SearchUtils.ModTileItems.ContainsKey(item.ModItem.Mod.Name)) {
                        SearchUtils.ModTileItems[item.ModItem.Mod.Name] = new HashSet<Item>();
                    }

                    SearchUtils.ModTileItems[item.ModItem.Mod.Name].Add(item);
                }
                else {
                    SearchUtils.DefaultTileItems.Add(item);
                }
            }

            if(item.createWall > 0) {
                if(item.ModItem != null) {
                    if(!SearchUtils.ModWallItems.ContainsKey(item.ModItem.Mod.Name)) {
                        SearchUtils.ModWallItems[item.ModItem.Mod.Name] = new HashSet<Item>();
                    }

                    SearchUtils.ModWallItems[item.ModItem.Mod.Name].Add(item);
                }
                else {
                    SearchUtils.DefaultWallItems.Add(item);
                }
            }
        }
    }
}
