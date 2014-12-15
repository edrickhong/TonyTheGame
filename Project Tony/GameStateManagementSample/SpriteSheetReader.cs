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
    public class SpriteSheetReader
    {
        //this class is used to convert frame numbers to the spritesheet frame positions eg: No:0 = pos:(0,0)
        //for the sake of convenience, a method to handle animations playing/speed/loop/etc will be added here as well

        int frameWidth, frameHeight;
        Texture2D texture;
        public string animationName = "";
        public float delay;
        public int[] currAnimation; // the current animation
        public int frameX, frameY;
        public int frameIndex;//frameIndex
        Rectangle source;
        public bool looping = true;
        float elapsed;//time elapsed
        int lastframeX;//Max Frame No X 
        int lastframeY;
        public bool pause = false;

        public SpriteSheetReader(Texture2D texture,int framewidth,int frameheight,int lengthX,int lengthY) {
            this.texture = texture;
            frameWidth = framewidth;
            frameHeight = frameheight;
            lastframeX = lengthX;
            lastframeY = lengthY;
            elapsed = 0;
        }

        public void setAnimation(int[] animation,float delay,bool loop){
            currAnimation=animation;
            looping=loop;
            this.delay = delay;
        }

        public void Update(GameTime gameTime)
        {
            if(!pause)
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            framepos(currAnimation[frameIndex]);
            PlayAnimation();

            source = new Rectangle(frameWidth * frameX, frameHeight * frameY, frameWidth, frameHeight);
        }
        public void Draw(SpriteBatch spritebatch,Vector2 pos,Color color,float rotation,Vector2 origin,float scale)
        {
            spritebatch.Draw(texture, pos, source, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }
        

        void framepos(int frameNo)
        {
            
            int frame = frameNo;
            frameX = 0;
            frameY = 0;
            for (int i = 0; i <= lastframeY; i++)
            {
                if (frame <= lastframeX)
                {
                    frameX = frame;
                    frameY = i;
                    break;
                }
                else
                {
                    frame -= lastframeX;
                    frame--;
                }
            }

        }
        void PlayAnimation()
        {
            if (looping && frameIndex == currAnimation.Length - 1)
            {
                frameIndex = 0;
            }
            if (elapsed > delay&&!pause)
            {
                if (frameIndex < currAnimation.Length - 1)
                    frameIndex++;
                elapsed = 0;
            }

        }
    }
}
