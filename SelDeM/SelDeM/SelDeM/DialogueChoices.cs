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
    class DialogueChoices : Game1
    {
        String[] confirm;
        bool choice;
        StreamReader reader;
        SpriteFont font;
        KeyboardState oldKB;
        Rectangle[] arrowRect;
        Texture2D arrowTexture;
        SpriteBatch spriteBatch;
        public DialogueChoices(SpriteBatch spriteBatch, ContentManager contentManager, ???,String path, GameTime gT)
        {
            oldKB = Keyboard.GetState();
            confirm = new String[2];
            readChoices(path);
            choice = true; //Choice1 is true, Choice2 is false
            Input(gT);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("DialogChoiceFont");

            arrowRect = new Rectangle[2];
            arrowRect[0] = new Rectangle(0, 0, 9, 9);
            arrowRect[1] = new Rectangle(9, 0, 9, 9);
            arrowTexture = Content.Load<Texture2D>("Choice Arrow");

            Draww(gT);
        }

        private void readChoices(String path)
        {
            reader = new StreamReader(path);
            confirm[0] = reader.ReadLine();
            confirm[1] = reader.ReadLine();
        }

        private void Input(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Up) && !oldKB.IsKeyDown(Keys.Up) || kb.IsKeyDown(Keys.Down) && kb.IsKeyDown(Keys.Down))
                choice = !choice;
        }

        private void Draww(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(arrowTexture, arrowRect[0], Color.White);
            spriteBatch.DrawString(font, confirm[0], new Vector2(50, 50), Color.White);
            spriteBatch.DrawString(font, confirm[1], new Vector2(80, 80), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
