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
        private static Random _randomColorChooser;
        private static readonly Color DEFAULT_COLOR = Color.Transparent;
        private static Color[] SOFT_DIRT_COLOR;
        private static Color[] MEDIUM_DIRT_COLOR;
        private static Color[] HARD_DIRT_COLOR;
        private static Color[] ROCK_COLOR;
        private static int COLOR_RANDOMIZER_OFFSET = 4;

        private static Rectangle _screenSize;
        private static Texture2D _texture;
        private static Color[] _colorBuffer;
        private static bool _initialized;
        static TerrainManager()
        {
            _initialized = false;
        }

        // 
        private static void InitGroundColors()
        {
            _randomColorChooser = new Random();

            //SOFT_DIRT_COLOR = new Color[] { new Color(112, 88, 26), new Color(112, 88, 26), new Color(112, 88, 26), new Color(157, 154, 68) };
            //MEDIUM_DIRT_COLOR = new Color[] { new Color(112, 88, 26), new Color(63, 22, 29), new Color(63, 22, 29), new Color(63, 22, 29) };
            //HARD_DIRT_COLOR = new Color[] { new Color(63, 28, 5), new Color(67, 24, 53), new Color(67, 24, 53), new Color(67, 24, 53) };
            //ROCK_COLOR = new Color[] { new Color(219, 228, 237), new Color(219, 228, 237), new Color(219, 228, 237), new Color(157, 154, 68) };

            SOFT_DIRT_COLOR = new Color[] { new Color(112, 88, 26), new Color(157, 154, 68) };
            MEDIUM_DIRT_COLOR = new Color[] { new Color(63, 22, 29), new Color(112, 88, 26) };
            HARD_DIRT_COLOR = new Color[] { new Color(67, 24, 53), new Color(63, 28, 5) };
            ROCK_COLOR = new Color[] { new Color(219, 228, 237), new Color(157, 154, 68) };
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

            InitGroundColors();
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
            TileInfo tile = (TileInfo)info;
            if (tile.IsTileType(TileInfo.GroundSoft))
            {
                c = SOFT_DIRT_COLOR[_randomColorChooser.Next(COLOR_RANDOMIZER_OFFSET + 1) == COLOR_RANDOMIZER_OFFSET ? 1 : 0];
            }
            else if (tile.IsTileType(TileInfo.GroundMed))
            {
                c = MEDIUM_DIRT_COLOR[_randomColorChooser.Next(COLOR_RANDOMIZER_OFFSET + 1) == COLOR_RANDOMIZER_OFFSET ? 1 : 0];
            }
            else if (tile.IsTileType(TileInfo.GroundHard))
            {
                c = HARD_DIRT_COLOR[_randomColorChooser.Next(COLOR_RANDOMIZER_OFFSET + 1) == COLOR_RANDOMIZER_OFFSET ? 1 : 0];
            }
            else if (tile.IsTileType(TileInfo.GroundImp))
            {
                c = ROCK_COLOR[_randomColorChooser.Next(COLOR_RANDOMIZER_OFFSET + 1) == COLOR_RANDOMIZER_OFFSET ? 1 : 0];
            }
            else if (tile.IsTileType(TileInfo.GroundDug))
            {
                c = DEFAULT_COLOR;
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

        /// <summary>
        /// Will only clear out pixels with an alpha value above 0
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="pixelElements"></param>
        /// <returns>The color array that was blended to the background</returns>
        public static Color[] ClearColorMask(Rectangle bounds, Color[] pixelElements)
        {
            int startIndex = (bounds.Y * _screenSize.Width) + bounds.X;
            int currentIndex = startIndex;

            for (int y = 0; y < bounds.Height; y++)
            {
                currentIndex = startIndex + (y * _screenSize.Width);
                for (int x = 0; x < bounds.Width; x++)
                {
                    if (pixelElements[(y * bounds.Width) + x].A > 0)
                        pixelElements[(y * bounds.Width) + x] = _colorBuffer[currentIndex + x];
                    else
                        pixelElements[(y * bounds.Width) + x] = new Color(0, 0, 0, 0);
                }
            }

            UpdateRectangle(bounds, pixelElements);

            return pixelElements;
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
