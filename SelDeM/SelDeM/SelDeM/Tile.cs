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
        DialogTree<DialogBox> tree;
        Texture2D texture;
        SpriteBatch sb;
        public static int count=0;

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

        public Tile(Rectangle r, String tileType, DialogTree<DialogBox> t)
        {
            rect = r;
            setTile(tileType);
            tree = t;
        }

        public Tile(Rectangle r, String tileType, DialogTree<DialogBox> t, Texture2D text, SpriteBatch sb)
        {
            rect = r;
            setTile(tileType);
            tree = t;
            texture = text;
            this.sb = sb;
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

        public void Draw()
        {
            if (texture != null)
                sb.Draw(texture, rect, null, Color.White, 0f, new Vector2(0,0), SpriteEffects.None, .8f);
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
                            Vector2 direction = new Vector2(0,0);
                            //Moves inserted rectangle away from tile depending on closest side (This should not happen as it will make the player jitter if attempted to apply opposite force)
                            if (player.Rectangle.X + player.Rectangle.Width + player.Speed <= rect.X + rect.Width / 2 && player.PlayerKeyboard.IsKeyDown(Keys.D))
                            {
                                player.CanWalk = false;
                            }
                            else if(player.Rectangle.X - player.Speed> rect.X + rect.Width / 2 && player.PlayerKeyboard.IsKeyDown(Keys.A))
                            {
                                player.CanWalk = false;
                            }
                            else if(player.Rectangle.Y - player.Speed <= rect.Y + rect.Height / 2 && player.PlayerKeyboard.IsKeyDown(Keys.S))
                            {
                                player.CanWalk = false;
                            }
                            else if(player.Rectangle.Y + player.Rectangle.Height + player.Speed > rect.Y + rect.Height / 2 && player.PlayerKeyboard.IsKeyDown(Keys.W))
                            {
                                player.CanWalk = false;
                            }
                            player.move(direction, ForcefieldOffset);
                            return true;
                        }
                        return false;
                    }
                case TileFlags.dialog:
                    {
                        if (rect.Intersects(player.Rectangle) && player.PlayerKeyboard.IsKeyDown(Keys.Space) && !player.OldPlayerKeyboard.IsKeyDown(Keys.Space))
                        {
                            Game1.curLevel.startDialog(tree);
                            setTile("plain");
                            texture = null;
                        }
                        break;
                    }
            }
            return rect.Intersects(player.Rectangle);
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

