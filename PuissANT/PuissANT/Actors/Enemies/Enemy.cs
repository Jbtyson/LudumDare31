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
        protected Enemy(Vector2 position, Texture2D tex)
            : base(position, tex)
        {
            
        }
    }
}
