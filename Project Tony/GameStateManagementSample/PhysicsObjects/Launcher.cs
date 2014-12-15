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
    class Launcher
    {
        //saw
        Texture2D texture2;
        int curFrame = 0;
        Vector2 pos2;
        public bool bounce = false;
        public bool gravity=false;
        bool Right;
        //launcher
        SpriteSheetReader spritesheet;
        public List<Saw> saws = new List<Saw>();
        Vector2 pos;
        Texture2D texture;
        int fire;
        //animations
        int[] LaunchR = { 1,2,3,4,5,6,7,8,9,8,7,6,5,4,3,2,1};//19
        int[] LaunchL = {23,16,17,18,19,12,13,14,15,14,13,12,19,18,17,16,23};

        public Launcher(Texture2D t,Vector2 p,bool isRight,Texture2D t2) {
            texture2 = t2;
            Right = isRight;
            pos = p;
            pos2 = pos;
           
            texture = t;
            spritesheet = new SpriteSheetReader(texture,165,120,3,5);
            if (isRight)
            {
                spritesheet.setAnimation(LaunchR, 80, true);
                pos2.X += 87;
                pos2.Y += 65;
            }
            else
            {
                spritesheet.setAnimation(LaunchL, 180, true);
                pos2.X += 62;
                pos2.Y += 75;
            }

        }

        public void Update(GameTime gameTime) {
            spritesheet.Update(gameTime);
            if (Right)
                fire = 15;
            else
                fire = 11;

            for(int i=0;i<saws.Count;i++){
                saws[i].Update(gameTime);
                if(saws[i].delete){saws.RemoveAt(i);
                System.Diagnostics.Debug.WriteLine("deleting");
                }
            }
            if (spritesheet.frameIndex == fire&&curFrame!=spritesheet.frameIndex)
            {
                saws.Add(new Saw(texture2, pos2, 0.1f, 1.5f, gravity, bounce, Right));
            }
            curFrame = spritesheet.frameIndex;
        }

        public void Collision(Player player) { }
        public void Bounce(PhysicsObjects.Tiles tile) { }

        public void Draw(SpriteBatch spriteBatch) { 
            spritesheet.Draw(spriteBatch,pos,Color.White,0f,new Vector2(0,0),1);
            foreach(Saw s in saws){
                s.Draw(spriteBatch);
            }
        }


        
    }
}
