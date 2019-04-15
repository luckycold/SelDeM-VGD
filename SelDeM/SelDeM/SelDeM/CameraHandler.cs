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
    public class CameraHandler
    {
        Vector2 brdrOfst;
        Camera cam;
        int mxZ, mnZ, screenWidth, screenHeight;
        Rectangle cFP;
        float camPS;

        public CameraHandler(GraphicsDevice gd)
        {
            cam = new Camera(gd);
            brdrOfst = new Vector2(0, 0);
            mnZ = 1;
            mxZ = 2;
            screenWidth = gd.Viewport.Width;
            screenHeight = gd.Viewport.Height;
            camPS = 1f;
        }

        public CameraHandler(GraphicsDevice gd, Vector2 borderOffset, int maxZoom, int minZoom, float camPanSpeed)
        {
            cam = new Camera(gd);
            brdrOfst = borderOffset;
            mnZ = minZoom;
            mxZ = maxZoom;
            screenWidth = gd.Viewport.Width;
            screenHeight = gd.Viewport.Height;
            camPS = camPanSpeed;
        }

        public Camera Camera
        {
            get { return cam; }
            set { cam = value; }
        }

        public void Update(Rectangle cameraFocusPoint)
        {
            cFP = cameraFocusPoint;
            Vector2 cT = new Vector2(0, 0);
            //Moves Camera with cameraFocusPoint
            if (cFP.X + cFP.Width >= cam.pos.X + screenWidth - brdrOfst.X)
                cT.X += camPS;
            if (cFP.X < cam.pos.X + brdrOfst.X)
                cT.X -= camPS;
            if (cFP.Y + cFP.Height >= cam.pos.Y + screenHeight - brdrOfst.Y)
                cT.Y += camPS;
            if (cFP.Y < cam.pos.Y + brdrOfst.Y)
                cT.Y -= camPS;

            //Moves Camera back onto map when it goes off screen
            if (cam.pos.X < Level.curRec.X)
                cT.X += camPS;
            if (cam.pos.X + screenWidth > Level.curRec.X + Level.curRec.Width)
                cT.X -= camPS;
            if (cam.pos.Y < Level.curRec.Y)
                cT.Y += camPS;
            if (cam.pos.Y + screenHeight > Level.curRec.Y + Level.curRec.Height)
                cT.Y -= camPS;

            cam.Move(cT);
        }

        public float ZoomIn(float amount)
        {
            if(cam.Zoom < mxZ)
                cam.Zoom += amount;
            return cam.Zoom;
        }

        public float ZoomOut(float amount)
        {
            if(cam.Zoom > mnZ)
                cam.Zoom -= amount;
            return cam.Zoom;
        }
    }
}
