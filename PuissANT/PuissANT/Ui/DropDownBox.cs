using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PuissANT.Pheromones;

namespace PuissANT.Ui
{
    public class DropDownBox
    {
        public Button Top;
        public List<Button> Children;
        public Vector2 Position, Dimensions;
        public Rectangle Hitbox;
        public string Text;
        public bool Expanded;

        public DropDownBox()
        {
            Position = Dimensions = Vector2.Zero;
            Top = new Button();
            Children = new List<Button>();
            Text = String.Empty;
            Hitbox = new Rectangle();
            Expanded = false;
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(150, 54);
            Position = new Vector2(1000, 50);
            foreach (TileInfo t in TileInfoSets.PheromoneTypes)
            {
                string neutralPath = "ui/" + t.ToString().Trim().ToLower() + "NeutralButton";
                string overPath = "ui/" + t.ToString().Trim().ToLower() + "OverButton";
                string pressedPath = "ui/" + t.ToString().Trim().ToLower() + "PressedButton";

                Button b = new Button();
                b.Location = new Rectangle((int)Position.X, (int)Position.Y + Children.Count * (int)Dimensions.Y,
                    (int)Dimensions.X, (int)Dimensions.Y);
                // Create the text
                b.Font = new Image().Font;
                b.ButtonText = String.Empty;
                // Set the image textures...this is retarded, fix later
                b.SetTexture(Game1.Instance.Content.Load<Texture2D>(neutralPath), Button.ButtonState.Neutral);
                b.SetTexture(Game1.Instance.Content.Load<Texture2D>(overPath), Button.ButtonState.Over);
                b.SetTexture(Game1.Instance.Content.Load<Texture2D>(pressedPath), Button.ButtonState.Pressed);
                b.ButtonClicked = HandleButtonClick;
                b.Value = t;
                Children.Add(b);
            }

            Top = Children[0];
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            Top.Update(gameTime);
            foreach (Button b in Children)
                b.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Top.Draw(spriteBatch);
            if(Expanded)
                foreach (Button b in Children)
                    b.Draw(spriteBatch);
        }

        public void HandleButtonClick(object sender)
        {
            PheromoneManger.Instance.HandlePheremoneButtonClick((TileInfo)sender);
        }
    }
}
