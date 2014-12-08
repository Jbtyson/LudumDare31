using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;
using PuissANT.Util;

namespace PuissANT.Actors.Ants
{
    public abstract class Ant : Actor
    {
        public double Health;
        protected Point Target;

        protected Ant(Point position,int width, int heigth, Texture2D tex)
            : base(position, width, heigth, tex)
        {
            ZValue = 127;
        }

        protected Ant(Point position, int width, int heigth, Texture2D tex, Rectangle rect)
            : base(position, width, heigth, tex, rect)
        {
            ZValue = 127;
        }

        public void SetTarget(Point t)
        {
            Target = t;
        }

        public void ClearTarget()
        {
            Target = INVALID_POINT;
        }

        protected Point GetNewTarget(params Type[] types)
        {
            IEnumerable<Pheromone> points = PheromoneManger.Instance.GetPheromoneOfTypes(types);
            double minDistance;
            if (points.Any())
            {
                Pheromone bestPoint = points.First();
                minDistance = Vector2.DistanceSquared(Position.ToVector2(), bestPoint.Position.ToVector2());
                foreach (Pheromone p in points)
                {
                    double d2 = Vector2.DistanceSquared(Position.ToVector2(), p.Position.ToVector2());
                    if (d2 < minDistance || RAND.Next(0, 4) == 0)
                    {
                        bestPoint = p;
                        minDistance = d2;
                    }
                }
                return bestPoint.Position;
            }
            else
            {
                return INVALID_POINT;
            }
        }

        protected bool MoveTowardsTarget()
        {
            Position = getNextPosition();
            for (int x = 0; x < _hitbox.Width && _hitbox.X + x < ScreenManager.Instance.GameWindow.Width; x++)
            {
                for (int y = 0; y < _hitbox.Height && _hitbox.Y + y < ScreenManager.Instance.GameWindow.Height; y++)
                {
                    TileInfo tile = (TileInfo)World.Instance[(int)_hitbox.X + x, (int)_hitbox.Y + y];
                    World.Instance[(int)_hitbox.X + x, (int)_hitbox.Y + y] =
                        (short)tile.OverwriteTileValue(TileInfo.GroundDug);
                }
            }

            return Position == Target;
        }

        protected abstract Point getNextPosition();
    }
}
