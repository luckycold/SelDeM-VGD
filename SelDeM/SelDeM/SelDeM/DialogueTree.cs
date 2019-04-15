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
    class DialogueTree
    {
        SpriteBatch spriteBatch;
        ContentManager content;
        GraphicsDeviceManager graphics;
        string text;
        DialogTree<DialogBox> tree;
        SpriteFont font;
        public DialogueTree(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager graphics, string text)
        {
            this.spriteBatch = spriteBatch;
            this.content = content;
            this.graphics = graphics;
            this.text = text;
            font = content.Load<SpriteFont>("DialogBoxFont");
            DialogBox parent = new DialogBox(this.spriteBatch, this.content, this.graphics, this.text);
            tree = new DialogTree<DialogBox>(parent);
            DialogBox child1Gen1 = new DialogBox(spriteBatch, content, graphics, text);
            DialogBox child2Gen1 = new DialogBox(spriteBatch, content, graphics, text);
            AddChildren(child1Gen1, child2Gen1);
        }

        private void AddChildren(DialogBox child1, DialogBox child2)
        {
            tree.AddChild(child1);
            tree.AddChild(child2);
        }

        public void Draw() //won't be drawing the tree
        {
            spriteBatch.DrawString(font, "" + tree[1], new Vector2(0,0), Color.Red);
        }
    }
}
