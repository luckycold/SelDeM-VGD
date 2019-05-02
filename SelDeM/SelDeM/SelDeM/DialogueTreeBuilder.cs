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
        static int cL = 0;

        public DialogueTreeBuilder(SpriteBatch spriteBatch, ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            sb = spriteBatch;
            cM = contentManager;
            g = graphics;
        }

        DialogTree<DialogBox> BuildTreeFromFile(string path)
        {
            StreamReader read = new StreamReader(path);
            List<string> dialogue = new List<string>();
            List<string> choices = new List<string>();
            int numOfChoices = 0;
            while (!read.EndOfStream)
            {
                dialogue.Add(read.ReadLine());
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
            foreach (string s in initdB[initdB.Count - 1].Substring(2).Split(','))
                choices.Add(s);
            initdB.RemoveAt(initdB.Count-1);
            string dT = "";
            foreach (string s in initdB)
                dT += "\n" + s;
            DialogTree<DialogBox> temp = new DialogTree<DialogBox>(new DialogBox(sb, cM, g, dT, choices));
            for(int x = 0; x < numOfChoices; x++)
            {
                temp=BuildTree(dialogue, ref temp);
            }
            return temp;
        }

        DialogTree<DialogBox> BuildTree(List<string> dialogue,ref DialogTree<DialogBox> parent)
        {
            throw new NotImplementedException();
            string dFB = "";

            while(!dialogue[cL].Contains("*+ "))
            {
                dFB += dialogue[cL];
                cL++;
            }
            int numOfChoices = Convert.ToInt32(dialogue[cL].Substring(3));
            for (int curChoice = 0; curChoice < numOfChoices; curChoice++)
            {
                cL++;
                parent.AddChild(new DialogBox(sb, cM, g, dFB));
                //Can't create a reference to a indexer aka list.count
                BuildTree(dialogue, ref parent[curChoice]);
            }
        }
    }
}
