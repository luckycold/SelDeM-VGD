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
        DialogTree<DialogBox> tree;
        string[] text;

        public DialogueTreeBuilder(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager graphics, string path)
        {
            text = new string[3];
            readFile(path);

            DialogBox parent = new DialogBox(spriteBatch, content, graphics, text[0]);
            tree = new DialogTree<DialogBox>(parent); //tree is null for some reason
            DialogBox[] child = new DialogBox[2];
            child[0] = new DialogBox(spriteBatch, content, graphics, text[1]);
            child[1] = new DialogBox(spriteBatch, content, graphics, text[2]);
            tree.AddChildren(child);
        }

        private void readFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            text[0] = reader.ReadLine() + reader.ReadLine();
            reader.ReadLine(); //Reads *+2
            text[1] = reader.ReadLine();
            reader.ReadLine(); //Reads *+E
            text[2] = reader.ReadLine();
            reader.ReadLine(); //Reads *+E
        }
    }
}
