using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Actors.Ants;
using PuissANT.Util;

namespace PuissANT.Actors.Enemies
{
    public class Worm : Enemy
    {
        private static TileInfo[] PASSIBLE_TILES = { TileInfo.GroundHard, TileInfo.GroundMed, TileInfo.GroundSoft };
        private const float PATH_UPDATE_TIME = 1;
        private const float ANI_UPDATE_TIME = 50;
        private const int DAMAGE = 5;
        private const int HEALTH = 100;

        public static int COUNT = 0;

        private float pathTimer;
        private float aniTimer;
        private int frame;
        private bool faceForward=true;
        private List<Vector2> buffer;
        private Random rand;

        public override int Damage
        {
            get { return DAMAGE; }
        }

        public Worm(Vector2 position, bool goLeft)
            : base(position, 98, 15, Game1.Instance.Content.Load<Texture2D>("sprites/enemies/worm"))
        {
            buffer = new List<Vector2>(8);
            rand = new Random();
            _drawingWindow.Height = 15;
            _drawingWindow.Y = 15;
            faceForward = goLeft;
        }

        public override void Update(GameTime time)
        {
            aniTimer += time.ElapsedGameTime.Milliseconds;
            if (aniTimer > ANI_UPDATE_TIME)
            {
                frame = ++frame % 3;
                _drawingWindow = new Rectangle(0, 15*frame, 98, 15);
                aniTimer = 0;
            }

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

            //Exit screen
            if (_position.X < -45 || _position.X > World.Instance.Width + 45) ActorManager.Instance.Remove(this);

        }

        //public override void Render(GameTime time, SpriteBatch batch)
        //{
        //    batch.Draw(_texture, _texturePoint, _drawingWindow, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, -200);
        //}

        protected Point getNextPosition()
        {
            if (faceForward)
            {
                // check for stone in front of worm
                if ((TileInfo)World.Instance[Position.X - 1, Position.Y] == TileInfo.GroundImp)
                {
                    faceForward = false;
                }
                else
                {
                    _position.X--;
                    updatePosition();
                    for (int i = 0; i < 10; i++)
                    {
                        if(_position.X > -45)
                            World.Instance.Set(Position.X - 45, Position.Y - 5 + i, TileInfo.GroundDug);
                        if (_position.X > -90)
                        {
                            World.Instance.Set(Position.X + 45, Position.Y - 5 + i, TileInfo.GroundSoft);
                            TerrainManager.UpdatePixel(Position.X + 45, Position.Y - 5 + i, (rand.Next(10) == 0) ? new Color(63, 28, 5) : new Color(112, 88, 26));
                        }
                    }
                }
            }
            else
            {
                if ((TileInfo)World.Instance[Position.X + 1, Position.Y] == TileInfo.GroundImp)
                {
                    faceForward = true;
                }
                else
                {
                    _position.X++;
                    updatePosition();
                    for (int i = 0; i < 15; i++)
                    {
                        if(_position.X < World.Instance.Width - 45)
                            World.Instance.Set(Position.X + 45, Position.Y - 7 + i, TileInfo.GroundDug);
                        if (_position.X < World.Instance.Width - 90)
                        {
                            World.Instance.Set(Position.X - 45, Position.Y - 7 + i, TileInfo.GroundSoft);
                            TerrainManager.UpdatePixel(Position.X - 45, Position.Y - 5 + i, (rand.Next(10) == 0) ? new Color(63, 28, 5) : new Color(112, 88, 26));
                        }
                    }
                }
            }
            return _position;
        }

        private bool canAttack(Ant a)
        {
            return true;
        }
    }
}
