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
using System.IO;

namespace SelDeM
{
    class DialogueChoices
    {
        String[] confirm;
        bool choice;
        StreamReader reader;
        SpriteFont font;
        KeyboardState oldKB;
        Rectangle[] arrowRect;
        Texture2D arrowTexture;
        SpriteBatch spriteBatch;
        Vector2 vector0, vector1;
        public DialogueChoices(SpriteBatch spriteBatch, ContentManager contentManager,String path)
        {
            oldKB = Keyboard.GetState();

            confirm = new String[2];
            readChoices(path);
            choice = false; //Choice1 is true, Choice2 is false

            this.spriteBatch = spriteBatch;
            arrowTexture = contentManager.Load<Texture2D>("ChoiceArrow");

            //these vectors will change the location of both the text and the arrow image
            vector0 = new Vector2(50, 400); 
            vector1 = new Vector2(50, 420);
            arrowRect = new Rectangle[3];
            arrowRect[0] = new Rectangle(0, 0, 20, 32);
            arrowRect[1] = new Rectangle(0, 32, 20, 32);
            arrowRect[2] = new Rectangle((int)vector1.X- arrowRect[0].Width, (int)vector1.Y, arrowRect[0].Width, arrowRect[0].Height);
            font = contentManager.Load<SpriteFont>("DialogChoiceFont");
        }

        private void readChoices(String path)
        {
            reader = new StreamReader(path);
            confirm[0] = reader.ReadLine();
            confirm[1] = reader.ReadLine();
        }

        public void Input()
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Up) && !oldKB.IsKeyDown(Keys.Up))
            {
                choice = true;
                arrowRect[2].Y = (int)vector0.Y;
            }

            if (kb.IsKeyDown(Keys.Down) && !oldKB.IsKeyDown(Keys.Down))
            {
                choice = false;
                arrowRect[2].Y = (int)vector1.Y;
            }
            kb = oldKB;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //Arrow blinking
            if (gameTime.TotalGameTime.Seconds % 2 == 0)
                spriteBatch.Draw(arrowTexture, arrowRect[2], arrowRect[0], Color.White);
            else
                spriteBatch.Draw(arrowTexture, arrowRect[2], arrowRect[1], Color.White);
            //Choice text
            spriteBatch.DrawString(font, confirm[0], vector0, Color.White);
            spriteBatch.DrawString(font, confirm[1], vector1, Color.White);

            //spriteBatch.DrawString(font, "" + choice, new Vector2(50, 50), Color.White); //to test true or false of choice

            spriteBatch.End();
        }
    }
}
