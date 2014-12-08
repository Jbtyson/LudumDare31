using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Actors.Ants;
using PuissANT.Util;

namespace PuissANT.Actors.Enemies
{
    public class Beatle : Enemy
    {
        private static TileInfo[] PASSIBLE_TILES = { TileInfo.GroundDug };
        private const float PATH_UPDATE_TIME = 1;
        private const float ANI_UPDATE_TIME = 500;
        private const int DAMAGE = 5;
        private const int HEALTH = 50;

        private float pathTimer;
        private float aniTimer;
        private int frame;
        private bool faceForward;
        private List<Vector2> buffer;

        public override int Damage
        {
            get { return DAMAGE; }
        }

        public Beatle(Vector2 position)
            : base(position, 9, 6, Game1.Instance.Content.Load<Texture2D>("sprites/enemies/beetle"))
        {
            buffer = new List<Vector2>(8);
        }

        public override void Update(GameTime time)
        {
            /*aniTimer += time.ElapsedGameTime.Milliseconds;
            if (aniTimer > ANI_UPDATE_TIME)
            {
                frame = ++frame % 3;
                _drawingWindow = new Rectangle(0, frame * 20, 30, 20);
                aniTimer = 0;
            }*/

            pathTimer += time.ElapsedGameTime.Milliseconds;
            if (!(pathTimer > PATH_UPDATE_TIME)) return;

            pathTimer = 0;
            Point newPostion = getNextPosition();
            if (newPostion.X - Position.X > 0)
                faceForward = true;
            else if (newPostion.X - Position.X < 0)
                faceForward = false;

            //Attack
            foreach (Ant a in ActorManager.Instance.GetActorsByType<Ant>(Position.ToVector2(), _hitbox.Width*_hitbox.Height))
            {
                if (canAttack(a))
                {
                    a.Attacked(this);
                    break;
                }
            }

        }

        protected Point getNextPosition()
        {
            buffer.Clear();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) //This is our current position.
                        continue;

                    Vector2 tempPosition = Position.ToVector2();
                    tempPosition.X += i;
                    tempPosition.Y += j;
                    if (tempPosition.X < 0 || tempPosition.X >= World.Instance.Width)
                        continue;

                    if (tempPosition.Y < 0 || tempPosition.Y >= World.Instance.Height)
                        continue;

                    if (!((TileInfo)World.Instance[(int)tempPosition.X, (int)tempPosition.Y]).IsPassable(PASSIBLE_TILES)) //Cannot go through this terrian anyway
                        continue;

                    buffer.Add(tempPosition);
                }
            }

            return buffer[RAND.Next(0, buffer.Count)].ToPoint();
        }

        private bool canAttack(Ant a)
        {
            return true;
        }
    }
}
