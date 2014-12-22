#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
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
using System.IO;
using ChaseCameraSample;
#endregion

namespace GameStateManagementSample
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        //for creating and reading maps
        int[,] Level;


        Vector2 camPos;

        Color AlphaFade = Color.Black;
        float Alpha = 0;

        SpriteBatch batch;
        Vector2 playerStart=Vector2.Zero;
        string[] msg;

        //Spikes
        List<Crate> crateList;
        List<PhysicsObjects.Launcher> launchList;
        List<Spike> spikeList;
        List<BouncePad> bounceList;
        Rectangle col;
        Crate crate2;
        Texture2D flag;
        Texture2D tail;
        Texture2D blank;
        BreakParticleEngine Break;
        Texture2D stars;
        Texture2D spikes;
        Texture2D wreckage1, wreckage2, wreckage3;
        List<Texture2D> breakList;
        ContentManager content;
        Texture2D saw;
        Texture2D bounceText;
        Texture2D cratetext;
        Texture2D launchtext;
        float pauseAlpha;
        Point point=new Point(0,0);
        _2DCamera camera;
        InputAction pauseAction;
        SpriteFont spriteFont;
        Vector2 rectpos = Vector2.Zero;
        int screenwidth, screenheight;
        Texture2D rectText;
        Texture2D playertext;
        PhysicsObjects.Map map;
        PhysicsObjects.Player player1;
        PhysicsObjects.Launcher launcher;
        List<Texture2D> blood;
        Texture2D splatter;
        Texture2D blod;
        Blood particleengine;
        float timer=0;
        Random ran;
        int random;
        bool fading = false;
        Texture2D sky;
        Texture2D smokeText;

        Game game;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.game = game;
            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start},
                new Keys[] { Keys.Escape },
                true);
        }

        

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");
                //initialize death text
                msg = new string[]{
                "OMG! You killed Tony,you bastard!",
	"First try!",
	"Fatality!",
	"I regret nothing",
	"Goodbye cruel world",
	"You're not good at this,are you?",
	"Don't quit your day job",
	"Missed it by that much",
	"Tony?Tony...TOOONYYY!",
	"You ded",
	"New hi-score!",
	"Splat!",
	"You don't last very long",
	"Tony had a short life expectancy",
	"Good thing you have life insurance",
	"Nuuuu!",
	"As imagined by George R.R Martin",
	"RIP",
	"Wasted",
	"I leave all my possessions \n to my pet dog",
	"Death be a cruel mistress",
	"10 points for trying",
	"Don't forget to show Death \n your customer loyalty card",
	"Look before you leap",
 	"You receive \n the participation award",
    "Real women are completely round",
    "Real women have curves, \n 360 degrees to be exact",
    "Tony is a strong female character. \n Take that Ubisoft!",
    "More bug free than AC:Unity. \n Take that Ubisoft",
            };
                //load textures
                sky = content.Load<Texture2D>("Sky");
                blank = content.Load<Texture2D>("blank");
                
                tail = content.Load<Texture2D>("cursor");
                flag = content.Load<Texture2D>("Flag");
                PhysicsObjects.Tiles.content = content;
                camera = new _2DCamera(ScreenManager.GraphicsDevice.Viewport);
                map = new PhysicsObjects.Map();
                rectText = content.Load<Texture2D>("rectText");
                launchtext = content.Load<Texture2D>("LauncherSheet1");
                spriteFont = content.Load<SpriteFont>("gameFont");
                saw = content.Load<Texture2D>("Buzzsaw");
                bounceText = content.Load<Texture2D>("Bounce");
                cratetext = content.Load<Texture2D>("Crate");
                splatter = content.Load<Texture2D>("BloodSplatter");
                smokeText = content.Load<Texture2D>("Smoke");
                spikes = content.Load<Texture2D>("Spikes");
                wreckage1 = content.Load<Texture2D>("Wreckage1");
                wreckage2 = content.Load<Texture2D>("Wreckage2");
                wreckage3 = content.Load<Texture2D>("Wreckage3");
                stars = content.Load<Texture2D>("Star");
                breakList = new List<Texture2D>();
                breakList.Add(smokeText);
                breakList.Add(wreckage1);
                breakList.Add(wreckage2);
                breakList.Add(wreckage3);
                breakList.Add(stars);
                Break = new BreakParticleEngine(breakList,new Vector2(100,300));
                
                blood = new List<Texture2D>();


                blod = content.Load<Texture2D>("Blood");
                blood.Add(blod);
                particleengine = new Blood(blood,new Vector2(90,90),splatter,rectText);

                bounceList = new List<BouncePad>();
                crateList = new List<Crate>();
                launchList = new List<PhysicsObjects.Launcher>();

                spikeList = new List<Spike>();



                playertext = content.Load<Texture2D>("PlayerSprite");
                //playerStart = new Vector2(50, 1250);
                
                screenwidth = ScreenManager.GraphicsDevice.Viewport.Width;
                screenheight = ScreenManager.GraphicsDevice.Viewport.Height;
                //col is the collider for the flag which serves as the end point. PS: redo flag texture cos it sucks
                col = new Rectangle((int)1450, (int)0, (int)(flag.Width * 0.07), (int)(flag.Height * 0.07));

                //read map from file

                try
                {
                    ReadMap("LevelDemo");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("*****ErrorHere****");
                    System.Diagnostics.Debug.WriteLine(ex);
                    System.Diagnostics.Debug.WriteLine("*****ErrorHere****");
                }
                //Level creation starts here. comment once created and saved

                //generate the map from 2darray Level of tile size of 64
                map.Generate(Level, 64);
                player1 = new PhysicsObjects.Player(playertext, playerStart, 0.8f, 1, true, rectText, smokeText, tail);
               
                //Saves the level to a lvl file of the name.Only use for level creation
                //WriteMap("A");

               // TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap;
                // A real game would probably have more content than this sample, so
                // it would take longer to load. We simulate that by delaying for a
                // while, giving you a chance to admire the beautiful loading screen.
                Thread.Sleep(1000);

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }

#if WINDOWS_PHONE
            if (Microsoft.Phone.Shell.PhoneApplicationService.Current.State.ContainsKey("PlayerPosition"))
            {
                playerPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"];
                enemyPosition = (Vector2)Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"];
            }
#endif
        }


        public override void Deactivate()
        {
#if WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State["PlayerPosition"] = playerPosition;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State["EnemyPosition"] = enemyPosition;
#endif

            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();

#if WINDOWS_PHONE
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("PlayerPosition");
            Microsoft.Phone.Shell.PhoneApplicationService.Current.State.Remove("EnemyPosition");
#endif
        }


        #endregion


        void WriteMap(string filename) {
            //looks/create lvl file
            StreamWriter write = new StreamWriter("Content\\" + filename + ".lvl");
            //writes the size of the map(x by y)
            write.WriteLine(Level.GetLength(0).ToString());
            write.WriteLine(Level.GetLength(1).ToString());
            string map="";
            //write the map
            for(int y=0;y<Level.GetLength(1);y++){
                for (int x = 0; x < Level.GetLength(0); x++)
                {
                    map+=Level[x,y];

                }
                write.WriteLine(map);
                map = "";
            }
            //write bouncepads
            write.WriteLine(bounceList.Count.ToString());
            foreach (BouncePad b in bounceList)
            {
                //write pos
                write.WriteLine(b.pos.X.ToString());
                write.WriteLine(b.pos.Y.ToString());
                //write dir
                write.WriteLine(b.bouncedirection.X.ToString());
                write.WriteLine(b.bouncedirection.Y.ToString());
                //write strength
                write.WriteLine(b.strength);

            }

            //write Spikes
            write.WriteLine(spikeList.Count.ToString());
            foreach(Spike s in spikeList){
                write.WriteLine(s.pos.X.ToString());
                write.WriteLine(s.pos.Y.ToString());

                write.WriteLine(s.rotation);
            }
            //write crates
            write.WriteLine(crateList.Count.ToString());
            foreach (Crate c in crateList) {
                write.WriteLine(c.pos.X.ToString());
                write.WriteLine(c.pos.Y.ToString());
            }
            //write launcher
            write.WriteLine(launchList.Count.ToString());
            foreach(PhysicsObjects.Launcher l in launchList){
                write.WriteLine(l.pos.X.ToString());
                write.WriteLine(l.pos.Y.ToString());

                if (l.Right)
                {
                    write.WriteLine("1");
                }
                else {
                    write.WriteLine("0");
                }
            }
            //write Player
            write.WriteLine(playerStart.X.ToString());
            write.WriteLine(playerStart.Y.ToString());

            //forgot to add endpos. add next time
            write.Close();
        }
        void ReadMap(string filename) {
            //Read the tilemap from file
            int xSize=0, ySize=0;

            StreamReader reader = new StreamReader("Content\\" + filename+".lvl");
            xSize =int.Parse(reader.ReadLine());
            ySize = int.Parse(reader.ReadLine());
            System.Diagnostics.Debug.WriteLine(xSize+":"+ySize);
            Level = new int[xSize, ySize];

            for(int y=0;y<ySize;y++){
                char[] row =reader.ReadLine().ToCharArray();
                for (int x = 0; x < xSize; x++) {
                    //System.Diagnostics.Debug.Write(row[x]);
                    Level[x, y] = (int)Char.GetNumericValue(row[x]);
                    //System.Diagnostics.Debug.WriteLine("|"+Level[x,y]+"|");
                }

               // System.Diagnostics.Debug.WriteLine("");
            }
            //Debug level map 
            //for (int y = 0; y < Level.GetLength(1); y++)
            //{
            //    for (int x = 0; x < Level.GetLength(0); x++)
            //    {
            //        System.Diagnostics.Debug.Write(Level[x, y]);
            //    }
            //    System.Diagnostics.Debug.WriteLine("");
            //}

            //read bouncepads

            int bounceNo =Int32.Parse(reader.ReadLine());
           // System.Diagnostics.Debug.WriteLine("bounceNo:"+reader.ReadLine());
            for (int i = 0; i < bounceNo; i++)
            {
                float x = 0, y = 0;
                string input = "";
                input = reader.ReadLine();
                x = float.Parse(input);
                input = reader.ReadLine();
                y = float.Parse(input);
                Vector2 pos = new Vector2(x, y);

                input = reader.ReadLine();
                x = float.Parse(input);
                input = reader.ReadLine();
                y = float.Parse(input);
                Vector2 direction = new Vector2(x, y);

                float strength = float.Parse(reader.ReadLine());

                System.Diagnostics.Debug.WriteLine("Pos" + pos + "dir" + direction + "Strength" + strength);
                bounceList.Add(new BouncePad(bounceText, pos, direction, strength, 0.4f, 0, rectText));
            }

            //read Spikes
            int spikeNo =int.Parse(reader.ReadLine());

            for (int i = 0; i < spikeNo;i++ ) {
                float x = 0, y = 0;
                string input = "";
                input = reader.ReadLine();
                x = float.Parse(input);
                input = reader.ReadLine();
                y = float.Parse(input);
                Vector2 pos = new Vector2(x, y);

                float dir = float.Parse(reader.ReadLine());
                spikeList.Add(new Spike(spikes,pos, rectText, dir));
                
            }
            //read crates
            int crateNo =int.Parse(reader.ReadLine());
            for (int i = 0; i < crateNo;i++ ) {
                float x = 0, y = 0;
                string input = "";
                input = reader.ReadLine();
                x = float.Parse(input);
                input = reader.ReadLine();
                y = float.Parse(input);
                Vector2 pos = new Vector2(x, y);

                crateList.Add(new Crate(cratetext, pos, 1));
            }
            //read launcher
            int launchNo =int.Parse(reader.ReadLine());
            for (int i = 0; i < launchNo;i++ ) {
                float x = 0, y = 0;
                string input = "";
                input = reader.ReadLine();
                x = float.Parse(input);
                input = reader.ReadLine();
                y = float.Parse(input);
                Vector2 pos = new Vector2(x, y);
                int aBool = int.Parse(reader.ReadLine());
                bool isright = aBool==1;
                

                launchList.Add(new PhysicsObjects.Launcher(launchtext, pos, isright, saw));
                
            }

            //Read Player
            float X = 0, Y = 0;
            string Input = "";
            Input = reader.ReadLine();
            X = float.Parse(Input);
            Input = reader.ReadLine();
            Y = float.Parse(Input);
            Vector2 Pos = new Vector2(X, Y);

            playerStart = Pos;
            //forgot to add endpos. add next time
            reader.Close();
        }



        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                //if player reaches end, transition to main menu
                if(col.Intersects(player1.col)){
                    GameScreen[] screens = new GameScreen[] {new BackgroundScreen(),new MainMenuScreen(game) };

                    LoadingScreen.Load(ScreenManager, true, PlayerIndex.One,screens);
                }
                //if player dies, reset to start. start fading
                if (player1.gameOver)
                {
                    if(!fading)
                    Alpha = 0;

                    fading = true;
                    player1.pos = playerStart;
                    
                }
                else
                    camPos = player1.pos; //only set camera to the player after fading is fin
                //time for fade transition
                if(timer>5000){
                    player1.pos = playerStart;
                    timer = 0;
                    fading = false;
                    player1.gameOver = false;
                    camPos = player1.pos;

                    
                    
                }
                //fades the screen 
                if (fading)
                {
                    

                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    //fade out while in between these values
                    if (timer >= 1000 && timer < 4800)
                    {
                        Alpha += 0.01f;
                        fading = true;

                    }
                    else
                    {
                        //fade in
                        Alpha -= 0.01f;
                    }


                    
                }
                //update the blood particle engine
                particleengine.Update();

                //if blood particle engine is on reset player pos and generate ran message. reset player pos is done twice cos i thougt it would fix it. wrong
                if(particleengine.toggle){
                    player1.pos = playerStart;
                    ran = new Random();
                    random = ran.Next(0,msg.Length-1);
                    GamePad.SetVibration(PlayerIndex.One, 1000, 1000);
                    
                }
                else
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);
                //check fo collision w players. if offscreen, delete
                foreach(PhysicsObjects.Launcher l in launchList){
                    foreach(PhysicsObjects.Saw saw in l.saws){
                        saw.Collision(player1, particleengine);
                        if (saw.pos.X > map.width || saw.pos.X < -50)
                            saw.delete = true;
                        if (saw.pos.Y > map.height || saw.pos.Y < -50)
                            saw.delete = true;
                    }
                }
                //checks for col with spikes. note, particle engine is switched on whenever player dies
                //engine automatically switches off after generating x number of particles
                foreach(Spike s in spikeList){
                    s.Update(player1,particleengine);
                }
                //check for col between saws and bouncepads
                foreach(PhysicsObjects.Launcher l in launchList){
                    foreach(PhysicsObjects.Saw s in l.saws){
                        foreach(BouncePad b in bounceList){
                            b.Collision(s, s.col);
                        }
                    }
                }
                //updates the particle system for the crate breaking effect
                 Break.Update();

                foreach(PhysicsObjects.Launcher l in launchList){
                    l.Update(gameTime);
                }
                if (player1.pos.X <= 0+5)
                    player1.pos.X = 0 + 5;


               
                for (int i = 0; i < map.tiles.Count - 1; i++)
                {
                    player1.SpriteCollision(map.tiles[i].rectangle, map.width, map.height);
                    particleengine.Collision(map.tiles[i],ScreenManager.GraphicsDevice);

//check collision between saws and the tiles
                    foreach (PhysicsObjects.Launcher l in launchList)
                    {
                        foreach (PhysicsObjects.Saw saw in l.saws)
                        {
                            saw.BounceCollision(map.tiles[i].rectangle, map.width, map.height);
                        }
                    }
                }
                
                foreach(Crate c in crateList){
                    //update crate
                    c.Update(gameTime);
                    //checks for col between player and crates if not slamming
                    if (!player1.punching && c.scale >= c.originalScale)
                        player1.SpriteCollision(c.col, map.width, map.height);
                    else
                        c.Collision(player1, Break);//else if punching,switch on break particle and bounce player
                }
                //moves the camera according to cam pos
                camera.Update(camPos, map.width, map.height);
                //checks if the player is grounded
                for (int i = 0; i < map.tiles.Count - 1; i++)
                {
                    point = new Point((int)player1.col.Location.X + (player1.col.Width/2), (int)(player1.pos.Y + player1.col.Height + 10));
                    player1.grounded = map.tiles[i].rectangle.Contains(point.X, point.Y);
                    if (player1.grounded)
                        break;
                    foreach(Crate c in crateList){
                        player1.grounded = c.col.Contains(point);

                        if (player1.grounded)
                            break;
                    }
                    
                }
                
                foreach(BouncePad b in bounceList){
                    b.Update(gameTime);
                    //checks for collision between player and bouncepads. bounce the player if there is
                    b.Collision(player1,player1.col);
                }

                player1.Update(gameTime);

                //check for sliding
                for (int i = 0; i < map.tiles.Count - 1; i++)
                {
                    
                    point = new Point((int)player1.col.Location.X-3, (int)(player1.pos.Y + (player1.col.Height/2)));
                    if (map.tiles[i].rectangle.Contains(point) && !player1.grounded)
                    {
                        player1.sliding = true;
                        break;
                    }
                    else
                        player1.sliding = false;
                    point = new Point((int)player1.col.Location.X+player1.col.Width + 6, (int)(player1.pos.Y + (player1.col.Height / 2)));
                    if (map.tiles[i].rectangle.Contains(point) && !player1.grounded)
                    {
                        player1.sliding = true;
                        break;
                    }
                    else
                        player1.sliding = false;
                }
                

                
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
#if WINDOWS_PHONE
                ScreenManager.AddScreen(new PhonePauseScreen(), ControllingPlayer);
#else
                ScreenManager.AddScreen(new PauseMenuScreen(game), ControllingPlayer);
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
#endif
            }
            else
            {
                //handle player input
                if(!player1.gameOver)
                player1.HandleInput(gameTime,input,gamePadState);
                
            }

            
        }

        //public void DebugPrintString(IAsyncResult result) {
        //    string msg = Guide.EndShowKeyboardInput(result);
        //    System.Diagnostics.Debug.WriteLine("Hello dickweed "+msg);
        //}

        //overlay text is drawn in respective to screen origin. used for debugging
        private void DrawOverlayText(SpriteBatch spriteBatch)
        {
            //uses a diff spritebatch draw. drawn in perspective of screen. doesnt follow camera: UI
            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, msg[random], new Vector2((screenwidth/2)-msg[random].Length-140, screenheight/2), Color.White);
            spriteBatch.Draw(blank,new Rectangle(0,0,screenwidth,screenheight),new Color(AlphaFade.R,AlphaFade.G,AlphaFade.B,MathHelper.Clamp(Alpha,0,255)));

            spriteBatch.End();
        }
        //
        
        //
        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.YellowGreen, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

           //GraphicsDevice device = graphics.GraphicsDevice;
            GraphicsDevice device = ScreenManager.GraphicsDevice;
            batch = spriteBatch;
            device.Clear(Color.YellowGreen);
            //draws in respective to space. eg: doesn't follow the camera
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,null,null,null,null,camera.transform);
           // spriteBatch.Begin();
            spriteBatch.Draw(sky,new Rectangle(0,0,(int)map.width,(int)map.height),Color.White);
            map.Draw(spriteBatch);
            foreach(PhysicsObjects.Launcher l in launchList){
                l.Draw(spriteBatch);
            }
             if (!player1.gameOver)
             {
                 player1.Draw(spriteBatch);
             }
            particleengine.Draw(spriteBatch);
            Break.Draw(spriteBatch);
            foreach(BouncePad b in bounceList){
                b.Draw(spriteBatch);
            }
            foreach(Spike s in spikeList){
                s.Draw(spriteBatch);
            }
            foreach(Crate c in crateList)
                c.Draw(spriteBatch);
            spriteBatch.Draw(flag,col,Color.White);
            spriteBatch.End();
            if (fading) 
            DrawOverlayText(spriteBatch);

           
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
