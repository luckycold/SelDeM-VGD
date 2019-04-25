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
        Boolean isDialogVisable;
        DialogTree<DialogBox> dT, curTreeLoc;
        static DialogueChoices choiceMaker;
        ContentManager content;
        double timer;


        public Level(SpriteBatch spriteBatch, Texture2D texture, int tileSize, Rectangle screenBounds, Player player, GraphicsDeviceManager graphics, ContentManager content)
        {
            sb = spriteBatch;
            tex = texture;
            this.tileSize = tileSize;
            this.content = content;
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
            dT = new DialogTree<DialogBox>(new DialogBox(spriteBatch, content, graphics, "Lorem ipsum dolor sit amet", new List<string>{ "1 perry perry", "2 perry perry" }));
            dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 1", new List<string>()));
            dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 2", new List<string>()));
            curTreeLoc = dT;
            timer = 0;
        }

        private void makeChoice(DialogBox obj)
        {
            if(curTreeLoc.Children.Count != 0)
                curTreeLoc = curTreeLoc[choiceMaker.choiceChosen];
            if(curTreeLoc.Value.Choices != null)
                choiceMaker = new DialogueChoices(sb, content, obj.Choices);
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

        public void Update(GameTime gameTime, KeyboardState kb, KeyboardState oldkb)
        {
            curRec = rec;
            foreach (Tile tile in levelTiles)
            {
                if (tile.Rectangle.Intersects(scrnB))
                    tile.checkFlagForPlayer(player);
            }
            if (curTreeLoc.Value.EnterPressed && curTreeLoc.Children.Count == 0)
            {
                isDialogVisable = false;
                curTreeLoc = dT;
                choiceMaker = null;
            }
            if (isDialogVisable && choiceMaker == null)
            {
                curTreeLoc.Value.update(gameTime);
                if (curTreeLoc.Value.EnterPressed)
                    choiceMaker = new DialogueChoices(sb, content, curTreeLoc.Value.Choices);
            }
            else if (isDialogVisable)
            {
                curTreeLoc.Value.update(gameTime);
                if (curTreeLoc.Value.isDone() && curTreeLoc.Value.Choices != null && choiceMaker.choiceChosen == -1)
                {
                    //ChoiceMaker is immediately making a choice when the next dialogue box is attempting to be entered.
                    choiceMaker.Update(kb, oldkb);
                }

                else if (curTreeLoc.Value.isDone() && curTreeLoc.Value.Choices != null && choiceMaker.choiceChosen != -1)
                {
                    makeChoice(curTreeLoc.Value);
                }
                player.CanWalk = false;
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

        public void Draw(GameTime gameTime)
        {
            sb.Draw(tex, rec, null, Color.White, 0f, new Vector2(0,0), SpriteEffects.None, 0f);
            if (isDialogVisable)
            {
                curTreeLoc.Value.Draw();
                if ((curTreeLoc.Value.EnterPressed && curTreeLoc.Value.isDone()) || (choiceMaker != null && choiceMaker.choiceChosen != 0))
                    choiceMaker.Draw(gameTime);
            }
        }

        public void startDialog()
        {
            isDialogVisable = true;
        }
    }
}
