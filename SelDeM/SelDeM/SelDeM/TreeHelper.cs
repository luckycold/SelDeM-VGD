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
    public class TreeHelper
    {
        DialogTree<DialogBox> dT, curTreeLoc;
        bool isDialogVisable, isChoiceVisable;
        int count;
        bool nextframe;
        static DialogueChoices choiceMaker;
        Player player;
        SpriteBatch sb;
        ContentManager content;

        public TreeHelper(DialogTree<DialogBox> dT, SpriteBatch sb, ContentManager content, GraphicsDeviceManager graphics, Player p)
        {
            this.dT = dT;
            curTreeLoc = dT;
            isDialogVisable = false;
            isChoiceVisable = false;
            count = 0;
            nextframe = false;
            this.sb = sb;
            this.content = content;
            choiceMaker = new DialogueChoices(sb, content, curTreeLoc.Value.Choices, graphics);
            player = p;
        }

        public void Update(GameTime gameTime, KeyboardState kb, KeyboardState oldkb)
        {
            
            if (isDialogVisable)
            {
                curTreeLoc.Value.update(gameTime, kb, oldkb);
                player.CanWalk = false;
                if (curTreeLoc.Value.EnterPressed && curTreeLoc.Value.isDone)
                {
                    if (curTreeLoc.Value.hasChoices())
                    {
                        choiceMaker = new DialogueChoices(sb, content, curTreeLoc.Value.Choices, Game1.graphics);
                        isDialogVisable = false;
                        isChoiceVisable = true;
                    }
                    else
                    {
                        choiceMaker = null;
                        isDialogVisable = false;
                        isChoiceVisable = false;
                        player.CanWalk = true;
                    }
                }
            }
            if (isChoiceVisable)
            {
                if (nextframe)
                {
                    choiceMaker.Update(kb, oldkb);
                    if (choiceMaker.choiceChosen != -1)
                    {
                        if (curTreeLoc.Children.Count > 0)
                            curTreeLoc = curTreeLoc[choiceMaker.choiceChosen];
                        Console.WriteLine("chosen");
                        isChoiceVisable = false;
                        isDialogVisable = true;
                        nextframe = false;
                    }
                }
                else
                {
                    count++;
                    if (count == 15)
                    {
                        nextframe = !nextframe;
                        count = 0;
                    }
                }
                player.CanWalk = false;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (isDialogVisable)
                curTreeLoc.Value.Draw();
            if (isChoiceVisable)
                choiceMaker.Draw(gameTime);
        }

        public bool DialogVisable
        {
            get { return isDialogVisable; }
            set { isDialogVisable = value; }
        }

        public bool ChoiceVisable
        {
            get { return isChoiceVisable; }
            set { isChoiceVisable = value; }
        }
    }
}
