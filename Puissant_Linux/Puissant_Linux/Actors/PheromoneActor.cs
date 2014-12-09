using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Actors
{
    public class PheromoneActor : Actor
    {
        private Vector2 _intensityTexturePosition;
        private readonly Texture2D _intensityTexture; 

        public PheromoneActor(Point position, float intensity, Color c, Texture2D tex) 
            : base(position, 0, 0, tex)
        {
            ZValue = 129;
            _intensityTexture = createCircleText(intensity, c);
            _intensityTexturePosition = new Vector2(
                ScreenManager.Instance.GameWindow.X + (_position.X - _intensityTexture.Width / 2),
                ScreenManager.Instance.GameWindow.Y + (_position.Y - _intensityTexture.Height / 2));
        }

        public override void Update(GameTime time)
        {
            //Do nothings
        }

        public override void Render(GameTime time, SpriteBatch batch)
        {
            batch.Draw(_intensityTexture, _intensityTexturePosition);
            base.Render(time, batch);
        }

        protected override void updatePosition()
        {
            base.updatePosition();

            _intensityTexturePosition = new Vector2(
                ScreenManager.Instance.GameWindow.X + (_position.X - _intensityTexture.Width / 2),
                ScreenManager.Instance.GameWindow.Y + (_position.Y - _intensityTexture.Height / 2));
        }

        private Texture2D createCircleText(float radius, Color color)
        {
            int r = (int) Math.Round(radius);
            Texture2D texture = new Texture2D(Game1.Instance.GraphicsDevice, r, r);
            Color[] colorData = new Color[r * r];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * r + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = color;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
