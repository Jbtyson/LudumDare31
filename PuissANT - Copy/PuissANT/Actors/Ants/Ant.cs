using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors.Ants
{
    public abstract class Ant : Actor
    {
        private static TileInfo[] PASSABLE_TILES = { TileInfo.GroundDug };

        protected static readonly Point INVALID_POINT = new Point(-1, -1);

        public double Health;

        protected Point Target;

        protected Ant(Point position,int width, int heigth, Texture2D tex)
            : base(position, width, heigth, tex)
        {
        }

        protected Ant(Point position, int width, int heigth, Texture2D tex, Rectangle rect)
            : base(position, width, heigth, tex, rect)
        {
        }

        public void SetTarget(Point t)
        {
            Target = t;
        }

        public void ClearTarget()
        {
            Target = INVALID_POINT;
        }
    }
}
