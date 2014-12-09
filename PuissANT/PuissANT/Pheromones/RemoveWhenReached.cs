using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuissANT.Pheromones
{
    public abstract class RemoveWhenReached : Pheromone 
    {
        public override void Reached()
        {
            PheromoneManger.Instance.Remove(this);
        }
    }
}
