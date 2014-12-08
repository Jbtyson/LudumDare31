using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Actors.Ants;

namespace PuissANT.Actors.Enemies
{
    public class Beatle : Enemy
    {
        public Beatle(Vector2 position, int width, int height, Texture2D tex)
            : base(position, width, height, tex)
        {
            
        }

        public override void Update(GameTime time)
        {
            //Move towards queen.
            QueenAnt qa = ActorManager.Instance.GetActorsByType<QueenAnt>().First();
            throw new NotImplementedException();
        }
    }
}
