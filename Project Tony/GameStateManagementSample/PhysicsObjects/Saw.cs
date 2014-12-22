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
    class Saw : PhysicsEntity
    {//saws have to be tweaked. life has to be deducted per bounce. now it deducts while intersecting w other objects
        int life = 100;
        bool Bounce;
        public bool delete=false;
        public float speed=3;
        bool isRight;
        float spinRate = 0.2f;
        public Saw(Texture2D t, Vector2 p,float s,float w,bool e,bool bounce,bool right) : base(t,p,s,w,false) {
            Bounce = bounce;
            isRight = right;
            origin = new Vector2(t.Width / 2, t.Height / 2);
            if (!isRight)
                life = 28;
        }
        //bounce collision with tiles. bounces against tiles when touching bottom or top. deletes when touching left or right
        public void BounceCollision(Rectangle rect, int xOffset, int yOffset)
        {
            if (col.TouchTop(rect))
            {
               // pos.Y = rect.Y - col.Height + 5;
                if(velocity.Y>0)
                velocity.Y *= -1;
                if(!isRight)
                life--;

            }

            else if (col.TouchBottom(rect) || (col.TouchRight(rect) && col.TouchLeft(rect)))
            {
                // pos.Y = rect.Y + col.Height+ 5;
                if (velocity.Y < 0)
                    velocity.Y *= -1;
                if(!isRight)
                life--;
            }

              if (col.TouchLeft(rect)&&isRight)
            {
                delete = true;
            }
             if (col.TouchRight(rect)&& isRight)
            {
                delete = true;
            }
            
             if (pos.X > xOffset - rect.Width) delete=true;
            if (pos.Y > yOffset - rect.Height) delete=true;
        }
        //checks for collision w player
        public void Collision(Player player,Blood blood) {
            if(col.Intersects(player.col)&&!player.gameOver){
                player.gameOver = true;
                blood.EmitterLocation = new Vector2(player.col.Location.X + (player.col.Width / 2), player.col.Location.Y + (player.col.Height / 2));
                blood.toggle = true;

            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (life == 0)
                delete = true;
            col = new Rectangle((int)pos.X,(int)pos.Y,(int)(texture.Width*scale),(int)(texture.Height*scale));
            pos += velocity;
           
            if (isRight)
            {
                rotation += spinRate;
                pos.X += speed*2;
            }
            else {
                rotation -= spinRate;
                pos.X -= speed*0.4f;
            }
        }
    }
}
