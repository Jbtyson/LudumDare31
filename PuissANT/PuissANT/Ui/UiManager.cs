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
        public List<Rectangle> HitBoxes;
        public StatsPanel Stats;
        public DropDownBox _box;

        public List<IPanel> PanelList
        {
            get { return _panels; }
        }

        public UiManager()
        {
            _panels = new List<IPanel>();
            Stats = new StatsPanel();
            _panels.Add(Stats);
            _box = new DropDownBox();
            HitBoxes = new List<Rectangle>();
        }

        public void LoadContent()
        {
            foreach (IPanel p in _panels)
                p.LoadContent();
            _box.LoadContent();

            // Add hitboxes of all buttons and displays
            foreach (ResourceDisplay r in Stats.Resources)
                HitBoxes.Add(r.Hitbox);
            HitBoxes.Add(_box.Hitbox);

        }

        public void UnloadContent()
        {
            foreach (IPanel p in _panels)
                p.UnloadContent();
            _box.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            foreach (IPanel p in _panels)
                p.Update(gameTime);
            _box.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IPanel p in _panels)
                p.Draw(spriteBatch);
            _box.Draw(spriteBatch);
        }

        public bool IsMouseOnUi()
        {
            Point m = MouseManager.Instance.MousePosition;
            foreach (Rectangle r in HitBoxes)
            {
                if (r.Contains(m))
                    return true;
            }
            if (_box.Expanded)
                if (_box.ListHitbox.Contains(m))
                    return true;
            return false;
        }
    }
}
