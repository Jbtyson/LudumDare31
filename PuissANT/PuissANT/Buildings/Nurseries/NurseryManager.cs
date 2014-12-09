using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PuissANT.Util;


namespace PuissANT.Buildings.Nurseries
{
    class NurseryManager
    {
        public static NurseryManager Instance = new NurseryManager();

        public List<BaseNursery> _tempAdd;
        public List<BaseNursery> _tempRemove;
        public List<BaseNursery> _nursery;

        private NurseryManager()
        {
            _tempAdd = new List<BaseNursery>();
            _tempRemove = new List<BaseNursery>();
            _nursery = new List<BaseNursery>();
        }

        public void Add(BaseNursery a)
        {
            _tempAdd.Add(a);
        }

        public void Remove(BaseNursery a)
        {
            _tempRemove.Add(a);
        }

        public IEnumerable<BaseNursery> GetAllActors()
        {
            return _nursery.AsReadOnly();
        }

        public IEnumerable<BaseNursery> GetAllActors(Vector2 origin, double radius)
        {
            return _nursery.Where(a => Vector2.DistanceSquared(a.Position.ToVector2(), origin) < radius);
        }

        public IEnumerable<T> GetActorsByType<T>()
            where T : BaseNursery
        {
            return _nursery.OfType<T>();
        }

        public IEnumerable<T> GetActorsByType<T>(Vector2 origin, double radius)
            where T : BaseNursery
        {
            return _nursery.OfType<T>().Where(a => Vector2.DistanceSquared(a.Position.ToVector2(), origin) < radius);
        }

        public void Update(GameTime game)
        {
            _nursery.AddRange(_tempAdd);
            _tempAdd.Clear();
            foreach (BaseNursery a in _tempRemove)
                _nursery.Remove(a);
            _tempRemove.Clear();
        }
    }
}
