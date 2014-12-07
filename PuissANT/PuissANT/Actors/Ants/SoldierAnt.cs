using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;

namespace PuissANT.Actors.Ants
{
    public class SoldierAnt : Ant
    {
        public SoldierAnt(Point position, int width, int height, Texture2D tex)
            : base(position, width, height, tex)
        {

        }

        public override void Update(GameTime time)
        {
            //Move towards attack pheremone
            IEnumerable<Pheromone> p = PheromoneManger.Instance.GetPheromoneOfType(PheromoneType.Attack);
            throw new System.NotImplementedException();
        }
        public override void Render(GameTime time, SpriteBatch batch)
        {
            throw new System.NotImplementedException();
        }
    }
}
