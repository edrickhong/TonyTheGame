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
    
    class Player : PhysicsEntity
    {
        //Ribbon
        RibbonTrail ribbon;

        SlidingSmokeParticleEngine slidingEmitter;
        List<Texture2D> smokeList;
        Texture2D smokeText;
        //
        //Jumping/landing dust
        JumpParticleEngine Dust;
        
        //
        int frameWidth=150,frameHeight=110;
        public bool punching;
        bool doubleJump = true;
        public bool gameOver = false;
        //controls which animations to play
        public enum PlayerState
        {
            Idle,
            Jumping,
            Moving,
            Punching,
            Sliding
        }
        float smash = 20;
        //
        Texture2D recttext;
        public bool grounded;
        public string msg = "nothing";
        //
        public bool sliding = false;
        bool deny=false;//doesn't allow other states to be played. eg:jumping
        GamePadState prevState;
        public string animationName="";
        float delay;
        float speed = 5;
        float jumpheight = 10;
        public bool facingRight = true;
        public PlayerState curstate = PlayerState.Idle;//sets start state as idle
        PlayerState prevPlayerState;
        int[] currAnimation; // the current animation
        public int frameX, frameY;
        int frameIndex;//frameIndex
        Rectangle source;
        float elapsed;//time elapsed
        //All the animations
        int[] jumpingR={21,22,23,24,28,29,30,31,35,36,37};
        int[] movingR = { 47, 48, 54, 55, 54, 48, 47 };//
        int[] idleR = {45,46,52,53,53,52,46,45 };//
        int[] jumpingL = { 3, 2, 1, 0, 10, 9, 8, 7, 17, 16, 15 };
        int[] movingL={60,59,67,66,67,59,60};//
        int[] idleL = { 33, 32, 40, 39, 39, 40, 32,33 };//
        int[] neutralR = {45};
        int[] neutralL = {33};
        int[] punchingR={42,42,44,49};
        int[] punchingL={6,5,4,13};
        int[] slidingR={63};
        int[] slidingL={18};
        bool looping = true;
        PlayerIndex playerIndex;

        //animation player has already has its own class. just too lazy to make changes
       //animation class territory
        void PlayAnimation() {
            if (looping && frameIndex == currAnimation.Length - 1)
            {
                frameIndex = 0;
            }
            if (elapsed > delay)
            {
                if(frameIndex<currAnimation.Length-1)
                frameIndex++;
                elapsed = 0;
            }
            
        }
        void framepos(int frameNo) {
            int lastframeX = 6;
            int frame = frameNo;
            frameX = 0;
            frameY = 0;
            for (int i = 0; i <=9;i++ ) {
                if (frame <= lastframeX)
                {
                    frameX = frame;
                    frameY = i;
                    break;
                }
                else {
                    frame -= lastframeX;
                    frame--;
                }
            }

        }
        //animation class territory
        public Player(Texture2D t,Vector2 p,float s,float w,bool b,Texture2D txt,Texture2D smoke,Texture2D tail):base(t,p,s,w,b){


            //player tag. ignore tags. was going to use a tag system, but didnt. oh well
            tag = 1;
            frameIndex = 0;
            currAnimation = idleR;
            prevPlayerState = curstate;
            playerIndex=PlayerIndex.One;
            prevState = GamePad.GetState(playerIndex,GamePadDeadZone.IndependentAxes);
            recttext = txt;
            smokeText = smoke;
            smokeList = new List<Texture2D>();
            smokeList.Add(smoke);
            slidingEmitter = new SlidingSmokeParticleEngine(smokeList,this);
            Dust = new JumpParticleEngine(smokeList,this);

            ribbon = new RibbonTrail(tail, 50, 30000, 600, this);
            
        }
        //handles animation state transition and what happens for each state
        void HandleState() {
            //while falling, play frame 0
            if (!grounded && curstate != PlayerState.Jumping && curstate != PlayerState.Punching)
                frameIndex = 0;


            if (prevPlayerState != curstate)
            {
                frameIndex = 0;
            }

            if (curstate == PlayerState.Sliding) {
                    
                if(!sliding){

                    gravityEnabled = true;
                    facingRight = !facingRight;
                    curstate = PlayerState.Idle;
                }
                if (facingRight)
                    currAnimation = slidingR;
                else
                    currAnimation = slidingL;
            }

            if (curstate == PlayerState.Punching)
            {
                punching = true;
                delay = 60;
                if (grounded)
                    curstate = PlayerState.Idle;
                if (frameIndex <= 1)
                    gravityEnabled = false;
                else
                {
                    gravityEnabled = true;
                    if (frameIndex == 3)
                        velocity.Y = smash;
                }
                looping = false;

                if (facingRight)
                    currAnimation = punchingR;
                else
                    currAnimation = punchingL;

            }
            else
                punching = false;

            if (curstate == PlayerState.Idle)
            {
                delay = 100;
                looping = true;
                
                if (facingRight)
                {
                    currAnimation = idleR;
                    animationName = "idleR";
                }
                else { currAnimation = idleL; animationName = "idleL"; }
            }
            if (curstate == PlayerState.Moving)
            {

                delay = 50;
                looping = true;
                if (facingRight)
                {
                    currAnimation = movingR; animationName = "movingR";
                }
                else { currAnimation = movingL; animationName = "movingL"; }
            }
            if (curstate == PlayerState.Jumping)
            {
                looping = false;
                delay = 16;
                if (facingRight)
                {
                    currAnimation = jumpingR; animationName = "jumpingR";
                }
                else { currAnimation = jumpingL; animationName = "jumpingL"; }

               
                if (frameIndex == currAnimation.Length - 1&&grounded)
                {
                    deny = false;

                }
            }
            //System.Diagnostics.Debug.WriteLine(animationName);
            prevPlayerState = curstate;
        }
        public override void Update(GameTime gameTime)
        {
            //update the particle systems
            ribbon.Update(gameTime);
            slidingEmitter.Update();
            Dust.Update();
            //bounces player at this frame when jumping
            if (frameIndex == 8 &&curstate==PlayerState.Jumping)
            {
                gacceleration = 0;
                velocity.Y = -jumpheight;
                
            }
            if(curstate==PlayerState.Jumping&&frameIndex==2&&grounded)
                Dust.toggleEmmiter = true;
            if (sliding)
                curstate = PlayerState.Sliding;
            //adds force to player
            AddForce(Force);
            //move according to velocity
            pos += velocity;
            //allow double jump and switch off gravity when grounded
            if (grounded)
            {
                gravityEnabled = false;
                doubleJump = true;
            }
            else if(!sliding)
                gravityEnabled = true;
            //collision box
            col = new Rectangle((int)(pos.X+CxOffset), (int)(pos.Y+CyOffset), 50, 45);

            base.Update(gameTime);
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            HandleState();
            framepos(currAnimation[frameIndex]);
            
            PlayAnimation();

            source = new Rectangle(frameWidth * frameX, frameHeight * frameY, frameWidth, frameHeight);
            Force = Vector2.Zero;
        }
        public override void Draw(SpriteBatch spritebatch)
        {//draw particle systems and player
            if(curstate!=PlayerState.Sliding&&!grounded)
            ribbon.Draw(spritebatch);
            Dust.Draw(spritebatch);
            slidingEmitter.Draw(spritebatch);
            spritebatch.Draw(texture, pos, source, color, rotation, new Vector2(0,0), scale, SpriteEffects.None, 0f);
            
            
        }

        
        public void HandleInput(GameTime gameTime, InputState input, GamePadState gamePadState)
        {
            Vector2 dir = gamePadState.ThumbSticks.Left;
            //handles the col box offset when changing directions(left/right)
            if (facingRight)
            {
                CxOffset = 40;
            }
            else
            {
                CxOffset = 18;
            }
            //if no input and grounded, set velocity x to zero. stops player from sliding on ground(acts like ice floor w/o it)
            if (dir.X == 0&&grounded)
            velocity.X = 0;
            else
            {//moves player
                if (!restrictX && curstate != PlayerState.Punching && curstate != PlayerState.Sliding)
                    velocity.X += dir.X * speed;
            }
            //jumping
            if (gamePadState.IsButtonDown(Buttons.A))
            {
                if(!prevState.IsButtonDown(Buttons.A)){
                    if (sliding)
                    {//jump for sliding
                        curstate = PlayerState.Jumping;
                        frameIndex = 9;
                        deny = true;
                        if (facingRight)
                        {
                            //velocity = AngleToVector(3.83972435f);
                            //velocity.X=-10;
                            velocity += new Vector2(-20,-8);
                        }
                        else
                        {
                            //velocity = AngleToVector(5.58505361f);
                           // velocity.X = 10;
                            velocity += new Vector2(20, -8);
                        }
                        facingRight = !facingRight;
                        sliding = false;
                    }
                    else if(doubleJump)
                    {
                        frameIndex = 0;
                        deny = true;
                        curstate = PlayerState.Jumping;
                        if (!grounded)
                            doubleJump = false;
                    }

                }
                
                
            }
            else if (sliding) {//slows down descent while sliding
                gravityEnabled = false;
                velocity.Y = 2;
            }
            //punching
            if(gamePadState.IsButtonDown(Buttons.X)&&curstate!=PlayerState.Punching){
                curstate = PlayerState.Punching;
                velocity.Y = 0;

            }//sets to idle animation
            if (grounded && !deny && !gravityEnabled && dir.X == 0)
                curstate = PlayerState.Idle;
            //transitions to diff movement animation
            else if (dir.X != 0)
            {
                if (grounded&&!deny&&!gravityEnabled)
                {
                    curstate = PlayerState.Moving;
                }
                if (velocity.X > 0)
                {
                    facingRight = true;
                }
                else if (velocity.X < 0)
                {
                    facingRight = false;
                }
                
            }
                
            
            prevState = gamePadState;
            restrictX = false;
        }
    }
}
