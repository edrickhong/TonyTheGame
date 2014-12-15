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
    public class EnemyClass
    {
        public int tag;
        public Rectangle collision;
        public Vector2 pos;
        public Texture2D texture;
        public Vector2 velocity;
        public bool draw;
        public int speed;
        public int height, width;
        public Player playerPos;
        public Vector2 direction;
        public int lives=3;
        protected bool move = true;
        public bool isrotating = true;
        public bool bounce;
        public float rotation;
        protected Color color = Color.White;
        public bool fire = false;
        public bool orbiting=false;
        public float rotdir = 0.01f;
       public Vector2 run;
       public bool knockback;
       protected float friction=1;
       public Vector2 knock=Vector2.Zero;
       public float size = 0.1f;
        protected float growth=0.04f;
        public bool charging = false;
        public EnemyClass(Texture2D newtexture, Vector2 vector,Player p)
        {
            texture = newtexture;
            pos = vector;
            height = texture.Height;
            width = texture.Width;
            draw = true;
            speed = 5;
            playerPos = p;
            bounce = false;
            tag = 0;
        }

        
        public bool lockOn(Vector2 touchpos) {
            if(Vector2.Distance(pos,touchpos)<=50){
                return true;
            }
            else
                return false;
        }

        public void Behaviour() {
            
            if (!knockback)
            {
                
                    direction = playerPos.pos - pos;
                    direction.Normalize();
                    pos += direction * speed;
                
            }
            else {
                System.Diagnostics.Debug.WriteLine(knock);
                pos += knock;
                if(knock.X==0 && knock.Y==0){
                    knockback = false;
                }


                if (knock.X > 0)
                    knock.X -= friction;
                if (knock.Y > 0)
                    knock.Y -= friction;
                if (knock.X < 0)
                    knock.X += friction;
                if (knock.Y < 0)
                    knock.Y += friction;
            }
            
        }
        public void LoadContent(ContentManager content)
        {
            //  texture = content.Load<Texture2D>("player1");
        }
        public virtual void Draw(SpriteBatch spritebatch)
        {
            if (draw)
            {
                //spritebatch.Draw(texture, pos, null, Color.White, rotation, new Vector2(width / 2f, height / 2f), 1f, SpriteEffects.None, 0f);
                //spritebatch.Draw(texture, pos, Color.White);
                spritebatch.Draw(texture, pos, null, color, rotation, new Vector2(width / 2f, height / 2f),size,SpriteEffects.None,0f);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            
            collision = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
            if (size >= 1)
                Behaviour();
            else
                size += growth;

        }

        public void Setrotate(Vector2 p)
        {

            rotation = -(float)Math.Atan2(-p.Y, p.X);

        }

        public bool Collide(Vector2 point) {

            return Vector2.Distance(pos,point)<40;
        }
        public float PositionY()
        {
            return pos.Y;
        }
        public float PositionX()
        {
            return pos.X;
        }

        public virtual void Reset() { }

    }
}
