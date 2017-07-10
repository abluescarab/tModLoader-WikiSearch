using System.Collections.Generic;
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
                if(item.modItem != null) {
                    if(!SearchUtils.modTileItems.ContainsKey(item.modItem.mod.Name)) {
                        SearchUtils.modTileItems[item.modItem.mod.Name] = new HashSet<Item>();
                    }

                    SearchUtils.modTileItems[item.modItem.mod.Name].Add(item);
                }
                else {
                    SearchUtils.defaultTileItems.Add(item);
                }
            }

            if(item.createWall > 0) {
                if(item.modItem != null) {
                    if(!SearchUtils.modWallItems.ContainsKey(item.modItem.mod.Name)) {
                        SearchUtils.modWallItems[item.modItem.mod.Name] = new HashSet<Item>();
                    }

                    SearchUtils.modWallItems[item.modItem.mod.Name].Add(item);
                }
                else {
                    SearchUtils.defaultWallItems.Add(item);
                }
            }
        }
    }
}
