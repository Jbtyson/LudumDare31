﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;

namespace PuissANT.Actors.Ants
{
    public class QueenAnt : Ant
    {
        public QueenAnt(Vector2 position, Texture2D tex)
            : base(position, tex)
        {
            
        }

        public override void Update(GameTime time)
        {
            //Move towards nest pheromone.
            Pheromone p = PheromoneManger.Instance.GetPheromoneOfType(PheromoneType.Nest).FirstOrDefault();
            //After that do nothing.
            throw new NotImplementedException();
        }

        public override void Render(GameTime time, SpriteBatch batch)
        {
            throw new NotImplementedException();
        }
    }
}
