using System;
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
        private float timer;
        private int frame;

        public QueenAnt(Vector2 position, Texture2D tex)
            : base(position, tex)
        {
            timer = 0;
            frame = 0;
        }

        public override void Update(GameTime time)
        {
            timer += time.ElapsedGameTime.Milliseconds;
            if (timer > 500)
            {
                frame = ++frame % 3;
                timer = 0;
            }

            //Move towards nest pheromone.
            Pheromone p = PheromoneManger.Instance.GetPheromoneOfType(PheromoneType.Nest).FirstOrDefault();
            //After that do nothing.
            //throw new NotImplementedException();
        }

        public override void Render(GameTime time, SpriteBatch batch)
        {
            batch.Draw(Texture, Position, new Rectangle(0,frame*20,30,20), Color.White);
            //batch.Draw(Texture, Position, new Rectangle(0, 0, 30, 20), Color.White, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, -50); 
            //throw new NotImplementedException();
        }
    }
}
