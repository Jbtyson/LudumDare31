using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT.Pheromones
{
    public abstract class Pheromone
    {
        public Point Position;
        public double Intensity;

        public abstract void Reached();
    }
}
