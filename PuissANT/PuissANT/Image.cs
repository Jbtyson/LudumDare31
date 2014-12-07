using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT
{
    /// <summary>
    /// Image allows us to manipulate and add effects to images
    /// </summary>
    public class Image
    {
        private Vector2 _origin;
        private ContentManager _content;
        private RenderTarget2D _renderTarget;

        public Texture2D Texture;
        public float Alpha;
        public bool IsActive;
        public string Text, FontName, Path, Effects;
        public SpriteFont Font;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Image()
        {
            Path = Text = Effects = string.Empty;
            FontName = "fonts/font";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
        }

        public void LoadContent(string path, string text)
        {
            Path = path;
            Text = text;

            // Get the content
            _content = ScreenManager.Instance.Content;

            // Load the texture
            if (Path != string.Empty)
                Texture = _content.Load<Texture2D>(Path);

            // Load the font
            Font = _content.Load<SpriteFont>(FontName);

            // Get the dimensions
            Vector2 dimensions = Vector2.Zero;
            if (Texture != null)
            {
                dimensions.X += Texture.Width;
                dimensions.Y += Texture.Height;
            }
            dimensions.X += Font.MeasureString(Text).X;
            if (Texture != null)
                dimensions.Y = Math.Max(Texture.Height, Font.MeasureString(Text).Y);
            else
                dimensions.Y = Font.MeasureString(Text).Y;

            // Get the source rect of the image
            if (SourceRect == Rectangle.Empty)
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            // Do graphics magic to draw things
            _renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(_renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.DrawString(Font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            // Store the render target and then reset it
            Texture = _renderTarget;
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
        }

        public void UnloadContent()
        {
            _content.Unload();
        }

        public void Update(GameTime gametime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            spriteBatch.Draw(Texture, Position + _origin, SourceRect, Color.White * Alpha, 0.0f, _origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}