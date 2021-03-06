﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Actors.Ants;

namespace PuissANT.Actors.Enemies
{
    public abstract class Enemy : Actor
    {
        protected static TileInfo[] PASSABLE_TILES = { TileInfo.GroundDug };

        public int Health;

        public virtual int Damage
        {
            get { return 1; }
        }

        protected Enemy(Vector2 position, int width, int heigth, Texture2D tex)
            : base(position, width, heigth, tex)
        {
            
        }

        protected Enemy(Point position, int width, int heigth, Texture2D tex, Rectangle drawingWindow)
            : base(position, width, heigth, tex, drawingWindow)
        {

        }

        public virtual void Attacked(Ant a)
        {
            Health -= a.Damage;

            if (Health <= 0)
            {
                //Kill this beetle
                ActorManager.Instance.Remove(this);
            }
        }
    }
}
