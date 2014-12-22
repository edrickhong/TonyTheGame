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
   public class CollisionTiles : Tiles
    {//loads and sets the texture of that tile name
        public CollisionTiles(int i,Rectangle rect) {
            texture = content.Load<Texture2D>("Tile"+i);
            this.rectangle = rect;
        }
    }
}
