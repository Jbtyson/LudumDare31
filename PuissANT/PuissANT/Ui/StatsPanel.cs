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

        public StatsPanel()
        {
            Dimensions = Vector2.Zero;
            Position = Vector2.Zero;
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(800, 120);
            Position = new Vector2(0, ScreenManager.Instance.ScreenSize.Y - Dimensions.Y);
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, (int)Dimensions.X, (int)Dimensions.Y);
            Color[] data = new Color[(int)Dimensions.X*(int)Dimensions.Y];
            for(int i = 0; i < data.Length; i++)
                data[i] = Color.White;
            texture.SetData<Color>(data);
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
