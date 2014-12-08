using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;
using PuissANT.Util;

namespace PuissANT.Actors
{
    public abstract class Actor
    {
        protected static readonly Random RAND = new Random();
        protected static readonly Point INVALID_POINT = new Point(-1, -1);

        protected Point _position;
        protected Rectangle _hitbox;
        protected Texture2D _texture;
        protected Vector2 _texturePoint;
        protected Rectangle _drawingWindow;
        
        
        public Point Position
        {
            get { return _position; }
            set { _position = value; updatePosition(); }
        }

        public Actor(Vector2 position, int width, int height, Texture2D tex)
            : this(position.ToPoint(), width, height, tex, new Rectangle(0, 0, tex.Width, tex.Height))
        {
            
        }

        public Actor(Point position, int width, int height, Texture2D tex)
            : this(position, width, height, tex, new Rectangle(0, 0, tex.Width, tex.Height))
        {

        }

        public Actor(Point position, int width, int height, Texture2D tex, Rectangle drawingWindow)
        {
            _position = position;
            _hitbox = new Rectangle(_position.X - width/2, _position.Y - height/2, width, height);
            _texture = tex;
            _drawingWindow = drawingWindow;
            _texturePoint = new Vector2(
                ScreenManager.Instance.GameWindow.X + (_position.X - _drawingWindow.Width / 2),
                ScreenManager.Instance.GameWindow.Y + (_position.Y - _drawingWindow.Height / 2));
            
        }

        public abstract void Update(GameTime time);

        public virtual void Render(GameTime time, SpriteBatch batch)
        {
            batch.Draw(_texture, _texturePoint, _drawingWindow, Color.White);
        }

        private void updatePosition()
        {
            //Update hitbox position.
            _hitbox.X = _position.X - _hitbox.Width/2;
            _hitbox.Y = _position.Y - _hitbox.Height/2;

            //Update texture position
            _texturePoint.X = ScreenManager.Instance.GameWindow.X + (_position.X - _drawingWindow.Width / 2);
            _texturePoint.Y = ScreenManager.Instance.GameWindow.Y + (_position.Y - _drawingWindow.Height / 2);
        }

        
    }
}
