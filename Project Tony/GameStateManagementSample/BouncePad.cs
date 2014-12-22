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
    class BouncePad
    {
        SpriteSheetReader spritesheet;
        Texture2D texture;
        Texture2D rectText;
        public Vector2 pos;
        public Vector2 bouncedirection;
        public float strength;
        float scale;
        Rectangle col;
        float rotation;
        int[] up = {4,3,2,1,0,1,2,3,4};
        public BouncePad(Texture2D t,Vector2 p,Vector2 direction,float strength,float s,float r,Texture2D rect) {
            texture = t;
            pos = p;
            bouncedirection = direction;
            this.strength = strength;
            scale = s;
            rotation = r;
            spritesheet = new SpriteSheetReader(texture,216,57,4,1);
            spritesheet.setAnimation(up,30,true);
            spritesheet.pause = true;
            col = new Rectangle((int)(pos.X),(int)pos.Y+13,(int)(216*scale),(int)(28*scale));
            rectText = rect;

        }
        public void Update(GameTime gameTime) {
            spritesheet.Update(gameTime);
            if (spritesheet.frameIndex == spritesheet.currAnimation.Length - 1)
                spritesheet.pause = true;
        }
        public void Collision(PhysicsObjects.PhysicsEntity body,Rectangle rect) {
            
                if (col.Intersects(rect))
                {
                    body.gacceleration = 0;
                    body.velocity.Y -= strength;
                    spritesheet.pause = false;
                }
            
        }
        public void Draw(SpriteBatch spritebatch){
            //spritebatch.Draw(texture, pos, null, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spritesheet.Draw(spritebatch,pos,Color.White,rotation,Vector2.Zero,scale);
            //spritebatch.Draw(rectText,col,Color.White);
        }
    }
}
