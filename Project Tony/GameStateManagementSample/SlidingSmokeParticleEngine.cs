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
    class SlidingSmokeParticleEngine
    {
        private Random random;
                public Vector2 EmitterLocation { get; set; }
                private List<SmokeParticles> particles;
                private List<Texture2D> textures;
                public bool toggleEmmiter=true;
                public int count=1;
                public PhysicsObjects.Player player;
                public SlidingSmokeParticleEngine(List<Texture2D> textures, PhysicsObjects.Player loc)
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
                    velocity.Y -= 4;
                    velocity.Normalize();
                    //velocity.X *= 2 * random.Next(-2,2)*2;
                    velocity.X = 0;
                    velocity.Y *= 2;
                    float angle =(float) random.NextDouble();
                    float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
                    Color color = new Color(
                            (float)random.NextDouble(),
                            (float)random.NextDouble(),
                            (float)random.NextDouble());
                    float size = (float)random.NextDouble();
                    int ttl = 5 + random.Next(0,15);
                    int increment=-30;
                    double fadedelay=2;


                    return new SmokeParticles(texture, position, velocity, angle, angularVelocity, Color.White, size*0.1f, ttl,increment,fadedelay);
                }

                public void Update()
                {
                    count = particles.Count;
                    if (player.facingRight){
                        EmitterLocation = new Vector2(player.pos.X+player.col.Width+50,player.pos.Y+(player.col.Height/2)+10);
                    }
                        
                    else{
                        EmitterLocation = new Vector2(player.pos.X+10 , player.pos.Y + (player.col.Height / 2)+20);
                    };
                    int total = 1; //No of particles to generate each turn
                    if (player.sliding)
                        toggleEmmiter = true;
                    else
                        toggleEmmiter=false;

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
