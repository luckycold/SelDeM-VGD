using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SelDeM
{
    class Level
    {
        SpriteBatch sb;
        Texture2D tex;
        Rectangle rec, scrnB;
        public static Rectangle curRec;
        int tileSize = 64;
        Tile[,] levelTiles;
        Player player;

        public Level(SpriteBatch spriteBatch, Texture2D texture, int tileSize, Rectangle screenBounds, Player player)
        {
            sb = spriteBatch;
            tex = texture;
            this.tileSize = tileSize;
            rec = new Rectangle(0, 0, tex.Width - tex.Width % tileSize, tex.Height - tex.Height % tileSize);
            levelTiles = new Tile[tex.Width/tileSize,tex.Height/tileSize];
            for (int row = 0; row < levelTiles.GetLength(0); row++)
            {
                for (int col = 0; col < levelTiles.GetLength(1); col++)
                {
                    levelTiles[row, col] = new Tile(new Rectangle(row * tileSize, col * tileSize, tileSize, tileSize));
                }
            }
            this.player = player;
            scrnB = screenBounds;
        }

        public Tile[,] Tiles
        {
            get { return levelTiles; }
            set { levelTiles = value; }
        }

        public Boolean setTile(int row, int col, Tile tile)
        {
            if (row >= 0 && levelTiles.GetLength(0) > row && col >= 0 && levelTiles.GetLength(1) > col)
            {
                levelTiles[row, col] = tile;
                return true;
            }
            return false;
        }

        public Rectangle Rectangle
        {
            get { return rec; }
            set { rec = value; }
        }

        public void Update()
        {
            curRec = rec;
            foreach (Tile tile in levelTiles)
            {
                if (tile.Rectangle.Intersects(scrnB))
                    tile.checkFlagForPlayer(player);
            }

            playerBoundaryCheck();

        }

        private void playerBoundaryCheck()
        {
            if (player.Rectangle.X < rec.X)
                player.move(new Vector2(1, 0));
            if (player.Rectangle.X + player.Rectangle.Width > rec.Width)
                player.move(new Vector2(-1, 0));
            if (player.Rectangle.Y < rec.Y)
                player.move(new Vector2(0, 1));
            if (player.Rectangle.Y + player.Rectangle.Height > rec.Height)
                player.move(new Vector2(0, -1));
        }

        public void Draw()
        {
            sb.Draw(tex, rec, Color.White);
        }
    }
}
