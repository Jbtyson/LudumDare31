using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PuissANT.Actors
{
    public class ActorManager
    {
        public static ActorManager Instance = new ActorManager();

        public List<Actor> _actors;

        private ActorManager()
        {
            _actors = new List<Actor>();
        }

        public void Add(Actor a)
        {
            _actors.Add(a);
        }

        public void Remove(Actor a)
        {
            _actors.Remove(a);
        }

        public IEnumerable<Actor> GetAllActors()
        {
            return _actors;
        }

        public IEnumerable<T> GetActorsByType<T>() 
            where T : Actor
        {
            return _actors.OfType<T>();
        }
    }
}
