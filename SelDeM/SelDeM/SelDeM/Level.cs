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
        Rectangle rec;
        int tileSize = 64;
        Tile[,] levelTiles;

        public Level(SpriteBatch spriteBatch, Texture2D texture, int tileSize)
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

        }

        public void Draw()
        {
            sb.Draw(tex, rec, Color.White);
        }
    }
}
