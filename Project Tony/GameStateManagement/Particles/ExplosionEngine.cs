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
    public class ExplosionEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<ExplosionParticle> particles;
        private List<Texture2D> textures;
        //Rocket rocket;
        public bool toggleEmmiter = true;
        public int count = 1;
        public Texture2D boomtext;

        //Animating explosion
        int frameX=0, frameY=0;
        Rectangle source;
        float delay=27f;
        float elapsed = 0;
        bool toggleAnimate = true;

        //fade texture
        public int AlphaValue=255;
        int fadeIncrement=-50;
        double fadeDelay=2.5;
        double fadeTimer;
 
        public ExplosionEngine(List<Texture2D> textures,Vector2 loc,Texture2D explosiontext)
        {
           // rocket = r;
            EmitterLocation = loc;
            this.textures = textures;
            this.particles = new List<ExplosionParticle>();
            random = new Random();
            boomtext = explosiontext;
            fadeTimer = fadeDelay;

           // particles.Add(GenerateBoom());
        }

        private ExplosionParticle GenerateNewParticle()
        {
            //Behaviour of all particles. Make note to mess around with opacity and fading effects

            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1)*3,
                    1f * (float)(random.NextDouble() * 2 - 1)*3);
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 5 + random.Next(0, 15);
            int increment = -100;
            double fadedelay = 5;


            return new ExplosionParticle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl, increment, fadedelay);
        }


        private ExplosionParticle GenerateBoom()
        {
            //Behaviour of all particles. Make note to mess around with opacity and fading effects

            Texture2D texture = boomtext;
            Vector2 position = EmitterLocation;
            Vector2 velocity = Vector2.Zero;
            float angle = 0;
            float angularVelocity = 0f;
            Color color = Color.White;
            float size = (float)random.NextDouble();
            int ttl = 5 + random.Next(0, 15);
            int increment = -90;
            double fadedelay = random.Next(2,7);


            return new ExplosionParticle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl, increment, fadedelay);
        }

        public void Update(GameTime gameTime)
        {
            if (toggleAnimate)
                source = new Rectangle(99 * frameX, 72 * frameY, 99, 72);
            else
            {
                source = new Rectangle(99, 72 * 2, 99, 72);
                fadeTimer--;
                if(fadeTimer<=0){
                    fadeTimer = fadeDelay;
                    if (AlphaValue > 0 || AlphaValue < 255)
                        AlphaValue += fadeIncrement;
                }
                
            }
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(elapsed>=delay && toggleAnimate){
                if(frameX==1&& frameY==2){
                    toggleAnimate = false;
                }
                if (frameX == 2)
                {
                    frameY++;
                    frameX = 0;
                }
                else
                    frameX++;

                elapsed = 0;
            }
            count = particles.Count;
            int total = 10; //No of particles to generate each turn

            if (toggleEmmiter)
            {
                if (count < total)
                {
                    for (int i = 0; i < total; i++)
                    {
                        particles.Add(GenerateNewParticle());
                    }
                }
                else
                    toggleEmmiter = false;
            }


            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(gameTime);

                if (particles[particle].fade)
                {
                    if (particles[particle].AlphaValue <= 0)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                    }
                }
                else
                {
                    if (particles[particle].TTL <= 0)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                    }
                }
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            
           

            spriteBatch.Draw(boomtext, new Rectangle((int)EmitterLocation.X - 50, (int)EmitterLocation.Y - 30, boomtext.Width / 3, boomtext.Height / 3), source, new Color(255, 255, 255, (byte)MathHelper.Clamp(AlphaValue, 0, 255)));
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            // spriteBatch.End();
        }

    }
}

