using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;

namespace PuissANT.Actors.Ants
{
    public class QueenAnt : WorkerAnt
    {
        public QueenAnt(Point position, int width, int height)
            : base(position, width, height, Game1.Instance.Content.Load<Texture2D>("ants/fireant.png"))
        {
            
        }
    }
}
