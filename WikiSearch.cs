using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;

namespace WikiSearch {
    public class WikiSearch : Mod {
        public const string SEARCH_URL = "http://terraria.gamepedia.com/index.php?search=";

        public static HashSet<Item> createTileItems = new HashSet<Item>();
        public static HashSet<Item> createWallItems = new HashSet<Item>();

        private HotKey wikiSearchKey = new HotKey("Search Wiki", Keys.Q);

        public override void Load() {
            RegisterHotKey(wikiSearchKey.Name, wikiSearchKey.DefaultKey.ToString());
        }

        public override void HotKeyPressed(string name) {
            if(HotKey.JustPressed(this, name)) {
                if(name.Equals(wikiSearchKey.Name)) {
                    SearchWiki();
                }
            }
        }

        private void SearchWiki() {
            // Check items first, then NPCs, then tiles
            if(ItemHover()) return;
            if(NPCHover()) return;
            if(TileHover()) return;
        }

        private bool ItemHover() {
            // only run if the inventory is open
            if(Main.playerInventory) {
                // check if the mouse is over an item
                if(!string.IsNullOrWhiteSpace(Main.HoverItem.Name)) {
                    string name = Main.HoverItem.Name;

                    // check if the hovered item is part of a mod
                    if(Main.HoverItem.modItem != null) {
                        ShowModMessage("item", name, Main.HoverItem.modItem.mod.DisplayName);
                    }
                    else {
                        DoSearch(name);
                    }

                    return true;
                }
            }

            return false;
        }

        private bool NPCHover() {
            // loop through the NPCs and check if the mouse is over any of them
            NPC npc = GetHoveringNPC();
            
            if(npc != null) {
                // check if the npc is part of a mod
                if(npc.modNPC != null) {
                    ShowModMessage("NPC", npc.TypeName, npc.modNPC.mod.DisplayName);
                }
                else {
                    DoSearch(npc.TypeName);
                }

                return true;
            }

            return false;
        }

        private bool TileHover() {
            // get the tile under the cursor
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
            Item item = null;
            bool active = false;
            string term = string.Empty;

            if(tile != null) {
                active = tile.active();
                
                // if the tile is inactive, it's either air or a wall
                if(active) {
                    item = createTileItems.FirstOrDefault(i => i.createTile == tile.type);
                }
                else if(tile.wall > 0) {
                    item = createWallItems.FirstOrDefault(i => i.createWall == tile.wall);
                }

                if(item != null) {
                    if(item.modItem != null) {
                        ShowModMessage("item", item.Name, item.modItem.mod.DisplayName);
                    }
                    else {
                        DoSearch(item.Name);
                    }
                }
                else if(active || (!active && tile.wall > 0)) {
                    Main.NewText("Cannot search for this tile. ID: " + (active ? tile.type : tile.wall) + ".");
                }

                return true;
            }

            return false;
        }

        private void ShowModMessage(string type, string name, string mod) {
            Main.NewText("Cannot search. \"" + name + "\" is a modded " + type + " from " + mod + ".");
        }

        private void DoSearch(string term) {
            Process.Start(SEARCH_URL + term.Replace(" ", "_"));
        }

        // Thanks to DrEinsteinium for the base method
        private NPC GetHoveringNPC() {
            // loop through the NPCs
            for(int i = 0; i < Main.npc.Length; i++) {
                // check if the NPC hitbox contains the mouse cursor
                if(Main.npc[i].Hitbox.Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y)) {
                    return Main.npc[i];
                }
            }

            return null;
        }
    }
}
