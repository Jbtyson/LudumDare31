using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Ui
{
    public class PhermoneCursor
    {
        private static PhermoneCursor _instance;
        private Texture2D[] cursors = new Texture2D[1];

        public Vector2 Position = Vector2.Zero;


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

        public void Update(GameTime gameTime)
        {
            Position.X = MouseManager.Instance.MouseX;
            Position.Y = MouseManager.Instance.MouseY;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(cursors[0], Position, Color.White);
        }


    }
}
