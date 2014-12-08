using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Ui
{
    public class StatsPanel : IPanel
    {
        public Vector2 Dimensions, ResourceDisplayStartPosition, ResourceDisplayOffset;
        public Image Image;
        public List<ResourceDisplay> Resources;

        private string _imagePath, _text;

        public StatsPanel()
        {
            Dimensions = Vector2.Zero;
            ResourceDisplayStartPosition = Vector2.Zero;
            ResourceDisplayOffset = Vector2.Zero;
            Image = new Image();
            Resources = new List<ResourceDisplay>();
            //http://opengameart.org/content/window-frame
            //_imagePath = "ui/panelBackground";
            _imagePath = "ui/statsBar";
            _text = String.Empty;
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(1080, 35);
            ResourceDisplayStartPosition = new Vector2(14, 14);
            ResourceDisplayOffset = new Vector2(192, 0);
            Image.Position = new Vector2(0, 0);
            Image.LoadContent(_imagePath, _text);

            int numResources = 0;
            Resources.Add(new ResourceDisplay("queenHealth"));
            Resources[numResources].LoadContent(new Vector2(ResourceDisplayStartPosition.X + numResources++ * ResourceDisplayOffset.X,
                ResourceDisplayStartPosition.Y));
            Resources.Add(new ResourceDisplay("ants"));
            Resources[numResources].LoadContent(new Vector2(ResourceDisplayStartPosition.X + numResources++ * ResourceDisplayOffset.X,
                ResourceDisplayStartPosition.Y));
            Resources.Add(new ResourceDisplay("dirt"));
            Resources[numResources].LoadContent(new Vector2(ResourceDisplayStartPosition.X + numResources++ * ResourceDisplayOffset.X,
                ResourceDisplayStartPosition.Y));           
            Resources.Add(new ResourceDisplay("food"));
            Resources[numResources].LoadContent(new Vector2(ResourceDisplayStartPosition.X + numResources++ * ResourceDisplayOffset.X,
                ResourceDisplayStartPosition.Y));
            Resources.Add(new ResourceDisplay("birthsPerSec"));
            Resources[numResources].LoadContent(new Vector2(ResourceDisplayStartPosition.X + numResources++ * ResourceDisplayOffset.X,
                ResourceDisplayStartPosition.Y));
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
            foreach (ResourceDisplay r in Resources)
                r.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Image.Draw(spriteBatch);
            foreach (ResourceDisplay r in Resources)
                r.Draw(spriteBatch);
        }

        Vector2 IPanel.Dimensions
        {
            get { return Dimensions; }
        }
    }
}
