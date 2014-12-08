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

        private float[] _histAngle = new float[4];
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

            // calculate angle ant is facing as an average of the prior 
            // four directions
            _histAngle[3] = _histAngle[2];
            _histAngle[2] = _histAngle[1];
            _histAngle[1] = _histAngle[0];

            // calculate angle ant is facing
            Vector2 facing = new Vector2(_position.X - oldPosition.X, _position.Y - oldPosition.Y);
            if (facing != Vector2.Zero) _histAngle[0] = (float)Math.Atan2(facing.Y, facing.X);
            float avgAngle = (_histAngle[3] + _histAngle[2] + _histAngle[1] + _histAngle[0]) / 4;
            if (avgAngle <= MathHelper.PiOver4 && avgAngle >= MathHelper.PiOver4)
                _angle = 0;
            else if (avgAngle > MathHelper.PiOver4 && avgAngle < 3 * MathHelper.PiOver4)
                _angle = MathHelper.PiOver2;
            else if (avgAngle < -MathHelper.PiOver4 && avgAngle > 3 * -MathHelper.PiOver4)
                _angle = -MathHelper.PiOver2;
            else
                _angle = MathHelper.Pi;
            //_angle -= MathHelper.PiOver2;
        }

        public override void Render(GameTime time, SpriteBatch batch)
        {
            batch.Draw(_texture, _texturePoint + new Vector2(15,10), _drawingWindow, Color.White, _angle, new Vector2(15,10), 1, SpriteEffects.FlipHorizontally, 0);
            //base.Render(time, batch);
        }
    }
}
