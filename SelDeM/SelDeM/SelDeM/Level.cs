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
    public class Level
    {
        SpriteBatch sb;
        Texture2D tex;
        Rectangle rec, scrnB;
        public static Rectangle curRec;
        int tileSize = 64;
        Tile[,] levelTiles;
        Player player;
        DialogBox d;
        Boolean isDialogVisable;

        public Level(SpriteBatch spriteBatch, Texture2D texture, int tileSize, Rectangle screenBounds, Player player, GraphicsDeviceManager graphics, ContentManager content)
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
            isDialogVisable = false;
            d = new DialogBox(spriteBatch, content, graphics, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc vitae faucibus orci, a consequat mauris. Curabitur sed fermentum nibh. Aenean volutpat augue erat, sit amet suscipit justo consequat non. Morbi id velit in lorem gravida finibus. Curabitur euismod pellentesque purus, in rutrum nisl ultricies vel. Praesent vestibulum purus a lacinia blandit. Etiam laoreet metus ultrices tristique convallis. Vivamus lacinia et odio a dignissim. Maecenas vel lectus eu sem pulvinar condimentum. Nulla a tristique ante.");
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

        public void Update(GameTime gameTime)
        {
            curRec = rec;
            foreach (Tile tile in levelTiles)
            {
                if (tile.Rectangle.Intersects(scrnB))
                    tile.checkFlagForPlayer(player);
            }

            if (isDialogVisable)
            {
                d.update(gameTime);
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
            if (isDialogVisable)
                d.Draw();
        }

        public void startDialog()
        {
            isDialogVisable = true;
        }
    }
}
