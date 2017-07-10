using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                WikiSearch.createTileItems.Add(item);
            }

            if(item.createWall > 0) {
                WikiSearch.createWallItems.Add(item);
            }
        }
    }
}
