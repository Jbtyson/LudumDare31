using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Util;

namespace PuissANT.Actors.Ants
{
    public class WorkerAnt : Ant
    {
        private static readonly Random RAND = new Random();
        private const short PASSIBLE_TERRIAN = (short) TileInfo.GroundDug | (short) TileInfo.GroundUndug;
        private const long UPDATE_TIME = 100000;
        private const short MEMORY = 500;

        private long _updateTimer;
        private readonly PriorityQueue<Point> _openQueue;
        private readonly List<Point> _closedList; 

        public WorkerAnt(Point position, int width, int height)
            : base(position, width, height, Game1.Instance.Content.Load<Texture2D>("ants/fireant.png"))
        {
            _openQueue = new PriorityQueue<Point>();
            _closedList = new List<Point>();
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
                    for (int x = 0; x < this._hitbox.Width && _hitbox.X + x < GameWindow.Width; x++)
                    {
                        for (int y = 0; y < _hitbox.Height && _hitbox.Y + y < GameWindow.Height; y++)
                        {
                            World.Instance[(int)_hitbox.X + x, (int)_hitbox.Y + y] |= (short)TileInfo.GroundDug;
                            World.Instance[(int)_hitbox.X + x, (int)_hitbox.Y + y] &= ~((short)TileInfo.GroundUndug);
                        }
                    }
                    
                    if (Position == Target)
                    {
                        _openQueue.Clear();
                        _closedList.Clear();
                    }
                }
                else
                {
                    //Find new target

                }
            }
            
        }

        private Point getNextPosition()
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

                    if(tempPosition.Y < 0 || tempPosition.Y >= World.Instance.Height)
                        continue;

                    if ((World.Instance[(int)tempPosition.X, (int)tempPosition.Y] & PASSIBLE_TERRIAN) == 0) //Cannot go through this terrian anyway
                        continue;
                    
                    //if(!_openQueue.ContainsValue(tempPosition) && _closedList.All(t => t != tempPosition))
                    if(_closedList.All(t => t != tempPosition.ToPoint()))
                    {
                        int value = (int)(Vector2.DistanceSquared(tempPosition, Target.ToVector2()) * 100);
                        value *= RAND.Next(1, 3);
                        if((World.Instance[(int)tempPosition.X, (int)tempPosition.Y] & (short)TileInfo.GroundUndug) != 0)
                            value *= RAND.Next(1, 3);
                        if (_openQueue.Count == MEMORY)
                            _openQueue.DequeueLast();
                        _openQueue.Enqueue(value, tempPosition.ToPoint());
                    }
                }
            }

            Point v = _openQueue.Dequeue();
            if(_closedList.Count == MEMORY)
                _closedList.RemoveAt(0);
            _closedList.Add(v);
            return v;
        }
    }
}
