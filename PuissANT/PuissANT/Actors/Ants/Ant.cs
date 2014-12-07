using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors.Ants
{
    public abstract class Ant : Actor
    {
        public double Health;

        protected Point Target;

        protected Ant(Point position, Texture2D tex)
            : base(position, tex)
        {
        }

        public void SetTarget(Point t)
        {
            Target = t;
        }
    }
}
