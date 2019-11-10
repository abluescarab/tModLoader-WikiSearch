using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace WikiSearch {
    class WikiSearchItem : GlobalItem {
        public override void SetDefaults(Item item) {
            base.SetDefaults(item);
            if(item.createTile > -1) {
                if(item.modItem != null) {
                    if(!SearchUtils.ModTileItems.ContainsKey(item.modItem.mod.Name)) {
                        SearchUtils.ModTileItems[item.modItem.mod.Name] = new HashSet<Item>();
                    }

                    SearchUtils.ModTileItems[item.modItem.mod.Name].Add(item);
                }
                else {
                    SearchUtils.DefaultTileItems.Add(item);
                }
            }

            if(item.createWall > 0) {
                if(item.modItem != null) {
                    if(!SearchUtils.ModWallItems.ContainsKey(item.modItem.mod.Name)) {
                        SearchUtils.ModWallItems[item.modItem.mod.Name] = new HashSet<Item>();
                    }

                    SearchUtils.ModWallItems[item.modItem.mod.Name].Add(item);
                }
                else {
                    SearchUtils.DefaultWallItems.Add(item);
                }
            }
        }
    }
}
