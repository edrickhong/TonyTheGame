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
namespace GameStateManagement
{
    public class Player
    {
        public Rectangle collision;
        public Vector2 pos;
        public Texture2D texture;
        public Vector2 velocity;
        public bool draw;
        TimeSpan shootrate = TimeSpan.Zero;
        public int speed;
        public int height, width;
        public float radius;
        public float rotation;
        float acceleration=2f;
        float decelleration = 1f;
        float tspeed=8f;
        public bool shield, knockback;
        public Player(Texture2D newtexture, Vector2 vector)
        {
            texture = newtexture;
            pos = vector;
            height = texture.Height;
            width = texture.Width;
            draw = true;
            speed = 20;
        }
        public void LoadContent(ContentManager content)
        {
          //  texture = content.Load<Texture2D>("player1");
        }
        public void Draw(SpriteBatch spritebatch)
        {

           // spriteBatch.Draw(texture,Position,null,Color.White,Rotation,new Vector2(texture.Width / 2f, texture.Height / 2f),1f,SpriteEffects.None,0f);
            if (draw)
                spritebatch.Draw(texture,pos,null,Color.White,rotation,new Vector2(width/2f,height/2f),1f,SpriteEffects.None,0f);
                //spritebatch.Draw(texture, pos, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            collision = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);

        }
        public void Setrotate(float X,float Y) {

            rotation = -(float)Math.Atan2(-Y,X);
           
        }
        public void Move(Vector2 ac) {
            //X movement
            if(ac.Y<=-0.114){
                if(velocity.X>=(-tspeed))
                velocity.X -= acceleration;
            }
            else if (ac.Y >= 0.110)
            {
                if (velocity.X <= (tspeed))
                    velocity.X += acceleration;
            }
            else {
                if(velocity.X>0){
                    velocity.X -= decelleration;
                }
                if (velocity.X < 0)
                    velocity.X += decelleration;
            }

            //Y movement

            if (ac.X <= -0.190)
            {
                if (velocity.Y >= -tspeed)
                    velocity.Y -= acceleration;
            }
            else if (ac.X >= 0.102)
            {
                if (velocity.Y <= (tspeed))
                    velocity.Y += acceleration;
            }
            else
            {
                if (velocity.Y > 0)
                {
                    velocity.Y -= decelleration;
                }
                if (velocity.Y < 0)
                    velocity.Y += decelleration;
            }

           
          //velocity.Y += ac.X; 

           
           pos += velocity;
        }

        public float PositionY()
        {
            return pos.Y;
        }
        public float PositionX()
        {
            return pos.X;
        }

        
    }
}
