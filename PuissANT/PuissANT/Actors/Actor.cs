using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors
{
    public abstract class Actor
    {
        public Vector2 Position { get; protected set; }
        public Texture2D Texure { get; protected set; }

        public Actor(Vector2 position)
        {
            Position = position;
        }

        public Actor(Vector2 position, Texture2D tex)
        {
            Position = position;
            Texure = tex;
        }

        public abstract void Update(GameTime time);

        public virtual void Render(GameTime time, SpriteBatch batch)
        {
            batch.Draw(Texure, Position, Color.White);
        }
    }
}
