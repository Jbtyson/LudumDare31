using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;
using PuissANT.Util;

namespace PuissANT.Actors.Ants
{
    public class WorkerAnt : Ant
    {
        private static readonly Random RAND = new Random();
        private const short PASSIBLE_TERRIAN = 0x1;
        private const long UPDATE_TIME = 100000;
        private const short MEMORY = 500;

        private long _updateTimer;
        private readonly PriorityQueue<Point> _openQueue;
        private readonly List<Point> _closedList; 

        public WorkerAnt(Point position)
            : base(position, 1, 1, Game1.Instance.Content.Load<Texture2D>("sprites/ants/fireant.png"))
        {
            _openQueue = new PriorityQueue<Point>();
            _closedList = new List<Point>();
            Target = INVALID_POINT;
        }

        protected WorkerAnt(Point position, int width, int height, Texture2D tex, Rectangle rect)
            : base(position, width, height, tex, rect)
        {
            _openQueue = new PriorityQueue<Point>();
            _closedList = new List<Point>();
            Target = INVALID_POINT;
        }

        public override void Update(GameTime time)
        {
            _updateTimer += time.ElapsedGameTime.Milliseconds;
            if (_updateTimer >= UPDATE_TIME)
            {
                _updateTimer = 0;
                if (Target != INVALID_POINT)
                {
                    //Build tunnel
                    if (MoveTowardsTarget())
                    {
                        _openQueue.Clear();
                        _closedList.Clear();
                        PheromoneManger.Instance.Remove(PheromoneManger.Instance.GetPheromoneAt(Position));
                        Target = INVALID_POINT;
                    }
                }
                else
                {
                    //Find new target
                    Target = GetNewTarget(
                        typeof(NestPheromone));
                }
            }
            
        }

        private Point GetNewTarget()
        {
            return GetNewTarget(typeof(NestPheromone));
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

                    if (!((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsPassable()) //Cannot go through this terrian anyway
                        continue;

                    if (_closedList.All(t => t != tempPosition.ToPoint()))
                    {
                        int value = ((int)(Vector2.DistanceSquared(tempPosition, Target.ToVector2()) * 100)) / 100;
                        value *= RAND.Next(1, 3);
                        if (((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsTileType(TileInfo.GroundSoft))
                            value *= 3;
                        else if (((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsTileType(TileInfo.GroundMed))
                            value *= 2;
                        else if (((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsTileType(TileInfo.GroundMed))
                            value *= RAND.Next(1, 2);
                        //else if (((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsTileType(TileInfo.GroundHard))
                            //value *= RAND.Next(1, 1);
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
