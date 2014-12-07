using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;
using PuissANT.Util;

namespace PuissANT.Actors.Ants
{
    public class QueenAnt : WorkerAnt
    {
        private float timer;
        private int frame;
        public QueenAnt(Point position, int width, int height)
            : base(position, width, height, Game1.Instance.Content.Load<Texture2D>("sprites/ants/hierophANT"))
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
        }

        public override void Render(GameTime time, SpriteBatch batch)
        {
            batch.Draw(_texture, _texturePoint.ToVector2(), new Rectangle(0,frame*20,30,20), Color.White);
        }
    }
}
