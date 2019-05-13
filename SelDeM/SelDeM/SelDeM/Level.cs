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
        bool isChoiceVisable, nextframe;
        int count;
        public static DialogueTreeBuilder dTB;


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
            //dT = new DialogTree<DialogBox>(new DialogBox(spriteBatch, content, graphics, "Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet", new List<string> { "1 perry perry", "2 perry perry", "3 perry perry" }));
            //dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 1 chosen", new List<string> { "1 perry perry" }));
            //dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 2", new List<string>()));
            //dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 3", new List<string>()));
            //dT[0].AddChild(new DialogBox(spriteBatch, content, graphics, "first", new List<string> { "1 perry perry", "2 perry perry", "3 perry perry" }));
            //dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "oneone", new List<string> { "1 perry perry", "2 perry perry", "3 perry perry" }));
            //dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "twotwo", new List<string>()));
            //dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "threethree", new List<string>()));
            //dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet", new List<string>()));
            //dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "two", new List<string>()));
            //dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "three", new List<string>()));
            dTB = new DialogueTreeBuilder(spriteBatch, content, graphics);
            dT = dTB.BuildTreeFromFile(@"Content\\Dialog.txt");
            curTreeLoc = dT;
            isChoiceVisable = false;
            choiceMaker = new DialogueChoices(sb, content, curTreeLoc.Value.Choices, graphics);
            nextframe = false;
            count = 0;
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
            if (isDialogVisable)
            {
                curTreeLoc.Value.update(gameTime, kb, oldkb);
                player.CanWalk = false;
                if (curTreeLoc.Value.EnterPressed && curTreeLoc.Value.isDone)
                {
                    if (curTreeLoc.Value.hasChoices())
                    {
                        choiceMaker = new DialogueChoices(sb, content, curTreeLoc.Value.Choices, Game1.graphics);
                        isDialogVisable = false;
                        isChoiceVisable = true;
                    }
                    else
                    {
                        choiceMaker = null;
                        isDialogVisable = false;
                        isChoiceVisable = false;
                        player.CanWalk = true;
                    }
                }
            }
            if (isChoiceVisable)
            {
                if (nextframe)
                {
                    choiceMaker.Update(kb, oldkb);
                    if (choiceMaker.choiceChosen != -1)
                    {
                        if (curTreeLoc.Children.Count > 0)
                            curTreeLoc = curTreeLoc[choiceMaker.choiceChosen];
                        Console.WriteLine("chosen");
                        isChoiceVisable = false;
                        isDialogVisable = true;
                        nextframe = false;
                    }
                }
                else
                {
                    count++;
                    if (count == 15)
                    {
                        nextframe = !nextframe;
                        count = 0;
                    }
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
            }
            if (isChoiceVisable)
            {
                choiceMaker.Draw(gameTime);
            }
        }

        public void startDialog()
        {
            isDialogVisable = true;
        }
    }
}
