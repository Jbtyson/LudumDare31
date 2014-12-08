using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PuissANT.Actors;
using PuissANT.Actors.Ants;

namespace PuissANT.Pheromones
{
    public class NestPheromone : RemoveWhenReached
    {
        public override void Update(GameTime time)
        {
            //Do nothing
        }

        public override void Reached()
        {
            QueenAnt ant = ActorManager.Instance.GetActorsByType<QueenAnt>().First();
            for (int i = 0; i < 3; i++)
            {
                ActorManager.Instance.Add(new WorkerAnt(ant.Position));
                //ActorManager.Instance.Add(new SoldierAnt(ant.Position));
            }

            base.Reached();
        }
    }
}
