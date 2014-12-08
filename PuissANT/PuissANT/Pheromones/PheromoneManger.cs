using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Actors;
using PuissANT.Actors.Ants;

namespace PuissANT.Pheromones
{
    /// <summary>
    /// Keeps track of active pheromones on the map.
    /// </summary>
    public class PheromoneManger
    {
        private static Color SOILDER_HIGHLIGHT = new Color(255f, 1f, 0f, 0.1f);
        private static Color WORKER_HIGHLIGHT = new Color(255f, 0f, 0f, 0.1f);

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
            Pheromone p = null;
            if ((type & TileInfo.Nest) != 0)
            {
                p = new NestPheromone()
                {
                    Intensity = 100.0f,
                    Position = poistion
                };
                PheromoneActor actor = new PheromoneActor(poistion,
                    p.Intensity,
                    SOILDER_HIGHLIGHT, 
                    Game1.Instance.Content.Load<Texture2D>("phermones/SoldierPhermone.png"));
                p.Actor = actor;
            }
            else
            {
                throw new Exception();
            }

            _tempAdd.Add(p);
            ActorManager.Instance.Add(p.Actor);
            World.Instance.Set(poistion.X, poistion.Y, type);
            foreach (WorkerAnt a in ActorManager.Instance.GetActorsByType<WorkerAnt>())
            {
                a.ClearTarget();
            }
        }

        public void Remove(Pheromone p)
        {
            _tempRemove.Add(p);
            ActorManager.Instance.Remove(p.Actor);
            foreach (WorkerAnt a in ActorManager.Instance.GetActorsByType<WorkerAnt>())
            {
                a.ClearTarget();
            }
        }

        public IEnumerable<T> GetPheromoneOfType<T>()
        {
            return _activePheromones.OfType<T>();
        }

        public IEnumerable<Pheromone> GetPheromoneOfTypes(params Type[] types)
        {
            return _activePheromones.Where(p => types.Any(t => t == p.GetType()));
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
