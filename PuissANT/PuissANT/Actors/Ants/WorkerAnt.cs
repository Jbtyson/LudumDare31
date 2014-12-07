using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors.Ants
{
    public class WorkerAnt : Ant
    {
        private static readonly Random RAND = new Random();
        private const short PASSIBLE_TERRIAN = (short) TileInfo.GroundDug | (short) TileInfo.GroundUndug;
        private const long UPDATE_TIME = 10000;

        private long _updateTimer;
        private PriorityQueue<Vector2> _openQueue; 

        public WorkerAnt(Vector2 position, Texture2D tex)
            : base(position, tex)
        {
            _openQueue = new PriorityQueue<Vector2>();
        }

        public override void Update(GameTime time)
        {
            _updateTimer += time.ElapsedGameTime.Ticks;
            if (_updateTimer >= UPDATE_TIME)
            {
                _updateTimer = 0;
                if (Position != Target)
                {
                    //Build tunnel
                    Position = getNextPosition();
                    World.Instance[Position] |= (short)TileInfo.GroundUndug;
                    World.Instance[Position] &= ~((short)TileInfo.GroundDug);
                }
                else
                {
                    //Find new target

                }
            }
            
        }

        private Vector2 getNextPosition()
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    Vector2 tempPosition = Position;
                    tempPosition.X += i;
                    tempPosition.Y += j;
                    if ((World.Instance[tempPosition] & PASSIBLE_TERRIAN) == 0)
                    {
                        continue;
                    }

                    double value = Vector2.DistanceSquared(tempPosition, Target);
                    if (!_openQueue.ContainsValue(tempPosition))
                    {
                        _openQueue.Enqueue((int)value * 100, tempPosition);
                    }
                }
            }

            return _openQueue.Dequeue();
        }
    }
}
