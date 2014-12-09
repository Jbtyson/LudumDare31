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

        public static int BEETLE_COUNT = 0;

        private const int ATTACK_DELAY = 1000;
        private int attackDelayTimer;

        private float pathTimer;
        private float aniTimer;
        private int frame;
        private bool faceForward;
        private List<Vector2> buffer;

        public override int Damage
        {
            get { return DAMAGE; }
        }

        public Beatle(Point position)
            : base(position, 9, 6, Game1.Instance.Content.Load<Texture2D>("sprites/enemies/beetle"), new Rectangle(0,0,69,50))
        {
            BEETLE_COUNT++;

            buffer = new List<Vector2>(8);
            aniTimer = 0;
            attackDelayTimer = 0;

            while (((TileInfo)World.Instance[Position]).IsTileType(TileInfo.GroundSoft))
            {
                Position = new Point(Position.X, Position.Y - 1);
                if (Position.Y < 0)
                {
                    Position = new Point(Position.X, World.Instance.Height - 1);
                }
            }

            while (((TileInfo)World.Instance[Position]).IsTileType(TileInfo.Sky))
            {
                Position = new Point(Position.X, Position.Y + 1);
                if (Position.Y > World.Instance.Height - 1)
                {
                    Position = new Point(Position.X, 0);
                }
            }
        }

        public override void Update(GameTime time)
        {
            aniTimer += time.ElapsedGameTime.Milliseconds;
            if (aniTimer > ANI_UPDATE_TIME)
            {
                frame = ++frame % 3;
                _drawingWindow = new Rectangle(0, frame * 50, 69, 50);
                aniTimer = 0;
            }

            pathTimer += time.ElapsedGameTime.Milliseconds;
            if (!(pathTimer > PATH_UPDATE_TIME)) return;

            pathTimer = 0;
            Position = getNextPosition();
            if (Position.X > World.Instance.Width / 2)
                faceForward = false;
            else
                faceForward = true;

            if (faceForward)
            {
                this._drawingWindow.X = 0;
            }
            else
            {

                this._drawingWindow.X = 73;
            }

            //Attack
            if (attackDelayTimer != 0)
            {
                attackDelayTimer -= time.ElapsedGameTime.Milliseconds;
                if (attackDelayTimer < 0)
                    attackDelayTimer = 0;
            }

            if (attackDelayTimer == 0)
            {
                foreach (Ant a in ActorManager.Instance.GetActorsByType<Ant>(Position.ToVector2(), _hitbox.Width * _hitbox.Height))
                {
                    if (canAttack(a))
                    {
                        a.Attacked(this);
                        attackDelayTimer = ATTACK_DELAY;
                        break;
                    }
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

                    //Only bias it towards the middle when it's on the surface
                    if (tempPosition.Y < World.Instance.Height / 5 + 20)
                    {
                        if (tempPosition.X > World.Instance.Width / 2 + 100 && i < 0)
                            buffer.Add(tempPosition);
                        else if (tempPosition.X < World.Instance.Width / 2 - 100 && i > 0)
                            buffer.Add(tempPosition);
                    }
                }
            }

            return buffer[RAND.Next(0, buffer.Count)].ToPoint();
        }

        private bool canAttack(Ant a)
        {
            return true;
        }

        public override void Attacked(Ant a)
        {
            base.Attacked(a);
            BEETLE_COUNT--;
        }
    }
}
