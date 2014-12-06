using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT
{
    public class ScreenManager
    {
        public GraphicsDevice GraphicsDevice;
        public SpriteBatch SpriteBatch;
        public Vector2 ScreenSize;
        public UiManager UiManager;
        public ContentManager Content;

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

        public void LoadContent(ContentManager Content)
        {
            this.Content = Content;
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
    }
}
