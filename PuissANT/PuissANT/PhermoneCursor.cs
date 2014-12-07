using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.ui
{
    public class PhermoneCursor
    {
        private static PhermoneCursor _instance;

        public Vector2 Position;
        private Vector2 Center = new Vector2(12, 12);

        private Texture2D[] cursors = new Texture2D[1];

        public static PhermoneCursor Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PhermoneCursor();
                return _instance;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            // Load Cursor Icons
            cursors[0] = Content.Load<Texture2D>("phermones/SoldierPhermone");
        }

        public void Update(TimeSpan gameTime)
        {
            Position.X = MouseManager.Instance.MouseX+500;
            Position.Y = MouseManager.Instance.MouseY;
        }

        public void Render(TimeSpan gameTime, SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(cursors[0], Position - Center, Color.White);
        }


    }
}
