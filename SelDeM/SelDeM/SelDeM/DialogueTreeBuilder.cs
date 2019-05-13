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
        DialogTree<DialogBox> temp;

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
                cL++;
                if (line.Contains("*+"))
                {
                    numOfChoices = Convert.ToInt32(line.Substring(3));
                    break;
                }
                
                initdB.Add(line);
            }
            
            foreach (string s in initdB[initdB.Count - 1].Substring(2).Split(','))
                choices.Add(s);
            initdB.RemoveAt(initdB.Count-1);
            string dT = "";
            foreach (string s in initdB)
                dT += s +"\n";
            temp = new DialogTree<DialogBox>(new DialogBox(sb, cM, g, dT, choices));
            //for(int x = 0; x < numOfChoices; x++)
           // {
                string p = dialogue[cL];
                temp = BuildTree(dialogue, choices.Count);
           // }
            return temp;
        }

        private DialogTree<DialogBox> BuildTree(List<string> dialogue, int numOfChoices)
        {
            int numOfChoiceTemp = 0;
            List<string> initdB = new List<string>();
            List<string> choices = new List<string>();
            for (int loc = 0; loc < numOfChoices; loc++)
            {
                foreach (string line in dialogue)
                {
                    cL++;
                    if (line.Contains("*+"))
                    {
                        numOfChoiceTemp = Convert.ToInt32(line.Substring(3));
                        break;
                    }

                    initdB.Add(line);
                }
                foreach (string s in initdB[initdB.Count - 1].Substring(2).Split(','))
                    choices.Add(s);
                initdB.RemoveAt(initdB.Count - 1);
                temp.AddChildNode(BuildTree(dialogue, 0));
            }
            if (choices.Count > 0)
                return new DialogTree<DialogBox>(new DialogBox(sb, cM, g, "", choices));
            else
                return new DialogTree<DialogBox>(new DialogBox(sb, cM, g, ""));
        }
    }
}
