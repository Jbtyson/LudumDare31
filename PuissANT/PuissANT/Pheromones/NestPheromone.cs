using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PuissANT.Actors;
using PuissANT.Actors.Ants;

namespace PuissANT.Pheromones
{
    public class NestPheromone : Pheromone
    {
        public override void Reached()
        {
            QueenAnt ant = ActorManager.Instance.GetActorsByType<QueenAnt>().First();
            for (int i = 0; i < 3; i++)
            {
                ActorManager.Instance.Add(new WorkerAnt(ant.Position));
            }
        }
    }
}
