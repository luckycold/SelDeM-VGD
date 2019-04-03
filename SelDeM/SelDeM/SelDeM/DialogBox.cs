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
    class DialogBox
    {
        private Rectangle dialogBoxRect;
        SpriteBatch spriteBatch;
        private SpriteFont sp1;
        private Texture2D dialogBoxTexture;
        ContentManager Content;
        GraphicsDeviceManager graphics;
        KeyboardState kb, oldkb;
        private string text;
        private string[] lines;
        private int numLines, index;

        public DialogBox(SpriteBatch spriteBatch, ContentManager Content, GraphicsDeviceManager graphics, string text)
        {
            this.spriteBatch = spriteBatch;
            this.Content = Content;
            Content.RootDirectory = "Content";
            sp1 = Content.Load<SpriteFont>("DialogBoxFont");
            dialogBoxTexture = Content.Load<Texture2D>("txtbox");
            //creates dialog box that will take up the lower 1/5 of the screen
            this.graphics = graphics;
            int width = graphics.PreferredBackBufferWidth-100;
            int height = graphics.PreferredBackBufferHeight/5;
            dialogBoxRect = new Rectangle(50, graphics.PreferredBackBufferHeight-height-25, width, height);
            kb = Keyboard.GetState();
            oldkb = kb;
            this.text = feedText(text);
            lines = this.text.Split('\n');
            //gets the amount of lines of text that can fit in the textbox
            numLines = (int)(dialogBoxRect.Height / sp1.LineSpacing) - 2;
            index = 0;
        }

        //method takes in a block of text as a string, and formats it to wrap around the text box.
        public string feedText(string t)
        {
            //wrap around text based on width of dialogbox
            string line = "";
            string formattedText = "";
            string[] words = t.Split(' ');
            foreach (string word in words)
            {
                //checks to see if the next word can fit on the current line
                ////if (sp1.MeasureString(word).Length() > (dialogBoxRect.Width - (int)(dialogBoxRect.Width * .06)))
                ////{
                ////    int index = 0;
                ////    char[] letters = word.ToArray<char>();
                ////    for (int i = letters.Length - 1; i > 0; i--)
                ////    {
                ////        string temp1 = "";
                ////        for (int j = 0; j < i; j++)
                ////        {
                ////            temp1 = temp1 + letters[j];
                ////        }
                ////        if (sp1.MeasureString(temp1).Length() > (dialogBoxRect.Width - (int)(dialogBoxRect.Width * .06)))
                ////        {
                ////            index = i;
                ////            break;
                ////        }
                ////    }
                ////    string final = "";
                ////    for (int i = 0; i < index; i++)
                ////    {
                ////        final = final + letters[i];
                ////    }
                ////    formattedText = formattedText + final + '\n';
                ////    line = "-";
                ////    word2 = word.Substring(index);
                ////}
                if (sp1.MeasureString(line+word).Length()>dialogBoxRect.Width-(int)(dialogBoxRect.Width*.06))
                {
                    formattedText = formattedText + line + '\n';
                    line = "";
                }
                line = line + word + ' ';
            }
            return formattedText + line;
        }

        public Rectangle Rectangle
        {
            get { return dialogBoxRect; }
            set { dialogBoxRect = value; }
        }

        public Texture2D Texture
        {
            get { return dialogBoxTexture; }
            set { dialogBoxTexture = value; }
        }

        public SpriteFont SpriteFont
        {
            get { return sp1; }
            set { sp1 = value; }
        }

        public void update()
        {
            kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Up) && !oldkb.IsKeyDown(Keys.Up))
                lineUp();
            if (kb.IsKeyDown(Keys.Down) && !oldkb.IsKeyDown(Keys.Down))
                lineDown();
            oldkb = kb;
        }

        public void lineUp()
        {
            if (index>0)
                index--;
        }

        public void lineDown()
        {
            if (index<lines.Length-1)
                index++;
        }

        public void Draw()
        {
            spriteBatch.Draw(dialogBoxTexture, dialogBoxRect, Color.White);
            string display = "";
            for (int i = 0; i < numLines; i++)
            {
                if (index+i<lines.Length)
                    display = display + lines[index + i] + '\n';
            }
            spriteBatch.DrawString(sp1, display, new Vector2(dialogBoxRect.X+(int)(dialogBoxRect.Width*.04), dialogBoxRect.Y+(int)(dialogBoxRect.Height*.15)), Color.White);
        }
    }
}
