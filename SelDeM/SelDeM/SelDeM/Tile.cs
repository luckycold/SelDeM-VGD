using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SelDeM
{
    public class Tile
    {
        Rectangle rect;
        enum TileFlags { plain, unwalkable, dialog }
        TileFlags flag;
        int ForcefieldOffset = 0;

        public Tile(Rectangle r, String tileType)
        {
            rect = r;
            setTile(tileType);
        }
        public Tile(Rectangle r)
        {
            rect = r;
            flag = TileFlags.plain;
        }

        public int TileForcefield
        {
            get { return ForcefieldOffset; }
            set { ForcefieldOffset = value; }
        }

        public Rectangle Rectangle
        {
            get
            {
                return this.rect;
            }
            set
            {
                this.rect = value;
            }
        }

        public Boolean checkFlagForPlayer(Player player)
        {
            switch (flag)
            {
                case TileFlags.plain:
                    {
                        return rect.Intersects(player.Rectangle);
                    }
                case TileFlags.unwalkable:
                    {
                        if (rect.Intersects(player.Rectangle))
                        {
                            //Moves inserted rectangle away from tile depending on closest side (This should not happen as it will make the player jitter if attempted to apply opposite force)
                            if (player.Rectangle.X + player.Rectangle.Width + player.Speed <= rect.X + rect.Width / 2 && player.PlayerKeyboard.IsKeyDown(Keys.D) && (player.HorizontalTile == this || player.HorizontalTile == null))
                            {
                                player.HorizontalTile = this;
                                player.setColX(1);
                                player.changeX(rect.X - player.Rectangle.Width);
                            }
                            else if(player.Rectangle.X - player.Speed> rect.X + rect.Width / 2 && player.PlayerKeyboard.IsKeyDown(Keys.A) && (player.HorizontalTile == this || player.HorizontalTile == null))
                            {
                                player.HorizontalTile = this;
                                player.setColX(-1);
                                player.changeX(rect.X + rect.Width);
                            }
                            else if(player.Rectangle.Y - player.Speed <= rect.Y + rect.Height / 2 && player.PlayerKeyboard.IsKeyDown(Keys.S) && (player.VerticalTile == this || player.VerticalTile == null))
                            {
                                player.VerticalTile = this;
                                player.setColY(1);
                                player.changeY(rect.Y - player.Rectangle.Height);
                            }
                            else if(player.Rectangle.Y + player.Rectangle.Height + player.Speed > rect.Y + rect.Height / 2 && player.PlayerKeyboard.IsKeyDown(Keys.W) && (player.VerticalTile == this || player.VerticalTile == null))
                            {
                                player.VerticalTile = this;
                                player.setColY(-1);
                                player.changeY(rect.Y + rect.Height);
                            }
                            return true;
                        }
                        if ((player.Rectangle.Y > rect.Y + rect.Height || player.Rectangle.Y + player.Rectangle.Height < rect.Y) ||
                            ((player.Rectangle.X > rect.X + rect.Width) || (player.Rectangle.X + player.Rectangle.Width < rect.X)) && player.VerticalTile == this)
                        {
                            player.setColY(0);
                            player.VerticalTile = null;
                        }

                        if ((player.Rectangle.X > rect.X + rect.Width || player.Rectangle.X + player.Rectangle.Width < rect.X) ||
                            ((player.Rectangle.Y > rect.Y + rect.Height) || (player.Rectangle.Y + player.Rectangle.Height < rect.Y)) && player.HorizontalTile == this)
                        {
                            player.setColX(0);
                            player.HorizontalTile = null;
                        }
                        
                        return false;
                    }
                case TileFlags.dialog:
                    {
                        if (rect.Intersects(player.Rectangle) && player.PlayerKeyboard.IsKeyDown(Keys.Enter))
                            Game1.curLevel.startDialog();
                        break;
                    }
            }
            return rect.Intersects(player.Rectangle);
        }

        internal String getLoc()
        {
            return rect.X + " " + rect.Y;
        }

        public void setTile(String tileType)
        {
            switch (tileType.ToLower())
            {
                case "plain":
                    {
                        flag = TileFlags.plain;
                        break;
                    }
                case "unwalkable":
                    {
                        flag = TileFlags.unwalkable;
                        break;
                    }
                case "dialog":
                    {
                        flag = TileFlags.dialog;
                        break;
                    }
                default:
                    {
                        flag = TileFlags.plain;
                        break;
                    }
            }
        }
    }
}

