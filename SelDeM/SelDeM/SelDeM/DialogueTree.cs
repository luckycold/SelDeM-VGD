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
        SortedDictionary<int, string> sortedDictionary;
        List<String> values;

        public DialogueTree(string textpath)
        {
            sortedDictionary = new SortedDictionary<int, string>();
            values = new List<string>();
            readTextFile(textpath);
            
            for(int i = 0; i < values.Count;i++)
            {
                sortedDictionary.Add(i, values[i]);
            }

        }

        private void readTextFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            while(!reader.EndOfStream)
            {
                values.Add(reader.ReadLine()); 
            }
        }

        public   whichChoice(/*int choiceNumber - How would only 0 and 1 even work for traversing tree, I think i has to be a string or an int that follows the order*/)
        {

        }

        //SpriteBatch spriteBatch;
        //ContentManager content;
        //GraphicsDeviceManager graphics;
        //string text;
        //DialogTree<DialogBox> tree;
        //SpriteFont font;
        //public DialogueTree(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager graphics, string text)
        //{
        //    this.spriteBatch = spriteBatch;
        //    this.content = content;
        //    this.graphics = graphics;
        //    this.text = text;
        //    font = content.Load<SpriteFont>("DialogBoxFont");
        //    DialogBox parent = new DialogBox(this.spriteBatch, this.content, this.graphics, this.text);
        //    tree = new DialogTree<DialogBox>(parent);
        //    DialogBox child1Gen1 = new DialogBox(spriteBatch, content, graphics, text);
        //    DialogBox child2Gen1 = new DialogBox(spriteBatch, content, graphics, text);
        //    AddChildren(child1Gen1, child2Gen1);
        //}

        //private void AddChildren(DialogBox child1, DialogBox child2)
        //{
        //    tree.AddChild(child1);
        //    tree.AddChild(child2);
        //}

        //public void Draw() //won't be drawing the tree
        //{
        //    spriteBatch.DrawString(font, "" + tree[1], new Vector2(0,0), Color.Red);
        //}


    }
}
