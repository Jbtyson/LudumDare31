using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT
{
    static class TerrainManager
    {
        private static readonly Color DEFAULT_COLOR = Color.White;
        private static readonly Color DIRT_COLOR = Color.Black;

        private static Rectangle _screenSize;
        private static Texture2D _texture;
        private static Color[] _colorBuffer;
        private static bool _initialized;
        static TerrainManager()
        {
            _initialized = false;
        }

        public static void Initialize(GraphicsDevice graphicsDevice, Rectangle windowsize)
        {
            _screenSize = windowsize;// new Rectangle(0, 0, (int)ScreenManager.Instance.ScreenSize.X, (int)ScreenManager.Instance.ScreenSize.Y);
            _texture = new Texture2D(graphicsDevice, _screenSize.Width, _screenSize.Height);
            _colorBuffer = new Color[_texture.Width * _texture.Height];
            for (int i = 0; i < _colorBuffer.Length; i++)
                _colorBuffer[i] = Color.Black;
            _initialized = true;
            SetTexture();
        }

        private static void CheckIfInitialized()
        {
            if (!_initialized)
                throw new NullReferenceException("Terrain Manager needs to be initialized with the Initialize(...) method!");
        }

        public static void ClearPixel(int x, int y)
        {
            CheckIfInitialized();
            Color currentColor = _colorBuffer[(y * _screenSize.Width) + x];
            currentColor.A = 0;
            
            UpdatePixel(x, y, currentColor);

        }
        public static void UpdatePixel(int x, int y, Color color)
        {
            CheckIfInitialized();
            int index = (y * _screenSize.Width) + x;
            _colorBuffer[index] = color;
        }

        public static void UpdatePixel(int x, int y, short info)
        {
            Color c = DEFAULT_COLOR;
            if ((info & (short)TileInfo.GroundUndug) != 0)
            {
                c = DIRT_COLOR;
            }

            UpdatePixel(x, y, c);
        }

        public static void UpdatePixel(int x, int y, float r, float g, float b, float a)
        {
            CheckIfInitialized();
            Color color = new Color(r, g, b, a);
            UpdatePixel(x, y, color);
        }

        public static void ClearRectangle(Rectangle bounds, float percentTrim = 0.0f)
        {
            CheckIfInitialized();
            UpdateRectangle(bounds, new Color[bounds.Width * bounds.Height], percentTrim);
        }

        public static void ClearRectangle(Vector2 position, int width, int height, float percentTrim = 0.0f)
        {
            ClearRectangle(new Rectangle((int)position.X, (int)position.Y, width, height), percentTrim);
        }


        public static void UpdateRectangle(Rectangle bounds, Color[] pixelElements, float percentTrim = 0.0f)
        {
            CheckIfInitialized();
            Point startPosition = bounds.Location;
            float widthTrim = bounds.Width * percentTrim;
            float heightTrim = bounds.Height * percentTrim;
            float rectWidth = bounds.Width;
            float rectHeight = bounds.Height;



            int startIndex = (startPosition.Y * _screenSize.Width) + startPosition.X;
            int currentIndex = startIndex;

            for(int y = (int)heightTrim; y < (int)(rectHeight - heightTrim); y++)
            {
                currentIndex = startIndex + (y * _screenSize.Width);
                for(int x = (int)widthTrim; x < (int)(rectWidth - widthTrim); x++)
                {
                    _colorBuffer[currentIndex + x] = pixelElements[(y * (int)rectWidth) + x];
                }
            }
        }

        public static void UpdateRectangle(Point position, Texture2D texture)
        {
            CheckIfInitialized();
            Rectangle bounds = texture.Bounds;
            bounds.Location = position;

            Color[] colorBuffer = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colorBuffer);

            UpdateRectangle(bounds, colorBuffer);
        }

        public static void SetTexture()
        {
            CheckIfInitialized();
            _texture.SetData<Color>(_colorBuffer);
        }

        public static void DrawTerrain(SpriteBatch spriteBatch)
        {
            CheckIfInitialized();
            spriteBatch.Draw(_texture, _screenSize, Color.White);
        }
    }
}
