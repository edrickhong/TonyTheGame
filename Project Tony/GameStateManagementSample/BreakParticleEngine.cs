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
    class BreakParticleEngine
    {
        
        private Random random;
                public Vector2 EmitterLocation { get; set; }
                private List<ParticleCustom> particles;
                private List<Texture2D> textures;
                public bool toggleEmmiter=false;
                public int count=1;

                public BreakParticleEngine(List<Texture2D> textures, Vector2 loc)
                {
                    EmitterLocation = loc;
                    this.textures = textures;
                    this.particles = new List<ParticleCustom>();
                    random = new Random();
                }

                private ParticleCustom GenerateNewParticle()
                {
                    //Behaviour of all particles. Make note to mess around with opacity and fading effects
                    int randomIndex=random.Next(textures.Count);
                    Texture2D texture = textures[randomIndex];
                    Vector2 position = EmitterLocation;
                    Vector2 velocity = new Vector2(
                            1f * (float)(random.NextDouble() * 2 - 1),
                            1f * (float)(random.NextDouble() * 2 - 1));
                    velocity.X *= 5;
                    velocity.Y *= 5;
                    
                    float angle =(float) random.NextDouble();
                    float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                    Color color = new Color(
                            (float)random.NextDouble(),
                            (float)random.NextDouble(),
                            (float)random.NextDouble());
                    float size = (float)random.NextDouble();
                    if(randomIndex==4||randomIndex==0){
                        size *= 0.1f;
                    }
                    int ttl = 5 + random.Next(0,15);


                    return new ParticleCustom(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl);
                }

                public void Update()
                {
                    count = particles.Count;
                    
                    int total = 10; //No of particles to generate each turn
                    

                    if (toggleEmmiter)
                    {
                        for (int i = 0; i < total; i++)
                        {
                            particles.Add(GenerateNewParticle());
                        }
                    }

                    if (particles.Count > 30)
                    {
                        toggleEmmiter = false;
                    }
                    for (int particle = 0; particle < particles.Count; particle++)
                    {
                        particles[particle].Update();

                       
                       
                            if (particles[particle].TTL <= 0)
                            {
                                particles.RemoveAt(particle);
                                particle--;
                            }
                        
                    }
                }



                public void Draw(SpriteBatch spriteBatch)
                {
                    //spriteBatch.Begin();
                    for (int index = 0; index < particles.Count; index++)
                    {
                        particles[index].Draw(spriteBatch);
                    }
                    // spriteBatch.End();
                }
    }
}
