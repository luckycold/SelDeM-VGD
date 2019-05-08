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
        private string text;
        private string[] lines;
        private int numLines, index;
        private string[] textChunks, typedText;
        private double typedTextLength;
        private bool[] typedAlready;
        private int delay;
        private bool isDoneDrawing;
        private List<string> choices;
        private bool enterPressed;

        public DialogBox(SpriteBatch spriteBatch, ContentManager Content, GraphicsDeviceManager graphics, string text/*, List<string> choices*/)
        {
            Choices = null;
            this.spriteBatch = spriteBatch;
            this.Content = Content;
            sp1 = Content.Load<SpriteFont>("DialogBoxFont");
            dialogBoxTexture = Content.Load<Texture2D>("txtbox");
            //creates dialog box that will take up the lower 1/5 of the screen
            this.graphics = graphics;
            int width = graphics.PreferredBackBufferWidth-100;
            int height = graphics.PreferredBackBufferHeight/5;
            dialogBoxRect = new Rectangle(50, graphics.PreferredBackBufferHeight-height-25, width, height);
            //keyboards for input
            this.text = feedText(text);
            lines = this.text.Split('\n');
            //gets the amount of lines of text that can fit in the textbox
            numLines = (int)(dialogBoxRect.Height / sp1.LineSpacing) - 2;
            //filling array with the chunks that will be displayed at a time
            textChunks = formatIntoChunks();
            typedText = new string[textChunks.Length];
            //initializing arrays
            for (int i =0; i<typedText.Length;i++)
                typedText[i] = "";
            typedAlready = new bool[typedText.Length];
            for (int i = 0; i < typedAlready.Length; i++)
                typedAlready[i] = false;
            index = 0;
            //delay in between each letter
            delay = 1;
            isDoneDrawing = false;
            typedTextLength = 0;
            this.choices = new List<string>();
            enterPressed = false;
            rectangleStartingPosition = new Vector2(dialogBoxRect.X, dialogBoxRect.Y);
        }
        public DialogBox(SpriteBatch spriteBatch, ContentManager Content, GraphicsDeviceManager graphics, string text, List<string> choices)
        {
            Choices = null;
            this.spriteBatch = spriteBatch;
            this.Content = Content;
            sp1 = Content.Load<SpriteFont>("DialogBoxFont");
            dialogBoxTexture = Content.Load<Texture2D>("txtbox");
            //creates dialog box that will take up the lower 1/5 of the screen
            this.graphics = graphics;
            int width = graphics.PreferredBackBufferWidth - 100;
            int height = graphics.PreferredBackBufferHeight / 5;
            dialogBoxRect = new Rectangle(50, graphics.PreferredBackBufferHeight - height - 25, width, height);
            //keyboards for input
            this.text = feedText(text);
            lines = this.text.Split('\n');
            //gets the amount of lines of text that can fit in the textbox
            numLines = (int)(dialogBoxRect.Height / sp1.LineSpacing) - 2;
            //filling array with the chunks that will be displayed at a time
            textChunks = formatIntoChunks();
            typedText = new string[textChunks.Length];
            //initializing arrays
            for (int i = 0; i < typedText.Length; i++)
                typedText[i] = "";
            typedAlready = new bool[typedText.Length];
            for (int i = 0; i < typedAlready.Length; i++)
                typedAlready[i] = false;
            index = 0;
            //delay in between each letter
            delay = 5;
            isDoneDrawing = false;
            typedTextLength = 0;
            this.choices = choices;
            enterPressed = false;
        }
        public void addChoice(string choice)
        {
            choices.Add(choice);
        }
        

        public string[] formatIntoChunks()
        {
            string[] arr;
            if (lines.Length < numLines)
                arr = new string[1];
            else
                arr = new string[(lines.Length / numLines)+1];
            string chunk = "";
            int ind = 0;
            if (lines.Length == 1)
            {
                arr[0] = lines[0];
                return arr;
            }
            for (int i =0; i<lines.Length;i++)
            {
                if ((i + 1) % numLines != numLines)
                    chunk = chunk + lines[i] + '\n';
                else
                    chunk = chunk + lines[i];
                if (((i + 1) % numLines == 0) ||( i == lines.Length - 1))
                {
                    arr[ind] = chunk;
                    chunk = "";
                    ind++;
                }
            }
            return arr;
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
                if (sp1.MeasureString(line+word).Length()>dialogBoxRect.Width-(int)(dialogBoxRect.Width*.08))
                {
                    formattedText = formattedText + line + '\n';
                    line = "";
                }
                line = line + word + ' ';
            }
            return formattedText + line;
        }

        public List<string> Choices
        {
            get { return choices; }
            set { choices = value; }
        }

        public int numChoices
        {
            get { return choices.Count; }
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

        public void update(GameTime gametime, KeyboardState kb, KeyboardState oldkb)
        {
            if (!isDoneDrawing)
            {
                if (delay == 0)
                {
                    typedText[index] = textChunks[index];
                }
                else if (typedTextLength < textChunks[index].Length)
                {
                    typedTextLength = typedTextLength + gametime.ElapsedGameTime.TotalMilliseconds / delay;
                    if (typedTextLength >= textChunks[index].Length)
                    {
                        typedTextLength = textChunks[index].Length;
                        isDoneDrawing = true;
                    }
                    typedText[index] = textChunks[index].Substring(0, (int)typedTextLength);
                }
            }
            else
            {
                typedAlready[index] = true;
                if (kb.IsKeyDown(Keys.Left) && !oldkb.IsKeyDown(Keys.Left))
                {
                    lineUp();
                    if (!typedAlready[index])
                    {
                        isDoneDrawing = false;
                        typedTextLength = 0;
                    }
                }
                if (kb.IsKeyDown(Keys.Right) && !oldkb.IsKeyDown(Keys.Right))
                {
                    lineDown();
                    if (!typedAlready[index])
                    {
                        isDoneDrawing = false;
                        typedTextLength = 0;
                    }
                }

            }
            if (isDone)
            {
                if (kb.IsKeyDown(Keys.Enter) && !oldkb.IsKeyDown(Keys.Enter))
                {
                    enterPressed = true;
                }
            }
        }

        public bool isDone
        {
            get { return (typedAlready[typedAlready.Length - 1] && (index == typedAlready.Length - 1)); }
        }

        public bool EnterPressed
        {
            get { return enterPressed; }
        }

        public void lineUp()
        {
            if (index>0)
                index--;
        }

        public void lineDown()
        {
            if (index<textChunks.Length-1)
                index++;
        }

        public void Draw()
        {
            int camerapositionX =(int) Game1.camHand.Camera.Pos.X;
            int camerapositionY = (int)Game1.camHand.Camera.Pos.Y;
            Vector2 textPos = new Vector2(dialogBoxRect.X + (int)(dialogBoxRect.Width * .04) + camerapositionX, dialogBoxRect.Y + (int)(dialogBoxRect.Height * .15) + camerapositionY);
            spriteBatch.Draw(dialogBoxTexture, new Rectangle(dialogBoxRect.X+camerapositionX, dialogBoxRect.Y+camerapositionY, dialogBoxRect.Width, dialogBoxRect.Height) , null, Color.White, 0f, new Vector2(0,0), SpriteEffects.None, .95f);
            spriteBatch.DrawString(sp1, typedText[index], textPos, Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 1f);
        }

        public bool hasChoices()
        {
            if (choices!=null)
                return choices.Count != 0;
            return false;
        }
    }
}
