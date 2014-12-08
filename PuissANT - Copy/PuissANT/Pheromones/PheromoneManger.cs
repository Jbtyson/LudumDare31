using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuissANT.Pheromones
{
    /// <summary>
    /// Keeps track of active pheromones on the map.
    /// </summary>
    public class PheromoneManger
    {
        public static PheromoneManger Instance = new PheromoneManger();

        /// <summary>
        /// The pheromone type selected by the mouse.
        /// </summary>
        public TileInfo MousePheromoneType;

        /// <summary>
        /// Pheromones active on the map.
        /// </summary>
        public List<Pheromone> _tempAdd;  
        public List<Pheromone> _tempRemove;  
        public List<Pheromone> _activePheromones;

        private PheromoneManger()
        {
            _tempAdd = new List<Pheromone>();
            _tempRemove = new List<Pheromone>();
            _activePheromones = new List<Pheromone>();
        }

        public void Add(TileInfo type, Point poistion)
        {
            if ((type & TileInfo.Nest) != 0)
            {
                _tempAdd.Add(new NestPheromone()
                {
                    Intensity = 0.0,
                    Position = poistion
                });
            }

            World.Instance.Set(poistion.X, poistion.Y, type);
        }

        public void Remove(Pheromone p)
        {
            _tempRemove.Add(p);
        }

        public IEnumerable<T> GetPheromoneOfType<T>()
        {
            return _activePheromones.OfType<T>();
        }

        public Pheromone GetPheromoneAt(Point p)
        {
            return _activePheromones.FirstOrDefault(pheromone => pheromone.Position == p);
        }

        public bool CanSetPheromone(TileInfo type)
        {
            //Evalute if ther are enough resources to set the given type.
            return true;
        }

        public void HandlePheremoneButtonClick(string text) {
            TileInfo type = TileInfo.Attack;
            TileInfo[] array = TileInfoSets.PheromoneTypes;
            foreach (TileInfo t in array)
            {
                if (text == t.ToString())
                {
                    type = t;
                    break;
                }
            }

            MousePheromoneType = type;
        }

        public void Update(GameTime time)
        {
            _activePheromones.AddRange(_tempAdd);
            _tempAdd.Clear();
            foreach (Pheromone p in _tempRemove)
                _activePheromones.Remove(p);
            _tempRemove.Clear();
        }
    }
}
