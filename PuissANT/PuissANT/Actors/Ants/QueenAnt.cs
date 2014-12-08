using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;
using PuissANT.Util;

namespace PuissANT.Actors.Ants
{
    public class QueenAnt : WorkerAnt
    {
        private const float PATH_UPDATE_TIME = 1000;
        private const float ANI_UPDATE_TIME = 500;

        private float pathTimer;
        private float aniTimer;
        private int frame;

        private float _angle;
        private bool _nestFound;

        public QueenAnt(Point position)
            : base(position, 6, 6, Game1.Instance.Content.Load<Texture2D>("sprites/ants/hierophANT"), new Rectangle(0, 0, 30, 20))
        {
            aniTimer = 0;
            pathTimer = 0;
            frame = 0;
        }

        public override void Update(GameTime time)
        {
            Point oldPosition = _position;
            
            aniTimer += time.ElapsedGameTime.Milliseconds;
            if (aniTimer > ANI_UPDATE_TIME)
            {
                frame = ++frame % 3;
                _drawingWindow = new Rectangle(0, frame*20, 30, 20);
                aniTimer = 0;
            // calculate angle ant is facing
            Vector2 facing = new Vector2(_position.X - oldPosition.X, _position.Y - oldPosition.Y);
            if (facing != Vector2.Zero) _angle = MathHelper.PiOver2 + (float)Math.Atan2(facing.Y, facing.X);
            }

            if (_nestFound) return;

            pathTimer += time.ElapsedGameTime.Milliseconds;
            if (!(pathTimer > PATH_UPDATE_TIME)) return;

            if (Target != INVALID_POINT)
            {
                if (MoveTowardsTarget())
                {
                    _nestFound = true;
                    NestPheromone p = PheromoneManger.Instance.GetPheromoneAt(Position) as NestPheromone;
                    p.Reached();
                }
            }
            else
            {
                Target = GetNewTarget<NestPheromone>();
            }

        }

        public override void Render(GameTime time, SpriteBatch batch)
        {
            batch.Draw(_texture, _texturePoint, _drawingWindow, Color.White, _angle, new Vector2(15,10), 1, SpriteEffects.None, 0);
        }
    }
}
