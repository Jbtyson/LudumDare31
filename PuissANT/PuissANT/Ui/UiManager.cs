using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Ui
{
    public class UiManager
    {
        private List<IPanel> _panels;

        public List<IPanel> PanelList
        {
            get { return _panels; }
        }

        public UiManager()
        {
            _panels = new List<IPanel>();
            _panels.Add(new StatsPanel());
            _panels.Add(new CommandPanel());
        }

        public void LoadContent()
        {
            foreach (IPanel p in _panels)
                p.LoadContent();
        }

        public void UnloadContent()
        {
            foreach (IPanel p in _panels)
                p.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            foreach (IPanel p in _panels)
                p.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IPanel p in _panels)
                p.Draw(spriteBatch);
        }
    }
}
