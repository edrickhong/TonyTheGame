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
    public class Blood
    {
        protected Random random;
        public Vector2 EmitterLocation { get; set; }
        protected List<BloodParticles> particles;
        protected List<Texture2D> textures;
        protected Texture2D splatter;
        protected List<Texture2D> splatterList;
        protected Color[] splatterColorArray;
        protected Color[] tileColorArray;
        public bool toggle=false;
        int offsetTile;
        int offsetSplatter;
        Texture2D rec;
        public Blood(List<Texture2D> textures, Vector2 location,Texture2D splatter,Texture2D rec)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<BloodParticles>();
            random = new Random();
            this.splatter = splatter;
            splatterColorArray = new Color[splatter.Width*splatter.Height];
            splatter.GetData<Color>(splatterColorArray);
            this.rec = rec;
            splatterList = new List<Texture2D>();
        }

        public virtual  BloodParticles GenerateNewParticle()
        {
            //Behaviour of all particles. Make note to mess around with opacity and fading effects

            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1),
                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
            float size = random.Next(0, 100);
            size = size / 10000;
            int ttl = 20 + random.Next(40);
            velocity.X *= 5;
            velocity.Y *= 5;

            return new BloodParticles(texture, position, velocity, angle, angularVelocity, Color.White,size, 1000,rec);
        }
        public void Collision(PhysicsObjects.Tiles tile,GraphicsDevice device)
        { 
            for(int i=0;i<particles.Count;i++)
            if(particles[i].col.Intersects(tile.rectangle)){
                    particles[i].delete=true;
                //get tile data
                tileColorArray = new Color[tile.texture.Width*tile.texture.Height];
                tile.texture.GetData<Color>(tileColorArray);
                //create new texture
                Texture2D newText = new Texture2D(device,tile.texture.Width,tile.texture.Height);
                //set Blood offset according to touch pos
                if(particles[i].col.TouchTop(tile.rectangle)){
                    offsetTile = 4500;
                    offsetSplatter = 8000;
                }
                if (particles[i].col.TouchBottom(tile.rectangle))
                {
                    offsetTile = 111520;
                    offsetSplatter = 0;
                }

                if (particles[i].col.TouchLeft(tile.rectangle))
                {
                    offsetTile = 29550;
                    offsetSplatter = 0;
                }
                if (particles[i].col.TouchRight(tile.rectangle))
                {
                    offsetTile = 200;
                    offsetSplatter = 200;
                }
                //Add blood to new texture
                newText.SetData<Color>(AddBlood(splatterColorArray, tileColorArray, offsetTile, offsetSplatter));
                
                //Set tile to new texture
                tile.texture = newText;
            }
        }
        public Color[] AddBlood(Color[] splatterarray,Color[]tilearray,int offsetTile,int offsetSplatter) {
            for (int i = 0; i < tilearray.Count(); i++)
            {
                if (i < splatterarray.Count())
                {
                    if (splatterarray[i].A > 0 && tilearray[i].A > 0 && i + offsetTile < tilearray.Count())
                    {
                        tilearray[i + offsetTile] = splatterarray[i];
                    }
                }
            }
            return tilearray;
           
        }
        public virtual void Update()
        {
            int total = 2;
            if (toggle)
            {
                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
            }

            if (particles.Count() >= 20)
                toggle = false;

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();

                if (particles[particle].delete||particles[particle].TTL<0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
                
                }
                
            }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }


        }



        

    }

