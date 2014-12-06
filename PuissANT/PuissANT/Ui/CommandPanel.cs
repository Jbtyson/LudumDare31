using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT
{
    public class CommandPanel : IPanel
    {
        Vector2 Dimensions;
        Image Image;

        string imgPath, text;

        public CommandPanel()
        {
            Dimensions = Vector2.Zero;
            Image = new Image();
            imgPath = "ui/commandBar";
            text = "this is the command bar";
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(240, 720);
            Image.Position = new Vector2(ScreenManager.Instance.ScreenSize.X - Dimensions.X, ScreenManager.Instance.ScreenSize.Y - Dimensions.Y);
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
