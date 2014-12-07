using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors
{
    public abstract class Actor
    {
        public static Rectangle GameWindow;

        public Vector2 Position { get; protected set; }
        public Texture2D Texture { get; protected set; }

        public Actor(Vector2 position)
        {
            Position = position;
        }

        public Actor(Vector2 position, Texture2D tex)
        {
            Position = position;
            Texture = tex;
        }

        public abstract void Update(GameTime time);

        public virtual void Render(GameTime time, SpriteBatch batch)
        {
            Vector2 pos = new Vector2(GameWindow.X + Position.X, GameWindow.Y + Position.Y);
            batch.Draw(Texture, pos, Color.White);
        }
    }
}
