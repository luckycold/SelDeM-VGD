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
/*
                1
            2       3
          4   5   6   7
                 8 9
*/
namespace SelDeM
{
    class DialogueTree
    {
        SpriteBatch spriteBatch;
        ContentManager content;
        GraphicsDeviceManager graphics;
        string[] text;
        DialogTree<DialogBox> tree;
        DialogBox[] children;
        SpriteFont font;


        public DialogueTree(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager graphics, string path)
        {
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.graphics = graphics;
            font = content.Load<SpriteFont>("DialogChoiceFont");
            text = new string[7];
            readFile(path);
            

            DialogBox parent = new DialogBox(this.spriteBatch, this.content, this.graphics, text[0]);
            tree = new DialogTree<DialogBox>(parent);
            children = new DialogBox[2];
            children[0] = new DialogBox(spriteBatch, content, graphics, text[1]);//the text will need to be changed
            children[1] = new DialogBox(spriteBatch, content, graphics, text[2]);
            tree.AddChildren(children);
        }

        private void readFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            int i = 0;
            while(!reader.EndOfStream)
            {
                string temp = reader.ReadLine();
                if (!temp.Contains("*+"))
                {
                    text[i] = temp;
                    i++;
                }
                else
                    continue;
            }
        }

        public void drawText()
        {
            spriteBatch.DrawString(font, "" + tree[0].Value, new Vector2(100, 100), Color.White);
        }
    }
}
