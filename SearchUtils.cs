using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace WikiSearch {
    public static class SearchUtils {
        public static HashSet<Item> createTileItems = new HashSet<Item>();
        public static HashSet<Item> createWallItems = new HashSet<Item>();

        public const string TERRARIA_WIKI = "http://terraria.gamepedia.com/index.php?search=%s";

        /// <summary>
        /// Begin searching for the item under the mouse cursor.
        /// </summary>
        public static void SearchWiki() {
            // Check items first, then NPCs, then tiles
            if(ItemHover()) return;
            if(NPCHover()) return;
            if(TileHover()) return;
        }

        /// <summary>
        /// Check for an item under the mouse cursor.
        /// </summary>
        /// <returns>whether an item is under the mouse cursor</returns>
        private static bool ItemHover() {
            // only run if the inventory is open
            if(Main.playerInventory) {
                // check if the mouse is over an item
                if(!string.IsNullOrWhiteSpace(Main.HoverItem.Name)) {
                    string name = Main.HoverItem.Name;

                    // check if the hovered item is part of a mod
                    if(Main.HoverItem.modItem != null) {
                        Mod mod = Main.HoverItem.modItem.mod;

                        // check if the mod is registered
                        if(WikiSearch.registeredMods.ContainsKey(mod)) {
                            DoSearch(WikiSearch.registeredMods[mod], name);
                        }
                        else {
                            ShowModMessage("item", name, Main.HoverItem.modItem.mod.DisplayName);
                        }
                    }
                    else {
                        DoSearch(TERRARIA_WIKI, name);
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check for an NPC under the mouse cursor.
        /// </summary>
        /// <returns>whether an NPC is under the mouse cursor</returns>
        private static bool NPCHover() {
            // loop through the NPCs and check if the mouse is over any of them
            NPC npc = GetHoveringNPC();

            if(npc != null) {
                // check if the npc is part of a mod
                if(npc.modNPC != null) {
                    Mod mod = npc.modNPC.mod;

                    if(WikiSearch.registeredMods.ContainsKey(mod)) {
                        DoSearch(WikiSearch.registeredMods[mod], npc.TypeName);
                    }
                    else {
                        ShowModMessage("NPC", npc.TypeName, npc.modNPC.mod.DisplayName);
                    }
                }
                else {
                    DoSearch(TERRARIA_WIKI, npc.TypeName);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Check for a tile under the mouse cursor.
        /// </summary>
        /// <returns>whether a tile is under the mouse cursor</returns>
        private static bool TileHover() {
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
                        Mod mod = item.modItem.mod;

                        if(WikiSearch.registeredMods.ContainsKey(mod)) {
                            DoSearch(WikiSearch.registeredMods[mod], item.Name);
                        }
                        else {
                            ShowModMessage("item", item.Name, item.modItem.mod.DisplayName);
                        }
                    }
                    else {
                        DoSearch(TERRARIA_WIKI, item.Name);
                    }
                }
                else if(active || (!active && tile.wall > 0)) {
                    Main.NewText("Cannot search for this tile. ID: " + (active ? tile.type : tile.wall) + ".");
                }

                return true;
            }

            return false;
        }

        private static void ShowModMessage(string type, string name, string mod) {
            Main.NewText("Cannot search. \"" + name + "\" is a modded " + type + " from " + mod + ".");
        }

        private static void DoSearch(string url, string term) {
            Process.Start(url.Replace("%s", term));
        }

        // Thanks to DrEinsteinium for the base method
        private static NPC GetHoveringNPC() {
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
