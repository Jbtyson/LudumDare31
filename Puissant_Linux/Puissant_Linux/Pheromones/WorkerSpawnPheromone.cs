using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PuissANT.Actors;
using PuissANT.Actors.Ants;
using PuissANT.Buildings.Nurseries;

namespace PuissANT.Pheromones
{
    public class WorkerSpawnPheromone : RemoveWhenReached
    {
        public override void Update(GameTime time)
        {
            //Do nothing
        }

        public override void Reached()
        {
            //Create worker nest.
            //for(int i = 0; i < 3; i++)
                //ActorManager.Instance.Add(new WorkerAnt(this.Position));

            NurseryManager.Instance.Add(new WorkerNursery(this.Position));

            base.Reached();
        }
    }
}
