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
   public class CollisionParticle : ParticleCustom
    {
       public Rectangle col;
       
       public bool delete = false;
       public CollisionParticle(Texture2D texture, Vector2 position, Vector2 velocity,float angle, float angularVelocity, Color color, float size, int ttl) : base(texture,position,velocity,angle,angularVelocity,color,size,ttl) {
           
       }

       public override void Update()
       {
           base.Update();
           col = new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width * Size), (int)(Texture.Height * Size));

       }
    }
   
}
