using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuissANT.Ui
{
    public class Button
    {
        //Used if you want the button to display different pictures for the 
        public enum ButtonState
        {
            Neutral,
            Over,
            Pressed
        }

        private ButtonState _buttonState;
        private bool _maintainPressedState; //Used if the mouse presses down on button but then leaves the
                                            // button, so if it re-enters it will re-press it

        private Texture2D[] _buttonImages; //Will hold one for each button state

        private Rectangle _buttonLocation; //Location/size for the button
        private Rectangle _buttonHitBox; //Area where clicking button will work

        private string _buttonText; //Text to show in the box, can be empty
        private SpriteFont _spriteFont; //Font for the sprite, will resize later to fit the box
        private Color _buttonTextColor;
        private Vector2 _buttonTextPosition;

        /// <summary>
        /// A Click event. When the button is clicked (called on release),
        ///  it calls this event.
        /// </summary>
        public delegate void ButtonEvent(object sender);

        public ButtonEvent ButtonClicked;



        public Button()
        {
            InitializeButton();
        }

        public Rectangle Location
        {
            get { return _buttonLocation; }
            set { _buttonLocation = value; _buttonHitBox = _buttonLocation; RecenterText(); }
        }

        public Rectangle HitBox
        {
            get { return _buttonHitBox; }
            set { _buttonHitBox = value; }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; RecenterText(); }
        }

        public Color ButtonTextColor
        {
            get { return _buttonTextColor; }
            set { _buttonTextColor = value; }
        }

        // James added this
        public SpriteFont Font
        {
            get { return _spriteFont; }
            set { _spriteFont = value; }
        }

        public void SetTexture(Texture2D texture, ButtonState buttonState = ButtonState.Neutral)
        {
            switch (buttonState)
            {
                case ButtonState.Neutral:
                    _buttonImages[0] = texture;
                    break;
                case ButtonState.Over:
                    _buttonImages[1] = texture;
                    break;
                case ButtonState.Pressed:
                    _buttonImages[2] = texture;
                    break;
            }
        }

        public void InitializeButton()
        {
            _buttonImages = new Texture2D[3];
            _buttonLocation = new Rectangle();
            _buttonHitBox = _buttonLocation;
            _buttonState = ButtonState.Neutral;
            _buttonText = string.Empty;
            _spriteFont = null;
            _buttonTextColor = Color.Black;
            _buttonTextPosition = new Vector2();
            _maintainPressedState = false;
        }

        public void Update(GameTime gameTime)
        {
            bool isMouseDown = MouseManager.Instance.IsClicked();
            Point location = MouseManager.Instance.MousePosition;
            bool isMouseOver = _buttonLocation.Contains(location);

            //Clear the pressed state variable if click is released outside of button
            if (!isMouseDown && _maintainPressedState)
                _maintainPressedState = false;

            switch (_buttonState)
            {
                case ButtonState.Neutral:
                    if (isMouseOver)
                    {
                        if(_maintainPressedState)
                            _buttonState = ButtonState.Pressed;
                        else
                            _buttonState = ButtonState.Over;
                    }
                    break;
                case ButtonState.Over:
                    if (!isMouseOver)
                    {
                        _buttonState = ButtonState.Neutral;
                    }
                    else
                    {
                        if (isMouseDown)
                        {
                            _maintainPressedState = true;
                            _buttonState = ButtonState.Pressed;
                        }
                    }
                    break;
                case ButtonState.Pressed:

                    if (!isMouseDown)
                    {
                        //Click
                        if(isMouseOver)
                            _buttonState = ButtonState.Over;
                        else
                            _buttonState = ButtonState.Neutral;
                        ButtonClicked(_buttonText);
                    }
                    else if (!isMouseOver)
                    {
                        _buttonState = ButtonState.Neutral;
                    }

                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D buttonTexture = GetStateTexture();
            if(buttonTexture != null)
                spriteBatch.Draw(buttonTexture, _buttonLocation, Color.White);

            if (_buttonText != string.Empty)
            {
                spriteBatch.DrawString(_spriteFont, _buttonText, _buttonTextPosition, _buttonTextColor);
            }
        }

        private void RecenterText()
        {
            //Need to get the position for the text & the size for the spritefont/text
            if (_buttonText == string.Empty)
                return;

            Vector2 textSize = _spriteFont.MeasureString(_buttonText);
            _buttonTextPosition = new Vector2(_buttonLocation.X + ((float)_buttonLocation.Width) / 2 -
                textSize.X / 2, _buttonLocation.Y + ((float)_buttonLocation.Height) / 2 -
                textSize.Y / 2);
        }

        //Can return null
        private Texture2D GetStateTexture()
        {
            Texture2D texture = null;
            switch (_buttonState)
            {
                case ButtonState.Neutral:
                    texture = _buttonImages[0];
                    break;
                case ButtonState.Over:
                    texture = _buttonImages[1];
                    break;
                case ButtonState.Pressed:
                    texture = _buttonImages[2];
                    break;
            }
            if (texture == null && _buttonImages[0] != null)
                texture = _buttonImages[0];

            return texture;
        }
    }
}
