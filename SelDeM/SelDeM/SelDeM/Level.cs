﻿using System;
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
            //dT = new DialogTree<DialogBox>(new DialogBox(spriteBatch, content, graphics, "Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet Lorem ipsum dolor sit amet", new List<string>{ "1 perry perry", "2 perry perry", "3 perry perry" }));
            //dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 1", new List<string> { "1 perry perry"}));
            //dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 2", new List<string>()));
            //dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Choice 3", new List<string>()));
            //dT[0].AddChild(new DialogBox(spriteBatch, content, graphics, "fdsaklf", new List<string> { "1 perry perry", "2 perry perry" , "3 perry perry"}));
            //dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "fdsjakefdsge", new List<string> { "1 perry perry", "2 perry perry", "3 perry perry" }));
            //dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "abcdef", new List<string>()));
            //dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "abcfdsafefdaefdef", new List<string>()));
            //dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "one", new List<string>()));
            //dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "two", new List<string>()));
            //dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "three", new List<string>()));
            dT = new DialogTree<DialogBox>(new DialogBox(spriteBatch, content, graphics, "Hi!", new List<string> { "Greet back"}));
            dT.AddChild(new DialogBox(spriteBatch, content, graphics, "Hello.", new List<string> { "Let him introduce himself" }));
            dT[0].AddChild(new DialogBox(spriteBatch, content, graphics, "My name is Pete Hamburg. I'm a high school student at Allen High School. I'm a junior. I have zero social skills, no friends, and my grades are below average, to say the least. I have been like this since middle school, and they always said that high school would be a new start for me, but that wasn't the case. My family is dysfunctional. My dad left us when I was in elementary school, and I haven't seen him since. I hardly remember what he looks like. My mother works two jobs to accommodate for us, one shift from the morning till five in the afternoon, and then she joins the night shift and gets home at around 2 in the morning every night. I have to take care of myself for the most part, at least I'm independent. Too independent it seems though, since I can't make friends and all of my love interests have ended miserably. What do you want me to do?", new List<string> { "Go to school", "Skip and eat breakfast", "Keep sleeping" }));
            dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "Wow, school is as boring as usual. I wish someone would come and bully me... *Bully walks up to character and pushes character down*", new List<string> { "Fight the bully", "Get bullied" }));
            dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "MMM, breakfast", new List<string> { "Eat food" ,"Throw away food" }));
            dT[0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "Keep sleeping", new List<string> { "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ" }));
            dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "Punches, kicks, somersaults into a hind kick, and lastly fatal blow.", new List<string> { }));
            dT[0][0][0].AddChild(new DialogBox(spriteBatch, content, graphics, "Oh no, I'm getting bullied. Ow, ouch, oof, yikes.", new List<string> { }));
            dT[0][0][1].AddChild(new DialogBox(spriteBatch, content, graphics, "Stomach capacity = 100%", new List<string> { }));
            dT[0][0][1].AddChild(new DialogBox(spriteBatch, content, graphics, "Nevermind, I guess this doesn't look that great. Better throw it away!", new List<string> { }));
            dT[0][0][2].AddChild(new DialogBox(spriteBatch, content, graphics, "Zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz\nzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz...", new List<string> { }));
            //dTB = new DialogueTreeBuilder(spriteBatch, content, graphics);
            //dT = dTB.BuildTreeFromFile(@"Content\\Dialog.txt");
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
                        curTreeLoc = dT;
                        choiceMaker = new DialogueChoices(sb, content, curTreeLoc.Value.Choices,Game1.graphics);
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
            Vector2 temp = new Vector2(0, 0);
            if (player.Rectangle.X < rec.X)
                temp.X += 1;
            if (player.Rectangle.X + player.Rectangle.Width > rec.Width)
                temp.X -= 1;
            if (player.Rectangle.Y < rec.Y)
                temp.Y += 1;
            if (player.Rectangle.Y + player.Rectangle.Height > rec.Height)
                temp.Y -= 1;
            player.move(temp);
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
