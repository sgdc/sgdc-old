using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SGDE.Input
{
    /// <summary>
    /// Mouse buttons
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// Left mouse button.
        /// </summary>
        LeftButton,
        /// <summary>
        /// Middle mouse button.
        /// </summary>
        MiddleButton,
        /// <summary>
        /// Right mouse button.
        /// </summary>
        RightButton,
        /// <summary>
        /// XBUTTON1 mouse button.
        /// </summary>
        XButton1,
        /// <summary>
        /// XBUTTON2 mouse button.
        /// </summary>
        XButton2
    }

    /// <summary>
    /// A mouse input device.
    /// </summary>
    public sealed class Mouse : InputComponent
    {
        private InputManager manager;

        internal Mouse(InputManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Always returns Mouse.
        /// </summary>
        public InputType Type
        {
            get
            {
                return InputType.Mouse;
            }
        }

        /// <summary>
        /// Get the current Mouse input state.
        /// </summary>
        /// <returns>The current, native, input state of the Mouse.</returns>
        public MouseState GetCurrentState()
        {
            return this.manager.c_mouse_state;
        }

        /// <summary>
        /// Get the past Mouse input state.
        /// </summary>
        /// <returns>The past, native, input state of the Mouse.</returns>
        public MouseState GetPastState()
        {
            return this.manager.o_mouse_state;
        }

        /// <summary>
        /// Get or set the mouse position. Positions are relative to the upper-left hand corner of the window.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return GetPosition(true);
            }
            set
            {
                SetPosition(value);
            }
        }

        /// <summary>
        /// Get the past mouse position. Positions are relative to the upper-left hand corner of the window.
        /// </summary>
        public Vector2 PastPosition
        {
            get
            {
                return GetPosition(false);
            }
        }

        /// <summary>
        /// Get the mouse position difference between the current and past positions.
        /// </summary>
        public Vector2 PositionDiff
        {
            get
            {
                return GetPosition(true) - GetPosition(false);
            }
        }

        private Vector2 GetPosition(bool cur)
        {
            MouseState state = cur ? this.manager.c_mouse_state : this.manager.o_mouse_state;
            return new Vector2(state.X, state.Y);
        }

        private void SetPosition(Vector2 value)
        {
            if (!((float.IsInfinity(value.X) || float.IsNaN(value.X)) || (float.IsInfinity(value.Y) || float.IsNaN(value.Y))))
            {
                Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)Math.Floor(value.X), (int)Math.Floor(value.Y));
            }
        }

        /// <summary>
        /// Get the mouse scroll wheel position.
        /// </summary>
        public int Scoll
        {
            get
            {
                return GetScroll(true);
            }
        }

        /// <summary>
        /// Get the past mouse scroll wheel position.
        /// </summary>
        public int PastScoll
        {
            get
            {
                return GetScroll(false);
            }
        }

        /// <summary>
        /// Get the mouse scroll wheel position difference between current and past scroll positions.
        /// </summary>
        public int ScollDiff
        {
            get
            {
                return GetScroll(true) - GetScroll(false);
            }
        }

        private int GetScroll(bool cur)
        {
            MouseState state = cur ? this.manager.c_mouse_state : this.manager.o_mouse_state;
            return state.ScrollWheelValue;
        }

        /// <summary>
        /// Determine if the specified button is pressed.
        /// </summary>
        /// <param name="but">The button to check if pressed.</param>
        /// <returns><code>true</code> if the button was pressed, <code>false</code> if otherwise.</returns>
        public bool IsButtonPressed(MouseButton but)
        {
            return IsDown(this.manager.c_mouse_state, but);
        }

        /// <summary>
        /// Determine if the specified button is clicked. This means that the button is down right now but wasn't always pressed down.
        /// </summary>
        /// <param name="but">The button to check if pressed.</param>
        /// <returns><code>true</code> if the button was pressed, <code>false</code> if otherwise.</returns>
        public bool IsButtonClicked(MouseButton but)
        {
            return IsDown(this.manager.c_mouse_state, but) && !IsDown(this.manager.o_mouse_state, but);
        }

        private bool IsDown(MouseState state, MouseButton but)
        {
            ButtonState bstate = ButtonState.Released;
            switch (but)
            {
                case MouseButton.LeftButton:
                    bstate = state.LeftButton;
                    break;
                case MouseButton.MiddleButton:
                    bstate = state.MiddleButton;
                    break;
                case MouseButton.RightButton:
                    bstate = state.RightButton;
                    break;
                case MouseButton.XButton1:
                    bstate = state.XButton1;
                    break;
                case MouseButton.XButton2:
                    bstate = state.XButton2;
                    break;
            }
            return bstate == ButtonState.Pressed;
        }
    }
}
