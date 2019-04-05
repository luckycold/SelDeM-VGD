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
    class Player
    {
        SpriteBatch sb;
        Texture2D tex;
        Rectangle rect;
        Vector2 pos;
        KeyboardState k, o;
        float speed;
        Boolean canWalk = true;

        public Player(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, float playerSpeed)
        {
            sb = spriteBatch;
            tex = texture;
            rect = rectangle;
            pos = new Vector2(rect.X, rect.Y);
            speed = playerSpeed;
        }

        public Rectangle Rectangle
        {
            get { return rect; }
            set { rect = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public KeyboardState PlayerKeyboard
        {
            get { return k; }
        }

        public Boolean CanWalk
        {
            get { return canWalk; }
            set { canWalk = value; }
        }

        public void Update(KeyboardState kb, KeyboardState oldkb, MouseState ms, MouseState oldms)
        {
            k = kb;
            o = oldkb;
            if(ms.ScrollWheelValue > oldms.ScrollWheelValue)
            {
                Game1.camHand.ZoomIn(.25f);
            }
            if (ms.ScrollWheelValue < oldms.ScrollWheelValue)
            {
                Game1.camHand.ZoomOut(.25f);
            }
            Vector2 direction = new Vector2(
                //X-Movement
                kb.IsKeyDown(Keys.A) && canWalk ? -1 : (kb.IsKeyDown(Keys.D) && canWalk ? 1 : 0)
                ,
                //Y-Movement
                kb.IsKeyDown(Keys.W) && canWalk ? -1 : (kb.IsKeyDown(Keys.S) && canWalk ? 1 : 0));
            canWalk = true;
            move(direction);
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public void move(Vector2 direction)
        {
            if(direction.X != 0 && direction.Y != 0)
                direction.Normalize();
            pos += direction * speed;
        }

        public void move(Vector2 direction, float addedForce)
        {
            if (direction.X != 0 && direction.Y != 0)
                direction.Normalize();
            float addedSpeed = speed + addedForce;
            pos += direction * addedSpeed;
        }

        public void Draw()
        {
            sb.Draw(tex, rect, Color.White);
        }
    }
}
