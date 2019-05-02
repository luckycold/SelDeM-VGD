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
            text = new string[length];



        }

        public DialogTree<DialogBox> getDialogBox(int i) //The way to use this is to first have an object of DialogueTreeBuilder, then: [objectName].getDialogBox([nodeNumber]).[DialogBoxMethods];
        {
            return tree[i];
        }

    }
}