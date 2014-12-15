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
using GameStateManagementSample;

namespace GameStateManagementSample
{
    public class BladeEffectEngine
    {

         protected Random random;
        public Vector2 EmitterLocation { get; set; }
        public List<BladeParticle> particles;
        public List<Texture2D> textures;
        Charger c;
        public bool toggleEmmiter=true;
        public int count=1;
        public float Pangle;
        protected int total;

        public BladeEffectEngine(List<Texture2D> textures,Charger r)
        {
            c = r;
            EmitterLocation = r.pos;
            this.textures = textures;
            this.particles = new List<BladeParticle>();
            random = new Random();
            total = 3; //No of particles to generate each turn
        }

        public virtual BladeParticle GenerateNewParticle()
        {
            //Behaviour of all particles. Make note to mess around with opacity and fading effects

            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1),
                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = Pangle;
             float angularVelocity = 0f;
            Color color = Color.White;
            float size = 1;
            int ttl = 0 ;
            float growth = 0.07f;


            return new BladeParticle(texture, position, velocity, angle, angularVelocity, color, size, ttl,growth);
        }

        public virtual void Update()
        {
            count = particles.Count;
            EmitterLocation = c.pos;
            

            if (toggleEmmiter)
            {
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
            }
            

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();

                if (particles[particle].Size <= 0.1)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                    }
                
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }

    }
    }

