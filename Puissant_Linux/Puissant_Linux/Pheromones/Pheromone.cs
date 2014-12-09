using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PuissANT.Actors;

namespace PuissANT.Pheromones
{
    public abstract class Pheromone
    {
        public Point Position;
        public float Intensity;
        public PheromoneActor Actor;

        public abstract void Update(GameTime time);
        public abstract void Reached();
    }
}
