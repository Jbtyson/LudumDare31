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

        private bool _nestFound;

        public QueenAnt(Point position, int width, int height)
            : base(position, width, height, Game1.Instance.Content.Load<Texture2D>("sprites/ants/hierophANT"), new Rectangle(0, 0, 30, 20))
        {
            aniTimer = 0;
            pathTimer = 0;
            frame = 0;
        }

        public override void Update(GameTime time)
        {
            
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
                }
            }
            else
            {
                Target = GetNewTarget(TileInfo.Nest);
            }
        }
    }
}
