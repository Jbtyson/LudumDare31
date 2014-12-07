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

        public WorkerAnt(Vector2 position, Texture2D tex)
            : base(position, tex)
        {
            
        }

        public override void Update(GameTime time)
        {
            //Build tunnel
            if (Position != Target)
            {
                Position = getNextPosition();
                World.Instance[Position] |= (short)TileInfo.GroundUndug;
                World.Instance[Position] &= ~((short) TileInfo.GroundDug);
            }
            else
            {
                //Find new target
            }
        }

        private Vector2 getNextPosition()
        {
            double bestValue = 0;
            Vector2 bestPosition = Position;
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
                    if (value < bestValue)
                    //if(value < bestValue || rand.Next() == 1)
                    {
                        bestValue = value;
                        bestPosition = tempPosition;
                    }
                }
            }

            return bestPosition;
        }
    }
}
