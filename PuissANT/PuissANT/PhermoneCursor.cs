using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;
using PuissANT.Util;

namespace PuissANT.ui
{
    public class PhermoneCursor
    {
        private static PhermoneCursor _instance;

        public Vector2 Position;
        private Vector2 Center = new Vector2(12, 12);

        private Dictionary<TileInfo, Texture2D> cursors = new Dictionary<TileInfo,Texture2D>(1);

        public static PhermoneCursor Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PhermoneCursor();
                return _instance;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            // Load Cursor Icons
            cursors[TileInfo.Nest] = Content.Load<Texture2D>("ui/antCursor");
            cursors[TileInfo.Attack] = Content.Load<Texture2D>("phermones/AttackPheromone");
            cursors[TileInfo.Gather] = Content.Load<Texture2D>("phermones/GatherPheromone");
            cursors[TileInfo.WorkerSpawn] = Content.Load<Texture2D>("phermones/WorkerPhermone");
            cursors[TileInfo.SoilderSpawn] = Content.Load<Texture2D>("phermones/SoldierPhermone");
        }

        public void Update(GameTime gameTime)
        {
            /*Position.X = MouseManager.Instance.MouseX+500;
            Position.Y = MouseManager.Instance.MouseY;*/
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(cursors[PheromoneManger.Instance.MousePheromoneType], MouseManager.Instance.MousePosition.ToVector2() - Center, Color.White);
        }


    }
}
