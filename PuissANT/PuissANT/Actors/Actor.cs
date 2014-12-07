using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Util;

namespace PuissANT.Actors
{
    public abstract class Actor
    {
        public static Rectangle GameWindow;

        protected Point _position;
        protected Rectangle _hitbox;
        protected Texture2D _texture;
        protected Point _texturePoint;
        
        public Point Position
        {
            get { return _position; }
            set { _position = value; updatePosition(); }
        }

        public Actor(Vector2 position, int width, int height, Texture2D tex)
            : this(position.ToPoint(), width, height, tex)
        {
            
        }

        public Actor(Point position, int width, int height, Texture2D tex)
        {
            _position = position;
            _hitbox = new Rectangle(_position.X - width/2, _position.Y - height/2, width, height);
            _texture = tex;
            _texturePoint = new Point(_position.X - tex.Width/2, _position.Y - tex.Height/2);
        }

        public abstract void Update(GameTime time);

        public virtual void Render(GameTime time, SpriteBatch batch)
        {
            Vector2 pos = new Vector2(GameWindow.X + Position.X, GameWindow.Y + Position.Y);
            batch.Draw(_texture, pos, Color.White);
        }

        private void updatePosition()
        {
            //Update hitbox position.
            _hitbox.X = _position.X - _hitbox.Width/2;
            _hitbox.Y = _position.Y - _hitbox.Height/2;

            //Update texture position
            _texturePoint.X = _position.X - _texture.Width/2;
            _texturePoint.Y = _position.Y - _texture.Height/2;
        }
    }
}
