using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT
{
    public class World
    {
        public static World Instance;

        public ResourceManager Resources;

        public static void Init(short width, short height, TileInfo initalInfo)
        {
            Instance = new World(height, width);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Instance[x, y] = (short)initalInfo;
                }
            }
        }

        private readonly short[] _tiles;
        public readonly short Height;
        public readonly short Width;

        public short this[Point p]
        {
            get { return this[p.X, p.Y]; }
            set
            {
                this[p.X, p.Y] = value;
            }
        }

        public short this[Vector2 v]
        {
            get { return this[(int)v.X, (int)v.Y]; }
            set
            {
                this[(int)v.X, (int)v.Y] = value;
            }
        }

        public short this[int x, int y]
        {
            get { return _tiles[(y*Width) + x]; }
            set
            {
                _tiles[(y*Width) + x] = value; 
                TerrainManager.UpdatePixel(x, y, value);
            }
        }

        private World(short height, short width)
        {
            Height = height;
            Width = width;
            _tiles = new short[height * width];
        }

        public void Set(int x, int y, TileInfo tile)
        {
            this[x, y] |= (short)tile;
        }
        
        public void Clear(int x, int y, TileInfo tile)
        {
            this[x, y] &= (short)~((int)tile);
        }
    }
}
