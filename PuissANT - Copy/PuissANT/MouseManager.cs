using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PuissANT
{
    /// <summary>
    ///  Manager for the mouse interface. This class provides information about
    ///  the state of the mouse.
    class MouseManager
    {
        /// <info>
        /// 
        ///  
        ///  There are 2 ways to access the click information. The first is by simply
        ///  querying the class about the state of a keyboard key or GameKey.
        ///  
        ///    MouseManager.Instance.WasJustPressed
        ///    
        ///    ...
        ///    
        ///    MouseManager.Instance.WasJustReleased
        ///    
        ///    ...
        ///    
        ///    
        ///  The second way is to subscribe to an event that gets fired when the given
        ///  mouse is pressed or released.
        ///  
        ///       ...   
        ///       
        ///         MouseManager.Instance.MouseClicked += mouse_Pressed;
        ///       
        ///       ...
        ///       
        ///    }
        ///    
        ///    ...
        ///    
        ///    public void mouse_Pressed()
        ///    {
        ///        //do action here
        ///    }
        ///  
        /// </info>

        /// <summary>
        /// Singleton class holder. I have a thing for
        ///  singletons, unfortunately.
        /// </summary>
        private static MouseManager _instance = null;

        /// <summary>
        /// A Mouse event. When the mouse is pressed or released,
        ///  it calls this event and passes the key that was pressed.
        /// </summary>
        public delegate void MouseEvent();

        public MouseEvent MouseClicked;

        public MouseEvent MouseReleased;

        /// <summary>
        /// Holds the previous state of the mouse 
        /// </summary>
        private MouseState _lastMouseState;

        private bool _clickStateChanged = false;

        /// <summary>
        /// Singleton access to the MouseManager class. Allows any
        ///  object to acces the same MouseManager through MouseManager.Instance
        /// </summary>
        public static MouseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MouseManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Constructor for the Mouse Manager. Initializes
        ///  the events and sets the current key mappings to the defaults.
        /// </summary>
        public MouseManager()
        {
            MouseClicked = delegate { };
            MouseReleased  = delegate { };

            _lastMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Used to update the Manager and see the latest changes
        /// in the user's input.
        /// </summary>
        /// <param name="gameTime">The amount of time elapsed since the last call</param>
        public void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            bool isClicked = (newMouseState.LeftButton == ButtonState.Pressed);
            bool wasClicked = (_lastMouseState.LeftButton == ButtonState.Pressed);

            if (isClicked != wasClicked)
            {
                _clickStateChanged = true;

                if (isClicked)
                    MouseClicked();
                else
                    MouseReleased();
            }
            else
                _clickStateChanged = false;

            _lastMouseState = newMouseState;
        }

        public bool IsClicked()
        {
            return _lastMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsReleased()
        {
            return _lastMouseState.LeftButton == ButtonState.Released;
        }

        public Point MousePosition
        {
            get
            {
                return _lastMouseState.Position;
            }
        }

        public int MouseX
        {
            get
            {
                return _lastMouseState.X;
            }
        }

        public int MouseY
        {
            get
            {
                return _lastMouseState.Y;
            }
        }

        public bool WasJustClicked
        {
            get
            {
                return _clickStateChanged && _lastMouseState.LeftButton == ButtonState.Pressed;
            }
        }

        public bool WasJustReleased
        {
            get
            {
                return _clickStateChanged && _lastMouseState.LeftButton == ButtonState.Released;
            }
        }

    }
}
