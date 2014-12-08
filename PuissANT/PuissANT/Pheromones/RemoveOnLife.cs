using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT.Pheromones
{
    public abstract class RemoveOnLife : Pheromone
    {
        private int _lifeTimer;

        public RemoveOnLife(Point p, int i)
        {
            Position = p;
            Intensity = i;
            _lifeTimer = i*100;
        }

        public override void Update(GameTime time)
        {
            _lifeTimer -= time.ElapsedGameTime.Milliseconds;
            if (_lifeTimer < 0)
            {
                PheromoneManger.Instance.Remove(this);
            }
        }
    }
}
