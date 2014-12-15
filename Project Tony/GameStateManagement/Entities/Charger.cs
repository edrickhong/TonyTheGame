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
   public class Charger : EnemyClass
    {
        public TimeSpan timer = TimeSpan.Zero;
        Vector2 dir;
        BladeEffectEngine blades;
       // List<Texture2D> textures = new List<Texture2D>();

        public Charger(Texture2D text,Vector2 vect,Player p,List<Texture2D>t):base(text,vect,p){
            lives = 5;
            color = Color.Green;
            bounce = false;
            blades = new BladeEffectEngine(t,this);
            blades.toggleEmmiter = false;
            tag = 1;
        }
        public override void Update(GameTime gameTime)
        {
            // base.Update(gameTime);
            blades.Pangle = this.rotation;
            if (size >= 1)
                Behaviour(gameTime);
            else
                size += growth;
            blades.Update();
            
        }
        public override void Reset()
        {
            base.Reset();
            charging = false;
            timer = TimeSpan.Zero;
            move = true;
            blades.toggleEmmiter = false;
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            blades.Draw(spritebatch);
            base.Draw(spritebatch);
            
        }

        public void Behaviour(GameTime gameTime)
        {
            //   System.Diagnostics.Debug.WriteLine(blades.toggleEmmiter);
            if (move)
            {
                base.Behaviour();
                //   System.Diagnostics.Debug.WriteLine("C is moving");
            }
            if (knockback)
                base.Behaviour();
            else
            {
                if (Vector2.Distance(pos, playerPos.pos) < 150)
                {
                    // System.Diagnostics.Debug.WriteLine("C is in range");
                    move = false;
                    charging = true;
                    //add code to rotate to pos, puase rot and double speed forward 
                }

                if (charging)
                {
                    //System.Diagnostics.Debug.WriteLine("C is aiming");
                    timer += gameTime.ElapsedGameTime;
                    if (isrotating)
                        dir = playerPos.pos - pos;
                }

                if (timer.Seconds >= 6f)
                {
                    //System.Diagnostics.Debug.WriteLine("C reset");
                    Reset();
                }
                else if (timer.Seconds >= 5f)
                {
                    //System.Diagnostics.Debug.WriteLine("C is charging");
                    dir.Normalize();
                    pos += dir * 15;
                    isrotating = false;
                    blades.toggleEmmiter = true;
                }
                if (timer.Seconds == 0)
                    isrotating = true;
            }
        }
    }
}
