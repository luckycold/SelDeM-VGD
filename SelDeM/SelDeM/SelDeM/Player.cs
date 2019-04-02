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
            Vector2 direction = new Vector2(
                //X-Movement
                kb.IsKeyDown(Keys.A) ? -1 : (kb.IsKeyDown(Keys.D) ? 1 : 0)
                ,
                //Y-Movement
                kb.IsKeyDown(Keys.W) ? -1 : (kb.IsKeyDown(Keys.S) ? 1 : 0));
            Vector2.Normalize(direction);
            move(direction);
            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public void move(Vector2 direction)
        {
            pos += direction * speed;
        }

        public void Draw()
        {
            sb.Draw(tex, rect, Color.White);
        }

        public void moveUp(float addedForce)
        {
            pos.Y -= speed + addedForce;
        }

        public void moveLeft(float addedForce)
        {
            pos.X -= speed + addedForce;
        }

        public void moveDown(float addedForce)
        {
            pos.Y += speed + addedForce;
        }

        public void moveRight(float addedForce)
        {
            pos.X += speed + addedForce;
        }

    }
}
