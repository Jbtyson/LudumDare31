#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using PuissANT.Actors;
using PuissANT.Actors.Ants;
#endregion

namespace PuissANT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle GameWindow;

        Texture2D antTexture;

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
            ScreenManager.Instance.LoadContent(Content);
            ResourceManager.Instance.LoadContent();
            
            int gameWindowVerticalOffset = (int)ScreenManager.Instance.UiManager.PanelList[0].Dimensions.Y;
            int gameWindowHorizontalOffset = (int)ScreenManager.Instance.UiManager.PanelList[1].Dimensions.X;

            GameWindow = new Rectangle(0, gameWindowVerticalOffset,
                (int)ScreenManager.Instance.ScreenSize.X - gameWindowHorizontalOffset,
                (int)ScreenManager.Instance.ScreenSize.Y - gameWindowVerticalOffset);
            Actor.GameWindow = GameWindow;

            TerrainManager.Initialize(GraphicsDevice, GameWindow);
            World.Init((short)GameWindow.Width, (short)GameWindow.Height, TileInfo.GroundUndug);
            for (int x = 0; x < GameWindow.Width; x++)
            {
                for (int y = 0; y < GameWindow.Height / 5; y++)
                {
                    World.Instance[x, y] = (short) TileInfo.Sky;
                }
            }

            antTexture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] colorBuf = new Color[antTexture.Width * antTexture.Height];
            for (int i = 0; i < colorBuf.Length; i++)
            {
                colorBuf[i] = Color.Blue;
            }
            antTexture.SetData<Color>(colorBuf);

            Random r = new Random();
            for (int i = 0; i < 20; i++)
            {
                WorkerAnt ant = new WorkerAnt(
                    new Vector2(r.Next(0, GameWindow.Width), (GameWindow.Height/5)),
                    antTexture);
                ant.SetTarget(new Vector2(r.Next(0, GameWindow.Width-1), r.Next(GameWindow.Height/5, GameWindow.Height-1)));
                ActorManager.Instance.Add(ant);
            }
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

            foreach (Actor a in ActorManager.Instance.GetAllActors())
                a.Update(gameTime);

            MouseManager.Instance.Update(gameTime);
            ScreenManager.Instance.Update(gameTime);
            ResourceManager.Instance.Update(gameTime);
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
            GraphicsDevice.Clear(Color.White);
            TerrainManager.SetTexture();

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            foreach (Actor a in ActorManager.Instance.GetAllActors())
                a.Render(gameTime, spriteBatch);
            TerrainManager.DrawTerrain(spriteBatch);
            ScreenManager.Instance.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool isGameOver()
        {
            //return ActorManager.Instance.GetActorsByType<QueenAnt>().First().Health <= 0;
            return false;
        }
    }
}
