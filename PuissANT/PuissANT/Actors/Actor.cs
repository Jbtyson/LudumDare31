using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Util;

namespace PuissANT.Actors
{
    public abstract class Actor
    {
        public static Rectangle GameWindow;
        
        public Point Position
        {
            get { return Hitbox.Center; }
            set
            {
                Point diff = value - Hitbox.Center;
                Hitbox = new Rectangle(
                    Hitbox.X + diff.X,
                    Hitbox.Y + diff.Y,
                    Texture.Width,
                    Texture.Height);
            }
        }

        //public Vector2 Position { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public Rectangle Hitbox { get; protected set; }

        public Actor(Vector2 position, Texture2D tex)
            : this(position.ToPoint(), tex)
        {
            
        }

        public Actor(Point position, Texture2D tex)
        {
            Texture = tex;
            Hitbox = new Rectangle(0, 0, tex.Width, tex.Height);
            Position = position;
        }

        public abstract void Update(GameTime time);

        public virtual void Render(GameTime time, SpriteBatch batch)
        {
            Vector2 pos = new Vector2(GameWindow.X + Position.X, GameWindow.Y + Position.Y);
            batch.Draw(Texture, pos, Color.White);
        }
    }
}
