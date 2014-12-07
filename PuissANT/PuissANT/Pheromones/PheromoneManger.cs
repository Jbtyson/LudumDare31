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

        public List<Pheromone> _activePheromones;

        private PheromoneManger()
        {
            _activePheromones = new List<Pheromone>();
        }

        public void Add(PheromoneType type, Point poistion)
        {
            _activePheromones.Add(new Pheromone()
            {
                Type = type,
                Position = poistion
            });
        }

        public void Remove(Pheromone p)
        {
            _activePheromones.Remove(p);
        }

        public IEnumerable<Pheromone> GetPheromoneOfType(PheromoneType type)
        {
            return _activePheromones.Where(p => p.Type == type);
        }

        public void HandlePheremoneButtonClick(string text) {
            PheromoneType type = PheromoneType.Attack;
            PheromoneType[] array = (PheromoneType[])Enum.GetValues(typeof(PheromoneType));
            foreach (PheromoneType t in array)
            {
                if (text == t.ToString())
                {
                    type = t;
                    break;
                }
            }
            Console.WriteLine(type.ToString() + " was clicked.");
        }
    }
}
