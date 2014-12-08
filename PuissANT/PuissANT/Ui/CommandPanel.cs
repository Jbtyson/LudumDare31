﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PuissANT.Pheromones;

namespace PuissANT.Ui
{
    public class CommandPanel : IPanel
    {
        public Vector2 Dimensions, ButtonDimensions, PheremoneStartPosition;
        public Image Image;
        public List<Button> Buttons;

        private string _imagePath, _text;
        private int _buttonOffset = 20;


        public CommandPanel()
        {
            Dimensions = Vector2.Zero;
            ButtonDimensions = Vector2.Zero;
            PheremoneStartPosition = Vector2.Zero;
            Image = new Image();
            Buttons = new List<Button>();

            _imagePath = "ui/commandBar";
            _text = "this is the command bar";
        }

        public void LoadContent()
        {
            Dimensions = new Vector2(200, 720);
            ButtonDimensions = new Vector2(150, 54);
            Image.Position = new Vector2(ScreenManager.Instance.ScreenSize.X - Dimensions.X, ScreenManager.Instance.ScreenSize.Y - Dimensions.Y);
            PheremoneStartPosition = new Vector2(Image.Position.X + (Dimensions.X / 2) - (ButtonDimensions.X / 2), Image.Position.Y + 100);
            Image.LoadContent(_imagePath, _text);

            // Create Pheremone buttons
            TileInfo[] array = TileInfoSets.PheromoneTypes;
            foreach (var v in array)
            {
                Buttons.Add(CreateButton(v.ToString()));
            }
        }


        public Button CreateButton(string text)
        {
            Button b = new Button();
            b.Location = new Rectangle((int)PheremoneStartPosition.X, (int)PheremoneStartPosition.Y + Buttons.Count * (_buttonOffset + (int)ButtonDimensions.Y),
                (int)ButtonDimensions.X, (int)ButtonDimensions.Y);
            // Create the text
            b.Font = Image.Font;
            b.ButtonText = String.Empty;
            // Set the image textures...this is retarded, fix later
            Image temp = new Image();
            temp.LoadContent("ui/" + text + "PherActive", String.Empty);
            b.SetTexture(temp.Texture, Button.ButtonState.Neutral);
            temp = new Image();
            temp.LoadContent("ui/" + text + "PherPressed", String.Empty);
            b.SetTexture(temp.Texture, Button.ButtonState.Over);
            temp = new Image();
            temp.LoadContent("ui/" + text + "PherInactive", String.Empty);
            b.SetTexture(temp.Texture, Button.ButtonState.Pressed);

            return b;
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
            foreach (Button b in Buttons)
                b.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
            foreach (Button b in Buttons)
                b.Draw(spriteBatch);
        }

        Vector2 IPanel.Dimensions
        {
            get { return Dimensions; }
        }
    }
}