using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PuissANT.ui
{
    public class NumberRenderer
    {
        private static NumberRenderer _instance;
        private Dictionary<int, Rectangle> _characterMap;
        private Texture2D _texture;

        public static NumberRenderer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NumberRenderer();
                return _instance;
            }
        }
        
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("ui/numbers");
            _characterMap = new Dictionary<int, Rectangle>(10);
            _characterMap['0'] = new Rectangle(1, 1, 12, 18);
            _characterMap['1'] = new Rectangle(16, 1, 8, 18);
            _characterMap['2'] = new Rectangle(24, 1, 9, 16);
            _characterMap['3'] = new Rectangle(29, 1, 9, 16);
            _characterMap['4'] = new Rectangle(42, 1, 9, 16);
            _characterMap['5'] = new Rectangle(55, 1, 6, 16);
            _characterMap['6'] = new Rectangle(65, 1, 9, 16);
            _characterMap['7'] = new Rectangle(76, 1, 10, 16);
            _characterMap['8'] = new Rectangle(87, 1, 10, 16);
            _characterMap['9'] = new Rectangle(97, 1, 10, 16);
        }

        
        public void DrawText(SpriteBatch spriteBatch, Vector2 position, string number)
        {
            
            foreach (char c in number)
            {
                Rectangle r;
                if (_characterMap.TryGetValue(c, out r))
                {
                    spriteBatch.Draw(_texture, position, r, Color.White);
                    position.X += r.Width;
                }
            }
        }
    }
}