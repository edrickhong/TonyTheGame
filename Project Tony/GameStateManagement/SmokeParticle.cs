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
    class SmokeParticle
    {
        public Texture2D Texture { get; set; }        // The texture that will be drawn to represent the particle
        public Vector2 Position { get; set; }        // The current position of the particle        
        public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
        public float Angle { get; set; }            // The current angle of rotation of the particle
        public float AngularVelocity { get; set; }    // The speed that the angle is changing
        public Color Color { get; set; }            // The color of the particle
        public float Size { get; set; }                // The size of the particle
        public int TTL { get; set; }                // The 'time to live' of the particle
        public int AlphaValue=255;  //Base alphaValue
        public int FadeIncrement; //controls fadeIn/Out
        public double FadeDelay; //rate of fade
        public bool fade = false; //Whether to implement fade

        double fadeTimer; //Timer for rate of fade

        public SmokeParticle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl)
        {

            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;


        }

        public SmokeParticle(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int ttl,int fadein,double fadeDe)
        {

            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            FadeIncrement = fadein;
            FadeDelay = fadeDe;
            fadeTimer = FadeDelay;
            fade = true;

        }

        public void Update()
        {
            Position += Velocity;
            Angle += AngularVelocity;
            if (fade)
            {
                fadeTimer--;
                if (fadeTimer <= 0)
                {
                    fadeTimer = FadeDelay;
                    if (AlphaValue > 0 || AlphaValue < 255)
                        AlphaValue += FadeIncrement;
                }
            }
            else {
                TTL--;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            if (fade) {
                spriteBatch.Draw(Texture, Position, sourceRectangle, new Color(Color.R,Color.G,Color.B,(byte)MathHelper.Clamp(AlphaValue,0,255)),
                    Angle, origin, Size, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                    Angle, origin, Size, SpriteEffects.None, 0f);
            }
        }

    }


}
