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
    class Map
    {
        public List<CollisionTiles> tiles = new List<CollisionTiles>();
        public int width, height;
        public Map() {
        }
        //adds a tile where the map value is>0
        public void Generate(int[,] map,int size) {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    int number = map[x, y];
                    if (number > 0)
                        tiles.Add(new CollisionTiles(number,new Rectangle(x*size,y*size,size,size)));

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch) {
            foreach (CollisionTiles c in tiles) {
                c.Draw(spritebatch);
            }
                
        }
    }
}
