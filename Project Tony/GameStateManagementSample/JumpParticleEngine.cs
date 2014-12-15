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
    class JumpParticleEngine
    {
        private Random random;
                public Vector2 EmitterLocation { get; set; }
                private List<SmokeParticles> particles;
                private List<Texture2D> textures;
                public bool toggleEmmiter=false;
                public int count=1;
                public PhysicsObjects.Player player;

                public JumpParticleEngine(List<Texture2D> textures, PhysicsObjects.Player loc)
                {
                    EmitterLocation = loc.pos;
                    this.textures = textures;
                    this.particles = new List<SmokeParticles>();
                    random = new Random();
                    player = loc;
                }

                private SmokeParticles GenerateNewParticle()
                {
                    //Behaviour of all particles. Make note to mess around with opacity and fading effects

                    Texture2D texture = textures[random.Next(textures.Count)];
                    Vector2 position = EmitterLocation;
                    Vector2 velocity = new Vector2(
                            1f * (float)(random.NextDouble() * 2 - 1),
                            1f * (float)(random.NextDouble() * 2 - 1));
                    velocity.X *=(float) -random.NextDouble()*7;
                    velocity.Y += 0.8f;
                    
                    float angle =(float) random.NextDouble();
                    float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                    Color color = new Color(
                            (float)random.NextDouble(),
                            (float)random.NextDouble(),
                            (float)random.NextDouble());
                    float size = (float)random.NextDouble();
                    int ttl = 5 + random.Next(0,15);
                    int increment=-60;
                    double fadedelay=3;


                    return new SmokeParticles(texture, position, velocity, angle, angularVelocity, Color.White, size*0.1f, ttl,increment,fadedelay);
                }

                public void Update()
                {
                    count = particles.Count;
                    EmitterLocation = new Vector2(player.col.Location.X+(player.col.Width/2),player.col.Location.Y+player.col.Height);
                    int total = 4; //No of particles to generate each turn
                    

                    if (toggleEmmiter)
                    {
                        for (int i = 0; i < total; i++)
                        {
                            particles.Add(GenerateNewParticle());
                        }
                    }

                    if(particles.Count>8){
                        toggleEmmiter = false;
                    }
                    for (int particle = 0; particle < particles.Count; particle++)
                    {
                        particles[particle].Update();

                        if (particles[particle].fade)
                        {
                            if (particles[particle].AlphaValue <= 0)
                            {
                                particles.RemoveAt(particle);
                                particle--;
                            }
                        }
                        else {
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
                    for (int index = 0; index < particles.Count; index++)
                    {
                        particles[index].Draw(spriteBatch);
                    }
                    // spriteBatch.End();
                }
    }
}
