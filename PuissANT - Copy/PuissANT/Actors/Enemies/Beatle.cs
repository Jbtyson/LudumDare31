using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Actors.Ants;

namespace PuissANT.Actors.Enemies
{
    public class Beatle : Enemy
    {
        private static TileInfo[] PASSABLE_TILES = { TileInfo.GroundDug, TileInfo.GroundSoft, TileInfo.GroundMed, TileInfo.GroundHard };

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

        public override void Render(GameTime time, SpriteBatch batch)
        {
            throw new NotImplementedException();
        }
    }
}
