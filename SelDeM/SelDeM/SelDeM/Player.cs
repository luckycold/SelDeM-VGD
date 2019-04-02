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

        public void Update(KeyboardState kb, KeyboardState oldkb)
        {
            k = kb;
            o = oldkb;
            if (kb.IsKeyDown(Keys.W))
            {
                moveUp();
            }
            if (kb.IsKeyDown(Keys.A))
            {
                moveLeft();
            }
            if (kb.IsKeyDown(Keys.S))
            {
                moveDown();
            }
            if (kb.IsKeyDown(Keys.D))
            {
                moveRight();
            }
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public void Draw()
        {
            sb.Draw(tex, rect, Color.White);
        }

        public void moveUp()
        {
            pos.Y -= speed;
        }

        public void moveLeft()
        {
            pos.X -= speed;
        }

        public void moveDown()
        {
            pos.Y += speed;
        }

        public void moveRight()
        {
            pos.X += speed;
        }
    }
}
