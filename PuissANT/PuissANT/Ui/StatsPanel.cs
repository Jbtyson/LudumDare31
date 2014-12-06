using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT
{
    public class StatsPanel : IPanel
    {
        Vector2 Dimensions, Position;
        Image Image;

        string imgPath, text;

        public StatsPanel()
        {
            Dimensions = Vector2.Zero;
            Position = Vector2.Zero;
            Image = new Image();
            imgPath = "ui/statsBar";
            text = "this is a bar";
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(800, 120);
            Position = new Vector2(0, ScreenManager.Instance.ScreenSize.Y - Dimensions.Y);
            Image.LoadContent(imgPath, text);
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
