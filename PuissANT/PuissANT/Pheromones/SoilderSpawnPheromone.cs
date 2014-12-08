using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT.Pheromones
{
    public class SoilderSpawnPheromone : RemoveWhenReached
    {
        public override void Update(GameTime time)
        {
            //Do nothing
        }

        public override void Reached()
        {
            //Create Soilder nest.

            base.Reached();
        }
    }
}
