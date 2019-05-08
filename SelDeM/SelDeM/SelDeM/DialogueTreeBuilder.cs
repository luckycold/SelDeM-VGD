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
    public class DialogueTreeBuilder
    {
        SpriteBatch sb;
        ContentManager cM;
        GraphicsDeviceManager g;
        int cL = 0;

        public DialogueTreeBuilder(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            sb = spriteBatch;
            cM = contentManager;
            g = graphics;
        }

        internal DialogTree<DialogBox> BuildTreeFromFile(string path)
        {
            StreamReader read = new StreamReader(path);
            List<string> dialogue = new List<string>();
            List<string> choices = new List<string>();
            int numOfChoices = 0;
            cL = 0;
            while (!read.EndOfStream)
            {
                string line = read.ReadLine();
                if(!line.Contains("*//"))
                    dialogue.Add(line);
            }
            List<string> initdB = new List<string>();
            foreach(string line in dialogue)
            {
                initdB.Add(line);
                if (line.Contains("*+"))
                {
                    numOfChoices = Convert.ToInt32(line.Substring(3));
                    break;
                }
            }
            foreach (string s in initdB[initdB.Count - 2].Substring(2).Split(','))
                choices.Add(s);
            initdB.RemoveAt(initdB.Count-1);
            string dT = "";
            foreach (string s in initdB)
                dT += "\n" + s;
            DialogTree<DialogBox> temp = new DialogTree<DialogBox>(new DialogBox(sb, cM, g, dT, choices));
            for(int x = 0; x < numOfChoices; x++)
            {
                temp = BuildTree(dialogue, temp);
            }
            return temp;
        }

        private DialogTree<DialogBox> BuildTree(List<string> dialogue, DialogTree<DialogBox> parent)
        {
            string dFB = "";
            DialogTree<DialogBox> tree = parent;
            if (cL < dialogue.Count && !dialogue[cL].Contains("*+ "))
            {
                List<string> initdB = new List<string>();
                List<string> choices = new List<string>();
                while (!dialogue[cL].Contains("*+ "))
                {
                    dFB += dialogue[cL];
                    initdB.Add(dialogue[cL]);
                    cL++;
                }
                int numOfChoices = Convert.ToInt32(dialogue[cL].Substring(3));
                 if (numOfChoices > 1)
                {
                    foreach (string s in initdB[initdB.Count - 1].Substring(2).Split(','))
                        choices.Add(s);
                    initdB.RemoveAt(initdB.Count - 1);
                }
                if (choices.Count == 0)
                    choices = new List<string>();
                string dT = "";
                foreach (string s in initdB)
                    dT += "\n" + s;
                tree.AddChild(new DialogBox(sb, cM, g, dFB, choices));
                for (int curChoice = 0; curChoice < numOfChoices; curChoice++)
                {
                    if(numOfChoices < parent.Children.Count-1)
                        tree = BuildTree(dialogue, parent.Children[curChoice]);
                }
            }
            else
                cL++;
            return tree;
        }
    }
}
