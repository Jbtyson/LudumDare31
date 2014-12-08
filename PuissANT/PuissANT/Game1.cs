#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PuissANT.Actors;
using PuissANT.Actors.Ants;
using PuissANT.Pheromones;
using PuissANT.ui;
using PuissANT.Util;

#endregion

namespace PuissANT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 Instance;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle GameWindow;
        Texture2D splashScreen;
        Texture2D backgroundTop;
        

        Texture2D antTexture;
        
        bool queenPlaced = false;
        int titleOffsetX;
        int titleOffsetY;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.ScreenSize.Y;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = false;
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent();
            ResourceManager.Instance.LoadContent();
            PhermoneCursor.Instance.LoadContent(Content);
            NumberRenderer.Instance.LoadContent(Content);

            // Load Cursor Icons
            Texture2D soldierPhermone = Content.Load<Texture2D>("phermones/SoldierPhermone");

            // Load Background textures
            splashScreen = Content.Load<Texture2D>("ui/splashScreen");
            backgroundTop = Content.Load<Texture2D>("ui/BackgroundTop");
            
            // Load And Play Music.
            try
            {
                SongCollection playlist = new SongCollection();
                playlist.Add(Content.Load<Song>("music/Robert del Naja - the shovel"));
                playlist.Add(Content.Load<Song>("music/Robert del Naja - HS"));
                playlist.Add(Content.Load<Song>("music/Robert del Naja - WS"));
                playlist.Add(Content.Load<Song>("music/Robert del Naja - DT3"));
                playlist.Add(Content.Load<Song>("music/Robert del Naja - BC"));
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(playlist);
            }
            catch (Exception e) { }

            int gameWindowVerticalOffset = (int)ScreenManager.Instance.UiManager.PanelList[0].Dimensions.Y;
            //int gameWindowHorizontalOffset = (int)ScreenManager.Instance.UiManager.PanelList[1].Dimensions.X;

            GameWindow = new Rectangle(0, gameWindowVerticalOffset,
                (int)ScreenManager.Instance.ScreenSize.X,// - gameWindowHorizontalOffset,
                (int)ScreenManager.Instance.ScreenSize.Y - gameWindowVerticalOffset);
            ScreenManager.Instance.GameWindow = GameWindow;

            TerrainManager.Initialize(GraphicsDevice, GameWindow);
            World.Init((short)GameWindow.Width, (short)GameWindow.Height, TileInfo.GroundSoft);
            
            // Load the title into the world
            Int32[] buffer = new Int32[28160];
            Image img = new Image();
            img.LoadContent("title/Title_0", String.Empty);
            img.Texture.GetData<Int32>(buffer, 0, 28160);
            titleOffsetX = GameWindow.Width / 2 - 200;
            titleOffsetY = GameWindow.Height / 5;
            for (int y = 0; y < 80; y++)
            {
                for (int x = 0; x < 352; x++)
                {
                    if (buffer[x + y * 352] == -16777216)
                        World.Instance[x + titleOffsetX, y + titleOffsetY - 28] = (short)TileInfo.GroundSoft;
                    else
                        World.Instance[x + titleOffsetX, y + titleOffsetY - 28] = (short)TileInfo.Sky;
                }
            }
            for (int x = 0; x < GameWindow.Width / 2 - 140; x++)
            {
                World.Instance[x, titleOffsetY + 8] = (short)TileInfo.Sky;
                World.Instance[x, titleOffsetY + 9] = (short)TileInfo.Sky;
                World.Instance[x, titleOffsetY + 10] = (short)TileInfo.Sky;
                World.Instance[x, titleOffsetY + 11] = (short)TileInfo.Sky;
                World.Instance[x + titleOffsetX + 340, titleOffsetY - 1] = (short)TileInfo.Sky;
                World.Instance[x + titleOffsetX + 340, titleOffsetY - 2] = (short)TileInfo.Sky;
                World.Instance[x + titleOffsetX + 340, titleOffsetY - 3] = (short)TileInfo.Sky;
                World.Instance[x + titleOffsetX + 340, titleOffsetY - 4] = (short)TileInfo.Sky;
            }
            ReticulateDirtLayers();

            //antTexture = Content.Load<Texture2D>("ants/fireant.png");
            /*antTexture = new Texture2D(graphics.GraphicsDevice, 2, 2);
            Color[] colorBuf = new Color[antTexture.Width * antTexture.Height];
            for (int i = 0; i < colorBuf.Length; i++)
            {
                colorBuf[i] = Color.Blue;
            }
            antTexture.SetData<Color>(colorBuf);*/


            QueenAnt queen = new QueenAnt(new Point(GameWindow.Width / 2, (GameWindow.Height/5))); 
            ActorManager.Instance.Add(queen);

            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                WorkerAnt ant = new WorkerAnt(
                    new Point(GameWindow.Width / 2, (GameWindow.Height/5)));
                //ant.SetTarget(new Vector2(r.Next(0, GameWindow.Width-1), r.Next(GameWindow.Height/5, GameWindow.Height-1)).ToPoint());
                ActorManager.Instance.Add(ant);
            }

            PheromoneManger.Instance.MousePheromoneType = TileInfo.Nest;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            ScreenManager.Instance.UnloadContent();
            ResourceManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            Point mouse = Mouse.GetState().Position;
            Window.Title = "X: " + mouse.X + " Y: " + mouse.Y;

            //Update managers.
            MouseManager.Instance.Update(gameTime);
            ScreenManager.Instance.Update(gameTime);
            ResourceManager.Instance.Update(gameTime);
            PheromoneManger.Instance.Update(gameTime);
            ActorManager.Instance.Update(gameTime);

            //Update user input.
            PhermoneCursor.Instance.Update(gameTime);
            if (MouseManager.Instance.WasJustClicked
                && ScreenManager.Instance.isPointWithinGameWindow(MouseManager.Instance.MousePosition)
                && PheromoneManger.Instance.CanSetPheromone(PheromoneManger.Instance.MousePheromoneType))
            {
                PheromoneManger.Instance.Add(PheromoneManger.Instance.MousePheromoneType, ScreenManager.Instance.getPointWithinGameWindow(MouseManager.Instance.MousePosition));
                
                // If this is the first click in the game, we switch the title image
                if (!queenPlaced)
                {
                    queenPlaced = true;

                    // Switch update screen
                    Int32[] buffer = new Int32[28160];
                    Image img = new Image();
                    img.LoadContent("title/Title_1", String.Empty);
                    img.Texture.GetData<Int32>(buffer, 0, 28160);
                    for (int y = 0; y < GameWindow.Height / 5; y++)
                    {
                        for (int x = 0; x < GameWindow.Width; x++)
                        {
                            World.Instance[x, y] = (short)TileInfo.Sky;
                        }
                    }
                    for (int y = 0; y < 80; y++)
                    {
                        for (int x = 0; x < 352; x++)
                        {
                            if (buffer[x + y * 352] == -16777216)
                                World.Instance[x + titleOffsetX, y + GameWindow.Height / 5 - 28] = (short)TileInfo.GroundSoft;
                            else
                                if(y > 28)
                                    World.Instance[x + titleOffsetX, y + GameWindow.Height / 5 - 28] = (short)TileInfo.Sky;
                                else
                                    World.Instance[x + titleOffsetX, y + GameWindow.Height / 5 - 28] = (short)TileInfo.GroundDug;
                        }
                    }
                    for (int x = 0; x < GameWindow.Width / 2 - 140; x++)
                    {
                        World.Instance[x, titleOffsetY] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 1] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 2] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 3] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 4] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 5] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 6] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 7] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 12] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 13] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 14] = (short)TileInfo.Sky;
                        World.Instance[x, titleOffsetY + 15] = (short)TileInfo.Sky;
                    }
                }
            }

            foreach (Actor a in ActorManager.Instance.GetAllActors())
                a.Update(gameTime);

            if(isGameOver())
                //handleGameOver

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //new Color(255,211,155)
            GraphicsDevice.Clear(new Color(216, 199, 84));
            TerrainManager.SetTexture();

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (!queenPlaced)
            {
                TerrainManager.DrawTerrain(spriteBatch);
                spriteBatch.Draw(splashScreen, Vector2.Zero, Color.White);

                foreach (Actor a in ActorManager.Instance.GetAllActors())
                    a.Render(gameTime, spriteBatch);

                PhermoneCursor.Instance.Render(gameTime, spriteBatch);
            }
            else
            {
                spriteBatch.Draw(backgroundTop, Vector2.Zero, Color.White);

                foreach (Actor a in ActorManager.Instance.GetAllActors())
                    a.Render(gameTime, spriteBatch);

                TerrainManager.DrawTerrain(spriteBatch);
                ScreenManager.Instance.Draw(spriteBatch);
                PhermoneCursor.Instance.Render(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool isGameOver()
        {
            //return ActorManager.Instance.GetActorsByType<QueenAnt>().First().Health <= 0;
            return false;
        }

        private void ReticulateDirtLayers()
        {
            //Add in layers of dirt. Will randomize better later
            float mediumDirtLayer = GameWindow.Height / 5 + GameWindow.Height * 4 / 5 / 2;
            float HardDirtLayer = GameWindow.Height / 5 + GameWindow.Height * 4 / 5 * 5 / 6;

            for (int x = 0; x < GameWindow.Width; x++)
            {
                for (int y = 0; y < GameWindow.Height; y++)
                {
                    if (((TileInfo)World.Instance[x, y]).IsTileType(TileInfo.Sky))
                        continue;

                    if (y - (float)Math.Abs(GameWindow.Width / 2 - x) / (GameWindow.Width / 2) * 50 + 25 >= HardDirtLayer)
                    {
                        World.Instance[x, y] = (short)TileInfo.GroundHard;
                    }
                    else if (y - (float)Math.Abs(GameWindow.Width / 2 - x) / (GameWindow.Width / 2) * 50 + 25 >= mediumDirtLayer)
                    {
                        World.Instance[x, y] = (short)TileInfo.GroundMed;
                    }
                }
            }
        }
    }
}
