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
    class PhysicsEntity
    {
        //governs all objects that will be using physics:gravity etc
        public Vector2 Force = Vector2.Zero;
        public bool restrictX=false;
        protected Vector2 origin = Vector2.Zero;
        protected Texture2D texture;
        protected float CxOffset = 40, CyOffset = 10;
        public Rectangle col;
        public Vector2 pos;
        protected Color color=Color.White;
        protected float rotation=0;
        protected float scale;
        public bool gravityEnabled;
        protected float gravity;
        protected float weight;
        protected float width, height;
        protected int tag;
        public float gacceleration=0;
        protected float terminalVelocityY=12;
        protected float terminalVelocityX = 2;
        public Vector2 velocity=Vector2.Zero;
        public PhysicsEntity(Texture2D t,Vector2 p,float s,float w,bool enabled) {
            texture = t;
            pos = p;
            scale = s;
            weight = w;
            gravityEnabled = enabled;
            width = texture.Width;
            height = texture.Height;
            gravity = 0.05f;
        }

        public virtual void Update(GameTime gameTime)
        {
            //if at terminal velocity, remain at terminal velocity
            if (velocity.X > terminalVelocityX)
                velocity.X = terminalVelocityX;
            if (velocity.X < -terminalVelocityX)
                velocity.X = -terminalVelocityX;

            if (gravityEnabled)
            {
                if (velocity.Y < terminalVelocityY) //accelerate according to gravity
                {
                    gacceleration += gravity;
                    velocity.Y += gacceleration;
                }
            }
            else
            {
                gacceleration = 0;//no acelleration by gravity
            }
        }

        public void AddForce(Vector2 force)
        {
            velocity += force;
        }

        public float VectorToAngle(Vector2 a)
        {
            //radians
            return (float)Math.Atan2(a.X, -a.Y);

        }
        public Vector2 AngleToVector(float r)
        {
            return new Vector2((float)Math.Cos(r), (float)Math.Sin(r));
        }

        

        public void SpriteCollision(Rectangle rect, int xOffset, int yOffset)
        {//checks for collison between two objects.(used for player)
            //for each if state ment(if physicsentity is touching object at top/bottom/etc)
            //eg; the one below if the entity is touching the top of the object
            if (col.TouchTop(rect))
            {
                //allows player to stand on object. stops pushing player down so player won't go thru object
                pos.Y = rect.Y - col.Height - (CyOffset-1);
                velocity.Y = 0;
                gacceleration = 0;

            }
            if (col.TouchLeft(rect)&&!col.TouchRight(rect))
            {
                //stops the player from moving into object
                pos.X = rect.X - col.Width - 4 - CxOffset-2;
                restrictX = true;
            }
            if (col.TouchRight(rect)&&!col.TouchLeft(rect))
            {
                //stops the player from moving into object
                pos.X = rect.X + rect.Width + 2 - CxOffset;
                restrictX = true; 
            }
            if (col.TouchBottom(rect) || (col.TouchRight(rect) && col.TouchLeft(rect)))
            {
                //bounce the player down
                if(velocity.Y<0)
                velocity.Y *= -1;
            }
            //clamps player to level
            if (pos.X > xOffset - rect.Width-40) pos.X = xOffset - rect.Width-40;
            if (pos.Y > yOffset - rect.Height) pos.Y = yOffset - rect.Height;
        }


        public virtual void Draw(SpriteBatch spritebatch)
        {
             spritebatch.Draw(texture, pos, null, color, rotation, origin,scale,SpriteEffects.None,0f);

        }

    }
     
}
