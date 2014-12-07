using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors.Ants
{
    public abstract class Ant : Actor
    {
        public double Health;

        protected Vector2 Target;

        protected Ant(Vector2 position, Texture2D tex)
            : base(position, tex)
        {
            
        }
    }
}
