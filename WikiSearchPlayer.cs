using Steamworks;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace WikiSearch {
    public class WikiSearchPlayer : ModPlayer {
        public const string TerrariaWiki = "http://terraria.fandom.com/index.php?search=%s";

        public override void ProcessTriggers(TriggersSet triggersSet) {
            if(WikiSearchSystem.WikiSearchKey != null &&
                WikiSearchSystem.WikiSearchKey.JustPressed) {
                if(SearchItem()) return;
                if(SearchNPC()) return;
                if(SearchTile()) return;
            }
        }

        private static bool SearchItem() {
            if(!Main.playerInventory || Main.HoverItem.type <= ItemID.None) {
                return false;
            }

            string url = TerrariaWiki;
            Item item = Main.HoverItem;
            string term = item.Name;

            if(!string.IsNullOrWhiteSpace(term) && item.ModItem != null) {
                Mod mod = item.ModItem.Mod;
                term = Regex.Replace(term, @"\[.+\]", "").Trim();

                if(WikiSearchSystem.RegisteredMods.ContainsKey(mod)) {
                    url = WikiSearchSystem.RegisteredMods[mod].Link;
                }
            }

            DoSearch(url, term);
            return true;
        }

        // Thanks to DrEinsteinium for the base method
        private static bool SearchNPC() {
            NPC npc = GetHoveringNPC();

            if(npc == null) {
                return false;
            }

            string term = npc.TypeName;
            string url = TerrariaWiki;

            if(npc.ModNPC != null) {
                Mod mod = npc.ModNPC.Mod;

                if(WikiSearchSystem.RegisteredMods.ContainsKey(mod)) {
                    url = WikiSearchSystem.RegisteredMods[mod].Link;
                }
            }

            DoSearch(url, term);
            return true;
        }

        private static bool SearchTile() {
            Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];

            if(tile == null) {
                return false;
            }

            string url = TerrariaWiki;
            string term = "";
            Mod mod = null;

            ModTile modTile = TileLoader.GetTile(tile.TileType);
            ModWall modWall = WallLoader.GetWall(tile.WallType);

            #region Naturally Generated
            if(tile.TileType == TileID.Pots) {
                term = "Pots";
            }
            else if(tile.TileType == TileID.DemonAltar) {
                term = WorldGen.crimson ? "Crimson Altar" : "Demon Altar";
            }
            else if(tile.TileType == TileID.ShadowOrbs) {
                term = WorldGen.crimson ? "Crimson Heart" : "Shadow Orb";
            }
            else if(tile.TileType == TileID.FallenLog) {
                term = "Fallen Log";
            }
            else if(tile.TileType == TileID.Larva) {
                term = "Larva";
            }
            else if(tile.TileType == TileID.PlanteraBulb) {
                term = "Plantera's Bulb";
            }
            else if(tile.LiquidAmount > 0) {
                switch(tile.LiquidType) {
                    case LiquidID.Water:
                        term = "Water";
                        break;
                    case LiquidID.Lava:
                        term = "Lava";
                        break;
                    case LiquidID.Honey:
                        term = "Honey";
                        break;
                }
            }
            #endregion
            // check if modded tile
            else if(modTile != null) {
                mod = modTile.Mod;

                if(WikiSearchSystem.RegisteredMods.ContainsKey(mod)) {
                    Item item = WikiSearchSystem.RegisteredMods[mod].TileItems.FirstOrDefault(t => t.createTile == tile.TileType);

                    if(item != null) {
                        term = item.Name;
                    }
                }
            }
            // check if vanilla tile
            else if(tile.HasTile) {
                Item item = WikiSearchSystem.DefaultTileItems.FirstOrDefault(t => t.createTile == tile.TileType);

                if(item != null) {
                    term = item.Name;
                }
                else {
                    WorldGen.KillTile_GetItemDrops(
                    Player.tileTargetX,
                    Player.tileTargetY,
                    tile,
                    out int dropItem,
                    out int _,
                    out int _,
                    out int _);

                    if(dropItem > ItemID.None) {
                        term = new Item(dropItem).Name;
                    }
                }
            }
            // check if modded wall
            else if(modWall != null) {
                mod = modWall.Mod;
                term = new Item(modWall.ItemDrop).Name;
            }
            // check if vanilla wall
            else if(tile.WallType > WallID.None) {
                Item item = WikiSearchSystem.DefaultWallItems.FirstOrDefault(w => w.createWall == tile.WallType);

                if(item != null) {
                    term = item.Name;
                }
            }

            if(mod != null && WikiSearchSystem.RegisteredMods.ContainsKey(mod)) {
                url = WikiSearchSystem.RegisteredMods[mod].Link;
            }

            if(string.IsNullOrWhiteSpace(term)) {
                Main.NewText("Cannot search for this tile. ID: " + (tile.HasTile ? tile.TileType : tile.WallType) + ".");
            }
            else {
                DoSearch(url, term);
            }

            return true;
        }

        private static void DoSearch(string url, string term) {
            // check if steam overlay option is true, steam is running, and if the game is using the steam overlay
            if(WikiSearchSystem.UseSteamOverlay && SteamAPI.IsSteamRunning() && SteamUtils.IsOverlayEnabled()) {
                SteamFriends.ActivateGameOverlayToWebPage(url.Replace("%s", term));
            }
            else {
                Process.Start(new ProcessStartInfo {
                    FileName = url.Replace("%s", term),
                    UseShellExecute = true
                });
            }
        }

        private static void ShowModMessage(string name, string mod) {
            Main.NewText($"Cannot search. \"{name}\" is a modded object from {mod}.");
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
