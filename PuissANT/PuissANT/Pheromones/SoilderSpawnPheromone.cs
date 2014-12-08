using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PuissANT.Actors;
using PuissANT.Actors.Ants;

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
            for(int i = 0; i < 3; i++)
                ActorManager.Instance.Add(new SoldierAnt(Position));

            base.Reached();
        }
    }
}
