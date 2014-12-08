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
        private static readonly TileInfo[] PASSABLE_TILES = { TileInfo.GroundDug, TileInfo.GroundSoft, TileInfo.GroundMed, TileInfo.GroundHard };
        private const float UPDATE_TIME = 1;
        private const short MEMORY = 500;

        private float _updateTimer;
        private readonly PriorityQueue<Point> _openQueue;
        private readonly List<Point> _closedList;
        private byte _diggingWaitTime;

        public WorkerAnt(Point position)
            : base(position, 1, 1, Game1.Instance.Content.Load<Texture2D>("sprites/ants/fireant.png"))
        {
            _openQueue = new PriorityQueue<Point>();
            _closedList = new List<Point>(MEMORY);
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
            if (!(_updateTimer > UPDATE_TIME)) return;

            _updateTimer = 0;
            if (Target != INVALID_POINT)
            {
                //Build tunnel
                if (_diggingWaitTime > 0)
                {
                    _diggingWaitTime--;
                }
                else if (MoveTowardsTarget())
                {
                    _openQueue.Clear();
                    _closedList.Clear();
                    Pheromone p = PheromoneManger.Instance.GetPheromoneAt(Position);
                    if (p != null)
                    {
                        //Ohterwise not the first on here.
                        p.Reached();
                        PheromoneManger.Instance.Remove(PheromoneManger.Instance.GetPheromoneAt(Position));
                    }
                    Target = INVALID_POINT;
                }
                else
                {
                    if (((TileInfo) World.Instance[(int) Position.X, (int) Position.Y]).IsTileType(
                        TileInfo.GroundMed))
                    {
                        _diggingWaitTime = 1;
                    }
                    else if (((TileInfo) World.Instance[(int) Position.X, (int) Position.Y]).IsTileType(
                        TileInfo.GroundHard))
                    {
                        _diggingWaitTime = 2;
                    }
                }
            }
            else
            {
                Target = GetNewTarget(
                    typeof (WorkerSpawnPheromone),
                    typeof(SoilderSpawnPheromone));
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

                    if (!((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsPassable(PASSABLE_TILES)) //Cannot go through this terrian anyway
                        continue;

                    if (_closedList.All(t => t != tempPosition.ToPoint()))
                    {
                        int value;
                        if (tempPosition.ToPoint() == Target)
                        {
                            value = 0;
                        }
                        else
                        {
                            value = ((int)(Vector2.DistanceSquared(tempPosition, Target.ToVector2()) * 100))/100;
                            value *= RAND.Next(1, 8);
                            if (((TileInfo) World.Instance[(int) tempPosition.X, (int) tempPosition.Y]).IsTileType(
                                    TileInfo.GroundSoft))
                            {
                                value /= 4;
                            }
                            else if (((TileInfo) World.Instance[(int) tempPosition.X, (int) tempPosition.Y]).IsTileType(
                                    TileInfo.GroundMed))
                            {
                                value /= 3;
                            }
                            else if (((TileInfo) World.Instance[(int) tempPosition.X, (int) tempPosition.Y])
                                    .IsTileType(TileInfo.GroundHard))
                            {
                                value /= 2;
                            }
                            else if (((TileInfo) World.Instance[(int) tempPosition.X, (int) tempPosition.Y])
                                    .IsTileType(TileInfo.GroundDug))
                            {
                                value  = (int)Math.Round((double)value * 2);
                            }
                        }

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

        public override void ClearTarget()
        {
            base.ClearTarget();
            _openQueue.Clear();
            _closedList.Clear();
        }
    }
}
