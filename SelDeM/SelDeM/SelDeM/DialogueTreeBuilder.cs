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
    class DialogueTreeBuilder
    {
        SpriteBatch spriteBatch;
        ContentManager content;
        GraphicsDeviceManager graphics;
        string[] text;
        DialogTree<DialogBox> tree;
        DialogBox[] children;
        SpriteFont font;

        public DialogueTreeBuilder(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager graphics, string path)
        {
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.graphics = graphics;

            font = content.Load<SpriteFont>("DialogChoiceFont");
            StreamReader reader = new StreamReader(path);
            int length = 0;
            while (!reader.EndOfStream)
            {
                if (!reader.ReadLine().Contains("*+"))
                {
                    reader.ReadLine();
                    length++;
                }
                else
                    continue;
            }
            text = new string[length];
            readFile(path);

            DialogBox parent = new DialogBox(this.spriteBatch, this.content, this.graphics, text[0]);
            tree = new DialogTree<DialogBox>(parent);
            //children = new DialogBox[2];
            //children[0] = new DialogBox(spriteBatch, content, graphics, text[1]);
            //children[1] = new DialogBox(spriteBatch, content, graphics, text[2]);

            tree.AddChildren(children);
            //tree[2].addChildren();
        }

        public DialogTree<DialogBox> getDialogBox(int i) //The way to use this is to first have an object of DialogueTreeBuilder, then: [objectName].getDialogBox([nodeNumber]).[DialogBoxMethods];
        {
            return tree[i];
        }

        private void readFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            int i = 0;
            int times2 = 0;
            while (!reader.EndOfStream)
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
    }
}