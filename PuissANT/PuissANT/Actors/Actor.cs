using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors
{
    public abstract class Actor
    {
        public Point Position { get; private set; }

        public Actor()
        {
            Position = new Point();
            ActorManager.Instance.Add(this);
        }

        public Actor(Point position)
        {
            Position = position;
        }

        public abstract void Update(GameTime time);

        public abstract void Render(GameTime time);
    }
}
