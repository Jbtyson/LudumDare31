﻿using System;
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
        Vector2 Dimensions;
        Image Image;

        string imgPath, text;

        public StatsPanel()
        {
            Dimensions = Vector2.Zero;
            Image = new Image();
            imgPath = "ui/statsBar";
            text = "this is the stats bar";
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(1040, 120);
            Image.Position = new Vector2(0, 0);
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
