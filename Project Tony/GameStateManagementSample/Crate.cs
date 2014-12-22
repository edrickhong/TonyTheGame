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
    class Crate
    {
        Texture2D texture;
        public Vector2 pos;
        public Rectangle col;
        public float scale;
        public float originalScale;
        float delay = 0;
        public Crate(Texture2D t,Vector2 p,float s) {
            texture = t;
            pos = p;
            scale =s;
            col = new Rectangle((int)pos.X,(int)pos.Y,(int)(texture.Width*scale),(int)(texture.Height*scale));
            originalScale = s;

        }


        public void Draw(SpriteBatch spritebatch) {
            spritebatch.Draw(texture,pos,null,Color.White,0,Vector2.Zero,scale,SpriteEffects.None,0);
            
        }
        public void Update(GameTime gameTime) {
            if (scale == 0)
                delay += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (delay > 80) {
                if (scale < originalScale)
                    scale += 0.005f;
            }
            if (scale == originalScale)
                delay = 0;
            
        }
        public void Collision(PhysicsObjects.Player player,BreakParticleEngine breakPart)
        {
            if (player.col.Intersects(col)||player.col.TouchTop(col)&&scale>=originalScale) {
                breakPart.EmitterLocation = pos;
                breakPart.toggleEmmiter = true;
                if (player.punching) {
                    player.gacceleration = 0;
                    player.velocity.Y = 0;
                    player.velocity.Y -= 20;
                    player.curstate = PhysicsObjects.Player.PlayerState.Idle;
                    scale = 0;
                }
            }
        }
        
    }
}
