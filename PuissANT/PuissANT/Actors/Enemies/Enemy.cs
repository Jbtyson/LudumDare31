using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors.Enemies
{
    public abstract class Enemy : Actor
    {
        protected static TileInfo[] PASSABLE_TILES = { TileInfo.GroundDug };

        protected Enemy(Vector2 position, int width, int heigth, Texture2D tex)
            : base(position, width, heigth, tex)
        {
            
        }
    }
}
