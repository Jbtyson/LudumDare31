using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Ui
{
    public class DropDownBox
    {
        public Image Image;
        public List<Image> Children;
        public Vector2 Position, Dimensions;
        public string Text;

        public DropDownBox()
        {
            Position = Dimensions = Vector2.Zero;
            Image = new Image();
            Children = new List<Image>();
            Text = String.Empty;
        }

        public void LoadContent()
        {
            
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
