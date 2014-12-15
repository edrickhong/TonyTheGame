//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
//using GameStateManagement;

//namespace GameStateManagementSample
//{
//    public class Ranger : EnemyClass
//    {
//        TimeSpan timer = TimeSpan.Zero;
//        int counter=0;
//        Random ran = new Random();
//        float ranTime;

//        public Ranger(Texture2D text, Vector2 vect, Player p) : base(text, vect, p) {
//            lives = 4;
//            color = Color.CadetBlue;
//            ranTime=(3f + ran.Next(0, 3));
//            bounce = true;
//            speed = 8;
//            tag = 2;
//        }

//        public void Behavior(GameTime gameTime) {
//            if(move)
//            base.Behaviour();

//            if (knockback)
//                base.Behaviour();
//            else
//            {
//                if (Vector2.Distance(playerPos.pos, pos) <= (200 + ran.Next(0, 30)))
//                {
//                    move = false;
//                    orbiting = false;
//                    timer += gameTime.ElapsedGameTime;
//                }
//                if (timer.Seconds <= ranTime && !move)
//                {
//                    //  System.Diagnostics.Debug.WriteLine("Orbiting");
//                    //orbit phase
//                    pos = Vector2.Transform(pos - playerPos.pos, Matrix.CreateRotationZ(rotdir)) + playerPos.pos;
//                    orbiting = true;
//                }
//                if (counter == 1)
//                {
//                    fire = false;
//                    orbiting = false;

//                }
//                if (timer.Seconds == ranTime && counter < 1)
//                {
//                    //  System.Diagnostics.Debug.WriteLine("shot fired");
//                    fire = true;
//                    counter++;
//                    System.Diagnostics.Debug.WriteLine("hit");

//                    run = -direction;
//                    run.X += ran.Next(-50, 50);
//                    run.Y += ran.Next(-50, 50);
//                    run.Normalize();

//                }
//                if (timer.Seconds >= 9f)
//                {
//                    //reset
//                    //  System.Diagnostics.Debug.WriteLine("reset");
//                    timer = TimeSpan.Zero;
//                    move = true;
//                    counter = 0;
//                }
//                if (timer.Seconds > ranTime)
//                {
//                    //run
//                    timer += gameTime.ElapsedGameTime;
//                    // System.Diagnostics.Debug.WriteLine("running:"+run);

//                    pos += run * speed;
//                }
//            }


//        }
//        public override void Update(GameTime gameTime)
//        {
//            direction = playerPos.pos - pos;
//            if (size >= 1)
//                Behavior(gameTime);
//            else
//                size += growth;
           
//        }
//    }
//}
