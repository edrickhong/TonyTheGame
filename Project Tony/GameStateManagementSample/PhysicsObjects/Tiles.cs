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

namespace GameStateManagementSample.PhysicsObjects
{
    public class Tiles
    {//basic tile info
        public Texture2D texture;
        public Rectangle rectangle;
        public static ContentManager content;
        public void Draw(SpriteBatch spritebatch) {
            spritebatch.Draw(texture,rectangle,Color.White);
        }
    }
}
