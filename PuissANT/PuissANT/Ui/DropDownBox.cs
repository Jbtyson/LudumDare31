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
        public Button Top, Dropdown;
        public List<Button> Children;
        public Vector2 Position, PherDimensions, DropdownDimensions;
        public Rectangle Hitbox, ListHitbox;
        public string Text;
        public bool Expanded, ButtonClickedLast, ButtonClickedCurrent;

        public DropDownBox()
        {
            Position = PherDimensions = DropdownDimensions = Vector2.Zero;
            Top = Dropdown = new Button();
            Children = new List<Button>();
            Text = String.Empty;
            Hitbox = new Rectangle();
            Expanded = ButtonClickedCurrent = ButtonClickedLast = false;
        }

        public void LoadContent()
        {
            PherDimensions = new Vector2(128, 32);
            DropdownDimensions = new Vector2(32, 32);
            Position = new Vector2(1000, 14);
            foreach (TileInfo t in TileInfoSets.PheromoneTypes)
            //foreach (TileInfo t in pt)
            {
                string neutralPath = "ui/" + t.ToString().Trim().ToLower() + "NeutralButton";
                string overPath = "ui/" + t.ToString().Trim().ToLower() + "OverButton";
                string pressedPath = "ui/" + t.ToString().Trim().ToLower() + "PressedButton";

                Button b = new Button();
                b.Location = new Rectangle((int)Position.X, (int)Position.Y + (Children.Count + 1) * (int)PherDimensions.Y,
                    (int)PherDimensions.X, (int)PherDimensions.Y);
                // Create the text
                b.Font = new Image().Font;
                b.ButtonText = String.Empty;
                // Set the image textures...this is retarded, fix later
                b.SetTexture(Game1.Instance.Content.Load<Texture2D>(neutralPath), Button.ButtonState.Neutral);
                b.SetTexture(Game1.Instance.Content.Load<Texture2D>(overPath), Button.ButtonState.Over);
                b.SetTexture(Game1.Instance.Content.Load<Texture2D>(pressedPath), Button.ButtonState.Pressed);
                b.ButtonClicked = HandlePherButtonClick;
                b.Value = t;
                Children.Add(b);
            }

            // Add the top button image
            Top = Children[0].Clone();
            Top.Location = new Rectangle((int)Position.X, (int)Position.Y,
                    (int)PherDimensions.X, (int)PherDimensions.Y);
            //Add the dropdown button
            Dropdown.Location = new Rectangle((int)Position.X + (int)PherDimensions.X, (int)Position.Y,
                    (int)DropdownDimensions.X, (int)DropdownDimensions.Y);
            // Create the text
            Dropdown.Font = new Image().Font;
            Dropdown.ButtonText = String.Empty;
            // Set the image textures...this is retarded, fix later
            Dropdown.SetTexture(Game1.Instance.Content.Load<Texture2D>("ui/dropdownDOWN"), Button.ButtonState.Neutral);
            Dropdown.SetTexture(Game1.Instance.Content.Load<Texture2D>("ui/dropdownDOWN"), Button.ButtonState.Over);
            Dropdown.SetTexture(Game1.Instance.Content.Load<Texture2D>("ui/dropdownDOWN"), Button.ButtonState.Pressed);
            Dropdown.ButtonClicked = HandleDropDownButtonClick;

            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)(PherDimensions.X + DropdownDimensions.X), (int)(PherDimensions.Y));
            ListHitbox = new Rectangle((int)Position.X, (int)(Position.Y + PherDimensions.Y), (int)PherDimensions.X, (int)(PherDimensions.Y) * Children.Count);
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            Top.Update(gameTime);
            Dropdown.Update(gameTime);
            foreach (Button b in Children)
                b.Update(gameTime);

            // Handle button clicks
            if (ButtonClickedLast)
                ButtonClickedCurrent = false;
            else
            {
                ButtonClickedLast = ButtonClickedCurrent;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Dropdown.Draw(spriteBatch);
            Top.Draw(spriteBatch);
            if(Expanded)
                foreach (Button b in Children)
                    b.Draw(spriteBatch);
        }

        public void HandlePherButtonClick(Button sender)
        {
            if (!Expanded)
                PheromoneManger.Instance.HandlePheremoneButtonClick((TileInfo)sender.Value);
            else
            {
                Top = (Button)sender.Clone();
                Top.Location = new Rectangle((int)Position.X, (int)Position.Y,
                    (int)PherDimensions.X, (int)PherDimensions.Y);
                Expanded = false;
                PheromoneManger.Instance.HandlePheremoneButtonClick((TileInfo)sender.Value);
            }
            ButtonClickedCurrent = true;
        }

        public void HandleDropDownButtonClick(Button sender)
        {
            Expanded = !Expanded;
            ButtonClickedCurrent = true;
        }
    }
}
