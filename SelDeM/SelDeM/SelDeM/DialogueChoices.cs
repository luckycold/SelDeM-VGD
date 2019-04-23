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
        bool enterPressed;
        StreamReader reader;
        SpriteFont font;
        KeyboardState oldKB;
        Rectangle[] arrowRect;
        Texture2D arrowTexture;
        SpriteBatch spriteBatch;
        Vector2 vector0, vector1;
        int maxChoice, choice;
        Vector2 textPos;

        public DialogueChoices(SpriteBatch spriteBatch, ContentManager contentManager, List<String> choices) //show these choices from list, and then return the int of choice chosen
        {
            oldKB = Keyboard.GetState();
            maxChoice = choices.Count;
            confirm = new String[choices.Count];
            enterPressed = false;
            choice = 0;
            readChoices(choices);


            this.spriteBatch = spriteBatch;
            arrowTexture = contentManager.Load<Texture2D>("ChoiceArrow");

            //these vectors will change the location of both the text and the arrow image
            arrowRect = new Rectangle[3];
            arrowRect[0] = new Rectangle(0, 0, 20, 32);
            arrowRect[1] = new Rectangle(0, 32, 20, 32);
            arrowRect[2] = new Rectangle(50, 800, arrowRect[0].Width, arrowRect[0].Height);
            textPos = new Vector2(arrowRect[2].X + arrowRect[2].Width, arrowRect[2].Y-arrowRect[2].Height/2);

            font = contentManager.Load<SpriteFont>("DialogChoiceFont");
        }

        private void readChoices(List<String> choices)
        {
            for(int x = 0; x < choices.Count; x++)
            {
                confirm[x] = choices[x];
            }
        }

        public void Update(KeyboardState kb, KeyboardState oldkb)
        {
            if (kb.IsKeyDown(Keys.Up) && !oldKB.IsKeyDown(Keys.Up) && !enterPressed)
            {
                if (choice < 0)
                {
                    choice--;
                    arrowRect[2].Y -= arrowRect[2].Height/2;
                }
            }

            if (kb.IsKeyDown(Keys.Down) && !oldKB.IsKeyDown(Keys.Down) && !enterPressed)
            {
                if (choice < maxChoice)
                {
                    choice++;
                    arrowRect[2].Y += arrowRect[2].Height / 2;
                }

            }
            if (kb.IsKeyDown(Keys.Enter))
            {
                enterPressed = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (enterPressed != true)
            {
                //Arrow blinking
                if (gameTime.TotalGameTime.Seconds % 2 == 0)
                    spriteBatch.Draw(arrowTexture, arrowRect[2], arrowRect[0], Color.White, 0f, new Vector2(0,0), SpriteEffects.None, 0.99f);
                else
                    spriteBatch.Draw(arrowTexture, arrowRect[2], arrowRect[1], Color.White, 0f, new Vector2(0,0), SpriteEffects.None, 0.99f);
                //Choice text
                for(int x = 0; x < confirm.Length; x++)
                {
                    spriteBatch.DrawString(font, confirm[x], new Vector2(textPos.X, textPos.Y + (x * arrowRect[0].Height/2)), Color.Black,0f,Vector2.Zero, 1, SpriteEffects.None,1f);
                }
                
            }
        }

        public int choiceChosen
        {
            get
            {
                if(enterPressed)
                    return choice;
                return -1;
            }

        }
    }
}
