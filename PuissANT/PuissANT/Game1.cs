#region Using Statements
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PuissANT.Actors;
using PuissANT.Actors.Ants;
using PuissANT.Pheromones;
using PuissANT.ui;
using PuissANT.Util;
using System.Collections.Generic;

#endregion

namespace PuissANT
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static Game1 Instance;

        /// <summary>
        /// The amount to scroll before changing pheromones.
        /// </summary>
        private const int CHANGE_OFFSET = 500;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle GameWindow;

        Texture2D antTexture;

        bool queenPlaced = false;
        int titleOffsetX;
        int titleOffsetY;
        private int scrollOffset;

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
            
            int gameWindowVerticalOffset = (int)ScreenManager.Instance.UiManager.PanelList[0].Dimensions.Y;
            //int gameWindowHorizontalOffset = (int)ScreenManager.Instance.UiManager.PanelList[1].Dimensions.X;

            GameWindow = new Rectangle(0, gameWindowVerticalOffset,
                (int)ScreenManager.Instance.ScreenSize.X,
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


            QueenAnt queen = new QueenAnt(new Point(GameWindow.Width / 2, (GameWindow.Height/5))); 
            ActorManager.Instance.Add(queen);

            /*Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                WorkerAnt ant = new WorkerAnt(
                    new Point(GameWindow.Width / 2, (GameWindow.Height/5)));
                //ant.SetTarget(new Vector2(r.Next(0, GameWindow.Width-1), r.Next(GameWindow.Height/5, GameWindow.Height-1)).ToPoint());
                ActorManager.Instance.Add(ant);
            }*/

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
                Point p = ScreenManager.Instance.getPointWithinGameWindow(MouseManager.Instance.MousePosition);

                //Just make sure that you can't place the nest higher than the horizon
                if ((queenPlaced || (p.Y > GameWindow.Height / 5 && !((TileInfo)World.Instance[p]).IsTileType(TileInfo.Sky)))
                    && !((TileInfo)World.Instance[p]).IsTileType(TileInfo.GroundImp))
                {
                    placePheromone(p);

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
                                    if (y > 28)
                                        World.Instance[x + titleOffsetX, y + GameWindow.Height / 5 - 28] = (short)TileInfo.GroundDug;
                                    else
                                        World.Instance[x + titleOffsetX, y + GameWindow.Height / 5 - 28] = (short)TileInfo.Sky;
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

                        PheromoneManger.Instance.MousePheromoneType = PheromoneManger.Instance.GetNextTileInfo();
                    }
                }
            }
            
            if(queenPlaced)
            {
                scrollOffset += MouseManager.Instance.ScrollOffset();
                if (scrollOffset > CHANGE_OFFSET)
                {
                    scrollOffset = 0;
                    PheromoneManger.Instance.MousePheromoneType = PheromoneManger.Instance.GetNextTileInfo();
                    Console.Out.WriteLine(PheromoneManger.Instance.MousePheromoneType.ToString());
                }
            }

            foreach (Actor a in ActorManager.Instance.GetAllActors())
                a.Update(gameTime);

            if (isGameOver())
            {
                //handleGameOver
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(216, 199, 84));
            TerrainManager.SetTexture();
            spriteBatch.Begin(
                SpriteSortMode.Immediate, 
                BlendState.NonPremultiplied);

            IEnumerable<Actor> actors = ActorManager.Instance.GetAllActors();
            IEnumerable<Actor> ug = actors.Where(a => a.ZValue < 128).OrderBy(a => a.ZValue);
            IEnumerable<Actor> ag = actors.Where(a => a.ZValue >= 128).OrderBy(a => a.ZValue);
            foreach(Actor actor in ug)
                actor.Render(gameTime, spriteBatch);

            TerrainManager.DrawTerrain(spriteBatch);

            foreach(Actor actor in ag)
                actor.Render(gameTime, spriteBatch);
            
            foreach (Actor a in ActorManager.Instance.GetAllActors())
                a.Render(gameTime, spriteBatch);

            ScreenManager.Instance.Draw(spriteBatch);
            PhermoneCursor.Instance.Render(gameTime, spriteBatch);

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
            float minMediumDirtLayerFraction = 5.0f / 13.0f;
            float maxMediumDirtLayerFraction = 8.0f / 13.0f;
            float minHardDirtLayerFraction = 9.0f / 13.0f;
            float maxHardDirtLayerFraction = 12.0f / 13.0f;

            float minMediumDirtLayer = GameWindow.Height / 5 + GameWindow.Height * 4 / 5 * minMediumDirtLayerFraction;
            float maxMediumDirtLayer = GameWindow.Height / 5 + GameWindow.Height * 4 / 5 * maxMediumDirtLayerFraction;
            float minHardDirtLayer = GameWindow.Height / 5 + GameWindow.Height * 4 / 5 * minHardDirtLayerFraction;
            float maxHardDirtLayer = GameWindow.Height / 5 + GameWindow.Height * 4 / 5 * maxHardDirtLayerFraction;
            
            Random RAND = new Random();

            int currentMediumDirtLevel = RAND.Next((int)minMediumDirtLayer, (int)maxMediumDirtLayer);
            int currentHardDirtLevel = RAND.Next((int)minHardDirtLayer, (int)maxHardDirtLayer);

            int MAX_DIRT_DELTA = 3;

            for (int x = 0; x < GameWindow.Width; x++)
            {
                //Recalculate the dirt level
                int direction = (RAND.Next((int)minMediumDirtLayer, (int)maxMediumDirtLayer) > currentMediumDirtLevel ? 1 : -1);
                int vel = RAND.Next(0, MAX_DIRT_DELTA);

                currentMediumDirtLevel += vel * direction;

                direction = (RAND.Next((int)minHardDirtLayer, (int)maxHardDirtLayer) > currentHardDirtLevel ? 1 : -1);
                vel = RAND.Next(0, MAX_DIRT_DELTA);

                currentHardDirtLevel += vel * direction;

                for (int y = 0; y < GameWindow.Height; y++)
                {
                    if (((TileInfo)World.Instance[x, y]).IsTileType(TileInfo.Sky))
                        continue;

                    if (y >= currentHardDirtLevel)
                    {
                        World.Instance[x, y] = (short)TileInfo.GroundHard;
                    }
                    else if (y >= currentMediumDirtLevel)
                    {
                        World.Instance[x, y] = (short)TileInfo.GroundMed;
                    }
                }
            }

            int MAX_STONES, MIN_STONES, MIN_STONE_SIZE, MAX_STONE_SIZE;
            
            List<Point> allStones = new List<Point>();
            //List<Tuple<int, int>> openStones = new List<Tuple<int, int>>();
            List<KeyValuePair<Point, int>> possibleStones = new List<KeyValuePair<Point, int>>();

            int stoneCount, possibleStoneRandCount;

            //QUICK, EXTREMELY DIRTY COPY/PASTE CODE TO MAKE THINGS EASIER.

            //Here's a description of the rock creation code. It picks a point randomly on the map.
            //It calculates the size of the rock, then starts choosing a pixel that is next to
            //one that the rock currently is on. It then adds the neighboring pixels to the new one
            //to the list of potential next rock parts. It also keeps a count of the # of rock pixels
            //adjacent to the possible next rock pixel to have a better chance of picking ones that
            //are surrounded. After it hits the size limit, it goes through and finds any that are
            //surrounded on all 4 sides and fills them in, then makes the rock on the map.

            //It does this for pockets of soft dirt, pockets of medium dirt and for rocks. It takes a
            // while, sorry it's not better optimized

            //Note: it may be better to try Nick's approach & use Bernoulli's Line Drawing Algorithm

            #region SoftDirtPatches
            MAX_STONES = 100;
            MIN_STONES = 50;

            MIN_STONE_SIZE = 100;
            MAX_STONE_SIZE = 250;

            stoneCount = RAND.Next(MIN_STONES, MAX_STONES);
            possibleStoneRandCount = 0;

            for (int i = 0; i < stoneCount; i++)
            {
                //Determine stone seed spot
                int x, y;
                x = RAND.Next(0, GameWindow.Width);
                y = RAND.Next((int)minMediumDirtLayer, (int)(minMediumDirtLayer + (maxMediumDirtLayer - minMediumDirtLayer) * 9 / 8));

                Point stone = new Point(x, y);
                //openStones.Add(stone);
                allStones.Add(stone);

                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x + 1, y), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x - 1, y), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x, y + 1), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x, y - 1), 1));

                possibleStoneRandCount = 4;

                int stoneSize = RAND.Next(MIN_STONE_SIZE, MAX_STONE_SIZE);

                for (int j = 0; j < stoneSize; j++)
                {
                    int randIndex = RAND.Next(0, possibleStoneRandCount);
                    int curIndex = 0;
                    foreach (KeyValuePair<Point, int> keyValue in possibleStones)
                    {
                        curIndex += keyValue.Value;
                        if (curIndex > randIndex)
                        {
                            //Add this stone
                            allStones.Add(keyValue.Key);

                            possibleStoneRandCount -= keyValue.Value;

                            //Add the surrounding stones to the possible stone count
                            Point newStone = new Point(keyValue.Key.X + 1, keyValue.Key.Y);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X - 1, keyValue.Key.Y);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X, keyValue.Key.Y + 1);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X, keyValue.Key.Y - 1);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            possibleStones.Remove(keyValue);
                            break;
                        }
                    }


                }

                //Clean up the stone
                while (possibleStones.Count > 0)
                {
                    if (possibleStones[0].Value == 4)
                    {
                        //Fill in surrounded holes
                        allStones.Add(possibleStones[0].Key);
                    }
                    possibleStones.RemoveAt(0);
                }
                possibleStoneRandCount = 0;

                //Create the stones
                while (allStones.Count > 0)
                {
                    if (allStones[0].X >= 0 && allStones[0].X < GameWindow.Width &&
                        allStones[0].Y >= GameWindow.Height / 5 &&
                        allStones[0].Y < GameWindow.Height)
                    {
                        World.Instance[allStones[0].X, allStones[0].Y] = (short)TileInfo.GroundSoft;
                    }

                    allStones.RemoveAt(0);
                }


            }
            #endregion

            #region MediumDirtPatches
            MAX_STONES = 30;
            MIN_STONES = 15;

            stoneCount = RAND.Next(MIN_STONES, MAX_STONES);
            possibleStoneRandCount = 0;

            for (int i = 0; i < stoneCount; i++)
            {
                //Determine stone seed spot
                int x, y;
                x = RAND.Next(0, GameWindow.Width);
                y = RAND.Next((int)minHardDirtLayer, (int)(minHardDirtLayer + (maxHardDirtLayer - minHardDirtLayer) * 9 / 8));

                Point stone = new Point(x, y);
                //openStones.Add(stone);
                allStones.Add(stone);

                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x + 1, y), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x - 1, y), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x, y + 1), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x, y - 1), 1));

                possibleStoneRandCount = 4;

                int stoneSize = RAND.Next(MIN_STONE_SIZE, MAX_STONE_SIZE);

                for (int j = 0; j < stoneSize; j++)
                {
                    int randIndex = RAND.Next(0, possibleStoneRandCount);
                    int curIndex = 0;
                    foreach (KeyValuePair<Point, int> keyValue in possibleStones)
                    {
                        curIndex += keyValue.Value;
                        if (curIndex > randIndex)
                        {
                            //Add this stone
                            allStones.Add(keyValue.Key);

                            possibleStoneRandCount -= keyValue.Value;

                            //Add the surrounding stones to the possible stone count
                            Point newStone = new Point(keyValue.Key.X + 1, keyValue.Key.Y);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X - 1, keyValue.Key.Y);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X, keyValue.Key.Y + 1);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X, keyValue.Key.Y - 1);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            possibleStones.Remove(keyValue);
                            break;
                        }
                    }


                }

                //Clean up the stone
                while (possibleStones.Count > 0)
                {
                    if (possibleStones[0].Value == 4)
                    {
                        //Fill in surrounded holes
                        allStones.Add(possibleStones[0].Key);
                    }
                    possibleStones.RemoveAt(0);
                }
                possibleStoneRandCount = 0;

                //Create the stones
                while (allStones.Count > 0)
                {
                    if (allStones[0].X >= 0 && allStones[0].X < GameWindow.Width &&
                        allStones[0].Y >= GameWindow.Height / 5 &&
                        allStones[0].Y < GameWindow.Height)
                    {
                        World.Instance[allStones[0].X, allStones[0].Y] = (short)TileInfo.GroundMed;
                    }

                    allStones.RemoveAt(0);
                }


            }
            #endregion

            #region rocks
            MAX_STONES = 10;
            MIN_STONES = 5;

            MIN_STONE_SIZE = 100;
            MAX_STONE_SIZE = 1000;

            stoneCount = RAND.Next(MIN_STONES, MAX_STONES);
            possibleStoneRandCount = 0;

            for (int i = 0; i < stoneCount; i++)
            {
                //Determine stone seed spot
                int x, y;
                x = RAND.Next(0, GameWindow.Width);
                y = RAND.Next(GameWindow.Height / 5, GameWindow.Height);

                Point stone = new Point(x, y);
                //openStones.Add(stone);
                allStones.Add(stone);

                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x + 1, y), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x - 1, y), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x, y + 1), 1));
                possibleStones.Add(new KeyValuePair<Point, int>(new Point(x, y - 1), 1));

                possibleStoneRandCount = 4;

                int stoneSize = RAND.Next(MIN_STONE_SIZE, MAX_STONE_SIZE);

                for (int j = 0; j < stoneSize; j++)
                {
                    int randIndex = RAND.Next(0, possibleStoneRandCount);
                    int curIndex = 0;
                    foreach (KeyValuePair<Point, int> keyValue in possibleStones)
                    {
                        curIndex += keyValue.Value;
                        if (curIndex > randIndex)
                        {
                            //Add this stone
                            allStones.Add(keyValue.Key);

                            possibleStoneRandCount -= keyValue.Value;

                            //Add the surrounding stones to the possible stone count
                            Point newStone = new Point(keyValue.Key.X + 1, keyValue.Key.Y);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X - 1, keyValue.Key.Y);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X, keyValue.Key.Y + 1);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            newStone = new Point(keyValue.Key.X, keyValue.Key.Y - 1);
                            if (!allStones.Contains(newStone))
                            {
                                addNewPossibleStone(newStone, possibleStones);
                                possibleStoneRandCount++;
                            }

                            possibleStones.Remove(keyValue);
                            break;
                        }
                    }


                }

                //Clean up the stone
                while (possibleStones.Count > 0)
                {
                    if (possibleStones[0].Value == 4)
                    {
                        //Fill in surrounded holes
                        allStones.Add(possibleStones[0].Key);
                    }
                    possibleStones.RemoveAt(0);
                }
                possibleStoneRandCount = 0;

                //Create the stones
                while (allStones.Count > 0)
                {
                    if (allStones[0].X >= 0 && allStones[0].X < GameWindow.Width &&
                        allStones[0].Y >= GameWindow.Height / 5 &&
                        allStones[0].Y < GameWindow.Height)
                    {
                        World.Instance[allStones[0].X, allStones[0].Y] = (short)TileInfo.GroundImp;
                    }

                    allStones.RemoveAt(0);
                }


            }
            #endregion

        }

        private void addNewPossibleStone(Point newStone, List<KeyValuePair<Point, int>> possibleStones)
        {
            bool foundIt = false;
            for (int k = 0; k < possibleStones.Count; k++)
            {
                if (possibleStones[k].Key == newStone)
                {
                    //If it's already in there, increment the count so it has
                    // a higher chance of being done
                    possibleStones[k] = new KeyValuePair<Point, int>(
                        possibleStones[k].Key, possibleStones[k].Value + 1);
                    foundIt = true;
                    break;
                }
            }
            if (!foundIt)
            {
                //Add it
                possibleStones.Add(new KeyValuePair<Point, int>(newStone, 1));
            }
        }

        private void placePheromone(Point p)
        {
            while (((TileInfo) World.Instance[p.X, p.Y + 1]).IsTileType(TileInfo.Sky))
            {
                p = new Point(p.X, p.Y + 1);
            }

            PheromoneManger.Instance.Add(PheromoneManger.Instance.MousePheromoneType, p);
        }
    }
}
