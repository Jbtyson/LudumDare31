using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;
using PuissANT.Util;

namespace PuissANT.Actors.Ants
{
    public class SoldierAnt : Ant
    {
        private static readonly TileInfo[] PASSIBLE_TILES = new TileInfo[] { TileInfo.GroundDug};
        private const int MEMORY = 500;
        private const int UPDATE_TIME = 1;

        private PriorityQueue<Point> _openQueue;
        private List<Point> _closedList;
        private int _updateTimer;

        public SoldierAnt(Point position)
            : base(position, 1, 1, Game1.Instance.Content.Load<Texture2D>("sprites/ants/fireant.png"))
        {
            _openQueue = new PriorityQueue<Point>();
            _closedList = new List<Point>(MEMORY);
            Target = INVALID_POINT;
        }

        public override void Update(GameTime time)
        {
            //Move towards attack pheremone
            _updateTimer += time.ElapsedGameTime.Milliseconds;
            if (_updateTimer >= UPDATE_TIME)
            {
                _updateTimer = 0;
                if (Target != INVALID_POINT)
                {
                    if (MoveTowardsTarget(false))
                    {
                        _openQueue.Clear();
                        _closedList.Clear();
                        Target = INVALID_POINT;
                    }

                    //Check for enemy to attack.
                }
                else
                {
                    Target = GetNewTarget(
                        typeof(AttackPheromone));
                }
            }
        }

        protected override Point getNextPosition()
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) //This is our current position.
                        continue;

                    Vector2 tempPosition = Position.ToVector2();
                    tempPosition.X += i;
                    tempPosition.Y += j;
                    if (tempPosition.X < 0 || tempPosition.X >= World.Instance.Width)
                        continue;

                    if (tempPosition.Y < 0 || tempPosition.Y >= World.Instance.Height)
                        continue;

                    if (!((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsPassable(PASSIBLE_TILES)) //Cannot go through this terrian anyway
                        continue;

                    if (_closedList.All(t => t != tempPosition.ToPoint()))
                    {
                        int value = ((int)(Vector2.DistanceSquared(tempPosition, Target.ToVector2()) * 100)) / 100;
                        value *= RAND.Next(1, 3);
                        if (tempPosition.ToPoint() == Target)
                            value = 0;
                        if (_openQueue.Count == MEMORY)
                            _openQueue.DequeueLast();
                        _openQueue.Enqueue(value, tempPosition.ToPoint());
                    }
                }
            }

            Point v = _openQueue.Dequeue();
            if (_closedList.Count == MEMORY)
                _closedList.RemoveAt(0);
            _closedList.Add(v);
            return v;
        }
    }
}
