using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT.Pheromones
{
    public class AttackPheromone : RemoveOnLife
    {
        public AttackPheromone(Point p, int i)
            : base(p, i)
        {
        }

        public override void Reached()
        {
            //Do Nothing
        }
    }
}
