using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameStateManagement;

namespace GameStateManagementSample
{
    class _2DCamera
    {
        public Matrix transform;
        private Vector2 centre;
        private Viewport viewport;
        public Vector2 relativecamPos;
        public _2DCamera(Viewport view) {
            viewport = view;
        }
        public void Update(Vector2 pos, int xOffset, int yOffset)
        {
            if (pos.X < viewport.Width / 2)
                centre.X = viewport.Width / 2;
            else if (pos.X > xOffset - (viewport.Width / 2))
                centre.X = xOffset - (viewport.Width / 2);
            else
                centre.X = pos.X;

            if (pos.Y < viewport.Height / 2)
                centre.Y = viewport.Height / 2;
            else if (pos.Y > yOffset - (viewport.Height / 2))
                centre.Y = yOffset - (viewport.Height / 2);
            else
                centre.Y = pos.Y;
            relativecamPos = new Vector2(-centre.X + (viewport.Width / 2), -centre.Y + (viewport.Height / 2));
            transform = Matrix.CreateTranslation(new Vector3(-centre.X + (viewport.Width / 2), -centre.Y + (viewport.Height / 2), 0));
        }
    }
   
}
