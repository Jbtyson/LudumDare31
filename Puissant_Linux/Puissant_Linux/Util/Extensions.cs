using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT.Util
{
    public static class Vector2Extensions
    {
        public static Point ToPoint(this Vector2 source)
        {
            return new Point((int)source.X, (int)source.Y);
        }

        public static Vector2 ToVector2(this Point source)
        {
            return new Vector2(source.X, source.Y);
        }
    }
}
