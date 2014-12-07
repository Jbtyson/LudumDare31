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

        public static void Init(short height, short width)
        {
            Instance = new World(height, width);
        }

        private readonly short[] _tiles;
        private readonly short _height;
        private readonly short _width;

        public short this[Point p]
        {
            get { return this[p.X, p.Y]; }
            set { this[p.X, p.Y] = value; }
        }

        public short this[Vector2 v]
        {
            get { return this[(int)v.X, (int)v.Y]; }
            set { this[(int)v.X, (int)v.Y] = value; }
        }

        public short this[int i, int j]
        {
            get { return _tiles[(i*_width) + j]; }
            set { _tiles[(i*_width) + j] = value; }
        }

        private World(short height, short width)
        {
            _height = height;
            _width = width;
            _tiles = new short[height * width];
        }
    }
}
