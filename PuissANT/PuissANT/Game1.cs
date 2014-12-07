#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
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
        public static Rectangle ScreenSize;

        Texture2D antTexture;
        Rectangle antPosition;

        struct ant
        {
            public Texture2D tex;
            public Vector2 pos;
            public Vector2 dest;
        }

        ant[] ants = new ant[50];


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;

            ScreenSize = new Rectangle(0, 0, 1280, 720);

            graphics.PreferredBackBufferWidth = ScreenSize.Width;
            graphics.PreferredBackBufferHeight = ScreenSize.Height;

            IsMouseVisible = true;

            Content.RootDirectory = "Content";
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
            TerrainManager.Initialize(GraphicsDevice, ScreenSize);

            antTexture = new Texture2D(GraphicsDevice, 2, 2);
            Color[] colorBuf = new Color[antTexture.Width * antTexture.Height];
            for (int i = 0; i < colorBuf.Length; i++)
            {
                colorBuf[i] = Color.Black;
            }
            antTexture.SetData<Color>(colorBuf);
            antPosition = new Rectangle(ScreenSize.Width / 2, 120, antTexture.Width, antTexture.Height);

            Random r = new Random();
            for(int i = 0; i < ants.Length; i++)
            {
                ants[i].tex = antTexture;
                ants[i].pos = new Vector2(r.Next(0, ScreenSize.Width), 120);
                ants[i].dest = ants[i].pos; 
            }

            TerrainManager.ClearRectangle(new Rectangle(0, 0, ScreenSize.Width, 120));

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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
            Random r = new Random();
            for (int i = 0; i < ants.Length; i++)
            {
                if(Vector2.DistanceSquared(ants[i].pos, ants[i].dest) < 20)
                //if (ants[i].pos.X == ants[i].dest.X && ants[i].pos.Y == ants[i].dest.Y)
                    ants[i].dest = new Vector2(r.Next(0, ScreenSize.Width-5), r.Next(120, ScreenSize.Height-5));

                TerrainManager.ClearRectangle(ants[i].pos, ants[i].tex.Width, ants[i].tex.Height);

                float slopeY = ants[i].dest.Y - ants[i].pos.Y;
                float slopeX = ants[i].dest.X - ants[i].pos.X;
                Vector2 slope = new Vector2(slopeX, slopeY);

                slope.Normalize();

                ants[i].pos += slope;
                /*
                if (Math.Abs(slopeX) > Math.Abs(slopeY))
                    if (slopeX > 0)
                        ants[i].pos += new Vector2(1, 0);
                    else
                        ants[i].pos += new Vector2(-1, 0);
                else
                    if (slopeY > 0)
                        ants[i].pos += new Vector2(0, 1);
                    else
                        ants[i].pos += new Vector2(0, -1);
                 * */
            }

            Point mouse = Mouse.GetState().Position;
            Window.Title = "X: " + mouse.X + " Y: " + mouse.Y;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);
            TerrainManager.SetTexture();

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            TerrainManager.DrawTerrain(spriteBatch);
            for (int i = 0; i < ants.Length; i++)
            {
                spriteBatch.Draw(ants[i].tex, ants[i].pos, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
