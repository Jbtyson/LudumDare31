using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT.Actors
{
    public class ActorManager
    {
        public static ActorManager Instance = new ActorManager();

        public List<Actor> _tempAdd;
        public List<Actor> _tempRemove;
        public List<Actor> _actors;

        private ActorManager()
        {
            _tempAdd = new List<Actor>();
            _tempRemove = new List<Actor>();
            _actors = new List<Actor>();
        }

        public void Add(Actor a)
        {
            _tempAdd.Add(a);
        }

        public void Remove(Actor a)
        {
            _tempRemove.Add(a);
        }

        public IEnumerable<Actor> GetAllActors()
        {
            return _actors.AsReadOnly();
        }

        public IEnumerable<T> GetActorsByType<T>() 
            where T : Actor
        {
            return _actors.OfType<T>();
        }

        public void Update(GameTime game)
        {
            _actors.AddRange(_tempAdd);
            _tempAdd.Clear();
            foreach (Actor a in _tempRemove)
                _actors.Remove(a);
            _tempRemove.Clear();
        }
    }
}
