using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PuissANT.Ui;

namespace PuissANT
{
    public class ScreenManager
    {
        public GraphicsDevice GraphicsDevice;
        public SpriteBatch SpriteBatch;
        public Vector2 ScreenSize;
        public UiManager UiManager;
        public Rectangle GameWindow;

        private static ScreenManager _instance;
        
        public static ScreenManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScreenManager();
                return _instance;
            }
        }

        private ScreenManager()
        {
            ScreenSize = new Vector2(1280, 720);
            UiManager = new UiManager();
        }

        public void LoadContent()
        {
            UiManager.LoadContent();
        }

        public void UnloadContent()
        {
            UiManager.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            UiManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UiManager.Draw(spriteBatch);
        }

        public Point getPointWithinGameWindow(Point p)
        {
            return new Point(
                p.X - GameWindow.X,
                p.Y - GameWindow.Y);
        }

        public bool isPointWithinGameWindow(Point p)
        {
            return p.X >= GameWindow.X
                   && p.X <= (GameWindow.Width + GameWindow.X)
                   && p.Y >= GameWindow.Y
                   && p.Y <= (GameWindow.Width + GameWindow.Y);
        }
    }
}
