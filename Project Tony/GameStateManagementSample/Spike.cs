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
    class Spike
    {
        Texture2D text;
        Vector2 pos;
        Rectangle col;
        Texture2D rect;
        float scale=0.4f;
        float rotation;
        public Spike(Texture2D t,Vector2 p,Texture2D rect,float r) {
            this.rect = rect;
            text = t;
            pos = p;
            rotation = r;
            if(r>4)
            col = new Rectangle((int)(pos.X+(text.Width*scale)-50),(int)(pos.Y-(text.Height*scale)),(int)(text.Width*scale)-100,(int)(text.Height*scale));
            else
                col = new Rectangle((int)pos.X, (int)pos.Y + 150, (int)(text.Width * scale), (int)(text.Height * scale) - 150); 
        }

        public void Update(PhysicsObjects.Player player,Blood blood) {
            if(col.Intersects(player.col)&&!player.gameOver){
                player.gameOver = true;
                blood.EmitterLocation = new Vector2(player.col.Location.X + (player.col.Width / 2), player.col.Location.Y + (player.col.Height / 2));
                blood.toggle = true;
            }
        }

        public void Draw(SpriteBatch spritebatch) {

            spritebatch.Draw(text, pos, null, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
           // spritebatch.Draw(rect, col, Color.Red);
        }
        
    }
}
