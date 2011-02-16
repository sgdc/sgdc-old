using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SGDE.Input
{
    /// <summary>
    /// A keyboard input device.
    /// </summary>
    public sealed class Keyboard : InputComponent
    {
        private InputManager manager;

        internal Keyboard(InputManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Always returns Keybaord.
        /// </summary>
        public InputType Type
        {
            get
            {
                return InputType.Keyboard;
            }
        }

        /// <summary>
        /// Get the current keyboard input state.
        /// </summary>
        /// <returns>The current, native, input state of the Keyboard.</returns>
        public KeyboardState GetCurrentState()
        {
            return this.manager.c_key_state;
        }

        /// <summary>
        /// Get the past keyboard input state.
        /// </summary>
        /// <returns>The past, native, input state of the Keyboard.</returns>
        public KeyboardState GetPastState()
        {
            return this.manager.o_key_state;
        }

        /// <summary>
        /// Get if the specified key is clicked. This means that the key is down right now but wasn't always pressed down.
        /// </summary>
        /// <param name="key">The key to check if clicked.</param>
        /// <returns><code>true</code> if the key is clicked, <code>false</code> if otherwise.</returns>
        public bool IsKeyClicked(Keys key)
        {
            return this.manager.c_key_state.IsKeyDown(key) && this.manager.o_key_state.IsKeyUp(key);
        }

        /// <summary>
        /// Get if the specified key is pressed. This means that the key is down right now.
        /// </summary>
        /// <param name="key">The key to check if pressed.</param>
        /// <returns><code>true</code> if the key is pressed, <code>false</code> if otherwise.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return this.manager.c_key_state.IsKeyDown(key);
        }

        /// <summary>
        /// Get the currently pressed keys.
        /// </summary>
        /// <returns>A list of the pressed keys.</returns>
        public Keys[] GetPressedKeys()
        {
            return this.manager.c_key_state.GetPressedKeys();
        }

        /// <summary>
        /// Get the currently clicked keys.
        /// </summary>
        /// <returns>A list of the clicked keys.</returns>
        public Keys[] GetClickedKeys()
        {
            List<Keys> pressedKeys = new List<Keys>(this.manager.c_key_state.GetPressedKeys());
            for (int i = 0; i < pressedKeys.Count; i++)
            {
                if (!this.manager.o_key_state.IsKeyUp(pressedKeys[i]))
                {
                    pressedKeys.RemoveAt(i);
                    i--;
                }
            }
            return pressedKeys.ToArray();
        }
    }
}
