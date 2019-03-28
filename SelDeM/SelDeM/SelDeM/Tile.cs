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
        enum TileFlags { plain, unwalkable }
        TileFlags flag;
        int playerForcefieldOffset = 64;

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

        public Boolean checkFlags(Rectangle rec)
        {
            switch (flag)
            {
                case TileFlags.plain:
                    {
                        return rect.Intersects(rec);
                    }
                case TileFlags.unwalkable:
                    {
                        if (rect.Intersects(rec))
                        {
                            //Moves inserted rectangle away from tile depending on closest side
                            if (rec.X + rec.Width <= rect.X + rect.Width / 2)
                            {
                                rec.X -= playerForcefieldOffset;
                            }
                            else if(rec.X + rec.Width > rect.X + rect.Width / 2)
                            {
                                rec.X += playerForcefieldOffset;
                            }
                            if(rec.Y + rec.Height <= rect.Y + rect.Height / 2)
                            {
                                rec.Y -= playerForcefieldOffset;
                            }
                            else if(rec.Y + rec.Height > rect.Y + rect.Height / 2)
                            {
                                rec.Y += playerForcefieldOffset;
                            }
                            return true;
                        }
                        return false;
                    }
            }
            return rect.Intersects(rec);
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
                default:
                    {
                        flag = TileFlags.plain;
                        break;
                    }
            }
        }
    }
}

