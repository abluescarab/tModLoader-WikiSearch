using Microsoft.Xna.Framework;
using Steamworks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WikiSearch {
    public static class SearchUtils {
        public static HashSet<Item> DefaultTileItems = new HashSet<Item>();
        public static HashSet<Item> DefaultWallItems = new HashSet<Item>();

        public static Dictionary<string, HashSet<Item>> ModTileItems = new Dictionary<string, HashSet<Item>>();
        public static Dictionary<string, HashSet<Item>> ModWallItems = new Dictionary<string, HashSet<Item>>();

        public const string TerrariaWiki = "http://terraria.gamepedia.com/index.php?search=%s";

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
                    if(Main.HoverItem.ModItem != null) {
                        Mod mod = Main.HoverItem.ModItem.Mod;
                        name = Regex.Replace(name, @"\[.+\]", "").Trim();

                        // check if the mod is registered
                        if(WikiSearchSystem.RegisteredMods.ContainsKey(mod)) {
                            DoSearch(WikiSearchSystem.RegisteredMods[mod], name);
                        }
                        else {
                            ShowModMessage("item", name, Main.HoverItem.ModItem.Mod.DisplayName);
                        }
                    }
                    else {
                        DoSearch(TerrariaWiki, name);
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
                if(npc.ModNPC != null) {
                    Mod mod = npc.ModNPC.Mod;

                    if(WikiSearchSystem.RegisteredMods.ContainsKey(mod)) {
                        DoSearch(WikiSearchSystem.RegisteredMods[mod], npc.TypeName);
                    }
                    else {
                        ShowModMessage("NPC", npc.TypeName, npc.ModNPC.Mod.DisplayName);
                    }
                }
                else {
                    DoSearch(TerrariaWiki, npc.TypeName);
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
            Vector2 hoverTile = GetHoveringTile();

            if((hoverTile.X <= 0) ||
               (hoverTile.Y <= 0) ||
               (hoverTile.X > Main.tile.Width) ||
               (hoverTile.Y > Main.tile.Height)) {
                return false;
                //tile = Main.tile[(int)hoverTile.X, (int)hoverTile.Y];
            }

            // get the tile under the cursor
            Tile tile = Main.tile[(int)hoverTile.X, (int)hoverTile.Y];
            Item item = null;
            bool active = false;
            string term = string.Empty;

            active = tile.HasTile;

            // if the tile is inactive, it's either air or a wall
            if(active) {
                item = DefaultTileItems.FirstOrDefault(i => i.createTile == tile.TileType);

                if(item == null) {
                    foreach(HashSet<Item> set in ModTileItems.Values) {
                        item = set.FirstOrDefault(m => m.createTile == tile.TileType);
                        if(item != null) break;
                    }
                }
            }
            else if(tile.WallType > 0) {
                item = DefaultWallItems.FirstOrDefault(i => i.createWall == tile.TileType);

                if(item == null) {
                    foreach(HashSet<Item> set in ModWallItems.Values) {
                        item = set.FirstOrDefault(m => m.createWall == tile.TileType);
                        if(item != null) break;
                    }
                }
            }
            else if(tile.LiquidAmount > 0) {
                int liquid = tile.LiquidType;
                string search = "";

                switch(liquid) {
                    case LiquidID.Water:
                        search = "Water";
                        break;
                    case LiquidID.Lava:
                        search = "Lava";
                        break;
                    case LiquidID.Honey:
                        search = "Honey";
                        break;
                }

                DoSearch(TerrariaWiki, search);
            }

            if(item != null) {
                if(item.ModItem != null) {
                    Mod mod = item.ModItem.Mod;

                    if(WikiSearchSystem.RegisteredMods.ContainsKey(mod)) {
                        DoSearch(WikiSearchSystem.RegisteredMods[mod], item.Name);
                    }
                    else {
                        ShowModMessage("item", item.Name, item.ModItem.Mod.DisplayName);
                    }
                }
                else {
                    DoSearch(TerrariaWiki, item.Name);
                }
            }
            else if(active || (!active && tile.WallType > 0)) {
                Main.NewText("Cannot search for this tile. ID: " + (active ? tile.TileType : tile.WallType) + ".");
            }

            return true;
        }

        private static void ShowModMessage(string type, string name, string mod) {
            Main.NewText("Cannot search. \"" + name + "\" is a modded " + type + " from " + mod + ".");
        }

        private static void DoSearch(string url, string term) {
            // check if steam overlay option is true, steam is running, and if the game is using the steam overlay
            if(WikiSearchSystem.UseSteamOverlay && SteamAPI.IsSteamRunning() && SteamUtils.IsOverlayEnabled()) {
                SteamFriends.ActivateGameOverlayToWebPage(url.Replace("%s", term));
            }
            else {
                Process.Start(url.Replace("%s", term));
            }
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

        // following code modified from game source
        private static Vector2 GetHoveringTile() {
            int tileTargetX = (int)((Main.mouseX + Main.screenPosition.X) / 16f);
            int tileTargetY = (int)((Main.mouseY + Main.screenPosition.Y) / 16f);

            if(Main.LocalPlayer.gravDir == -1f) {
                tileTargetY = (int)((Main.screenPosition.Y + Main.screenHeight - Main.mouseY) / 16f);
            }

            return new Vector2(tileTargetX, tileTargetY);
        }
    }
}
