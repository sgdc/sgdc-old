using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Input
{
    class MouseComponent
    {
        private MouseState _currentState;
        public MouseState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        private MouseState _pastState;
        public MouseState PastState
        {
            get { return _pastState; }
            set { _pastState = value; }
        }

        public enum MouseButton
        {
            Left,
            Middle,
            Right
        }

        public void Update()
        {
            PastState = CurrentState;
            CurrentState = Mouse.GetState();
        }

        public bool IsButtonPressed(MouseButton mouseButton)
        {
            return IsButtonDown(CurrentState, mouseButton);
        }

        public bool IsButtonReleased(MouseButton mouseButton)
        {
            return (IsButtonDown(CurrentState, mouseButton) && !IsButtonDown(PastState, mouseButton));
        }

        private bool IsButtonDown(MouseState mouseState, MouseButton button)
        {
            ButtonState state = ButtonState.Released;
            switch (button)
            {
                case MouseButton.Left:
                    state = mouseState.LeftButton;
                    break;
                case MouseButton.Middle:
                    state = mouseState.MiddleButton;
                    break;
                case MouseButton.Right:
                    state = mouseState.RightButton;
                    break;
            }
            return state == ButtonState.Pressed;
        }

        public Vector2 GetCurrentMousePosition()
        {
            return new Vector2(CurrentState.X, CurrentState.Y);
        }

        public Vector2 GetPastMousePosition()
        {
            return new Vector2(PastState.X, PastState.Y);
        }
    }
}
