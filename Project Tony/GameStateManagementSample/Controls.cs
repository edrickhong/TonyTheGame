using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.GamerServices;
using GameStateManagement;

namespace GameStateManagementSample
{
    class Controls : MenuScreen
    {
        ContentManager content;
        Texture2D controller;
        GamePadState gamepad;
        Game game;
        Color color;
        float alpha = 0;
        float rate = 0.1f;
        public Controls(Game game) :base("Controls") {
            content =game.Content;
            this.game = game;
            controller = content.Load<Texture2D>("Controls");
            
           // spritebatch = ScreenManager.SpriteBatch;
            color = Color.White;
            
        }

        public override void Draw(GameTime gameTime)
        {
           // base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(controller,new Vector2(200,15),new Color(color.R,color.G,color.B,MathHelper.Clamp(alpha,0,255)));
            spriteBatch.End();
            
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (gamepad.IsButtonDown(Buttons.B))
                alpha =0;
            base.HandleInput(gameTime, input);
            

            alpha +=rate;
        }
    }
}
